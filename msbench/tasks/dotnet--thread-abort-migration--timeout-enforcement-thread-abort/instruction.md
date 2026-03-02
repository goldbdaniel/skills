Migrate this .NET Framework code to .NET 8. Provide complete migration guidance with code and pitfalls.

```csharp
public class TimedExecutor
{
    public T ExecuteWithTimeout<T>(Func<T> work, TimeSpan timeout)
    {
        T result = default;
        Exception error = null;

        var thread = new Thread(() =>
        {
            try { result = work(); }
            catch (ThreadAbortException) { Thread.ResetAbort(); }
            catch (Exception ex) { error = ex; }
        });

        thread.Start();

        if (!thread.Join(timeout))
        {
            thread.Abort();
            throw new TimeoutException($"Operation exceeded {timeout.TotalSeconds}s timeout");
        }

        if (error != null)
            throw new AggregateException(error);

        return result;
    }
}
```
