using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace JobApplicationTracker.Models
{
    public class JobApplication
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string JobTitle { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public DateTime ApplicationDate { get; set; } = DateTime.Now;
        public string JobDescription { get; set; } = string.Empty;
        public string Salary { get; set; } = string.Empty;
        public int WeeklyWorkHours { get; set; } = 0;
        public string City { get; set; } = string.Empty;
        public string StreetAddress { get; set; } = string.Empty;
        public string ContactStatus { get; set; } = string.Empty;
        public string InterviewDate { get; set; } = string.Empty;
        public bool IsRejected { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }
}
