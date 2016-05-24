namespace Art_Gallery.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.IndividualPiece", "ArtWorkId");
            AddForeignKey("dbo.IndividualPiece", "ArtWorkId", "dbo.ArtWork", "ArtWorkId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IndividualPiece", "ArtWorkId", "dbo.ArtWork");
            DropIndex("dbo.IndividualPiece", new[] { "ArtWorkId" });
        }
    }
}
