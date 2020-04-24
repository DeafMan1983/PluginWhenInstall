using System;
using PluginBase;
namespace HelloExtension
{
    public class HelloModule : ICommand
    {
        public string Name { get => "Hello Module"; }

        public string Description { get => "Hello World is automacally installed on PluginWhenInstall."; }

        public int Execute()
        {
            Console.WriteLine(
                "Hello World - It is loading from PluginWhenInstall.\n" +
                "Thanks!\n"
            );
            return 0;
        }
    }
}
