using Sofos2ToDatawarehouse.Extractor.CancelChargeAmount.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Extractor.CancelChargeAmount
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
                    System.Console.WriteLine("The cancel charge amount extractor will now begin.");
                    CancelChargeAmountController controller = new CancelChargeAmountController();
                    controller.ProcessExtraction();

                    //Console.WriteLine("\nAll files have been extracted successfully.");
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
