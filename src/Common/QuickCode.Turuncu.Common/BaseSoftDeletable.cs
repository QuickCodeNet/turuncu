using System.ComponentModel.DataAnnotations.Schema;

namespace QuickCode.Turuncu.Common;

public class BaseSoftDeletable
{
    [Column("IsDeleted")]
    public bool IsDeleted { get; set; }
    
    [Column("DeletedOnUtc")]
    public DateTime? DeletedOnUtc { get; set; }
}