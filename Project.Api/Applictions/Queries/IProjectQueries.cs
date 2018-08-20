using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Api.Applictions.Queries
{
    public interface IProjectQueries
    {
        Task<dynamic> GetProjectByUserIdAsync(int userId);
        Task<dynamic> GetProjectDetailAsync(int projectId);
    }
}
