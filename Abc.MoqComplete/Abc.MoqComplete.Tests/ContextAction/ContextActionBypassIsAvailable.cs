using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Diagnostics;
using JetBrains.DocumentManagers;
using JetBrains.Lifetimes;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.ContextActions;
using JetBrains.ReSharper.Feature.Services.Intentions;
using JetBrains.ReSharper.Feature.Services.Util;
using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using JetBrains.ReSharper.Psi;

namespace Abc.MoqComplete.Tests.ContextAction
{
    public abstract class ContextActionBypassIsAvailable<TAction> : ContextActionExecuteTestBase<TAction>
        where TAction : ContextActionBase
    {
        protected override void DoTestAction(Lifetime lifetime, IProject project)
        {
            var solution = project.GetSolution();
            var documentManager = solution.GetComponent<DocumentManager>();
            var caretPosition = GetCaretPosition() ?? CaretPositionsProcessor.PositionNames.SelectMany(i => CaretPositionsProcessor.Positions(i)).First("Caret position is not set. Please add {caret} or {selstart} to a test file.");
            var textControl = OpenTextControl(lifetime, caretPosition);
            var name = InitTextControl(textControl);
            ContextActionInstance contextAction;
            using (BulbMenuTestUtil.AssertNoWriteLockOnTheMainThreadRequested(Locks))
            {
                contextAction = CreateContextAction(solution, textControl).NotNull();
            }
            ExecuteWithGold(textControl.Document, writer =>
            {
                IReadOnlyList<IIntentionAction> sortedBulbActions;
                using (BulbMenuTestUtil.AssertNoWriteLockOnTheMainThreadRequested(Locks))
                {
                    sortedBulbActions = CreateSortedBulbActions(textControl, contextAction);
                    BulbMenuTestUtil.ExecuteAllTextsInMenu(sortedBulbActions);
                }
                if (sortedBulbActions.Count == 0)
                {
                    writer.WriteLine("NOT AVAILABLE");
                }

                var execute = SelectActionToExecute(sortedBulbActions, name, textControl);
                if (execute == null)
                {
                    writer.WriteLine("NOT AVAILABLE FOR {0}", name);
                    foreach (var intentionAction in sortedBulbActions)
                        writer.WriteLine(intentionAction.BulbAction.Text);
                }
                else
                {
                    var markersSet = new TextMarkersSet();
                    ExecuteBulbAction(textControl, execute.BulbAction, solution, markersSet);
                    var projectFile = documentManager.TryGetProjectFile(textControl.Document);
                    if (projectFile == null)
                    {
                        writer.Write("File is removed");
                    }
                    else
                    {
                        DumpTextControl(textControl, writer, true, true, markersSet);
                        WriteCodeBehind(textControl.Document, writer);
                        if (LanguageForDumpRanges == null)
                            return;
                        DumpRanges(projectFile.ToSourceFile().NotNull(), writer, false, LanguageForDumpRanges.GetType());
                    }
                }
            });

            foreach (var allProjectFile in project.GetAllProjectFiles())
            {
                var projectFile = allProjectFile;
                if (!(projectFile.Location == caretPosition.FileName) && !Enumerable.Contains(CaretPositionsProcessor.SkipExtensions, projectFile.Location.ExtensionWithDot, StringComparer.OrdinalIgnoreCase))
                    ExecuteWithGold(projectFile, sw =>
                    {
                        var document = documentManager.GetOrCreateDocument(projectFile);
                        sw.Write(document.GetText());
                        if (LanguageForDumpRanges == null)
                            return;
                        DumpRanges(projectFile.ToSourceFile().NotNull(), sw, false, LanguageForDumpRanges.GetType());
                    });
            }
        }
    }
}
