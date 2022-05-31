using Motorsazan.CustomerAppWeb.Api.Models.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Motorsazan.CustomerAppWeb.Api.Utilities
{
    public class DataBase
    {
        public static QueryResult ExecuteStoredProcedure(string storedProcedureName,
            List<SqlParameter> sqlParameters = default)
        {
            sqlParameters = sqlParameters ?? new List<SqlParameter>();

            sqlParameters.Add(new SqlParameter
            {
                ParameterName = "@output_status", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output
            });
            sqlParameters.Add(new SqlParameter
            {
                ParameterName = "@output_message",
                SqlDbType = SqlDbType.NVarChar,
                Size = 4000,
                Direction = ParameterDirection.Output
            });
            sqlParameters.Add(new SqlParameter
            {
                ParameterName = "@returnvalue",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.ReturnValue
            });

            return Execute(storedProcedureName, CommandType.StoredProcedure, sqlParameters.ToArray());
        }

        public static QueryResult Execute(string commandText, CommandType commandType, SqlParameter[] parameters)
        {
            QueryResult result;
            var dS = new DataSet();
            var returnCodeId = parameters.Length - 1;
            var resultTextId = parameters.Length - 2;
            var spResultCodeId = parameters.Length - 3;

            try
            {
                using(var connection = new SqlConnection(DataBaseConfig.ConnectionString))
                using(var command = new SqlCommand(commandText, connection)
                      {
                          CommandTimeout = DataBaseConfig.CommandTimeout, CommandType = commandType
                      })
                using(var dataAdaptor = new SqlDataAdapter(command))
                {
                    command.Parameters.AddRange(parameters);

                    connection.Open();
                    _ = dataAdaptor.Fill(dS);
                }

                if((int)parameters[returnCodeId].Value != 1)
                {
                    result = new QueryResult
                    {
                        ReturnCode = (int)parameters[returnCodeId].Value,
                        Text = (string)parameters[resultTextId].Value
                    };
                    return result;
                }

                if((int)parameters[spResultCodeId].Value != 1)
                {
                    result = new QueryResult
                    {
                        SPCode = (int)parameters[spResultCodeId].Value,
                        ReturnCode = (int)parameters[returnCodeId].Value,
                        Text = (string)parameters[resultTextId].Value
                    };
                    return result;
                }

                result = new QueryResult
                {
                    DataSet = dS,
                    SPCode = (int)parameters[spResultCodeId].Value,
                    ReturnCode = (int)parameters[returnCodeId].Value,
                    Text = (string)parameters[resultTextId].Value
                };
                return result;
            }
            catch(Exception ex)
            {
                result = new QueryResult {Text = ex.Message, ReturnCode = ex.HResult};
                return result;
            }
        }
    }
}