using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Parts;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.Modules;

namespace Abc.MoqComplete.Services
{
    [SolutionComponent(Instantiation.DemandAnyThreadUnsafe)]
    public class TestProjectProvider : ITestProjectProvider
    {
        private readonly ConcurrentDictionary<string, bool> _isMoqContainedByProjectName = new ConcurrentDictionary<string, bool>();

        private static readonly string[] MoqReferenceNames =
        {
            "Moq",
            "Moq.AutoMock"
        };

        public bool IsTestProject(IPsiModule psiModule)
        {
            if (!_isMoqContainedByProjectName.TryGetValue(psiModule.DisplayName, out var isMoqContained))
            {
                var references = psiModule.GetPsiServices().Modules.GetModuleReferences(psiModule);
                isMoqContained = references.Any(r => MoqReferenceNames.Contains(r.Module.Name));
                _isMoqContainedByProjectName[psiModule.DisplayName] = isMoqContained;
            }

            return isMoqContained;
        }
    }
}
