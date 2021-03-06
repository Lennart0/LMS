namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FromScratch_Again : DbMigration
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
                        ModuleId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Modules", t => t.ModuleId)
                .Index(t => t.ModuleId);
            
            CreateTable(
                "dbo.Document",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Type = c.Int(nullable: false),
                        CourseId = c.Guid(),
                        ModuleId = c.Guid(),
                        ActivityId = c.Guid(),
                        Url = c.String(),
                        IsLocal = c.Boolean(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        UploadDate = c.DateTime(nullable: false),
                        PublishDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId)
                .ForeignKey("dbo.Modules", t => t.ModuleId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .Index(t => t.CourseId)
                .Index(t => t.ModuleId)
                .Index(t => t.ActivityId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        LunchStart = c.DateTime(),
                        LunchEnd = c.DateTime(),
                        DayStart = c.DateTime(),
                        DayEnd = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Modules",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        CourseId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(),
                        CourseId = c.Guid(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId)
                .Index(t => t.CourseId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AssignmentSubmission",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FeedBack = c.String(),
                        assignmentId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Document", t => t.Id)
                .ForeignKey("dbo.TimeSensetiveDocument", t => t.assignmentId)
                .Index(t => t.Id)
                .Index(t => t.assignmentId);
            
            CreateTable(
                "dbo.PlainDocuments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Document", t => t.Id)
                .Index(t => t.Id);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TimeSensetiveDocument", "Id", "dbo.Document");
            DropForeignKey("dbo.PlainDocuments", "Id", "dbo.Document");
            DropForeignKey("dbo.AssignmentSubmission", "assignmentId", "dbo.TimeSensetiveDocument");
            DropForeignKey("dbo.AssignmentSubmission", "Id", "dbo.Document");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Document", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Document", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Modules", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Document", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.Activities", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.Document", "CourseId", "dbo.Courses");
            DropIndex("dbo.TimeSensetiveDocument", new[] { "Id" });
            DropIndex("dbo.PlainDocuments", new[] { "Id" });
            DropIndex("dbo.AssignmentSubmission", new[] { "assignmentId" });
            DropIndex("dbo.AssignmentSubmission", new[] { "Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "CourseId" });
            DropIndex("dbo.Modules", new[] { "CourseId" });
            DropIndex("dbo.Document", new[] { "UserId" });
            DropIndex("dbo.Document", new[] { "ActivityId" });
            DropIndex("dbo.Document", new[] { "ModuleId" });
            DropIndex("dbo.Document", new[] { "CourseId" });
            DropIndex("dbo.Activities", new[] { "ModuleId" });
            DropTable("dbo.TimeSensetiveDocument");
            DropTable("dbo.PlainDocuments");
            DropTable("dbo.AssignmentSubmission");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Modules");
            DropTable("dbo.Courses");
            DropTable("dbo.Document");
            DropTable("dbo.Activities");
        }
    }
}
