namespace CaseStudy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LibanonDBv1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ISBNs", "RateScore", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ISBNs", "RateScore", c => c.Double());
        }
    }
}
