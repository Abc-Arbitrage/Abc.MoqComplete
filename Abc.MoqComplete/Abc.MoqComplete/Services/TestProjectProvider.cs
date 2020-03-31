using System.Collections.Generic;
using System.Linq;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.Modules;

namespace Abc.MoqComplete.Services
{
    [SolutionComponent]
    public class TestProjectProvider : ITestProjectProvider
    {
        private readonly Dictionary<string, bool> _isMoqContainedByProjectName = new Dictionary<string, bool>();

        private static readonly string[] MoqReferenceNames =
        {
            "Moq",
            "Moq.AutoMock"
        };

        public bool IsTestProject(IPsiModule psiModule)
        {
            if (!_isMoqContainedByProjectName.TryGetValue(psiModule.DisplayName, out var isMoqContained))
            {
                isMoqContained = psiModule.GetReferences(null).Any(r => MoqReferenceNames.Contains(r.Module.Name));
                _isMoqContainedByProjectName.Add(psiModule.DisplayName, isMoqContained);
            }

            return isMoqContained;
        }
    }
}