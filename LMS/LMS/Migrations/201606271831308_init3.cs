namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "LunchStart", c => c.DateTime(nullable: false));
            AddColumn("dbo.Courses", "LunchEnd", c => c.DateTime(nullable: false));
            AddColumn("dbo.Courses", "DayStart", c => c.DateTime(nullable: false));
            AddColumn("dbo.Courses", "DayEnd", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "DayEnd");
            DropColumn("dbo.Courses", "DayStart");
            DropColumn("dbo.Courses", "LunchEnd");
            DropColumn("dbo.Courses", "LunchStart");
        }
    }
}
