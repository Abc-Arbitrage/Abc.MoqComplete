using Abc.MoqComplete.Extensions;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems.Impl;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Naming.Extentions;
using JetBrains.ReSharper.Psi.Naming.Impl;
using JetBrains.ReSharper.Psi.Naming.Settings;
using JetBrains.ReSharper.Psi.Resx.Utils;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace Abc.MoqComplete.CompletionProvider
{
    [Language(typeof(CSharpLanguage))]
    public class MockVariableNameProvider : ItemsProviderOfSpecificContext<CSharpCodeCompletionContext>
    {
        protected override bool IsAvailable(CSharpCodeCompletionContext context)
        {
            var codeCompletionType = context.BasicContext.CodeCompletionType;
            return codeCompletionType == CodeCompletionType.BasicCompletion || codeCompletionType == CodeCompletionType.SmartCompletion;
        }

        protected override bool AddLookupItems(CSharpCodeCompletionContext context, IItemsCollector collector)
        {
            var identifier = context.TerminatedContext.TreeNode as IIdentifier;

            if (identifier == null)
                return false;

            var localVarDeclaration = identifier.Parent as ILocalVariableDeclaration;
            var fieldDeclaration = identifier.Parent as IFieldDeclaration;
            var regularParameterDeclaration = identifier.Parent as IRegularParameterDeclaration;

            var (kind, scalarTypeName) = GetKindAndDeclaration(localVarDeclaration, fieldDeclaration, regularParameterDeclaration);

            if (kind != null && scalarTypeName != null)
                AddToCollector(context, collector, scalarTypeName, kind.Value, ScopeKind.TypeAndNamespace);

            return kind != null;
        }

        private static (NamedElementKinds?, IReferenceName) GetKindAndDeclaration(ILocalVariableDeclaration localVarDeclaration, IFieldDeclaration fieldDeclaration,
                                                  IRegularParameterDeclaration regularParameterDeclaration)
        {
            NamedElementKinds? kind = null;
            IReferenceName scalarTypeName = null;

            if (localVarDeclaration != null)
            {
                kind = NamedElementKinds.Locals;
                scalarTypeName = localVarDeclaration.GetScalarTypename();
            }

            else if (fieldDeclaration != null)
            {
                scalarTypeName = fieldDeclaration.GetScalarTypename();
                var isPrivate = fieldDeclaration.GetAccessRights().Has(AccessRights.PRIVATE);

                if (fieldDeclaration.IsStatic)
                {
                    if (isPrivate)
                        kind = fieldDeclaration.IsReadonly ? NamedElementKinds.PrivateStaticReadonly : NamedElementKinds.PrivateStaticFields;
                    else
                        kind = fieldDeclaration.IsReadonly ? NamedElementKinds.StaticReadonly : NamedElementKinds.PublicFields;
                }
                else
                    kind = isPrivate ? NamedElementKinds.PrivateInstanceFields : NamedElementKinds.PublicFields;
            }

            else if (regularParameterDeclaration != null)
            {
                kind = NamedElementKinds.Parameters;
                scalarTypeName = regularParameterDeclaration.ScalarTypeName;
            }

            return (kind, scalarTypeName);
        }

        private static void AddToCollector(CSharpCodeCompletionContext context, IItemsCollector collector, IReferenceName referenceName, NamedElementKinds elementKinds, ScopeKind localSelfScoped)
        {
            var referenceNameResolveResult = referenceName.Reference.Resolve();
            var referencedElementAsString = referenceNameResolveResult.DeclaredElement.ConvertToString();

            if (referencedElementAsString == "Class:Moq.Mock`1")
            {
                var typeArgumentList = referenceName.TypeArgumentList;
                var typeArguments = typeArgumentList.TypeArguments;

                if (typeArguments.Count == 1)
                {
                    var typeArgument = typeArguments[0];
                    var scalarType = typeArgument.GetScalarType();
                    
                    if (scalarType == null)
                        return;

                    var genericTypeResolveResult = scalarType.Resolve();
                    var namingManager = typeArgument.GetPsiServices().Naming;
                    var suggestionOptions = new SuggestionOptions();
                    string proposedName;

                    if (genericTypeResolveResult.IsEmpty)
                        proposedName = namingManager.Suggestion.GetDerivedName(typeArgument.GetPresentableName(CSharpLanguage.Instance), elementKinds, localSelfScoped,
                                                                               CSharpLanguage.Instance, suggestionOptions, referenceName.GetSourceFile());
                    else
                        proposedName = namingManager.Suggestion.GetDerivedName(genericTypeResolveResult.DeclaredElement, elementKinds, localSelfScoped,
                                                                               CSharpLanguage.Instance, suggestionOptions, referenceName.GetSourceFile());

                    var textLookupItem = new TextLookupItem(proposedName);
                    textLookupItem.InitializeRanges(context.CompletionRanges, context.BasicContext);
                    textLookupItem.SetTopPriority();
                    collector.Add(textLookupItem);

                    var textLookupItem2 = new TextLookupItem(proposedName + "Mock");
                    textLookupItem2.InitializeRanges(context.CompletionRanges, context.BasicContext);
                    textLookupItem2.SetTopPriority();
                    collector.Add(textLookupItem2);
                }
            }
        }
    }
}