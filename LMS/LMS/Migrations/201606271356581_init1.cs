namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Document", newName: "Documents");
            DropForeignKey("dbo.TimeSensetiveDocument", "Id", "dbo.Document");
            DropIndex("dbo.TimeSensetiveDocument", new[] { "Id" });
            AddColumn("dbo.Documents", "DeadLine", c => c.DateTime());
            AlterColumn("dbo.Documents", "type", c => c.Int());
            DropTable("dbo.TimeSensetiveDocument");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TimeSensetiveDocument",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DeadLine = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.Documents", "type", c => c.Int(nullable: false));
            DropColumn("dbo.Documents", "DeadLine");
            CreateIndex("dbo.TimeSensetiveDocument", "Id");
            AddForeignKey("dbo.TimeSensetiveDocument", "Id", "dbo.Document", "Id");
            RenameTable(name: "dbo.Documents", newName: "Document");
        }
    }
}
