using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CSharp.ContextActions;

namespace Abc.MoqComplete.ContextActions
{
    public class ComponentResolver
    {
        public static T GetComponent<T>(ICSharpContextActionDataProvider dataProvider ) where T : class => dataProvider.PsiModule.GetSolution().GetComponent<T>();
    }
}
