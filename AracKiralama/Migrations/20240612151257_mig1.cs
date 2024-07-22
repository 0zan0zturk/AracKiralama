using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AracKiralama.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Araclar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AracPlaka = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AracMarka = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AracModel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AracYil = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AracYakit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AracSanziman = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AracMusait = table.Column<bool>(type: "bit", nullable: false),
                    MusteriId = table.Column<int>(type: "int", nullable: true),
                    SubeId = table.Column<int>(type: "int", nullable: true),
                    MotorHacmi = table.Column<int>(type: "int", nullable: true),
                    AracFotograf = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GunlukFiyat = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Araclar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Musteriler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MusteriSifre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MusteriTc = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MusteriAd = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MusteriSoyad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EPosta = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MusteriDogTar = table.Column<DateOnly>(type: "date", nullable: false),
                    MusteriAdres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AracId = table.Column<int>(type: "int", nullable: true),
                    SubeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Musteriler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sehirler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SehirAdi = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sehirler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subeler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubeAdi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubeAdresi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SehirId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subeler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subeler_Sehirler_SehirId",
                        column: x => x.SehirId,
                        principalTable: "Sehirler",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "KiralamaIslemleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KiralamaBaslangicTarihi = table.Column<DateOnly>(type: "date", nullable: false),
                    KiralamaBaslangicSaati = table.Column<int>(type: "int", nullable: false),
                    KiralamaBitisTarihi = table.Column<DateOnly>(type: "date", nullable: false),
                    KiralamaBitisSaati = table.Column<int>(type: "int", nullable: false),
                    MusteriId = table.Column<int>(type: "int", nullable: false),
                    AracId = table.Column<int>(type: "int", nullable: false),
                    SubeId = table.Column<int>(type: "int", nullable: false),
                    SehirId = table.Column<int>(type: "int", nullable: false),
                    IslemZamani = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KiralananGunSayisi = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KiralamaIslemleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KiralamaIslemleri_Araclar_AracId",
                        column: x => x.AracId,
                        principalTable: "Araclar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KiralamaIslemleri_Musteriler_MusteriId",
                        column: x => x.MusteriId,
                        principalTable: "Musteriler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KiralamaIslemleri_Sehirler_SehirId",
                        column: x => x.SehirId,
                        principalTable: "Sehirler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KiralamaIslemleri_Subeler_SubeId",
                        column: x => x.SubeId,
                        principalTable: "Subeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KiralamaIslemleri_AracId",
                table: "KiralamaIslemleri",
                column: "AracId");

            migrationBuilder.CreateIndex(
                name: "IX_KiralamaIslemleri_MusteriId",
                table: "KiralamaIslemleri",
                column: "MusteriId");

            migrationBuilder.CreateIndex(
                name: "IX_KiralamaIslemleri_SehirId",
                table: "KiralamaIslemleri",
                column: "SehirId");

            migrationBuilder.CreateIndex(
                name: "IX_KiralamaIslemleri_SubeId",
                table: "KiralamaIslemleri",
                column: "SubeId");

            migrationBuilder.CreateIndex(
                name: "IX_Musteriler_EPosta",
                table: "Musteriler",
                column: "EPosta",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Musteriler_MusteriTc",
                table: "Musteriler",
                column: "MusteriTc",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Musteriler_Telefon",
                table: "Musteriler",
                column: "Telefon",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subeler_SehirId",
                table: "Subeler",
                column: "SehirId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KiralamaIslemleri");

            migrationBuilder.DropTable(
                name: "Araclar");

            migrationBuilder.DropTable(
                name: "Musteriler");

            migrationBuilder.DropTable(
                name: "Subeler");

            migrationBuilder.DropTable(
                name: "Sehirler");
        }
    }
}
