using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Api.Applictions.Service
{
    public class TestRecommendService : IRecommendService
    {
        public Task<bool> IsProjectInRecommend(int projectId, int userId)
        {
            return Task.FromResult(true);
        }
    }
}
