﻿using ChannelPlayground.ChannelPatterns;
using ChannelPlayground.PipelinePatterns;

namespace ChannelPlayground
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {
                await Menu();
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
            }
        }

        private static async Task Menu()
        {
            Console.BackgroundColor= ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            Console.WriteLine("||========================================================================================================||");
            Console.WriteLine("|| Choose an example to run:                                                                              ||");
            Console.WriteLine("|| 1.  SimpleChannel                                                                                      ||");
            Console.WriteLine("|| 2.  SimpleChannel.ExtensionAPI                                                                         ||");
            Console.WriteLine("|| 3.  FanOutChannel                                                                                      ||");
            Console.WriteLine("|| 4.  FanOutChannel.ExtensionAPI                                                                         ||");
            Console.WriteLine("|| 5.  FanInChannel                                                                                       ||");
            Console.WriteLine("|| 6.  FanInChannel.ExtensionAPI                                                                          ||");
            Console.WriteLine("|| 7.  BufferedChannel                                                                                    ||");
            Console.WriteLine("|| 8.  BufferedChannel.ExtensionAPI                                                                       ||");
            Console.WriteLine("|| 9.  RouterPipeline                                                                                     ||");
            Console.WriteLine("|| 10. RouterPipeline.ExtensionAPI                                                                        ||");
            Console.WriteLine("|| 11. BroadcastPipeline                                                                                  ||");
            Console.WriteLine("|| 12. BroadcastPipeline.ExtensionAPI                                                                     ||");
            Console.WriteLine("|| 0.  Exit.                                                                                              ||");
            Console.WriteLine("||========================================================================================================||");
            
            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    await SimpleChannel.BaseAPI();
                    break;
                case "2":
                    await SimpleChannel.ExtensionAPI();
                    break;
                case "3":
                    await FanOutChannel.BaseAPI();
                    break;
                case "4":
                    await FanOutChannel.ExtensionAPI();
                    break;
                case "5":
                    await FanInChannel.BaseAPI();
                    break;
                case "6":
                    await FanInChannel.ExtensionAPI();
                    break;
                case "7":
                    await BufferedChannel.BaseAPI();
                    break;
                case "8":
                    await BufferedChannel.ExtensionAPI();
                    break;
                case "9":
                    await RouterPipeline.BaseAPI();
                    break;
                case "10":
                    await RouterPipeline.ExtensionAPI();
                    break;

                case "11":
                    await BroadcastPipeline.BaseAPI();
                    break;
                case "12":
                    await BroadcastPipeline.ExtensionAPI();
                    break;
                case "0":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        }
    }
}