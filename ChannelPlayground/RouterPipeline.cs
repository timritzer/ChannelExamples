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
    internal class RouterPipeline
    {
        public static async Task BaseAPI()
        {
            var oddChannel = Channel.CreateUnbounded<int>();
            var evenChannel = Channel.CreateUnbounded<int>();

            await Task.WhenAll(
                Task.Run(() => RouteNumbers(oddChannel.Writer, evenChannel.Writer)),
                Task.Run(() => ReadNumbers(oddChannel.Reader, "Odd Channel")),
                Task.Run(() => ReadNumbers(evenChannel.Reader, "Even Channel")));
        }

        private static async Task RouteNumbers(ChannelWriter<int> oddWriter, ChannelWriter<int> evenWriter)
        {
            foreach (var item in Enumerable.Range(0, 10))
            {
                if (item % 2 == 0)
                {
                    await evenWriter.WriteAsync(item);
                }
                else
                {
                    await oddWriter.WriteAsync(item);
                }
            }
            oddWriter.Complete();
            evenWriter.Complete();
        }

        private static async Task ReadNumbers(ChannelReader<int> reader, string name)
        {
            while (await reader.WaitToReadAsync())
            {
                while (reader.TryRead(out var item))
                {
                    Console.WriteLine($"{name} Read Item: {item}");
                }
            }
        }

        public static async Task ExtensionAPI()
        {
            var oddChannel = Channel.CreateUnbounded<int>();
            var evenChannel = Channel.CreateUnbounded<int>();

            var producer = async () => await Channel.CreateUnbounded<int>()
                .Source(Enumerable.Range(0, 10))
                .ReadAllAsync((item) => (item % 2 == 0 ? evenChannel : oddChannel).Writer.WriteAsync(item));

            await Task.WhenAll(
                Task.Run(async () => await producer()).ContinueWith(async (t) => {
                    await oddChannel.CompleteAsync();
                    await evenChannel.CompleteAsync();
                }),
                Task.Run(() => ReadNumbers(oddChannel.Reader, "Odd Channel")),
                Task.Run(() => ReadNumbers(evenChannel.Reader, "Even Channel")));
        }
    }
}
