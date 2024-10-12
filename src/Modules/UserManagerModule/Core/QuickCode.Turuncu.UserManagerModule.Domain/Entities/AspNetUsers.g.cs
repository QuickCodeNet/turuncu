using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickCode.Turuncu.Common;

namespace QuickCode.Turuncu.UserManagerModule.Domain.Entities;

[Table("AspNetUsers")]
public partial class AspNetUsers  
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	[Column("Id")]
	[StringLength(450)]
	public string Id { get; set; }
	
	[Column("FirstName")]
	[StringLength(200)]
	public string? FirstName { get; set; }
	
	[Column("LastName")]
	[StringLength(200)]
	public string? LastName { get; set; }
	
	[Column("PermissionGroupId")]
	public int PermissionGroupId { get; set; }
	
	[Column("UserName")]
	[StringLength(256)]
	public string? UserName { get; set; }
	
	[Column("NormalizedUserName")]
	[StringLength(256)]
	public string? NormalizedUserName { get; set; }
	
	[Column("Email")]
	[StringLength(256)]
	public string? Email { get; set; }
	
	[Column("NormalizedEmail")]
	[StringLength(256)]
	public string? NormalizedEmail { get; set; }
	
	[Column("EmailConfirmed")]
	public bool EmailConfirmed { get; set; }
	
	[Column("PasswordHash")]
	[StringLength(int.MaxValue)]
	public string? PasswordHash { get; set; }
	
	[Column("SecurityStamp")]
	[StringLength(int.MaxValue)]
	public string? SecurityStamp { get; set; }
	
	[Column("ConcurrencyStamp")]
	[StringLength(int.MaxValue)]
	public string? ConcurrencyStamp { get; set; }
	
	[Column("PhoneNumber")]
	[StringLength(int.MaxValue)]
	public string? PhoneNumber { get; set; }
	
	[Column("PhoneNumberConfirmed")]
	public bool PhoneNumberConfirmed { get; set; }
	
	[Column("TwoFactorEnabled")]
	public bool TwoFactorEnabled { get; set; }
	
	[Column("LockoutEnd")]
	public DateTimeOffset? LockoutEnd { get; set; }
	
	[Column("LockoutEnabled")]
	public bool LockoutEnabled { get; set; }
	
	[Column("AccessFailedCount")]
	public int AccessFailedCount { get; set; }
	
	[InverseProperty("User")]
	public virtual ICollection<AspNetUserRoles> AspNetUserRoles { get; } = new List<AspNetUserRoles>();
	[InverseProperty("User")]
	public virtual ICollection<AspNetUserClaims> AspNetUserClaims { get; } = new List<AspNetUserClaims>();
	[InverseProperty("User")]
	public virtual ICollection<AspNetUserTokens> AspNetUserTokens { get; } = new List<AspNetUserTokens>();
	[InverseProperty("User")]
	public virtual ICollection<AspNetUserLogins> AspNetUserLogins { get; } = new List<AspNetUserLogins>();
	[InverseProperty("User")]
	public virtual ICollection<RefreshTokens> RefreshTokens { get; } = new List<RefreshTokens>();
	[ForeignKey("PermissionGroupId")]
	[InverseProperty("AspNetUsers")]
	public virtual PermissionGroups PermissionGroup { get; set; } = null!;
}

