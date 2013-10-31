using StUtil.CodeGen.CodeObjects;
using StUtil.CodeGen.CodeObjects.CodeStructures;
using StUtil.CodeGen.CodeObjects.Attributes;
using StUtil.CodeGen.CodeObjects.Generic;
using StUtil.CodeGen.CodeObjects.Misc;
using StUtil.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen.CSharp
{
    /// <summary>
    /// Class used to convert code objects to C# syntax
    /// </summary>
    public class CSharpCodeGenerator : BaseDotNetCodeGenerator
    {
        public CSharpCodeGenerator()
        {
            base.CodeFormatter = new CSharpCodeFormatter();
            string memberContainer = "{Regions}{-\n\n}{Classes}{-\n\n}{Events}{-\n\n}{Fields}{-\n\n}{Properties}{-\n\n}{Methods}{-\n}";
            string attributeAccessModModifier = "{Attributes}{-\n}{AccessModifier}{- }{Modifier}{- }";

            RegisterDefinition<AttributeSection>("[{Attributes}]");
            RegisterDefinition<StUtil.CodeGen.CodeObjects.Attributes.Attribute>("{Name}({Parameters}{+, }{NamedParameters})");
            RegisterDefinition<GenericConstraint>("{GenericArgument} : {TypeConstraints}");

            RegisterDefinition<Namespace>("{Usings}{-\n\n}namespace {Name}\n\\{\n" + memberContainer + "\\}");
            RegisterDefinition<Using>("using {Value};");
            RegisterDefinition<Region>("#region {Name}\n" + memberContainer + "#endregion");

            RegisterDefinition<Class>(attributeAccessModModifier + "class {Name}{+<}{GenericArguments}{->}{+ : }{Inherits}{+ where }{GenericConstraints}\n\\{\n" + memberContainer + "\\}");
            RegisterDefinition<Field>(attributeAccessModModifier + "{ReturnType} {Name}{+ = }{Value};");
            RegisterDefinition<Event>(attributeAccessModModifier + "event {HandlerType} {Name};");
            RegisterDefinition<Method>(attributeAccessModModifier + "{Implements}{- }{ReturnType} {Name}{+<}{GenericArguments}{->}({Parameters}){+ where }{GenericConstraints}\n\\{\n{Code}{-\n}\\}");
            RegisterDefinition<Property>(attributeAccessModModifier + "{ReturnType} {Name}\\{\n{Getter}\n{Setter}\n\\}");
        }

        protected override string ConvertFromType(Type obj, Type[] type, object owner)
        {
            return obj.GetGenericTypeDefinition().FullName.Substring(0, obj.GetGenericTypeDefinition().FullName.LastIndexOf('`'))
                        + "<" + string.Join(",", type.Select(a => ObjectToString(a, obj))) + ">";
        }

        protected override string ConvertFromAccessModifiers(AccessModifiers obj, object owner)
        {
            switch (obj)
            {
                case AccessModifiers.Internal:
                    return "internal";
                case AccessModifiers.Private:
                    return "private";
                case AccessModifiers.Protected:
                    return "protected";
                case AccessModifiers.ProtectedInternal:
                    return "protected internal";
                case AccessModifiers.Public:
                    return "public";
            }
            return "";
        }

        protected override string ConvertFromClassModifiers(ClassModifiers obj, object owner)
        {
            switch (obj)
            {
                case ClassModifiers.Abstract:
                    return "abstract";
                case ClassModifiers.Sealed:
                    return "sealed";
                case ClassModifiers.Static:
                    return "static";
            }
            return "";
        }

        protected override string ConvertFromFieldModifiers(FieldModifiers obj, object owner)
        {
            switch (obj)
            {
                case FieldModifiers.Constant:
                    return "const";
                case FieldModifiers.Readonly:
                    return "readonly";
                case FieldModifiers.Static:
                    return "static";
                case FieldModifiers.StaticReadonly:
                    return "static readonly";
                case FieldModifiers.Volatile:
                    return "volatile";
                case FieldModifiers.StaticVolatile:
                    return "static volatile";
            }
            return "";
        }

        protected override string ConvertFromEventModifiers(EventModifiers obj, object owner)
        {
            switch (obj)
            {
                case EventModifiers.Static:
                    return "static";
            }
            return "";
        }

        protected override string ConvertFromMethodModifiers(MethodModifiers obj, object owner)
        {
            switch (obj)
            {
                case MethodModifiers.Abstract:
                    return "abstract";
                case MethodModifiers.New:
                    return "new";
                case MethodModifiers.Override:
                    return "override";
                case MethodModifiers.Static:
                    return "static";
                case MethodModifiers.Virtual:
                    return "virtual";
            }
            return "";
        }
    }
}
