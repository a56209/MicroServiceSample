using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Project.Domain.SeedWork;

namespace Project.Domain.AggregatesModel
{
    public interface IProjectRepository:IRepository<Project>
    {
        Task<Project> GetAsync(int Id);
        //Task<Project> AddAsync(Project project);
        //Task<Project> UpdateAsync(Project project);
        Project Add(Project project);
        void Update(Project project);
    }
}
