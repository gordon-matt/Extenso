using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Radzen;
using Radzen.Blazor;

namespace Demo.Extenso.AspNetCore.Blazor.OData.Extensions
{
    public static class FilterDescriptorExtensions
    {
        internal static readonly IDictionary<FilterOperator, string> ODataFilterOperators = new Dictionary<FilterOperator, string>
        {
            {FilterOperator.Equals, "eq"},
            {FilterOperator.NotEquals, "ne"},
            {FilterOperator.LessThan, "lt"},
            {FilterOperator.LessThanOrEquals, "le"},
            {FilterOperator.GreaterThan, "gt"},
            {FilterOperator.GreaterThanOrEquals, "ge"},
            {FilterOperator.StartsWith, "startswith"},
            {FilterOperator.EndsWith, "endswith"},
            {FilterOperator.Contains, "contains"},
            {FilterOperator.DoesNotContain, "DoesNotContain"}
        };

        private static string GetColumnODataFilter<T>(RadzenDataGrid<T> dataGrid, FilterDescriptor column, bool second = false)
        {
            string property = column.Property.Replace('.', '/');

            var columnFilterOperator = !second ? column.FilterOperator : column.SecondFilterOperator;

            string value = !second ? (string)Convert.ChangeType(column.FilterValue, typeof(string)) :
                (string)Convert.ChangeType(column.SecondFilterValue, typeof(string));

            var filterPropertyType = column.Property.GetType();

            if (dataGrid.FilterCaseSensitivity == FilterCaseSensitivity.CaseInsensitive && filterPropertyType == typeof(string))
            {
                property = $"tolower({property})";
            }

            if (filterPropertyType == typeof(string))
            {
                if (!string.IsNullOrEmpty(value) && columnFilterOperator == FilterOperator.Contains)
                {
                    return dataGrid.FilterCaseSensitivity == FilterCaseSensitivity.CaseInsensitive ?
                        $"contains({property}, tolower('{value}'))" :
                        $"contains({property}, '{value}')";
                }
                else if (!string.IsNullOrEmpty(value) && columnFilterOperator == FilterOperator.DoesNotContain)
                {
                    return dataGrid.FilterCaseSensitivity == FilterCaseSensitivity.CaseInsensitive ?
                        $"not(contains({property}, tolower('{value}')))" :
                        $"not(contains({property}, '{value}'))";
                }
                else if (!string.IsNullOrEmpty(value) && columnFilterOperator == FilterOperator.StartsWith)
                {
                    return dataGrid.FilterCaseSensitivity == FilterCaseSensitivity.CaseInsensitive ?
                        $"startswith({property}, tolower('{value}'))" :
                        $"startswith({property}, '{value}')";
                }
                else if (!string.IsNullOrEmpty(value) && columnFilterOperator == FilterOperator.EndsWith)
                {
                    return dataGrid.FilterCaseSensitivity == FilterCaseSensitivity.CaseInsensitive ?
                        $"endswith({property}, tolower('{value}'))" :
                        $"endswith({property}, '{value}')";
                }
                else if (!string.IsNullOrEmpty(value) && columnFilterOperator == FilterOperator.Equals)
                {
                    return dataGrid.FilterCaseSensitivity == FilterCaseSensitivity.CaseInsensitive ?
                        $"{property} eq tolower('{value}')" :
                        $"{property} eq '{value}'";
                }
                else if (!string.IsNullOrEmpty(value) && columnFilterOperator == FilterOperator.NotEquals)
                {
                    return dataGrid.FilterCaseSensitivity == FilterCaseSensitivity.CaseInsensitive ?
                        $"{property} ne tolower('{value}')" :
                        $"{property} ne '{value}'";
                }
            }
            else if (typeof(IEnumerable).IsAssignableFrom(column.Property.GetType()) && filterPropertyType != typeof(string))
            {
            }
            else if (PropertyAccess.IsNumeric(filterPropertyType))
            {
                return $"{property} {ODataFilterOperators[columnFilterOperator]} {value}";
            }
            else if (filterPropertyType == typeof(bool) || filterPropertyType == typeof(bool?))
            {
                return $"{property} eq {value.ToLower()}";
            }
            else if (filterPropertyType == typeof(DateTime) ||
                    filterPropertyType == typeof(DateTime?) ||
                    filterPropertyType == typeof(DateTimeOffset) ||
                    filterPropertyType == typeof(DateTimeOffset?))
            {
                return $"{property} {ODataFilterOperators[columnFilterOperator]} {DateTime.Parse(value, null, System.Globalization.DateTimeStyles.RoundtripKind):yyyy-MM-ddTHH:mm:ss.fffZ}";
            }
            else if (filterPropertyType == typeof(Guid) || filterPropertyType == typeof(Guid?))
            {
                return $"{property} {ODataFilterOperators[columnFilterOperator]} {value}";
            }

            return string.Empty;
        }

        public static string ToODataFilterString<T>(this IEnumerable<FilterDescriptor> columns, RadzenDataGrid<T> dataGrid)
        {
            static bool canFilter(FilterDescriptor c) => c.Property != null &&
                !(c.FilterValue == null || c.FilterValue as string == string.Empty) && c.Property != null;

            if (columns.Where(canFilter).Any())
            {
                var gridLogicalFilterOperator = columns.FirstOrDefault()?.LogicalFilterOperator;
                string gridBooleanOperator = gridLogicalFilterOperator == LogicalFilterOperator.And ? "and" : "or";

                var whereList = new List<string>();
                foreach (var column in columns.Where(canFilter))
                {
                    string property = column.Property.Replace('.', '/');

                    string value = (string)Convert.ChangeType(column.FilterValue, typeof(string));
                    string secondValue = (string)Convert.ChangeType(column.SecondFilterValue, typeof(string));

                    if (!string.IsNullOrEmpty(value))
                    {
                        string linqOperator = ODataFilterOperators[column.FilterOperator];
                        if (linqOperator == null)
                        {
                            linqOperator = "==";
                        }

                        string booleanOperator = column.LogicalFilterOperator == LogicalFilterOperator.And ? "and" : "or";

                        if (string.IsNullOrEmpty(secondValue))
                        {
                            whereList.Add(GetColumnODataFilter(dataGrid, column));
                        }
                        else
                        {
                            whereList.Add($"({GetColumnODataFilter(dataGrid, column)} {booleanOperator} {GetColumnODataFilter(dataGrid, column, true)})");
                        }
                    }
                }

                return string.Join($" {gridBooleanOperator} ", whereList.Where(i => !string.IsNullOrEmpty(i)));
            }

            return "";
        }
    }
}