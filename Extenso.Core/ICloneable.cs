namespace Extenso
{
    public interface ICloneable<T>
    {
        T ShallowCopy();

        T DeepCopy();
    }
}