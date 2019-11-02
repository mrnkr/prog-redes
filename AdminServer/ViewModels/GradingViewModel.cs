using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminServer.ViewModels
{
    public class GradingViewModel
    {
        public string studentId { get; set; }
        public string subjectId { get; set; }
        public int grade { get; set; }
    }
}