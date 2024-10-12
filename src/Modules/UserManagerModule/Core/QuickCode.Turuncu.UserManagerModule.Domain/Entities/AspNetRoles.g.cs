using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickCode.Turuncu.Common;

namespace QuickCode.Turuncu.UserManagerModule.Domain.Entities;

[Table("AspNetRoles")]
public partial class AspNetRoles  
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	[Column("Id")]
	[StringLength(450)]
	public string Id { get; set; }
	
	[Column("Name")]
	[StringLength(256)]
	public string Name { get; set; }
	
	[Column("NormalizedName")]
	[StringLength(256)]
	public string? NormalizedName { get; set; }
	
	[Column("ConcurrencyStamp")]
	[StringLength(int.MaxValue)]
	public string? ConcurrencyStamp { get; set; }
	
	[InverseProperty("Role")]
	public virtual ICollection<AspNetUserRoles> AspNetUserRoles { get; } = new List<AspNetUserRoles>();
	[InverseProperty("Role")]
	public virtual ICollection<AspNetRoleClaims> AspNetRoleClaims { get; } = new List<AspNetRoleClaims>();
}

