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
        private const string _moqReferenceName = "Moq";

        public bool IsTestProject(IPsiModule psiModule)
        {
            if (!_isMoqContainedByProjectName.TryGetValue(psiModule.DisplayName, out var isMoqcontained))
            {
                isMoqcontained = psiModule.GetReferences(null).Any(r => r.Module.Name == _moqReferenceName);
                _isMoqContainedByProjectName.Add(psiModule.DisplayName, isMoqcontained);
            }

            return isMoqcontained;
        }
    }
}