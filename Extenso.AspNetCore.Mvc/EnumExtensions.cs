using System.Collections;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc;

public static class EnumExtensions
{
    extension(Type type)
    {
        public SelectList ToSelectList(object selectedValue = null, string emptyText = null, bool nameIsId = false)
        {
            if (!type.GetTypeInfo().IsEnum)
            {
                throw new NotSupportedException("The type must be is enum type.");
            }

            var array = Enum.GetValues(type);

            var values = (from object e in array
                          select new
                          {
                              Id = nameIsId ? e.ToString() : e.ConvertTo<int>().ToString(),
                              Name = Extenso.EnumExtensions.GetDisplayName(e, out int order),
                              Order = order
                          }).ToList();

            values = values
                .Where(x => x.Order != -1)
                .OrderBy(x => x.Order)
                .ToList();

            if (emptyText != null)
            {
                values.Insert(0, new
                {
                    Id = nameIsId ? string.Empty : "-1",
                    Name = emptyText,
                    Order = -1
                });
            }

            return new SelectList(values, "Id", "Name", selectedValue);
        }

        public MultiSelectList ToMultiSelectList(IEnumerable selectedValues = null, string emptyText = null, bool nameIsId = false)
        {
            if (!type.GetTypeInfo().IsEnum)
            {
                throw new NotSupportedException("The type must be is enum type.");
            }

            var array = Enum.GetValues(type);

            var values = (from object e in array
                          select new
                          {
                              Id = nameIsId ? e.ToString() : e.ConvertTo<int>().ToString(),
                              Name = Extenso.EnumExtensions.GetDisplayName(e, out int order),
                              Order = order
                          }).ToList();

            values = values
                .Where(x => x.Order != -1)
                .OrderBy(x => x.Order)
                .ToList();

            if (emptyText != null)
            {
                values.Insert(0, new
                {
                    Id = nameIsId ? string.Empty : "-1",
                    Name = emptyText,
                    Order = -1
                });
            }

            return new MultiSelectList(values, "Id", "Name", selectedValues);
        }
    }

    public static SelectList ToSelectList<T>(object selectedValue = null, string emptyText = null, bool nameIsId = false) where T : struct
    {
        if (!typeof(T).GetTypeInfo().IsEnum)
        {
            throw new NotSupportedException("You must specify an enum type");
        }

        var values = (from T e in Extenso.EnumExtensions.GetValues<T>()
                      select new
                      {
                          Id = nameIsId ? e.ToString() : e.ConvertTo<int>().ToString(),
                          Name = Extenso.EnumExtensions.GetDisplayName(e, out int order),
                          Order = order
                      }).ToList();

        values = values
            .Where(x => x.Order != -1)
            .OrderBy(x => x.Order)
            .ToList();

        if (emptyText != null)
        {
            values.Insert(0, new
            {
                Id = nameIsId ? string.Empty : "-1",
                Name = emptyText,
                Order = -1
            });
        }

        return new SelectList(values, "Id", "Name", selectedValue);
    }

    public static MultiSelectList ToMultiSelectList<T>(IEnumerable selectedValues = null, string emptyText = null, bool nameIsId = false) where T : struct
    {
        if (!typeof(T).GetTypeInfo().IsEnum)
        {
            throw new NotSupportedException("You must specify an enum type");
        }

        var values = (from T e in Extenso.EnumExtensions.GetValues<T>()
                      select new
                      {
                          Id = nameIsId ? e.ToString() : e.ConvertTo<int>().ToString(),
                          Name = Extenso.EnumExtensions.GetDisplayName(e, out int order),
                          Order = order
                      }).ToList();

        values = values
            .Where(x => x.Order != -1)
            .OrderBy(x => x.Order)
            .ToList();

        if (emptyText != null)
        {
            values.Insert(0, new
            {
                Id = nameIsId ? string.Empty : "-1",
                Name = emptyText,
                Order = -1
            });
        }

        return new MultiSelectList(values, "Id", "Name", selectedValues);
    }
}