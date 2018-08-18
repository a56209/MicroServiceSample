using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Project.Domain;

namespace Project.Api.Applictions.Commands
{
    public class CreateProjectCommand:IRequest<Domain.AggregatesModel.Project>
    {
        public Domain.AggregatesModel.Project Project { get; set; }
    }
}
