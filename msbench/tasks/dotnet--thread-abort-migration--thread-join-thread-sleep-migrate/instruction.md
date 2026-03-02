I'm migrating this .NET Framework code to .NET 8 and need to ensure all Thread.Abort
usage is addressed. Check this code and provide your Thread.Abort migration guidance.

```csharp
public class SimpleWorker
{
    public void RunBatch(List<WorkItem> items)
    {
        var threads = new List<Thread>();

        foreach (var item in items)
        {
            var t = new Thread(() => Process(item));
            t.Start();
            threads.Add(t);
        }

        foreach (var t in threads)
            t.Join();
    }

    private void Process(WorkItem item)
    {
        // simulate work
        Thread.Sleep(100);
        item.Result = ComputeResult(item.Input);
    }
}
```
