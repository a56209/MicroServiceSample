using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Project.Domain;
using Project.Api.Applictions.Commands;
using Project.Api.Applictions.Service;
using Project.Api.Applictions.Queries;
using MediatR;

namespace Project.Api.Controllers
{
    [Route("api/projects")]
    public class ProjectController : BaseController
    {
        private IMediator _mediator;
        private IRecommendService _recommendService;
        private IProjectQueries _projectQueries;

        public ProjectController(IMediator mediator, IRecommendService recommendService,IProjectQueries projectQueries)
        {
            _mediator = mediator;
            _recommendService = recommendService;
            _projectQueries = projectQueries;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetProjectsAsync()
        {
            var projects = await _projectQueries.GetProjectByUserIdAsync(UserIdentity.UserId);
            return Ok(projects);
        }

        [HttpGet]
        [Route("my/{projectId}")]
        public async Task<IActionResult> GetMyProjectDetail(int projectId)
        {
            var project = await _projectQueries.GetProjectDetailAsync(projectId);
            if(project.UserId == UserIdentity.UserId)
            { 
            return Ok(project);
            }
            else
            {
                return BadRequest("无权查看该项目");
            }
        }

        [HttpGet]
        [Route("recommends/{projectId}")]
        public async Task<IActionResult> GerRecommendProjectDetail(int projectId)
        {
            if(await _recommendService.IsProjectInRecommend(projectId,UserIdentity.UserId))
            {
                var project = await _projectQueries.GetProjectDetailAsync(projectId);
                return Ok(project);
            }
            else
            {
                return BadRequest("无权查看该项目");
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateProjectAsync([FromBody]Domain.AggregatesModel.Project project)
        {
            var command = new CreateProjectCommand() { Project = project};
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPut]
        [Route("view/{porjectId}")]
        public async Task<IActionResult> ViewProjectAsync(int projectId)
        {
            if (await _recommendService.IsProjectInRecommend(projectId, UserIdentity.UserId))
            {
                return BadRequest("没有查看该项目的权限！");
            }
            var command = new ViewProjectCommand() {
                UserId = UserIdentity.UserId,
                UserName = UserIdentity.Name,
                Avatar = UserIdentity.Avatar,
                ProjectId = projectId
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        [Route("join/{projectId}")]
        public async Task<IActionResult> JoinProjectAsync([FromBody]Domain.AggregatesModel.ProjectContributor contributor)
        {
            if (await _recommendService.IsProjectInRecommend(contributor.ProjectId, UserIdentity.UserId))
            {
                return BadRequest("没有查看该项目的权限！");
            }
            var command = new JoinProjectCommand()
            {
                Contributor = contributor
            };

            var result = await _mediator.Send(command);
            return Ok(result);
    }

    }
}
