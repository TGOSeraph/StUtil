using StUtil.CodeGen.CodeObjects;
using StUtil.CodeGen.CodeObjects.CodeStructures;
using StUtil.CodeGen.CodeObjects.Generic;
using StUtil.CodeGen.CodeObjects.Misc;
using StUtil.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen
{
    public abstract class BaseDotNetCodeGenerator : IDotNetCodeGenerator
    {
        public IDotNetCodeFormatter CodeFormatter { get; set; }
        public delegate string CodeConversionDelegate<T>(T obj) where T : ICodeObject;
        public delegate string ObjectConversionDelegate<T>(T obj, object owner);

        protected static Dictionary<Type, TypeDefinition> definitions = new Dictionary<Type, TypeDefinition>();
        protected static Dictionary<Type, Delegate> objConverters = new Dictionary<Type, Delegate>();

        public BaseDotNetCodeGenerator()
        {
            RegisterCodeConverter<ICodeObjectList>(ToSyntaxFromListObject);
            RegisterCodeConverter<ICodeObjectDictionary>(ToSyntaxFromDictionary);
            RegisterObjectConverter<AccessModifiers>(ConvertFromAccessModifiers);
            RegisterObjectConverter<ClassModifiers>(ConvertFromClassModifiers);
            RegisterObjectConverter<FieldModifiers>(ConvertFromFieldModifiers);
            RegisterObjectConverter<EventModifiers>(ConvertFromEventModifiers);
            RegisterObjectConverter<MethodModifiers>(ConvertFromMethodModifiers);
            RegisterObjectConverter<string>(ConvertFromString);
            RegisterObjectConverter<Type>(CovertFromType);
        }

        protected TypeDefinition GetOrAddTypeDef(Type t)
        {
            TypeDefinition def;
            if (definitions.ContainsKey(t))
            {
                def = definitions[t];
            }
            else
            {
                def = new TypeDefinition();
                definitions.Add(t, def);
            }
            return def;
        }
        protected TypeDefinition GetOrAddTypeDef<T>()
        {
            return GetOrAddTypeDef(typeof(T));
        }

        public void RegisterDefinition<T>(string definition) where T : ICodeObject
        {
            TypeDefinition def = GetOrAddTypeDef<T>();
            def.Definition = definition;
            def.Matches = Parse(definition);
        }
        public void RegisterCodeConverter<T>(CodeConversionDelegate<T> converter) where T : ICodeObject
        {
            GetOrAddTypeDef<T>().Converter = converter;
        }
        public void RegisterObjectConverter<T>(ObjectConversionDelegate<T> converter)
        {
            objConverters.Add(typeof(T), converter);
        }

        protected static List<KeyMatch> Parse(string format)
        {
            bool inKey = false;
            string currKey = "";
            List<KeyMatch> keys = new List<KeyMatch>();
            char lastChar = '\0';
            for (int i = 0; i < format.Length; i++)
            {
                char c = format[i];
                switch (c)
                {
                    case '{':
                        if (inKey)
                        {
                            currKey += c;
                        }
                        else
                        {
                            if (lastChar != '\\')
                            {
                                inKey = true;
                            }
                        }
                        break;
                    case '}':
                        if (inKey)
                        {
                            if (lastChar == '\\')
                            {
                                currKey += c;
                            }
                            else
                            {
                                inKey = false;
                                keys.Add(new KeyMatch
                                {
                                    Index = i - currKey.Length - 1,
                                    Text = currKey
                                });
                                currKey = "";
                            }
                        }
                        break;
                    default:
                        if (inKey)
                        {
                            currKey += c;
                        }
                        break;
                }
                lastChar = c;
            }
            return keys;
        }

        protected ReflectionHelper GetReflector(Type t)
        {
            TypeDefinition def = GetOrAddTypeDef(t);
            if (def.Reflector == null)
            {
                def.Reflector = new ReflectionHelper(t);
            }
            return def.Reflector;
        }
        protected ReflectionHelper GetReflector<T>()
        {
            return GetReflector(typeof(T));
        }

        protected virtual string ObjectToString(object obj, object owner)
        {
            if (obj == null)
            {
                return "";
            }

            if (obj is ICodeObject)
            {
                return ToSyntaxInternal((ICodeObject)obj);
            }
            else
            {
                if (obj.GetType().FullName == "System.RuntimeType" && objConverters.ContainsKey(typeof(Type)))
                {
                    return (string)objConverters[typeof(Type)].DynamicInvoke(obj, owner);
                }
                else if (objConverters.ContainsKey(obj.GetType()))
                {
                    return (string)objConverters[obj.GetType()].DynamicInvoke(obj, owner);
                }
                return obj == null ? "" : obj.ToString();
            }
        }

        public string ToSyntax(ICodeObject obj)
        {
            if (CodeFormatter != null)
            {
                return CodeFormatter.Format(ToSyntaxInternal(obj));
            }
            return (ToSyntaxInternal(obj));
        }

        protected string ToSyntaxInternal(ICodeObject obj)
        {
            Type t = obj.GetType();

            //See if we have a definition for this type
            if (definitions.ContainsKey(t) && definitions[t].Definition != null)
            {
                return ToSyntaxFromDefinition(obj, definitions[t].Definition, definitions[t].Matches);
            }
            else
            {
                //IF not, see if we have a converter that can convert from a base type
                foreach (Type convT in definitions.Keys)
                {
                    if (convT.IsAssignableFrom(t) && definitions[convT].Converter != null)
                    {
                        return (string)definitions[convT].Converter.DynamicInvoke(obj);
                    }
                }

                //Else see if we can convert using a single property
                ReflectionHelper helper = GetReflector(t);
                IEnumerable<ReflectedProperty> props = helper.GetProperties();
                if (props.Count() == 1)
                {
                    return ObjectToString(props.First().Get(obj), obj);
                }
            }

            //No syntax generator found
            throw new KeyNotFoundException(t.FullName);
        }

        protected virtual string ToSyntaxFromListObject(ICodeObjectList obj)
        {
            string output = "";
            foreach (object item in obj.Items)
            {
                output += ObjectToString(item, obj) + obj.JoinString;
            }
            return output.Length > 0 ? output.Substring(0, output.Length - obj.JoinString.Length) : output;
        }

        public string ToSyntaxFromDictionary(ICodeObjectDictionary obj)
        {
            string output = "";
            foreach (dynamic item in obj.Items)
            {
                output += ObjectToString(item.Key, obj) + obj.SeparatorString + ObjectToString(item.Value, obj) + obj.JoinString;
            }
            return output.Length > 0 ? output.Substring(0, output.Length - obj.JoinString.Length) : output;
        }


        protected virtual string ToSyntaxFromDefinition(ICodeObject obj, string definition, List<KeyMatch> matches)
        {
            ReflectionHelper reflector = GetReflector(obj.GetType());
            int offset = 0;
            string last = null;
            for (int i = 0; i < matches.Count; i++)
            {
                KeyMatch match = matches[i];
                HandleMatch(match, matches, obj, ref i, reflector, ref last, ref offset, ref definition);
            }

            return definition.Replace("\\{", "{").Replace("\\}", "}");
        }

        protected virtual void HandleMatch(KeyMatch match, List<KeyMatch> matches, object obj, ref int index, ReflectionHelper reflector, ref string last, ref int offset, ref string definition)
        {
            switch (match.Text[0])
            {
                case '-':
                    if (string.IsNullOrEmpty(last))
                    {
                        last = UpdateDefinition(match, ref definition, "", ref offset);
                    }
                    else
                    {
                        last = UpdateDefinition(match, ref definition, GetPropertyOrValue(reflector, match.Text.Substring(1), obj), ref offset);
                    }
                    break;
                case '+':
                    KeyMatch nextMatch = matches[index + 1];
                    string last2 = last;
                    string def2 = definition;
                    int offset2 = offset;
                    int index2 = index;
                    HandleMatch(nextMatch, matches, obj, ref index2, reflector, ref last2, ref offset2, ref def2);
                    if (string.IsNullOrEmpty(last2))
                    {
                        last = UpdateDefinition(match, ref definition, "", ref offset);
                    }
                    else
                    {
                        last = UpdateDefinition(match, ref definition, GetPropertyOrValue(reflector, match.Text.Substring(1), obj), ref offset);
                    }
                    break;
                case '#':
                    last = UpdateDefinition(match, ref definition, GetPropertyOrValue(reflector, match.Text.Substring(1), obj), ref offset);
                    break;
                default:
                    last = UpdateDefinition(match, ref definition, GetPropertyOrValue(reflector, match.Text, obj), ref offset);
                    break;
            }
        }
        protected virtual string GetPropertyOrValue(ReflectionHelper reflector, string match, object obj)
        {
            ReflectedProperty prop = reflector.GetProperty(match);
            if (prop == null)
            {
                return match;
            }
            else
            {
                return ObjectToString(prop.Get(obj), obj);
            }
        }
        private static string UpdateDefinition(KeyMatch match, ref string definition, string value, ref int offset)
        {
            definition = definition.Substring(0, match.Index - offset)
                + value
                + (match.Index + match.Length - offset > definition.Length
                    ? ""
                    : definition.Substring(match.Index + match.Length - offset));
            offset += match.Length - value.Length;
            return value;
        }


        protected abstract string ConvertFromAccessModifiers(AccessModifiers obj, object owner);
        protected abstract string ConvertFromClassModifiers(ClassModifiers obj, object owner);
        protected abstract string ConvertFromFieldModifiers(FieldModifiers obj, object owner);
        protected abstract string ConvertFromEventModifiers(EventModifiers obj, object owner);
        protected abstract string ConvertFromMethodModifiers(MethodModifiers obj, object owner);

        protected string ConvertFromString(string obj, object owner)
        {
            if (owner.GetType().GetCustomAttributes(typeof(KeywordAttribute), true).Length > 0)
            {
                return obj;
            }
            else
            {
                return "\"" + obj + "\"";
            }
        }

        private string CovertFromType(Type obj, object owner)
        {
            if (obj == typeof(void))
            {
                return "void";
            }
            if (obj.GetGenericArguments().Length > 0)
            {
                return ConvertFromType(obj, obj.GetGenericArguments(), owner);
            }
            else
            {
                return obj.FullName;
            }
        }

        protected abstract string ConvertFromType(Type obj, Type[] type, object owner);

    }
}
