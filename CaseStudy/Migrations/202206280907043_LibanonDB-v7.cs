namespace CaseStudy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LibanonDBv7 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Borrowers", "Email", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Borrowers", "Email", c => c.String(nullable: false));
        }
    }
}
