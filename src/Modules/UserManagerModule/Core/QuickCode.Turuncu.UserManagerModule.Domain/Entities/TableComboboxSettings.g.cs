using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickCode.Turuncu.Common;

namespace QuickCode.Turuncu.UserManagerModule.Domain.Entities;

[Table("TableComboboxSettings")]
public partial class TableComboboxSettings  : BaseSoftDeletable 
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	[Column("TableName")]
	[StringLength(250)]
	public string TableName { get; set; }
	
	[Column("IdColumn")]
	[StringLength(250)]
	public string IdColumn { get; set; }
	
	[Column("RefTableName")]
	[StringLength(250)]
	public string RefTableName { get; set; }
	
	[Column("RefTableColumnId")]
	[StringLength(250)]
	public string RefTableColumnId { get; set; }
	
	[Column("RefIdColumn")]
	[StringLength(250)]
	public string RefIdColumn { get; set; }
	
	[Column("TextColumns")]
	[StringLength(int.MaxValue)]
	public string TextColumns { get; set; }
	
	[Column("StringFormat")]
	[StringLength(int.MaxValue)]
	public string StringFormat { get; set; }
	
}

