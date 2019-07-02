using JetBrains.ReSharper.Psi.Modules;

namespace MoqComplete.Services
{
    public interface ITestProjectProvider
    {
        bool IsTestProject(IPsiModule psiModuleDisplayName);
    }
}
