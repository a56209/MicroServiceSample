using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Project.Domain.AggregatesModel;

namespace Project.Domain.Events
{
    public class ProjectJoinedEvent:INotification
    {
        public ProjectContributor Contributor { get; set; }
    }
}
