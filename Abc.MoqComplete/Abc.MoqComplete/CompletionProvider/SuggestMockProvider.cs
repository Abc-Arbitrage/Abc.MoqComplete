using Abc.MoqComplete.Extensions;
using Abc.MoqComplete.Services;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Features.Intellisense.CodeCompletion.CSharp;
using JetBrains.ReSharper.Features.Intellisense.CodeCompletion.CSharp.Rules;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExpectedTypes;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Resources;
using JetBrains.ReSharper.Psi.Util;

namespace Abc.MoqComplete.CompletionProvider
{
    [Language(typeof(CSharpLanguage))]
    public class SuggestMockProvider : CSharpItemsProviderBase<CSharpCodeCompletionContext>
    {
        protected override bool IsAvailable(CSharpCodeCompletionContext context)
        {
            var codeCompletionType = context.BasicContext.CodeCompletionType;
            if (codeCompletionType != CodeCompletionType.BasicCompletion && codeCompletionType != CodeCompletionType.SmartCompletion)
                return false;

            var testProjectProvider = context.BasicContext.Solution.GetComponent<ITestProjectProvider>();

            return testProjectProvider.IsTestProject(context.PsiModule);
        }

        protected override bool AddLookupItems(CSharpCodeCompletionContext context, IItemsCollector collector)
        {
            if (context.ExpectedTypesContext == null)
                return false;

            foreach (var expectedType in context.ExpectedTypesContext.ExpectedITypes)
            {
                if (expectedType.Type.IsInterfaceType())
                {
                    var typeName = expectedType.Type.GetPresentableName(CSharpLanguage.Instance);
                    var newMock = GetLookupItem(context, "new Mock<" + typeName + ">().Object");
                    var mockOf = GetLookupItem(context, $"Mock.Of<{typeName}>()");
                    collector.Add(newMock);
                    collector.Add(mockOf);
                }
            }
            return context.ExpectedTypesContext.ExpectedITypes.Count > 0;
        }

        private static ILookupItem GetLookupItem(CSharpCodeCompletionContext context, string proposedCompletion)
        {
            var lookupItem = CSharpLookupItemFactory.Instance.CreateKeywordLookupItem(context, proposedCompletion, TailType.None, PsiSymbolsThemedIcons.Variable.Id);

            var node = context.NodeInFile;
            while (!(node is ICSharpArgument) && node != null) node = node?.Parent;
            if (node != null)
            {
                var arg = (ICSharpArgument)node;
                var range = arg.GetExtendedDocumentRange();
                lookupItem.SetRanges(context.CompletionRanges.WithReplaceRange(range));
            }

            lookupItem.SetTopPriority();
            return lookupItem;
        }
    }
}
