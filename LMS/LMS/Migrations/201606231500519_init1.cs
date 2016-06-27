namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Description = c.String(),
                        Name = c.String(),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        Module_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Modules", t => t.Module_Id)
                .Index(t => t.Module_Id);
            
            CreateTable(
                "dbo.Modules",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Description = c.String(),
                        Name = c.String(),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        Course_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.Course_Id)
                .Index(t => t.Course_Id);
            
            AddColumn("dbo.Courses", "Start", c => c.DateTime(nullable: false));
            AddColumn("dbo.Courses", "End", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Modules", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.Activities", "Module_Id", "dbo.Modules");
            DropIndex("dbo.Modules", new[] { "Course_Id" });
            DropIndex("dbo.Activities", new[] { "Module_Id" });
            DropColumn("dbo.Courses", "End");
            DropColumn("dbo.Courses", "Start");
            DropTable("dbo.Modules");
            DropTable("dbo.Activities");
        }
    }
}
