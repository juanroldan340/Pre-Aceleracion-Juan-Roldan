using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisneyWebAPI.Migrations
{
    public partial class DisneyFirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    History = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.CharacterId);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    GenreId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.GenreId);
                });

            migrationBuilder.CreateTable(
                name: "MoviesOrSeries",
                columns: table => new
                {
                    MovieOrSerieId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GenreId = table.Column<int>(type: "int", nullable: true),
                    Qualification = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoviesOrSeries", x => x.MovieOrSerieId);
                    table.ForeignKey(
                        name: "FK_MoviesOrSeries_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "GenreId");
                });

            migrationBuilder.CreateTable(
                name: "CharactersMoviesOrSeries",
                columns: table => new
                {
                    CharactersId = table.Column<int>(type: "int", nullable: false),
                    MoviesOrSeriesId = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    MovieOrSerieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharactersMoviesOrSeries", x => new { x.CharactersId, x.MoviesOrSeriesId });
                    table.ForeignKey(
                        name: "FK_CharactersMoviesOrSeries_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "CharacterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharactersMoviesOrSeries_MoviesOrSeries_MovieOrSerieId",
                        column: x => x.MovieOrSerieId,
                        principalTable: "MoviesOrSeries",
                        principalColumn: "MovieOrSerieId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "GenreId", "Image", "Name" },
                values: new object[,]
                {
                    { 1, null, "Aventura" },
                    { 2, null, "Romántica" },
                    { 3, null, "Acción" },
                    { 4, null, "Ciencia Ficción" },
                    { 5, null, "Comedia" },
                    { 6, null, "Terror" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharactersMoviesOrSeries_CharacterId",
                table: "CharactersMoviesOrSeries",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharactersMoviesOrSeries_MovieOrSerieId",
                table: "CharactersMoviesOrSeries",
                column: "MovieOrSerieId");

            migrationBuilder.CreateIndex(
                name: "IX_MoviesOrSeries_GenreId",
                table: "MoviesOrSeries",
                column: "GenreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharactersMoviesOrSeries");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "MoviesOrSeries");

            migrationBuilder.DropTable(
                name: "Genres");
        }
    }
}
