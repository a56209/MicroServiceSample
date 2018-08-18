using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Project.Domain.AggregatesModel;
using Project.Domain;

namespace Project.Api.Applictions.Commands
{
    public class JoinProjectCommandHandler:IRequestHandler<JoinProjectCommand>
    {
        private IProjectRepository _projectRepository;

        public JoinProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }   

        public async Task<Unit> Handle(JoinProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetAsync(request.Contributor.ProjectId);

            if (project == null)
            {
                throw new Domain.Exceptions.ProjectDomainException($"project not found: {request.Contributor.ProjectId}");
            }

            project.AddContributor(request.Contributor);

            await _projectRepository.unitOfWork.SaveEntitiesAsync();
            throw new NotImplementedException();
        }

        //protected override async Task Handle(JoinProjectCommand request, CancellationToken cancellationToken)
        //{
        //    var project = await _projectRepository.GetAsync(request.Contributor.ProjectId);

        //    if (project == null)
        //    {
        //        throw new Domain.Exceptions.ProjectDomainException($"project not found: {request.Contributor.ProjectId}");
        //    }

        //    project.AddContributor(request.Contributor);

        //    await _projectRepository.unitOfWork.SaveEntitiesAsync();
        //}

        //public async Task<Unit> Handle(JoinProjectCommand request, CancellationToken cancellationToken)
        //{
        //    var project = await _projectRepository.GetAsync(request.Contributor.ProjectId);

        //    if (project == null)
        //    {
        //        throw new Domain.Exceptions.ProjectDomainException($"project not found: {request.Contributor.ProjectId}");
        //    }

        //    project.AddContributor(request.Contributor);
        //    MediatR.Unit
        //    return await _projectRepository.unitOfWork.SaveEntitiesAsync();
        //}

        //public async Task<int> Handle(JoinProjectCommand request, CancellationToken cancellationToken)
        //{
        //    var project = await _projectRepository.GetAsync(request.Contributor.ProjectId);

        //    if (project == null)
        //    {
        //        throw new Domain.Exceptions.ProjectDomainException($"project not found: {request.Contributor.ProjectId}");
        //    }

        //    project.AddContributor(request.Contributor);
        //    //MediatR.Unit
        //    return await _projectRepository.unitOfWork.SaveChangesAsync();
        //}


    }
}
