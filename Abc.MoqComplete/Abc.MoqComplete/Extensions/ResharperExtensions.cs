using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.Tree;

namespace Abc.MoqComplete.Extensions
{
    internal static class ResharperExtensions
    {
        [CanBeNull]
        public static T GetParentSafe<T>([CanBeNull] this ITreeNode treeNode) where T : class => treeNode?.Parent as T;
    }
}
