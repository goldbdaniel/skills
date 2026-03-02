Migrate this .NET Framework code to .NET 8. Provide complete migration guidance with code and pitfalls.

```csharp
public class EventProcessor
{
    private readonly ManualResetEvent _newEvent = new ManualResetEvent(false);
    private readonly Queue<string> _queue = new Queue<string>();
    private Thread _thread;
    private readonly object _lock = new object();

    public void Start()
    {
        _thread = new Thread(RunLoop);
        _thread.Start();
    }

    private void RunLoop()
    {
        while (true)
        {
            _newEvent.WaitOne(); // blocks until signaled
            _newEvent.Reset();

            string item;
            lock (_lock) { item = _queue.Dequeue(); }
            Handle(item);
        }
    }

    public void Enqueue(string item)
    {
        lock (_lock) { _queue.Enqueue(item); }
        _newEvent.Set();
    }

    public void Stop()
    {
        _thread.Interrupt(); // wake from WaitOne
        _thread.Join(5000);
        if (_thread.IsAlive)
            _thread.Abort();
    }
}
```
