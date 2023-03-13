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
    internal class FanInChannel : ChannelBase
    {
        public static async Task BaseAPI()
        {
            var channel = Channel.CreateUnbounded<string>();

            var producer = Task.WhenAll(RunMultiple((index) => WriteNumbers(channel.Writer, index), 3)).ContinueWith((t) => channel.Writer.Complete());
            var consumer = Task.Run(() => ReadNumbers(channel.Reader));
            
            await Task.WhenAll(producer, consumer);
        }

        private static async Task WriteNumbers(ChannelWriter<string> writer, int writerNumber)
        {
            foreach (var item in Enumerable.Range(0, 10))
            {
                await writer.WriteAsync($"(Writer {writerNumber}) " + item);
            }
        }

        private static async Task ReadNumbers(ChannelReader<string> reader)
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
            var channel = Channel.CreateUnbounded<string>();
            await Task.WhenAll(
                    channel.Writer.WriteAllConcurrentlyAsync(3, 
                        Enumerable.Range(0, 30).
                            Select(x => ValueTask.FromResult($"(Writer Thread {Thread.CurrentThread.ManagedThreadId} " + x)), complete: true),
                    channel.ReadAll((item) => Console.WriteLine($"Read Item: {item}")).AsTask());
        }
    }
}
