using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SONYACHNA.database.Migrations
{
    /// <inheritdoc />
    public partial class DateTimeToUnixLong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreatedAtUnix",
                table: "SurveySessions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long?>(
                name: "CompletedAtUnix",
                table: "Todos",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedAtUnix",
                table: "Todos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long?>(
                name: "DueAtUnix",
                table: "Todos",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long?>(
                name: "UpdatedAtUnix",
                table: "Todos",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.Sql(@"
                UPDATE SurveySessions
                SET CreatedAtUnix = CASE
                    WHEN CreatedAt IS NULL THEN 0
                    ELSE CAST(strftime('%s', CreatedAt) AS INTEGER)
                END;
            ");

            migrationBuilder.Sql(@"
                UPDATE Todos
                SET
                    CreatedAtUnix = CASE
                        WHEN CreatedAt IS NULL THEN 0
                        ELSE CAST(strftime('%s', CreatedAt) AS INTEGER)
                    END,
                    UpdatedAtUnix = CASE
                        WHEN UpdatedAt IS NULL THEN NULL
                        ELSE CAST(strftime('%s', UpdatedAt) AS INTEGER)
                    END,
                    CompletedAtUnix = CASE
                        WHEN CompletedAt IS NULL THEN NULL
                        ELSE CAST(strftime('%s', CompletedAt) AS INTEGER)
                    END,
                    DueAtUnix = CASE
                        WHEN DueAt IS NULL THEN NULL
                        ELSE CAST(strftime('%s', DueAt) AS INTEGER)
                    END;
            ");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SurveySessions");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "DueAt",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Todos");

            migrationBuilder.RenameColumn(
                name: "CreatedAtUnix",
                table: "SurveySessions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtUnix",
                table: "Todos",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "DueAtUnix",
                table: "Todos",
                newName: "DueAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAtUnix",
                table: "Todos",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "CompletedAtUnix",
                table: "Todos",
                newName: "CompletedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedAtText",
                table: "SurveySessions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompletedAtText",
                table: "Todos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedAtText",
                table: "Todos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DueAtText",
                table: "Todos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedAtText",
                table: "Todos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.Sql(@"
                UPDATE SurveySessions
                SET CreatedAtText = datetime(CreatedAt, 'unixepoch');
            ");

            migrationBuilder.Sql(@"
                UPDATE Todos
                SET
                    CreatedAtText = datetime(CreatedAt, 'unixepoch'),
                    UpdatedAtText = CASE
                        WHEN UpdatedAt IS NULL THEN NULL
                        ELSE datetime(UpdatedAt, 'unixepoch')
                    END,
                    CompletedAtText = CASE
                        WHEN CompletedAt IS NULL THEN NULL
                        ELSE datetime(CompletedAt, 'unixepoch')
                    END,
                    DueAtText = CASE
                        WHEN DueAt IS NULL THEN NULL
                        ELSE datetime(DueAt, 'unixepoch')
                    END;
            ");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SurveySessions");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "DueAt",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Todos");

            migrationBuilder.RenameColumn(
                name: "CreatedAtText",
                table: "SurveySessions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtText",
                table: "Todos",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "DueAtText",
                table: "Todos",
                newName: "DueAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAtText",
                table: "Todos",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "CompletedAtText",
                table: "Todos",
                newName: "CompletedAt");
        }
    }
}
