﻿using Sofos2.Sender.Accounting.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sofos2.Sender.Accounting
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
                    AccountingController controller = new AccountingController();

                    System.Console.WriteLine("The charge amount sender will now begin.");
                    await controller.SendingChargeAmountToAPIAsync();

                    Console.WriteLine("\nAll files have been processed.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Thread.Sleep(3000);
                System.Environment.Exit(1);
            }



        }
    }
}
