using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AracKiralama.Migrations
{
    /// <inheritdoc />
    public partial class mig12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KiralamaIslemleri_Subeler_SubeId",
                table: "KiralamaIslemleri");

            migrationBuilder.AlterColumn<string>(
                name: "MusteriSifre",
                table: "Musteriler",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "SubeId",
                table: "KiralamaIslemleri",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Sanzimanlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SanzimanTipi = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sanzimanlar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Yakitlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YakitTipi = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Yakitlar", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_KiralamaIslemleri_Subeler_SubeId",
                table: "KiralamaIslemleri",
                column: "SubeId",
                principalTable: "Subeler",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KiralamaIslemleri_Subeler_SubeId",
                table: "KiralamaIslemleri");

            migrationBuilder.DropTable(
                name: "Sanzimanlar");

            migrationBuilder.DropTable(
                name: "Yakitlar");

            migrationBuilder.AlterColumn<string>(
                name: "MusteriSifre",
                table: "Musteriler",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<int>(
                name: "SubeId",
                table: "KiralamaIslemleri",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_KiralamaIslemleri_Subeler_SubeId",
                table: "KiralamaIslemleri",
                column: "SubeId",
                principalTable: "Subeler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
