using System.Collections.Generic;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace Abc.MoqComplete.ContextActions.Services
{
    public interface ICsharpMemberProvider
    {
        IEnumerable<string> GetConstructorParameters(string constructorString);
        Dictionary<string, string> GetClassFields(IClassBody classBody, PsiLanguageType languageType);
        string GetGenericMock(string typeStr);
    }
}
