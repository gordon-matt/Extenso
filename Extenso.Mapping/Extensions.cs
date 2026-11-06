namespace Extenso.Mapping;

/// <summary>
/// Provides extension methods for values.
/// </summary>
public static class Extensions
{
    //extension<TSource>(TSource source)
    //{
    //    public TDest MapTo<TDest>() => ExtensoMapper.Map<TSource, TDest>(source);
    //}

    extension(object source)
    {
        /// <summary>
        /// Maps to.
        /// </summary>
        /// <typeparam name="TDest">The type of the dest.</typeparam>
        /// <returns>The t dest.</returns>
        public TDest MapTo<TDest>() => source is null
            ? throw new ArgumentNullException(nameof(source))
            : (TDest)ExtensoMapper.Map(source, source.GetType(), typeof(TDest));
    }
}