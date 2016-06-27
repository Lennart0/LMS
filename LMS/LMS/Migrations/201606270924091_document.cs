namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class document : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Type = c.Int(nullable: false),
                        Url = c.String(),
                        IsLocal = c.Boolean(nullable: false),
                        UploadDate = c.DateTime(nullable: false),
                        PublishDate = c.DateTime(nullable: false),
                        DeadLine = c.DateTime(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Activity_Id = c.Guid(),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.Activity_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Activity_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Documents", "Activity_Id", "dbo.Activities");
            DropIndex("dbo.Documents", new[] { "User_Id" });
            DropIndex("dbo.Documents", new[] { "Activity_Id" });
            DropTable("dbo.Documents");
        }
    }
}
