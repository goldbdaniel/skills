Migrate this .NET Framework ASP.NET code to ASP.NET Core (.NET 8). Provide complete migration guidance with code and pitfalls.

```csharp
public class ReportController : Controller
{
    protected void btnExport_Click(object sender, EventArgs e)
    {
        var data = GenerateReport();
        Response.Clear();
        Response.ContentType = "text/csv";
        Response.AddHeader("Content-Disposition", "attachment; filename=report.csv");
        Response.Write(data);
        Response.End(); // throws ThreadAbortException internally
    }

    protected void btnRedirect_Click(object sender, EventArgs e)
    {
        if (!User.Identity.IsAuthenticated)
        {
            Response.Redirect("/login", true); // true = endResponse, calls Thread.Abort
            return;
        }

        try
        {
            ProcessRequest();
            Response.Redirect("/success", true);
        }
        catch (ThreadAbortException)
        {
            // Expected from Response.Redirect — swallow it
        }
        catch (Exception ex)
        {
            LogError(ex);
            Response.Redirect("/error", true);
        }
    }
}
```
