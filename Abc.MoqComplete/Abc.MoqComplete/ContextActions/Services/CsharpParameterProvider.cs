using System;
using System.Linq;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CSharp.Analyses.Bulbs;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace Abc.MoqComplete.ContextActions.Services
{
    [SolutionComponent]
    public class CsharpParameterProvider : ICsharpParameterProvider
    {
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
