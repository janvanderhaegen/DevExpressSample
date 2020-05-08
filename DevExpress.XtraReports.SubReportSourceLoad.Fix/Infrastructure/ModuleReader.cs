using System;
using System.ComponentModel.Composition;
using System.IO;
using Mono.Cecil;

[Export, PartCreationPolicy(CreationPolicy.Shared)]
[Export(typeof(IDisposable))]
public class ModuleReader : IDisposable
{
    WeaveBugfixTask config;
    public ModuleDefinition Module { get; set; }

    [ImportingConstructor]
    public ModuleReader(
            WeaveBugfixTask config)
    {
        this.config = config;
    }

    public void Execute()
    {
        var readerParameters = new ReaderParameters
        {
            ReadSymbols = false,
            ReadWrite = true,
        };
        Module = ModuleDefinition.ReadModule(config.AssemblyLocation, readerParameters);
    }

    public void Dispose()
    {
        if (this.Module != null)
            this.Module.Dispose();
    }
}