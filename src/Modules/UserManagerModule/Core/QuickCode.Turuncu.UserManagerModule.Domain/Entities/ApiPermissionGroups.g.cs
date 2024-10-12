using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickCode.Turuncu.Common;

namespace QuickCode.Turuncu.UserManagerModule.Domain.Entities;

[Table("ApiPermissionGroups")]
public partial class ApiPermissionGroups  : BaseSoftDeletable 
{
	[Key]
	[Column("Id")]
	public int Id { get; set; }
	
	[Column("PermissionGroupId")]
	public int PermissionGroupId { get; set; }
	
	[Column("ApiMethodDefinitionId")]
	public int ApiMethodDefinitionId { get; set; }
	
	[ForeignKey("ApiMethodDefinitionId")]
	[InverseProperty("ApiPermissionGroups")]
	public virtual ApiMethodDefinitions ApiMethodDefinition { get; set; } = null!;
	[ForeignKey("PermissionGroupId")]
	[InverseProperty("ApiPermissionGroups")]
	public virtual PermissionGroups PermissionGroup { get; set; } = null!;
}

