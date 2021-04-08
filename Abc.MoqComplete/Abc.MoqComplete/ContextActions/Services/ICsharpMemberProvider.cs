using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Feature.Services.CSharp.ContextActions;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.TextControl;

namespace Abc.MoqComplete.ContextActions.Services
{
    public interface ICsharpMemberProvider
    {
        IEnumerable<string> GetConstructorParameters(string constructorString);
        Dictionary<string, string> GetClassFields(IClassBody classBody, PsiLanguageType languageType);
        string GetGenericMock(string typeStr);
        bool IsAbstractOrInterface(IParameter parameter);
        int GetCurrentParameterNumber(IObjectCreationExpression selectedElement, ICSharpContextActionDataProvider dataProvider);
        Action<ITextControl> FillCurrentParameterWithMock(string shortName,
                                                          IArgumentList argumentList,
                                                          IObjectCreationExpression selectedElement,
                                                          int parameterNumber,
                                                          ICSharpContextActionDataProvider dataProvider);
    }
}
