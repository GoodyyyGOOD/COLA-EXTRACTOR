using Sofos2.Sender.Inventory.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sofos2.Sender.Inventory
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1)
            {
                Console.WriteLine("The same program is already running in progress, please close this window ASAP.");
                System.Environment.Exit(1);
            }
            else
            {
                try
                {
                    InventoryController controller = new InventoryController();

                    System.Console.WriteLine("The inventory sender will now begin.");
                    await controller.SendingItemsToAPIAsync();

                    Console.WriteLine("\nAll files have been processed.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Thread.Sleep(3000);
            }
        }
    }
}
