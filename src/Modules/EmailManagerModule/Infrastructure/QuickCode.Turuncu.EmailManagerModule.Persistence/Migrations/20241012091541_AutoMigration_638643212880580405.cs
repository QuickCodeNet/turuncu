using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickCode.Turuncu.EmailManagerModule.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AutoMigration_638643212880580405 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CAMPAIGN_TYPES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NAME = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    TEMPLATE = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAMPAIGN_TYPES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EMAIL_SENDERS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GSM_NUMBER = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PROVIDER_NAME = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMAIL_SENDERS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "INFO_TYPES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NAME = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    TEMPLATE = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INFO_TYPES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "OTP_TYPES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NAME = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    TEMPLATE = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OTP_TYPES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CAMPAIGN_MESSAGES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EMAIL_SENDER_ID = table.Column<int>(type: "int", nullable: false),
                    CAMPAIGN_TYPE_ID = table.Column<int>(type: "int", nullable: false),
                    GSM_NUMBER = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    MESSAGE = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    MESSAGE_DATE = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    MESSAGE_SID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    DAILY_COUNTER = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAMPAIGN_MESSAGES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CAMPAIGN_MESSAGES_CAMPAIGN_TYPES_CAMPAIGN_TYPE_ID",
                        column: x => x.CAMPAIGN_TYPE_ID,
                        principalTable: "CAMPAIGN_TYPES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CAMPAIGN_MESSAGES_EMAIL_SENDERS_EMAIL_SENDER_ID",
                        column: x => x.EMAIL_SENDER_ID,
                        principalTable: "EMAIL_SENDERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "INFO_MESSAGES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EMAIL_SENDER_ID = table.Column<int>(type: "int", nullable: false),
                    INFO_TYPE_ID = table.Column<int>(type: "int", nullable: false),
                    GSM_NUMBER = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    MESSAGE = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    MESSAGE_DATE = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    MESSAGE_SID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    DAILY_COUNTER = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INFO_MESSAGES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_INFO_MESSAGES_EMAIL_SENDERS_EMAIL_SENDER_ID",
                        column: x => x.EMAIL_SENDER_ID,
                        principalTable: "EMAIL_SENDERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_INFO_MESSAGES_INFO_TYPES_INFO_TYPE_ID",
                        column: x => x.INFO_TYPE_ID,
                        principalTable: "INFO_TYPES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OTP_MESSAGES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EMAIL_SENDER_ID = table.Column<int>(type: "int", nullable: false),
                    OTP_TYPE_ID = table.Column<int>(type: "int", nullable: false),
                    GSM_NUMBER = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    OTP_CODE = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    MESSAGE = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    EXPIRE_SECONDS = table.Column<int>(type: "int", nullable: false),
                    MESSAGE_DATE = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    MESSAGE_SID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    DAILY_COUNTER = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OTP_MESSAGES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OTP_MESSAGES_EMAIL_SENDERS_EMAIL_SENDER_ID",
                        column: x => x.EMAIL_SENDER_ID,
                        principalTable: "EMAIL_SENDERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OTP_MESSAGES_OTP_TYPES_OTP_TYPE_ID",
                        column: x => x.OTP_TYPE_ID,
                        principalTable: "OTP_TYPES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CAMPAIGN_MESSAGES_CAMPAIGN_TYPE_ID",
                table: "CAMPAIGN_MESSAGES",
                column: "CAMPAIGN_TYPE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CAMPAIGN_MESSAGES_EMAIL_SENDER_ID",
                table: "CAMPAIGN_MESSAGES",
                column: "EMAIL_SENDER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CAMPAIGN_MESSAGES_IsDeleted",
                table: "CAMPAIGN_MESSAGES",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_CAMPAIGN_TYPES_IsDeleted",
                table: "CAMPAIGN_TYPES",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_EMAIL_SENDERS_IsDeleted",
                table: "EMAIL_SENDERS",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_INFO_MESSAGES_EMAIL_SENDER_ID",
                table: "INFO_MESSAGES",
                column: "EMAIL_SENDER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_INFO_MESSAGES_INFO_TYPE_ID",
                table: "INFO_MESSAGES",
                column: "INFO_TYPE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_INFO_MESSAGES_IsDeleted",
                table: "INFO_MESSAGES",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_INFO_TYPES_IsDeleted",
                table: "INFO_TYPES",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_OTP_MESSAGES_EMAIL_SENDER_ID",
                table: "OTP_MESSAGES",
                column: "EMAIL_SENDER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_OTP_MESSAGES_IsDeleted",
                table: "OTP_MESSAGES",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_OTP_MESSAGES_OTP_TYPE_ID",
                table: "OTP_MESSAGES",
                column: "OTP_TYPE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_OTP_TYPES_IsDeleted",
                table: "OTP_TYPES",
                column: "IsDeleted",
                filter: "IsDeleted = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CAMPAIGN_MESSAGES");

            migrationBuilder.DropTable(
                name: "INFO_MESSAGES");

            migrationBuilder.DropTable(
                name: "OTP_MESSAGES");

            migrationBuilder.DropTable(
                name: "CAMPAIGN_TYPES");

            migrationBuilder.DropTable(
                name: "INFO_TYPES");

            migrationBuilder.DropTable(
                name: "EMAIL_SENDERS");

            migrationBuilder.DropTable(
                name: "OTP_TYPES");
        }
    }
}
