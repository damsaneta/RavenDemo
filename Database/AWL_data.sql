USE AWL
GO

DELETE FROM [dbo].[ProductInventory]
DELETE FROM [dbo].Product 
DELETE FROM [dbo].ProductSubcategory
DELETE FROM [dbo].[ProductCategory]
DELETE FROM [dbo].[UnitMeasure]
DELETE FROM [dbo].Location
GO
--------------------------------------------------
SET IDENTITY_INSERT [dbo].[ProductCategory] ON
GO
INSERT INTO [dbo].[ProductCategory] (ID, [Name], rowguid, ModifiedDate)
SELECT [ProductCategoryID],[Name], rowguid, ModifiedDate
FROM [AdventureWorks2012].[Production].[ProductCategory]
GO
SET IDENTITY_INSERT [dbo].[ProductCategory] OFF
GO
--------------------------------------------------
SET IDENTITY_INSERT [dbo].ProductSubcategory ON
GO
INSERT INTO [dbo].ProductSubcategory (ID, ProductCategoryID, [Name], rowguid, ModifiedDate)
SELECT [ProductSubcategoryID]
      ,[ProductCategoryID]
      ,[Name]
	  ,rowguid
	  ,ModifiedDate
  FROM [AdventureWorks2012].[Production].[ProductSubcategory]
GO
SET IDENTITY_INSERT [dbo].ProductSubcategory OFF
GO
--------------------------------------------------
INSERT INTO [dbo].[UnitMeasure]
           ([UnitMeasureCode]
           ,[Name]
		   ,ModifiedDate)
SELECT [UnitMeasureCode]
      ,[Name]
	  ,ModifiedDate
  FROM [AdventureWorks2012].[Production].[UnitMeasure]
  GO
--------------------------------------------------
SET IDENTITY_INSERT [dbo].Product ON
GO
INSERT INTO [dbo].[Product]
           (ID
		   ,[Name]
           ,[ProductNumber]
           ,[Color]
           ,[SafetyStockLevel]
           ,[ReorderPoint]
           ,[StandardCost]
           ,[ListPrice]
           ,[Size]
           ,[SizeUnitMeasureCode]
           ,[WeightUnitMeasureCode]
           ,[Weight]
           ,[DaysToManufacture]
           ,[ProductLine]
           ,[Class]
           ,[Style]
           ,[ProductSubcategoryID]
           ,[SellStartDate]
           ,[SellEndDate]
		   ,rowguid
		   ,ModifiedDate)
SELECT [ProductID]
      ,[Name]
      ,[ProductNumber]
      ,[Color]
      ,[SafetyStockLevel]
      ,[ReorderPoint]
      ,[StandardCost]
      ,[ListPrice]
      ,[Size]
      ,[SizeUnitMeasureCode]
      ,[WeightUnitMeasureCode]
      ,[Weight]
      ,[DaysToManufacture]
      ,[ProductLine]
      ,[Class]
      ,[Style]
      ,[ProductSubcategoryID]
      ,[SellStartDate]
      ,[SellEndDate]
	  ,rowguid
	  ,ModifiedDate
  FROM [AdventureWorks2012].[Production].[Product]
GO
SET IDENTITY_INSERT [dbo].Product OFF
GO
--------------------------------------------------
SET IDENTITY_INSERT [dbo].[Location] ON
GO
INSERT INTO [dbo].[Location]
           (ID, [Name], ModifiedDate)
SELECT [LocationID]
      ,[Name]
	  ,ModifiedDate
  FROM [AdventureWorks2012].[Production].[Location]
GO
SET IDENTITY_INSERT [dbo].Location OFF
GO
--------------------------------------------------
INSERT INTO [dbo].[ProductInventory]
           ([ProductID]
           ,[LocationID]
           ,[Shelf]
           ,[Bin]
           ,[Quantity]
		   ,rowguid
		   ,ModifiedDate)
SELECT [ProductID]
      ,[LocationID]
      ,[Shelf]
      ,[Bin]
      ,[Quantity]
	  ,rowguid
	  ,ModifiedDate
  FROM [AdventureWorks2012].[Production].[ProductInventory]
GO


