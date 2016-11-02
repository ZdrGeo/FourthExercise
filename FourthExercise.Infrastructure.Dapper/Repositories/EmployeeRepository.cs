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

namespace FourthExercise.Infrastructure.Dapper.Repositories
{
    public class EmployeeRepository : IReadEmployeeRepository, IWriteEmployeeRepository
    {
        public EmployeeRepository(DbConnection dbConnection)
        {
            if (dbConnection == null) { throw new ArgumentNullException("dbConnection"); }

            this.dbConnection = dbConnection;
        }

        private DbConnection dbConnection;

        public async Task<IEnumerable<EmployeeModel>> FindWithNameAsync(string name)
        {
            bool close = await dbConnection.OpenIfNeededAsync();

            string sql = @"
                SELECT
                    Employees.Id, Employees.FirstName, Employees.LastName, Employees.Email, Employees.JobRoleId, Employees.Salary, JobRoles.Id, JobRoles.Name
                FROM Employees
                INNER JOIN JobRoles
                ON JobRoles.Id = Employees.JobRoleId
                WHERE Employees.FirstName LIKE '%' + @Name + '%'
                    OR Employees.LastName LIKE '%' + @Name + '%'
            ";

            IEnumerable<EmployeeModel> employeeModels = await dbConnection.QueryAsync<EmployeeModel, JobRoleModel, EmployeeModel>(sql, (em, jrm) =>
            {
                em.JobRole = jrm; return em;
            }, new { Name = name ?? string.Empty });

            if (close) { dbConnection.Close(); }

            return employeeModels;
        }

        public async Task<EmployeeModel> GetAsync(int id)
        {
            bool close = await dbConnection.OpenIfNeededAsync();

            string sql = @"
                SELECT
                    Employees.Id, Employees.FirstName, Employees.LastName, Employees.Email, Employees.JobRoleId, Employees.Salary, JobRoles.Id, JobRoles.Name
                FROM Employees
                INNER JOIN JobRoles
                ON JobRoles.Id = Employees.JobRoleId
                WHERE Employees.Id = @Id
            ";

            EmployeeModel employeeModel = (await dbConnection.QueryAsync<EmployeeModel, JobRoleModel, EmployeeModel>(sql, (em, jrm) =>
            {
                em.JobRole = jrm; return em;
            }, new { Id = id })).FirstOrDefault();

            if (close) { dbConnection.Close(); }

            return employeeModel;
        }

        public async Task AddAsync(EmployeeModel employeeModel)
        {
            bool close = await dbConnection.OpenIfNeededAsync();

            string sql = @"
                INSERT INTO Employees (
                    FirstName, LastName, Email, JobRoleId, Salary
                ) VALUES (
                    @FirstName, @LastName, @Email, @JobRoleId, @Salary
                )
            ";

            await dbConnection.ExecuteAsync(sql, employeeModel);

            if (close) { dbConnection.Close(); }
        }

        public async Task SetAsync(EmployeeModel employeeModel)
        {
            bool close = await dbConnection.OpenIfNeededAsync();

            string sql = @"
                UPDATE Employees
                SET
                    FirstName = @FirstName, LastName = @LastName, Email = @Email, JobRoleId = @JobRoleId, Salary = @Salary
                WHERE Id = @Id
            ";

            await dbConnection.ExecuteAsync(sql, employeeModel);

            if (close) { dbConnection.Close(); }
        }

        public async Task RemoveAsync(EmployeeModel employeeModel)
        {
            bool close = await dbConnection.OpenIfNeededAsync();

            string sql = @"
                DELETE FROM Employees
                WHERE Id = @Id
            ";

            await dbConnection.ExecuteAsync(sql, new { employeeModel.Id });

            if (close) { dbConnection.Close(); }
        }
    }
}
