using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickCode.Turuncu.Common;

namespace QuickCode.Turuncu.UserManagerModule.Domain.Entities;

[Table("PortalPermissionGroups")]
public partial class PortalPermissionGroups  : BaseSoftDeletable 
{
	[Key]
	[Column("Id")]
	public int Id { get; set; }
	
	[Column("PortalPermissionId")]
	public int PortalPermissionId { get; set; }
	
	[Column("PermissionGroupId")]
	public int PermissionGroupId { get; set; }
	
	[Column("PortalPermissionTypeId")]
	public int PortalPermissionTypeId { get; set; }
	
	[ForeignKey("PortalPermissionTypeId")]
	[InverseProperty("PortalPermissionGroups")]
	public virtual PortalPermissionTypes PortalPermissionType { get; set; } = null!;
	[ForeignKey("PortalPermissionId")]
	[InverseProperty("PortalPermissionGroups")]
	public virtual PortalPermissions PortalPermission { get; set; } = null!;
	[ForeignKey("PermissionGroupId")]
	[InverseProperty("PortalPermissionGroups")]
	public virtual PermissionGroups PermissionGroup { get; set; } = null!;
}

