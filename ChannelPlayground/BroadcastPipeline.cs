using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Open.ChannelExtensions;

namespace ChannelPlayground
{
    internal class BroadcastPipeline
    {
        public static async Task BaseAPI()
        {
            var sqRootChannel = Channel.CreateUnbounded<int>();
            var squareChannel = Channel.CreateUnbounded<int>();

            await Task.WhenAll(
                Task.Run(() => BroadcastNumbers(sqRootChannel.Writer, squareChannel.Writer)),
                Task.Run(() => CalculateSqRoot(sqRootChannel.Reader)),
                Task.Run(() => CalculateSq(squareChannel.Reader)));
        }

        private static async Task BroadcastNumbers(params ChannelWriter<int>[] writers)
        {
            foreach (var item in Enumerable.Range(0, 10))
            {
                var writingTasks = writers.Select(w => w.WriteAsync(item).AsTask());
                await Task.WhenAll(writingTasks);
            }

            foreach (var writer in writers)
            {
                writer.TryComplete();
            }
        }

        private static async Task CalculateSqRoot(ChannelReader<int> reader)
        {
            while (await reader.WaitToReadAsync())
            {
                while (reader.TryRead(out var item))
                {
                    Console.WriteLine($"Read Item: {item}. Square Root is {Math.Sqrt(item)}");
                }
            }
        }

        private static async Task CalculateSq(ChannelReader<int> reader)
        {
            while (await reader.WaitToReadAsync())
            {
                while (reader.TryRead(out var item))
                {
                    Console.WriteLine($"Read Item: {item}. Square is {item * item}");
                }
            }
        }


        public static async Task ExtensionAPI()
        {
            var sqRootChannel = Channel.CreateUnbounded<int>();
            var squareChannel = Channel.CreateUnbounded<int>();
            var writers = new List<ChannelWriter<int>>() { sqRootChannel.Writer, squareChannel.Writer };

            var producer = async () => await Channel.CreateUnbounded<int>()
                .Source(Enumerable.Range(0, 10))
                .ReadAllAsEnumerablesAsync(async (items) => {
                    foreach(var item in items)
                    {
                        var writingTasks = writers.Select(c => c.WriteAsync(item).AsTask());
                        await Task.WhenAll(writingTasks);
                    }
                    foreach (var writer in writers)
                    {
                        writer.TryComplete();
                    }
                });

            await Task.WhenAll(
                Task.Run(async () => await producer()),
                Task.Run(() => CalculateSqRoot(sqRootChannel.Reader)),
                Task.Run(() => CalculateSq(squareChannel.Reader)));
        }
    }
}
