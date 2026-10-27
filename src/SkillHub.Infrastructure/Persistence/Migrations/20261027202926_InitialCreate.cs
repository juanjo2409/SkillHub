using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SkillHub.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Instructors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Specialty = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FullName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    IdentityUserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    IdentityUserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
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
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
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
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
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
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
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
                name: "Workshops",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MaxCapacity = table.Column<int>(type: "integer", nullable: false),
                    InstructorId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workshops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workshops_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Instructors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Enrollments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ParticipantId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkshopId = table.Column<Guid>(type: "uuid", nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CancelledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CancelledByUserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enrollments_Participants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrollments_Workshops_WorkshopId",
                        column: x => x.WorkshopId,
                        principalTable: "Workshops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "11111111-1111-1111-1111-111111111111", "11111111-1111-1111-1111-111111111111", "Employee", "EMPLOYEE" },
                    { "22222222-2222-2222-2222-222222222222", "22222222-2222-2222-2222-222222222222", "Participant", "PARTICIPANT" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "33333333-3333-3333-3333-000000000001", 0, "33333333-3333-3333-3333-000000000001", "andres.mejia@correo.com", true, false, null, "ANDRES.MEJIA@CORREO.COM", "ANDRES.MEJIA@CORREO.COM", "AQAAAAIAAYagAAAAEAfN3rgifEd6oggjwXuixnnGEPR5CSuTSMhgJ/3/FB2LehswhgXmxdC0xSAjqO7Eyg==", null, false, "33333333-3333-3333-3333-000000000001", false, "andres.mejia@correo.com" },
                    { "33333333-3333-3333-3333-000000000002", 0, "33333333-3333-3333-3333-000000000002", "rosa.barrios@correo.com", true, false, null, "ROSA.BARRIOS@CORREO.COM", "ROSA.BARRIOS@CORREO.COM", "AQAAAAIAAYagAAAAEG9UOZXynzwJC+rO/aDyQBl1+sAGyiU1RYd1b2Ocz3lPRx8tsVaadz9iJflOgdldmw==", null, false, "33333333-3333-3333-3333-000000000002", false, "rosa.barrios@correo.com" },
                    { "33333333-3333-3333-3333-000000000003", 0, "33333333-3333-3333-3333-000000000003", "mauricio.jimenez@correo.com", true, false, null, "MAURICIO.JIMENEZ@CORREO.COM", "MAURICIO.JIMENEZ@CORREO.COM", "AQAAAAIAAYagAAAAEGODGff0lFv8xuado/221vLCbiahyKHx/iPcLc42I5S75oypSso7OKoL1cYQ/phARg==", null, false, "33333333-3333-3333-3333-000000000003", false, "mauricio.jimenez@correo.com" },
                    { "33333333-3333-3333-3333-000000000004", 0, "33333333-3333-3333-3333-000000000004", "natalia.herrera@correo.com", true, false, null, "NATALIA.HERRERA@CORREO.COM", "NATALIA.HERRERA@CORREO.COM", "AQAAAAIAAYagAAAAEPwubueLdW1xvWMkRcQiT+JqGC6wyPCUynPu0y6uc9qSe28D8oJENsCy1957pooLLA==", null, false, "33333333-3333-3333-3333-000000000004", false, "natalia.herrera@correo.com" },
                    { "33333333-3333-3333-3333-000000000005", 0, "33333333-3333-3333-3333-000000000005", "hernan.quintero@correo.com", true, false, null, "HERNAN.QUINTERO@CORREO.COM", "HERNAN.QUINTERO@CORREO.COM", "AQAAAAIAAYagAAAAEHkOSZikUbwwv7msNeI+H+Q/NKN5Q3CAkkxVYCPWQIYtDDT0Ll90se4TedsGx2s+8w==", null, false, "33333333-3333-3333-3333-000000000005", false, "hernan.quintero@correo.com" },
                    { "33333333-3333-3333-3333-000000000006", 0, "33333333-3333-3333-3333-000000000006", "paola.vargas@correo.com", true, false, null, "PAOLA.VARGAS@CORREO.COM", "PAOLA.VARGAS@CORREO.COM", "AQAAAAIAAYagAAAAECT7aWaq02ylkv3nqfHOBxyFrXu2cVe/cr3Jdj1mc0OuE1ibx4DEoIdZ+RPGKxuMZQ==", null, false, "33333333-3333-3333-3333-000000000006", false, "paola.vargas@correo.com" },
                    { "55555555-5555-5555-5555-000000000001", 0, "55555555-5555-5555-5555-000000000001", "jorge.perez@correo.com", true, false, null, "JORGE.PEREZ@CORREO.COM", "JORGE.PEREZ@CORREO.COM", "AQAAAAIAAYagAAAAEHyrk5/vBjffZHZ3kuSdAC3ISWjloUNLWkcvHu6bCznWoa+oStffzidUZqsyuVTzRQ==", null, false, "55555555-5555-5555-5555-000000000001", false, "jorge.perez@correo.com" },
                    { "55555555-5555-5555-5555-000000000002", 0, "55555555-5555-5555-5555-000000000002", "luz.ospina@correo.com", true, false, null, "LUZ.OSPINA@CORREO.COM", "LUZ.OSPINA@CORREO.COM", "AQAAAAIAAYagAAAAEKiZVMzCij81FiTEyysEftmNWIMZ2cBoPtvsE3wPwTP6zSDjRrpOuPef/pLdCQCsbw==", null, false, "55555555-5555-5555-5555-000000000002", false, "luz.ospina@correo.com" },
                    { "55555555-5555-5555-5555-000000000003", 0, "55555555-5555-5555-5555-000000000003", "carlos.rincon@correo.com", true, false, null, "CARLOS.RINCON@CORREO.COM", "CARLOS.RINCON@CORREO.COM", "AQAAAAIAAYagAAAAEA6lEvCsu68xGi1fiI56tEdmoEX8gEYXBarp78bjvNmC3ctKa73vL6P+1fqmUZibyw==", null, false, "55555555-5555-5555-5555-000000000003", false, "carlos.rincon@correo.com" },
                    { "55555555-5555-5555-5555-000000000004", 0, "55555555-5555-5555-5555-000000000004", "valentina.gomez@correo.com", true, false, null, "VALENTINA.GOMEZ@CORREO.COM", "VALENTINA.GOMEZ@CORREO.COM", "AQAAAAIAAYagAAAAEOmPZcY6u9Y+XjhThI67wyAcvDBw0uSFlcwn1EqWu6yLtnvNQQIbrgdp3Ct4ddACkQ==", null, false, "55555555-5555-5555-5555-000000000004", false, "valentina.gomez@correo.com" },
                    { "55555555-5555-5555-5555-000000000005", 0, "55555555-5555-5555-5555-000000000005", "juan.restrepo@correo.com", true, false, null, "JUAN.RESTREPO@CORREO.COM", "JUAN.RESTREPO@CORREO.COM", "AQAAAAIAAYagAAAAEHb2jKQYdeLijNeN5dmOcdpDybRh/yEuTBAo7dhL6smTEIPEZpgv0ZeZ7TUIsZMrvA==", null, false, "55555555-5555-5555-5555-000000000005", false, "juan.restrepo@correo.com" },
                    { "55555555-5555-5555-5555-000000000006", 0, "55555555-5555-5555-5555-000000000006", "maria.torres@correo.com", true, false, null, "MARIA.TORRES@CORREO.COM", "MARIA.TORRES@CORREO.COM", "AQAAAAIAAYagAAAAEO9m8j1i1FpOenSeO6VaE+bZIu0ZrBC7wHHx1zVjSz8FuKfzehrP9sBE7cL4sQ04Hg==", null, false, "55555555-5555-5555-5555-000000000006", false, "maria.torres@correo.com" },
                    { "55555555-5555-5555-5555-000000000007", 0, "55555555-5555-5555-5555-000000000007", "santiago.ramirez@correo.com", true, false, null, "SANTIAGO.RAMIREZ@CORREO.COM", "SANTIAGO.RAMIREZ@CORREO.COM", "AQAAAAIAAYagAAAAEBkRuh0zAedtmMKNCmxdXLiUU4u0NvREgDnTg+f9kweOencLfMXoqOVtob0vZto5pQ==", null, false, "55555555-5555-5555-5555-000000000007", false, "santiago.ramirez@correo.com" },
                    { "55555555-5555-5555-5555-000000000008", 0, "55555555-5555-5555-5555-000000000008", "daniela.morales@correo.com", true, false, null, "DANIELA.MORALES@CORREO.COM", "DANIELA.MORALES@CORREO.COM", "AQAAAAIAAYagAAAAEJBswMa1x5hsftU8PAZCTJCP2Ro4NJk3w5lwuBi5PmHqNIMPw5fSp8AdCK0RcKstrQ==", null, false, "55555555-5555-5555-5555-000000000008", false, "daniela.morales@correo.com" },
                    { "55555555-5555-5555-5555-000000000009", 0, "55555555-5555-5555-5555-000000000009", "sebastian.castro@correo.com", true, false, null, "SEBASTIAN.CASTRO@CORREO.COM", "SEBASTIAN.CASTRO@CORREO.COM", "AQAAAAIAAYagAAAAELJa/svia57b/4zaM4RNe5PJnMPyocf5plUCUVU2cU/VKOkfGVA1q/RY5PwvAnekNg==", null, false, "55555555-5555-5555-5555-000000000009", false, "sebastian.castro@correo.com" },
                    { "55555555-5555-5555-5555-000000000010", 0, "55555555-5555-5555-5555-000000000010", "laura.martinez@correo.com", true, false, null, "LAURA.MARTINEZ@CORREO.COM", "LAURA.MARTINEZ@CORREO.COM", "AQAAAAIAAYagAAAAEJU2t42NQHegCsmANmkKd/DomzAWp/aibt43fz5ITTXd86M7TYfe5rQaIkIYcBEegA==", null, false, "55555555-5555-5555-5555-000000000010", false, "laura.martinez@correo.com" },
                    { "55555555-5555-5555-5555-000000000011", 0, "55555555-5555-5555-5555-000000000011", "andres.salazar@correo.com", true, false, null, "ANDRES.SALAZAR@CORREO.COM", "ANDRES.SALAZAR@CORREO.COM", "AQAAAAIAAYagAAAAEH1au0HLr9F16z+eTbJKOIvjbH8By9iDEXLqY93JrvJXA6pjG4v/N3pxzTFgaqzWIQ==", null, false, "55555555-5555-5555-5555-000000000011", false, "andres.salazar@correo.com" },
                    { "55555555-5555-5555-5555-000000000012", 0, "55555555-5555-5555-5555-000000000012", "camila.cardenas@correo.com", true, false, null, "CAMILA.CARDENAS@CORREO.COM", "CAMILA.CARDENAS@CORREO.COM", "AQAAAAIAAYagAAAAEEA8IftaQKn3//KKmeLgdAK58afv+/rkQZ+h/VMCVNKvk1ip7rJeGRC+DpEHV+0Crg==", null, false, "55555555-5555-5555-5555-000000000012", false, "camila.cardenas@correo.com" },
                    { "55555555-5555-5555-5555-000000000013", 0, "55555555-5555-5555-5555-000000000013", "diego.munoz@correo.com", true, false, null, "DIEGO.MUNOZ@CORREO.COM", "DIEGO.MUNOZ@CORREO.COM", "AQAAAAIAAYagAAAAEJddm1Ai7lZ97VRiwpFRLr6DI3ZW9Yqcio2/t0PSRbiGSIxmBDSQiniCiREzQG4bWQ==", null, false, "55555555-5555-5555-5555-000000000013", false, "diego.munoz@correo.com" },
                    { "55555555-5555-5555-5555-000000000014", 0, "55555555-5555-5555-5555-000000000014", "juliana.ortega@correo.com", true, false, null, "JULIANA.ORTEGA@CORREO.COM", "JULIANA.ORTEGA@CORREO.COM", "AQAAAAIAAYagAAAAENG/ULo+2VCpHFzd4kFb9rR5v3FOWwdSNFuzDopgm2jWGWfv+iHeUcgbu3+U3HMP7A==", null, false, "55555555-5555-5555-5555-000000000014", false, "juliana.ortega@correo.com" },
                    { "55555555-5555-5555-5555-000000000015", 0, "55555555-5555-5555-5555-000000000015", "felipe.agudelo@correo.com", true, false, null, "FELIPE.AGUDELO@CORREO.COM", "FELIPE.AGUDELO@CORREO.COM", "AQAAAAIAAYagAAAAEOzV853YX0eOoI7n1PkVVC32JYXL9ZNH9/V3hcFdIOxv6jiF2v0uUGPEXrc1xezTJA==", null, false, "55555555-5555-5555-5555-000000000015", false, "felipe.agudelo@correo.com" },
                    { "55555555-5555-5555-5555-000000000016", 0, "55555555-5555-5555-5555-000000000016", "natalia.montoya@correo.com", true, false, null, "NATALIA.MONTOYA@CORREO.COM", "NATALIA.MONTOYA@CORREO.COM", "AQAAAAIAAYagAAAAEBHvogT9PMixdSjA/fR47Yo66YVaBCkLuMoZuqEW/u4/OYb4tEqNkG/d0zEQLqk/Fw==", null, false, "55555555-5555-5555-5555-000000000016", false, "natalia.montoya@correo.com" },
                    { "55555555-5555-5555-5555-000000000017", 0, "55555555-5555-5555-5555-000000000017", "oscar.guerrero@correo.com", true, false, null, "OSCAR.GUERRERO@CORREO.COM", "OSCAR.GUERRERO@CORREO.COM", "AQAAAAIAAYagAAAAEPhbwyEIxInQqz2ZNq8RJIDOQhE0dhIp7C+xnRQVCbIEJ0m16lMfOtowMMMXFug3qA==", null, false, "55555555-5555-5555-5555-000000000017", false, "oscar.guerrero@correo.com" },
                    { "55555555-5555-5555-5555-000000000018", 0, "55555555-5555-5555-5555-000000000018", "paula.zapata@correo.com", true, false, null, "PAULA.ZAPATA@CORREO.COM", "PAULA.ZAPATA@CORREO.COM", "AQAAAAIAAYagAAAAEHRNZvDGI4jGuXHWQRcW4XyB6q74+SRxSqwXmthOeCPX8IvrMbpKxj452LpouBtzNw==", null, false, "55555555-5555-5555-5555-000000000018", false, "paula.zapata@correo.com" },
                    { "55555555-5555-5555-5555-000000000019", 0, "55555555-5555-5555-5555-000000000019", "cristian.becerra@correo.com", true, false, null, "CRISTIAN.BECERRA@CORREO.COM", "CRISTIAN.BECERRA@CORREO.COM", "AQAAAAIAAYagAAAAEOKV5mc3wmf7gCbNrZIgiwHWJm3fTDtu2GBOn76d+sXTZqCqkxIgrVOqJpf4rD7aoA==", null, false, "55555555-5555-5555-5555-000000000019", false, "cristian.becerra@correo.com" },
                    { "55555555-5555-5555-5555-000000000020", 0, "55555555-5555-5555-5555-000000000020", "mariana.londono@correo.com", true, false, null, "MARIANA.LONDONO@CORREO.COM", "MARIANA.LONDONO@CORREO.COM", "AQAAAAIAAYagAAAAENFNk2/7C5uai/0LxAKOwrXyg/2S+UrBZ2PIINccdKOK+JC+VQffYCGk/4M+1UIy8A==", null, false, "55555555-5555-5555-5555-000000000020", false, "mariana.londono@correo.com" },
                    { "55555555-5555-5555-5555-000000000021", 0, "55555555-5555-5555-5555-000000000021", "javier.cuellar@correo.com", true, false, null, "JAVIER.CUELLAR@CORREO.COM", "JAVIER.CUELLAR@CORREO.COM", "AQAAAAIAAYagAAAAECtlmxfg9ka9xLYi27QAIAwmfNDZ+jMpMGajWlxT7+gsf2g8qg4wMwB2CCxcUEjSTA==", null, false, "55555555-5555-5555-5555-000000000021", false, "javier.cuellar@correo.com" },
                    { "55555555-5555-5555-5555-000000000022", 0, "55555555-5555-5555-5555-000000000022", "isabella.navarro@correo.com", true, false, null, "ISABELLA.NAVARRO@CORREO.COM", "ISABELLA.NAVARRO@CORREO.COM", "AQAAAAIAAYagAAAAEGecuLVKTOk6WbeARXWQ4H3HOJadnjNmOje/m5SRh8XBnpNyPr5YMTbRltFpNY/2Ew==", null, false, "55555555-5555-5555-5555-000000000022", false, "isabella.navarro@correo.com" },
                    { "55555555-5555-5555-5555-000000000023", 0, "55555555-5555-5555-5555-000000000023", "kevin.mosquera@correo.com", true, false, null, "KEVIN.MOSQUERA@CORREO.COM", "KEVIN.MOSQUERA@CORREO.COM", "AQAAAAIAAYagAAAAEDQw+03fie6NRLvbFujgBf/r1MPnI+8VRGMM8WJp2KIawPY6NIvGdjrfdDPmYw+V0Q==", null, false, "55555555-5555-5555-5555-000000000023", false, "kevin.mosquera@correo.com" },
                    { "55555555-5555-5555-5555-000000000024", 0, "55555555-5555-5555-5555-000000000024", "ana.jaramillo@correo.com", true, false, null, "ANA.JARAMILLO@CORREO.COM", "ANA.JARAMILLO@CORREO.COM", "AQAAAAIAAYagAAAAEE+y8yIGJuBaYb08LnF/pCWIsZUKxF6yGqNJ/7cfzeoezuFiS6cWo+TuSb6iQtc8pg==", null, false, "55555555-5555-5555-5555-000000000024", false, "ana.jaramillo@correo.com" },
                    { "55555555-5555-5555-5555-000000000025", 0, "55555555-5555-5555-5555-000000000025", "luis.arias@correo.com", true, false, null, "LUIS.ARIAS@CORREO.COM", "LUIS.ARIAS@CORREO.COM", "AQAAAAIAAYagAAAAELomLWa6IkuR2s/7MC99UN1eYYQSRqyYSXHQuYR38ga2SsgCyipxEnnBc+oWoBx/Gg==", null, false, "55555555-5555-5555-5555-000000000025", false, "luis.arias@correo.com" },
                    { "55555555-5555-5555-5555-000000000026", 0, "55555555-5555-5555-5555-000000000026", "sara.valencia@correo.com", true, false, null, "SARA.VALENCIA@CORREO.COM", "SARA.VALENCIA@CORREO.COM", "AQAAAAIAAYagAAAAEDq50h2AzQOiQHp/FvKh0HoekfZRR6JMQEEfPtGB+lrnISwoOWmgE9w9BX6qoSm/kA==", null, false, "55555555-5555-5555-5555-000000000026", false, "sara.valencia@correo.com" },
                    { "55555555-5555-5555-5555-000000000027", 0, "55555555-5555-5555-5555-000000000027", "esteban.rodriguez@correo.com", true, false, null, "ESTEBAN.RODRIGUEZ@CORREO.COM", "ESTEBAN.RODRIGUEZ@CORREO.COM", "AQAAAAIAAYagAAAAEGuO/Uhn7fJJzPrd+3woZDe2C8VVf872i4IOeP0E32+O2FVui/en8ib3hVJHZTnIfQ==", null, false, "55555555-5555-5555-5555-000000000027", false, "esteban.rodriguez@correo.com" },
                    { "55555555-5555-5555-5555-000000000028", 0, "55555555-5555-5555-5555-000000000028", "manuela.escobar@correo.com", true, false, null, "MANUELA.ESCOBAR@CORREO.COM", "MANUELA.ESCOBAR@CORREO.COM", "AQAAAAIAAYagAAAAEKDbbhLBXMFb/jKXdaM6d2xSzUj3pGlOhoGFQUbnLWKiWelz/M9mMy7UPTsgnfTMJA==", null, false, "55555555-5555-5555-5555-000000000028", false, "manuela.escobar@correo.com" },
                    { "55555555-5555-5555-5555-000000000029", 0, "55555555-5555-5555-5555-000000000029", "wilmer.benitez@correo.com", true, false, null, "WILMER.BENITEZ@CORREO.COM", "WILMER.BENITEZ@CORREO.COM", "AQAAAAIAAYagAAAAEDKrK8l+HpuVPd1/jezAqwjRWOLBKeqNPOQuTUMzRQZcSUnlBm7SWjemFIHX/pjD6A==", null, false, "55555555-5555-5555-5555-000000000029", false, "wilmer.benitez@correo.com" },
                    { "55555555-5555-5555-5555-000000000030", 0, "55555555-5555-5555-5555-000000000030", "yuliana.patino@correo.com", true, false, null, "YULIANA.PATINO@CORREO.COM", "YULIANA.PATINO@CORREO.COM", "AQAAAAIAAYagAAAAECE/YgJmFN/dMjmRtJnROUqk3W22l5FbhBH1n8Ki48yYmJfdK7kwYaFkKuSFw3OaGA==", null, false, "55555555-5555-5555-5555-000000000030", false, "yuliana.patino@correo.com" }
                });

            migrationBuilder.InsertData(
                table: "Instructors",
                columns: new[] { "Id", "Email", "FullName", "IdentityUserId", "Specialty" },
                values: new object[,]
                {
                    { new Guid("44444444-4444-4444-4444-000000000001"), "andres.mejia@correo.com", "Andrés Felipe Mejía Arango", "33333333-3333-3333-3333-000000000001", "Fotografía" },
                    { new Guid("44444444-4444-4444-4444-000000000002"), "rosa.barrios@correo.com", "Rosa Elena Barrios Polo", "33333333-3333-3333-3333-000000000002", "Cocina" },
                    { new Guid("44444444-4444-4444-4444-000000000003"), "mauricio.jimenez@correo.com", "Mauricio Jiménez Cárdenas", "33333333-3333-3333-3333-000000000003", "Oratoria" },
                    { new Guid("44444444-4444-4444-4444-000000000004"), "natalia.herrera@correo.com", "Natalia Herrera Castaño", "33333333-3333-3333-3333-000000000004", "Diseño gráfico" },
                    { new Guid("44444444-4444-4444-4444-000000000005"), "hernan.quintero@correo.com", "Hernán Darío Quintero Gil", "33333333-3333-3333-3333-000000000005", "Música" },
                    { new Guid("44444444-4444-4444-4444-000000000006"), "paola.vargas@correo.com", "Paola Andrea Vargas Mora", "33333333-3333-3333-3333-000000000006", "Marketing digital" }
                });

            migrationBuilder.InsertData(
                table: "Participants",
                columns: new[] { "Id", "Email", "FullName", "IdentityUserId" },
                values: new object[,]
                {
                    { new Guid("66666666-6666-6666-6666-000000000001"), "jorge.perez@correo.com", "Jorge Andrés Pérez Rojas", "55555555-5555-5555-5555-000000000001" },
                    { new Guid("66666666-6666-6666-6666-000000000002"), "luz.ospina@correo.com", "Luz Marina Ospina Ríos", "55555555-5555-5555-5555-000000000002" },
                    { new Guid("66666666-6666-6666-6666-000000000003"), "carlos.rincon@correo.com", "Carlos Eduardo Rincón Díaz", "55555555-5555-5555-5555-000000000003" },
                    { new Guid("66666666-6666-6666-6666-000000000004"), "valentina.gomez@correo.com", "Valentina Gómez Ruiz", "55555555-5555-5555-5555-000000000004" },
                    { new Guid("66666666-6666-6666-6666-000000000005"), "juan.restrepo@correo.com", "Juan Camilo Restrepo Vélez", "55555555-5555-5555-5555-000000000005" },
                    { new Guid("66666666-6666-6666-6666-000000000006"), "maria.torres@correo.com", "María Fernanda Torres León", "55555555-5555-5555-5555-000000000006" },
                    { new Guid("66666666-6666-6666-6666-000000000007"), "santiago.ramirez@correo.com", "Santiago Ramírez Henao", "55555555-5555-5555-5555-000000000007" },
                    { new Guid("66666666-6666-6666-6666-000000000008"), "daniela.morales@correo.com", "Daniela Morales Pineda", "55555555-5555-5555-5555-000000000008" },
                    { new Guid("66666666-6666-6666-6666-000000000009"), "sebastian.castro@correo.com", "Sebastián Castro Duque", "55555555-5555-5555-5555-000000000009" },
                    { new Guid("66666666-6666-6666-6666-000000000010"), "laura.martinez@correo.com", "Laura Sofía Martínez Suárez", "55555555-5555-5555-5555-000000000010" },
                    { new Guid("66666666-6666-6666-6666-000000000011"), "andres.salazar@correo.com", "Andrés Mauricio Salazar Ortiz", "55555555-5555-5555-5555-000000000011" },
                    { new Guid("66666666-6666-6666-6666-000000000012"), "camila.cardenas@correo.com", "Camila Andrea Cárdenas Ruiz", "55555555-5555-5555-5555-000000000012" },
                    { new Guid("66666666-6666-6666-6666-000000000013"), "diego.munoz@correo.com", "Diego Alejandro Muñoz Peña", "55555555-5555-5555-5555-000000000013" },
                    { new Guid("66666666-6666-6666-6666-000000000014"), "juliana.ortega@correo.com", "Juliana Ortega Sánchez", "55555555-5555-5555-5555-000000000014" },
                    { new Guid("66666666-6666-6666-6666-000000000015"), "felipe.agudelo@correo.com", "Felipe Agudelo Marín", "55555555-5555-5555-5555-000000000015" },
                    { new Guid("66666666-6666-6666-6666-000000000016"), "natalia.montoya@correo.com", "Natalia Montoya Correa", "55555555-5555-5555-5555-000000000016" },
                    { new Guid("66666666-6666-6666-6666-000000000017"), "oscar.guerrero@correo.com", "Óscar Iván Guerrero Lozano", "55555555-5555-5555-5555-000000000017" },
                    { new Guid("66666666-6666-6666-6666-000000000018"), "paula.zapata@correo.com", "Paula Andrea Zapata Ríos", "55555555-5555-5555-5555-000000000018" },
                    { new Guid("66666666-6666-6666-6666-000000000019"), "cristian.becerra@correo.com", "Cristian David Becerra Mejía", "55555555-5555-5555-5555-000000000019" },
                    { new Guid("66666666-6666-6666-6666-000000000020"), "mariana.londono@correo.com", "Mariana Londoño Álvarez", "55555555-5555-5555-5555-000000000020" },
                    { new Guid("66666666-6666-6666-6666-000000000021"), "javier.cuellar@correo.com", "Javier Alonso Cuéllar Pardo", "55555555-5555-5555-5555-000000000021" },
                    { new Guid("66666666-6666-6666-6666-000000000022"), "isabella.navarro@correo.com", "Isabella Navarro Fuentes", "55555555-5555-5555-5555-000000000022" },
                    { new Guid("66666666-6666-6666-6666-000000000023"), "kevin.mosquera@correo.com", "Kevin Stiven Mosquera Palacios", "55555555-5555-5555-5555-000000000023" },
                    { new Guid("66666666-6666-6666-6666-000000000024"), "ana.jaramillo@correo.com", "Ana María Jaramillo Gaviria", "55555555-5555-5555-5555-000000000024" },
                    { new Guid("66666666-6666-6666-6666-000000000025"), "luis.arias@correo.com", "Luis Fernando Arias Cano", "55555555-5555-5555-5555-000000000025" },
                    { new Guid("66666666-6666-6666-6666-000000000026"), "sara.valencia@correo.com", "Sara Valencia Bermúdez", "55555555-5555-5555-5555-000000000026" },
                    { new Guid("66666666-6666-6666-6666-000000000027"), "esteban.rodriguez@correo.com", "Esteban Rodríguez Molina", "55555555-5555-5555-5555-000000000027" },
                    { new Guid("66666666-6666-6666-6666-000000000028"), "manuela.escobar@correo.com", "Manuela Escobar Toro", "55555555-5555-5555-5555-000000000028" },
                    { new Guid("66666666-6666-6666-6666-000000000029"), "wilmer.benitez@correo.com", "Wilmer Alfonso Benítez Cruz", "55555555-5555-5555-5555-000000000029" },
                    { new Guid("66666666-6666-6666-6666-000000000030"), "yuliana.patino@correo.com", "Yuliana Patiño Osorio", "55555555-5555-5555-5555-000000000030" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "11111111-1111-1111-1111-111111111111", "33333333-3333-3333-3333-000000000001" },
                    { "11111111-1111-1111-1111-111111111111", "33333333-3333-3333-3333-000000000002" },
                    { "11111111-1111-1111-1111-111111111111", "33333333-3333-3333-3333-000000000003" },
                    { "11111111-1111-1111-1111-111111111111", "33333333-3333-3333-3333-000000000004" },
                    { "11111111-1111-1111-1111-111111111111", "33333333-3333-3333-3333-000000000005" },
                    { "11111111-1111-1111-1111-111111111111", "33333333-3333-3333-3333-000000000006" },
                    { "22222222-2222-2222-2222-222222222222", "55555555-5555-5555-5555-000000000001" },
                    { "22222222-2222-2222-2222-222222222222", "55555555-5555-5555-5555-000000000002" },
                    { "22222222-2222-2222-2222-222222222222", "55555555-5555-5555-5555-000000000003" },
                    { "22222222-2222-2222-2222-222222222222", "55555555-5555-5555-5555-000000000004" },
                    { "22222222-2222-2222-2222-222222222222", "55555555-5555-5555-5555-000000000005" },
                    { "22222222-2222-2222-2222-222222222222", "55555555-5555-5555-5555-000000000006" },
                    { "22222222-2222-2222-2222-222222222222", "55555555-5555-5555-5555-000000000007" },
                    { "22222222-2222-2222-2222-222222222222", "55555555-5555-5555-5555-000000000008" },
                    { "22222222-2222-2222-2222-222222222222", "55555555-5555-5555-5555-000000000009" },
                    { "22222222-2222-2222-2222-222222222222", "55555555-5555-5555-5555-000000000010" },
                    { "22222222-2222-2222-2222-222222222222", "55555555-5555-5555-5555-000000000011" },
                    { "22222222-2222-2222-2222-222222222222", "55555555-5555-5555-5555-000000000012" },
                    { "22222222-2222-2222-2222-222222222222", "55555555-5555-5555-5555-000000000013" },
                    { "22222222-2222-2222-2222-222222222222", "55555555-5555-5555-5555-000000000014" },
                    { "22222222-2222-2222-2222-222222222222", "55555555-5555-5555-5555-000000000015" },
                    { "22222222-2222-2222-2222-222222222222", "55555555-5555-5555-5555-000000000016" },
                    { "22222222-2222-2222-2222-222222222222", "55555555-5555-5555-5555-000000000017" },
                    { "22222222-2222-2222-2222-222222222222", "55555555-5555-5555-5555-000000000018" },
                    { "22222222-2222-2222-2222-222222222222", "55555555-5555-5555-5555-000000000019" },
                    { "22222222-2222-2222-2222-222222222222", "55555555-5555-5555-5555-000000000020" },
                    { "22222222-2222-2222-2222-222222222222", "55555555-5555-5555-5555-000000000021" },
                    { "22222222-2222-2222-2222-222222222222", "55555555-5555-5555-5555-000000000022" },
                    { "22222222-2222-2222-2222-222222222222", "55555555-5555-5555-5555-000000000023" },
                    { "22222222-2222-2222-2222-222222222222", "55555555-5555-5555-5555-000000000024" },
                    { "22222222-2222-2222-2222-222222222222", "55555555-5555-5555-5555-000000000025" },
                    { "22222222-2222-2222-2222-222222222222", "55555555-5555-5555-5555-000000000026" },
                    { "22222222-2222-2222-2222-222222222222", "55555555-5555-5555-5555-000000000027" },
                    { "22222222-2222-2222-2222-222222222222", "55555555-5555-5555-5555-000000000028" },
                    { "22222222-2222-2222-2222-222222222222", "55555555-5555-5555-5555-000000000029" },
                    { "22222222-2222-2222-2222-222222222222", "55555555-5555-5555-5555-000000000030" }
                });

            migrationBuilder.InsertData(
                table: "Workshops",
                columns: new[] { "Id", "Code", "Description", "InstructorId", "MaxCapacity", "Title", "EndDate", "StartDate" },
                values: new object[,]
                {
                    { new Guid("77777777-7777-7777-7777-000000000001"), "FOT-001", "Composición, luz y edición básica desde el celular", new Guid("44444444-4444-4444-4444-000000000001"), 15, "Fotografía con celular", new DateTime(2026, 12, 5, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 5, 8, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("77777777-7777-7777-7777-000000000002"), "FOT-002", "Manejo de luz natural para retratos al aire libre", new Guid("44444444-4444-4444-4444-000000000001"), 10, "Retrato en exteriores", new DateTime(2027, 1, 16, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2027, 1, 16, 8, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("77777777-7777-7777-7777-000000000003"), "FOT-003", "Flujo de revelado y ajustes de color", new Guid("44444444-4444-4444-4444-000000000001"), 12, "Edición en Lightroom", new DateTime(2027, 2, 6, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2027, 2, 6, 14, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("77777777-7777-7777-7777-000000000004"), "COC-001", "Arroz con coco, patacones y mojarra frita", new Guid("44444444-4444-4444-4444-000000000002"), 12, "Cocina costeña", new DateTime(2026, 12, 5, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 5, 14, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("77777777-7777-7777-7777-000000000005"), "COC-002", "Tortas, galletas y postres de nevera", new Guid("44444444-4444-4444-4444-000000000002"), 10, "Repostería básica", new DateTime(2027, 1, 9, 13, 0, 0, 0, DateTimeKind.Utc), new DateTime(2027, 1, 9, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("77777777-7777-7777-7777-000000000006"), "COC-003", "Platos balanceados para la semana", new Guid("44444444-4444-4444-4444-000000000002"), 14, "Cocina saludable", new DateTime(2027, 2, 13, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2027, 2, 13, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("77777777-7777-7777-7777-000000000007"), "ORA-001", "Técnicas para hablar en público con seguridad", new Guid("44444444-4444-4444-4444-000000000003"), 20, "Oratoria para principiantes", new DateTime(2026, 12, 12, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 12, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("77777777-7777-7777-7777-000000000008"), "ORA-002", "Cómo estructurar y presentar una idea en 10 minutos", new Guid("44444444-4444-4444-4444-000000000003"), 18, "Presentaciones efectivas", new DateTime(2027, 1, 23, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2027, 1, 23, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("77777777-7777-7777-7777-000000000009"), "DIS-001", "Piezas gráficas para redes sociales", new Guid("44444444-4444-4444-4444-000000000004"), 18, "Diseño en Canva", new DateTime(2026, 12, 19, 17, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 19, 14, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("77777777-7777-7777-7777-000000000010"), "DIS-002", "Principios visuales para piezas profesionales", new Guid("44444444-4444-4444-4444-000000000004"), 15, "Fundamentos de color y tipografía", new DateTime(2027, 1, 30, 17, 0, 0, 0, DateTimeKind.Utc), new DateTime(2027, 1, 30, 14, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("77777777-7777-7777-7777-000000000011"), "MUS-001", "Acordes básicos y primeras canciones", new Guid("44444444-4444-4444-4444-000000000005"), 10, "Guitarra desde cero", new DateTime(2026, 12, 12, 17, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 12, 14, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("77777777-7777-7777-7777-000000000012"), "MUS-002", "Grabación y mezcla con herramientas gratuitas", new Guid("44444444-4444-4444-4444-000000000005"), 12, "Producción musical en casa", new DateTime(2027, 2, 20, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2027, 2, 20, 14, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("77777777-7777-7777-7777-000000000013"), "MKT-001", "Estrategia de contenido para pequeños negocios", new Guid("44444444-4444-4444-4444-000000000006"), 20, "Marketing en redes sociales", new DateTime(2026, 12, 19, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 19, 8, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("77777777-7777-7777-7777-000000000014"), "MKT-002", "Configuración y medición de campañas", new Guid("44444444-4444-4444-4444-000000000006"), 16, "Publicidad en Meta e Instagram", new DateTime(2027, 1, 16, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2027, 1, 16, 14, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("77777777-7777-7777-7777-000000000015"), "MKT-003", "Validar y lanzar una idea de negocio en línea", new Guid("44444444-4444-4444-4444-000000000006"), 20, "Emprendimiento digital", new DateTime(2027, 2, 27, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2027, 2, 27, 9, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

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
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_ParticipantId_WorkshopId",
                table: "Enrollments",
                columns: new[] { "ParticipantId", "WorkshopId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_WorkshopId",
                table: "Enrollments",
                column: "WorkshopId");

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_Email",
                table: "Instructors",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Participants_Email",
                table: "Participants",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workshops_Code",
                table: "Workshops",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workshops_InstructorId",
                table: "Workshops",
                column: "InstructorId");
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
                name: "Enrollments");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropTable(
                name: "Workshops");

            migrationBuilder.DropTable(
                name: "Instructors");
        }
    }
}
