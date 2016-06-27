namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init1 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Courses", new[] { "Name", "DayStart" }, unique: true, name: "IX_NameDayStart");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Courses", "IX_NameDayStart");
        }
    }
}
