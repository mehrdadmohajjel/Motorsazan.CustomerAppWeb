using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Motorsazan.CustomerAppWeb.Api.Utilities
{
    public static class Log
    {
        [Obsolete]
        static Log()
        {
            var projectNameParts = Assembly.GetCallingAssembly().GetName().Name.Split('.');
            var projectName = $"{projectNameParts[0]}.{projectNameParts[1]}";

            var logFilePath = $@"C:\Motorsazan\{projectName}\Logs\Api\Log.log";

            var elasticSearchUrl = ConfigurationManager.AppSettings["ElasticSearchUrl"];

            var elasticSinkConfigurations = new ElasticsearchSinkOptions(new Uri(elasticSearchUrl))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}",
                MinimumLogEventLevel = LogEventLevel.Error
            };

            Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .WriteTo.Debug()
                .WriteTo.File(logFilePath,
                    outputTemplate:
                    "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:WRN}] {MachineName} {EnvironmentUserName} {Message:lj}{NewLine}{Exception}")
                .WriteTo.Elasticsearch(elasticSinkConfigurations)
                .CreateLogger();
        }

        public static Logger Logger { get; }

        private static string Body(string message) => "[" + message + "]";

        private static string ExecutionTime(double time) => "[" + time + "]";

        private static string CurrentMethod(string sourceFilePath, string memberName, int sourceLineNumber)
        {
            sourceFilePath = sourceFilePath.Contains('\\')
                ? sourceFilePath.Substring(sourceFilePath.LastIndexOf('\\') + 1)
                : sourceFilePath;
            sourceFilePath = sourceFilePath.Contains('.')
                ? sourceFilePath.Substring(0, sourceFilePath.IndexOf('.'))
                : sourceFilePath;

            return "[" + sourceFilePath + "." + memberName + " line:" + sourceLineNumber + "]";
        }

        private static string Format(string message, double time, string sourceFilePath, string memberName,
            int sourceLineNumber)
        {
            var currentMethod = CurrentMethod(sourceFilePath, memberName, sourceLineNumber);
            var executionTime = ExecutionTime(time);
            var body = Body(message);

            return executionTime + ";" + currentMethod + ";" + body;
        }

        public static void Debug(string message, double time = 0, [CallerFilePath] string sourceFilePath = "",
            [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0) =>
            Logger.Debug(Format(message, time, sourceFilePath, memberName, sourceLineNumber));

        public static void Information(string message, double time, [CallerFilePath] string sourceFilePath = "",
            [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0) =>
            Logger.Information(Format(message, time, sourceFilePath, memberName, sourceLineNumber));

        public static void Warning(string message, double time, [CallerFilePath] string sourceFilePath = "",
            [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0) =>
            Logger.Warning(Format(message, time, sourceFilePath, memberName, sourceLineNumber));

        public static void Error(string message, double time, [CallerFilePath] string sourceFilePath = "",
            [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0) =>
            Logger.Error(Format(message, time, sourceFilePath, memberName, sourceLineNumber));

        public static void Fatal(string message, double time, [CallerFilePath] string sourceFilePath = "",
            [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0) =>
            Logger.Fatal(Format(message, time, sourceFilePath, memberName, sourceLineNumber));
    }
}