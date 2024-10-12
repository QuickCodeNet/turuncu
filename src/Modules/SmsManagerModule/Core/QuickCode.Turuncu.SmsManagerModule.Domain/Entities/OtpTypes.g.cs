using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickCode.Turuncu.Common;

namespace QuickCode.Turuncu.SmsManagerModule.Domain.Entities;

[Table("OTP_TYPES")]
public partial class OtpTypes  : BaseSoftDeletable 
{
	[Key]
	[Column("ID")]
	public int Id { get; set; }
	
	[Column("NAME")]
	[StringLength(250)]
	public string Name { get; set; }
	
	[Column("TEMPLATE")]
	[StringLength(250)]
	public string Template { get; set; }
	
	[InverseProperty("OtpType")]
	public virtual ICollection<OtpMessages> OtpMessages { get; } = new List<OtpMessages>();
}

