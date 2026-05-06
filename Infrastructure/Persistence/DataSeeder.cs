using Microsoft.EntityFrameworkCore;
using WildTracker.Domain.Entities;
using WildTracker.Domain.Enums;
using WildTracker.Domain.ValueObjects;

namespace WildTracker.Infrastructure.Persistence;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Animals.AnyAsync()) return;

        // ── Users ─────────────────────────────────────────────────────────────

        var admin = new AppUser(
            "Admin", "WildTracker",
            "admin@wildtracker.pl",
            BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            UserRole.Admin);

        var ranger = new AppUser(
            "Jan", "Kowalski",
            "ranger@wildtracker.pl",
            BCrypt.Net.BCrypt.HashPassword("Ranger123!"),
            UserRole.Ranger);

        var researcher = new AppUser(
            "Anna", "Nowak",
            "researcher@wildtracker.pl",
            BCrypt.Net.BCrypt.HashPassword("Research123!"),
            UserRole.Researcher);

        context.Users.AddRange(admin, ranger, researcher);

        // ── Animals ───────────────────────────────────────────────────────────

        var wolf = new Animal(
            "WOLF-001", "Gray Wolf Alpha",
            Species.Wolf, AnimalHealthStatus.Healthy,
            "Pack leader observed in Białowieża Forest");

        var bear = new Animal(
            "BEAR-001", "Brown Bear B1",
            Species.Bear, AnimalHealthStatus.Injured,
            "Injured front-left paw, under observation");

        var lynx = new Animal(
            "LYNX-001", "Eurasian Lynx L1",
            Species.Lynx, AnimalHealthStatus.Healthy,
            "Young male, tracking collar attached");

        var deer = new Animal(
            "DEER-001", "Red Deer D1",
            Species.Deer, AnimalHealthStatus.Healthy,
            null);

        var boar = new Animal(
            "BOAR-001", "Wild Boar W1",
            Species.Boar, AnimalHealthStatus.Sick,
            "Signs of illness, moving slowly");

        context.Animals.AddRange(wolf, bear, lynx, deer, boar);
        await context.SaveChangesAsync();

        // ── Sighting Reports ──────────────────────────────────────────────────

        var report1 = new SightingReport(
            wolf.Id, ranger.Id,
            DateTime.UtcNow.AddDays(-10),
            ReportType.Sighting, SightingSource.Manual,
            new LocationDetails(
                new Coordinates(52.706, 23.854),
                "Białowieża", "Białowieża Forest District"),
            "Pack of 3 wolves spotted near the river");
        report1.Approve();

        var report2 = new SightingReport(
            wolf.Id, researcher.Id,
            DateTime.UtcNow.AddDays(-5),
            ReportType.Sighting, SightingSource.CameraTrap,
            new LocationDetails(
                new Coordinates(52.718, 23.901),
                "Białowieża", "Białowieża Forest District"),
            "Camera trap captured wolf at night — estimated 42 kg male");

        var report3 = new SightingReport(
            bear.Id, ranger.Id,
            DateTime.UtcNow.AddDays(-15),
            ReportType.Injury, SightingSource.Manual,
            new LocationDetails(
                new Coordinates(49.231, 19.982),
                "Tatry", "Tatrzański National Park"),
            "Bear with injured front-left paw, limping visibly");
        report3.Approve();
        report3.Resolve();

        var report4 = new SightingReport(
            lynx.Id, researcher.Id,
            DateTime.UtcNow.AddDays(-3),
            ReportType.Sighting, SightingSource.Drone,
            new LocationDetails(
                new Coordinates(49.103, 22.451),
                "Bieszczady", "Bieszczady District"),
            "Lynx tracked by drone for 20 minutes, collar signal strong");

        var report5 = new SightingReport(
            boar.Id, ranger.Id,
            DateTime.UtcNow.AddDays(-7),
            ReportType.Sighting, SightingSource.Manual,
            new LocationDetails(
                new Coordinates(52.197, 20.850),
                "Mazowieckie", "Kampinos National Park"),
            "Boar showing signs of illness, avoiding the rest of the sounder");

        var report6 = new SightingReport(
            deer.Id, ranger.Id,
            DateTime.UtcNow.AddDays(-2),
            ReportType.Sighting, SightingSource.Manual,
            new LocationDetails(
                new Coordinates(51.107, 17.038),
                "Dolnośląskie", "Milicz Ponds Nature Reserve"),
            "Herd of 8 deer grazing at dawn");
        report6.Approve();

        context.SightingReports.AddRange(report1, report2, report3, report4, report5, report6);
        await context.SaveChangesAsync();

        // Mark animals as last seen
        wolf.MarkSeen(report2.ObservedAtUtc);
        bear.MarkSeen(report3.ObservedAtUtc);
        lynx.MarkSeen(report4.ObservedAtUtc);
        deer.MarkSeen(report6.ObservedAtUtc);
        boar.MarkSeen(report5.ObservedAtUtc);
        await context.SaveChangesAsync();

        // ── Observation Notes ─────────────────────────────────────────────────

        context.ObservationNotes.AddRange(
            new ObservationNote(report1.Id, researcher.Id,
                "Pack appears healthy. Alpha male estimated at 40 kg."),
            new ObservationNote(report1.Id, ranger.Id,
                "Camera traps set up in sector B3 for follow-up monitoring."),
            new ObservationNote(report3.Id, ranger.Id,
                "Bear avoiding weight on left paw. Veterinary intervention recommended."),
            new ObservationNote(report3.Id, researcher.Id,
                "Injury pattern consistent with snare trap. Reported to local authorities."),
            new ObservationNote(report4.Id, researcher.Id,
                "Collar transmitting at 30-minute intervals. Battery at 87%."),
            new ObservationNote(report5.Id, ranger.Id,
                "Boar separated from sounder — will check for ASF symptoms.")
        );

        await context.SaveChangesAsync();
    }
}
