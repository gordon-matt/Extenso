namespace Extenso.Mapping;

public static class Extensions
{
    //extension<TSource>(TSource source)
    //{
    //    public TDest MapTo<TDest>() => ExtensoMapper.Map<TSource, TDest>(source);
    //}

    extension(object source)
    {
        public TDest MapTo<TDest>() => source is null
            ? throw new ArgumentNullException(nameof(source))
            : (TDest)ExtensoMapper.Map(source, source.GetType(), typeof(TDest));
    }
}