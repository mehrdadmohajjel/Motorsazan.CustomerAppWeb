using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Xml.Serialization;
using DevExpress.Web.Internal;
using Motorsazan.CustomerAppWeb.Shared.Enums;
using Newtonsoft.Json;

namespace Motorsazan.CustomerAppWeb.Shared.Utilities
{
    public static class Tools
    {
        public static string ConvertArabicCharacterToPersian(string word)
        {
            return word.Replace("ی", "ي")
                .Replace("ک", "ك");
        }

        public static string ConvertDataTableToJson(DataTable table)
        {
            return JsonConvert.SerializeObject(table);
        }

        public static string ConvertToEnglishNumber(string number)
        {
            var arabic = new string[10] {"٠", "١", "٢", "٣", "٤", "٥", "٦", "٧", "٨", "٩"};
            var persian = new string[10] {"۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹"};

            const int loopLength = 10;
            for (var i = 0; i < loopLength; i++)
            {
                var iAsNum = i.ToString();
                number = number.Replace(persian[i], iAsNum)
                    .Replace(arabic[i], iAsNum);
            }

            return number;
        }

        public static DateTime ConvertToLatinDate(string shamsiDate, string format = "yyyy-MM-dd", char splitter = '/')
        {
            if (format is null) throw new ArgumentNullException(nameof(format));

            var pc = new PersianCalendar();
            var shamsiList = shamsiDate.Split(splitter);
            var year = Convert.ToInt32(shamsiList[0]);
            var month = Convert.ToInt32(shamsiList[1]);
            var day = Convert.ToInt32(shamsiList[2]);

            return new DateTime(year, month, day, pc);
        }

        public static DateTime ConvertToLatinDateTime(string shamsiDate, string time)
        {
            var date = ConvertToLatinDate(shamsiDate);
            var splitTime = time.Split(':');
            var hour = Convert.ToInt32(splitTime[0]);
            var minute = Convert.ToInt32(splitTime[1]);
            date += new TimeSpan(hour, minute, 0);
            return date;
        }

        public static string GetCurrentUserAccName()
        {
            var currentUser = HttpContext.Current.User.Identity.Name;
            if (currentUser == "") currentUser = Environment.UserName;
            var slash = currentUser.LastIndexOf("\\", StringComparison.Ordinal);
            currentUser = currentUser.Substring(slash + 1);
            return currentUser;
        }

        public static string GetIpAddress()
        {
            var ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            ip = string.IsNullOrEmpty(ip)
                ? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]
                : ip.Split(',')[0];

            return ip;
        }

        public static List<dynamic> GetEnumList<T>()
        {
            var typeOfDescriptionAttribute = typeof(DescriptionAttribute);

            return Enum.GetValues(typeof(T))
                .Cast<Enum>()
                .Select(value => new
                {
                    (Attribute.GetCustomAttribute(value.GetType()
                                .GetField(value.ToString()),
                            typeOfDescriptionAttribute)
                        as DescriptionAttribute)?.Description,
                    value
                })
                .OrderBy(item => item.value)
                .ToList<dynamic>();
        }

        public static string GetGenericName<T>()
        {
            var genericType = typeof(T);
            string name;
            if (genericType.IsGenericType)
            {
                var genericName = genericType.GetGenericTypeDefinition();
                name = genericName.Name.Substring(0, genericType.Name.IndexOf('`'));
            }
            else
            {
                name = genericType.Name;
            }

            return name;
        }

        public static object GetGenericObjectSpecialFieldValue<T>(T genericItem, string fieldName)
        {
            if (genericItem == null) return null;

            var type = typeof(T);
            var properties = type.GetProperties();

            foreach (var propertyInfo in properties)
            {
                var namesEqual = propertyInfo.Name.ToLower().Trim() == fieldName.ToLower().Trim();
                if (!namesEqual) continue;

                return propertyInfo.GetValue(genericItem);
            }

            return null;
        }

        public static T GetJsonRequest<T>(string requestUrl)
        {
            try
            {
                var apiRequest = WebRequest.Create(requestUrl);
                var apiResponse = (HttpWebResponse) apiRequest.GetResponse();

                var isResponseValid = apiResponse.StatusCode == HttpStatusCode.OK;
                if (!isResponseValid) return default;

                var jsonOutput = "";
                using (var streamReader = new StreamReader(apiResponse.GetResponseStream()))
                {
                    jsonOutput = streamReader.ReadToEnd();
                }

                var jsResult = JsonConvert.DeserializeObject<T>(jsonOutput);
                return jsResult != null ? jsResult : default;
            }
            catch (Exception)
            {
                return default;
            }
        }

        public static string GetShamsiYear(DateTime date)
        {
            return new PersianCalendar().GetYear(date).ToString();
        }

        public static string GetStoredProcedureFarsiErrorMessage(string errorMessage)
        {
            var outputMessage = "خطای سیستمی : لطفا با واحد فناوری اطلاعات تماس بگیرید";
            if (errorMessage.ToLower().Contains("duplicate")) outputMessage = "عنوان انتخابی قبلا ثبت شده است";
            return outputMessage;
        }

        public static T GetXmlRequest<T>(string requestUrl)
        {
            try
            {
                var apiRequest = WebRequest.Create(requestUrl);
                var apiResponse = (HttpWebResponse) apiRequest.GetResponse();

                var isResponseValid = apiResponse.StatusCode == HttpStatusCode.OK;
                if (!isResponseValid) return default;

                var xmlOutput = "";
                using (var streamReader = new StreamReader(apiResponse.GetResponseStream()))
                {
                    xmlOutput = streamReader.ReadToEnd();
                }

                var xmlSerialize = new XmlSerializer(typeof(T));
                var xmlResult = (T) xmlSerialize.Deserialize(new StringReader(xmlOutput));

                return xmlResult != null ? xmlResult : default;
            }
            catch (Exception)
            {
                return default;
            }
        }

        public static bool HasColumn(this IDataRecord dr, string columnName)
        {
            const StringComparison ignoreCase = StringComparison.InvariantCultureIgnoreCase;
            for (var i = 0; i < dr.FieldCount; i++)
            {
                var isColumnNameEqual = dr.GetName(i).Equals(columnName, ignoreCase);
                if (isColumnNameEqual) return true;
            }

            return false;
        }

        public static T Map<T>(IDataRecord record)
        {
            var dataTransferObject = Activator.CreateInstance<T>();
            foreach (var property in typeof(T).GetProperties())
            {
                var hasColumn = record.HasColumn(property.Name);
                var value = record.GetOrdinal(property.Name);
                var hasValue = !record.IsDBNull(value);

                if (hasColumn && hasValue) property.SetValue(dataTransferObject, record[property.Name]);
            }

            return dataTransferObject;
        }

        public static (DateTime startDate, DateTime endDate) NormalizeDates(string startDate, string endDate,
            DatePeriodType datePeriodType)
        {
            switch (datePeriodType)
            {
                case DatePeriodType.RecentWeek:
                    return (DateTime.Now.AddDays(-7), DateTime.Now);
                case DatePeriodType.RecentMonth:
                    return (DateTime.Now.AddMonths(-1), DateTime.Now);
                case DatePeriodType.RecentThreeMonths:
                    return (DateTime.Now.AddMonths(-3), DateTime.Now);
                case DatePeriodType.RecentSixMonths:
                    return (DateTime.Now.AddMonths(-6), DateTime.Now);
                case DatePeriodType.CurrentDay:
                    return (DateTime.Now, DateTime.Now);
                case DatePeriodType.Yesterday:
                    return (DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-1));
                case DatePeriodType.ThisYear:
                {
                    var pc = new PersianCalendar();
                    var fromYearFirstDay = ConvertToLatinDate($"{pc.GetYear(DateTime.Now)}/01/01");
                    return (fromYearFirstDay, DateTime.Now);
                }
                case DatePeriodType.CustomPeriod:
                {
                    var persianCalendar = new PersianCalendar();

                    var splitStartDate = startDate.Split('/')
                        .Select(x => Convert.ToInt32(x))
                        .ToList();
                    var splitEndDate = endDate.Split('/')
                        .Select(x => Convert.ToInt32(x))
                        .ToList();

                    var startYear = splitStartDate[0];
                    var startMonth = splitStartDate[1];
                    var startDay = splitStartDate[2];

                    var endYear = splitEndDate[0];
                    var endMonth = splitEndDate[1];
                    var endDay = splitEndDate[2];


                    var normalizedStartDate = new DateTime(startYear, startMonth, startDay, persianCalendar);

                    var normalizedEndDate = new DateTime(endYear, endMonth, endDay, persianCalendar);

                    return (normalizedStartDate, normalizedEndDate);
                }
                default:
                    throw new Exception("Provided 'DatePeriodType' for 'NormalizeDates' function is invalid.");
            }
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> data)
        {
            var properties = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                _ = table.Columns.Add(prop.Name, type);
            }

            foreach (var item in data)
            {
                var row = table.NewRow();
                foreach (PropertyDescriptor prop in properties) row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }

            return table;
        }

        public static DataTable ToDataTable<T>(this T item)
        {
            var type = typeof(T);
            return ToDataTable(item, type);
        }

        public static DataTable ToDataTable<T>(this T item, Type type)
        {
            var properties = TypeDescriptor.GetProperties(type);
            var table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                var propPropertyType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                _ = table.Columns.Add(prop.Name, propPropertyType);
            }

            var row = table.NewRow();
            foreach (PropertyDescriptor prop in properties) row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
            table.Rows.Add(row);

            return table;
        }

        public static DataTable ToDataTableWithEnumerableType<T>(this T data, Type type)
        {
            var isArray = type.IsArray;
            var isList = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);

            if (isList || isArray)
                type = isArray
                    ? type.GetElementType()
                    : type.GetGenericArguments()[0];

            var properties = TypeDescriptor.GetProperties(type);
            var table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                var propertyType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                _ = table.Columns.Add(prop.Name, propertyType);
            }

            if (data == null) return table;

            foreach (var item in (IEnumerable) data)
            {
                var row = table.NewRow();
                foreach (PropertyDescriptor prop in properties) row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }

            return table;
        }

        public static string ToShamsiDateString(this DateTime value, string separator = "/")
        {
            var pc = new PersianCalendar();
            var year = pc.GetYear(value);
            var month = pc.GetMonth(value).ToString("00");
            var day = pc.GetDayOfMonth(value).ToString("00");

            return year + separator + month + separator + day;
        }

        public static void RemovedCallback(string key, object value, CacheItemRemovedReason reason)
        {
            var fileName = value.ToString();
            var fileExist = File.Exists(fileName);
            if (fileExist) File.Delete(fileName);
        }

        public static void RemoveFileWithDelay(string key, string fullPath, int delay)
        {
            if (HttpUtils.GetCache()[key] != null) return;

            var timeSpan = new TimeSpan(0, delay, 0);
            var absoluteExpiration = DateTime.Now.Add(timeSpan);
            var cacheItem = new CacheItemRemovedCallback(RemovedCallback);

            HttpUtils.GetCache().Insert(key, fullPath, null, absoluteExpiration,
                Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, cacheItem);
        }

        public static T[] PrependGetAllItemToArray<T>(T[] arrayToAppend, T getAllItem)
        {
            var newArray = new T[arrayToAppend.Length + 1];
            newArray[0] = getAllItem;
            Array.Copy(arrayToAppend, 0, newArray, 1, arrayToAppend.Length);
            return newArray;
        }
    }
}