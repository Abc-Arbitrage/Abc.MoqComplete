using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

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
    }
}