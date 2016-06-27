namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datatimechange1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Courses", "LunchStart", c => c.Time(nullable: false, precision: 7));
            AlterColumn("dbo.Courses", "LunchEnd", c => c.Time(nullable: false, precision: 7));
            AlterColumn("dbo.Courses", "DayStart", c => c.Time(nullable: false, precision: 7));
            AlterColumn("dbo.Courses", "DayEnd", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Courses", "DayEnd", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Courses", "DayStart", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Courses", "LunchEnd", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Courses", "LunchStart", c => c.DateTime(nullable: false));
        }
    }
}
