namespace Extenso.Data.Entity
{
    public interface IEntity
    {
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        object[] KeyValues { get; }
    }
}