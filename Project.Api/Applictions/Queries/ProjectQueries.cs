using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;

namespace Project.Api.Applictions.Queries
{
    public class ProjectQueries : IProjectQueries
    {
        private readonly string _connStr;

        public ProjectQueries(string connStr)
        {
            _connStr = connStr;
        }

        public async Task<dynamic> GetProjectByUserIdAsync(int userId)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                conn.Open();
                string sql = @"SELECT a.*
                                FROM Projects as a
                                WHERE a.UserId = @userId";
                var result = await conn.QueryAsync(sql, new { userId });
                return result;
            }
        }

        public async Task<dynamic> GetProjectDetailAsync(int projectId)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                conn.Open();
                var strSql = @"SELECT Projects.* FROM Projects INNER JOIN ProjectsVisibleRules ON
                                Projects.Id = ProjectsVisibleRules.ProjectId
                                WHERE Projects.Id = @projectId";
                var result = await conn.QueryAsync<dynamic>(strSql,new { projectId });
                return result;
            }
            
        }
    }
}
