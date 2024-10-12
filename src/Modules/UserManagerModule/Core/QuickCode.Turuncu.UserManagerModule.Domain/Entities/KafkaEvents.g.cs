using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickCode.Turuncu.Common;

namespace QuickCode.Turuncu.UserManagerModule.Domain.Entities;

[Table("KafkaEvents")]
public partial class KafkaEvents  : BaseSoftDeletable 
{
	[Key]
	[Column("Id")]
	public int Id { get; set; }
	
	[Column("ApiMethodDefinitionId")]
	public int ApiMethodDefinitionId { get; set; }
	
	[Column("TopicName")]
	[StringLength(250)]
	public string TopicName { get; set; }
	
	[Column("OnComplete")]
	public bool OnComplete { get; set; }
	
	[Column("OnError")]
	public bool OnError { get; set; }
	
	[Column("OnTimeout")]
	public bool OnTimeout { get; set; }
	
	[ForeignKey("ApiMethodDefinitionId")]
	[InverseProperty("KafkaEvents")]
	public virtual ApiMethodDefinitions ApiMethodDefinition { get; set; } = null!;
}

