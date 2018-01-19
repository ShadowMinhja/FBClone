using System;
using System.Collections.Generic;
using System.Linq;
using FBClone.Data;
using FBClone.Data.Infrastructure;
using FBClone.Data.Repositories;
using FBClone.Model;
using System.Data.SqlClient;
using System.Diagnostics;

namespace FBClone.Service
{
    public interface IEntityDBService
    {
        IEnumerable<T> GetSPData<T>(string reportName, List<SqlParameter> sqlParams);
        void ExecuteStoredProc(string sqlCmd, SqlParameter[] parameters);
    }

    public class EntityDBService : IEntityDBService
    {
        private FBCloneContext dbContext;
        public EntityDBService()
        {
            this.dbContext = new FBCloneContext();
        }

        public IEnumerable<T> GetSPData<T>(string reportName, List<SqlParameter> sqlParams)
        {
            List<T> results = new List<T>();
            try
            {
                foreach(var param in sqlParams)
                {
                    reportName += " '" + param.SqlValue + "',";
                }
                var query = this.dbContext.Database.SqlQuery<T>(String.Format(reportName.TrimEnd(','), sqlParams.ToArray()));
                results = query.Cast<T>().ToList();
                return results;
            }
            catch (SqlException ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        public void ExecuteStoredProc(string sqlCmd, SqlParameter[] parameters)
        {
            this.dbContext.Database.ExecuteSqlCommand(sqlCmd, parameters);
        }
    }
}
