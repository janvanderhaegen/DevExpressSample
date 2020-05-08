using System.ComponentModel.Composition;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Mdb;
using Mono.Cecil.Pdb;

[Export, PartCreationPolicy(CreationPolicy.Shared)]
public class ModuleWriter
{
    ModuleReader moduleReader;
    WeaveBugfixTask config;

    [ImportingConstructor]
    public ModuleWriter(ModuleReader moduleReader, WeaveBugfixTask config)
    {
        this.moduleReader = moduleReader;
        this.config = config;
    }

    public void Execute()
    {
        Execute(config.AssemblyLocation);
    }

    public void Execute(string targetPath)
    {
        var parameters = new WriterParameters
        {
            WriteSymbols = false
        };
        moduleReader.Module.Write(parameters);
    }
}