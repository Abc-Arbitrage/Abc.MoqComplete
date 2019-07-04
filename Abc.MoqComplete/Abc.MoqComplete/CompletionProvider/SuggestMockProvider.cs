using System.Collections.Generic;
using System.Linq;
using Abc.MoqComplete.Extensions;
using Abc.MoqComplete.Services;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Features.Intellisense.CodeCompletion.CSharp;
using JetBrains.ReSharper.Features.Intellisense.CodeCompletion.CSharp.Rules;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.ExpectedTypes;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Resources;
using JetBrains.ReSharper.Psi.Resx.Utils;
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
            var candidateExistingElements = new List<ISymbolInfo>();
            var table = GetSymbolTable(context);

            table?.ForAllSymbolInfos(info =>
            {
                var declaredElement = info.GetDeclaredElement();
                var type = declaredElement.Type();

                if (type != null)
                {
                    if (type.GetClassType().ConvertToString() == "Class:Moq.Mock`1")
                    {
                        IType typeParameter = TypesUtil.GetTypeArgumentValue(type, 0);
                        if (typeParameter != null && context.ExpectedTypesContext != null && context.ExpectedTypesContext.ExpectedITypes != null && context.ExpectedTypesContext.ExpectedITypes.Select(x => x.Type).Where(x => x != null).Any(x => typeParameter.IsExplicitlyConvertibleTo(x, ClrPredefinedTypeConversionRule.INSTANCE)))
                        {
                            candidateExistingElements.Add(info);
                        }
                    }
                }
            });

            foreach (var candidateExistingElement in candidateExistingElements)
            {
                var proposedCompletion = candidateExistingElement.ShortName + ".Object";
                var lookupItem = GetLookupItem(context, proposedCompletion);
                collector.Add(lookupItem);
            }

            if (context.ExpectedTypesContext != null)
            {
                foreach (var expectedType in context.ExpectedTypesContext.ExpectedITypes)
                {
                    if (expectedType.Type == null)
                        continue;

                    if (expectedType.Type.IsInterfaceType())
                    {
                        string typeName = expectedType.Type.GetPresentableName(CSharpLanguage.Instance);
                        var proposedCompletion = "new Mock<" + typeName + ">().Object";
                        var lookupItem = GetLookupItem(context, proposedCompletion);
                        collector.Add(lookupItem);
                    }
                }
            }
            return true;
        }

        private static ILookupItem GetLookupItem(CSharpCodeCompletionContext context, string proposedCompletion)
        {
            var lookupItem = CSharpLookupItemFactory.Instance.CreateKeywordLookupItem(context, proposedCompletion, TailType.None, PsiSymbolsThemedIcons.Variable.Id);
            lookupItem.WithInitializedRanges(context.CompletionRanges, context.BasicContext);
            lookupItem.SetTopPriority();
            return lookupItem;
        }
    }
}
