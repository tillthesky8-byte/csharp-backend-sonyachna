using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SONYACHNA.database.Migrations
{
    /// <inheritdoc />
    public partial class Inital : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_SurveySessions_SurveySessionId",
                table: "Answers");

            migrationBuilder.RenameColumn(
                name: "dueAt",
                table: "Todos",
                newName: "DueAt");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DueAt",
                table: "Todos",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Todos",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Todos",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CompletedAt",
                table: "Todos",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "SurveySessions",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "SurveySessionId",
                table: "Answers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_SurveySessions_SurveySessionId",
                table: "Answers",
                column: "SurveySessionId",
                principalTable: "SurveySessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_SurveySessions_SurveySessionId",
                table: "Answers");

            migrationBuilder.RenameColumn(
                name: "DueAt",
                table: "Todos",
                newName: "dueAt");

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedAt",
                table: "Todos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "dueAt",
                table: "Todos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedAt",
                table: "Todos",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "CompletedAt",
                table: "Todos",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedAt",
                table: "SurveySessions",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "SurveySessionId",
                table: "Answers",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_SurveySessions_SurveySessionId",
                table: "Answers",
                column: "SurveySessionId",
                principalTable: "SurveySessions",
                principalColumn: "Id");
        }
    }
}
