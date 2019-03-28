using System;
using System.ComponentModel.DataAnnotations;

namespace BrightTest.Models
{
    public class Request
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string URL { get; set; }

        [Required]
        public int StatusCode { get; set; }

        [Required]
        public string Title { get; set; }
    }
}
