using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickCode.Turuncu.Common;

namespace QuickCode.Turuncu.UserManagerModule.Domain.Entities;

[Table("PortalMenus")]
public partial class PortalMenus  : BaseSoftDeletable 
{
	[Key]
	[Column("Id")]
	public int Id { get; set; }
	
	[Column("Name")]
	[StringLength(250)]
	public string Name { get; set; }
	
	[Column("Text")]
	[StringLength(250)]
	public string Text { get; set; }
	
	[Column("Tooltip")]
	[StringLength(250)]
	public string Tooltip { get; set; }
	
	[Column("ActionName")]
	[StringLength(250)]
	public string ActionName { get; set; }
	
	[Column("OrderNo")]
	public int OrderNo { get; set; }
	
	[Column("ParentName")]
	[StringLength(250)]
	public string ParentName { get; set; }
	
	[Column("ItemType")]
	[StringLength(1)]
	public string ItemType { get; set; }
	
}

