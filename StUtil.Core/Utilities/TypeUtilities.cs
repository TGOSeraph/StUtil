using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;

namespace StUtil.Utilities
{
    public static class TypeUtilities
    {
        /// <summary>
        /// Converts the object to the specified type.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="t">The type.</param>
        /// <returns></returns>
        public static object ConvertType(object obj, Type t)
        {
            if (t.IsEnum)
            {
                int i = -1;
                if (int.TryParse(obj.ToString(), out i))
                {
                    return Enum.ToObject(t, i);
                }
                else
                {
                    return Enum.Parse(t, obj.ToString());
                }
            }
            else if (t == typeof(Color))
            {
                return Color.FromName(obj.ToString());
            }
            return Convert.ChangeType(obj, t);
        }

        /// <summary>
        /// Converts the object to the specified type.
        /// </summary>
        /// <typeparam name="U">The type to convert to.</typeparam>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static U ConvertType<U>(object obj)
        {
            return (U)ConvertType(obj, typeof(U));
        }

        /// <summary>
        /// Gets a type from a type name
        /// </summary>
        /// <param name="typeName">The name of the type to get</param>
        /// <returns>The type from the type name</returns>
        public static Type GetTypeFromSimpleName(string typeName)
        {
            if (typeName == null)
                throw new ArgumentNullException("typeName");

            bool isArray = false, isNullable = false;

            if (typeName.IndexOf("[]") != -1)
            {
                isArray = true;
                typeName = typeName.Remove(typeName.IndexOf("[]"), 2);
            }

            if (typeName.IndexOf("?") != -1)
            {
                isNullable = true;
                typeName = typeName.Remove(typeName.IndexOf("?"), 1);
            }

            typeName = typeName.ToLower();

            string parsedTypeName = null;
            switch (typeName)
            {
                case "bool":
                case "boolean":
                    parsedTypeName = "System.Boolean";
                    break;

                case "byte":
                    parsedTypeName = "System.Byte";
                    break;

                case "char":
                    parsedTypeName = "System.Char";
                    break;

                case "datetime":
                    parsedTypeName = "System.DateTime";
                    break;

                case "datetimeoffset":
                    parsedTypeName = "System.DateTimeOffset";
                    break;

                case "decimal":
                    parsedTypeName = "System.Decimal";
                    break;

                case "double":
                    parsedTypeName = "System.Double";
                    break;

                case "float":
                    parsedTypeName = "System.Single";
                    break;

                case "int16":
                case "short":
                    parsedTypeName = "System.Int16";
                    break;

                case "int32":
                case "int":
                    parsedTypeName = "System.Int32";
                    break;

                case "int64":
                case "long":
                    parsedTypeName = "System.Int64";
                    break;

                case "object":
                    parsedTypeName = "System.Object";
                    break;

                case "sbyte":
                    parsedTypeName = "System.SByte";
                    break;

                case "string":
                    parsedTypeName = "System.String";
                    break;

                case "timespan":
                    parsedTypeName = "System.TimeSpan";
                    break;

                case "uint16":
                case "ushort":
                    parsedTypeName = "System.UInt16";
                    break;

                case "uint32":
                case "uint":
                    parsedTypeName = "System.UInt32";
                    break;

                case "uint64":
                case "ulong":
                    parsedTypeName = "System.UInt64";
                    break;

                case "intptr":
                    parsedTypeName = "System.IntPtr";
                    break;
            }

            if (parsedTypeName != null)
            {
                if (isArray)
                    parsedTypeName = parsedTypeName + "[]";

                if (isNullable)
                    parsedTypeName = String.Concat("System.Nullable`1[", parsedTypeName, "]");
            }
            else
                parsedTypeName = typeName;

            // Expected to throw an exception in case the type has not been recognized.
            return Type.GetType(parsedTypeName);
        }

        /// <summary>
        /// Gets a type from a type name
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns></returns>
        public static Type[] LoadType(string typeName)
        {
            return LoadType(typeName, true);
        }

        /// <summary>
        /// Gets a type from a type name
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="referenced">if set to <c>true</c> then check referenced assemblies.</param>
        /// <returns></returns>
        public static Type[] LoadType(string typeName, bool referenced)
        {
            return LoadType(typeName, referenced, false);
        }

        /// <summary>
        /// Gets a type from a type name
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="referenced">if set to <c>true</c> then check referenced assemblies.</param>
        /// <param name="gac">if set to <c>true</c> then check the GAC.</param>
        /// <returns></returns>
        public static Type[] LoadType(string typeName, bool referenced, bool gac)
        {
            //check for problematic work
            if (string.IsNullOrEmpty(typeName) || !referenced && !gac)
                return new Type[] { };

            Assembly currentAssembly = Assembly.GetExecutingAssembly();

            List<string> assemblyFullnames = new List<string>();
            List<Type> types = new List<Type>();

            if (referenced)
            {            //Check refrenced assemblies
                foreach (AssemblyName assemblyName in currentAssembly.GetReferencedAssemblies())
                {
                    //Load method resolve refrenced loaded assembly
                    Assembly assembly = Assembly.Load(assemblyName.FullName);

                    //Check if type is exists in assembly
                    var type = assembly.GetType(typeName, false, true);

                    if (type != null && !assemblyFullnames.Contains(assembly.FullName))
                    {
                        types.Add(type);
                        assemblyFullnames.Add(assembly.FullName);
                    }
                }
            }

            if (gac)
            {
                //GAC files
                string gacPath = Environment.GetFolderPath(System.Environment.SpecialFolder.Windows) + "\\assembly";
                var files = GetGlobalAssemblyCacheFiles(gacPath);
                foreach (string file in files)
                {
                    try
                    {
                        //reflection only
                        Assembly assembly = Assembly.ReflectionOnlyLoadFrom(file);

                        //Check if type is exists in assembly
                        var type = assembly.GetType(typeName, false, true);

                        if (type != null && !assemblyFullnames.Contains(assembly.FullName))
                        {
                            types.Add(type);
                            assemblyFullnames.Add(assembly.FullName);
                        }
                    }
                    catch
                    {
                    }
                }
            }

            return types.ToArray();
        }

        /// <summary>
        /// Gets the global assembly cache files.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private static string[] GetGlobalAssemblyCacheFiles(string path)
        {
            List<string> files = new List<string>();

            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);

            foreach (System.IO.FileInfo fi in di.GetFiles("*.dll"))
            {
                files.Add(fi.FullName);
            }

            foreach (System.IO.DirectoryInfo diChild in di.GetDirectories())
            {
                var files2 = GetGlobalAssemblyCacheFiles(diChild.FullName);
                files.AddRange(files2);
            }

            return files.ToArray();
        }
    }
}