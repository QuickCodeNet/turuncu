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
	[StringLength(1000)]
	public string TopicName { get; set; }
	
	[Column("IsActive")]
	public bool IsActive { get; set; }
	
	[InverseProperty("KafkaEvent")]
	public virtual ICollection<TopicWorkflows> TopicWorkflows { get; } = new List<TopicWorkflows>();
	[ForeignKey("ApiMethodDefinitionId")]
	[InverseProperty("KafkaEvents")]
	public virtual ApiMethodDefinitions ApiMethodDefinition { get; set; } = null!;
}

