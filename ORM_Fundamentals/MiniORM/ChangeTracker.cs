using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MiniORM
{
	internal class ChangeTracker<T>
		where T : class, new()
	{
		private readonly List<T> allEntities;
		private readonly List<T> added;
		private readonly List<T> removed;

		public ChangeTracker(IEnumerable<T> entities)
		{
			this.added = new List<T>();
			this.removed = new List<T>();
			this.allEntities = CloneEntities(entities);
		}
		public IReadOnlyCollection<T> Added => this.added.AsReadOnly();
		public IReadOnlyCollection<T> Removed => this.removed.AsReadOnly();
		public void Add(T item) => this.added.Add(item);
		public void Remove(T item) => this.removed.Add(item);

		public IEnumerable<T> GetModifiedEntities(DbSet<T> dbSet)
		{
			var modifiedEntities = new List<T>();

			var primaryKeys = typeof(T)
				.GetProperties()
				.Where(pi => pi.HasAttribute<KeyAttribute>())
				.ToArray();

			foreach (var proxyEntity in this.allEntities)
			{
				var primaryKeyValues = GetPrimaryKeyValues(primaryKeys, proxyEntity).ToArray();
					

				var entity = dbSet.Entities
					.Single(e => GetPrimaryKeyValues(primaryKeys, e)
					.SequenceEqual(primaryKeyValues));
				var isModifies = IsModified(proxyEntity, entity);

			}
			return modifiedEntities;
		}

		private static List<T> CloneEntities(IEnumerable<T> entities)
		{
			var cloneEntitites = new List<T>();

			PropertyInfo[] propertiesToClone = typeof(T).GetProperties()
				.Where(pi => DbContext.AllowedSqlTypes.Contains(pi.PropertyType))
				.ToArray();

			foreach (var entity in entities)
			{
				var clonedEntity = Activator.CreateInstance<T>();

				foreach (var property in propertiesToClone)
				{
					var value = property.GetValue(entity);
					property.SetValue(clonedEntity, value);
				}
				cloneEntitites.Add(clonedEntity);
			}
			return cloneEntitites;
		}
		private bool IsModified(T proxyEntity, object entity)
		{
			var mmonitoredProperties = typeof(T).GetProperties()
				.Where(pi => DbContext.AllowedSqlTypes.Contains(pi.PropertyType));

			var modifiedProperties = mmonitoredProperties
				.Where(pi => !Equals(pi.GetValue(entity), pi.GetValue(proxyEntity)))
				.ToArray();

			var isModified = modifiedProperties.Any();
			return isModified;
		}

		private static IEnumerable<object> GetPrimaryKeyValues(IEnumerable<PropertyInfo> primaryKey, T entity)
		{
			return primaryKey.Select(pk => pk.GetValue(entity));
		}
	}
}