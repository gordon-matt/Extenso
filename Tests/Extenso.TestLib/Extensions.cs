using Extenso.TestLib.Data.Entities;
using Extenso.TestLib.ViewModels;

namespace Extenso.TestLib;

public static class Extensions
{
    public static Product ToEntity(this ProductViewModel model) => new()
    {
        ProductId = model.ProductId,
        Name = model.Name,
        ProductNumber = model.ProductNumber,
        MakeFlag = model.MakeFlag,
        FinishedGoodsFlag = model.FinishedGoodsFlag,
        Color = model.Color,
        SafetyStockLevel = model.SafetyStockLevel,
        ReorderPoint = model.ReorderPoint,
        StandardCost = model.StandardCost,
        ListPrice = model.ListPrice,
        Size = model.Size,
        SizeUnitMeasureCode = model.SizeUnitMeasureCode,
        WeightUnitMeasureCode = model.WeightUnitMeasureCode,
        Weight = model.Weight,
        DaysToManufacture = model.DaysToManufacture,
        ProductLine = model.ProductLine,
        Class = model.Class,
        Style = model.Style,
        ProductSubcategoryId = model.ProductSubcategoryId,
        ProductModelId = model.ProductModelId,
        SellStartDate = model.SellStartDate,
        SellEndDate = model.SellEndDate,
        DiscontinuedDate = model.DiscontinuedDate,
        Rowguid = model.Rowguid,
        ModifiedDate = model.ModifiedDate
    };

    public static ProductViewModel ToModel(this Product entity) => new()
    {
        ProductId = entity.ProductId,
        Name = entity.Name,
        ProductNumber = entity.ProductNumber,
        MakeFlag = entity.MakeFlag,
        FinishedGoodsFlag = entity.FinishedGoodsFlag,
        Color = entity.Color,
        SafetyStockLevel = entity.SafetyStockLevel,
        ReorderPoint = entity.ReorderPoint,
        StandardCost = entity.StandardCost,
        ListPrice = entity.ListPrice,
        Size = entity.Size,
        SizeUnitMeasureCode = entity.SizeUnitMeasureCode,
        WeightUnitMeasureCode = entity.WeightUnitMeasureCode,
        Weight = entity.Weight,
        DaysToManufacture = entity.DaysToManufacture,
        ProductLine = entity.ProductLine,
        Class = entity.Class,
        Style = entity.Style,
        ProductSubcategoryId = entity.ProductSubcategoryId,
        ProductModelId = entity.ProductModelId,
        SellStartDate = entity.SellStartDate,
        SellEndDate = entity.SellEndDate,
        DiscontinuedDate = entity.DiscontinuedDate,
        Rowguid = entity.Rowguid,
        ModifiedDate = entity.ModifiedDate
    };

    public static ProductModel ToEntity(this ProductModelViewModel model) => new()
    {
        ProductModelId = model.ProductModelId,
        Name = model.Name,
        CatalogDescription = model.CatalogDescription,
        Instructions = model.Instructions,
        Rowguid = model.Rowguid,
        ModifiedDate = model.ModifiedDate
    };

    public static ProductModelViewModel ToModel(this ProductModel entity) => new()
    {
        ProductModelId = entity.ProductModelId,
        Name = entity.Name,
        CatalogDescription = entity.CatalogDescription,
        Instructions = entity.Instructions,
        Rowguid = entity.Rowguid,
        ModifiedDate = entity.ModifiedDate,
        Products = entity.Products.Select(p => p.ToModel()).ToList()
    };
}