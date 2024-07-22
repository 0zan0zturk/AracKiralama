using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AracKiralama.Migrations
{
    /// <inheritdoc />
    public partial class mig5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SonAracId",
                table: "Musteriler",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Musteriler_SonAracId",
                table: "Musteriler",
                column: "SonAracId");

            migrationBuilder.AddForeignKey(
                name: "FK_Musteriler_Araclar_SonAracId",
                table: "Musteriler",
                column: "SonAracId",
                principalTable: "Araclar",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Musteriler_Araclar_SonAracId",
                table: "Musteriler");

            migrationBuilder.DropIndex(
                name: "IX_Musteriler_SonAracId",
                table: "Musteriler");

            migrationBuilder.DropColumn(
                name: "SonAracId",
                table: "Musteriler");
        }
    }
}
