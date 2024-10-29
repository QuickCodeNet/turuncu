using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickCode.Turuncu.Common;

namespace QuickCode.Turuncu.UserManagerModule.Domain.Entities;

[Table("TopicWorkflows")]
public partial class TopicWorkflows  : BaseSoftDeletable 
{
	[Key]
	[Column("Id")]
	public int Id { get; set; }
	
	[Column("KafkaEventId")]
	public int KafkaEventId { get; set; }
	
	[Column("WorkflowContent")]
	[StringLength(int.MaxValue)]
	public string WorkflowContent { get; set; }
	
	[ForeignKey("KafkaEventId")]
	[InverseProperty("TopicWorkflows")]
	public virtual KafkaEvents KafkaEvent { get; set; } = null!;
}

