using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.ReSharper.Feature.Services;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.TestFramework;
using JetBrains.TestFramework;
using JetBrains.TestFramework.Application.Zones;
using NUnit.Framework;

[assembly: RequiresSTA]
#pragma warning disable 618
[assembly: TestDataPathBase("MoqComplete.Tests/data")]
#pragma warning restore 618

namespace MoqComplete.Tests
{
    [ZoneDefinition]
    public interface IUnitTestZone : ITestsEnvZone, IRequire<PsiFeatureTestZone>
    { }

    [ZoneMarker]
    public class ZoneMarker : IRequire<ICodeEditingZone>, IRequire<ILanguageCSharpZone>, IRequire<IUnitTestZone>
    { }

    [SetUpFixture]
    public class MoqCompleteTestsAssembly : ExtensionTestEnvironmentAssembly<IUnitTestZone>
    {

    }
}
