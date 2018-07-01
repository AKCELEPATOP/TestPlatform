namespace TestService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class huiMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PatternCategories", "Easy", c => c.Int(nullable: false));
            AlterColumn("dbo.PatternCategories", "Complex", c => c.Int(nullable: false));
            AlterColumn("dbo.PatternCategories", "Middle", c => c.Int(nullable: false));
            DropColumn("dbo.PatternCategories", "Count");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PatternCategories", "Count", c => c.Int(nullable: false));
            AlterColumn("dbo.PatternCategories", "Middle", c => c.Double(nullable: false));
            AlterColumn("dbo.PatternCategories", "Complex", c => c.Double(nullable: false));
            DropColumn("dbo.PatternCategories", "Easy");
        }
    }
}
