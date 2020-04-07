using System.Collections.Generic;

namespace Abc.MoqComplete.ContextActions.Services
{
    public interface IParameterProvider
    {
        IEnumerable<string> GetParameters(string constructorString);
    }
}
