
using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Serialization;
using Mono.Cecil;
using Mono.Cecil.Cil;

[Export, PartCreationPolicy(CreationPolicy.Shared)]
public class FileChangedChecker
{
    ModuleReader moduleReader;
    const string key = "Bugfix.WeaveCompletedAttribute";
    private TaskLogger logger;

    [ImportingConstructor]
    public FileChangedChecker(WeaveBugfixTask config, ModuleReader moduleReader, TaskLogger logger)
    {
        this.moduleReader = moduleReader;
        this.logger = logger;
    }

    public bool ShouldStart()
    {
        if (!moduleReader.Module.Types.Any(x => x.FullName == key))
        {
            var attributeType = this.moduleReader.Module.ImportReference(typeof(Attribute));

            var weaveCompletedAttribute = new TypeDefinition("Bugfix", "WeaveCompletedAttribute", TypeAttributes.NotPublic, attributeType);

            var ctor = new MethodDefinition(".ctor", MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, this.moduleReader.Module.ImportReference(typeof(void)));

            var cil = ctor.Body.GetILProcessor();
            cil.Emit(OpCodes.Ldarg_0);
            cil.Emit(OpCodes.Call, this.moduleReader.Module.ImportReference(typeof(Attribute).GetConstructor(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, null, Type.EmptyTypes, null)));
            cil.Emit(OpCodes.Ret);
            var t = ctor.IsConstructor;

            weaveCompletedAttribute.Methods.Add(ctor);
            moduleReader.Module.Types.Add(weaveCompletedAttribute);
            moduleReader.Module.CustomAttributes.Add(new CustomAttribute(ctor));
            return true;
        }
        else
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.logger.LogMessages.Add("This assembly has not changed since last weave. Weaving anyway ");
                return true;
            }
            this.logger.LogMessages.Add("This assembly has not changed since last weave. Skipping");
            return false;
        }
    }
}

