namespace CaseStudy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LibanonDBv0 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Author = c.String(nullable: false),
                        Image = c.String(nullable: false),
                        PublishYear = c.String(nullable: false),
                        Category = c.String(),
                        Summary = c.String(nullable: false),
                        OwnerId = c.Int(nullable: false),
                        IsBorrowed = c.Boolean(nullable: false),
                        BorrowerId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Borrowers", t => t.BorrowerId)
                .ForeignKey("dbo.Owners", t => t.OwnerId)
                .Index(t => t.OwnerId)
                .Index(t => t.BorrowerId);
            
            CreateTable(
                "dbo.Borrowers",
                c => new
                    {
                        BorrowerId = c.Int(nullable: false, identity: true),
                        BorrowerName = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        Email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.BorrowerId);
            
            CreateTable(
                "dbo.ISBNs",
                c => new
                    {
                        ISBNId = c.Int(nullable: false),
                        ISBNString = c.String(nullable: false),
                        RateScore = c.Double(),
                    })
                .PrimaryKey(t => t.ISBNId)
                .ForeignKey("dbo.Books", t => t.ISBNId, cascadeDelete: true)
                .Index(t => t.ISBNId);
            
            CreateTable(
                "dbo.Owners",
                c => new
                    {
                        OwnerId = c.Int(nullable: false, identity: true),
                        OwnerName = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        Email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.OwnerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "OwnerId", "dbo.Owners");
            DropForeignKey("dbo.ISBNs", "ISBNId", "dbo.Books");
            DropForeignKey("dbo.Books", "BorrowerId", "dbo.Borrowers");
            DropIndex("dbo.ISBNs", new[] { "ISBNId" });
            DropIndex("dbo.Books", new[] { "BorrowerId" });
            DropIndex("dbo.Books", new[] { "OwnerId" });
            DropTable("dbo.Owners");
            DropTable("dbo.ISBNs");
            DropTable("dbo.Borrowers");
            DropTable("dbo.Books");
        }
    }
}
