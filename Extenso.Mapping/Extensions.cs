namespace Extenso.Mapping;

public static class Extensions
{
    public static TDest MapTo<TSource, TDest>(this TSource source) => ExtensoMapper.Map<TSource, TDest>(source);

    public static TDest MapTo<TDest>(this object source) => source == null
        ? throw new ArgumentNullException(nameof(source))
        : (TDest)ExtensoMapper.Map(source, source.GetType(), typeof(TDest));
}