using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CRUD
{
    [Table("user")]
    public class User
    {
        [Key]
        [Column("user_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        [Column("user_name")]
        public string? Name { get; set; }
        [Required]
        [Column ("phonenumber")]
        public string? PhoneNumber { get; set; }
        public override string ToString()
        {
            return $"Id: {Id}\tName: {Name}\tPhoneNumber: {PhoneNumber}";
        }
    }
}
