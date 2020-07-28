using System;
using JetBrains.ReSharper.Feature.Services.CSharp.Analyses.Bulbs;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.TextControl;

namespace Abc.MoqComplete.ContextActions.Services
{
    public interface ICsharpParameterProvider
    {
        int GetCurrentParameterNumber(IObjectCreationExpression selectedElement, ICSharpContextActionDataProvider dataProvider);
        Action<ITextControl> FillCurrentParameterWithMock(string shortName,
                                                          IArgumentList argumentList,
                                                          IObjectCreationExpression selectedElement,
                                                          int parameterNumber,
                                                          ICSharpContextActionDataProvider dataProvider);
    }
}
