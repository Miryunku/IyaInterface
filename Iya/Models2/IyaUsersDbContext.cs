using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Iya.Models2
{
    public partial class IyaUsersDbContext : DbContext
    {
        public IyaUsersDbContext()
        {
        }

        public IyaUsersDbContext(DbContextOptions<IyaUsersDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Collection> Collections { get; set; }
        public virtual DbSet<ComponentCollectionContent> ComponentCollectionContents { get; set; }
        public virtual DbSet<KanjiCollectionContent> KanjiCollectionContents { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<WordCollectionContent> WordCollectionContents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlite("Data Source=IyaUsers.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Collection>(entity =>
            {
                entity.ToTable("collections");

                entity.Property(e => e.CollectionId)
                    .HasColumnName("collection_id");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .IsRequired()
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .IsRequired()
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Size)
                    .HasColumnName("size")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Creation)
                    .HasColumnName("creation")
                    .IsRequired()
                    .HasDefaultValueSql("''");

                entity.Property(e => e.LastVisit)
                    .HasColumnName("last_visit")
                    .IsRequired()
                    .HasDefaultValueSql("''");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Collections)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<ComponentCollectionContent>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("component_collection_contents");

                entity.Property(e => e.CollectionId).HasColumnName("collection_id");

                entity.Property(e => e.ComponentId).HasColumnName("component_id");
            });

            modelBuilder.Entity<KanjiCollectionContent>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("kanji_collection_contents");

                entity.Property(e => e.CollectionId).HasColumnName("collection_id");

                entity.Property(e => e.KanjiId).HasColumnName("kanji_id");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id");

                entity.HasIndex(e => e.Name, "IX_users_name")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .IsRequired()
                    .HasDefaultValueSql("''");

                entity.Property(e => e.ComponentCollectionsQuantity).HasColumnName("component_collections_quantity");

                entity.Property(e => e.KanjiCollectionsQuantity).HasColumnName("kanji_collections_quantity");

                entity.Property(e => e.WordCollectionsQuantity).HasColumnName("word_collections_quantity");
            });

            modelBuilder.Entity<WordCollectionContent>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("word_collection_contents");

                entity.Property(e => e.CollectionId).HasColumnName("collection_id");

                entity.Property(e => e.WordId).HasColumnName("word_id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
