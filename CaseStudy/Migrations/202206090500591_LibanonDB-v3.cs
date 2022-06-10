namespace CaseStudy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LibanonDBv3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Books", "IsBorrowed", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Books", "IsBorrowed", c => c.Boolean(nullable: false));
        }
    }
}
