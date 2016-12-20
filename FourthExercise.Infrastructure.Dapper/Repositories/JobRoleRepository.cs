using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;

using Dapper;

using FourthExercise.Models;
using FourthExercise.Infrastructure.Repositories;
using Checks;

namespace FourthExercise.Infrastructure.Dapper.Repositories
{
    public class JobRoleRepository : IReadJobRoleRepository
    {
        public JobRoleRepository(DbConnection dbConnection)
        {
            Check.NotNull(dbConnection, nameof(dbConnection));

            this.dbConnection = dbConnection;
        }

        private DbConnection dbConnection;

        public async Task<IEnumerable<JobRoleModel>> GetAllAsync()
        {
            bool close = await dbConnection.OpenIfNeededAsync();

            string sql = @"
                SELECT
                    Id, Name
                FROM JobRoles
            ";

            IEnumerable<JobRoleModel> jobRoleEmployees = await dbConnection.QueryAsync<JobRoleModel>(sql);

            if (close) { dbConnection.Close(); }

            return jobRoleEmployees;
        }
    }
}
