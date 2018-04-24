using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class KiiTestCollector
    {
        public KiiTestCollector ()
        {
        }

        [Test(), KiiUTInfo(
            action = "When we call this method,",
            expected = "alltest.html file will be created!"
            )]
        public void Collect()
        {
            StreamWriter sw = new StreamWriter("alltest.html");

            sw.WriteLine("<!DOCTYPE html>");
            sw.WriteLine("<html>");
            sw.WriteLine("<head>");

            sw.WriteLine("<style type=\"text/css\" rel=\"stylesheet\">");
            sw.WriteLine(".classname { background-color:#FFcc33; }");
            sw.WriteLine("</style>");

            sw.WriteLine("</head>");
            sw.WriteLine("<body>");
            sw.WriteLine("<table>");

            int allTests = 0;
            int testWithInfo = 0;
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetName().Name.StartsWith("System") || assembly.GetName().Name.StartsWith("Mono"))
                {
                    continue;
                }
                foreach (Type type in assembly.GetExportedTypes())
                {
                    if (type.GetCustomAttributes(
                        typeof(NUnit.Framework.TestFixtureAttribute), true).Length == 0)
                    {
                        continue;
                    }

                    sw.WriteLine("<tr><th class=\"classname\" colspan=\"3\">" + type.Name + "</th></tr>");

                    foreach (MethodInfo method in type.GetMethods())
                    {
                        if (method.GetCustomAttributes(
                            typeof(NUnit.Framework.TestAttribute), true).Length == 0)
                        {
                            // this method is not test method
                            continue;
                        }
                        ++allTests;
                        sw.Write("<tr><th>" + method.Name + "</th>");


                        KiiUTInfoAttribute[] attributes = (KiiUTInfoAttribute[]) method.GetCustomAttributes(typeof(KiiUTInfoAttribute), true);
                        if (attributes.Length == 0)
                        {
                            // we should print warning
                            sw.WriteLine("<td>{not set}</td><td>{not set}</td></tr>");
                            continue;
                        }
                        ++testWithInfo;
                        sw.WriteLine("<td>" + attributes[0].action + "</td><td>" + attributes[0].expected + "</td></tr>");
                        Console.Write(method.Name + " ");
                        Console.WriteLine(attributes[0].action + " / " + attributes[0].expected);
                    }
                }
            }
            sw.WriteLine("</table>");
            sw.WriteLine("</body>");
            sw.WriteLine("</html>");
            sw.Close();

            Console.WriteLine("{0}/{1}", testWithInfo, allTests);
        }
    }
}

