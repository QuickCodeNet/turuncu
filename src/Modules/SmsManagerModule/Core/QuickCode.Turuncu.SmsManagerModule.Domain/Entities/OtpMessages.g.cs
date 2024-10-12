using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickCode.Turuncu.Common;

namespace QuickCode.Turuncu.SmsManagerModule.Domain.Entities;

[Table("OTP_MESSAGES")]
public partial class OtpMessages  : BaseSoftDeletable 
{
	[Key]
	[Column("ID")]
	public int Id { get; set; }
	
	[Column("SMS_SENDER_ID")]
	public int SmsSenderId { get; set; }
	
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
	
	[ForeignKey("SmsSenderId")]
	[InverseProperty("OtpMessages")]
	public virtual SmsSenders SmsSender { get; set; } = null!;
	[ForeignKey("OtpTypeId")]
	[InverseProperty("OtpMessages")]
	public virtual OtpTypes OtpType { get; set; } = null!;
}

