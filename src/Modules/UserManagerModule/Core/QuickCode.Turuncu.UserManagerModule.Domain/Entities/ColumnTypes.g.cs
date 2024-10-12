using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickCode.Turuncu.Common;

namespace QuickCode.Turuncu.UserManagerModule.Domain.Entities;

[Table("ColumnTypes")]
public partial class ColumnTypes  : BaseSoftDeletable 
{
	[Key]
	[Column("Id")]
	public int Id { get; set; }
	
	[Column("TypeName")]
	[StringLength(250)]
	public string TypeName { get; set; }
	
	[Column("IosComponentName")]
	[StringLength(250)]
	public string IosComponentName { get; set; }
	
	[Column("IosType")]
	[StringLength(250)]
	public string IosType { get; set; }
	
	[Column("IconCode")]
	[StringLength(250)]
	public string IconCode { get; set; }
	
}

