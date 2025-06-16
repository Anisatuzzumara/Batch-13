using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStoreMVC.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    AuthorID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Bio = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.AuthorID);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AuthorID = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookID);
                    table.ForeignKey(
                        name: "FK_Books_Authors_AuthorID",
                        column: x => x.AuthorID,
                        principalTable: "Authors",
                        principalColumn: "AuthorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "AuthorID", "Bio", "Name" },
                values: new object[,]
                {
                    { 1, "Penulis Novel Inspiratif", "Andrea Hirata" },
                    { 2, "Penulis Novel Fiksi", "J.K. Rowling" },
                    { 3, "Penulis Novel Sejarah", "Pramoedya Ananta Toer" },
                    { 4, "Penulis Novel Fantasi", "J.R.R. Tolkien" },
                    { 5, "Penulis Novel Fantasi", "J.R.R. Tolkien" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookID", "AuthorID", "Price", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { 1, 1, 10.91m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified).AddTicks(2001), "Laskar Pelangi" },
                    { 2, 2, 30.45m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified).AddTicks(1965), "Harry Potter and The Philosopher's Stone" },
                    { 3, 3, 25.10m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified).AddTicks(1955), "Bumi Manusia" },
                    { 4, 4, 50.67m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified).AddTicks(1918), "The Lord Of The Ring The Fellowship Of The Ring" },
                    { 5, 5, 40.43m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified).AddTicks(1993), "The Lord Of The Ring The Two Towers" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorID",
                table: "Books",
                column: "AuthorID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Authors");
        }
    }
}
