using System;
using System.Collections.Generic;

namespace RTMGMTCore2.Models
{
    public partial class FixRecords
    {
        public int Id { get; set; }
        public string ReportingId { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string ReportsToId { get; set; }
        public string RtString { get; set; }
        public string EmployeeId { get; set; }
    }
}
