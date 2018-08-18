
using Project.Domain.AggregatesModel;
using Project.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Project.Infrastructure.Repositories
{
    public class PraojectRepository : IProjectRepository
    {
        private ProjectContext _dbContext;
        public IUnitOfWork unitOfWork => _dbContext;    

        public PraojectRepository(ProjectContext ProjectContext)
        {
            _dbContext = ProjectContext;
        }


        //public IUnitOfWork unitOfWork => throw new NotImplementedException();


        public async Task<Domain.AggregatesModel.Project> GetAsync(int Id)
        {
            var project = await _dbContext.Projects
                .Include(p => p.Properties)
                .Include(p => p.Viewers)
                .Include(p => p.Contributors)
                .Include(p => p.VisibleRule)
                .SingleOrDefaultAsync(p => p.Id == Id);

            return project;
        }

        public void Update(Domain.AggregatesModel.Project project)
        {
            _dbContext.Update(project);
        }

        public Domain.AggregatesModel.Project Add(Domain.AggregatesModel.Project project)
        {
            if (project.IsTransient())
            {
                return _dbContext.Projects.Add(project).Entity;
            }
            else
            {
                return project;
            }
        }
    }
}
