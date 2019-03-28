using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrightTest.ViewModels
{
    public class RequestViewModel
    {
        public int Id { get; set; }
        
        public string DateTime { get; set; }

        public string URL { get; set; }

        public int StatusCode { get; set; }

        public string Title { get; set; }
    }
}
