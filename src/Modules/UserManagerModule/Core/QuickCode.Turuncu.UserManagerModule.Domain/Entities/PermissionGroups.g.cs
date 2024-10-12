using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickCode.Turuncu.Common;

namespace QuickCode.Turuncu.UserManagerModule.Domain.Entities;

[Table("PermissionGroups")]
public partial class PermissionGroups  : BaseSoftDeletable 
{
	[Key]
	[Column("Id")]
	public int Id { get; set; }
	
	[Column("Name")]
	[StringLength(50)]
	public string Name { get; set; }
	
	[InverseProperty("PermissionGroup")]
	public virtual ICollection<AspNetUsers> AspNetUsers { get; } = new List<AspNetUsers>();
	[InverseProperty("PermissionGroup")]
	public virtual ICollection<PortalPermissionGroups> PortalPermissionGroups { get; } = new List<PortalPermissionGroups>();
	[InverseProperty("PermissionGroup")]
	public virtual ICollection<ApiPermissionGroups> ApiPermissionGroups { get; } = new List<ApiPermissionGroups>();
}

