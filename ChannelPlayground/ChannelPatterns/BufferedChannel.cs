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
    internal class BufferedChannel
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
            await foreach (var item in GetNumbersAsync())
            {
                await writer.WriteAsync(item);
            }
            writer.Complete();
        }

        private static async Task ReadNumbers(ChannelReader<int> reader)
        {
            while (await reader.WaitToReadAsync())
            {
                await Task.Delay(500);
                var sum = 0;
                while (reader.TryRead(out var item))
                {
                    sum += item;
                }

                Console.WriteLine($"Sum of batch: {sum}");
            }
        }

        public static async Task ExtensionAPI()
        {
            await Channel.CreateUnbounded<int>()
                 .Source(GetNumbersAsync(), CancellationToken.None)
                 .Batch(10)
                 .ReadAll((item) => Console.WriteLine($"Read Item: {item.Sum()}"));
        }

        private static async IAsyncEnumerable<int> GetNumbersAsync()
        {
            foreach (var tens in Enumerable.Range(0, 10))
            {
                foreach (var ones in Enumerable.Range(0, 10))
                {
                    yield return tens * 10 + ones;
                }
                await Task.Delay(500);
            }
        }
    }
}
