using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace YBP.Framework.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "YbpProcesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    InstanceId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Pfx = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YbpProcesses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "YbpActionHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FinishedUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAuthorized = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Params = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessId = table.Column<int>(type: "int", nullable: false),
                    Results = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartedUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Succeed = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YbpActionHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YbpActionHistory_YbpProcesses_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "YbpProcesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "YbpFlags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsSet = table.Column<bool>(type: "bit", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ProcessId = table.Column<int>(type: "int", nullable: false),
                    UpdatedUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YbpFlags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YbpFlags_YbpProcesses_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "YbpProcesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "YbpFlagHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FlagId = table.Column<int>(type: "int", nullable: false),
                    IsSet = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YbpFlagHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YbpFlagHistory_YbpFlags_FlagId",
                        column: x => x.FlagId,
                        principalTable: "YbpFlags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ybp_ActionHistory_Name",
                table: "YbpActionHistory",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_YbpActionHistory_ProcessId",
                table: "YbpActionHistory",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_YbpFlagHistory_FlagId",
                table: "YbpFlagHistory",
                column: "FlagId");

            migrationBuilder.CreateIndex(
                name: "IX_YbpFlags_ProcessId",
                table: "YbpFlags",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_Ybp_Process",
                table: "YbpProcesses",
                columns: new[] { "Pfx", "InstanceId" },
                unique: true,
                filter: "[Pfx] IS NOT NULL AND [InstanceId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "YbpActionHistory");

            migrationBuilder.DropTable(
                name: "YbpFlagHistory");

            migrationBuilder.DropTable(
                name: "YbpFlags");

            migrationBuilder.DropTable(
                name: "YbpProcesses");
        }
    }
}
