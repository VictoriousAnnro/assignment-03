using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assignment3.Entities.Migrations
{
    public partial class AddTaskTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskTag_Tags_TagId",
                table: "TaskTag");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskTag_Tasks_TaskId",
                table: "TaskTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskTag",
                table: "TaskTag");

            migrationBuilder.RenameTable(
                name: "TaskTag",
                newName: "TaskTags");

            migrationBuilder.RenameIndex(
                name: "IX_TaskTag_TaskId",
                table: "TaskTags",
                newName: "IX_TaskTags_TaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskTags",
                table: "TaskTags",
                columns: new[] { "TagId", "TaskId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTags_Tags_TagId",
                table: "TaskTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTags_Tasks_TaskId",
                table: "TaskTags",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskTags_Tags_TagId",
                table: "TaskTags");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskTags_Tasks_TaskId",
                table: "TaskTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskTags",
                table: "TaskTags");

            migrationBuilder.RenameTable(
                name: "TaskTags",
                newName: "TaskTag");

            migrationBuilder.RenameIndex(
                name: "IX_TaskTags_TaskId",
                table: "TaskTag",
                newName: "IX_TaskTag_TaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskTag",
                table: "TaskTag",
                columns: new[] { "TagId", "TaskId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTag_Tags_TagId",
                table: "TaskTag",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTag_Tasks_TaskId",
                table: "TaskTag",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
