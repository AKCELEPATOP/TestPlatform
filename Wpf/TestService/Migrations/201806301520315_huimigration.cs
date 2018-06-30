namespace TestService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class huimigration : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attachments", "QuestionId", "dbo.Questions");
            DropIndex("dbo.Attachments", new[] { "QuestionId" });
            DropTable("dbo.Attachments");
        }
    }
}
