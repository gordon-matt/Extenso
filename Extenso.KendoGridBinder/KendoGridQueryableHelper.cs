using AutoMapper;
using AutoMapper.QueryableExtensions;

using Extenso.KendoGridBinder.AutoMapperExtensions;

namespace Extenso.KendoGridBinder;

/// <summary>
/// Represents the kendo grid queryable helper.
/// </summary>
public class KendoGridQueryableHelper
{
    private readonly AutoMapperUtils _autoMapperUtils;
    private readonly MapperConfiguration _mapperConfiguration;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the KendoGridQueryableHelper class.
    /// </summary>
    /// <param name="mapperConfiguration">The mapper configuration.</param>
    public KendoGridQueryableHelper(MapperConfiguration mapperConfiguration)
    {
        _mapperConfiguration = mapperConfiguration;
        _autoMapperUtils = new AutoMapperUtils(mapperConfiguration);
        _mapper = mapperConfiguration.CreateMapper();
    }

    /// <summary>
    /// Converts the value to the kendo grid ex.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <param name="query">The query.</param>
    /// <param name="request">The request.</param>
    /// <param name="includes">The includes.</param>
    /// <param name="mappings">The mappings.</param>
    /// <param name="conversion">The conversion.</param>
    /// <param name="canUseAutoMapperProjection">The can use auto mapper projection.</param>
    /// <returns>The kendo grid ex.</returns>
    public KendoGrid<TEntity, TViewModel> ToKendoGridEx<TEntity, TViewModel>(IQueryable<TEntity> query,
        KendoGridBaseRequest request,
        IEnumerable<string> includes = null,
        Dictionary<string, MapExpression<TEntity>> mappings = null,
        Func<IQueryable<TEntity>, IEnumerable<TViewModel>> conversion = null,
        bool canUseAutoMapperProjection = false)
    {
        var mappings2 = _autoMapperUtils.GetModelMappings<TEntity, TViewModel>(mappings);
        var conversion2 = conversion ?? GetAutoMapperConversion<TEntity, TViewModel>(canUseAutoMapperProjection);

        return new KendoGrid<TEntity, TViewModel>(request, query, mappings2, conversion2, includes);
    }

    /// <summary>
    /// Filters by.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <param name="query">The query.</param>
    /// <param name="request">The request.</param>
    /// <param name="includes">The includes.</param>
    /// <param name="mappings">The mappings.</param>
    /// <param name="conversion">The conversion.</param>
    /// <param name="canUseAutoMapperProjection">The can use auto mapper projection.</param>
    /// <returns>The ts.</returns>
    public IEnumerable<TViewModel> FilterBy<TEntity, TViewModel>(IQueryable<TEntity> query,
        KendoGridBaseRequest request,
        IEnumerable<string> includes = null,
        Dictionary<string, MapExpression<TEntity>> mappings = null,
        Func<IQueryable<TEntity>, IEnumerable<TViewModel>> conversion = null,
        bool canUseAutoMapperProjection = false)
    {
        var mappings2 = _autoMapperUtils.GetModelMappings<TEntity, TViewModel>(mappings);
        var conversion2 = conversion ?? GetAutoMapperConversion<TEntity, TViewModel>(canUseAutoMapperProjection);

        return new KendoGrid<TEntity, TViewModel>(request, query, mappings2, conversion2, includes).Data;
    }

    private Func<IQueryable<TEntity>, IEnumerable<TViewModel>> GetAutoMapperConversion<TEntity, TViewModel>(bool canUseAutoMapperProjection = false)
    {
        Func<IQueryable<TEntity>, IEnumerable<TViewModel>> conversion;

        if (AutoMapperUtils.SameTypes<TEntity, TViewModel>())
        {
            conversion = q => q.Cast<TViewModel>().ToList();
        }
        else
        {
            // https://github.com/AutoMapper/AutoMapper/issues/362
            // The idea behind Project().To is to be passed to a query provider like EF or NHibernate that will then do the appropriate SQL creation,
            // not necessarily that the in-memory-execution will work.
            // Project.To has a TON of limitations as it's built explicitly for real query providers, and only does things like MapFrom etc.
            // To put it another way - don't use Project.To unless you're passing that to EF or NH or another DB query provider that knows what to do with the expression tree.
            conversion = canUseAutoMapperProjection
                ? (q => q.ProjectTo<TViewModel>(_mapperConfiguration).AsEnumerable())
                : _mapper.Map<IEnumerable<TViewModel>>;
        }

        return conversion;
    }
}