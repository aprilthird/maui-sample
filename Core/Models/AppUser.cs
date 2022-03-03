using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class AppUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Range(0, 120)]
        public int Age { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public AppUser()
        {
        }

        public AppUser(string name, string email, int age, DateTime birthDate)
        {
            Name = name;
            Email = email;
            Age = age;
            BirthDate = birthDate;
        }
    }
}
