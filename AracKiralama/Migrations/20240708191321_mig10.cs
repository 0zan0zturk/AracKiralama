using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AracKiralama.Migrations
{
    /// <inheritdoc />
    public partial class mig10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GirisSayisi",
                table: "Musteriler",
                newName: "MusteriGirisId");

            migrationBuilder.AddColumn<int>(
                name: "GirisBilgisiId",
                table: "Musteriler",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MusteriGirisleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MusteriId = table.Column<int>(type: "int", nullable: false),
                    GirisZamani = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CikisZamani = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusteriGirisleri", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Musteriler_GirisBilgisiId",
                table: "Musteriler",
                column: "GirisBilgisiId");

            migrationBuilder.AddForeignKey(
                name: "FK_Musteriler_MusteriGirisleri_GirisBilgisiId",
                table: "Musteriler",
                column: "GirisBilgisiId",
                principalTable: "MusteriGirisleri",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Musteriler_MusteriGirisleri_GirisBilgisiId",
                table: "Musteriler");

            migrationBuilder.DropTable(
                name: "MusteriGirisleri");

            migrationBuilder.DropIndex(
                name: "IX_Musteriler_GirisBilgisiId",
                table: "Musteriler");

            migrationBuilder.DropColumn(
                name: "GirisBilgisiId",
                table: "Musteriler");

            migrationBuilder.RenameColumn(
                name: "MusteriGirisId",
                table: "Musteriler",
                newName: "GirisSayisi");
        }
    }
}
