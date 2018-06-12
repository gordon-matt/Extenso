using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc
{
    /// <summary>
    /// Useful extension methods for collections
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Creates a <see cref="SelectList"/> from the given collection
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns>An instance of <see cref="SelectList"/></returns>
        public static SelectList ToSelectList(this IEnumerable<string> enumerable)
        {
            return enumerable.ToSelectList(x => x, x => x);
        }

        /// <summary>
        /// Creates a <see cref="SelectList"/> from the given collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="valueFieldSelector"></param>
        /// <param name="textFieldSelector"></param>
        /// <returns>An instance of <see cref="SelectList"/></returns>
        public static SelectList ToSelectList<T>(this IEnumerable<T> enumerable, Func<T, object> valueFieldSelector, Func<T, string> textFieldSelector)
        {
            var values = from T item in enumerable
                         select new
                         {
                             ValueField = Convert.ToString(valueFieldSelector(item)),
                             TextField = textFieldSelector(item)
                         };
            return new SelectList(values, "ValueField", "TextField");
        }

        /// <summary>
        /// Creates a <see cref="SelectList"/> from the given collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="valueFieldSelector"></param>
        /// <param name="textFieldSelector"></param>
        /// <param name="emptyText"></param>
        /// <returns>An instance of <see cref="SelectList"/></returns>
        public static SelectList ToSelectList<T>(this IEnumerable<T> enumerable, Func<T, object> valueFieldSelector, Func<T, string> textFieldSelector, string emptyText)
        {
            var values = (from T item in enumerable
                          select new
                          {
                              ValueField = Convert.ToString(valueFieldSelector(item)),
                              TextField = textFieldSelector(item)
                          }).ToList();

            if (emptyText != null) // we don't check for empty, because empty string can be valid for emptyText value.
            {
                values.Insert(0, new { ValueField = string.Empty, TextField = emptyText });
            }

            return new SelectList(values, "ValueField", "TextField");
        }

        /// <summary>
        /// Creates a <see cref="SelectList"/> from the given collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="valueFieldSelector"></param>
        /// <param name="textFieldSelector"></param>
        /// <param name="selectedValue"></param>
        /// <returns>An instance of <see cref="SelectList"/></returns>
        public static SelectList ToSelectList<T>(this IEnumerable<T> enumerable, Func<T, object> valueFieldSelector, Func<T, string> textFieldSelector, object selectedValue)
        {
            var values = from T item in enumerable
                         select new
                         {
                             ValueField = Convert.ToString(valueFieldSelector(item)),
                             TextField = textFieldSelector(item)
                         };
            return new SelectList(values, "ValueField", "TextField", selectedValue);
        }

        /// <summary>
        /// Creates a <see cref="SelectList"/> from the given collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="valueFieldSelector"></param>
        /// <param name="textFieldSelector"></param>
        /// <param name="selectedValue"></param>
        /// <param name="emptyText"></param>
        /// <returns>An instance of <see cref="SelectList"/></returns>
        public static SelectList ToSelectList<T>(this IEnumerable<T> enumerable, Func<T, object> valueFieldSelector, Func<T, string> textFieldSelector, object selectedValue, string emptyText)
        {
            var values = (from T item in enumerable
                          select new
                          {
                              ValueField = Convert.ToString(valueFieldSelector(item)),
                              TextField = textFieldSelector(item)
                          }).ToList();

            if (emptyText != null) // we don't check for empty, because empty string can be valid for emptyText value.
            {
                values.Insert(0, new { ValueField = string.Empty, TextField = emptyText });
            }
            return new SelectList(values, "ValueField", "TextField", selectedValue);
        }

        /// <summary>
        /// Creates a <see cref="MultiSelectList"/> from the given collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="valueFieldSelector"></param>
        /// <param name="textFieldSelector"></param>
        /// <param name="selectedValues"></param>
        /// <param name="emptyText"></param>
        /// <returns>An instance of <see cref="MultiSelectList"/></returns>
        public static MultiSelectList ToMultiSelectList<T, TValue>(this IEnumerable<T> enumerable, Func<T, object> valueFieldSelector, Func<T, string> textFieldSelector, IEnumerable<TValue> selectedValues, string emptyText = null)
        {
            var values = (from T item in enumerable
                          select new
                          {
                              ValueField = Convert.ToString(valueFieldSelector(item)),
                              TextField = textFieldSelector(item)
                          }).ToList();

            if (emptyText != null) // we don't check for empty, because empty string can be valid for emptyText value.
            {
                values.Insert(0, new { ValueField = string.Empty, TextField = emptyText });
            }

            return new MultiSelectList(values, "ValueField", "TextField", selectedValues);
        }
    }
}