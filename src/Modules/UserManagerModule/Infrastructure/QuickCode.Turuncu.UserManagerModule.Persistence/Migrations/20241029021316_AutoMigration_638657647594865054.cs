using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickCode.Turuncu.UserManagerModule.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AutoMigration_638657647594865054 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefIdColumn",
                table: "TableComboboxSettings");

            migrationBuilder.DropColumn(
                name: "RefTableColumnId",
                table: "TableComboboxSettings");

            migrationBuilder.DropColumn(
                name: "RefTableName",
                table: "TableComboboxSettings");

            migrationBuilder.DropColumn(
                name: "OnComplete",
                table: "KafkaEvents");

            migrationBuilder.DropColumn(
                name: "OnError",
                table: "KafkaEvents");

            migrationBuilder.RenameColumn(
                name: "OnTimeout",
                table: "KafkaEvents",
                newName: "IsActive");

            migrationBuilder.AlterColumn<string>(
                name: "TopicName",
                table: "KafkaEvents",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "TopicWorkflows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KafkaEventId = table.Column<int>(type: "int", nullable: false),
                    WorkflowContent = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopicWorkflows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopicWorkflows_KafkaEvents_KafkaEventId",
                        column: x => x.KafkaEventId,
                        principalTable: "KafkaEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TopicWorkflows_IsDeleted",
                table: "TopicWorkflows",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_TopicWorkflows_KafkaEventId",
                table: "TopicWorkflows",
                column: "KafkaEventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TopicWorkflows");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "KafkaEvents",
                newName: "OnTimeout");

            migrationBuilder.AddColumn<string>(
                name: "RefIdColumn",
                table: "TableComboboxSettings",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefTableColumnId",
                table: "TableComboboxSettings",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefTableName",
                table: "TableComboboxSettings",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TopicName",
                table: "KafkaEvents",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "OnComplete",
                table: "KafkaEvents",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "OnError",
                table: "KafkaEvents",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }
    }
}
