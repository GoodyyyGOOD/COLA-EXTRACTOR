﻿using Sofos2ToDatawarehouse.Extractor.Accounting.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Extractor.Accounting
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
                    System.Console.WriteLine("The accounting extractor will now begin.");
                    ChargeAmountController controller = new ChargeAmountController();
                    await controller.CancelChargeAmountExtraction();
                    await controller.ChargeAmountExtraction();
                    await controller.ColaStubExtraction();
                    await controller.CancelTransactionExtraction();
                    await controller.ColaTransactionExtraction();

                    //Console.WriteLine("\nAll files have been extracted successfully.");
                }
                catch (Exception e)
                {
                    Console.WriteLine("No available data",e.Message);
                }
                Thread.Sleep(3000);
                System.Environment.Exit(1);

            }
        }
    }
}
