using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CSharp.ContextActions;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace Abc.MoqComplete.ContextActions.Services
{
    [SolutionComponent]
    public class CsharpMemberProvider : ICsharpMemberProvider
    {
        public IEnumerable<string> GetConstructorParameters(string constructorString)
        {
            var index = 0;
            var openedBracket = 0;

            while (constructorString[index] != '(')
                index++;

            var sb = new StringBuilder();
            while (constructorString[index] != ')')
            {
                index++;
                
                if (constructorString[index] == '<')
                    openedBracket++;
                
                else if (constructorString[index] == '>')
                    openedBracket--;

                if (openedBracket ==0 && (constructorString[index] == ',' || constructorString[index] == ')'))
                {
                    yield return sb.ToString();
                    sb.Clear();
                }
                else
                    sb.Append(constructorString[index]);
            }
        }

        public Dictionary<string, string> GetClassFields(IClassBody classBody, PsiLanguageType languageType)
        {
            var dic = new Dictionary<string, string>();
            var fields = classBody.FieldDeclarations.Select(x => x.TypeUsage.FirstChild as IReferenceName).Where(x => x != null && x.ShortName == "Mock").ToArray();

            foreach (var referenceName in fields)
            {
                var types = referenceName.TypeArguments.Select(x => x.GetPresentableName(languageType, DeclaredElementPresenterTextStyles.Empty).Text);
                var strType = string.Join(",", types);
                var mockType = GetGenericMock(strType);
                var field = (IFieldDeclaration)referenceName.Parent.NextSibling.NextSibling;

                if (!dic.ContainsKey(mockType))
                    dic.Add(mockType, field.DeclaredName);
            }

            return dic;
        }

        public string GetGenericMock(string typeStr)
        {
            return $"Moq.Mock<{typeStr}>";
        }

        public bool IsAbstractOrInterface(IParameter parameter)
        {
            var declaredElement = parameter.Type.GetScalarType()?.Resolve().DeclaredElement;

            return declaredElement is IInterface 
                   || declaredElement is IClass c && c.IsAbstract;
        }

        public int GetCurrentParameterNumber(IObjectCreationExpression selectedElement, ICSharpContextActionDataProvider dataProvider)
        {
            var delimiterPositions = selectedElement.Delimiters.Select(x => x.GetNavigationRange().StartOffset.Offset).ToArray();
            var currentPosition = dataProvider.DocumentSelection.StartOffset.Offset;

            var parameterNumber = 0;
            while (parameterNumber < delimiterPositions.Length && currentPosition > delimiterPositions[parameterNumber])
                parameterNumber++;

            return parameterNumber;
        }

        public Action<ITextControl> FillCurrentParameterWithMock(string shortName,
                                                                 IArgumentList argumentList,
                                                                 IObjectCreationExpression selectedElement,
                                                                 int parameterNumber,
                                                                 ICSharpContextActionDataProvider dataProvider)
        {
            var argument = dataProvider.ElementFactory.CreateArgument(ParameterKind.VALUE, dataProvider.ElementFactory.CreateExpression($"{shortName}.Object"));
            ICSharpArgument arg;
            var shouldRemoveEndComma = true;

            if (argumentList.Arguments.Count <= 1)
                arg = selectedElement.AddArgumentAfter(argument, null);
            else if (parameterNumber != 0)
                arg = selectedElement.AddArgumentAfter(argument, argumentList.Arguments[parameterNumber - 1]);
            else
            {
                arg = selectedElement.AddArgumentBefore(argument, argumentList.Arguments[1]);
                shouldRemoveEndComma = false;
            }

            var argumentRange = arg.GetDocumentRange();

            // Remove last comma Hack!
            return textControl =>
            {
                TextRange range;

                if (shouldRemoveEndComma)
                    range = new TextRange(argumentRange.EndOffset.Offset, argumentRange.EndOffset.Offset + 1);
                else
                    range = new TextRange(argumentRange.StartOffset.Offset - 2, argumentRange.StartOffset.Offset);

                var text = textControl.Document.GetText(range);

                if (text.Contains(","))
                    textControl.Document.DeleteText(range);
            };
        }
    }
}