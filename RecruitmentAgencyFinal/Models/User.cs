using System;
using System.ComponentModel.DataAnnotations;

namespace RecruitmentAgencyFinal.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Display(Name = "ФИО")]
        public string FullName { get; set; }

        [Display(Name = "Роль")]
        public string Role { get; set; }

        [Display(Name = "Дата регистрации")]
        public DateTime RegisteredAt { get; set; }

        [Display(Name = "Активен")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Доступ до")]
        public DateTime? AccessUntil { get; set; }
    }
}