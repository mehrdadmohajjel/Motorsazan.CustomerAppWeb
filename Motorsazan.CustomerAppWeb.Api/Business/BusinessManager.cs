using Motorsazan.CustomerAppWeb.Api.Models.Base;
using Motorsazan.CustomerAppWeb.Api.Utilities;
using System;
using System.Diagnostics;

namespace Motorsazan.CustomerAppWeb.Api.Business
{
    public class BusinessManager
    {
        public TOutput CallStoredProcedure<TInput, TOutput>(string storedProcedureName, TInput input)
            where TInput : class, new() where TOutput : class
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var sqlParameters = input.ToSqlParameters();
                var serviceResult = DataBase.ExecuteStoredProcedure(storedProcedureName, sqlParameters);

                var isValid = QueryResultStatus.IsQueryResultValid(serviceResult);
                if(!isValid)
                {
                    throw new Exception(serviceResult.Text);
                }

                return serviceResult.ToOutput<TOutput>();
            }
            catch(Exception ex)
            {
                sw.Stop();
                Log.Fatal(ex.GetBaseException().ToString(), sw.Elapsed.TotalMilliseconds);
                throw new Exception(ex.GetBaseException().ToString());
            }
            finally
            {
                sw.Stop();
            }
        }

        public void CallStoredProcedure<TInput>(string storedProcedureName, TInput input)
            where TInput : class, new()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var sqlParameters = input.ToSqlParameters();
                var serviceResult = DataBase.ExecuteStoredProcedure(storedProcedureName, sqlParameters);

                var isValid = QueryResultStatus.IsQueryResultValid(serviceResult);
                if(!isValid)
                {
                    throw new Exception(serviceResult.Text);
                }
            }
            catch(Exception ex)
            {
                sw.Stop();
                Log.Fatal(ex.GetBaseException().ToString(), sw.Elapsed.TotalMilliseconds);
                throw new Exception(ex.GetBaseException().ToString());
            }
            finally
            {
                sw.Stop();
            }
        }

        public TOutput CallStoredProcedure<TOutput>(string storedProcedureName)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var serviceResult = DataBase.ExecuteStoredProcedure(storedProcedureName);

                var isValid = QueryResultStatus.IsQueryResultValid(serviceResult);
                if(!isValid)
                {
                    throw new Exception(serviceResult.Text);
                }

                return serviceResult.ToOutput<TOutput>();
            }
            catch(Exception ex)
            {
                sw.Stop();
                Log.Fatal(ex.GetBaseException().ToString(), sw.Elapsed.TotalMilliseconds);
                throw new Exception(ex.GetBaseException().ToString());
            }
            finally
            {
                sw.Stop();
            }
        }

        public bool CallStoredProcedure(string storedProcedureName)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var serviceResult = DataBase.ExecuteStoredProcedure(storedProcedureName);

                return QueryResultStatus.IsQueryResultValid(serviceResult);
            }
            catch(Exception ex)
            {
                sw.Stop();
                Log.Fatal(ex.GetBaseException().ToString(), sw.Elapsed.TotalMilliseconds);
                throw new Exception(ex.GetBaseException().ToString());
            }
            finally
            {
                sw.Stop();
            }
        }

        public string CallStoredProcedureAndReturnMessageIfExits<TInput>(string storedProcedureName, TInput input)
            where TInput : class, new()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var sqlParameters = input.ToSqlParameters();
                var serviceResult = DataBase.ExecuteStoredProcedure(storedProcedureName, sqlParameters);

                var isValid = QueryResultStatus.IsQueryResultValid(serviceResult);

                return !isValid ? serviceResult.Text : "";
            }
            catch(Exception ex)
            {
                sw.Stop();
                Log.Fatal(ex.GetBaseException().ToString(), sw.Elapsed.TotalMilliseconds);
                throw new Exception(ex.GetBaseException().ToString());
            }
            finally
            {
                sw.Stop();
            }
        }

        public (string errorMessage, TOutput output) CallStoredProcedureAndReturnMessageIfExits<TInput, TOutput>(
            string storedProcedureName, TInput input)
            where TInput : class, new()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var sqlParameters = input.ToSqlParameters();
                var serviceResult = DataBase.ExecuteStoredProcedure(storedProcedureName, sqlParameters);

                var isValid = QueryResultStatus.IsQueryResultValid(serviceResult);

                if(!isValid)
                {
                    return (serviceResult.Text, default);
                }

                return (null, serviceResult.ToOutput<TOutput>());
            }
            catch(Exception ex)
            {
                sw.Stop();
                Log.Fatal(ex.GetBaseException().ToString(), sw.Elapsed.TotalMilliseconds);
                throw new Exception(ex.GetBaseException().ToString());
            }
            finally
            {
                sw.Stop();
            }
        }
    }
}