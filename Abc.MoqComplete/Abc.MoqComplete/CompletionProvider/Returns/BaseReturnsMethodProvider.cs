using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abc.MoqComplete.Extensions;
using Abc.MoqComplete.Services;
using Abc.MoqComplete.Services.MethodProvider;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Features.Intellisense.CodeCompletion.CSharp;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExpectedTypes;
using JetBrains.ReSharper.Psi.Resources;
using JetBrains.ReSharper.Psi.Tree;

namespace Abc.MoqComplete.CompletionProvider.Returns
{
    public abstract class BaseReturnsMethodProvider<T> : ItemsProviderOfSpecificContext<CSharpCodeCompletionContext> where T : class, IMockedMethodProvider
    {
        protected override bool IsAvailable(CSharpCodeCompletionContext context)
        {
            CodeCompletionType codeCompletionType = context.BasicContext.CodeCompletionType;

            return codeCompletionType == CodeCompletionType.SmartCompletion || codeCompletionType == CodeCompletionType.BasicCompletion;
        }

        protected override bool AddLookupItems(CSharpCodeCompletionContext context, IItemsCollector collector)
        {
            IIdentifier identifier = context.TerminatedContext.TreeNode as IIdentifier;
            IReferenceExpression expression = identifier.GetParentSafe<IReferenceExpression>();

            if (expression == null)
            {
                return false;
            }

            if (!(expression.ConditionalQualifier is IInvocationExpression invocation))
            {
                return false;
            }

            ISolution solution = context.BasicContext.Solution;
            IMoqMethodIdentifier methodIdentifier = solution.GetComponent<IMoqMethodIdentifier>();

            if (methodIdentifier.IsMoqCallbackMethod(invocation))
            {
                invocation = invocation.InvokedExpression?.FirstChild as IInvocationExpression;
            }

            T methodProvider = solution.GetComponent<T>();
            IMethod mockedMethod = methodProvider.GetMockedMethodFromSetupMethod(invocation);

            if (mockedMethod == null || mockedMethod.Parameters.Count == 0 || mockedMethod.ReturnType.IsVoid())
            {
                return false;
            }

            List<string> types = methodProvider.GetMockedMethodParameterTypesString(invocation).ToList();
            List<string> variablesName = mockedMethod.Parameters.Select(p => p.ShortName).ToList();
            string returnCallback = $"Returns<{string.Join(", ", types)}>(({string.Join(", ", variablesName)}) => )";
            AddProposedCallback(context, collector, returnCallback);

            IType returnType = mockedMethod.ReturnType;

            if (returnType.IsGenericTask() || returnType.IsGenericValueTask())
            {
                var returnsAsyncCallback = new StringBuilder("ReturnsAsync((");
                for (var i = 0; i < types.Count; i++)
                {
                    returnsAsyncCallback.Append($"{types[i]} {variablesName[i]}, ");
                }
                
                // Remove last space and comma
                returnsAsyncCallback.Remove(returnsAsyncCallback.Length - 2, 2);
                returnsAsyncCallback.Append(") => )");
                
                AddProposedCallback(context, collector, returnsAsyncCallback.ToString());
            }

            return true;
        }

        private static void AddProposedCallback(CSharpCodeCompletionContext context, IItemsCollector collector, string proposedCallback)
        {
            var item = CSharpLookupItemFactory.Instance.CreateKeywordLookupItem(context,
                proposedCallback,
                TailType.None,
                PsiSymbolsThemedIcons.Method.Id);

            item.SetInsertCaretOffset(-1);
            item.SetReplaceCaretOffset(-1);
            item.WithInitializedRanges(context.CompletionRanges, context.BasicContext);
            item.SetTopPriority();
            collector.Add(item);
        }
    }
}