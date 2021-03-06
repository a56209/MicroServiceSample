﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Project.Domain;
using Project.Domain.AggregatesModel;

namespace Project.Api.Applictions.Commands
{
    public class CreateProjectCommandHandler:IRequestHandler<CreateProjectCommand,Domain.AggregatesModel.Project>
    {
        private IProjectRepository _projectRepository;

        public CreateProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Domain.AggregatesModel.Project> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            _projectRepository.Add(request.Project);
            await _projectRepository.unitOfWork.SaveEntitiesAsync();

            return request.Project;
        }

      
    }
}
