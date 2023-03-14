using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Open.ChannelExtensions;

namespace ChannelPlayground.ChannelPatterns
{
    internal class SimpleChannel
    {
        public static async Task BaseAPI()
        {
            var channel = Channel.CreateUnbounded<int>();

            await Task.WhenAll(
                Task.Run(() => WriteNumbers(channel.Writer)),
                Task.Run(() => ReadNumbers(channel.Reader)));
        }

        private static async Task WriteNumbers(ChannelWriter<int> writer)
        {
            foreach (var item in Enumerable.Range(0, 10))
            {
                await writer.WriteAsync(item);
            }
            writer.Complete();
        }

        private static async Task ReadNumbers(ChannelReader<int> reader)
        {
            while (await reader.WaitToReadAsync())
            {
                while (reader.TryRead(out var item))
                {
                    Console.WriteLine($"Read Item: {item}");
                }
            }
        }

        public static async Task ExtensionAPI()
        {
            await Channel.CreateUnbounded<int>()
                 .Source(Enumerable.Range(0, 10))
                 .ReadAll((item) => Console.WriteLine($"Read Item: {item}"));
        }
    }
}
