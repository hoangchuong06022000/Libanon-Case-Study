namespace CaseStudy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LibanonDBv6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ISBNs", "NumberOfRating", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ISBNs", "NumberOfRating");
        }
    }
}
