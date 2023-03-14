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
    internal class FanOutChannel : ChannelBase
    {
        public static async Task BaseAPI()
        {
            var channel = Channel.CreateUnbounded<int>();

            var producer = Task.Run(() => WriteNumbers(channel.Writer));
            var consumer = RunMultiple((index) => ReadNumbers(channel.Reader, index), 3);

            await WhenAll(producer, consumer);
        }

        private static async Task WriteNumbers(ChannelWriter<int> writer)
        {
            foreach (var item in Enumerable.Range(0, 10))
            {
                await writer.WriteAsync(item);
            }
            writer.Complete();
        }

        private static async Task ReadNumbers(ChannelReader<int> reader, int readerNumber)
        {
            while (await reader.WaitToReadAsync())
            {
                while (reader.TryRead(out var item))
                {
                    Console.WriteLine($"Reader# {readerNumber} read Item: {item}");
                }
            }
        }

        public static async Task ExtensionAPI()
        {
            await Channel.CreateUnbounded<int>()
                 .Source(Enumerable.Range(0, 10))
                 .ReadAllConcurrently(3, (item) => Console.WriteLine($"Reader thread id: {Thread.CurrentThread.ManagedThreadId} Read Item: {item}"));
        }
    }
}
