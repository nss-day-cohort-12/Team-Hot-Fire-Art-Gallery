namespace Art_Gallery.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArtWork",
                c => new
                    {
                        ArtWorkId = c.Int(nullable: false, identity: true),
                        Artist = c.String(),
                        Title = c.String(),
                        YearOriginalCreated = c.String(),
                        Medium = c.String(),
                        Dimensions = c.String(),
                        NumberMade = c.Int(nullable: false),
                        NumberInInventory = c.Int(nullable: false),
                        NumberSold = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ArtWorkId);
            
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        AgentId = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Address = c.String(),
                        PhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.CustomerId);
            
            CreateTable(
                "dbo.IndividualPiece",
                c => new
                    {
                        IndividualPieceId = c.Int(nullable: false, identity: true),
                        ArtWorkId = c.Int(nullable: false),
                        Image = c.String(),
                        DateCreated = c.String(),
                        Cost = c.Single(nullable: false),
                        Price = c.Single(nullable: false),
                        Sold = c.Boolean(nullable: false),
                        Location = c.String(),
                        EditionNumber = c.String(),
                        InvoiceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IndividualPieceId);
            
            CreateTable(
                "dbo.ArtShow",
                c => new
                    {
                        ArtShowId = c.Int(nullable: false, identity: true),
                        ArtistsRepresented = c.String(),
                        ShowLocation = c.String(),
                        Agents = c.Int(nullable: false),
                        Overhead = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.ArtShowId);
            
            CreateTable(
                "dbo.Invoice",
                c => new
                    {
                        InvoiceId = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        AgentId = c.Int(nullable: false),
                        PaymentMethod = c.String(),
                        ShippingAddress = c.String(),
                        PieceSold = c.String(),
                    })
                .PrimaryKey(t => t.InvoiceId);
            
            CreateTable(
                "dbo.Agent",
                c => new
                    {
                        AgentId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Location = c.String(),
                        Address = c.String(),
                        PhoneNumber = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AgentId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Agent");
            DropTable("dbo.Invoice");
            DropTable("dbo.ArtShow");
            DropTable("dbo.IndividualPiece");
            DropTable("dbo.Customer");
            DropTable("dbo.ArtWork");
        }
    }
}
