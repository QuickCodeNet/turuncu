using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuickCode.Turuncu.EmailManagerModule.Domain.Entities;

namespace QuickCode.Turuncu.EmailManagerModule.Persistence.Contexts;

public partial class ReadDbContext : DbContext
{
	public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options) { }


	public virtual DbSet<InfoMessages> InfoMessages { get; set; }

	public virtual DbSet<CampaignMessages> CampaignMessages { get; set; }

	public virtual DbSet<OtpMessages> OtpMessages { get; set; }

	public virtual DbSet<OtpTypes> OtpTypes { get; set; }

	public virtual DbSet<InfoTypes> InfoTypes { get; set; }

	public virtual DbSet<CampaignTypes> CampaignTypes { get; set; }

	public virtual DbSet<EmailSenders> EmailSenders { get; set; }


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<InfoMessages>()
		.Property(b => b.MessageDate)
		.IsRequired()
		.HasColumnType("datetime")
		.HasDefaultValueSql("getdate()");

		modelBuilder.Entity<CampaignMessages>()
		.Property(b => b.MessageDate)
		.IsRequired()
		.HasColumnType("datetime")
		.HasDefaultValueSql("getdate()");

		modelBuilder.Entity<OtpMessages>()
		.Property(b => b.MessageDate)
		.IsRequired()
		.HasColumnType("datetime")
		.HasDefaultValueSql("getdate()");

		modelBuilder.Entity<InfoMessages>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<InfoMessages>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<CampaignMessages>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<CampaignMessages>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<OtpMessages>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<OtpMessages>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<OtpTypes>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<OtpTypes>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<InfoTypes>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<InfoTypes>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<CampaignTypes>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<CampaignTypes>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<EmailSenders>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<EmailSenders>().HasQueryFilter(r => !r.IsDeleted);


		modelBuilder.Entity<InfoMessages>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<CampaignMessages>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<OtpMessages>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<OtpTypes>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<InfoTypes>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<CampaignTypes>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<EmailSenders>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");

		OnModelCreatingPartial(modelBuilder);

		foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
		{
		    relationship.DeleteBehavior = DeleteBehavior.Restrict;
		}
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


    public override int SaveChanges()
    {
        throw new InvalidOperationException("ReadDbContext is read-only.");
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException("ReadDbContext is read-only.");
    }

}
