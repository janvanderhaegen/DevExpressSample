using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Xml.Serialization;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Generic;


public class XtraReport_SkippIfFilled_Weaver : ICompilationStep
{
    ModuleReader moduleReader;
    private TaskLogger taskLogger;

    [ImportingConstructor]
    public XtraReport_SkippIfFilled_Weaver(ModuleReader moduleReader, TaskLogger taskLogger)
    {
        this.moduleReader = moduleReader;
        this.taskLogger = taskLogger;
    }


    public void Execute()
    {
        var xtraReportType = this.moduleReader.Module.Types.SingleOrDefault(t => t.Name == "XtraReport" && t.Namespace == "DevExpress.XtraReports.UI");
        if (xtraReportType == null)
        {
            throw new InvalidOperationException("Something went wrong while trying to weave in the bugfix: cannot find the type 'XtraReport'");
        }
        /*
             private bool SkipIfFilled()
            {
              IDrillDownServiceBase service1 = (IDrillDownServiceBase) ServiceProviderExtensions.GetService<IDrillDownServiceBase>((IServiceProvider) this);
              IInteractionService service2 = (IInteractionService) ServiceProviderExtensions.GetService<IInteractionService>((IServiceProvider) this);
              if (this.MasterReport == null)
              {
                if (XtraReport.IsInteracting((IInteractionServiceBase) service1, (IInteractionServiceBase) service2))
                  return true;
              } 
              //From here
              if (this.MasterReport != null)
                return this.MasterReport.CurrentRowIndex > 0;
              return false;              
              //To Here
            }
         */
        var skipIfFilledMethod = xtraReportType.Methods.SingleOrDefault(c => c.Name == "SkipIfFilled" && c.IsPrivate);
        if (skipIfFilledMethod == null)
        {
            throw new InvalidOperationException("Something went wrong while trying to weave in the bugfix: cannot find the method 'SkipIfFilled'");
        }
        var body = skipIfFilledMethod.Body.Instructions;
        /*
         //From here
         [22] call get_MasterReport()
         [23] brfalse.s IL0042
            - Transfers control to a target instruction if value is false, a null reference, or zero.
            - AKA: goTo IL0042 ([30])
         [24] ldarg.0   
            - Loads the argument at index 0 onto the evaluation stack.
            - AKA: 'this'
         [25] call get_MasterReport()
         [26] callVirt get_currentRowIndex()
         [27] ldc.i4.0
            - Push 0 onto the stack as int32. 
         [28] cgt
            - Push 1 (of type int32) if value1 greater that value2, else push 0. 
            - AKA 0 > this.getMasterReport().getCurrentRowIndex()
               then push 1 else 0
         [29] ret
            - return previous 0 or 1
         [30] "IL0042": ldc.i4.0
            - load 0
         [31] ret
            - return previous 0
         [32] ldc.i4.1
            - load 0
         [33] ret
            - return previous 1
         //To Here
         */


        //Find [26] callVirt get_currentRowIndex()
        var currentRowIndexInstruction = body.LastOrDefault(c => c.OpCode == OpCodes.Callvirt && c.Operand is MethodDefinition && ((MethodDefinition)c.Operand).Name == "get_CurrentRowIndex");
        if (currentRowIndexInstruction == null)
        {
            throw new InvalidOperationException("Something went wrong while trying to weave in the bugfix: cannot find the call to 'get_CurrentRowIndex'");
        }
        var start = currentRowIndexInstruction.Previous.Previous;
        if (start.Previous.OpCode != OpCodes.Brfalse_S)
        {
            throw new InvalidOperationException("Something went wrong while trying to weave in the bugfix: incorrect start.prev.prev.opcode");
        }
        var end = currentRowIndexInstruction.Next.Next.Next;
        if (end.Next.OpCode != OpCodes.Ldc_I4_0)
        {
            throw new InvalidOperationException("Something went wrong while trying to weave in the bugfix: incorrect end.next.opcode");
        }

        //Replace "return this.MasterReport.CurrentRowIndex > 0;" with hardcoded "return true";
        //AKA
        //Replace [24] -> [29] with hardcoded "return true"; 
        var ilProcessor = skipIfFilledMethod.Body.GetILProcessor();
        ilProcessor.InsertBefore(start, Instruction.Create(OpCodes.Ldc_I4_1));
        ilProcessor.InsertBefore(start, Instruction.Create(OpCodes.Ret));

        var instructionsToRemove = new List<Instruction>();
        instructionsToRemove.Add(start);
        var next = start;
        while (true)
        {
            next = next.Next;
            instructionsToRemove.Add(next);
            if (next == null)
            {
                throw new InvalidOperationException("Something went wrong while trying to weave in the bugfix: start.next[x] is null before reaching end");
            }
            if (next == end)
                break;
        }
        foreach (var instructionToRemove in instructionsToRemove)
        {
            ilProcessor.Remove(instructionToRemove);
        }
        this.taskLogger.LogMessages.Add($"Weaved the fix =)");
    }
}

