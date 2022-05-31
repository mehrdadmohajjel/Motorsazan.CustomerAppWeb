namespace Motorsazan.CustomerAppWeb.Api.Models.Base
{
    public static class QueryResultStatus
    {
        public static int StatusSuccess
        {
            get => 1;
        }

        public static int ReturnSuccess
        {
            get => 1;
        }

        public static bool IsQueryResultValid(QueryResult queryResult)
        {
            var isResultValid = queryResult.ReturnCode == ReturnSuccess &&
                                queryResult.SPCode == StatusSuccess;
            return isResultValid;
        }
    }
}