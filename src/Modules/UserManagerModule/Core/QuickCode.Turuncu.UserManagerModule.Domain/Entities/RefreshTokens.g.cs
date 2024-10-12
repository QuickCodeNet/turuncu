using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickCode.Turuncu.Common;

namespace QuickCode.Turuncu.UserManagerModule.Domain.Entities;

[Table("RefreshTokens")]
public partial class RefreshTokens  : BaseSoftDeletable 
{
	[Key]
	[Column("Id")]
	public int Id { get; set; }
	
	[Column("UserId")]
	[StringLength(450)]
	public string UserId { get; set; }
	
	[Column("Token")]
	[StringLength(500)]
	public string Token { get; set; }
	
	[Column("ExpiryDate")]
	public DateTime ExpiryDate { get; set; }
	
	[Column("CreatedDate")]
	public DateTime CreatedDate { get; set; }
	
	[Column("IsRevoked")]
	public bool IsRevoked { get; set; }
	
	[ForeignKey("UserId")]
	[InverseProperty("RefreshTokens")]
	public virtual AspNetUsers User { get; set; } = null!;
}

