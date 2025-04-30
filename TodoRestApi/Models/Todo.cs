using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoRestApi.Models;

[Index("UserId", Name = "IX_Notes_user_id")]
public partial class Todo
{
    [Key]
    public int Id { get; set; }

    [Column("content")]
    [StringLength(500)]
    public string? Content { get; set; }

    [Column("user_id")]
    public int? UserId { get; set; }

    [Column("title")]
    [StringLength(100)]
    public string? Title { get; set; }
    public bool IsDone { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("Todo")]
    public virtual ICollection<TodoTag> TodoTags { get; set; } = new List<TodoTag>();

    [ForeignKey("UserId")]
    [InverseProperty("Todos")]
    public virtual User? User { get; set; }
}
