using System;
using PluginBase;
namespace OtherExtension
{
    public class OtherModule : ICommand
    {
        public string Name => "Other Module";

        public string Description => "Other module is example for PluginWhenInstall";

        public int Execute()
        {
            Console.WriteLine(
                "Other Text from OtherExtension loads into PluginWhenInstall.\n" +
                "It is really nice example like Adobe Exchange installs any packages\n" +
                "for DreamWeaver MX 6, Fireworks CS5 or PhotoShop CC 2020\n"
            );
            return 0;
        }
    }
}
