using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickCode.Turuncu.Common;

namespace QuickCode.Turuncu.UserManagerModule.Domain.Entities;

[Table("AspNetUserClaims")]
public partial class AspNetUserClaims  
{
	[Key]
	[Column("Id")]
	public int Id { get; set; }
	
	[Column("UserId")]
	[StringLength(450)]
	public string UserId { get; set; }
	
	[Column("ClaimType")]
	[StringLength(int.MaxValue)]
	public string? ClaimType { get; set; }
	
	[Column("ClaimValue")]
	[StringLength(int.MaxValue)]
	public string? ClaimValue { get; set; }
	
	[ForeignKey("UserId")]
	[InverseProperty("AspNetUserClaims")]
	public virtual AspNetUsers User { get; set; } = null!;
}

