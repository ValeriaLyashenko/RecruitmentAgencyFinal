using System;

namespace RecruitmentAgencyFinal.Models
{
    public class UserTemp
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public DateTime RegisteredAt { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? AccessUntil { get; set; }
    }
}