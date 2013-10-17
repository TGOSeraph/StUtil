using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using StUtil.Extensions;
using StUtil.Reflection;

namespace StUtil.Debugging.Testing
{
    public static class Tester
    {
        public static Dictionary<Type, List<TestResult>> Run(bool consoleLog = false)
        {
            Type[] types = Assembly.GetCallingAssembly().GetTypes();
            Dictionary<Type, List<TestResult>> results = new Dictionary<Type, List<TestResult>>();
            foreach (Type type in types)
            {
                if (type.IsClass && type.GetCustomAttributes(typeof(TestClassAttribute), false).Length > 0)
                {
                    results.Add(type, Run(type, consoleLog));
                }
            }
            return results;
        }

        public static List<TestResult> Run<T>(bool consoleLog = false)
        {
            return Run(typeof(T), consoleLog);
        }

        public static List<TestResult> Run(Type type, bool consoleLog = false)
        {
            List<TestResult> output = new List<TestResult>();

            if (consoleLog)
            {
                Console.WriteLine("Evaluating Class: " + type.Name);
            }

            Stopwatch sw = new Stopwatch();
            var methods = new StUtil.Reflection.ReflectionHelper(type).GetMethods();
            foreach (var test in methods.ToDictionary(m => m, m =>
            {
                return (TestMethodAttribute)m
                    .Member
                    .GetCustomAttributes(typeof(TestMethodAttribute), false)
                    .FirstOrDefault();
            })
            .Where(k => k.Value != null))
            {
                if (consoleLog)
                {
                    Console.WriteLine("Testing Method: " + test.Key.Member.Name);
                }

                sw.Reset();
                object res = null;
                int lastPerc = 0;
                int posL = Console.CursorLeft;
                int posT = Console.CursorTop;
                ReflectedMethod validator = null;

                if (!string.IsNullOrEmpty(test.Value.ValidationFunction))
                {
                    validator = methods.First(m => m.Member.Name == test.Value.ValidationFunction);
                }

                for (int i = 0; i < test.Value.Iterations; i++)
                {
                    if (consoleLog)
                    {
                        int perc = (int)(((double)i / test.Value.Iterations) * 100);

                        Console.SetCursorPosition(posL, posT);
                        if (perc > lastPerc)
                        {
                            Console.WriteLine(i.ToString() + "/" + test.Value.Iterations.ToString() + " - " + perc + "%");
                            lastPerc = perc;
                        }
                    }

                    sw.Start();
                    res = test.Key.Invoke(null);
                    sw.Stop();
                }

                output.Add(new TestResult
                {
                    ActualResult = res,
                    ExpectedResult = test.Value.ExpectedValue,
                    Method = test.Key.Member,
                    TimeTaken = TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds / test.Value.Iterations),
                    Iterations = test.Value.Iterations,
                    Pass = (validator == null
                        ? res.GetHashCode() == test.Value.ExpectedValue.GetHashCode()
                        : (bool)(validator.Parameters.Count() == 1
                            ? validator.Invoke(null, new object[] { res })
                            : validator.Invoke(null, new object[] { test.Key.Member.Name, res }))),
                    ValidationFunction = test.Value.ValidationFunction
                });
            }

            return output;
        }
    }
}
