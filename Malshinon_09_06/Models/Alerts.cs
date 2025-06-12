using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon_09_06.DAL
{
    internal class Alerts
    {
        public int? Id {  get; }
        public int TargetId { get; }
        DateTime? CreatedAt { get; set; } 
        public string Reason { get; }

        public Alerts(int? id, int targetId, DateTime? createdAt, string reason)
        {
            Id = id;
            TargetId = targetId;
            CreatedAt = createdAt;
            Reason = reason;
        }

        public override string ToString()
        {
            return $"Alert INFO: \n" +
                   $"Alert ID: {Id} \n" +
                   $"Target ID: {TargetId} \n" +
                   $"Time Created: {CreatedAt} \n" +
                   $"Reason Of Alert: {Reason} \n";
        }
    }
}
