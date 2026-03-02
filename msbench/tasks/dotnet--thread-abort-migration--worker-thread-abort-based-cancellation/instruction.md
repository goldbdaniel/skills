Migrate this .NET Framework code to .NET 8. Provide complete migration guidance with code and pitfalls.

```csharp
public class BackgroundProcessor
{
    private Thread _workerThread;
    private volatile bool _running;

    public void Start()
    {
        _running = true;
        _workerThread = new Thread(ProcessLoop);
        _workerThread.IsBackground = true;
        _workerThread.Start();
    }

    private void ProcessLoop()
    {
        while (_running)
        {
            try
            {
                var item = GetNextItem(); // may block
                ProcessItem(item);
            }
            catch (ThreadAbortException)
            {
                // Clean up partial work
                RollbackCurrentTransaction();
                Thread.ResetAbort(); // prevent rethrow so finally runs cleanly
            }
            finally
            {
                FlushBuffers();
            }
        }
    }

    public void Stop()
    {
        _running = false;
        if (_workerThread != null && _workerThread.IsAlive)
        {
            Thread.Sleep(2000); // give it time
            if (_workerThread.IsAlive)
                _workerThread.Abort(); // force stop
        }
    }
}
```
