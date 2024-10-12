using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickCode.Turuncu.Common;

namespace QuickCode.Turuncu.UserManagerModule.Domain.Entities;

[Table("ApiMethodDefinitions")]
public partial class ApiMethodDefinitions  : BaseSoftDeletable 
{
	[Key]
	[Column("Id")]
	public int Id { get; set; }
	
	[Column("HttpMethod")]
	[StringLength(250)]
	public string HttpMethod { get; set; }
	
	[Column("ControllerName")]
	[StringLength(250)]
	public string ControllerName { get; set; }
	
	[Column("Path")]
	[StringLength(250)]
	public string Path { get; set; }
	
	[Column("ItemType")]
	[StringLength(1)]
	public string ItemType { get; set; }
	
	[InverseProperty("ApiMethodDefinition")]
	public virtual ICollection<KafkaEvents> KafkaEvents { get; } = new List<KafkaEvents>();
	[InverseProperty("ApiMethodDefinition")]
	public virtual ICollection<ApiPermissionGroups> ApiPermissionGroups { get; } = new List<ApiPermissionGroups>();
}

