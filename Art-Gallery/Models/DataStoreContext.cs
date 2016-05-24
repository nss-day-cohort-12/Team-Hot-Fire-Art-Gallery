using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Art_Gallery.Models
{
    public class DataStoreContext : DbContext
    {
        public DbSet<ArtWork> ArtWork { get; set; }
        public DbSet<ArtShow> ArtShow { get; set; }
        public DbSet<IndividualPiece> IndividualPiece { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Agent> Agent { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArtWork>()
                .ToTable("ArtWork")
                .HasKey(a => a.ArtWorkId);

            modelBuilder.Entity<ArtShow>()
                .ToTable("ArtShow")
                .HasKey(s => s.ArtShowId);

            modelBuilder.Entity<IndividualPiece>()
                .ToTable("IndividualPiece")
                .HasKey(i => i.IndividualPieceId);

            modelBuilder.Entity<Invoice>()
                .ToTable("Invoice")
                .HasKey(n => n.InvoiceId);

            modelBuilder.Entity<Customer>()
                .ToTable("Customer")
                .HasKey(c => c.CustomerId);

            modelBuilder.Entity<Agent>()
                .ToTable("Agent")
                .HasKey(g => g.AgentId);
        }
    }
}