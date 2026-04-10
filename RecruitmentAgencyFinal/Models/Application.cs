using System;
using System.ComponentModel.DataAnnotations;

namespace RecruitmentAgencyFinal.Models
{
    public class Application
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ResumeId { get; set; }
        public virtual Resume Resume { get; set; }

        [Required]
        public int VacancyId { get; set; }
        public virtual Vacancy Vacancy { get; set; }

        [Display(Name = "Дата отклика")]
        public DateTime AppliedAt { get; set; } = DateTime.Now;

        [Display(Name = "Статус")]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

        [Display(Name = "Комментарий менеджера")]
        public string ManagerComment { get; set; }
    }
}