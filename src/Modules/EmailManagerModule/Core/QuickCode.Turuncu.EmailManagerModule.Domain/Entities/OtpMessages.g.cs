using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickCode.Turuncu.Common;

namespace QuickCode.Turuncu.EmailManagerModule.Domain.Entities;

[Table("OTP_MESSAGES")]
public partial class OtpMessages  : BaseSoftDeletable 
{
	[Key]
	[Column("ID")]
	public int Id { get; set; }
	
	[Column("EMAIL_SENDER_ID")]
	public int EmailSenderId { get; set; }
	
	[Column("OTP_TYPE_ID")]
	public int OtpTypeId { get; set; }
	
	[Column("GSM_NUMBER")]
	[StringLength(250)]
	public string GsmNumber { get; set; }
	
	[Column("OTP_CODE")]
	[StringLength(250)]
	public string OtpCode { get; set; }
	
	[Column("MESSAGE")]
	[StringLength(250)]
	public string Message { get; set; }
	
	[Column("EXPIRE_SECONDS")]
	public int ExpireSeconds { get; set; }
	
	[Column("MESSAGE_DATE")]
	public DateTime? MessageDate { get; set; }
	
	[Column("MESSAGE_SID")]
	[StringLength(250)]
	public string MessageSid { get; set; }
	
	[Column("DAILY_COUNTER")]
	public int DailyCounter { get; set; }
	
	[ForeignKey("EmailSenderId")]
	[InverseProperty("OtpMessages")]
	public virtual EmailSenders EmailSender { get; set; } = null!;
	[ForeignKey("OtpTypeId")]
	[InverseProperty("OtpMessages")]
	public virtual OtpTypes OtpType { get; set; } = null!;
}

