using System;
using System.Collections.Generic;
using System.Linq;
using StUtil.CodeGen;
using System.Windows.Forms;
namespace StUtil.Tests
{
    using StUtil.CodeGen.CodeObjects.Misc;
    using StUtil.CodeGen.CodeObjects.CodeStructures;
    using StUtil.CodeGen.CodeObjects.Generic;
    using StUtil.CodeGen.CodeObjects.Data;
    using StUtil.CodeGen.CodeObjects.Attributes;

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            StUtil.CodeGen.CSharp.CSharpCodeGenerator csharp = new CodeGen.CSharp.CSharpCodeGenerator();

            Namespace nsHelloWorld = new Namespace("StUtil.HelloWorld");
            Region regHelloWorld = new Region("Hello World");
            Class clsHelloWorld = new Class("HelloWorld")
            {
                AccessModifier = AccessModifiers.Public,
                ClassModifier = ClassModifiers.Abstract,
                GenericArguments = new CodeObjectList<GenericArgument>(", ", "T"),
                GenericConstraints = new CodeObjectList<GenericConstraint>(", ", new GenericConstraint("T", "class")),
            };
            clsHelloWorld.Inherits.Add(new CodeGen.CodeObjects.Data.TypeObject(typeof(IDisposable)));

            Region regPublicMethods = new Region("Public Methods");
            regPublicMethods.Events.Add(new Event("TestEvent", typeof(EventHandler<FormClosedEventArgs>), AccessModifiers.Public));
            Class clsHelloWorldInternal = new Class("HelloWorldUtilities")
            {
                ClassModifier = ClassModifiers.Static,
                AccessModifier = AccessModifiers.Private
            };
            clsHelloWorldInternal.Fields.Add(new Field("MESSAGE", typeof(string), AccessModifiers.Internal, new AttributeSection(new Attribute("Description", new DataObject("Debug"))))
            {
                Modifier = FieldModifiers.Constant,
                Value = new DataObject("Hello World")
            });

            Class clsHelloWorldRegioned = new Class("HelloWorldConstants")
            {
                ClassModifier = ClassModifiers.Static,
                AccessModifier = AccessModifiers.Private
            };
            clsHelloWorldRegioned.Methods.Add(new Method("Hi", typeof(void), AccessModifiers.Public));

            regPublicMethods.Classes.Add(clsHelloWorldRegioned);
            clsHelloWorld.Classes.Add(clsHelloWorldInternal);
            clsHelloWorld.Regions.Add(regPublicMethods);
            regHelloWorld.Classes.Add(clsHelloWorld);
            nsHelloWorld.Usings.Add(new Using("System"));
            nsHelloWorld.Usings.Add(new Using("System.ComponentModel"));
            nsHelloWorld.Regions.Add(regHelloWorld);

            string str = csharp.ToSyntax(nsHelloWorld);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
