using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    public class IntegrationBaseEvent
    {
        public IntegrationBaseEvent() : this(Guid.NewGuid(), DateTime.Now)
        {
            
        }

        public IntegrationBaseEvent(Guid id, DateTime creationDate)
        {
            Id = id;
            CreationDate = creationDate;
        }
        public Guid Id { get; set; } //Correlation Id
        public DateTime CreationDate { get; set; }
    }
}
