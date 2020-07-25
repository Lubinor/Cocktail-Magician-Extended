using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CocktailMagician.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ImageData = table.Column<byte[]>(nullable: true),
                    ImageSource = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cocktails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    AverageRating = table.Column<double>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ImageData = table.Column<byte[]>(nullable: true),
                    ImageSource = table.Column<string>(nullable: true),
                    CreatorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cocktails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cocktails_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bars",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    CityId = table.Column<int>(nullable: false),
                    Address = table.Column<string>(maxLength: 100, nullable: false),
                    Phone = table.Column<string>(maxLength: 20, nullable: false),
                    AverageRating = table.Column<double>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ImageData = table.Column<byte[]>(nullable: true),
                    ImageSource = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bars_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CocktailsUsersReviews",
                columns: table => new
                {
                    CocktailId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Rating = table.Column<double>(nullable: false),
                    Comment = table.Column<string>(maxLength: 500, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CocktailsUsersReviews", x => new { x.CocktailId, x.UserId });
                    table.ForeignKey(
                        name: "FK_CocktailsUsersReviews_Cocktails_CocktailId",
                        column: x => x.CocktailId,
                        principalTable: "Cocktails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CocktailsUsersReviews_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IngredientsCocktails",
                columns: table => new
                {
                    IngredientId = table.Column<int>(nullable: false),
                    CocktailId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredientsCocktails", x => new { x.IngredientId, x.CocktailId });
                    table.ForeignKey(
                        name: "FK_IngredientsCocktails_Cocktails_CocktailId",
                        column: x => x.CocktailId,
                        principalTable: "Cocktails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IngredientsCocktails_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BarsCocktails",
                columns: table => new
                {
                    BarId = table.Column<int>(nullable: false),
                    CocktailId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BarsCocktails", x => new { x.BarId, x.CocktailId });
                    table.ForeignKey(
                        name: "FK_BarsCocktails_Bars_BarId",
                        column: x => x.BarId,
                        principalTable: "Bars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BarsCocktails_Cocktails_CocktailId",
                        column: x => x.CocktailId,
                        principalTable: "Cocktails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BarsUsersReviews",
                columns: table => new
                {
                    BarId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Rating = table.Column<double>(nullable: false),
                    Comment = table.Column<string>(maxLength: 500, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BarsUsersReviews", x => new { x.BarId, x.UserId });
                    table.ForeignKey(
                        name: "FK_BarsUsersReviews_Bars_BarId",
                        column: x => x.BarId,
                        principalTable: "Bars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BarsUsersReviews_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 2, "653ca683-cb31-42fb-9e2d-16f5c45fd2d7", "Cocktail Magician", "COCKTAIL MAGICIAN" },
                    { 1, "6c8a1361-25eb-4452-a970-5b119e7d5a38", "Bar Crawler", "BAR CRAWLER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedOn", "Email", "EmailConfirmed", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { 2, 0, "a7a0b805-914d-4628-85d0-697cb8d5f4ea", new DateTime(2020, 7, 25, 22, 43, 6, 667, DateTimeKind.Utc).AddTicks(304), "user@user.com", false, false, true, null, "USER@USER.COM", "USER@USER.COM", "AQAAAAEAACcQAAAAEFlZMHp8m2KU83UnynVz3Batt7TG8N1i2GWdzQqb3sG3VDjvWeANYCp3tDHeRWQV2Q==", null, false, "f4bf961c-0e41-4c92-9642-b0b6dd1a0da1", false, "user@user.com" },
                    { 1, 0, "5ef5b47a-53f8-465d-a149-6786ed53958e", new DateTime(2020, 7, 25, 22, 43, 6, 654, DateTimeKind.Utc).AddTicks(7083), "admin@admin.com", false, false, true, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAEAACcQAAAAEKeTWzLMhoQ8op9DyTnxDMRcByqDX28b66SPbU8h8GFZEMUaLLdBONmM/+HH+6viPQ==", null, false, "7634aebe-1f77-4681-97af-4ddebd85ad6d", false, "admin@admin.com" }
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "CreatedOn", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2020, 7, 25, 22, 43, 6, 673, DateTimeKind.Utc).AddTicks(4699), false, "Sofia" },
                    { 4, new DateTime(2020, 7, 25, 22, 43, 6, 673, DateTimeKind.Utc).AddTicks(5358), false, "Burgas" },
                    { 3, new DateTime(2020, 7, 25, 22, 43, 6, 673, DateTimeKind.Utc).AddTicks(5356), false, "Varna" },
                    { 2, new DateTime(2020, 7, 25, 22, 43, 6, 673, DateTimeKind.Utc).AddTicks(5330), false, "Plovdiv" }
                });

            migrationBuilder.InsertData(
                table: "Ingredients",
                columns: new[] { "Id", "CreatedOn", "ImageData", "ImageSource", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 2, new DateTime(2020, 7, 25, 22, 43, 6, 673, DateTimeKind.Utc).AddTicks(8030), null, null, false, "Gin" },
                    { 3, new DateTime(2020, 7, 25, 22, 43, 6, 673, DateTimeKind.Utc).AddTicks(8041), null, null, false, "Rum" },
                    { 4, new DateTime(2020, 7, 25, 22, 43, 6, 673, DateTimeKind.Utc).AddTicks(8044), null, null, false, "Soda" },
                    { 5, new DateTime(2020, 7, 25, 22, 43, 6, 673, DateTimeKind.Utc).AddTicks(8046), null, null, false, "Coke" },
                    { 6, new DateTime(2020, 7, 25, 22, 43, 6, 673, DateTimeKind.Utc).AddTicks(8092), null, null, false, "Lemon juice" },
                    { 8, new DateTime(2020, 7, 25, 22, 43, 6, 673, DateTimeKind.Utc).AddTicks(8095), null, null, false, "Milk" },
                    { 9, new DateTime(2020, 7, 25, 22, 43, 6, 673, DateTimeKind.Utc).AddTicks(8096), null, null, false, "Coffee liqueur" },
                    { 10, new DateTime(2020, 7, 25, 22, 43, 6, 673, DateTimeKind.Utc).AddTicks(8097), null, null, false, "Orange juice" },
                    { 11, new DateTime(2020, 7, 25, 22, 43, 6, 673, DateTimeKind.Utc).AddTicks(8099), null, null, false, "Tomato juice" },
                    { 12, new DateTime(2020, 7, 25, 22, 43, 6, 673, DateTimeKind.Utc).AddTicks(8101), null, null, false, "Tabasco" },
                    { 13, new DateTime(2020, 7, 25, 22, 43, 6, 673, DateTimeKind.Utc).AddTicks(8102), null, null, false, "Lime" },
                    { 1, new DateTime(2020, 7, 25, 22, 43, 6, 673, DateTimeKind.Utc).AddTicks(7510), null, null, false, "Vodka" },
                    { 7, new DateTime(2020, 7, 25, 22, 43, 6, 673, DateTimeKind.Utc).AddTicks(8093), null, null, false, "Sugar" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 2, 1 }
                });

            migrationBuilder.InsertData(
                table: "Bars",
                columns: new[] { "Id", "Address", "AverageRating", "CityId", "CreatedOn", "ImageData", "ImageSource", "IsDeleted", "Name", "Phone" },
                values: new object[,]
                {
                    { 1, "104 Vitosha blvd.", 0.0, 1, new DateTime(2020, 7, 25, 22, 43, 6, 674, DateTimeKind.Utc).AddTicks(3748), null, null, false, "Memento", "0889 555 682" },
                    { 2, "22 Tsar Ivan Shishman str.", 0.0, 1, new DateTime(2020, 7, 25, 22, 43, 6, 674, DateTimeKind.Utc).AddTicks(4679), null, null, false, "Bilkova", "0898 639 068" },
                    { 3, "36 Yoakim Gruev str.", 0.0, 2, new DateTime(2020, 7, 25, 22, 43, 6, 674, DateTimeKind.Utc).AddTicks(4709), null, null, false, "Petnoto", "0878 509 703" },
                    { 4, "Central Beach", 0.0, 3, new DateTime(2020, 7, 25, 22, 43, 6, 674, DateTimeKind.Utc).AddTicks(4711), null, null, false, "Cubo", "0898 425 232" },
                    { 5, "1 Tsar Peter str.", 0.0, 4, new DateTime(2020, 7, 25, 22, 43, 6, 674, DateTimeKind.Utc).AddTicks(4713), null, null, false, "Barcode", "0895 509 659" },
                    { 6, "53 Stefan Stambolov blvd.", 0.0, 4, new DateTime(2020, 7, 25, 22, 43, 6, 674, DateTimeKind.Utc).AddTicks(4716), null, null, false, "Fabric Club", "0887 909 019" }
                });

            migrationBuilder.InsertData(
                table: "Cocktails",
                columns: new[] { "Id", "AverageRating", "CreatedOn", "CreatorId", "ImageData", "ImageSource", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 4, 0.0, new DateTime(2020, 7, 25, 22, 43, 6, 674, DateTimeKind.Utc).AddTicks(1327), 1, null, null, false, "Mai Tai" },
                    { 5, 0.0, new DateTime(2020, 7, 25, 22, 43, 6, 674, DateTimeKind.Utc).AddTicks(1329), 1, null, null, false, "Gin Fizz" },
                    { 1, 0.0, new DateTime(2020, 7, 25, 22, 43, 6, 674, DateTimeKind.Utc).AddTicks(394), 2, null, null, false, "Mojito" },
                    { 2, 0.0, new DateTime(2020, 7, 25, 22, 43, 6, 674, DateTimeKind.Utc).AddTicks(1308), 2, null, null, false, "Cuba Libre" },
                    { 3, 0.0, new DateTime(2020, 7, 25, 22, 43, 6, 674, DateTimeKind.Utc).AddTicks(1325), 2, null, null, false, "Sex on the Beach" },
                    { 6, 0.0, new DateTime(2020, 7, 25, 22, 43, 6, 674, DateTimeKind.Utc).AddTicks(1331), 2, null, null, false, "Bloody Mary" }
                });

            migrationBuilder.InsertData(
                table: "BarsCocktails",
                columns: new[] { "BarId", "CocktailId", "IsDeleted" },
                values: new object[,]
                {
                    { 3, 4, false },
                    { 6, 6, false },
                    { 5, 6, false },
                    { 4, 6, false },
                    { 3, 6, false },
                    { 2, 6, false },
                    { 1, 6, false },
                    { 6, 3, false },
                    { 2, 3, false },
                    { 6, 1, false },
                    { 1, 2, false },
                    { 4, 1, false },
                    { 4, 5, false },
                    { 1, 1, false },
                    { 5, 1, false },
                    { 3, 1, false },
                    { 2, 1, false }
                });

            migrationBuilder.InsertData(
                table: "IngredientsCocktails",
                columns: new[] { "IngredientId", "CocktailId", "IsDeleted" },
                values: new object[,]
                {
                    { 3, 2, false },
                    { 5, 2, false },
                    { 13, 2, false },
                    { 13, 1, false },
                    { 7, 1, false },
                    { 1, 3, false },
                    { 10, 3, false },
                    { 4, 1, false },
                    { 2, 5, false },
                    { 6, 5, false },
                    { 3, 1, false },
                    { 6, 4, false },
                    { 3, 4, false },
                    { 1, 6, false },
                    { 11, 6, false },
                    { 12, 6, false }
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
                name: "IX_Bars_CityId",
                table: "Bars",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_BarsCocktails_CocktailId",
                table: "BarsCocktails",
                column: "CocktailId");

            migrationBuilder.CreateIndex(
                name: "IX_BarsUsersReviews_UserId",
                table: "BarsUsersReviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Cocktails_CreatorId",
                table: "Cocktails",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_CocktailsUsersReviews_UserId",
                table: "CocktailsUsersReviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IngredientsCocktails_CocktailId",
                table: "IngredientsCocktails",
                column: "CocktailId");
        }

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
                name: "BarsCocktails");

            migrationBuilder.DropTable(
                name: "BarsUsersReviews");

            migrationBuilder.DropTable(
                name: "CocktailsUsersReviews");

            migrationBuilder.DropTable(
                name: "IngredientsCocktails");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Bars");

            migrationBuilder.DropTable(
                name: "Cocktails");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
