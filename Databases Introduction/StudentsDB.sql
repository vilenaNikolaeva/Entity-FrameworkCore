/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [Id]
      ,[AccountId]
      ,[OldBalance]
      ,[NewBalance]
      ,[Amount]
      ,[DateTime]
  FROM [Bank].[dbo].[Transactions]