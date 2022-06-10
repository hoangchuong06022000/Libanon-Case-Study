namespace CaseStudy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LibanonDBv2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ISBNs", "RateScore", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ISBNs", "RateScore", c => c.Double(nullable: false));
        }
    }
}
