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
    /// <summary>
    /// Class used to iterate over each class in the assembly and running any tests within
    /// </summary>
    /// <remarks>
    /// 2013-10-18  - Initial version
    /// </remarks>
    public static class Tester
    {
        /// <summary>
        /// Run all tests in the calling assembly
        /// </summary>
        /// <param name="consoleLog">If information should be output to console</param>
        /// <returns>A list of test results for each type tested</returns>
        public static Dictionary<Type, List<TestResult>> Run(bool consoleLog = false)
        {
            //Get all the types in the assembly
            Type[] types = Assembly.GetCallingAssembly().GetTypes();
            Dictionary<Type, List<TestResult>> results = new Dictionary<Type, List<TestResult>>();
            foreach (Type type in types)
            {
                //If we are dealing with a class that has the test attribute
                if (type.IsClass && type.GetCustomAttributes(typeof(TestClassAttribute), false).Length > 0)
                {
                    //Run the test and store the result
                    results.Add(type, Run(type, consoleLog));
                }
            }
            return results;
        }

        /// <summary>
        /// Run tests on a specific class
        /// </summary>
        /// <typeparam name="T">The type of the class containing the tests to run</typeparam>
        /// <param name="consoleLog">If information should be output to console</param>
        /// <returns>The test results</returns>
        public static List<TestResult> Run<T>(bool consoleLog = false) where T : class
        {
            return Run(typeof(T), consoleLog);
        }

        /// <summary>
        /// Run tests on a specific class
        /// </summary>
        /// <param name="type">The type of the class containing the tests to run</param>
        /// <param name="consoleLog">If information should be output to console</param>
        /// <returns>The test results</returns>
        public static List<TestResult> Run(Type type, bool consoleLog = false)
        {
            if (type.IsClass)
            {
                throw new ArgumentException("Must be of type class", "type");
            }
            List<TestResult> output = new List<TestResult>();

            if (consoleLog)
            {
                Console.WriteLine("Evaluating Class: " + type.Name);
            }

            Stopwatch sw = new Stopwatch();

            //Get all methods from the class
            var methods = new StUtil.Reflection.ReflectionHelper(type).GetMethods();

            //Iterate over each method that has the test method attribute
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

                //Check if we have a result validation function
                if (!string.IsNullOrEmpty(test.Value.ValidationFunction))
                {
                    //If so, select it from the methods
                    validator = methods.First(m => m.Member.Name == test.Value.ValidationFunction);
                }

                //Run the test the specific number of times
                for (int i = 0; i < test.Value.Iterations; i++)
                {
                    if (consoleLog)
                    {
                        int perc = (int)(((double)i / test.Value.Iterations) * 100);
                        //If logging, update console
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

                //Else we need to update the output with the results of the test
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
