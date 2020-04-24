using PluginBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PluginWhenInstall
{
    class MainClass
    {
        /*
         *  Search plugin *dll assemblies in Plugin directory
         */
        static string[] pluginPaths
        {
            get
            {
                string root = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Extensions"));
                List<string> asmpaths = new List<string>();

                foreach (string asmpath in Directory.EnumerateFiles(root, "*.dll"))
                {
                    if (File.Exists(asmpath))
                    {
                        asmpaths.Add(asmpath);
                    }
                }

                return asmpaths.ToArray();
            }
        }

        static Assembly asm;

        static void Main(string[] args)
        {
            try
            {
                string root = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Extensions"));
                string extensionDiretcory = Path.GetFullPath(Path.Combine(root.Replace('/', Path.DirectorySeparatorChar)));
                if (!File.Exists(extensionDiretcory))
                {
                    Console.WriteLine($"Our extensions are installeds. {extensionDiretcory}");
                    IEnumerable<ICommand> commands = pluginPaths.SelectMany(pluginPath =>
                    {
                        Assembly pluginAssembly = LoadPlugin(pluginPath);
                        return CreateCommands(pluginAssembly);
                    }).ToList();

                    foreach (ICommand command in commands)
                    {
                        Console.WriteLine($"{command.Name}\t - {command.Description}");
                        foreach (Type type in asm.GetExportedTypes())
                        {
                            var o = Activator.CreateInstance(type);
                            o = command.Execute();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static Assembly LoadPlugin(string relativePath)
        {
            // Navigate up to the solution root
            string root = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Extensions"));

            string pluginLocation = Path.GetFullPath(Path.Combine(root, relativePath.Replace('/', Path.DirectorySeparatorChar)));
            Console.WriteLine($"Loading commands from: {pluginLocation}");
            /*
             *  Eh it works fine for old version :-D
             */
            asm = Assembly.LoadFile(pluginLocation);
            return asm = Assembly.Load(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
        }

        static IEnumerable<ICommand> CreateCommands(Assembly assembly)
        {
            int count = 0;

            foreach (Type type in assembly.GetTypes())
            {
                if (typeof(ICommand).IsAssignableFrom(type))
                {
                    ICommand result = Activator.CreateInstance(type) as ICommand;
                    if (result != null)
                    {
                        count++;
                        yield return result;
                    }
                }
            }
        }
    }
}
