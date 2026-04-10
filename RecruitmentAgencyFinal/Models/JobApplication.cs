using System;
using System.ComponentModel.DataAnnotations;

namespace RecruitmentAgencyFinal.Models
{
    public class JobApplication
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ResumeId { get; set; }
        public virtual Resume Resume { get; set; }

        [Required]
        public int VacancyId { get; set; }
        public virtual Vacancy Vacancy { get; set; }

        [Display(Name = "Дата подачи")]
        public DateTime AppliedAt { get; set; } = DateTime.Now;

        [Display(Name = "Статус")]
        public string Status { get; set; } = "Pending";

        [Display(Name = "Комментарий менеджера")]
        public string ManagerComment { get; set; }
    }
}