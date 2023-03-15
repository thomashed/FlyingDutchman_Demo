using System.Diagnostics;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer.Stubs;

public class FlyingDutchmanAirlinesContext_Stub : FlyingDutchmanAirlinesContext
{
    public FlyingDutchmanAirlinesContext_Stub(DbContextOptions<FlyingDutchmanAirlinesContext> options) 
        : base(options)
    {
        base.Database.EnsureDeleted(); // deletes the in memory database
    }

    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<EntityEntry> pendingChanges = ChangeTracker.Entries().Where(e => e.State == EntityState.Added);
        IEnumerable<Booking> bookings = pendingChanges.Select(e => e.Entity).OfType<Booking>();
        IEnumerable<Airport> airports = pendingChanges.Select(e => e.Entity).OfType<Airport>();

        if (bookings.Any(b => b.CustomerId != 1))
        {
            throw new Exception("Database Error!");
        }

        // TODO: continue this
        // if (!airports.Any())
        // {
        //     throw new Exception("Database Error!"); 
        // }
        
        await base.SaveChangesAsync(cancellationToken);
        return 1;
    }
}