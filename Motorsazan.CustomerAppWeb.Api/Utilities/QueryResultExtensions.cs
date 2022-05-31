using Motorsazan.CustomerAppWeb.Api.Models.Base;
using Motorsazan.CustomerAppWeb.Shared.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Motorsazan.CustomerAppWeb.Api.Utilities
{
    /// <summary>
    ///     Contains extension methods of QueryResult type
    /// </summary>
    public static class QueryResultExtensions
    {
        private static object ConvertValueToType(object data, Type type)
        {
            if(type.IsEnum)
            {
                return Enum.Parse(type.UnderlyingSystemType, data.ToString());
            }

            if(DBNull.Value.Equals(data) || data == null)
            {
                return type.IsValueType
                    ? Activator.CreateInstance(type)
                    : null;
            }

            var isNullable = type.IsGenericType &&
                             type.GetGenericTypeDefinition() ==
                             typeof(Nullable<>);

            var typeToConvert = isNullable
                ? type.GetGenericArguments()[0]
                : type;

            var isByteArray = data.GetType() == typeof(byte[]);

            //if data is a byte[] and typeToConvert is string type so its a file stream
            //and needs to bo converted to base 64 string
            var isDataAFileStream = isByteArray && typeToConvert == typeof(string);
            if(isDataAFileStream)
            {
                data = Convert.ToBase64String((byte[])data);
            }

            return Convert.ChangeType(
                !(data is IConvertible)
                    ? data.ToString()
                    : data, typeToConvert);
        }

        // ------------- 
        // TOutput can be one of below cases:
        //
        // 1: TOutput is a class with all none-list and none-array and none-class type properties:
        // Ex:
        //
        //      Class A 
        //      {
        //          public int Number { get; set; }
        //
        //          public string Text { get; set; }
        //
        //          public DateTime Date { get; set; }
        //      }
        //
        // ------------- 
        // 2: TOutput is a class with properties of none-class type (string and arrays lists are valid)
        // Ex:
        //
        //      Class B 
        //      {
        //          public int[] Numbers { get; set; }
        //
        //          public List<int> Ages { get; set; }
        //
        //          public string Text { get; set; }
        //      }
        //
        // ------------- 
        // 3: TOutput is a class with properties which are all class types (value types and arrays and lists are not valid)
        //    Note that class types of properties are also one of this cases too.
        // Ex:
        //
        //      Class C 
        //      {
        //          public A AClassType { get; set: }
        //
        //          public A AnotherClassType { get; set: }
        //      }
        //
        // ------------- 
        // 4: TOutput is a class with properties which are all arrays of class types (value types and none-class-type-arrays or none-class-type-lists are not valid)
        //    Note that class types of properties are also one of this cases too.
        // Ex:
        //
        //      Class D 
        //      {
        //          public A[] AClassTypeArray { get; set: }
        //
        //          public List<A> AClassTypeList { get; set: }
        //      }
        //
        // ------------- 
        // 4: TOutput is a class with properties which can be anything (value types, value-type-arrays, value-type-lists,
        //                                                              classTypes, class-type-arrays and class-type-lists are all valid)
        //    Note that class types of properties are also one of this cases too.
        // Ex:
        //
        //      Class E 
        //      {
        //          public A AClassType { get; set: }
        //
        //          public int Numbers { get; set: }
        //
        //          public string Text { get; set: }
        //
        //          public DateTime Date { get; set: }
        //
        //          public A[] AClassTypeArray { get; set: }
        //
        //          public List<A> AClassTypeList { get; set: }
        //      }
        //
        // ------------- 
        // 5: TOutput is a class-type-array or a class-type-list with properties ONLY none-array nor none-list type properties
        //    (value types, class-type are all valid)
        //
        //    TOutput is F[] or List<F>
        //
        //    Note that class types of properties are also one of this cases too.
        // Ex:      
        //
        //
        //      Class F 
        //      {
        //          public A AClassType { get; set: }
        //
        //          public int Numbers { get; set: }
        //
        //          public string Text { get; set: }
        //
        //          public DateTime Date { get; set: }
        //      }

        /// <summary>
        ///     Extension method that converts QueryResult to a TOutput
        /// </summary>
        /// <typeparam name="TOutput">Target class of conversion</typeparam>
        /// <param name="queryResult">Source class of conversion</param>
        /// <returns></returns>
        public static TOutput ToOutput<TOutput>(this QueryResult queryResult)
        {
            var tableIndex = 0;
            var columnIndex = 0;

            var tables = queryResult.DataSet.Tables;

            var typeOfOutput = typeof(TOutput);

            var isArray = typeOfOutput.IsArray;
            var isList = typeOfOutput.IsGenericType && typeOfOutput.GetGenericTypeDefinition() == typeof(List<>);

            if(isList || isArray)
            {
                var elementType = isArray
                    ? typeOfOutput.GetElementType()
                    : typeOfOutput.GetGenericArguments()[0];

                var elementTypeProperties = elementType.GetProperties();
                var isAnyListOrArrayProperty = elementTypeProperties.Any(p =>
                    !p.GetCustomAttributes(false)
                        .Any(attribute => attribute is IgnoreInStoredProcedureOutputAttribute) &&
                    (p.PropertyType.IsArray || (p.PropertyType.IsGenericType &&
                                                p.PropertyType.GetGenericTypeDefinition() == typeof(List<>))));


                if(isAnyListOrArrayProperty)
                {
                    throw new InvalidOperationException(
                        $"When TOutput is an array or a list it's all properties most be none-array and none-list types, otherwise this extension method ({nameof(ToOutput)}) can not be used");
                }


                if(!elementType.IsClass || elementType == typeof(string))
                {
                    var valueTypeList = FillIterableValueType(elementType, tables, tableIndex);


                    if(isList)
                    {
                        return (TOutput)valueTypeList;
                    }

                    var valueTypeArray = Array.CreateInstance(elementType, valueTypeList.Count);
                    valueTypeList.CopyTo(valueTypeArray, 0);

                    return (TOutput)Convert.ChangeType(valueTypeArray, typeOfOutput);
                }


                var filledToList = FillIterableClassType(elementType, tables, tableIndex, isList);

                if(!isArray)
                {
                    return (TOutput)Convert.ChangeType(filledToList, typeOfOutput);
                }

                var array = Array.CreateInstance(elementType, filledToList.Count);
                filledToList.CopyTo(array, 0);

                return (TOutput)Convert.ChangeType(array, typeOfOutput);
            }


            if(typeOfOutput.IsValueType || typeOfOutput == typeof(string))
            {
                if(tables.Count < 1 || tables[tableIndex].Rows.Count < 1)
                {
                    return (TOutput)ConvertValueToType(null, typeOfOutput);
                }

                var data = tables[0].Rows[0].ItemArray[0];

                return (TOutput)ConvertValueToType(data, typeOfOutput);
            }

            var instance = Activator.CreateInstance(typeOfOutput);
            var properties = typeOfOutput.GetProperties();

            // -------------- NOTE --------------
            // Below block of codes checks if there is no data in any rows of all tables
            // returns null not an instance of class with default values set to properties

            // ==================================
            // ---------- Block Begins ----------
            // ==================================

            var isAnyOfPropertiesArrayOrListType = properties.Any(p =>
                p.PropertyType.IsArray ||
                (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(List<>)));

            var hasAnyRecordReturnedFromDatabase = false;
            if(!isAnyOfPropertiesArrayOrListType)
            {
                for(var i = 0; i < tables.Count; i++)
                {
                    var t = tables[i];

                    for(var j = 0; j < t.Rows.Count; j++)
                    {
                        var r = t.Rows[j];

                        if(r.ItemArray.Length <= 0)
                        {
                            continue;
                        }

                        hasAnyRecordReturnedFromDatabase = true;
                        break;
                    }

                    if(hasAnyRecordReturnedFromDatabase)
                    {
                        break;
                    }
                }

                if(!hasAnyRecordReturnedFromDatabase)
                {
                    return default;
                }
            }

            // ==================================
            // ----------- Block End ------------
            // ==================================

            foreach(var property in properties)
            {
                if(tableIndex >= tables.Count)
                {
                    break;
                }

                var propertyType = property.PropertyType;

                var isPropertyArrayType = propertyType.IsArray;
                var isPropertyListType = propertyType.IsGenericType &&
                                         propertyType.GetGenericTypeDefinition() == typeof(List<>);

                // Property is an array or a list type
                if(!isPropertyArrayType && !isPropertyListType)
                {
                    if(tables[tableIndex].Rows.Count < 1)
                    {
                        continue;
                    }

                    var itemArray = tables[tableIndex].Rows[0].ItemArray;

                    // Property is class-value-type-array or class-value-type-list
                    if(propertyType.IsClass && propertyType != typeof(string))
                    {
                        FillClassTypeProperty(instance, property, itemArray, ref tableIndex);
                    }
                    else
                    {
                        if(columnIndex >= itemArray.Length)
                        {
                            break;
                        }

                        var data = itemArray[columnIndex];

                        var value = ConvertValueToType(data, propertyType);

                        property.SetValue(instance, value);

                        columnIndex++;
                    }
                }
                else
                {
                    var elementType = isPropertyArrayType
                        ? propertyType.GetElementType()
                        : propertyType.GetGenericArguments()[0];

                    if(elementType.IsClass && elementType != typeof(string))
                    {
                        if(columnIndex > 0)
                        {
                            tableIndex++;
                        }

                        var filedClassTypeArrayOrList =
                            FillIterableClassType(elementType, tables, tableIndex, isPropertyListType);

                        property.SetValue(instance, filedClassTypeArrayOrList);
                    }
                    else
                    {
                        var list = FillIterableValueType(elementType, tables, tableIndex);


                        if(!isPropertyListType)
                        {
                            var array = Array.CreateInstance(elementType, list.Count);
                            list.CopyTo(array, 0);

                            property.SetValue(instance, array);
                        }

                        property.SetValue(instance, list);

                        columnIndex = 0;
                    }

                    tableIndex++;
                }
            }

            return (TOutput)instance;
        }

        private static void FillClassTypeProperty(object instance, PropertyInfo property,
            IReadOnlyList<object> itemArray,
            ref int tableIndex)
        {
            var classPropertyType = property.PropertyType;
            var classPropertyTypeProperties = classPropertyType.GetProperties();

            var classPropertyTypeInstance = Activator.CreateInstance(classPropertyType);

            var columnIndex = 0;

            foreach(var classPropertyTypeProperty in classPropertyTypeProperties)
            {
                if(columnIndex >= itemArray.Count)
                {
                    break;
                }

                var classPropertyTypePropertyType = classPropertyTypeProperty.PropertyType;
                var data = itemArray[columnIndex];

                var value = ConvertValueToType(data, classPropertyTypePropertyType);

                classPropertyTypeProperty.SetValue(classPropertyTypeInstance, value);

                columnIndex++;
            }

            tableIndex++;

            property.SetValue(instance, classPropertyTypeInstance);
        }

        private static IList FillIterableClassType(Type elementType, DataTableCollection tables, int tableIndex,
            bool isPropertyListType)
        {
            var classPropertyTypeProperties = elementType.GetProperties();

            var constructedListType = typeof(List<>).MakeGenericType(elementType);

            var list = (IList)Activator.CreateInstance(constructedListType);

            for(var index = 0; index < tables[tableIndex].Rows.Count; index++)
            {
                var elementTypeInstance = Activator.CreateInstance(elementType);

                var items = tables[tableIndex].Rows[index].ItemArray;

                var columnIndex = 0;

                foreach(var classPropertyTypeProperty in classPropertyTypeProperties)
                {
                    var classPropertyTypePropertyType = classPropertyTypeProperty.PropertyType;

                    if(classPropertyTypeProperty.GetCustomAttributes(false)
                       .Any(attr => attr is IgnoreInStoredProcedureOutputAttribute))
                    {
                        continue;
                    }

                    if(classPropertyTypePropertyType.GetCustomAttributes(false)
                       .Any(attr => attr is IgnoreInStoredProcedureOutputAttribute))
                    {
                        continue;
                    }

                    if(columnIndex >= items.Length)
                    {
                        break;
                    }

                    var data = items[columnIndex];

                    var value = ConvertValueToType(data, classPropertyTypePropertyType);

                    classPropertyTypeProperty.SetValue(elementTypeInstance, value);

                    columnIndex++;
                }

                _ = list.Add(elementTypeInstance);
            }

            if(isPropertyListType)
            {
                return list;
            }

            var array = Array.CreateInstance(elementType, list.Count);
            list.CopyTo(array, 0);

            return array;
        }

        private static IList FillIterableValueType(Type elementType, DataTableCollection tables, int tableIndex)
        {
            var constructedListType = typeof(List<>).MakeGenericType(elementType);

            var list = (IList)Activator.CreateInstance(constructedListType);

            for(var i = 0; i < tables[tableIndex].Rows.Count; i++)
            {
                var data = tables[tableIndex].Rows[i].ItemArray[0];

                var value = ConvertValueToType(data, elementType);

                _ = list.Add(value);
            }

            return list;
        }
    }
}