using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Project.Domain.Events
{
    public class ProjectCreatedEvent:INotification
    {
        public Domain.AggregatesModel.Project Project { get; set; }
    }
}
