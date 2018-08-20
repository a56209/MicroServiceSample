using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Project.Domain.AggregatesModel;

namespace Project.Api.Applictions.Commands
{
    public class ViewProjectCommandHandler : IRequestHandler<ViewProjectCommand>
    {
        private IProjectRepository _projectRepository;

        public ViewProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Unit> Handle(ViewProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetAsync(request.ProjectId);

            if (project == null)
            {
                throw new Domain.Exceptions.ProjectDomainException($"project not found: {request.ProjectId}");
            }

            project.AddViewer(request.UserId, request.UserName, request.Avatar);
            var result = await _projectRepository.unitOfWork.SaveEntitiesAsync(cancellationToken);
            
            return Unit.Value;
            //throw new NotImplementedException();
        }
    }
}
