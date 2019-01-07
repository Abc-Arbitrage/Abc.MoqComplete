using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Features.Intellisense.CodeCompletion.CSharp;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExpectedTypes;
using JetBrains.ReSharper.Psi.Resources;
using JetBrains.ReSharper.Psi.Tree;
using MoqComplete.Extensions;
using System.Linq;

namespace MoqComplete.CompletionProvider
{
    [Language(typeof(CSharpLanguage))]
    public sealed class ItIsAnyParameterProvider : ItemsProviderOfSpecificContext<CSharpCodeCompletionContext>
    {
        protected override bool IsAvailable(CSharpCodeCompletionContext context)
        {
            var codeCompletionType = context.BasicContext.CodeCompletionType;
            return codeCompletionType == CodeCompletionType.BasicCompletion || codeCompletionType == CodeCompletionType.SmartCompletion;
        }

        protected override bool AddLookupItems(CSharpCodeCompletionContext context, IItemsCollector collector)
        {
            var identifier = context.TerminatedContext.TreeNode as IIdentifier;
            var mockedMethodArgument = identifier.GetParentSafe<IReferenceExpression>().GetParentSafe<ICSharpArgument>();

            if (mockedMethodArgument == null)
                return false;

            var mockedMethodInvocationExpression = mockedMethodArgument.GetParentSafe<IArgumentList>().GetParentSafe<IInvocationExpression>();

            if (mockedMethodInvocationExpression == null)
                return false;

            var setupMethodInvocationExpression = mockedMethodInvocationExpression.GetParentSafe<ILambdaExpression>()
                                                                                  .GetParentSafe<IArgument>()
                                                                                  .GetParentSafe<IArgumentList>()
                                                                                  .GetParentSafe<IInvocationExpression>();

            if (setupMethodInvocationExpression == null || !setupMethodInvocationExpression.IsMoqSetupMethod())
                return false;

            var argumentIndex = mockedMethodArgument.IndexOf();

            if (context.ExpectedTypesContext != null)
            {
                foreach (var expectedType in context.ExpectedTypesContext.ExpectedITypes)
                {
                    if (expectedType.Type == null)
                        continue;

                    var typeName = expectedType.Type.GetPresentableName(CSharpLanguage.Instance);
                    var proposedCompletion = context.IsQualified ? $"IsAny<{typeName}>()" : $"It.IsAny<{typeName}>()";
                    AddLookup(context, collector, proposedCompletion);
                }
            }

            if (argumentIndex == 0 && mockedMethodInvocationExpression.Reference != null && !context.IsQualified)
            {
                var mockedMethodResolved = mockedMethodInvocationExpression.Reference.Resolve();
                var declaredElements = Enumerable.Repeat(mockedMethodResolved.DeclaredElement, 1).Concat(mockedMethodResolved.Result.Candidates).Where(x => x != null);
                var methods = declaredElements.OfType<IMethod>().Where(x => x.Parameters.Count > 1).ToList();

                methods.ForEach(method =>
                {
                    var parameter = method.Parameters.Select(x => "It.IsAny<" + x.Type.GetPresentableName(CSharpLanguage.Instance) + ">()");
                    var proposedCompletion = string.Join(", ", parameter);
                    AddLookup(context, collector, proposedCompletion);
                });
            }

            return true;
        }

        private static void AddLookup(CSharpCodeCompletionContext context, IItemsCollector collector, string proposedCompletion)
        {
            var textLookupItem = CSharpLookupItemFactory.Instance.CreateKeywordLookupItem(context, proposedCompletion, TailType.None, PsiSymbolsThemedIcons.Method.Id);
            textLookupItem.WithInitializedRanges(context.CompletionRanges, context.BasicContext);
            textLookupItem.PlaceTop();
            collector.Add(textLookupItem);
        }
    }
}
