namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datatimechange3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Courses", "LunchStart");
            DropColumn("dbo.Courses", "LunchEnd");
            DropColumn("dbo.Courses", "DayStart");
            DropColumn("dbo.Courses", "DayEnd");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Courses", "DayEnd", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.Courses", "DayStart", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.Courses", "LunchEnd", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.Courses", "LunchStart", c => c.Time(nullable: false, precision: 7));
        }
    }
}
