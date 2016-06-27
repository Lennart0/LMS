namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class document1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "Course_Id", c => c.Guid());
            AddColumn("dbo.Documents", "Module_Id", c => c.Guid());
            CreateIndex("dbo.Documents", "Course_Id");
            CreateIndex("dbo.Documents", "Module_Id");
            AddForeignKey("dbo.Documents", "Course_Id", "dbo.Courses", "Id");
            AddForeignKey("dbo.Documents", "Module_Id", "dbo.Modules", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "Module_Id", "dbo.Modules");
            DropForeignKey("dbo.Documents", "Course_Id", "dbo.Courses");
            DropIndex("dbo.Documents", new[] { "Module_Id" });
            DropIndex("dbo.Documents", new[] { "Course_Id" });
            DropColumn("dbo.Documents", "Module_Id");
            DropColumn("dbo.Documents", "Course_Id");
        }
    }
}
