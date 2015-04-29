using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Playground.Mvc.DataModel
{
    [Table("Files")]
    public class UploadedFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public byte[] Content { get; set; }

        [Required]
        [StringLength(50)]
        public string ContentType { get; set; }
    }
}
