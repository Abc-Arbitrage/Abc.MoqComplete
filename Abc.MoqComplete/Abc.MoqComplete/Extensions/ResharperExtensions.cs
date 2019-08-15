using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace Abc.MoqComplete.Extensions
{
    internal static class ResharperExtensions
    {
        [CanBeNull]
        public static T GetParentSafe<T>([CanBeNull] this ITreeNode treeNode) where T : class => treeNode?.Parent as T;

        public static IReferenceName GetScalarTypename(this IDeclaration fieldDeclaration)
        {
            CompositeElement compositeElement = (CompositeElement)fieldDeclaration;
            if (!(compositeElement.parent is IMultipleFieldDeclaration) || compositeElement.parent.GetChildRole((TreeElement)compositeElement) != (short)104)
                return (IReferenceName)null;
            CompositeElement parent = compositeElement.parent;
            IReferenceName referenceName = (IReferenceName)null;
            CompositeElement childByRole1 = (CompositeElement)parent.FindChildByRole((short)19);
            if (childByRole1 != null)
            {
                TreeElement childByRole2 = childByRole1.FindChildByRole((short)19);
                if (childByRole2 != null)
                    referenceName = (IReferenceName)childByRole2;
            }
            return referenceName;
        }
    }
}
