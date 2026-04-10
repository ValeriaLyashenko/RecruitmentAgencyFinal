using System;
using System.ComponentModel.DataAnnotations;

namespace RecruitmentAgencyFinal.Models
{
    public class Resume
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required(ErrorMessage = "ФИО обязательно")]
        [Display(Name = "ФИО")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Дата рождения обязательна")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата рождения")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Должность обязательна")]
        [Display(Name = "Желаемая должность")]
        public string Position { get; set; }

        [Display(Name = "Опыт работы (лет)")]
        [Range(0, 50, ErrorMessage = "Опыт должен быть от 0 до 50 лет")]
        public int Experience { get; set; }

        [Display(Name = "Образование")]
        public string Education { get; set; }

        [Display(Name = "Навыки")]
        public string Skills { get; set; }

        [Required(ErrorMessage = "Ожидаемая зарплата обязательна")]
        [Display(Name = "Ожидаемая зарплата (руб.)")]
        [DataType(DataType.Currency)]
        public decimal ExpectedSalary { get; set; }

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Телефон обязателен")]
        [Display(Name = "Телефон")]
        [RegularExpression(@"^\+?[0-9\s\-\(\)]{10,20}$", ErrorMessage = "Некорректный телефон")]
        public string Phone { get; set; }

        [Display(Name = "Дата создания")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Статус")]
        public string Status { get; set; } = "Active";
    }
}