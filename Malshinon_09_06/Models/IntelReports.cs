using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon_09_06
{
    internal class IntelReports
    {
        public int ReporterId { get; set; }
        public int TargetId { get; set; }  
        public string Text { get; }

        public IntelReports(int reporterId, int targetId, string text)
        {
            ReporterId = reporterId;
            TargetId = targetId;
            Text = text;
        }
    }
}
