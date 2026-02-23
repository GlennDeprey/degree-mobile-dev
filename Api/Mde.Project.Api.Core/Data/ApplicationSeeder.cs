using Mde.Project.Api.Core.Entities.Products;
using Mde.Project.Api.Core.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mde.Project.Api.Core.Entities.Warehouses;

namespace Mde.Project.Api.Core.Data
{
    public class ApplicationSeeder
    {
        private readonly ModelBuilder _modelBuilder;
        public ApplicationSeeder(ModelBuilder modelBuilder) 
        {
            _modelBuilder = modelBuilder;
        }

        public void Seed()
        {
            #region Identity

            var roles = new List<IdentityRole>
            {
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "Customer", NormalizedName = "CUSTOMER" }
            };

            var userPassword = "Test123?";
            var users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    UserName="siegfried.derdeyn@howest.be",
                    NormalizedUserName="SIEGFRIED.DERDEYN@HOWEST.BE",
                    Email="siegfried.derdeyn@howest.be",
                    NormalizedEmail="SIEGFRIED.DERDEYN@HOWEST.BE",
                    EmailConfirmed=true,
                    FirstName="Siegfried",
                    LastName="Derdeyn",
                },
                new ApplicationUser
                {
                    UserName="glenn.deprey@student.howest.be",
                    NormalizedUserName="GLENN.DEPREY@STUDENT.HOWEST.BE",
                    Email="glenn.deprey@student.howest.be",
                    NormalizedEmail="GLENN.DEPREY@STUDENT.HOWEST.BE",
                    EmailConfirmed=true,
                    FirstName="Glenn",
                    LastName="Deprey",
                }
            };

            users.ForEach(user => user.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user, userPassword));

            var userRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string> { UserId = users[0].Id, RoleId = roles[0].Id },
                new IdentityUserRole<string> { UserId = users[1].Id, RoleId = roles[1].Id },
                new IdentityUserRole<string> { UserId = users[1].Id, RoleId = roles[0].Id }
            };

            _modelBuilder.Entity<IdentityRole>().HasData(roles);
            _modelBuilder.Entity<ApplicationUser>().HasData(users);
            _modelBuilder.Entity<IdentityUserRole<string>>().HasData(userRoles);

            #endregion
            #region Product Taxes
            var productTaxes = new List<ProductTax>
            {
                new ProductTax { Id = Guid.NewGuid(), Name = "0%", TaxRate = 0, CreatedOn = DateTime.Now },
                new ProductTax { Id = Guid.NewGuid(), Name = "6%", TaxRate = 0.06, CreatedOn = DateTime.Now },
                new ProductTax { Id = Guid.NewGuid(), Name = "12%", TaxRate = 0.12, CreatedOn = DateTime.Now },
                new ProductTax { Id = Guid.NewGuid(), Name = "21%", TaxRate = 0.21, CreatedOn = DateTime.Now }
            };
            #endregion
            #region Product Brands
            var brands = new List<Brand>
            {
                new Brand { Id = Guid.NewGuid(), Name = "Albert Heijn", CreatedOn = DateTime.Now },
                new Brand { Id = Guid.NewGuid(), Name = "LU", CreatedOn = DateTime.Now },
                new Brand { Id = Guid.NewGuid(), Name = "Purina One", CreatedOn = DateTime.Now },
                new Brand { Id = Guid.NewGuid(), Name = "Van Wijngaarden", CreatedOn = DateTime.Now },
                new Brand { Id = Guid.NewGuid(), Name = "Dreft", CreatedOn = DateTime.Now },
                new Brand { Id = Guid.NewGuid(), Name = "Pampers", CreatedOn = DateTime.Now },
                new Brand { Id = Guid.NewGuid(), Name = "Axe", CreatedOn = DateTime.Now },
                new Brand { Id = Guid.NewGuid(), Name = "Dash", CreatedOn = DateTime.Now },
                new Brand { Id = Guid.NewGuid(), Name = "Lea & Perrins", CreatedOn = DateTime.Now },
            };
            #endregion
            #region Product Categories
            var categories = new List<ProductCategory>
            {
                new ProductCategory { Id = Guid.NewGuid(), Name = "Voeding", CreatedOn = DateTime.Now },
                new ProductCategory { Id = Guid.NewGuid(), Name = "Verzorging", CreatedOn = DateTime.Now },
                new ProductCategory { Id = Guid.NewGuid(), Name = "Huisdieren", CreatedOn = DateTime.Now },
                new ProductCategory { Id = Guid.NewGuid(), Name = "Huishouden", CreatedOn = DateTime.Now },
                new ProductCategory { Id = Guid.NewGuid(), Name = "Baby en Kind", CreatedOn = DateTime.Now },
            };
            #endregion
            #region Products
            var products = new List<Product>
            {
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "AH Witlof los (kg)",
                    BrandId = brands[0].Id,
                    Image = "103114cc-50b0-40c4-aec5-3621d29b6cfc.jpg",
                    Description = "Let op: dit product wordt in de winkel gewogen om de exacte prijs vast te stellen",
                    SalesPrice = 1.17m,
                    SalesTaxId = productTaxes[1].Id,
                    CategoryId = categories[0].Id,
                    Barcode = "0800345750178",
                    CreatedOn = DateTime.Parse("2025-05-26 15:10:47.9382250"),
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "AH Kattenbakvulling ex hygiene schep&schoon",
                    BrandId = brands[0].Id,
                    Image = "5507ede3-f158-4a90-be22-bb80dd8d0cba.jpg",
                    Description = "De AH Schep & Schoon Super Hygiëne kattenbakvulling is een fijne kattenbakvulling, verrijkt met blue bit korrels waardoor de hardnekkige geurtjes van urine én uitwerpselen worden ingesloten.",
                    SalesPrice = 3.15m,
                    SalesTaxId = productTaxes[3].Id,
                    CategoryId = categories[2].Id,
                    Barcode = "8856729110016",
                    CreatedOn = DateTime.Parse("2025-05-26 15:14:15.8802693"),
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Purina ONE Adult rijk aan zalm",
                    BrandId = brands[2].Id,
                    Image = "d7903db8-4519-4b46-9dc9-6f6516784699.jpg",
                    Description = "Purina one adult kattenvoer rijk aan zalm is speciaal afgestemd op volwassen katten (van 1 jaar en ouder)",
                    SalesPrice = 4.70m,
                    SalesTaxId = productTaxes[3].Id,
                    CategoryId = categories[2].Id,
                    Barcode = "7934592776551",
                    CreatedOn = DateTime.Parse("2025-05-26 15:18:21.3117540"),
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "AH Bananen tros",
                    BrandId = brands[0].Id,
                    Image = "db676fc2-134b-42b3-96bb-c3d27063743e.jpg",
                    Description = "De banaan is de bekendste tropische fruitsoort en een echte favoriet. Lekker als tussendoortje of om mee nemen als snack voor onderweg.",
                    SalesPrice = 0.90m,
                    SalesTaxId = productTaxes[1].Id,
                    CategoryId = categories[0].Id,
                    Barcode = "0784244549357",
                    CreatedOn = DateTime.Parse("2025-05-26 15:23:30.6195150"),
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "AH Extra lang lekker tijger wit heel",
                    BrandId = brands[0].Id,
                    Image = "900496d5-eed8-4296-93c9-e1af3451af9c.jpg",
                    Description = "Heerlijk zacht en mals witbrood met een mooie gemelleerde korst.",
                    SalesPrice = 1.80m,
                    SalesTaxId = productTaxes[3].Id,
                    CategoryId = categories[0].Id,
                    Barcode = "3448530513633",
                    CreatedOn = DateTime.Parse("2025-05-26 15:28:49.3085608"),
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "AH Scharrel kipburger 4 stuks",
                    BrandId = brands[0].Id,
                    Image = "8b125098-ac4d-4caa-8e21-a53283ee481b.jpg",
                    Description = "Deze kipburgers zijn al voorgegaard, alleen even bakken in de pan voor een heerlijk knapperig laagje.",
                    SalesPrice = 3.75m,
                    SalesTaxId = productTaxes[3].Id,
                    CategoryId = categories[0].Id,
                    Barcode = "0324255992584",
                    CreatedOn = DateTime.Parse("2025-05-26 19:14:39.7473425"),
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Van Wijngaarden's Zaanse mayonaise",
                    BrandId = brands[3].Id,
                    Image = "576bc590-9fc3-4bc7-8760-71eea0326a7e.jpg",
                    Description = "De authentieke Zaanse mayonaise in handige knijpfles met steropening.",
                    SalesPrice = 3.15m,
                    SalesTaxId = productTaxes[3].Id,
                    CategoryId = categories[0].Id,
                    Barcode = "4370107769466",
                    CreatedOn = DateTime.Parse("2025-05-26 19:18:11.7511596"),
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Lea & Perrins Worcestershire saus",
                    BrandId = brands[8].Id,
                    Image = "ea13d884-f19c-4b17-b6f8-d80f5710cb75.jpg",
                    Description = "Lea en perrins worcestershire sauce is een zuur-pittige tafelsaus voor vlees- en visgerechten en is met haar uitgesproken smaak sinds 1837 een van de meest invloedrijke sauzen in de wereldkeuken.",
                    SalesPrice = 1.96m,
                    SalesTaxId = productTaxes[3].Id,
                    CategoryId = categories[0].Id,
                    Barcode = "4844730411660",
                    CreatedOn = DateTime.Parse("2025-05-26 19:19:54.7854121"),
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Dash Poeder original bel",
                    BrandId = brands[7].Id,
                    Image = "7a451181-5686-48e7-a8c0-2268ae40ed72.jpg",
                    Description = "Dash witter dan wit + anti-residu technologie waspoeder biedt briljant schone resultaten, zelfs bij een koud en kort cyclus.",
                    SalesPrice = 1120.00m,
                    SalesTaxId = productTaxes[3].Id,
                    CategoryId = categories[3].Id,
                    Barcode = "5513225762264",
                    CreatedOn = DateTime.Parse("2025-05-26 19:29:23.4985112"),
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Axe Apollo showergel",
                    BrandId = brands[6].Id,
                    Image = "aef8c3f1-a525-4ca2-b4a4-e17e04511a7a.jpg",
                    Description = "Axe apollo 3-in-1 douchegel voor lichaam, gezicht en haar hydrateert en houdt je 12 uur lang fris, met de heerlijke geur van salie en cederhout.",
                    SalesPrice = 2.50m,
                    SalesTaxId = productTaxes[3].Id,
                    CategoryId = categories[1].Id,
                    Barcode = "5222172458266",
                    CreatedOn = DateTime.Parse("2025-05-26 19:51:43.9508221"),
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Axe Black deodorant bodyspray",
                    BrandId = brands[6].Id,
                    Image = "14d0f9b8-8597-4556-b9bb-cf77b8d00ddc.jpg",
                    Description = "Axe black deodorant bodyspray: voor effectieve bescherming en een onweerstaanbare geur.",
                    SalesPrice = 3.80m,
                    SalesTaxId = productTaxes[3].Id,
                    CategoryId = categories[1].Id,
                    Barcode = "1589354243664",
                    CreatedOn = DateTime.Parse("2025-05-26 19:52:54.0399339"),
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Pampers Premium protection luiers maat 6",
                    BrandId = brands[5].Id,
                    Image = "e3bbcf0f-0622-407c-8ffa-7f4ae8cfbd06.jpg",
                    Description = "Pampers n graden1 luiers voor comfort en bescherming van de gevoelige huid.",
                    SalesPrice = 10.30m,
                    SalesTaxId = productTaxes[3].Id,
                    CategoryId = categories[4].Id,
                    Barcode = "0420170197490",
                    CreatedOn = DateTime.Parse("2025-05-26 21:10:22.6010079"),
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "AH Beregoede baby lotiondoekjes sensitive",
                    BrandId = brands[0].Id,
                    Image = "54e83a02-3fb3-45f9-a414-154ccef7f110.jpg",
                    Description = "AH Beregoede Baby lotiondoekjes sensitive​.",
                    SalesPrice = 0.62m,
                    SalesTaxId = productTaxes[3].Id,
                    CategoryId = categories[4].Id,
                    Barcode = "3609655643166",
                    CreatedOn = DateTime.Parse("2025-05-26 22:31:28.2114786"),
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Dreft Afwasmiddel platinum quick wash original",
                    BrandId = brands[4].Id,
                    Image = "96718dcf-3431-428b-9a6d-80fc2e9d5491.jpg",
                    Description = "Dreft platinum quickwash original met vloeibaar afwasmiddel ben je sneller de keuken uit door sneller en moeiteloos schoonmaken.",
                    SalesPrice = 2.37m,
                    SalesTaxId = productTaxes[3].Id,
                    CategoryId = categories[3].Id,
                    Barcode = "9541621865377",
                    CreatedOn = DateTime.Parse("2025-05-26 22:38:52.6648507"),
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "AH Gerookte zalm",
                    BrandId = brands[0].Id,
                    Image = "1c220f9e-3741-4a9e-a968-7eca9010c4ff.jpg",
                    Description = "Deze smakelijk gerookte zalmfilet is lekker op de boterham met verse dille en ideaal voor fijne hapjes, op toastjes of blini's met wat zure room.",
                    SalesPrice = 3.60m,
                    SalesTaxId = productTaxes[3].Id,
                    CategoryId = categories[0].Id,
                    Barcode = "4006381333948",
                    CreatedOn = DateTime.Parse("2025-05-17 16:22:23.4157325"),
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "AH Romige brie 60+",
                    BrandId = brands[0].Id,
                    Image = "78f71df6-f8e4-4407-8132-2e14c818d0cd.jpg",
                    Description = "Franse kaas",
                    SalesPrice = 1.50m,
                    SalesTaxId = productTaxes[3].Id,
                    CategoryId = categories[0].Id,
                    Barcode = "4006381333962",
                    CreatedOn = DateTime.Parse("2025-05-17 16:22:23.4157331"),
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "LU Mikado melk chocolade",
                    BrandId = brands[1].Id,
                    Image = "d17ec364-5dfd-4f5a-96e7-2b0d6bdb7893.jpg",
                    Description = "Dunne, knapperige biscuitstokjes bedekt met heerlijke melkchocolade.",
                    SalesPrice = 165.00m,
                    SalesTaxId = productTaxes[3].Id,
                    CategoryId = categories[0].Id,
                    Barcode = "4006381333931",
                    CreatedOn = DateTime.Parse("2025-05-17 16:22:23.4157320"),
                }
            };
            #endregion
            #region Warehouse Locations
            var warehouseLocations = new List<WarehouseLocationInfo>
            {
                new WarehouseLocationInfo
                {
                    Id = Guid.NewGuid(),
                    Address = "123 Main St",
                    City = "City A",
                    State = "State A",
                    Country = "Country A",
                    PostalCode = "12345",
                    Longitude = 10.0,
                    Latitude = 20.0,
                    CreatedOn = DateTime.Now,
                },
                new WarehouseLocationInfo
                {
                    Id = Guid.NewGuid(),
                    Address = "456 Elm St",
                    City = "City B",
                    State = "State B",
                    Country = "Country B",
                    PostalCode = "67890",
                    Longitude = 30.0,
                    Latitude = 40.0,
                    CreatedOn = DateTime.Now,
                }
            };
            #endregion
            #region Google Location Info
            var googleLocationInfo = new List<WarehouseGoogleInfo>
            {
                new WarehouseGoogleInfo
                {
                    Id = Guid.NewGuid(),
                    GoogleAddress = "Albert Heijn Veurne, Koksijdestraat, Veurne, Belgium",
                    GoogleAddressId = "ChIJucK6Jt6X3EcRHP0v6wEBFlo"
                },
                new WarehouseGoogleInfo
                {
                    Id = Guid.NewGuid(),
                    GoogleAddress = "Albert Heijn Oostende, Torhoutsesteenweg, Ostend, Belgium",
                    GoogleAddressId = "ChIJJakyy0mv3EcRNMCeP-A3ZfA"
                },
            };
            #endregion
            #region Warehouses
            var warehouses = new List<Warehouse>
            {
                new Warehouse
                {
                    Id = Guid.NewGuid(),
                    Name = "AH Veurne",
                    ShortName = "AHBEWV-V",
                    Phone = "058 69 01 00",
                    LocationInfoId = warehouseLocations[0].Id,
                    GoogleInfoId = googleLocationInfo[0].Id,
                    Earnings = 10000,
                    CreatedOn = DateTime.Now
                },
                new Warehouse
                {
                    Id = Guid.NewGuid(),
                    Name = "AH Oostende",
                    ShortName = "AHBEWV-O",
                    Phone = "059 36 95 00",
                    LocationInfoId = warehouseLocations[1].Id,
                    GoogleInfoId = googleLocationInfo[1].Id,
                    Earnings = 20000,
                    CreatedOn = DateTime.Now
                }
            };
            #endregion
            #region Warehouse Photos
            var warehousePhotos = new List<WarehousePhoto>
            {
                new WarehousePhoto
                {
                    Id = Guid.NewGuid(),
                    GoogleInfoId = googleLocationInfo[0].Id,
                    PhotoUri = "https://lh3.googleusercontent.com/place-photos/AJnk2cyV0QKUpAYpc0r7nhWvVWXQjhTklkG-tqr7-cCV0hqw752hTcDFio7v1TS7pieay3dGJyZKE1qcSsYYz2Nc1Ia4MqrzOzCL8yHLJqWcuP0tN1DVvl5OgdNROhD251l2wc6D4J76CDRYqF-9=s4800-w120-h120",
                    CreatedOn = DateTime.Now
                },
                new WarehousePhoto
                {
                    Id = Guid.NewGuid(),
                    GoogleInfoId = googleLocationInfo[0].Id,
                    PhotoUri = "https://lh3.googleusercontent.com/places/ANXAkqFCZz1f5-dL1L3OwpxGrN9BEeYjWba3xb455XIBgW-QpzakxwSeJYon89dJGX_MpRtj5p8LKLpvYkmOH-0fiD8F_OzWOax35zw=s4800-w120-h120",
                    CreatedOn = DateTime.Now
                },
                new WarehousePhoto
                {
                    Id = Guid.NewGuid(),
                    GoogleInfoId = googleLocationInfo[0].Id,
                    PhotoUri = "https://lh3.googleusercontent.com/places/ANXAkqFUatKQMvNT7Um3L6mrHJh_hJ_AVciMztLc_sCwoyLACEx0Z0DyOjY_pBlwX03fiuPqo98dLpnmpOJDX7rjnmUm9nOp8U7nFqw=s4800-w120-h120",
                    CreatedOn = DateTime.Now
                },
                new WarehousePhoto
                {
                    Id = Guid.NewGuid(),
                    GoogleInfoId = googleLocationInfo[0].Id,
                    PhotoUri = "https://lh3.googleusercontent.com/places/ANXAkqF73sx2p_xjCyaSEh-nsT-pYbG0tbuiiW7Iiq680aT7cI4SWiji9zmDVbM6IdT0I66Mn2ILIgaRUT-vHelRV1-QUiQswPYbIAY=s4800-w120-h120",
                    CreatedOn = DateTime.Now
                },
                new WarehousePhoto
                {
                    Id = Guid.NewGuid(),
                    GoogleInfoId = googleLocationInfo[0].Id,
                    PhotoUri = "https://lh3.googleusercontent.com/place-photos/AJnk2cwwQ8ZGAyA3PKUTbPqaXvjBlB0R428Cq1xTCwumhZZlPmbptdHabWJOrD_lwhvFTb6ZtuiVXRz4t_T2Qoc_5jAZHXsJdE4S_sqZO1B9mBem8H5D-euptffcEU7PVAc0-6iC6PAG-2TRX8H9oQ=s4800-w120-h120",
                    CreatedOn = DateTime.Now
                },
                new WarehousePhoto
                {
                    Id = Guid.NewGuid(),
                    GoogleInfoId = googleLocationInfo[0].Id,
                    PhotoUri = "https://lh3.googleusercontent.com/places/ANXAkqH9tKAG77TzGH2pnwZGJLexdPLLYViha-i3LBWX9-8vhnJRqu2IQfMyRK7dvB3J9kzdc2RyhsmASDhs2YfbIyPczCuzDeEHAcU=s4800-w120-h120",
                    CreatedOn = DateTime.Now
                },
                new WarehousePhoto
                {
                    Id = Guid.NewGuid(),
                    GoogleInfoId = googleLocationInfo[0].Id,
                    PhotoUri = "https://lh3.googleusercontent.com/place-photos/AJnk2cyvHuVkb0HN2eVbSYFjJ0Unv-ke4JKVCyqDFvnTZcr1eBdRTbvRZLrnMUOgQ3Ad-wZxQE8bznEWhIzNMsPCj_FGRYxhMxOxZGBDMJmtC-nxGhpMPKL0XQmySiH0tSCPETOCy6eaHF0mYmyaEQ=s4800-w120-h120",
                    CreatedOn = DateTime.Now
                },
                new WarehousePhoto
                {
                    Id = Guid.NewGuid(),
                    GoogleInfoId = googleLocationInfo[0].Id,
                    PhotoUri = "https://lh3.googleusercontent.com/places/ANXAkqEAjmvnI1AiffFG-FOjtcVTZ7wq1GUEPh_1q_KGk8IWVIisol62un2y-RqEEteNQLP5-jx1uLpCBWeDZIyISStK4eOTkvQrZ2Y=s4800-w120-h120",
                    CreatedOn = DateTime.Now
                },
                new WarehousePhoto
                {
                    Id = Guid.NewGuid(),
                    GoogleInfoId = googleLocationInfo[0].Id,
                    PhotoUri = "https://lh3.googleusercontent.com/places/ANXAkqEtayDJrKMJzeuJhaBlX67L0vrSBSEi7atRtbX0fF7mptHrQmAoH12YjPai_iRmdFpaIut8MicT8YVmP4Pxkwn0L5HGro5xhLE=s4800-w120-h120",
                    CreatedOn = DateTime.Now
                },
                new WarehousePhoto
                {
                    Id = Guid.NewGuid(),
                    GoogleInfoId = googleLocationInfo[0].Id,
                    PhotoUri = "https://lh3.googleusercontent.com/places/ANXAkqHD6KsHJ92smoCMkCpvMg66IoEz4tfXsbeWXeHSNXfv_F9YZbM6gm0KQIvGDoEo3g5P_fnJbhK6O2QMP4n4vZuk4Nh_04Ybji8=s4800-w120-h120",
                    CreatedOn = DateTime.Now
                },
                new WarehousePhoto
                {
                    Id = Guid.NewGuid(),
                    GoogleInfoId = googleLocationInfo[1].Id,
                    PhotoUri = "https://lh3.googleusercontent.com/place-photos/AJnk2cx3V5M_RQj8F_Y2WhJ5qg2iLQUibOtC7izUEDVCQ_RD_KFBDop1UANboEM6o21yO7U2RuennHqHomMrBtZkpUm11uGEfmY78WKRTCrzzhyeIAUv7lBPidREKw8WuRWTl3DchP3wkdJmIaVH=s4800-w120-h120",
                    CreatedOn = DateTime.Now
                },
                new WarehousePhoto
                {
                    Id = Guid.NewGuid(),
                    GoogleInfoId = googleLocationInfo[1].Id,
                    PhotoUri = "https://lh3.googleusercontent.com/place-photos/AJnk2cwF-HFGQc4GQkQgXZReh13PWDIJEAR5_FEVfOAKScyZVggjnw7EEX8F_k0s09xlcUjv2pLioT58D47ivYLgxoqHtZ79lpMdBYj4QJ8MoDzYyByD20e6E2ayNtNPpPs384vTzyW6hX9dLEN6VA=s4800-w120-h120",
                    CreatedOn = DateTime.Now
                },
                new WarehousePhoto
                {
                    Id = Guid.NewGuid(),
                    GoogleInfoId = googleLocationInfo[1].Id,
                    PhotoUri = "https://lh3.googleusercontent.com/place-photos/AJnk2cxKWGXcUmrl1UuBfU5N_1Ib4Awzo1BjRqo3ZywWKARTpIKvcITTwRIGpUEObruZKYONGbkuitp-bBO3LcyU6v5x2XV62GAHYlilwweOEZHdW4UnOBOxdVzn__VaNwWLqkWbEpI2fsEk0LQvEg=s4800-w120-h120",
                    CreatedOn = DateTime.Now,
                },
                new WarehousePhoto
                {
                    Id = Guid.NewGuid(),
                    GoogleInfoId = googleLocationInfo[1].Id,
                    PhotoUri = "https://lh3.googleusercontent.com/place-photos/AJnk2cwZZW_MIP10MKKSKTKkuImanBos4E3TRBmJi9ySugTwK749k29MaG9V3GpAy6GoedfuLhiYt2jo5tQyG_6N12A1ILXq7bP4RhIc0kQM5Ld23AX-_7pueZ3C0_oMqXVZzwJhY1-PwmGh-o7N=s4800-w120-h120",
                    CreatedOn = DateTime.Now,
                },
                new WarehousePhoto
                {
                    Id = Guid.NewGuid(),
                    GoogleInfoId = googleLocationInfo[1].Id,
                    PhotoUri = "https://lh3.googleusercontent.com/place-photos/AJnk2cyWqxh6Oi6pNdXrsy6WmwfqXGHPuge_3unkJF2q9XB5YqLrZr7K2d1vvFGXiQB6rPOIRBb5UMJUP0pmhzJURmIrpJYJzBf5aAeGvtPlXrUNIGI6tkpuTceoLcnbbL3GhgBSlXDG9g8VbFcT75A=s4800-w120-h120",
                    CreatedOn = DateTime.Now,
                },
                new WarehousePhoto
                {
                    Id = Guid.NewGuid(),
                    GoogleInfoId = googleLocationInfo[1].Id,
                    PhotoUri = "https://lh3.googleusercontent.com/place-photos/AJnk2cxCRY7Nu1k6geIqE6Ow9Q8A17AigUNkP_anyn0ejNkUC9SzBiePj6FcxSVXdoHawdsRkPYaXb74UFmzDNF19_kfr9mryCf0eszGtRu41vzoyi-y-TK0EqbohrQDVh0otStjpAZTELtnoKffyw=s4800-w120-h120",
                    CreatedOn = DateTime.Now,
                },
                new WarehousePhoto
                {
                    Id = Guid.NewGuid(),
                    GoogleInfoId = googleLocationInfo[1].Id,
                    PhotoUri = "https://lh3.googleusercontent.com/place-photos/AJnk2cx0xcKTzbbxdSkmztmotc3niWWoDgzMxHpNxvlzYRINKB8Ala0JpVxeq8SnqSfoHVhqsEwaPT5mmhBEYrPgssqlYRmR6JtWbuQdq-O-RTkbM348rk2D8pCJM1HRZPwmPPyd0Cc0jaBWs5MVCQ=s4800-w120-h120",
                    CreatedOn = DateTime.Now,
                },
                new WarehousePhoto
                {
                    Id = Guid.NewGuid(),
                    GoogleInfoId = googleLocationInfo[1].Id,
                    PhotoUri = "https://lh3.googleusercontent.com/place-photos/AJnk2cwfwWkMEzaisUP5A9UBXYMHT3fMyJQ_rDuN7E6czy38Bkqqi_nmU2t6Jarb2Lo4iCWCMH881ERLjv7CbzczOQDNrd5SgLHrqBlBzlXAzSJH2ima1ek-rySZgZtV2eg214phQZEAxl-4m0x7FkE=s4800-w120-h120",
                    CreatedOn = DateTime.Now,
                },
                new WarehousePhoto
                {
                    Id = Guid.NewGuid(),
                    GoogleInfoId = googleLocationInfo[1].Id,
                    PhotoUri = "https://lh3.googleusercontent.com/place-photos/AJnk2czTVkjR_fVLL61EAHEn2RpkhTlInggcqsmTyo-89e-N2_3z3rxdpvOmCJ96_krO04pBLnJ-VbSbHnM5u4U6gl7C7YePKP55hSDfxYel8pIWVkkRZsQ7uXo73_GIYqPAP9UGbq77s-F4linufOs=s4800-w120-h120",
                    CreatedOn = DateTime.Now,
                },
                new WarehousePhoto
                {
                    Id = Guid.NewGuid(),
                    GoogleInfoId = googleLocationInfo[1].Id,
                    PhotoUri = "https://lh3.googleusercontent.com/place-photos/AJnk2cxBfrBIA5vR8QuuLat6hruRnyKQmTeURCWCs32dskSGCKx-XypFJ-cPUZB-vmmzhRrS66IKu49Zfs0wVsUIF8MtQ70_XYK60DR7zU9rJwT17TaiwRL6AKamn9VY520fCkQwzGfT2wlGO5A4EA=s4800-w120-h120",
                    CreatedOn = DateTime.Now,
                }
            };
            #endregion

            _modelBuilder.Entity<ProductTax>().HasData(productTaxes);
            _modelBuilder.Entity<Brand>().HasData(brands);
            _modelBuilder.Entity<ProductCategory>().HasData(categories);
            _modelBuilder.Entity<Product>().HasData(products);
            _modelBuilder.Entity<WarehouseLocationInfo>().HasData(warehouseLocations);
            _modelBuilder.Entity<WarehouseGoogleInfo>().HasData(googleLocationInfo);
            _modelBuilder.Entity<WarehousePhoto>().HasData(warehousePhotos);
            _modelBuilder.Entity<Warehouse>().HasData(warehouses);

        }
    }
}
