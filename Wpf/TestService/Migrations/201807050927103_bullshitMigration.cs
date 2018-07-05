namespace TestService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bullshitMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        True = c.Boolean(nullable: false),
                        QuestionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(maxLength: 1000),
                        Time = c.Long(nullable: false),
                        Complexity = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Path = c.String(),
                        QuestionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 30),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PatternCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Complex = c.Int(nullable: false),
                        Middle = c.Int(nullable: false),
                        Easy = c.Int(nullable: false),
                        PatternId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Patterns", t => t.PatternId, cascadeDelete: true)
                .Index(t => t.PatternId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Patterns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        UserGroupId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserGroups", t => t.UserGroupId)
                .Index(t => t.UserGroupId);
            
            CreateTable(
                "dbo.PatternQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PatternId = c.Int(nullable: false),
                        QuestionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Patterns", t => t.PatternId, cascadeDelete: true)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.PatternId)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.Stats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreate = c.DateTime(nullable: false),
                        Right = c.Int(nullable: false),
                        Total = c.Int(nullable: false),
                        PatternId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Patterns", t => t.PatternId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.PatternId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FIO = c.String(),
                        UserName = c.String(nullable: false, maxLength: 256),
                        PasswordHash = c.String(nullable: false),
                        SecurityStamp = c.String(),
                        Email = c.String(maxLength: 256),
                        UserGroupId = c.Int(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserGroups", t => t.UserGroupId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.UserGroupId);
            
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
                "dbo.UserGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Questions", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.AspNetUsers", "UserGroupId", "dbo.UserGroups");
            DropForeignKey("dbo.Patterns", "UserGroupId", "dbo.UserGroups");
            DropForeignKey("dbo.Stats", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Stats", "PatternId", "dbo.Patterns");
            DropForeignKey("dbo.PatternQuestions", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.PatternQuestions", "PatternId", "dbo.Patterns");
            DropForeignKey("dbo.PatternCategories", "PatternId", "dbo.Patterns");
            DropForeignKey("dbo.PatternCategories", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Attachments", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Answers", "QuestionId", "dbo.Questions");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "UserGroupId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Stats", new[] { "UserId" });
            DropIndex("dbo.Stats", new[] { "PatternId" });
            DropIndex("dbo.PatternQuestions", new[] { "QuestionId" });
            DropIndex("dbo.PatternQuestions", new[] { "PatternId" });
            DropIndex("dbo.Patterns", new[] { "UserGroupId" });
            DropIndex("dbo.PatternCategories", new[] { "CategoryId" });
            DropIndex("dbo.PatternCategories", new[] { "PatternId" });
            DropIndex("dbo.Attachments", new[] { "QuestionId" });
            DropIndex("dbo.Questions", new[] { "CategoryId" });
            DropIndex("dbo.Answers", new[] { "QuestionId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.UserGroups");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Stats");
            DropTable("dbo.PatternQuestions");
            DropTable("dbo.Patterns");
            DropTable("dbo.PatternCategories");
            DropTable("dbo.Categories");
            DropTable("dbo.Attachments");
            DropTable("dbo.Questions");
            DropTable("dbo.Answers");
        }
    }
}
