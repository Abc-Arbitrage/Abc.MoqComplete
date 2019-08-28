using System.Collections.Generic;
using JetBrains.ProjectModel;

namespace Abc.MoqComplete.ContextActions.Services
{
    public interface IParameterProvider
    {
        IEnumerable<string> GetParameters(string constructorString);
    }
}
