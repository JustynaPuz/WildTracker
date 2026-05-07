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

        var admin      = User("Admin",    "WildTracker",  "admin@wildtracker.pl",           "Admin123!",   UserRole.Admin);
        var rangerPL   = User("Jan",      "Kowalski",     "ranger@wildtracker.pl",           "Ranger123!",  UserRole.Ranger);
        var resrchPL   = User("Anna",     "Nowak",        "researcher@wildtracker.pl",       "Research123!", UserRole.Researcher);
        var rangerUS   = User("James",    "Harlow",       "j.harlow@yellowstone.gov",        "Ranger123!",  UserRole.Ranger);
        var resrchCA   = User("Sophie",   "Tremblay",     "stremblay@wildlife.ca",           "Research123!", UserRole.Researcher);
        var rangerNO   = User("Erik",     "Bjørnstad",    "e.bjornstad@miljo.no",            "Ranger123!",  UserRole.Ranger);
        var resrchRU   = User("Mikhail",  "Volkov",       "m.volkov@wwf.ru",                 "Research123!", UserRole.Researcher);
        var rangerKE   = User("Amara",    "Oduya",        "a.oduya@kws.go.ke",               "Ranger123!",  UserRole.Ranger);
        var resrchIN   = User("Priya",    "Sharma",       "p.sharma@wii.gov.in",             "Research123!", UserRole.Researcher);
        var rangerDE   = User("Lukas",    "Bauer",        "l.bauer@bfn.de",                  "Ranger123!",  UserRole.Ranger);
        var resrchBR   = User("Carlos",   "Mendes",       "c.mendes@icmbio.gov.br",          "Research123!", UserRole.Researcher);
        var rangerAU   = User("Fiona",    "McAllister",   "f.mcallister@parks.vic.gov.au",   "Ranger123!",  UserRole.Ranger);
        var resrchES   = User("María",    "García",       "m.garcia@csic.es",                "Research123!", UserRole.Researcher);
        var rangerZA   = User("Sipho",    "Dlamini",      "s.dlamini@sanparks.org",          "Ranger123!",  UserRole.Ranger);
        var resrchJP   = User("Kenji",    "Tanaka",       "k.tanaka@env.go.jp",              "Research123!", UserRole.Researcher);

        context.Users.AddRange(admin, rangerPL, resrchPL, rangerUS, resrchCA, rangerNO, resrchRU,
            rangerKE, resrchIN, rangerDE, resrchBR, rangerAU, resrchES, rangerZA, resrchJP);

        // ── Animals ───────────────────────────────────────────────────────────
        // Poland
        var w01 = A("WOLF-PL-001", "Białowieża Alpha Male",         Species.Wolf, AnimalHealthStatus.Healthy,  "Pack leader, GPS collar #W01. Białowieża Forest.");
        var w02 = A("WOLF-PL-002", "Białowieża Beta Female",        Species.Wolf, AnimalHealthStatus.Healthy,  "Adult female, same pack as W01. 3 pups this season.");
        var w03 = A("WOLF-PL-003", "Białowieża Juvenile Male",      Species.Wolf, AnimalHealthStatus.Healthy,  "1-year-old male, dispersal expected next spring.");
        var w04 = A("WOLF-PL-004", "Bieszczady Lone Wolf",          Species.Wolf, AnimalHealthStatus.Injured,  "Adult male, limping on rear-right leg. Possible trap injury.");
        var b01 = A("BEAR-PL-001", "Tatra Brown Bear T1",           Species.Bear, AnimalHealthStatus.Injured,  "Adult male, injured front-left paw.");
        var b02 = A("BEAR-PL-002", "Tatra Brown Bear T2",           Species.Bear, AnimalHealthStatus.Healthy,  "Female with 2 cubs. Den on northern slope.");
        var l01 = A("LYNX-PL-001", "Bieszczady Lynx Female",        Species.Lynx, AnimalHealthStatus.Healthy,  "GPS collar #L01. Established home range 280 km².");
        var l02 = A("LYNX-PL-002", "Carpathian Lynx Juvenile",      Species.Lynx, AnimalHealthStatus.Healthy,  "8-month-old male. Still with mother.");
        var d01 = A("DEER-PL-001", "Red Deer Milicz Female",        Species.Deer, AnimalHealthStatus.Healthy,  "Part of herd of 9. Milicz Ponds.");
        var d02 = A("DEER-PL-002", "Red Deer Białowieża Male",      Species.Deer, AnimalHealthStatus.Healthy,  "Large stag, 12-point antlers. Dominant male.");
        var bo1 = A("BOAR-PL-001", "Wild Boar Kampinos M1",         Species.Boar, AnimalHealthStatus.Sick,     "ASF exposure suspected. Isolated from sounder.");
        var bo2 = A("BOAR-PL-002", "Wild Boar Augustów Female",     Species.Boar, AnimalHealthStatus.Healthy,  "Adult sow, litter of 6 observed.");

        // USA – Yellowstone & Alaska
        var w05 = A("WOLF-YNP-001", "Junction Butte Alpha",         Species.Wolf, AnimalHealthStatus.Healthy,  "Alpha male, Junction Butte Pack. Collar #926M.");
        var w06 = A("WOLF-YNP-002", "Wapiti Lake Female",           Species.Wolf, AnimalHealthStatus.Healthy,  "Alpha female, Wapiti Lake Pack. Collar #907F.");
        var w07 = A("WOLF-YNP-003", "8 Mile Pack Yearling",         Species.Wolf, AnimalHealthStatus.Healthy,  "Yearling male, 8 Mile Pack. Uncollared.");
        var b03 = A("BEAR-YNP-001", "Grizzly 399",                  Species.Bear, AnimalHealthStatus.Healthy,  "Famous Yellowstone grizzly, 28 years old.");
        var b04 = A("BEAR-YNP-002", "Grizzly 610",                  Species.Bear, AnimalHealthStatus.Healthy,  "Daughter of 399. Collar #610F.");
        var b05 = A("BEAR-YNP-003", "Grizzly 863",                  Species.Bear, AnimalHealthStatus.Healthy,  "Subadult male. Frequently seen at Trout Lake.");
        var b06 = A("BEAR-AK-001",  "Kodiak Brown Bear AK-M1",      Species.Bear, AnimalHealthStatus.Healthy,  "Dominant male at Karluk Lake salmon run. ~480 kg.");
        var d03 = A("DEER-US-001",  "White-tailed Deer NY-F1",      Species.Deer, AnimalHealthStatus.Healthy,  "Adult female, suburban interface, New York State.");
        var d04 = A("DEER-US-002",  "Mule Deer Utah-M1",            Species.Deer, AnimalHealthStatus.Healthy,  "Large buck, Capitol Reef NP. 10-point rack.");
        var bi1 = A("BIRD-US-001",  "Bald Eagle WA-M1",             Species.Bird, AnimalHealthStatus.Healthy,  "Adult male. Nesting pair at Skagit River.");
        var bi2 = A("BIRD-US-002",  "California Condor AZ-F1",      Species.Bird, AnimalHealthStatus.Healthy,  "Tagged #AC-8, released 2022. Grand Canyon South Rim.");

        // Canada – Banff & British Columbia
        var w08 = A("WOLF-BNP-001", "Banff Bow Valley Lone Wolf",   Species.Wolf, AnimalHealthStatus.Healthy,  "Dispersed from Cascade Pack. GPS collar active.");
        var w09 = A("WOLF-BC-001",  "Great Bear Rainforest Wolf",   Species.Wolf, AnimalHealthStatus.Healthy,  "Coastal wolf, fish-eating ecotype. BC coast.");
        var b07 = A("BEAR-BNP-001", "Banff Grizzly BNP-M1",        Species.Bear, AnimalHealthStatus.Healthy,  "Adult male. Regular user of wildlife overpasses.");
        var b08 = A("BEAR-BC-001",  "Spirit Bear BC-F1",            Species.Bear, AnimalHealthStatus.Healthy,  "Rare white-phase black bear (Kermode). Princess Royal Island.");
        var mo1 = A("MOOSE-BNP-001","Banff Bull Moose M1",          Species.Moose, AnimalHealthStatus.Healthy, "Large bull, ~600 kg. Vermilion Lakes resident.");
        var mo2 = A("MOOSE-BC-001", "BC Moose Female F1",           Species.Moose, AnimalHealthStatus.Healthy, "Adult cow with twin calves. Prince George area.");
        var l03 = A("LYNX-CA-001",  "Canadian Lynx Yukon-F1",       Species.Lynx, AnimalHealthStatus.Healthy,  "Adult female. Home range 135 km². Collar #YK-04.");
        var l04 = A("LYNX-CA-002",  "Canadian Lynx Ontario-M1",     Species.Lynx, AnimalHealthStatus.Healthy,  "Subadult male. High snowshoe hare prey year.");

        // Norway & Sweden
        var w10 = A("WOLF-NO-001",  "Kynna Pack Alpha Male",        Species.Wolf, AnimalHealthStatus.Healthy,  "Cross-border Norway–Sweden territory.");
        var w11 = A("WOLF-NO-002",  "Slettås Pack Female",          Species.Wolf, AnimalHealthStatus.Healthy,  "Alpha female. 5 pups confirmed this denning season.");
        var w12 = A("WOLF-SE-001",  "Galven Pack Alpha",            Species.Wolf, AnimalHealthStatus.Healthy,  "Established Swedish pack, 11 individuals.");
        var mo3 = A("MOOSE-NO-001", "Femunden Bull Moose",          Species.Moose, AnimalHealthStatus.Healthy, "Large bull near Femunden lake. ~580 kg.");
        var mo4 = A("MOOSE-NO-002", "Hedmark Cow Moose F1",         Species.Moose, AnimalHealthStatus.Healthy, "Cow with single calf. Hedmark county.");
        var mo5 = A("MOOSE-SE-001", "Dalarna Bull Moose M2",        Species.Moose, AnimalHealthStatus.Healthy, "Tagged #SE-M2. Road collision risk area.");
        var b09 = A("BEAR-SE-001",  "Scandinavian Brown Bear SE-M1",Species.Bear, AnimalHealthStatus.Healthy,  "Adult male, Jämtland county. ~220 kg.");
        var b10 = A("BEAR-SE-002",  "Scandinavian Brown Bear SE-F1",Species.Bear, AnimalHealthStatus.Healthy,  "Female with 3 cubs, Dalarna. Remarkably large litter.");

        // Russia – Kamchatka & Siberia
        var b11 = A("BEAR-KAM-001", "Kamchatka Brown Bear K1",      Species.Bear, AnimalHealthStatus.Healthy,  "Dominant male, Kurilskoye Lake. ~480 kg.");
        var b12 = A("BEAR-KAM-002", "Kamchatka Brown Bear K2",      Species.Bear, AnimalHealthStatus.Dead,     "Female, found deceased. Suspected poaching.");
        var b13 = A("BEAR-KAM-003", "Kamchatka Brown Bear K3",      Species.Bear, AnimalHealthStatus.Healthy,  "Subadult female. Collar #KAM-F3.");
        var w13 = A("WOLF-SIB-001", "Siberian Wolf Pack Alpha",      Species.Wolf, AnimalHealthStatus.Healthy,  "Alpha male, large boreal pack. Yakutia.");
        var w14 = A("WOLF-SIB-002", "Altai Wolf M1",                 Species.Wolf, AnimalHealthStatus.Healthy,  "Adult male, Altai Mountains. Crosses into Mongolia.");

        // Germany & Switzerland
        var l05 = A("LYNX-DE-001",  "Black Forest Lynx Hilde",      Species.Lynx, AnimalHealthStatus.Healthy,  "Reintroduced female. Range expanded 30% this year.");
        var l06 = A("LYNX-DE-002",  "Harz Lynx Male",               Species.Lynx, AnimalHealthStatus.Healthy,  "Adult male, Harz NP. First confirmed Harz lynx since reintroduction.");
        var l07 = A("LYNX-CH-001",  "Swiss Jura Lynx F2",           Species.Lynx, AnimalHealthStatus.Healthy,  "Female. GPS collar #CH-F2. Range crosses into France.");
        var fo1 = A("FOX-DE-001",   "Red Fox Schwarzwald M1",       Species.Fox,  AnimalHealthStatus.Healthy,  "Juvenile male. Den near Titisee.");
        var fo2 = A("FOX-DE-002",   "Urban Red Fox Berlin F1",      Species.Fox,  AnimalHealthStatus.Healthy,  "Female adapted to urban Berlin. 4 cubs in park den.");
        var d05 = A("DEER-DE-001",  "Roe Deer Bavaria M1",          Species.Deer, AnimalHealthStatus.Healthy,  "Adult buck, agricultural fringe, Bavaria.");
        var bo3 = A("BOAR-DE-001",  "Wild Boar Brandenburg M1",     Species.Boar, AnimalHealthStatus.Healthy,  "Large boar, GPS collar #BB-01. Forest-field interface.");

        // Spain & Portugal
        var l08 = A("LYNX-ES-001",  "Iberian Lynx Doñana M1",       Species.Lynx, AnimalHealthStatus.Healthy,  "Adult male. Doñana NP. Part of successful reintroduction.");
        var l09 = A("LYNX-ES-002",  "Iberian Lynx Sierra Morena F1",Species.Lynx, AnimalHealthStatus.Healthy,  "Breeding female. 3 kittens confirmed this season.");
        var w15 = A("WOLF-ES-001",  "Iberian Wolf Cantabria M1",    Species.Wolf, AnimalHealthStatus.Healthy,  "Adult male, Cantabrian Mountains. Dispersing south.");
        var w16 = A("WOLF-ES-002",  "Iberian Wolf Galicia Pack",    Species.Wolf, AnimalHealthStatus.Healthy,  "Pack of 6, Galicia. Livestock conflict area.");
        var d06 = A("DEER-ES-001",  "Iberian Red Deer Coto Doñana", Species.Deer, AnimalHealthStatus.Healthy,  "Male, large herd. Coto Doñana reserve.");
        var bo4 = A("BOAR-ES-001",  "Iberian Wild Boar ES-M1",      Species.Boar, AnimalHealthStatus.Healthy,  "Adult male, Extremadura dehesa.");

        // Romania & Balkans
        var b14 = A("BEAR-ROM-001", "Carpathian Bear Brașov M1",    Species.Bear, AnimalHealthStatus.Healthy,  "Conflict animal near Brașov. Deterrence programme.");
        var b15 = A("BEAR-ROM-002", "Carpathian Bear Bucegi F1",    Species.Bear, AnimalHealthStatus.Healthy,  "Female, 2 cubs. Core zone of Bucegi NP.");
        var w17 = A("WOLF-ROM-001", "Bucegi Pack Alpha Female",     Species.Wolf, AnimalHealthStatus.Healthy,  "Alpha female. 4 pups this season.");
        var w18 = A("WOLF-ROM-002", "Retezat Pack Male",            Species.Wolf, AnimalHealthStatus.Healthy,  "Disperser establishing new territory. GPS collar #RO-M2.");

        // Italy
        var w19 = A("WOLF-IT-001",  "Apennine Wolf Abruzzo M1",     Species.Wolf, AnimalHealthStatus.Healthy,  "Adult male, Abruzzo NP. Pack of 7.");
        var w20 = A("WOLF-IT-002",  "Alpine Wolf Stelvio F1",       Species.Wolf, AnimalHealthStatus.Healthy,  "Female, Stelvio NP. Cross-border with Switzerland.");
        var bo5 = A("BOAR-IT-001",  "Tuscan Wild Boar IT-M1",       Species.Boar, AnimalHealthStatus.Healthy,  "Adult male, Maremma Regional Park.");

        // Kenya & East Africa
        var bi3 = A("BIRD-KE-001",  "Secretary Bird Mara F1",       Species.Bird, AnimalHealthStatus.Healthy,  "Nesting female, Maasai Mara. 2 eggs confirmed.");
        var bi4 = A("BIRD-KE-002",  "Martial Eagle Mara M1",        Species.Bird, AnimalHealthStatus.Healthy,  "Adult male. Largest eagle in Africa.");
        var ot1 = A("OTHER-KE-001", "Honey Badger Mara M1",         Species.Other, AnimalHealthStatus.Healthy, "Radio tag #HB-04. Exceptionally bold.");
        var ot2 = A("OTHER-KE-002", "African Wild Dog Laikipia M1", Species.Other, AnimalHealthStatus.Healthy, "Pack of 12. Laikipia Plateau. Collar on alpha #LK-01.");

        // South Africa
        var ot3 = A("OTHER-ZA-001", "African Leopard Kruger M1",    Species.Other, AnimalHealthStatus.Healthy, "Adult male, Kruger NP. Large home range.");
        var ot4 = A("OTHER-ZA-002", "African Wild Dog Hluhluwe M1", Species.Other, AnimalHealthStatus.Healthy, "Alpha male, pack of 8. Hluhluwe-iMfolozi Park.");
        var bi5 = A("BIRD-ZA-001",  "Cape Vulture Drakensberg F1",  Species.Bird, AnimalHealthStatus.Injured,  "Wing injury, likely power line collision. Under care.");
        var bi6 = A("BIRD-ZA-002",  "Verreaux's Eagle ZA-M1",       Species.Bird, AnimalHealthStatus.Healthy,  "Adult male, Drakensberg. Permanent territory.");

        // India
        var ot5 = A("OTHER-IN-001", "Bengal Tiger Ranthambore F1",  Species.Other, AnimalHealthStatus.Healthy, "T-19 'Krishna', dominant tigress. 4 cubs.");
        var ot6 = A("OTHER-IN-002", "Snow Leopard Spiti M1",        Species.Other, AnimalHealthStatus.Healthy, "Adult male, Spiti Valley. GPS collar #SL-M1.");
        var b16 = A("BEAR-IN-001",  "Sloth Bear Pench M1",          Species.Bear, AnimalHealthStatus.Healthy,  "Adult male, Pench Tiger Reserve.");
        var b17 = A("BEAR-IN-002",  "Himalayan Brown Bear HP-F1",   Species.Bear, AnimalHealthStatus.Healthy,  "Female with cubs, Great Himalayan NP.");

        // Japan
        var b18 = A("BEAR-JP-001",  "Hokkaido Brown Bear HK-M1",    Species.Bear, AnimalHealthStatus.Healthy,  "Adult male, Shiretoko Peninsula. ~200 kg.");
        var fo3 = A("FOX-JP-001",   "Ezo Red Fox Hokkaido F1",      Species.Fox,  AnimalHealthStatus.Healthy,  "Adult female, Akan-Mashu NP. 3 cubs.");
        var d07 = A("DEER-JP-001",  "Hokkaido Sika Deer M1",        Species.Deer, AnimalHealthStatus.Healthy,  "Large stag. Overabundant population, managed cull.");

        // Brazil & South America
        var ot7 = A("OTHER-BR-001", "Jaguar Pantanal M1",           Species.Other, AnimalHealthStatus.Healthy, "Adult male, ~120 kg. GPS collar #JAG-01.");
        var ot8 = A("OTHER-BR-002", "Giant Anteater Cerrado M1",    Species.Other, AnimalHealthStatus.Healthy, "Adult male, Emas NP. Radio tag #ANT-03.");
        var ot9 = A("OTHER-BR-003", "Maned Wolf Cerrado F1",        Species.Other, AnimalHealthStatus.Healthy, "Female with 3 pups. Serra da Canastra NP.");

        // Australia
        var ot10= A("OTHER-AU-001", "Tasmanian Devil TAS-F1",       Species.Other, AnimalHealthStatus.Sick,    "Female, DFTD facial tumour disease detected.");
        var ot11= A("OTHER-AU-002", "Dingo Fraser Island M1",       Species.Other, AnimalHealthStatus.Healthy, "Pack leader, Fraser Island. Monitored for tourist conflict.");
        var fo4 = A("FOX-AU-001",   "Red Fox Victoria F1",          Species.Fox,  AnimalHealthStatus.Healthy,  "Invasive species. Monitoring near bilby habitat.");
        var bi7 = A("BIRD-AU-001",  "Wedge-tailed Eagle AU-M1",     Species.Bird, AnimalHealthStatus.Healthy,  "Adult male. Wingspan 220 cm. Alpine NP Victoria.");

        // Mongolia & Central Asia
        var ot12= A("OTHER-MN-001", "Snow Leopard Altai F1",        Species.Other, AnimalHealthStatus.Healthy, "Adult female, Altai Tavan Bogd NP. GPS collar.");
        var w21 = A("WOLF-MN-001",  "Mongolian Steppe Wolf M1",     Species.Wolf, AnimalHealthStatus.Healthy,  "Pack of 14. Large range across Gobi-Altai.");

        // Arctic & Iceland
        var fo5 = A("FOX-IS-001",   "Arctic Fox Iceland F1",        Species.Fox,  AnimalHealthStatus.Healthy,  "Adult female, Hornstrandir NR. 6 pups this year.");
        var fo6 = A("FOX-IS-002",   "Arctic Fox Iceland M1",        Species.Fox,  AnimalHealthStatus.Healthy,  "Adult male, Westfjords. Blue-phase morph.");
        var bi8 = A("BIRD-NO-001",  "White-tailed Eagle Norway M1", Species.Bird, AnimalHealthStatus.Healthy,  "Adult male, Lofoten Islands. Wingspan 240 cm.");

        // UK
        var d08 = A("DEER-UK-001",  "Red Deer Highland Stag M1",    Species.Deer, AnimalHealthStatus.Healthy,  "Dominant stag, Cairngorms NP. 14-point antlers.");
        var d09 = A("DEER-UK-002",  "Fallow Deer New Forest F1",    Species.Deer, AnimalHealthStatus.Healthy,  "Adult doe, New Forest. Part of managed herd.");
        var fo7 = A("FOX-UK-001",   "Urban Red Fox London F1",      Species.Fox,  AnimalHealthStatus.Healthy,  "Female, Hackney. Den under garden shed. 5 cubs.");

        // France
        var d10 = A("DEER-FR-001",  "European Roe Deer Vosges M1",  Species.Deer, AnimalHealthStatus.Healthy,  "Adult buck, Vosges mountains.");
        var bo6 = A("BOAR-FR-001",  "Wild Boar Ardennes FR-M1",     Species.Boar, AnimalHealthStatus.Healthy,  "Large boar, Ardennes forest. 120 kg.");
        var w22 = A("WOLF-FR-001",  "French Alps Wolf Mercantour F1",Species.Wolf, AnimalHealthStatus.Healthy, "Alpha female, Mercantour NP. Pack re-established 2020.");

        // Borneo & SE Asia
        var b19 = A("BEAR-BN-001",  "Bornean Sun Bear BN-M1",       Species.Bear, AnimalHealthStatus.Healthy,  "Adult male, Danum Valley. Smallest bear species.");
        var ot13= A("OTHER-BN-001", "Clouded Leopard Borneo F1",    Species.Other, AnimalHealthStatus.Healthy, "Adult female, camera-trapped in Maliau Basin.");

        context.Animals.AddRange(
            w01,w02,w03,w04, b01,b02, l01,l02, d01,d02, bo1,bo2,
            w05,w06,w07, b03,b04,b05,b06, d03,d04, bi1,bi2,
            w08,w09, b07,b08, mo1,mo2, l03,l04,
            w10,w11,w12, mo3,mo4,mo5, b09,b10,
            b11,b12,b13, w13,w14,
            l05,l06,l07, fo1,fo2, d05, bo3,
            l08,l09, w15,w16, d06, bo4,
            b14,b15, w17,w18,
            w19,w20, bo5,
            bi3,bi4, ot1,ot2,
            ot3,ot4, bi5,bi6,
            ot5,ot6, b16,b17,
            b18, fo3, d07,
            ot7,ot8,ot9,
            ot10,ot11, fo4, bi7,
            ot12, w21,
            fo5,fo6, bi8,
            d08,d09, fo7,
            d10, bo6, w22,
            b19, ot13);

        await context.SaveChangesAsync();

        // ── Sighting Reports ──────────────────────────────────────────────────
        // Format: R(animal, reporter, daysAgo, type, source, lat, lon, region, district, description, approve?, resolve?)

        var reports = new List<SightingReport>
        {
            // Poland – wolves
            R(w01,rangerPL,-60,ReportType.Sighting,SightingSource.CameraTrap,52.706,23.854,"Białowieża","Białowieża Forest","Pack of 4 crossing main trail at 02:14.",true),
            R(w01,resrchPL,-30,ReportType.Sighting,SightingSource.Manual,52.718,23.901,"Białowieża","Białowieża Forest","Alpha male leading pack south. Collar signal strong.",true),
            R(w02,resrchPL,-45,ReportType.Sighting,SightingSource.Sensor,52.711,23.877,"Białowieża","Białowieża Forest","GPS collar ping. Female resting near sector B7.",true),
            R(w02,rangerPL,-8, ReportType.DangerousBehavior,SightingSource.Manual,52.698,23.832,"Białowieża","Białowieża Forest","Female approached village perimeter at dusk. Deterred.",false),
            R(w03,resrchPL,-20,ReportType.Sighting,SightingSource.CameraTrap,52.702,23.862,"Białowieża","Białowieża Forest","Juvenile male foraging alone. Dispersal behaviour observed.",false),
            R(w04,rangerPL,-55,ReportType.Injury,SightingSource.Manual,49.312,22.701,"Bieszczady","Bieszczady NP","Lone wolf limping. Rear-right leg injury. Likely snare.",true),
            R(w04,resrchPL,-12,ReportType.Sighting,SightingSource.Drone,49.298,22.714,"Bieszczady","Bieszczady NP","Drone: wolf moving slower than normal but foraging.",true),
            // Poland – bears
            R(b01,rangerPL,-90,ReportType.Injury,SightingSource.Manual,49.231,19.982,"Tatry","Tatrzański NP","Bear with injured front-left paw. Snare suspected.",true,false,true),
            R(b01,rangerPL,-18,ReportType.Sighting,SightingSource.Drone,49.244,20.017,"Tatry","Tatrzański NP","Paw still swollen but bear feeding actively.",true),
            R(b02,resrchPL,-35,ReportType.Sighting,SightingSource.Manual,49.218,19.956,"Tatry","Tatrzański NP","Female with 2 cubs at berry patch. All healthy.",true),
            // Poland – other
            R(l01,resrchPL,-15,ReportType.Sighting,SightingSource.Drone,49.103,22.451,"Bieszczady","Bieszczady District","Drone tracked lynx 22 min. Collar battery 81%.",false),
            R(l02,rangerPL,-22,ReportType.Sighting,SightingSource.CameraTrap,49.099,22.388,"Bieszczady","Bieszczady District","Juvenile male photographed with mother at dawn.",true),
            R(d01,rangerPL,-8, ReportType.Sighting,SightingSource.Manual,51.107,17.038,"Dolnośląskie","Milicz Ponds","Herd of 9 deer including 1 fawn. All healthy.",true),
            R(d02,resrchPL,-41,ReportType.Sighting,SightingSource.Manual,52.714,23.895,"Białowieża","Białowieża Forest","Large stag in velvet. Estimated 200 kg.",true),
            R(bo1,rangerPL,-40,ReportType.Sighting,SightingSource.Manual,52.197,20.850,"Mazowieckie","Kampinos NP","Boar isolated. ASF symptoms suspected.",false),
            R(bo2,rangerPL,-14,ReportType.Sighting,SightingSource.Manual,53.850,22.994,"Warmia-Mazury","Augustów Primeval Forest","Sow with 6 piglets. Healthy.",true),
            // USA – Yellowstone wolves
            R(w05,rangerUS,-55,ReportType.Sighting,SightingSource.Manual,44.979,-110.696,"Wyoming","Yellowstone – Lamar Valley","Junction Butte alpha sighted at dawn. Pack of 9.",true),
            R(w05,rangerUS,-22,ReportType.Sighting,SightingSource.CameraTrap,44.895,-110.412,"Wyoming","Yellowstone – Pelican Valley","Camera trap: alpha hunting elk calf. Successful kill.",true),
            R(w06,rangerUS,-48,ReportType.Sighting,SightingSource.Sensor,44.721,-110.802,"Wyoming","Yellowstone – Wapiti corridor","GPS telemetry: female with 3 pups at den.",true),
            R(w07,rangerUS,-33,ReportType.Sighting,SightingSource.Manual,44.604,-110.534,"Wyoming","Yellowstone – Hayden Valley","Yearling male trailing bison herd.",true),
            // USA – bears
            R(b03,rangerUS,-50,ReportType.Sighting,SightingSource.Manual,44.521,-110.831,"Wyoming","Yellowstone – South Entrance","Grizzly 399 with 2 cubs near roadside.",true),
            R(b03,resrchCA,-18,ReportType.Sighting,SightingSource.Sensor,44.533,-110.798,"Wyoming","Yellowstone – Lewis Lake","GPS: 399 moved 14 km overnight. Both cubs present.",true),
            R(b04,rangerUS,-35,ReportType.Injury,SightingSource.Manual,44.661,-110.531,"Wyoming","Yellowstone – Hayden Valley","Grizzly 610 with minor flank laceration. Healing well.",true),
            R(b05,rangerUS,-20,ReportType.Sighting,SightingSource.CameraTrap,44.730,-110.382,"Wyoming","Yellowstone – Trout Lake","Subadult grizzly 863 fishing alone at outlet.",true),
            R(b06,rangerUS,-65,ReportType.Sighting,SightingSource.Manual,57.512,-153.982,"Alaska","Kodiak National Wildlife Refuge","Dominant male at salmon run. Fought off 2 rivals.",true),
            // USA – other
            R(d03,rangerUS,-10,ReportType.DangerousBehavior,SightingSource.Manual,41.032,-74.121,"New York","Hudson Valley State Forest","Deer grazing on residential gardens. No injury.",true),
            R(d04,rangerUS,-27,ReportType.Sighting,SightingSource.Manual,38.122,-111.131,"Utah","Capitol Reef NP","Large mule deer buck in velvet near visitor centre.",true),
            R(bi1,rangerUS,-16,ReportType.Sighting,SightingSource.Drone,48.342,-121.841,"Washington","Skagit River Bald Eagle Area","Mated pair on nest. 2 eggs visible via drone.",true),
            R(bi2,rangerUS,-42,ReportType.Sighting,SightingSource.Manual,36.054,-112.194,"Arizona","Grand Canyon South Rim","Condor AC-8 soaring above rim. Wing tags confirmed.",true),
            // Canada
            R(w08,resrchCA,-48,ReportType.Sighting,SightingSource.Sensor,51.178,-115.571,"Alberta","Banff NP – Bow Valley","GPS: lone wolf crossing Trans-Canada at wildlife overpass.",true),
            R(w08,resrchCA,-12,ReportType.Sighting,SightingSource.CameraTrap,51.228,-115.924,"Alberta","Banff NP – Johnston Canyon","Camera at wolf gate: lone male, good body condition.",false),
            R(w09,resrchCA,-38,ReportType.Sighting,SightingSource.Manual,52.004,-128.042,"British Columbia","Great Bear Rainforest","Coastal wolf fishing sockeye at river mouth. 3 others.",true),
            R(b07,resrchCA,-25,ReportType.Sighting,SightingSource.CameraTrap,51.411,-115.921,"Alberta","Banff NP – Lake Louise","Grizzly at wildlife crossing #3. Ear tag confirmed.",true),
            R(b08,resrchCA,-31,ReportType.Sighting,SightingSource.Manual,52.893,-128.521,"British Columbia","Princess Royal Island","Spirit bear fishing alone at estuary. Unique sighting.",true),
            R(mo1,resrchCA,-28,ReportType.Sighting,SightingSource.Manual,51.188,-115.607,"Alberta","Banff NP – Vermilion Lakes","Bull moose feeding on aquatic vegetation. Velvet on.",true),
            R(mo2,resrchCA,-14,ReportType.Sighting,SightingSource.Manual,53.919,-122.758,"British Columbia","Prince George area","Cow moose with twin calves in forest clearing.",true),
            R(l03,resrchCA,-19,ReportType.Sighting,SightingSource.Sensor,60.812,-135.201,"Yukon","Whitehorse surroundings","GPS collar: female moved 22 km in 48h. Active foraging.",true),
            R(l04,resrchCA,-9, ReportType.Sighting,SightingSource.CameraTrap,46.041,-81.399,"Ontario","Algonquin Provincial Park","Subadult lynx at trail camera. High snowshoe hare density.",false),
            // Norway & Sweden
            R(w10,rangerNO,-70,ReportType.Sighting,SightingSource.Sensor,61.441,12.158,"Innlandet","Kynna wolf territory","GPS: cross-border movement into Sweden confirmed.",true),
            R(w10,rangerNO,-25,ReportType.DangerousBehavior,SightingSource.Manual,61.388,12.214,"Innlandet","Kynna – farm boundary","Two sheep killed. Wolf tracks confirm Kynna male.",true),
            R(w11,rangerNO,-34,ReportType.Sighting,SightingSource.Drone,61.022,11.881,"Innlandet","Slettås territory","Drone: female with 5 pups at den. Good condition.",true),
            R(w12,rangerNO,-50,ReportType.Sighting,SightingSource.Sensor,61.688,14.921,"Dalarna","Galven Pack territory","GPS telemetry: pack of 11, all collared adults healthy.",true),
            R(mo3,rangerNO,-14,ReportType.Sighting,SightingSource.Manual,62.193,11.882,"Innlandet","Femunden NP","Bull moose at lake shore. Antlers fully grown.",true),
            R(mo4,rangerNO,-6, ReportType.Sighting,SightingSource.Manual,60.801,11.401,"Innlandet","Hedmark county","Cow with single calf. Calf appears healthy.",true),
            R(mo5,rangerNO,-21,ReportType.DangerousBehavior,SightingSource.Manual,60.512,14.211,"Dalarna","E45 highway corridor","Bull moose near road at dusk. Signage reviewed.",true),
            R(b09,rangerNO,-44,ReportType.Sighting,SightingSource.Manual,63.122,13.801,"Jämtland","Jämtland county forest","Adult male bear seen digging for rodents.",true),
            R(b10,rangerNO,-17,ReportType.Sighting,SightingSource.CameraTrap,60.812,14.311,"Dalarna","Dalarna county forest","Female with 3 cubs at salt lick. Remarkable litter.",true),
            // Russia
            R(b11,resrchRU,-80,ReportType.Sighting,SightingSource.Manual,51.462,157.033,"Kamchatka Krai","Kronotsky Reserve","480 kg male dominant at Kurilskoye salmon run.",true),
            R(b11,resrchRU,-38,ReportType.Sighting,SightingSource.CameraTrap,51.498,157.071,"Kamchatka Krai","Kurilskoye Lake","Dominant male displacing 3 others at fishing spot.",true),
            R(b12,resrchRU,-62,ReportType.Death,SightingSource.Manual,51.389,156.922,"Kamchatka Krai","Kronotsky – south sector","Carcass: gunshot wound. Poaching suspected.",true,false,true),
            R(b13,resrchRU,-29,ReportType.Sighting,SightingSource.Sensor,51.511,157.104,"Kamchatka Krai","Kronotsky Reserve","Subadult female collar active. Moved 8 km overnight.",true),
            R(w13,resrchRU,-52,ReportType.Sighting,SightingSource.Sensor,65.411,130.042,"Yakutia","Lena River basin","Alpha male GPS ping. Pack of ~14 in boreal forest.",true),
            R(w14,resrchRU,-39,ReportType.Sighting,SightingSource.CameraTrap,50.311,87.911,"Altai Republic","Altai Mountains – Kosh-Agach","Adult male photographed at mountain pass. Good condition.",true),
            // Germany & Switzerland
            R(l05,rangerDE,-42,ReportType.Sighting,SightingSource.CameraTrap,47.998,8.154,"Baden-Württemberg","Black Forest – Titisee","Hilde on forest trail at 03:40. Body condition good.",true),
            R(l05,resrchPL,-11,ReportType.Sighting,SightingSource.Sensor,47.942,8.223,"Baden-Württemberg","Black Forest – Feldberg","Collar ping at Feldberg. Unusual high-altitude movement.",false),
            R(l06,rangerDE,-28,ReportType.Sighting,SightingSource.CameraTrap,51.722,10.544,"Lower Saxony","Harz NP","First confirmed Harz lynx sighting in 3 months.",true),
            R(l07,rangerDE,-19,ReportType.Sighting,SightingSource.Sensor,47.199,7.001,"Jura","Swiss-French border zone","Collar ping 4 km inside France. Range confirmed.",true),
            R(fo1,rangerDE,-6, ReportType.Sighting,SightingSource.Manual,47.877,8.151,"Baden-Württemberg","Titisee village edge","Juvenile fox near campsite bins. Deterred.",true),
            R(fo2,rangerDE,-12,ReportType.Sighting,SightingSource.Manual,52.521,13.412,"Berlin","Tempelhof Park","Urban female with 4 cubs at park den.",true),
            R(d05,rangerDE,-18,ReportType.Sighting,SightingSource.Manual,47.801,11.991,"Bavaria","Agricultural fringe, Rosenheim","Buck grazing at field edge at dusk.",true),
            R(bo3,rangerDE,-31,ReportType.Sighting,SightingSource.Sensor,52.411,14.012,"Brandenburg","Brandenburg mixed forest","GPS collar: large boar, 2 km from farmland.",true),
            // Spain
            R(l08,resrchES,-21,ReportType.Sighting,SightingSource.CameraTrap,37.021,-6.312,"Andalucía","Doñana NP","Adult Iberian lynx male on trail camera. Excellent condition.",true),
            R(l09,resrchES,-14,ReportType.Sighting,SightingSource.Manual,38.211,-4.921,"Jaén","Sierra Morena","Female with 3 kittens at den site. All healthy.",true),
            R(w15,resrchES,-37,ReportType.Sighting,SightingSource.Sensor,43.312,-4.711,"Cantabria","Picos de Europa NP","GPS: male wolf moving south toward Duero river.",true),
            R(w16,resrchES,-25,ReportType.DangerousBehavior,SightingSource.Manual,42.811,-7.921,"Galicia","Ourense province","Pack killed 3 sheep overnight. Livestock guardian dogs absent.",true),
            R(d06,rangerPL,-10,ReportType.Sighting,SightingSource.Manual,37.011,-6.381,"Andalucía","Coto Doñana","Large herd of ~30 red deer grazing at dawn.",true),
            R(bo4,resrchES,-33,ReportType.Sighting,SightingSource.Manual,39.411,-6.221,"Extremadura","Monfragüe NP dehesa","Large boar at waterhole. 130 kg estimated.",true),
            // Romania
            R(b14,resrchPL,-58,ReportType.DangerousBehavior,SightingSource.Manual,45.647,25.606,"Brașov County","Bucegi NP – Brașov outskirts","Bear raiding chicken coops. Deterrence fired.",true),
            R(b14,resrchPL,-17,ReportType.Sighting,SightingSource.CameraTrap,45.589,25.441,"Brașov County","Bucegi NP – core zone","Bear back in forest zone after deterrence. Positive.",true),
            R(b15,resrchPL,-40,ReportType.Sighting,SightingSource.Drone,45.411,25.011,"Prahova County","Bucegi NP","Female with 2 cubs at high-altitude meadow.",true),
            R(w17,resrchPL,-44,ReportType.Sighting,SightingSource.Drone,45.402,25.471,"Prahova County","Bucegi NP – high ridge","Alpha female with 4 pups at den. Pack of 9.",true),
            R(w17,rangerNO,-9, ReportType.Sighting,SightingSource.Sensor,45.418,25.509,"Prahova County","Bucegi NP","Female moved 18 km overnight. Active hunting run.",true),
            R(w18,resrchPL,-24,ReportType.Sighting,SightingSource.Sensor,45.311,22.891,"Hunedoara County","Retezat NP","GPS: disperser establishing new home range.",false),
            // Italy
            R(w19,rangerDE,-31,ReportType.Sighting,SightingSource.CameraTrap,41.921,13.891,"Abruzzo","Abruzzo, Lazio and Molise NP","Pack of 7 at forest camera. All appear healthy.",true),
            R(w20,rangerDE,-16,ReportType.Sighting,SightingSource.Sensor,46.511,10.511,"South Tyrol","Stelvio NP","Female collar crossing into Switzerland. Range updated.",true),
            R(bo5,rangerDE,-11,ReportType.Sighting,SightingSource.Manual,42.591,11.191,"Tuscany","Maremma Regional Park","Large boar at mudhole. 120 kg estimated.",true),
            // Kenya
            R(bi3,rangerKE,-33,ReportType.Sighting,SightingSource.Manual,-1.508,35.142,"Narok County","Maasai Mara NR","Secretary bird on nest. 2 eggs confirmed.",true),
            R(bi4,rangerKE,-19,ReportType.Sighting,SightingSource.Manual,-1.411,35.021,"Narok County","Maasai Mara NR","Martial eagle soaring, carrying monitor lizard prey.",true),
            R(ot1,rangerKE,-19,ReportType.DangerousBehavior,SightingSource.Manual,-1.523,35.188,"Narok County","Mara River zone","Honey badger raided tourist camp. Food storage secured.",true),
            R(ot2,rangerKE,-41,ReportType.Sighting,SightingSource.Sensor,-0.219,36.812,"Laikipia Plateau","Ol Pejeta Conservancy","GPS: wild dog pack of 12 moved 35 km in 24h.",true),
            R(ot2,rangerKE,-7, ReportType.Sighting,SightingSource.CameraTrap,-0.231,36.791,"Laikipia Plateau","Ol Pejeta Conservancy","Camera: pups visible at den entrance. 7 pups.",true),
            // South Africa
            R(ot3,rangerZA,-28,ReportType.Sighting,SightingSource.CameraTrap,-23.911,31.421,"Limpopo","Kruger NP – Letaba","Leopard male at camera trap. Est. 70 kg.",true),
            R(ot4,rangerZA,-17,ReportType.Sighting,SightingSource.Sensor,-28.011,32.021,"KwaZulu-Natal","Hluhluwe-iMfolozi Park","Alpha male collar: pack hunted impala. 3 kills.",true),
            R(bi5,rangerZA,-45,ReportType.Injury,SightingSource.Manual,-29.411,29.211,"KwaZulu-Natal","Drakensberg – Giant's Castle","Cape vulture grounded. Wing fracture, power-line collision.",true),
            R(bi6,rangerZA,-11,ReportType.Sighting,SightingSource.Manual,-29.511,29.412,"KwaZulu-Natal","Drakensberg – Cathedral Peak","Verreaux's eagle pair at nest. 1 eaglet visible.",true),
            // India
            R(ot5,resrchIN,-22,ReportType.Sighting,SightingSource.CameraTrap,26.011,76.391,"Rajasthan","Ranthambore Tiger Reserve","T-19 with 4 cubs at camera trap 3A. All healthy.",true),
            R(ot5,resrchIN,-7, ReportType.Sighting,SightingSource.Sensor,26.018,76.411,"Rajasthan","Ranthambore Tiger Reserve","GPS collar: tigress hunted sambar. 3 cubs ate.",true),
            R(ot6,resrchIN,-36,ReportType.Sighting,SightingSource.CameraTrap,32.211,78.051,"Himachal Pradesh","Spiti Valley – Pin Valley NP","Snow leopard male photographed at 4,200m.",true),
            R(b16,resrchIN,-18,ReportType.Sighting,SightingSource.Manual,21.711,79.211,"Madhya Pradesh","Pench Tiger Reserve","Adult sloth bear foraging on termite mounds.",true),
            R(b17,resrchIN,-29,ReportType.Sighting,SightingSource.CameraTrap,31.512,77.711,"Himachal Pradesh","Great Himalayan NP","Female with 2 cubs at high altitude. ~3,600m.",true),
            // Japan
            R(b18,resrchJP,-24,ReportType.Sighting,SightingSource.Manual,44.011,145.211,"Hokkaido","Shiretoko NP – Rausu","Brown bear fishing at river. ~200 kg, good condition.",true),
            R(fo3,resrchJP,-11,ReportType.Sighting,SightingSource.CameraTrap,43.511,144.201,"Hokkaido","Akan-Mashu NP","Ezo fox with 3 cubs at den. Healthy.",true),
            R(d07,resrchJP,-15,ReportType.Sighting,SightingSource.Drone,43.691,144.921,"Hokkaido","Eastern Hokkaido","Drone count: 47 sika deer in agricultural field. Cull justified.",true),
            // Brazil
            R(ot7,resrchBR,-33,ReportType.Sighting,SightingSource.Sensor,-17.511,-57.321,"Mato Grosso do Sul","Pantanal – Porto Jofre","GPS: jaguar male, 120 kg. Territory 90 km².",true),
            R(ot7,resrchBR,-9, ReportType.Sighting,SightingSource.CameraTrap,-17.491,-57.288,"Mato Grosso do Sul","Pantanal – Transpantaneira","Camera: jaguar stalking caiman. Unsuccessful hunt.",true),
            R(ot8,resrchBR,-21,ReportType.Sighting,SightingSource.Manual,-18.211,-52.901,"Goiás","Emas NP","Giant anteater foraging at termite mound. Radio tag active.",true),
            R(ot9,resrchBR,-14,ReportType.Sighting,SightingSource.CameraTrap,-20.081,-46.521,"Minas Gerais","Serra da Canastra NP","Maned wolf female with 3 pups at camera.",true),
            // Australia
            R(ot10,rangerAU,-38,ReportType.Injury,SightingSource.Manual,-41.511,146.211,"Tasmania","Narawntapu NP","Tasmanian devil with visible DFTD tumour on face.",true),
            R(ot11,rangerAU,-22,ReportType.DangerousBehavior,SightingSource.Manual,-25.311,153.121,"Queensland","Fraser Island (K'gari)","Dingo pack leader approached beach campers. Managed.",true),
            R(fo4,rangerAU,-16,ReportType.Sighting,SightingSource.CameraTrap,-30.411,131.291,"South Australia","Bilby habitat – Roxby Downs","Invasive fox near bilby warren. Baiting programme activated.",true),
            R(bi7,rangerAU,-9, ReportType.Sighting,SightingSource.Manual,-36.811,147.211,"Victoria","Alpine NP – Mt Hotham","Wedge-tailed eagle pair nesting on cliff. 2 eggs.",true),
            // Mongolia & Arctic
            R(ot12,resrchRU,-41,ReportType.Sighting,SightingSource.Sensor,49.211,88.421,"Bayan-Ölgii","Altai Tavan Bogd NP","GPS: snow leopard female, 4,100m. 2 cubs confirmed.",true),
            R(w21,resrchRU,-27,ReportType.Sighting,SightingSource.Sensor,44.811,101.911,"Gobi-Altai","Gobi-Altai NP","GPS pack of 14 in steppe. Huge range: >2,000 km².",true),
            R(fo5,rangerNO,-12,ReportType.Sighting,SightingSource.Manual,66.411,-22.511,"Westfjords","Hornstrandir NR","Arctic fox female with 6 pups. Den in lava field.",true),
            R(fo6,rangerNO,-8, ReportType.Sighting,SightingSource.CameraTrap,66.384,-22.498,"Westfjords","Hornstrandir NR","Blue-phase male photographed near coast. Rare morph.",true),
            R(bi8,rangerNO,-19,ReportType.Sighting,SightingSource.Manual,68.211,13.901,"Nordland","Lofoten Islands","White-tailed eagle pair nesting. 1 chick visible.",true),
            // UK & France
            R(d08,rangerNO,-24,ReportType.Sighting,SightingSource.Manual,57.012,-3.712,"Highland","Cairngorms NP","Dominant stag, 14-point rack. Herd of 22.",true),
            R(d09,rangerDE,-13,ReportType.Sighting,SightingSource.Manual,50.881,-1.612,"Hampshire","New Forest NP","Fallow doe herd of 18. Managed grazing.",true),
            R(fo7,rangerDE,-5, ReportType.Sighting,SightingSource.Manual,51.541,-0.042,"London","Hackney Marshes","Urban fox female with 5 cubs observed from bridge.",true),
            R(d10,rangerDE,-21,ReportType.Sighting,SightingSource.Manual,48.211,7.121,"Alsace","Vosges Regional NP","Roe deer buck in velvet at forest edge.",true),
            R(bo6,rangerDE,-31,ReportType.Sighting,SightingSource.Manual,49.711,5.211,"Ardennes","Ardennes cross-border forest","Large boar, 120 kg. Forest-field interface.",true),
            R(w22,resrchES,-28,ReportType.Sighting,SightingSource.Sensor,44.091,7.211,"PACA","Mercantour NP","Alpha female collar active. Pack of 8 adults.",true),
            // Borneo
            R(b19,resrchIN,-35,ReportType.Sighting,SightingSource.CameraTrap,5.011,117.821,"Sabah","Danum Valley CA","Sun bear on camera. Smallest bear species, ~55 kg.",true),
            R(ot13,resrchIN,-18,ReportType.Sighting,SightingSource.CameraTrap,4.811,116.991,"Sabah","Maliau Basin CA","Clouded leopard female on camera at 02:11.",true),
        };

        context.SightingReports.AddRange(reports);
        await context.SaveChangesAsync();

        // Update last-seen on all animals
        var lastByAnimal = reports
            .GroupBy(r => r.AnimalId)
            .ToDictionary(g => g.Key, g => g.Max(r => r.ObservedAtUtc));

        foreach (var animal in context.Animals.Local)
        {
            if (lastByAnimal.TryGetValue(animal.Id, out var ts))
                animal.MarkSeen(ts);
        }
        await context.SaveChangesAsync();

        // ── Observation Notes ─────────────────────────────────────────────────

        var rByIdx = reports; // alias for brevity
        Note(context, rByIdx[0],  rangerPL,  "Pack in excellent condition. All 4 adults healthy. No signs of disease.");
        Note(context, rByIdx[0],  resrchPL,  "Camera trap SD cards swapped. Previous footage shows pack using sector B3 regularly.");
        Note(context, rByIdx[1],  resrchPL,  "Alpha male estimated 42 kg. Coat clean, collar signal strong on 433 MHz.");
        Note(context, rByIdx[3],  rangerPL,  "Local municipality notified. Livestock owners advised to pen animals before dusk.");
        Note(context, rByIdx[5],  rangerPL,  "Snare fragments found 200m away. Illegal trap reported to police.");
        Note(context, rByIdx[5],  resrchPL,  "Vet assessment: wound infected but not life-threatening. Self-healing likely in 3-4 weeks.");
        Note(context, rByIdx[7],  rangerPL,  "Snare trap reported to law enforcement. Injury consistent with wire snare.");
        Note(context, rByIdx[7],  resrchPL,  "Bear still foraging — positive sign. Will monitor weekly via drone.");
        Note(context, rByIdx[9],  resrchPL,  "Female in very good condition. Cubs estimated 6 months old. Both mobile and alert.");
        Note(context, rByIdx[14], rangerPL,  "ASF field test pending. Boar quarantined in monitoring zone B.");
        Note(context, rByIdx[16], rangerUS,  "Junction Butte pack count updated: 9 individuals, 3 pups. Strongest season in 5 years.");
        Note(context, rByIdx[17], rangerUS,  "Kill at grid LL-77. Carcass left as per protocol. Ravens arrived within 20 min.");
        Note(context, rByIdx[20], rangerUS,  "Crowd control deployed. 399 and cubs stayed calm at ~200 m from road.");
        Note(context, rByIdx[20], resrchCA,  "399 showing remarkable vehicle tolerance. Both cubs displaying normal development.");
        Note(context, rByIdx[21], resrchCA,  "14 km overnight range typical of pre-denning foraging. Battery at 61%.");
        Note(context, rByIdx[22], rangerUS,  "Laceration ~8 cm, healing without intervention. Suspected territorial fight.");
        Note(context, rByIdx[24], rangerUS,  "Dominant male displaced 2 rivals. Caught 11 salmon in 3-hour observation window.");
        Note(context, rByIdx[29], resrchCA,  "Wildlife overpass #4 used for 3rd confirmed time this year. Infrastructure working.");
        Note(context, rByIdx[31], resrchCA,  "Coastal wolf diet analysis: 60% fish in summer months per scat analysis.");
        Note(context, rByIdx[34], resrchCA,  "Spirit bear sighting logged with Gitga'at First Nation guardians. Very rare event.");
        Note(context, rByIdx[35], resrchCA,  "Bull in rut approach. Antler velvet shedding this week. Will be aggressive.");
        Note(context, rByIdx[38], resrchCA,  "Lynx GPS data shows tight overlap with snowshoe hare population peaks.");
        Note(context, rByIdx[39], rangerNO,  "GPS coordinates shared with Swedish Naturvårdsverket per cross-border protocol.");
        Note(context, rByIdx[40], rangerNO,  "Farmers compensated. Temporary electric fence installed. Third incident this season.");
        Note(context, rByIdx[40], resrchRU,  "Third livestock kill from Kynna pack. Population management review requested.");
        Note(context, rByIdx[41], rangerNO,  "Pup survival excellent this year. 5 pups from single litter — exceptional.");
        Note(context, rByIdx[47], resrchRU,  "Dominant male. Had exclusive access to prime fishing pool for full 2-hour watch.");
        Note(context, rByIdx[48], resrchRU,  "Camera footage submitted to WWF Kamchatka bear population database.");
        Note(context, rByIdx[49], resrchRU,  "Carcass samples sent to Petropavlovsk lab. Anti-poaching patrol frequency doubled.");
        Note(context, rByIdx[50], resrchRU,  "Subadult female in good condition. Will attempt satellite collar upgrade next visit.");
        Note(context, rByIdx[53], rangerDE,  "Hilde's range expanded 30% since last year. Now spans 3 forest districts.");
        Note(context, rByIdx[54], resrchPL,  "High-altitude movement unusual this season. Possible prey scarcity below treeline.");
        Note(context, rByIdx[55], rangerDE,  "First Harz lynx camera image in 3 months. Confirms resident individual.");
        Note(context, rByIdx[57], rangerDE,  "Urban fox family under observation. Cubs visible from 18:30 nightly.");
        Note(context, rByIdx[62], resrchES,  "Iberian lynx population now 1,100+ individuals. Remarkable recovery from 94 in 2002.");
        Note(context, rByIdx[63], resrchES,  "3 kittens healthy. Mother's home range: 18 km². Good prey base (rabbits abundant).");
        Note(context, rByIdx[65], resrchES,  "Galician pack responsible for 23 livestock kills this season. Compensation paid.");
        Note(context, rByIdx[67], rangerKE,  "Secretary bird nest at GPS MM-2241. Chick hatched 2 weeks after this report.");
        Note(context, rByIdx[68], rangerKE,  "Martial eagle: largest eagle in Africa, wingspan ~188 cm. Schedules daily patrol.");
        Note(context, rByIdx[69], rangerKE,  "Camp briefed on food storage. Steel-box lockers provided to all guides.");
        Note(context, rByIdx[70], rangerKE,  "Wild dog pack range: 35 km/day is typical. Most wide-ranging carnivore in Africa.");
        Note(context, rByIdx[71], rangerKE,  "7 pups at den — excellent breeding success. Now 19 individuals in pack.");
        Note(context, rByIdx[72], rangerZA,  "Leopard camera trap data merged with Kruger-wide leopard density survey.");
        Note(context, rByIdx[73], rangerZA,  "Wild dog pack hunting success rate 80% — highest of all large African predators.");
        Note(context, rByIdx[74], rangerZA,  "Vulture admitted to Bird of Prey Programme, Johannesburg. Recovery expected 6 weeks.");
        Note(context, rByIdx[76], resrchIN,  "T-19 Krishna is Ranthambore's most-studied tigress. 4th litter in 7 years.");
        Note(context, rByIdx[77], resrchIN,  "Cubs eating solid food. Estimated 4 months old. GPS collar battery 44%.");
        Note(context, rByIdx[78], resrchIN,  "Snow leopard at 4,200 m — above typical range. Climate shift pushing prey upward.");
        Note(context, rByIdx[81], resrchJP,  "Sika deer overabundance causing significant forest regeneration damage in Hokkaido.");
        Note(context, rByIdx[82], resrchBR,  "Jaguar territory overlap with cattle ranch. Conflict mitigation programme underway.");
        Note(context, rByIdx[84], resrchBR,  "Giant anteater radio tag battery 60%. Range: 1,400 ha. 3 ant species preferred.");
        Note(context, rByIdx[85], resrchBR,  "Maned wolf pups healthy. Frugivore diet unusual for canid — eats lobeira fruit.");
        Note(context, rByIdx[86], rangerAU,  "DFTD tumour stage 2. Animal enrolled in insurance population programme.");
        Note(context, rByIdx[87], rangerAU,  "Fraser Island dingo management protocol reviewed after previous fatal incident 2001.");
        Note(context, rByIdx[88], rangerAU,  "Fox incursion 3rd this season near bilby habitat. Baiting successful last 2 years.");
        Note(context, rByIdx[90], resrchRU,  "Snow leopard with 2 cubs — highest reproductive output recorded at this site.");
        Note(context, rByIdx[93], rangerNO,  "Arctic fox 6-pup litter — exceptional. Average is 4. High lemming year.");
        Note(context, rByIdx[96], rangerNO,  "White-tailed eagle chick fledged 8 weeks after this report. Both adults feeding.");
        Note(context, rByIdx[99], resrchES,  "Mercantour pack re-established after 15-year absence. 8 adults, no pups yet this year.");
        Note(context, rByIdx[100],resrchIN,  "Sun bear: endangered. Danum Valley has highest density in Borneo (~8 per 100 km²).");
        Note(context, rByIdx[101],resrchIN,  "Clouded leopard extremely cryptic. This camera trap image is first in 14 months.");

        await context.SaveChangesAsync();
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static AppUser User(string first, string last, string email, string pass, UserRole role) =>
        new(first, last, email, BCrypt.Net.BCrypt.HashPassword(pass), role);

    private static Animal A(string id, string name, Species species, AnimalHealthStatus health, string? desc) =>
        new(id, name, species, health, desc);

    private static SightingReport R(
        Animal animal, AppUser reporter,
        int daysAgo, ReportType type, SightingSource source,
        double lat, double lon, string region, string district,
        string desc,
        bool approve = false, bool reject = false, bool resolve = false)
    {
        var report = new SightingReport(
            animal.Id, reporter.Id,
            DateTime.UtcNow.AddDays(daysAgo),
            type, source,
            new LocationDetails(new Coordinates(lat, lon), region, district),
            desc);

        if (approve)  report.Approve();
        if (reject)   report.Reject();
        if (resolve)  { if (report.Status != ReportStatus.Verified) report.Approve(); report.Resolve(); }
        return report;
    }

    private static void Note(AppDbContext ctx, SightingReport report, AppUser author, string content) =>
        ctx.ObservationNotes.Add(new ObservationNote(report.Id, author.Id, content));
}
