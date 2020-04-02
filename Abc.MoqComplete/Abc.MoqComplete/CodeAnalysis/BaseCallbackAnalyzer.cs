using System.Linq;
using Abc.MoqComplete.Services;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Conversions;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace Abc.MoqComplete.CodeAnalysis
{
	public abstract class BaseCallbackAnalyzer : ElementProblemAnalyzer<IInvocationExpression>
	{
		protected abstract TreeNodeCollection<ICSharpArgument>? GetArguments(ISolution solution, IInvocationExpression methodInvocation);

		protected abstract void AddHighlighting(IHighlightingConsumer consumer, DocumentRange range);

		protected override void Run(IInvocationExpression element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
		{
			var methodIdentifier = element.GetSolution().GetComponent<IMoqMethodIdentifier>();

			if (!methodIdentifier.IsMoqCallbackMethod(element))
			{
				return;
			}

			var expectedTypeParameters = element.TypeArguments;

			if (expectedTypeParameters.Count == 0)
			{
				return;
			}

			var pointer = element.InvokedExpression;
			TreeNodeCollection<ICSharpArgument>? arguments = null;

			while (pointer != null && arguments == null && pointer.FirstChild is IInvocationExpression methodInvocation)
			{
				arguments = GetArguments(element.GetSolution(), methodInvocation);
				pointer = methodInvocation.InvokedExpression;
			}

			if (arguments == null)
			{
				return;
			}

			var actualTypesParameters = arguments.Value.Select(x => x.Value.Type()).ToArray();
			var rule = element.GetPsiModule().GetTypeConversionRule();

			if (actualTypesParameters.Length <= 0)
			{
				return;
			}

			if (expectedTypeParameters.Count != actualTypesParameters.Length)
			{
				AddWarning(element, consumer);
			} else
			{
				for (var i = 0; i < expectedTypeParameters.Count; i++)
				{
					var actualParameterType = actualTypesParameters[i];
					var expectedParameterType = expectedTypeParameters[i];

					if (!actualParameterType.Equals(expectedParameterType)
						&& !actualParameterType.IsImplicitlyConvertibleTo(expectedParameterType, rule))
					{
						AddWarning(element, consumer);
					}
				}
			}
		}

		private void AddWarning(IInvocationExpression element, IHighlightingConsumer consumer)
		{
			DocumentRange range;

			if (element.FirstChild?.LastChild is ITypeArgumentList typeInvocation)
			{
				range = typeInvocation.GetDocumentRange();
			} else
			{
				range = element.InvokedExpression.GetDocumentRange();
			}

			AddHighlighting(consumer, range);
		}
	}
}