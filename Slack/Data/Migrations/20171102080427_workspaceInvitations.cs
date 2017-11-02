using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Slack.Data.Migrations
{
    public partial class workspaceInvitations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkspaceInvitation",
                columns: table => new
                {
                    WorkspaceInvitationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CloseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpirationTime = table.Column<long>(type: "bigint", nullable: true),
                    InvitationGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserEmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkspaceName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkspaceInvitation", x => x.WorkspaceInvitationID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkspaceInvitation");
        }
    }
}
