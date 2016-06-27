namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init2 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Documents", newName: "Document");
            CreateTable(
                "dbo.TimeSensetiveDocument",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DeadLine = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Document", t => t.Id)
                .Index(t => t.Id);
            
            AlterColumn("dbo.Document", "Type", c => c.Int(nullable: false));
            DropColumn("dbo.Document", "DeadLine");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Document", "DeadLine", c => c.DateTime());
            DropForeignKey("dbo.TimeSensetiveDocument", "Id", "dbo.Document");
            DropIndex("dbo.TimeSensetiveDocument", new[] { "Id" });
            AlterColumn("dbo.Document", "Type", c => c.Int());
            DropTable("dbo.TimeSensetiveDocument");
            RenameTable(name: "dbo.Document", newName: "Documents");
        }
    }
}
