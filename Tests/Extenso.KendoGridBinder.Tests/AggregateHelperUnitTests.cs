using Extenso.KendoGridBinder.ModelBinder;
using NUnit.Framework.Legacy;

namespace Extenso.KendoGridBinder.Tests;

[TestFixture]
internal class AggregateHelperUnitTests
{
    [Test]
    public void AggregateHelper_TestMapNull()
    {
        var objects = AggregateHelper.Map(null);
        ClassicAssert.IsNull(objects);
    }
}