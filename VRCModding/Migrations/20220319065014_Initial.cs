using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VRCModding.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DisplayNames",
                columns: table => new
                {
                    Name = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisplayNames", x => x.Name);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastLogin = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreationDateUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserFK = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    LastLogin = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DisplayNameFK = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_DisplayNames_DisplayNameFK",
                        column: x => x.DisplayNameFK,
                        principalTable: "DisplayNames",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Accounts_Users_UserFK",
                        column: x => x.UserFK,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Hwids",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserFK = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    LastLogin = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hwids", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hwids_Users_UserFK",
                        column: x => x.UserFK,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ips",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserFK = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    LastLogin = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ips_Users_UserFK",
                        column: x => x.UserFK,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UsedDisplayNames",
                columns: table => new
                {
                    AccountFK = table.Column<string>(type: "varchar(40)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayNameFK = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FirstSeen = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastUsage = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsedDisplayNames", x => new { x.AccountFK, x.DisplayNameFK });
                    table.ForeignKey(
                        name: "FK_UsedDisplayNames_Accounts_AccountFK",
                        column: x => x.AccountFK,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsedDisplayNames_DisplayNames_DisplayNameFK",
                        column: x => x.DisplayNameFK,
                        principalTable: "DisplayNames",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_DisplayNameFK",
                table: "Accounts",
                column: "DisplayNameFK",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserFK",
                table: "Accounts",
                column: "UserFK");

            migrationBuilder.CreateIndex(
                name: "IX_Hwids_UserFK",
                table: "Hwids",
                column: "UserFK");

            migrationBuilder.CreateIndex(
                name: "IX_Ips_UserFK",
                table: "Ips",
                column: "UserFK");

            migrationBuilder.CreateIndex(
                name: "IX_UsedDisplayNames_DisplayNameFK",
                table: "UsedDisplayNames",
                column: "DisplayNameFK");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hwids");

            migrationBuilder.DropTable(
                name: "Ips");

            migrationBuilder.DropTable(
                name: "UsedDisplayNames");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "DisplayNames");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
