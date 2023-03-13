namespace ChannelPlayground
{
    internal class ChannelBase
    {

        protected static IEnumerable<Task> RunMultiple(Func<int, Task> action, int count)
        {
            return Enumerable.Range(0, count).Select(x => Task.Run(() => action(x)));
        }
        protected static Task WhenAll(Task first, IEnumerable<Task> rest) => Task.WhenAll(rest.Prepend(first));
        protected static Task WhenAll(IEnumerable<Task> rest, Task last) => Task.WhenAll(rest.Append(last));
    }
}