using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace StUtil.CodeGen.CSharp
{
    public class CSharpCodeExecutor : IDotNetCodeExecutor
    {
        public object ExecuteCode(string code, string @namespace, string @class, string function, bool isStatic, string[] referencedAssemblies, object[] args)
        {
            object returnval = null;
            Assembly asm = BuildAssembly(code, referencedAssemblies);
            object instance = null;
            Type type = null;
            if (isStatic)
            {
                type = asm.GetType(@namespace + "." + @class);
            }
            else
            {
                instance = asm.CreateInstance(@namespace + "." + @class);
                type = instance.GetType();
            }
            MethodInfo method = type.GetMethod(function);
            returnval = method.Invoke(instance, args ?? new object[] { });
            return returnval;
        }

        private static Assembly BuildAssembly(string code, string[] referencedAssemblies)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();

            CSharpCodeProvider compiler = (CSharpCodeProvider)CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters compilerparams = new CompilerParameters();
            compilerparams.GenerateExecutable = false;
            compilerparams.GenerateInMemory = true;
            if (referencedAssemblies != null)
            {
                compilerparams.ReferencedAssemblies.AddRange(referencedAssemblies);
            }
            CompilerResults results = compiler.CompileAssemblyFromSource(compilerparams, code);
            if (results.Errors.HasErrors)
            {
                StringBuilder errors = new StringBuilder("Compiler Errors :" + Environment.NewLine);
                foreach (CompilerError error in results.Errors)
                {
                    errors.AppendFormat("Line {0},{1}\t: {2}" + Environment.NewLine, error.Line, error.Column, error.ErrorText);
                }
                throw new Exception(errors.ToString());
            }
            else
            {
                return results.CompiledAssembly;
            }
        }
    }
}
