using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Iya.Models
{
    public partial class IyaDbContext : DbContext
    {
        public IyaDbContext()
        {
        }

        public IyaDbContext(DbContextOptions<IyaDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Component> Components { get; set; }
        public virtual DbSet<Kanji> Kanjis { get; set; }
        public virtual DbSet<Word> Words { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlite("Data Source=Iya.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Component>(entity =>
            {
                entity.ToTable("components");

                entity.Property(e => e.ComponentId)
                    .HasColumnName("component_id");

                entity.HasIndex(e => e.Component1, "IX_components_component")
                    .IsUnique();

                entity.Property(e => e.Component1)
                    .HasColumnName("component")
                    .IsRequired()
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Meaning)
                    .HasColumnName("meaning")
                    .IsRequired()
                    .HasDefaultValueSql("''");

                entity.Property(e => e.IsCustomMng)
                    .HasColumnName("is_custom_mng");

                entity.Property(e => e.IsMngLost)
                    .HasColumnName("is_mng_lost");

                entity.Property(e => e.IsKanji)
                    .HasColumnName("is_kanji");
            });

            modelBuilder.Entity<Kanji>(entity =>
            {
                entity.ToTable("kanjis");

                entity.Property(e => e.KanjiId)
                    .HasColumnName("kanji_id");

                entity.HasIndex(e => e.Kanji1, "IX_kanjis_kanji")
                    .IsUnique();

                entity.Property(e => e.Kanji1)
                    .HasColumnName("kanji")
                    .IsRequired()
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Components)
                    .HasColumnName("components")
                    .IsRequired()
                    .HasDefaultValueSql("''");

                entity.Property(e => e.OnReadings)
                    .HasColumnName("on_readings")
                    .IsRequired()
                    .HasDefaultValueSql("''");

                entity.Property(e => e.KunReadings)
                    .HasColumnName("kun_readings")
                    .IsRequired()
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Meaning)
                    .HasColumnName("meaning")
                    .IsRequired()
                    .HasDefaultValueSql("''");

                entity.Property(e => e.JlptLvl)
                    .HasColumnName("jlpt_lvl");
            });

            modelBuilder.Entity<Word>(entity =>
            {
                entity.ToTable("words");

                entity.HasIndex(e => e.Word1, "IX_words_word")
                    .IsUnique();

                entity.Property(e => e.WordId)
                    .HasColumnName("word_id");

                entity.Property(e => e.Word1)
                    .HasColumnName("word")
                    .IsRequired()
                    .HasDefaultValueSql("''");

                entity.Property(e => e.KanjiReading)
                    .HasColumnName("kanji_reading")
                    .IsRequired()
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Reading)
                    .HasColumnName("reading")
                    .IsRequired()
                    .HasDefaultValueSql("''");

                entity.Property(e => e.OtherForms)
                    .HasColumnName("other_forms")
                    .IsRequired()
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Meaning)
                    .HasColumnName("meaning")
                    .IsRequired()
                    .HasDefaultValueSql("''");

                entity.Property(e => e.JlptLvl)
                    .HasColumnName("jlpt_lvl")
                    .IsRequired();

                entity.Property(e => e.Comment)
                    .HasColumnName("comment")
                    .IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
