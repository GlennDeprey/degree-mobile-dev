using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mde.Project.Api.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EditedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EditedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductTaxes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaxRate = table.Column<double>(type: "float", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EditedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTaxes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseGoogleInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GoogleAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GoogleAddressId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EditedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseGoogleInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseLocationInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EditedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseLocationInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrandId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalesPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalesTaxId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Barcode = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EditedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Products_ProductCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ProductCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Products_ProductTaxes_SalesTaxId",
                        column: x => x.SalesTaxId,
                        principalTable: "ProductTaxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "WarehousePhotos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GoogleInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhotoUri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EditedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehousePhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehousePhotos_WarehouseGoogleInfos_GoogleInfoId",
                        column: x => x.GoogleInfoId,
                        principalTable: "WarehouseGoogleInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocationInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GoogleInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Earnings = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EditedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Warehouses_WarehouseGoogleInfos_GoogleInfoId",
                        column: x => x.GoogleInfoId,
                        principalTable: "WarehouseGoogleInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Warehouses_WarehouseLocationInfos_LocationInfoId",
                        column: x => x.LocationInfoId,
                        principalTable: "WarehouseLocationInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuantityChange = table.Column<int>(type: "int", nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EditedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reports_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    MinimumQuantity = table.Column<int>(type: "int", nullable: false),
                    RefillQuantity = table.Column<int>(type: "int", nullable: false),
                    HasAutoRefill = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EditedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarehouseItems_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseStats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalSales = table.Column<int>(type: "int", nullable: false),
                    TotalRestock = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EditedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseStats_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "561cfac6-9a45-4d69-844a-1d50dd115b8c", null, "Customer", "CUSTOMER" },
                    { "6ff26741-5e4a-4378-b03e-0987c53103b8", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "ImageUrl", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "640b537e-af5b-414d-9fd5-304706d7a058", 0, "8fa53cac-0c20-442f-b401-6bf106f3ab0f", "siegfried.derdeyn@howest.be", true, "Siegfried", null, "Derdeyn", false, null, "SIEGFRIED.DERDEYN@HOWEST.BE", "SIEGFRIED.DERDEYN@HOWEST.BE", "AQAAAAIAAYagAAAAECK4oVg5svx+x3Hc0okj+h3AqWSnjL9OQj4eY8HcRvzfKbINRANFXihnq8uwvp8KjQ==", null, false, "29e2f6bb-3658-4953-945e-0a72c950b287", false, "siegfried.derdeyn@howest.be" },
                    { "8a7214da-7deb-4f03-8ee1-0bc19c9f9204", 0, "0d9df6f9-bb0e-4fbb-8af2-d6c08e57bd53", "glenn.deprey@student.howest.be", true, "Glenn", null, "Deprey", false, null, "GLENN.DEPREY@STUDENT.HOWEST.BE", "GLENN.DEPREY@STUDENT.HOWEST.BE", "AQAAAAIAAYagAAAAEHrnwAo/lMU2e90Pc9szRmbPhtxBzgy5lW5cYWBxWV+L/d0iNijrvFzefdXF+MiKfw==", null, false, "f505a6bb-0226-4926-beaf-fe506832f952", false, "glenn.deprey@student.howest.be" }
                });

            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "Id", "CreatedOn", "DeletedOn", "EditedOn", "Name" },
                values: new object[,]
                {
                    { new Guid("4554a065-64b4-4bcb-af54-061d5c4b9c9b"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8570), null, null, "Purina One" },
                    { new Guid("548731b0-d59d-4aeb-bd5f-49e4f453a163"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8581), null, null, "Axe" },
                    { new Guid("7591840b-6686-4c4d-9a4f-5c581baae897"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8582), null, null, "Dash" },
                    { new Guid("9b557a9d-6970-48f6-8b45-eec45decc7eb"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8584), null, null, "Lea & Perrins" },
                    { new Guid("a4c2bb7e-4bca-412d-9c10-9d35ffd5cc49"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8564), null, null, "Albert Heijn" },
                    { new Guid("b0989e94-7672-4a91-a1ae-fc04d08504dd"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8568), null, null, "LU" },
                    { new Guid("b9cf81f1-9f8e-4554-a93a-99d236d873dc"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8579), null, null, "Pampers" },
                    { new Guid("d905ae1d-77e0-4de1-b2a5-50f98115b755"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8571), null, null, "Van Wijngaarden" },
                    { new Guid("feeb22d0-55b8-44b7-aca0-a0684d0face9"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8573), null, null, "Dreft" }
                });

            migrationBuilder.InsertData(
                table: "ProductCategory",
                columns: new[] { "Id", "CreatedOn", "DeletedOn", "EditedOn", "Name" },
                values: new object[,]
                {
                    { new Guid("04894e91-b148-4ab3-8948-ab9827f76458"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8592), null, null, "Huisdieren" },
                    { new Guid("55a177b3-626d-46da-bd19-1fbc3f0e09d3"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8596), null, null, "Baby en Kind" },
                    { new Guid("67a44f61-b799-4fb5-a3d0-8365b8dd1ee6"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8588), null, null, "Voeding" },
                    { new Guid("ab76d188-6d72-4560-a6b8-f8e3e1e426e4"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8591), null, null, "Verzorging" },
                    { new Guid("e60f3f21-3c5d-4cb6-bde3-577c74f01b70"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8594), null, null, "Huishouden" }
                });

            migrationBuilder.InsertData(
                table: "ProductTaxes",
                columns: new[] { "Id", "CreatedOn", "DeletedOn", "EditedOn", "Name", "TaxRate" },
                values: new object[,]
                {
                    { new Guid("4e217eab-6f38-44ac-aee7-4cd1739ce271"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8557), null, null, "12%", 0.12 },
                    { new Guid("572e39bb-2ca6-4175-8aed-2834fe2cad51"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8502), null, null, "0%", 0.0 },
                    { new Guid("b2aa3916-c2b1-4435-94c7-878331cf833e"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8559), null, null, "21%", 0.20999999999999999 },
                    { new Guid("e9014240-e5f9-4248-b870-2e1bf67bf72a"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8554), null, null, "6%", 0.059999999999999998 }
                });

            migrationBuilder.InsertData(
                table: "WarehouseGoogleInfos",
                columns: new[] { "Id", "CreatedOn", "DeletedOn", "EditedOn", "GoogleAddress", "GoogleAddressId" },
                values: new object[,]
                {
                    { new Guid("032e6f09-d956-4a63-9fb0-7707dc11aab2"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Albert Heijn Oostende, Torhoutsesteenweg, Ostend, Belgium", "ChIJJakyy0mv3EcRNMCeP-A3ZfA" },
                    { new Guid("e27e6faf-66a0-4cbe-9d7a-7a922c84b1d9"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Albert Heijn Veurne, Koksijdestraat, Veurne, Belgium", "ChIJucK6Jt6X3EcRHP0v6wEBFlo" }
                });

            migrationBuilder.InsertData(
                table: "WarehouseLocationInfos",
                columns: new[] { "Id", "Address", "City", "Country", "CreatedOn", "DeletedOn", "EditedOn", "Latitude", "Longitude", "PostalCode", "State" },
                values: new object[,]
                {
                    { new Guid("73e22f73-d372-4b0e-b09a-4302c39e13db"), "456 Elm St", "City B", "Country B", new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8771), null, null, 40.0, 30.0, "67890", "State B" },
                    { new Guid("e737f309-03ce-46fd-8e23-bffd8dcb8a12"), "123 Main St", "City A", "Country A", new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8768), null, null, 20.0, 10.0, "12345", "State A" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "6ff26741-5e4a-4378-b03e-0987c53103b8", "640b537e-af5b-414d-9fd5-304706d7a058" },
                    { "561cfac6-9a45-4d69-844a-1d50dd115b8c", "8a7214da-7deb-4f03-8ee1-0bc19c9f9204" },
                    { "6ff26741-5e4a-4378-b03e-0987c53103b8", "8a7214da-7deb-4f03-8ee1-0bc19c9f9204" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Barcode", "BrandId", "CategoryId", "CreatedOn", "DeletedOn", "Description", "EditedOn", "Image", "Name", "SalesPrice", "SalesTaxId" },
                values: new object[,]
                {
                    { new Guid("07ba3b4a-946c-47a4-bb67-beb795db8573"), "3609655643166", new Guid("a4c2bb7e-4bca-412d-9c10-9d35ffd5cc49"), new Guid("55a177b3-626d-46da-bd19-1fbc3f0e09d3"), new DateTime(2025, 5, 26, 22, 31, 28, 211, DateTimeKind.Unspecified).AddTicks(4786), null, "AH Beregoede Baby lotiondoekjes sensitive​.", null, "54e83a02-3fb3-45f9-a414-154ccef7f110.jpg", "AH Beregoede baby lotiondoekjes sensitive", 0.62m, new Guid("b2aa3916-c2b1-4435-94c7-878331cf833e") },
                    { new Guid("291b5c1b-3f92-4ddf-a9ae-aeb0cd973294"), "5513225762264", new Guid("7591840b-6686-4c4d-9a4f-5c581baae897"), new Guid("e60f3f21-3c5d-4cb6-bde3-577c74f01b70"), new DateTime(2025, 5, 26, 19, 29, 23, 498, DateTimeKind.Unspecified).AddTicks(5112), null, "Dash witter dan wit + anti-residu technologie waspoeder biedt briljant schone resultaten, zelfs bij een koud en kort cyclus.", null, "7a451181-5686-48e7-a8c0-2268ae40ed72.jpg", "Dash Poeder original bel", 1120.00m, new Guid("b2aa3916-c2b1-4435-94c7-878331cf833e") },
                    { new Guid("33a825fe-215f-4d56-85ed-24c9478f618b"), "4370107769466", new Guid("d905ae1d-77e0-4de1-b2a5-50f98115b755"), new Guid("67a44f61-b799-4fb5-a3d0-8365b8dd1ee6"), new DateTime(2025, 5, 26, 19, 18, 11, 751, DateTimeKind.Unspecified).AddTicks(1596), null, "De authentieke Zaanse mayonaise in handige knijpfles met steropening.", null, "576bc590-9fc3-4bc7-8760-71eea0326a7e.jpg", "Van Wijngaarden's Zaanse mayonaise", 3.15m, new Guid("b2aa3916-c2b1-4435-94c7-878331cf833e") },
                    { new Guid("363bc808-b527-4204-a27d-d45e04040627"), "0784244549357", new Guid("a4c2bb7e-4bca-412d-9c10-9d35ffd5cc49"), new Guid("67a44f61-b799-4fb5-a3d0-8365b8dd1ee6"), new DateTime(2025, 5, 26, 15, 23, 30, 619, DateTimeKind.Unspecified).AddTicks(5150), null, "De banaan is de bekendste tropische fruitsoort en een echte favoriet. Lekker als tussendoortje of om mee nemen als snack voor onderweg.", null, "db676fc2-134b-42b3-96bb-c3d27063743e.jpg", "AH Bananen tros", 0.90m, new Guid("e9014240-e5f9-4248-b870-2e1bf67bf72a") },
                    { new Guid("5ed190a8-ac31-4acc-bcd3-64d1030b4f80"), "0800345750178", new Guid("a4c2bb7e-4bca-412d-9c10-9d35ffd5cc49"), new Guid("67a44f61-b799-4fb5-a3d0-8365b8dd1ee6"), new DateTime(2025, 5, 26, 15, 10, 47, 938, DateTimeKind.Unspecified).AddTicks(2250), null, "Let op: dit product wordt in de winkel gewogen om de exacte prijs vast te stellen", null, "103114cc-50b0-40c4-aec5-3621d29b6cfc.jpg", "AH Witlof los (kg)", 1.17m, new Guid("e9014240-e5f9-4248-b870-2e1bf67bf72a") },
                    { new Guid("5ef148b2-bf95-4dc6-b249-6b2a1e509f8a"), "0420170197490", new Guid("b9cf81f1-9f8e-4554-a93a-99d236d873dc"), new Guid("55a177b3-626d-46da-bd19-1fbc3f0e09d3"), new DateTime(2025, 5, 26, 21, 10, 22, 601, DateTimeKind.Unspecified).AddTicks(79), null, "Pampers n graden1 luiers voor comfort en bescherming van de gevoelige huid.", null, "e3bbcf0f-0622-407c-8ffa-7f4ae8cfbd06.jpg", "Pampers Premium protection luiers maat 6", 10.30m, new Guid("b2aa3916-c2b1-4435-94c7-878331cf833e") },
                    { new Guid("7d9770fa-6902-4359-88de-765a3a8a417c"), "9541621865377", new Guid("feeb22d0-55b8-44b7-aca0-a0684d0face9"), new Guid("e60f3f21-3c5d-4cb6-bde3-577c74f01b70"), new DateTime(2025, 5, 26, 22, 38, 52, 664, DateTimeKind.Unspecified).AddTicks(8507), null, "Dreft platinum quickwash original met vloeibaar afwasmiddel ben je sneller de keuken uit door sneller en moeiteloos schoonmaken.", null, "96718dcf-3431-428b-9a6d-80fc2e9d5491.jpg", "Dreft Afwasmiddel platinum quick wash original", 2.37m, new Guid("b2aa3916-c2b1-4435-94c7-878331cf833e") },
                    { new Guid("7f74c69a-ff82-4cb9-9476-5cdb4003367b"), "4006381333948", new Guid("a4c2bb7e-4bca-412d-9c10-9d35ffd5cc49"), new Guid("67a44f61-b799-4fb5-a3d0-8365b8dd1ee6"), new DateTime(2025, 5, 17, 16, 22, 23, 415, DateTimeKind.Unspecified).AddTicks(7325), null, "Deze smakelijk gerookte zalmfilet is lekker op de boterham met verse dille en ideaal voor fijne hapjes, op toastjes of blini's met wat zure room.", null, "1c220f9e-3741-4a9e-a968-7eca9010c4ff.jpg", "AH Gerookte zalm", 3.60m, new Guid("b2aa3916-c2b1-4435-94c7-878331cf833e") },
                    { new Guid("88062673-cff2-40e7-b378-26065c11ffe0"), "3448530513633", new Guid("a4c2bb7e-4bca-412d-9c10-9d35ffd5cc49"), new Guid("67a44f61-b799-4fb5-a3d0-8365b8dd1ee6"), new DateTime(2025, 5, 26, 15, 28, 49, 308, DateTimeKind.Unspecified).AddTicks(5608), null, "Heerlijk zacht en mals witbrood met een mooie gemelleerde korst.", null, "900496d5-eed8-4296-93c9-e1af3451af9c.jpg", "AH Extra lang lekker tijger wit heel", 1.80m, new Guid("b2aa3916-c2b1-4435-94c7-878331cf833e") },
                    { new Guid("91707164-ecd8-4f8f-a1f3-219e70b1ca48"), "8856729110016", new Guid("a4c2bb7e-4bca-412d-9c10-9d35ffd5cc49"), new Guid("04894e91-b148-4ab3-8948-ab9827f76458"), new DateTime(2025, 5, 26, 15, 14, 15, 880, DateTimeKind.Unspecified).AddTicks(2693), null, "De AH Schep & Schoon Super Hygiëne kattenbakvulling is een fijne kattenbakvulling, verrijkt met blue bit korrels waardoor de hardnekkige geurtjes van urine én uitwerpselen worden ingesloten.", null, "5507ede3-f158-4a90-be22-bb80dd8d0cba.jpg", "AH Kattenbakvulling ex hygiene schep&schoon", 3.15m, new Guid("b2aa3916-c2b1-4435-94c7-878331cf833e") },
                    { new Guid("92899278-1e8c-4cbc-9fe1-9262e446b131"), "7934592776551", new Guid("4554a065-64b4-4bcb-af54-061d5c4b9c9b"), new Guid("04894e91-b148-4ab3-8948-ab9827f76458"), new DateTime(2025, 5, 26, 15, 18, 21, 311, DateTimeKind.Unspecified).AddTicks(7540), null, "Purina one adult kattenvoer rijk aan zalm is speciaal afgestemd op volwassen katten (van 1 jaar en ouder)", null, "d7903db8-4519-4b46-9dc9-6f6516784699.jpg", "Purina ONE Adult rijk aan zalm", 4.70m, new Guid("b2aa3916-c2b1-4435-94c7-878331cf833e") },
                    { new Guid("b0f83058-6b6a-442d-a8c9-740bfa96947b"), "4844730411660", new Guid("9b557a9d-6970-48f6-8b45-eec45decc7eb"), new Guid("67a44f61-b799-4fb5-a3d0-8365b8dd1ee6"), new DateTime(2025, 5, 26, 19, 19, 54, 785, DateTimeKind.Unspecified).AddTicks(4121), null, "Lea en perrins worcestershire sauce is een zuur-pittige tafelsaus voor vlees- en visgerechten en is met haar uitgesproken smaak sinds 1837 een van de meest invloedrijke sauzen in de wereldkeuken.", null, "ea13d884-f19c-4b17-b6f8-d80f5710cb75.jpg", "Lea & Perrins Worcestershire saus", 1.96m, new Guid("b2aa3916-c2b1-4435-94c7-878331cf833e") },
                    { new Guid("d21141fa-525a-486e-a372-a10a06d2b311"), "0324255992584", new Guid("a4c2bb7e-4bca-412d-9c10-9d35ffd5cc49"), new Guid("67a44f61-b799-4fb5-a3d0-8365b8dd1ee6"), new DateTime(2025, 5, 26, 19, 14, 39, 747, DateTimeKind.Unspecified).AddTicks(3425), null, "Deze kipburgers zijn al voorgegaard, alleen even bakken in de pan voor een heerlijk knapperig laagje.", null, "8b125098-ac4d-4caa-8e21-a53283ee481b.jpg", "AH Scharrel kipburger 4 stuks", 3.75m, new Guid("b2aa3916-c2b1-4435-94c7-878331cf833e") },
                    { new Guid("dc9e838a-95f9-4da7-afcb-1f1adf676fda"), "1589354243664", new Guid("548731b0-d59d-4aeb-bd5f-49e4f453a163"), new Guid("ab76d188-6d72-4560-a6b8-f8e3e1e426e4"), new DateTime(2025, 5, 26, 19, 52, 54, 39, DateTimeKind.Unspecified).AddTicks(9339), null, "Axe black deodorant bodyspray: voor effectieve bescherming en een onweerstaanbare geur.", null, "14d0f9b8-8597-4556-b9bb-cf77b8d00ddc.jpg", "Axe Black deodorant bodyspray", 3.80m, new Guid("b2aa3916-c2b1-4435-94c7-878331cf833e") },
                    { new Guid("df8a5743-1f68-4cec-8c7c-202155168029"), "5222172458266", new Guid("548731b0-d59d-4aeb-bd5f-49e4f453a163"), new Guid("ab76d188-6d72-4560-a6b8-f8e3e1e426e4"), new DateTime(2025, 5, 26, 19, 51, 43, 950, DateTimeKind.Unspecified).AddTicks(8221), null, "Axe apollo 3-in-1 douchegel voor lichaam, gezicht en haar hydrateert en houdt je 12 uur lang fris, met de heerlijke geur van salie en cederhout.", null, "aef8c3f1-a525-4ca2-b4a4-e17e04511a7a.jpg", "Axe Apollo showergel", 2.50m, new Guid("b2aa3916-c2b1-4435-94c7-878331cf833e") },
                    { new Guid("e82fd8cb-d681-4d10-a5f7-5e919e51af31"), "4006381333962", new Guid("a4c2bb7e-4bca-412d-9c10-9d35ffd5cc49"), new Guid("67a44f61-b799-4fb5-a3d0-8365b8dd1ee6"), new DateTime(2025, 5, 17, 16, 22, 23, 415, DateTimeKind.Unspecified).AddTicks(7331), null, "Franse kaas", null, "78f71df6-f8e4-4407-8132-2e14c818d0cd.jpg", "AH Romige brie 60+", 1.50m, new Guid("b2aa3916-c2b1-4435-94c7-878331cf833e") },
                    { new Guid("fa31206e-5134-4158-8ffa-fe35049330f6"), "4006381333931", new Guid("b0989e94-7672-4a91-a1ae-fc04d08504dd"), new Guid("67a44f61-b799-4fb5-a3d0-8365b8dd1ee6"), new DateTime(2025, 5, 17, 16, 22, 23, 415, DateTimeKind.Unspecified).AddTicks(7320), null, "Dunne, knapperige biscuitstokjes bedekt met heerlijke melkchocolade.", null, "d17ec364-5dfd-4f5a-96e7-2b0d6bdb7893.jpg", "LU Mikado melk chocolade", 165.00m, new Guid("b2aa3916-c2b1-4435-94c7-878331cf833e") }
                });

            migrationBuilder.InsertData(
                table: "WarehousePhotos",
                columns: new[] { "Id", "CreatedOn", "DeletedOn", "EditedOn", "GoogleInfoId", "PhotoUri" },
                values: new object[,]
                {
                    { new Guid("065487d7-2efb-43f0-a6b8-49420cc9b4d3"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8793), null, null, new Guid("e27e6faf-66a0-4cbe-9d7a-7a922c84b1d9"), "https://lh3.googleusercontent.com/place-photos/AJnk2cyV0QKUpAYpc0r7nhWvVWXQjhTklkG-tqr7-cCV0hqw752hTcDFio7v1TS7pieay3dGJyZKE1qcSsYYz2Nc1Ia4MqrzOzCL8yHLJqWcuP0tN1DVvl5OgdNROhD251l2wc6D4J76CDRYqF-9=s4800-w120-h120" },
                    { new Guid("0a5ce818-6b46-4634-b959-0ef2200ef068"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8796), null, null, new Guid("e27e6faf-66a0-4cbe-9d7a-7a922c84b1d9"), "https://lh3.googleusercontent.com/places/ANXAkqFCZz1f5-dL1L3OwpxGrN9BEeYjWba3xb455XIBgW-QpzakxwSeJYon89dJGX_MpRtj5p8LKLpvYkmOH-0fiD8F_OzWOax35zw=s4800-w120-h120" },
                    { new Guid("0fd1f091-505d-47f9-87c0-0905819f0e9a"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8841), null, null, new Guid("032e6f09-d956-4a63-9fb0-7707dc11aab2"), "https://lh3.googleusercontent.com/place-photos/AJnk2cx3V5M_RQj8F_Y2WhJ5qg2iLQUibOtC7izUEDVCQ_RD_KFBDop1UANboEM6o21yO7U2RuennHqHomMrBtZkpUm11uGEfmY78WKRTCrzzhyeIAUv7lBPidREKw8WuRWTl3DchP3wkdJmIaVH=s4800-w120-h120" },
                    { new Guid("2d8b5178-b830-4a4a-bcb0-852cebd31f84"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8846), null, null, new Guid("032e6f09-d956-4a63-9fb0-7707dc11aab2"), "https://lh3.googleusercontent.com/place-photos/AJnk2cwZZW_MIP10MKKSKTKkuImanBos4E3TRBmJi9ySugTwK749k29MaG9V3GpAy6GoedfuLhiYt2jo5tQyG_6N12A1ILXq7bP4RhIc0kQM5Ld23AX-_7pueZ3C0_oMqXVZzwJhY1-PwmGh-o7N=s4800-w120-h120" },
                    { new Guid("2db8d636-ecd1-4827-b477-3570be73ae63"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8799), null, null, new Guid("e27e6faf-66a0-4cbe-9d7a-7a922c84b1d9"), "https://lh3.googleusercontent.com/places/ANXAkqFUatKQMvNT7Um3L6mrHJh_hJ_AVciMztLc_sCwoyLACEx0Z0DyOjY_pBlwX03fiuPqo98dLpnmpOJDX7rjnmUm9nOp8U7nFqw=s4800-w120-h120" },
                    { new Guid("2fdefac0-8852-4eb1-81b2-e37594df24f7"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8859), null, null, new Guid("032e6f09-d956-4a63-9fb0-7707dc11aab2"), "https://lh3.googleusercontent.com/place-photos/AJnk2cxBfrBIA5vR8QuuLat6hruRnyKQmTeURCWCs32dskSGCKx-XypFJ-cPUZB-vmmzhRrS66IKu49Zfs0wVsUIF8MtQ70_XYK60DR7zU9rJwT17TaiwRL6AKamn9VY520fCkQwzGfT2wlGO5A4EA=s4800-w120-h120" },
                    { new Guid("33279093-cee9-4618-bd46-85cb261a6a0d"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8845), null, null, new Guid("032e6f09-d956-4a63-9fb0-7707dc11aab2"), "https://lh3.googleusercontent.com/place-photos/AJnk2cxKWGXcUmrl1UuBfU5N_1Ib4Awzo1BjRqo3ZywWKARTpIKvcITTwRIGpUEObruZKYONGbkuitp-bBO3LcyU6v5x2XV62GAHYlilwweOEZHdW4UnOBOxdVzn__VaNwWLqkWbEpI2fsEk0LQvEg=s4800-w120-h120" },
                    { new Guid("61621208-0f39-4ed7-b46c-6b0eee0f67ea"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8850), null, null, new Guid("032e6f09-d956-4a63-9fb0-7707dc11aab2"), "https://lh3.googleusercontent.com/place-photos/AJnk2cxCRY7Nu1k6geIqE6Ow9Q8A17AigUNkP_anyn0ejNkUC9SzBiePj6FcxSVXdoHawdsRkPYaXb74UFmzDNF19_kfr9mryCf0eszGtRu41vzoyi-y-TK0EqbohrQDVh0otStjpAZTELtnoKffyw=s4800-w120-h120" },
                    { new Guid("6b3f6e47-e4a8-4c2e-85b9-4c9b999c94e3"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8832), null, null, new Guid("e27e6faf-66a0-4cbe-9d7a-7a922c84b1d9"), "https://lh3.googleusercontent.com/places/ANXAkqEAjmvnI1AiffFG-FOjtcVTZ7wq1GUEPh_1q_KGk8IWVIisol62un2y-RqEEteNQLP5-jx1uLpCBWeDZIyISStK4eOTkvQrZ2Y=s4800-w120-h120" },
                    { new Guid("80bb7641-d480-44d1-bd3a-13ed9b529722"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8805), null, null, new Guid("e27e6faf-66a0-4cbe-9d7a-7a922c84b1d9"), "https://lh3.googleusercontent.com/places/ANXAkqH9tKAG77TzGH2pnwZGJLexdPLLYViha-i3LBWX9-8vhnJRqu2IQfMyRK7dvB3J9kzdc2RyhsmASDhs2YfbIyPczCuzDeEHAcU=s4800-w120-h120" },
                    { new Guid("85a3c8c3-7e99-4089-93f8-4a035a7aac41"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8835), null, null, new Guid("e27e6faf-66a0-4cbe-9d7a-7a922c84b1d9"), "https://lh3.googleusercontent.com/places/ANXAkqEtayDJrKMJzeuJhaBlX67L0vrSBSEi7atRtbX0fF7mptHrQmAoH12YjPai_iRmdFpaIut8MicT8YVmP4Pxkwn0L5HGro5xhLE=s4800-w120-h120" },
                    { new Guid("8a571e5e-96bc-4c25-bf6a-9660ae315d11"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8807), null, null, new Guid("e27e6faf-66a0-4cbe-9d7a-7a922c84b1d9"), "https://lh3.googleusercontent.com/place-photos/AJnk2cyvHuVkb0HN2eVbSYFjJ0Unv-ke4JKVCyqDFvnTZcr1eBdRTbvRZLrnMUOgQ3Ad-wZxQE8bznEWhIzNMsPCj_FGRYxhMxOxZGBDMJmtC-nxGhpMPKL0XQmySiH0tSCPETOCy6eaHF0mYmyaEQ=s4800-w120-h120" },
                    { new Guid("8c4b608c-291d-4de1-b018-9dddb603800a"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8838), null, null, new Guid("e27e6faf-66a0-4cbe-9d7a-7a922c84b1d9"), "https://lh3.googleusercontent.com/places/ANXAkqHD6KsHJ92smoCMkCpvMg66IoEz4tfXsbeWXeHSNXfv_F9YZbM6gm0KQIvGDoEo3g5P_fnJbhK6O2QMP4n4vZuk4Nh_04Ybji8=s4800-w120-h120" },
                    { new Guid("8dacf0ed-8419-417e-86b1-bf0b98fabadc"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8802), null, null, new Guid("e27e6faf-66a0-4cbe-9d7a-7a922c84b1d9"), "https://lh3.googleusercontent.com/place-photos/AJnk2cwwQ8ZGAyA3PKUTbPqaXvjBlB0R428Cq1xTCwumhZZlPmbptdHabWJOrD_lwhvFTb6ZtuiVXRz4t_T2Qoc_5jAZHXsJdE4S_sqZO1B9mBem8H5D-euptffcEU7PVAc0-6iC6PAG-2TRX8H9oQ=s4800-w120-h120" },
                    { new Guid("917d4b2c-42aa-48b3-9fe4-41de02569cb8"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8843), null, null, new Guid("032e6f09-d956-4a63-9fb0-7707dc11aab2"), "https://lh3.googleusercontent.com/place-photos/AJnk2cwF-HFGQc4GQkQgXZReh13PWDIJEAR5_FEVfOAKScyZVggjnw7EEX8F_k0s09xlcUjv2pLioT58D47ivYLgxoqHtZ79lpMdBYj4QJ8MoDzYyByD20e6E2ayNtNPpPs384vTzyW6hX9dLEN6VA=s4800-w120-h120" },
                    { new Guid("9677d799-c4be-4f39-8951-586f458ace74"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8800), null, null, new Guid("e27e6faf-66a0-4cbe-9d7a-7a922c84b1d9"), "https://lh3.googleusercontent.com/places/ANXAkqF73sx2p_xjCyaSEh-nsT-pYbG0tbuiiW7Iiq680aT7cI4SWiji9zmDVbM6IdT0I66Mn2ILIgaRUT-vHelRV1-QUiQswPYbIAY=s4800-w120-h120" },
                    { new Guid("96cb5905-d28d-4d87-a493-9afa637653d9"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8855), null, null, new Guid("032e6f09-d956-4a63-9fb0-7707dc11aab2"), "https://lh3.googleusercontent.com/place-photos/AJnk2cwfwWkMEzaisUP5A9UBXYMHT3fMyJQ_rDuN7E6czy38Bkqqi_nmU2t6Jarb2Lo4iCWCMH881ERLjv7CbzczOQDNrd5SgLHrqBlBzlXAzSJH2ima1ek-rySZgZtV2eg214phQZEAxl-4m0x7FkE=s4800-w120-h120" },
                    { new Guid("c1e30422-c64e-4065-900d-baa6707e35eb"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8848), null, null, new Guid("032e6f09-d956-4a63-9fb0-7707dc11aab2"), "https://lh3.googleusercontent.com/place-photos/AJnk2cyWqxh6Oi6pNdXrsy6WmwfqXGHPuge_3unkJF2q9XB5YqLrZr7K2d1vvFGXiQB6rPOIRBb5UMJUP0pmhzJURmIrpJYJzBf5aAeGvtPlXrUNIGI6tkpuTceoLcnbbL3GhgBSlXDG9g8VbFcT75A=s4800-w120-h120" },
                    { new Guid("da9ed8f6-4e24-4111-a8ce-1b81ca8d4d66"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8853), null, null, new Guid("032e6f09-d956-4a63-9fb0-7707dc11aab2"), "https://lh3.googleusercontent.com/place-photos/AJnk2cx0xcKTzbbxdSkmztmotc3niWWoDgzMxHpNxvlzYRINKB8Ala0JpVxeq8SnqSfoHVhqsEwaPT5mmhBEYrPgssqlYRmR6JtWbuQdq-O-RTkbM348rk2D8pCJM1HRZPwmPPyd0Cc0jaBWs5MVCQ=s4800-w120-h120" },
                    { new Guid("fcda3b9e-ce8f-49b3-ba2f-835bc7f9ed34"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8857), null, null, new Guid("032e6f09-d956-4a63-9fb0-7707dc11aab2"), "https://lh3.googleusercontent.com/place-photos/AJnk2czTVkjR_fVLL61EAHEn2RpkhTlInggcqsmTyo-89e-N2_3z3rxdpvOmCJ96_krO04pBLnJ-VbSbHnM5u4U6gl7C7YePKP55hSDfxYel8pIWVkkRZsQ7uXo73_GIYqPAP9UGbq77s-F4linufOs=s4800-w120-h120" }
                });

            migrationBuilder.InsertData(
                table: "Warehouses",
                columns: new[] { "Id", "CreatedOn", "DeletedOn", "Earnings", "EditedOn", "GoogleInfoId", "LocationInfoId", "Name", "Phone", "ShortName" },
                values: new object[,]
                {
                    { new Guid("b9d2182c-9bb0-47e0-ad6f-107da4f13bd0"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8789), null, 20000m, null, new Guid("032e6f09-d956-4a63-9fb0-7707dc11aab2"), new Guid("73e22f73-d372-4b0e-b09a-4302c39e13db"), "AH Oostende", "059 36 95 00", "AHBEWV-O" },
                    { new Guid("e30fbe65-a787-4226-a9a8-be373574ceec"), new DateTime(2025, 5, 28, 12, 25, 48, 818, DateTimeKind.Local).AddTicks(8785), null, 10000m, null, new Guid("e27e6faf-66a0-4cbe-9d7a-7a922c84b1d9"), new Guid("e737f309-03ce-46fd-8e23-bffd8dcb8a12"), "AH Veurne", "058 69 01 00", "AHBEWV-V" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SalesTaxId",
                table: "Products",
                column: "SalesTaxId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ProductId",
                table: "Reports",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_WarehouseId",
                table: "Reports",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseItems_ProductId",
                table: "WarehouseItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseItems_WarehouseId",
                table: "WarehouseItems",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehousePhotos_GoogleInfoId",
                table: "WarehousePhotos",
                column: "GoogleInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_GoogleInfoId",
                table: "Warehouses",
                column: "GoogleInfoId",
                unique: true,
                filter: "[GoogleInfoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_LocationInfoId",
                table: "Warehouses",
                column: "LocationInfoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseStats_WarehouseId",
                table: "WarehouseStats",
                column: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "WarehouseItems");

            migrationBuilder.DropTable(
                name: "WarehousePhotos");

            migrationBuilder.DropTable(
                name: "WarehouseStats");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "ProductCategory");

            migrationBuilder.DropTable(
                name: "ProductTaxes");

            migrationBuilder.DropTable(
                name: "WarehouseGoogleInfos");

            migrationBuilder.DropTable(
                name: "WarehouseLocationInfos");
        }
    }
}
