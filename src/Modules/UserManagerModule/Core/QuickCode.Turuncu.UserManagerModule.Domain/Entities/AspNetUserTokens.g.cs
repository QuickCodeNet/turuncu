using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickCode.Turuncu.Common;

namespace QuickCode.Turuncu.UserManagerModule.Domain.Entities;

[Table("AspNetUserTokens")]
public partial class AspNetUserTokens  
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	[Column("UserId")]
	[StringLength(450)]
	public string UserId { get; set; }
	
	[Column("LoginProvider")]
	[StringLength(450)]
	public string LoginProvider { get; set; }
	
	[Column("Name")]
	[StringLength(450)]
	public string Name { get; set; }
	
	[Column("Value")]
	[StringLength(int.MaxValue)]
	public string? Value { get; set; }
	
	[ForeignKey("UserId")]
	[InverseProperty("AspNetUserTokens")]
	public virtual AspNetUsers User { get; set; } = null!;
}

