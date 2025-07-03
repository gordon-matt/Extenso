using System.Linq.Expressions;
using Extenso.TestLib.Data.Entities;
using LinqKit;
using Microsoft.Extensions.Logging;

namespace Extenso.Data.Entity.Tests;

public class ProductModelRepository : EntityFrameworkRepository<ProductModel>
{
    public ProductModelRepository(IDbContextFactory contextFactory, ILoggerFactory loggerFactory)
        : base(contextFactory, loggerFactory)
    {
    }

    // This is an example of how to apply mandatory filters to the queries.
    // In this case, we require a "Category" filter to be applied.
    // A more realistic scenario would be something like a Tenant ID or User ID
    protected override Expression<Func<ProductModel, bool>> ApplyMandatoryFilters(
        Expression<Func<ProductModel, bool>> predicate,
        IDictionary<string, object> filters)
    {
        predicate = filters.TryGetValue("Category", out var category) && category is string cat
            ? predicate.And(p => p.Products.Any(p => p.ProductSubcategory.ProductCategory.Name == cat))
            : throw new ArgumentException("Category filter is required.", nameof(filters));

        return predicate;
    }
}