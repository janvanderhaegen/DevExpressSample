using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Build.Utilities;
using System.Text.RegularExpressions;

[Export(typeof(TaskLogger)), PartCreationPolicy(CreationPolicy.Shared)]
public class TaskLogger
{
    public IList<string> LogMessages { get; } = new List<string>();
}


public sealed class WeaveBugfixTask : Task
{
    private const string devexpressAssemblyName = @"(.*)DevExpress\.XtraReports\.v\d{1,2}\.\d{1,2}\.dll$";
    private static readonly Regex devexpressAssemblyRecognizer = new Regex(devexpressAssemblyName, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Compiled);
    public string References { get; set; }
    public string AssemblyLocation { get; private set; }

    static AssemblyCatalog assemblyCatalog;

    static WeaveBugfixTask()
    {
        assemblyCatalog = new AssemblyCatalog(typeof(WeaveBugfixTask).Assembly);
    }

    public override bool Execute()
    {
        if (string.IsNullOrEmpty(this.References))
        {
            this.Log.LogMessage("Bugfix Weaver: no references specified. Cannot weave. If you have not performed a build you can ignore this message");
            return true;
        }
        else
        {
            this.AssemblyLocation = this.References.Split(';').Select(c => c.Trim()).SingleOrDefault(c => devexpressAssemblyRecognizer.IsMatch(c));
            if (string.IsNullOrEmpty(this.AssemblyLocation))
            {
                this.Log.LogMessage("Bugfix Weaver: no references found to the DevExpress.XtraReports assembly. Cannot weave. If you have not performed a build you can ignore this message");
                return true;
            }
        }

        using (var container = new CompositionContainer(assemblyCatalog))
        {
            try
            {
                this.Log.LogMessage("Bugfix Weaver: starting");
                container.ComposeExportedValue(this);
                container.ComposeExportedValue(BuildEngine);
                container.GetExportedValue<ModuleReader>().Execute();

                if (!container.GetExportedValue<FileChangedChecker>().ShouldStart())
                    return true;

                foreach (var compilationStep in container.GetExportedValues<ICompilationStep>())
                {
                    compilationStep.Execute();
                }

                //Saving back to disk 
                container.GetExportedValue<ModuleWriter>().Execute();

            }
            finally
            {
                foreach (var message in container.GetExportedValue<TaskLogger>().LogMessages)
                {
                    this.Log.LogMessage($"Bugfix Weaver: {message}");
                }


                try
                {
                    var cleanup = container.GetExportedValues<IDisposable>();
                    foreach (var disposable in cleanup)
                        disposable.Dispose();
                }
                catch (Exception)
                {

                }

                this.Log.LogMessage("Bugfix Code Weaver: finished");
            }
        }
        return true;
    }

}