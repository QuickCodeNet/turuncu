using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickCode.Turuncu.Common;

namespace QuickCode.Turuncu.SmsManagerModule.Domain.Entities;

[Table("SMS_SENDERS")]
public partial class SmsSenders  : BaseSoftDeletable 
{
	[Key]
	[Column("ID")]
	public int Id { get; set; }
	
	[Column("GSM_NUMBER")]
	[StringLength(250)]
	public string GsmNumber { get; set; }
	
	[Column("PROVIDER_NAME")]
	[StringLength(250)]
	public string ProviderName { get; set; }
	
	[InverseProperty("SmsSender")]
	public virtual ICollection<InfoMessages> InfoMessages { get; } = new List<InfoMessages>();
	[InverseProperty("SmsSender")]
	public virtual ICollection<OtpMessages> OtpMessages { get; } = new List<OtpMessages>();
	[InverseProperty("SmsSender")]
	public virtual ICollection<CampaignMessages> CampaignMessages { get; } = new List<CampaignMessages>();
}

