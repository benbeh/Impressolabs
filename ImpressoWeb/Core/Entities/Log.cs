using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Log
    {
        [Key]
        public int Id { get; set; }

        public string Message { get; set; }

        public string ExceptionMessage { get; set; }

        public string StackTrace { get; set; }

        public string InnerExceptionMessage { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}