﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace MiniORM
{
	public abstract class DbContext
	{
		private readonly DatabaseConnection connection;
		private readonly Dictionary<Type, PropertyInfo> dbSetPropeties;

		internal static readonly Type[] AllowedSqlTypes=
		{
			typeof(string),
			typeof(int),
			typeof(uint),
			typeof(long),
			typeof(ulong),
			typeof(decimal),
			typeof(bool),
			typeof(DateTime)
		};

		protected DbContext(string connectionString)
		{
			this.connection = new DatabaseConnection(connectionString);
			this.dbSetPropeties = this.DiscoverDbSets();

			using (new ConnectionManager(connection))
			{
				this.InitializaDbSet();
			}
			this.MapAllRelations();
		}
		public void SaveChanges()
		{
			var dbSets =this.dbSetPropeties
				.Select(pi => pi.Value.GetValue(this))
				.ToArray();

			foreach (IEnumerable<object>  dbSet in dbSets)
			{
				var invalidEntities = dbSet
					.Where(en => !IsObjectValid(en))
					.ToArray();

				if (invalidEntities.Any())
				{
					throw new InvalidOperationException($"{invalidEntities.Length} Invalid Enitites found in {dbSet.GetType().Name}!");
				}
			}
			using (new ConnectionManager(connection))
			{
				using (var transaction=this.connection.StartTransaction())
				{
					foreach (IEnumerable dbSet in dbSets)
					{
						var dbSetType = dbSet.GetType().GetGenericArguments().First();
						var persistMethod=typeof(DbContext)
							.GetMethod("Persist",BindingFlags.Instance | BindingFlags.NonPublic)
							.MakeGenericMethod(dbSetType);

						try
						{
							persistMethod.Invoke(this, new object[] { dbSet, transaction });
						}
						catch (TargetInvocationException tie)
						{
							throw tie.InnerException;
						}
						catch (InvalidCastException)
						{
							transaction.Rollback();
							throw;
						}
						catch (SqlException)
						{
							transaction.Rollback();
							throw;
						}
						transaction.Commit();
					}
				}
			}
		}

		private bool IsObjectValid(object entity)
		{
			var validationContext = new ValidationContext(entity);
			var validationErrors = new List<ValidationResult>();

			var validationResult = Validator.TryValidateObject(entity, validationContext, validationErrors, validateAllProperties: true);
			return validationResult;
		}

		private void Persist<TEntity>(DbSet<TEntity> dbSet)
			where TEntity: class,new()
		{
			string tableName =this.GetTableName(typeof(TEntity));

			var columns = this.connection.FetchColumnNames(tableName).ToArray();

			if (dbSet.ChangeTracker.Added.Any())
			{
				this.connection.InsertEntities(dbSet.ChangeTracker.Added, tableName, columns);
			}
			var modifiedEntities = dbSet.ChangeTracker.GetModifiedEntities(dbSet).ToArray();
			
			if (modifiedEntities.Any())
			{
				this.connection.UpdateEntities(modifiedEntities, tableName, columns);
			}
			if (dbSet.ChangeTracker.Removed.Any())
			{
				this.connection.DeleteEntities(dbSet.ChangeTracker.Removed, tableName, columns);
			}
		}
		private void PopulateDbSet<TEntity>(PropertyInfo dbSet)
			where TEntity : class, new()
		{
			IEnumerable<TEntity> entities =this.LoadTableEntities<TEntity>();
			var dbSetInstance = new DbSet<TEntity>(entities);
			ReflectionHelper.ReplaceBackingField(this, dbSet.Name, dbSetInstance);
		}

		private IEnumerable<TEntity> LoadTableEntities<TEntity>()
			where TEntity : class
		{
			Type table = typeof(TEntity);
			var columns = GetEntityCoumnNames(table);
			var tableName = GetTableName(table);
			var fetchedRows = this.connection.FetchResultSet<TEntity>(tableName, columns).ToArray();
			return fetchedRows;
		}

		private string GetTableName(Type tableType)
		{
			var tableName = ((TableAttribute)Attribute.GetCustomAttribute(tableType,typeof(TableAttribute)))?.Name;
			if (tableName==null)
			{
				tableName = this.dbSetPropeties[tableType].Name;
			}
			return tableName;
		}

		private string[] GetEntityCoumnNames(Type table)
		{
			var tableName=this.GetTableName(table);
			var dbColumns = this.connection.FetchColumnNames(tableName);

			var columns = table.GetProperties()
				.Where(pi => dbColumns.Contains(pi.Name) && !pi.HasAttribute<NotMappedAttribute>() &&
				AllowedSqlTypes.Contains(pi.PropertyType))
				.Select(pi => pi.Name)
				.ToArray();

			return columns;
		}

		private void MapAllRelations()
		{
			foreach (var dbSetProperty in this.dbSetPropeties)
			{
				var dbSetType = dbSetProperty.Key;
				var mapRelationsGeneric = typeof(DbContext)
					.GetMethod("MapRelations", BindingFlags.Instance | BindingFlags.NonPublic)
					.MakeGenericMethod(dbSetType);
				var dbSet = dbSetProperty.Value.GetValue(this);
				mapRelationsGeneric.Invoke(this, new[] { dbSet });
			}
			
		}
		private void MapRelations<TEntity>(DbSet<TEntity> dbSet)
			where TEntity : class,new()
		{
			var entityType = typeof(TEntity);
			MapNavigationProperties(dbSet);
			var collections = entityType
				.GetProperties()
				.Where(pi => pi.PropertyType.IsGenericType &&
					   pi.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
				.ToArray();

			foreach (var collection in collections)
			{
				var collectionType = collection.PropertyType.GenericTypeArguments.First();
				var mapCollectionMethod = typeof(DbContext)
					.GetMethod("mapCollection", BindingFlags.Instance | BindingFlags.NonPublic)
					.MakeGenericMethod(entityType, collectionType);
				mapCollectionMethod.Invoke(this, new object[] { dbSet, collection });
			}
		}
		private void MapCollection<TDbSet, TCollection>(DbSet<TDbSet> dbSet, PropertyInfo collectionProperty)
			where TDbSet : class,new() where TCollection : class,new()
		{
			var entityType = typeof(TDbSet);
			var collectionType = typeof(TCollection);

			var primaryKeys = collectionType.GetProperties()
				.Where(pi => pi.HasAttribute<KeyAttribute>())
				.ToArray();
			var primaryKey = primaryKeys.First();
			var foreignKey = entityType.GetProperties()
				.First(pi => pi.HasAttribute<KeyAttribute>());

			var isManyToMany = primaryKeys.Length >= 2;
			if (isManyToMany)
			{
				primaryKey = collectionType.GetProperties()
					.First(pi => collectionType.GetProperty(pi.GetCustomAttribute<ForeignKeyAttribute>().Name)
					.PropertyType == entityType);
			}

			var navigationDbSet = (DbSet < TCollection >) this.dbSetPropeties[collectionType].GetValue(this);
			foreach (var entity in dbSet)
			{
				var primaryKeyValue = foreignKey.GetValue(entity);
				var navigationEntities = navigationDbSet
					.Where(ne => primaryKey.GetValue(ne).Equals(primaryKeyValue));

				ReflectionHelper.ReplaceBackingField(entity, collectionProperty.Name, navigationEntities);
			}

		}

		private void MapNavigationProperties<TEntity>(DbSet<TEntity> dbSet) 
			where TEntity : class, new()
		{
			var entityType = typeof(TEntity);

			var foreignKeys = entityType.GetProperties()
				.Where(pi => pi.HasAttribute<ForeignKeyAttribute>())
				.ToArray();

			foreach (var foreignKey in foreignKeys)
			{
				string navigationPropertyName = foreignKey.GetCustomAttribute<ForeignKeyAttribute>().Name;

				PropertyInfo navigationProperty = entityType.GetProperty(navigationPropertyName);

				var navigationDbSet = this.dbSetPropeties[navigationProperty.PropertyType].GetValue(this);

				var navigationPrimaryKey = navigationProperty.PropertyType.GetProperties()
					.First(pi => pi.HasAttribute<KeyAttribute>());
				foreach (var entity in dbSet)
				{
					var foreignKeyValue = foreignKey.GetValue(entity);

					var navigationPropertyValue = ((IEnumerable<object>)navigationDbSet)
						.First(np => navigationPrimaryKey.GetValue(np).Equals(foreignKeyValue));

					navigationProperty.SetValue(entity, navigationPropertyValue);
				}
			}
		}
		 
		private void InitializaDbSet()
		{
			foreach (var dbSet in this.dbSetPropeties)
			{
				var dbSetType = dbSet.Key;
				var dbSetProperty = dbSet.Value;

				var populateDbSetGeneric = typeof(DbContext)
					.GetMethod("PopulateDbSet", BindingFlags.Instance | BindingFlags.NonPublic)
					.MakeGenericMethod(dbSetType);
				populateDbSetGeneric.Invoke(this, new object[] { dbSetProperty });
			}
		}

		private Dictionary<Type, PropertyInfo> DiscoverDbSets()
		{
			var dbSets = this.GetType()
				.GetProperties()
				.Where(pi => pi.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
				.ToDictionary(pi => pi.PropertyType.GetGenericArguments().First(), pi => pi);

			return dbSets;
		}
	}
}