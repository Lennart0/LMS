namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AssignmentSubmission_and_navprop_ids_added : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Document", name: "Activity_Id", newName: "ActivityId");
            RenameColumn(table: "dbo.Activities", name: "Module_Id", newName: "ModuleId");
            RenameColumn(table: "dbo.Document", name: "User_Id", newName: "UserId");
            RenameColumn(table: "dbo.Modules", name: "Course_Id", newName: "CourseId");
            RenameIndex(table: "dbo.Activities", name: "IX_Module_Id", newName: "IX_ModuleId");
            RenameIndex(table: "dbo.Document", name: "IX_User_Id", newName: "IX_UserId");
            RenameIndex(table: "dbo.Document", name: "IX_Activity_Id", newName: "IX_ActivityId");
            RenameIndex(table: "dbo.Modules", name: "IX_Course_Id", newName: "IX_CourseId");
            CreateTable(
                "dbo.AssignmentSubmission",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FeedBack = c.String(),
                        assignmentId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Document", t => t.Id)
                .ForeignKey("dbo.TimeSensetiveDocument", t => t.assignmentId)
                .Index(t => t.Id)
                .Index(t => t.assignmentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssignmentSubmission", "assignmentId", "dbo.TimeSensetiveDocument");
            DropForeignKey("dbo.AssignmentSubmission", "Id", "dbo.Document");
            DropIndex("dbo.AssignmentSubmission", new[] { "assignmentId" });
            DropIndex("dbo.AssignmentSubmission", new[] { "Id" });
            DropTable("dbo.AssignmentSubmission");
            RenameIndex(table: "dbo.Modules", name: "IX_CourseId", newName: "IX_Course_Id");
            RenameIndex(table: "dbo.Document", name: "IX_ActivityId", newName: "IX_Activity_Id");
            RenameIndex(table: "dbo.Document", name: "IX_UserId", newName: "IX_User_Id");
            RenameIndex(table: "dbo.Activities", name: "IX_ModuleId", newName: "IX_Module_Id");
            RenameColumn(table: "dbo.Modules", name: "CourseId", newName: "Course_Id");
            RenameColumn(table: "dbo.Document", name: "UserId", newName: "User_Id");
            RenameColumn(table: "dbo.Activities", name: "ModuleId", newName: "Module_Id");
            RenameColumn(table: "dbo.Document", name: "ActivityId", newName: "Activity_Id");
        }
    }
}
