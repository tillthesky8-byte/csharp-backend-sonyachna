using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SONYACHNA.database.Migrations
{
    /// <inheritdoc />
    public partial class ModelRefresh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DreamEntries_SurveySessions_SurveySessionId",
                table: "DreamEntries");

            migrationBuilder.DropIndex(
                name: "IX_DreamEntries_SurveySessionId",
                table: "DreamEntries");

            migrationBuilder.DropColumn(
                name: "SurveySessionId",
                table: "DreamEntries");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "DreamEntries",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "DreamEntries");

            migrationBuilder.AddColumn<int>(
                name: "SurveySessionId",
                table: "DreamEntries",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DreamEntries_SurveySessionId",
                table: "DreamEntries",
                column: "SurveySessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_DreamEntries_SurveySessions_SurveySessionId",
                table: "DreamEntries",
                column: "SurveySessionId",
                principalTable: "SurveySessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
