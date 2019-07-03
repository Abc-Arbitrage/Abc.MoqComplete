using JetBrains.ReSharper.Psi.Modules;

namespace Abc.MoqComplete.Services
{
    public interface ITestProjectProvider
    {
        bool IsTestProject(IPsiModule psiModuleDisplayName);
    }
}
