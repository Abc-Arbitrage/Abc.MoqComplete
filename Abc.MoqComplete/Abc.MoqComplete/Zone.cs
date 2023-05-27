using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.ReSharper.Psi.CSharp;

namespace Abc.MoqComplete
{
    [ZoneMarker]
    public sealed class ZoneMarker : IRequire<ILanguageCSharpZone>
    {
    }
}
