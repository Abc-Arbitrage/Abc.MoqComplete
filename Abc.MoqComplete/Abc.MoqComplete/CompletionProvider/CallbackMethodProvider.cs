using System.Linq;
using Abc.MoqComplete.Extensions;
using Abc.MoqComplete.Services;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.AspectLookupItems.BaseInfrastructure;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.AspectLookupItems.Info;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Features.Intellisense.CodeCompletion.CSharp;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExpectedTypes;
using JetBrains.ReSharper.Psi.Resources;
using JetBrains.ReSharper.Psi.Tree;

namespace Abc.MoqComplete.CompletionProvider
{
    [Language(typeof(CSharpLanguage))]
    public class CallbackMethodProvider : ItemsProviderOfSpecificContext<CSharpCodeCompletionContext>
    {
        protected override bool IsAvailable(CSharpCodeCompletionContext context)
        {
            var codeCompletionType = context.BasicContext.CodeCompletionType;
            return codeCompletionType == CodeCompletionType.SmartCompletion || codeCompletionType == CodeCompletionType.BasicCompletion;
        }

        protected override bool AddLookupItems(CSharpCodeCompletionContext context, IItemsCollector collector)
        {
            var methodIdentitifer = context.BasicContext.Solution.GetComponent<IMoqMethodIdentifier>();
            var mockedMethodProvider = context.BasicContext.Solution.GetComponent<IMockedMethodProvider>();
            var identifier = context.TerminatedContext.TreeNode as IIdentifier;
            var expression = identifier.GetParentSafe<IReferenceExpression>();
            if (expression == null)
                return false;

            if (!(expression.ConditionalQualifier is IInvocationExpression invocation))
                return false;

            if (methodIdentitifer.IsMoqReturnMethod(invocation))
                invocation = invocation.InvokedExpression?.FirstChild as IInvocationExpression;

            var mockedMethod = mockedMethodProvider.GetMockedMethodFromSetupMethod(invocation);
            
            if (mockedMethod == null || mockedMethod.Parameters.Count == 0)
                return false;

            var types = mockedMethodProvider.GetMockedMethodParameterTypes(invocation);
            var variablesName = mockedMethod.Parameters.Select(p => p.ShortName);
            var proposedCallback = $"Callback<{string.Join(",", types)}>(({string.Join(",", variablesName)}) => {{}})";
            var item = CSharpLookupItemFactory.Instance.CreateKeywordLookupItem(context, proposedCallback, TailType.None, PsiSymbolsThemedIcons.Method.Id) as LookupItem<TextualInfo>;
            if (item == null)
                return false;

            item.SetInsertCaretOffset(-2);
            item.SetReplaceCaretOffset(-2);
            item.WithInitializedRanges(context.CompletionRanges, context.BasicContext);
            item.SetTopPriority();
            collector.Add(item);
            return true;
        }
    }
}
