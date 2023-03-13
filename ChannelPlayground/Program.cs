namespace ChannelPlayground
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {
                await Menu();
            }
        }

        private static async Task Menu()
        {
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine("Choose an example to run:");
            Console.WriteLine("1. SimpleChannel");
            Console.WriteLine("2. SimpleChannel.ExtensionAPI");
            Console.WriteLine("3. FanOutChannel");
            Console.WriteLine("4. FanOutChannel.ExtensionAPI");
            Console.WriteLine("5. FanInChannel");
            Console.WriteLine("6. FanInChannel.ExtensionAPI");
            Console.WriteLine("0. Exit.");
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