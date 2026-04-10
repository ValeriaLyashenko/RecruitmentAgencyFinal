using System;
using System.ComponentModel.DataAnnotations;

namespace RecruitmentAgencyFinal.Models
{
    public class Candidate
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "ФИО обязательно для заполнения")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "ФИО должно содержать от 2 до 200 символов")]
        [Display(Name = "ФИО")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Должность обязательна для заполнения")]
        [StringLength(100, ErrorMessage = "Должность не может быть длиннее 100 символов")]
        [Display(Name = "Должность")]
        public string Position { get; set; }

        [Required(ErrorMessage = "Email обязателен для заполнения")]
        [EmailAddress(ErrorMessage = "Введите корректный email адрес")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Телефон обязателен для заполнения")]
        [RegularExpression(@"^\+?[0-9\s\-\(\)]{10,20}$", ErrorMessage = "Введите корректный номер телефона")]
        [Display(Name = "Телефон")]
        public string Phone { get; set; }

        [Display(Name = "Дата подачи заявки")]
        public DateTime ApplicationDate { get; set; }
    }
}