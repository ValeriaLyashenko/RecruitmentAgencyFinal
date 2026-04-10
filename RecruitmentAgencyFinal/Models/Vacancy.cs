using System;
using System.ComponentModel.DataAnnotations;

namespace RecruitmentAgencyFinal.Models
{
    public class Vacancy
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Название вакансии")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Зарплата (руб.)")]
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        [Display(Name = "Требования")]
        public string Requirements { get; set; }

        [Display(Name = "Опыт работы (лет)")]
        public int ExperienceRequired { get; set; }

        [Display(Name = "Активна")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Дата создания")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Кем создана")]
        public string CreatedBy { get; set; }
    }
}