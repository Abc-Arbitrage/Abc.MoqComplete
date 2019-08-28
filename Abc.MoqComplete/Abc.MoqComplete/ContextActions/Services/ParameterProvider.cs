using System.Collections.Generic;
using System.Text;
using JetBrains.ProjectModel;

namespace Abc.MoqComplete.ContextActions.Services
{
    [SolutionComponent]
    public class ParameterProvider : IParameterProvider
    {
        public IEnumerable<string> GetParameters(string constructorString)
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
    }
}