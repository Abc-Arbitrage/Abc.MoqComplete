using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Components;
using JetBrains.Diagnostics;
using JetBrains.DocumentManagers;
using JetBrains.Lifetimes;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.ContextActions;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Hotspots;
using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.TestFramework;
using JetBrains.TextControl;
using NUnit.Framework;

namespace Abc.MoqComplete.Tests.ContextAction
{
    public abstract class ContextActionBypassIsAvailable<TAction> : ContextActionExecuteTestBase<TAction>
        where TAction : ContextActionBase
    {
        protected override void ProcessAction(Lifetime lifetime, Func<ITextControl, TAction> actionCreator, IProject project)
        {
            var solution = project.GetSolution();
            var documentManager = solution.GetComponent<DocumentManager>();
            var caretPosition = GetCaretPosition() ?? CaretPositionsProcessor.PositionNames.SelectMany(_ => CaretPositionsProcessor.Positions(_) as IEnumerable<CaretPosition>).First("Caret position is not set. Please add {caret} or {selstart} to a test file.");
            var textControl = OpenTextControl(lifetime, project, caretPosition);
            var name = InitTextControl(textControl);
            var contextAction = actionCreator(textControl);
            ExecuteWithGold(textControl.Document, sw =>
            {
                Assert.NotNull(contextAction);
                var list = ComputeActions(contextAction, textControl).Select(x => x.BulbAction).ToList();
                if (list.Count == 0)
                {
                    OnNoItemsForAvailableAction(contextAction, textControl, sw);
                }
                else
                {
                    var bulbAction1 = SelectItem(contextAction, name, textControl);
                    if (bulbAction1 == null)
                    {
                        sw.WriteLine("NOT AVAILABLE FOR {0}", name);
                        foreach (var bulbAction2 in list)
                            sw.WriteLine(bulbAction2.Text);
                    }
                    else
                    {
                        var document = textControl.Document;
                        ExecuteItem(textControl, bulbAction1, solution);
                        var currentSession = ShellInstance.GetComponent<HotspotSessionExecutor>().CurrentSession;
                        if (currentSession != null)
                        {
                            while (!currentSession.HotspotSession.IsFinished)
                            {
                                ProcessHotspot(textControl, currentSession.HotspotSession.CurrentHotspot.NotNull());
                                currentSession.HotspotSession.GoToNextHotspot();
                            }
                        }
                        var projectFile = documentManager.TryGetProjectFile(document);
                        if (projectFile == null)
                        {
                            sw.Write("File is removed");
                        }
                        else
                        {
                            DumpTextControl(textControl, DumpCaretPosition, DumpSelection)(sw);
                            WriteCodeBehind(document, sw);
                            if (LanguageForDumpRanges == null)
                                return;
                            DumpRanges(projectFile.ToSourceFile().NotNull(), sw, false, LanguageForDumpRanges.GetType());
                        }
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
