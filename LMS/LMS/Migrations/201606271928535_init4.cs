namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Courses", "LunchStart", c => c.DateTime());
            AlterColumn("dbo.Courses", "LunchEnd", c => c.DateTime());
            AlterColumn("dbo.Courses", "DayStart", c => c.DateTime());
            AlterColumn("dbo.Courses", "DayEnd", c => c.DateTime());
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
