using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuickCode.Turuncu.UserManagerModule.Domain.Entities;

namespace QuickCode.Turuncu.UserManagerModule.Persistence.Contexts;

public partial class WriteDbContext : DbContext
{
	public WriteDbContext(DbContextOptions<WriteDbContext> options) : base(options) { }


	public virtual DbSet<PortalMenus> PortalMenus { get; set; }

	public virtual DbSet<ColumnTypes> ColumnTypes { get; set; }

	public virtual DbSet<PortalPermissionTypes> PortalPermissionTypes { get; set; }

	public virtual DbSet<PortalPermissions> PortalPermissions { get; set; }

	public virtual DbSet<ApiMethodDefinitions> ApiMethodDefinitions { get; set; }

	public virtual DbSet<KafkaEvents> KafkaEvents { get; set; }

	public virtual DbSet<ApiPermissionGroups> ApiPermissionGroups { get; set; }

	public virtual DbSet<PortalPermissionGroups> PortalPermissionGroups { get; set; }

	public virtual DbSet<PermissionGroups> PermissionGroups { get; set; }

	public virtual DbSet<TableComboboxSettings> TableComboboxSettings { get; set; }

	public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }

	public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }

	public virtual DbSet<RefreshTokens> RefreshTokens { get; set; }

	public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }

	public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }

	public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }

	public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }

	public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<PortalMenus>()
		.Property(b => b.ItemType)
		.IsRequired()
		.HasDefaultValueSql("'m'");

		modelBuilder.Entity<PortalPermissions>()
		.Property(b => b.ItemType)
		.IsRequired()
		.HasDefaultValueSql("'m'");

		modelBuilder.Entity<ApiMethodDefinitions>()
		.Property(b => b.ItemType)
		.IsRequired()
		.HasDefaultValueSql("'m'");

		modelBuilder.Entity<KafkaEvents>()
		.Property(b => b.OnComplete)
		.IsRequired()
		.HasDefaultValue(true);

		modelBuilder.Entity<KafkaEvents>()
		.Property(b => b.OnError)
		.IsRequired()
		.HasDefaultValue(true);

		modelBuilder.Entity<KafkaEvents>()
		.Property(b => b.OnTimeout)
		.IsRequired()
		.HasDefaultValue(true);

		modelBuilder.Entity<PortalPermissionGroups>()
		.Property(b => b.PortalPermissionTypeId)
		.IsRequired()
		.HasDefaultValueSql("0");

		modelBuilder.Entity<AspNetUsers>()
		.Property(b => b.PermissionGroupId)
		.IsRequired()
		.HasDefaultValueSql("1");

		modelBuilder.Entity<RefreshTokens>()
		.Property(b => b.CreatedDate)
		.IsRequired()
		.HasColumnType("datetime")
		.HasDefaultValueSql("getdate()");

		modelBuilder.Entity<RefreshTokens>()
		.Property(b => b.IsRevoked)
		.IsRequired()
		.HasDefaultValue(true);

		modelBuilder.Entity<PortalMenus>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<PortalMenus>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<ColumnTypes>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<ColumnTypes>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<PortalPermissionTypes>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<PortalPermissionTypes>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<PortalPermissions>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<PortalPermissions>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<ApiMethodDefinitions>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<ApiMethodDefinitions>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<KafkaEvents>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<KafkaEvents>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<ApiPermissionGroups>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<ApiPermissionGroups>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<PortalPermissionGroups>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<PortalPermissionGroups>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<PermissionGroups>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<PermissionGroups>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<TableComboboxSettings>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<TableComboboxSettings>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<RefreshTokens>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<RefreshTokens>().HasQueryFilter(r => !r.IsDeleted);


		modelBuilder.Entity<PortalMenus>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<ColumnTypes>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<PortalPermissionTypes>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<PortalPermissions>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<ApiMethodDefinitions>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<KafkaEvents>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<ApiPermissionGroups>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<PortalPermissionGroups>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<PermissionGroups>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<TableComboboxSettings>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<RefreshTokens>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");

		OnModelCreatingPartial(modelBuilder);

		foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
		{
		    relationship.DeleteBehavior = DeleteBehavior.Restrict;
		}
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
