using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;

namespace FourthExercise.Infrastructure.Dapper
{
    public static class DbConnectionExtensions
    {
        public static async Task<bool> OpenIfNeededAsync(this DbConnection dbConnection)
        {
            bool wasNedded = false;

            if (dbConnection.State == ConnectionState.Broken) { dbConnection.Close(); }
            if (dbConnection.State == ConnectionState.Closed) { await dbConnection.OpenAsync(); wasNedded = true; }

            return wasNedded;
        }
    }
}
