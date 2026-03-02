I have an EF Core 8 app. These two operations are extremely slow because they load all entities into memory:

```csharp
// Operation 1: Deactivate all products not sold in 6 months
var staleProducts = await _db.Products
    .Where(p => p.LastSoldDate < DateTime.UtcNow.AddMonths(-6))
    .ToListAsync();
foreach (var p in staleProducts)
{
    p.IsActive = false;
}
await _db.SaveChangesAsync();

// Operation 2: Delete all audit logs older than 1 year
var oldLogs = await _db.AuditLogs
    .Where(l => l.CreatedAt < DateTime.UtcNow.AddYears(-1))
    .ToListAsync();
_db.AuditLogs.RemoveRange(oldLogs);
await _db.SaveChangesAsync();
```

There are 500K products and 2M audit logs. How do I make these efficient?
