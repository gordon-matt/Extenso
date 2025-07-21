namespace Extenso.KendoGridBinder.Containers;

public class FilterObjectWrapper
{
    public string Logic { get; set; }

    public IEnumerable<FilterObject> FilterObjects { get; set; }

    public string LogicToken
    {
        get
        {
            return Logic switch
            {
                "and" => "&&",
                "or" => "||",
                _ => null,
            };
        }
    }
}