using Ardalis.Specification.EntityFrameworkCore.SharedKernel.Utils;
using System.Text.RegularExpressions;
using Ardalis.Specification;
using System.Linq.Expressions;
using AlchiwebApp.PagingFiltering.Filtering;
using AlchiwebApp.PagingFiltering.Filtering.Enums;
using System.Reflection;

namespace Ardalis.Specification.EntityFrameworkCore.SharedKernel.Extensions;

public static class FieldFilterListExtensions
{
    public static Expression<Func<TEntity, bool>>? GetFiltersForQuery<TEntity, TEnum, TField, TFilteringEnum>(this IEnumerable<FieldFilter<TEnum, TField>> filterList, TFilteringEnum fieldTypes)
        where TField : notnull
        where TEnum : struct, Enum
        where TFilteringEnum : struct, Enum
    {
        if (filterList == null || filterList.Count() == 0)
            return null;
        ParameterExpression entity;
        try
        {
            entity = Expression.Parameter(typeof(TEntity));
        }
        catch (Exception)
        {
            return null;
        }
        Expression propertyField;
        Expression? totalExpression = null;
        foreach (var fieldNestedName in Enum.GetValues<TFilteringEnum>().Where(e => (int)(object)e != 0 && fieldTypes.HasFlag(e)))
        {
            try
            {
                propertyField = ExpressionHelper.CreateMemberExpression<TEntity, TFilteringEnum>(entity, fieldNestedName);
            }
            catch (Exception)
            {
                continue;
            }
            bool flowControl = CreateExpression(filterList, propertyField, ref totalExpression);
            if (!flowControl)
            {
                continue;
            }
        }
        if (totalExpression == null)
            return null;
        return Expression.Lambda<Func<TEntity, bool>>(totalExpression, [entity]);
    }

    public static Expression<Func<TEntity, bool>>? GetFiltersForQuery<TEntity, TEnum, TField>(this IEnumerable<FieldFilter<TEnum, TField>> filterList, Expression expression)
        where TField : notnull
        where TEnum : struct, Enum
    => filterList.GetFiltersForQuery<TEntity, TEnum, TField>([expression]);

    public static Expression<Func<TEntity, bool>>? GetFiltersForQuery<TEntity, TEnum, TField>(this IEnumerable<FieldFilter<TEnum, TField>> filterList, Expression[] expressions)
        where TField : notnull
        where TEnum : struct, Enum
    {
        if (filterList == null || filterList.Count() == 0)
            return null;
        ParameterExpression entity;
        try
        {
            entity = Expression.Parameter(typeof(TEntity));
        }
        catch (Exception)
        {
            return null;
        }
        Expression? totalExpression = null;
        foreach (var propertyField in expressions)
        {
            bool flowControl = CreateExpression(filterList, propertyField, ref totalExpression);
            if (!flowControl)
            {
                continue;
            }
        }
        if (totalExpression == null)
            return null;
        return Expression.Lambda<Func<TEntity, bool>>(totalExpression, [entity]);
    }

    private static bool CreateExpression<TEnum, TField>(IEnumerable<FieldFilter<TEnum, TField>> filterList, Expression propertyField, ref Expression? totalExpression) where TEnum : struct, Enum
        where TField : notnull
    {
        bool isFirstFilter = true;
        switch (filterList)
        {
            case IEnumerable<FieldFilter<TEnum, string>> stringFieldFilters:
                MethodInfo? regexMethod = null;

                MethodInfo? containsMethod = null;
                MethodInfo? equalsMethod = null;
                MethodInfo? startswithMethod = null;
                MethodInfo? endswithMethod = null;

                var regexOptions = Expression.Constant(RegexOptions.IgnoreCase | RegexOptions.Compiled);
#if !ALCHIWEBAPP_SQLSERVER
                regexMethod = typeof(Regex).GetMethod("IsMatch", [typeof(string), typeof(string), typeof(RegexOptions)]);
#endif
                if (regexMethod == null)
                {
                    containsMethod = typeof(String).GetMethod("Contains", [typeof(string)]);
                    equalsMethod = typeof(String).GetMethod("Equals", [typeof(string)]);
                    startswithMethod = typeof(String).GetMethod("StartsWith", [typeof(string)]);
                    endswithMethod = typeof(String).GetMethod("EndsWith", [typeof(string)]);

                    if (containsMethod == null && startswithMethod == null && endswithMethod == null && equalsMethod == null)
                        return false;
                }

                foreach (var filter in stringFieldFilters)
                {
                    if (isFirstFilter)
                    {
                        filter.IsOrCondition = true;
                        isFirstFilter = false;
                    }
                    // cast for being compatible with other enum...
                    var comparator = (int)(object)filter.Comparator;
                    Expression? newExpression = null;
                    if (regexMethod == null)
                    {
                        var constantSearchPattern = Expression.Constant(filter.SearchValue);
                        var methodCall = comparator switch
                        {
                            (int)StringFieldFilterEnum.StartsWith =>
                                startswithMethod == null ? null : Expression.Call(propertyField, startswithMethod, constantSearchPattern),
                            (int)StringFieldFilterEnum.EndsWith =>
                                endswithMethod == null ? null : Expression.Call(propertyField, endswithMethod, constantSearchPattern),
                            (int)StringFieldFilterEnum.Equals or
                            (int)StringFieldFilterEnum.NotEquals =>
                                equalsMethod == null ? null : Expression.Call(propertyField, equalsMethod, constantSearchPattern),
                            _ =>
                                containsMethod == null ? null : Expression.Call(propertyField, containsMethod, constantSearchPattern),
                        };
                        if (methodCall != null)
                        {
                            newExpression = comparator switch
                            {
                                (int)StringFieldFilterEnum.StartsWith or
                                (int)StringFieldFilterEnum.EndsWith or
                                (int)StringFieldFilterEnum.Equals or
                                (int)StringFieldFilterEnum.Contains =>
                                    methodCall,
                                (int)StringFieldFilterEnum.NotEquals or
                                (int)StringFieldFilterEnum.NotContains =>
                                    Expression.Not(methodCall),
                                _ => null
                            };
                        }
                    }
                    else {
                        var searchPattern = comparator switch
                        {
                            (int)StringFieldFilterEnum.StartsWith => $"^{filter.SearchValue}.*$",
                            (int)StringFieldFilterEnum.EndsWith => $"^.*{filter.SearchValue}$",
                            (int)StringFieldFilterEnum.NotContains or
                            (int)StringFieldFilterEnum.Contains => $"^.*{filter.SearchValue}.*$",
                            _ or
                            (int)StringFieldFilterEnum.NotEquals or
                            (int)StringFieldFilterEnum.Equals => $"^{filter.SearchValue}$",
                        };
                        var regexMethodCall = Expression.Call(regexMethod, [propertyField, Expression.Constant(searchPattern), regexOptions]);
                        newExpression = comparator switch
                        {
                            (int)StringFieldFilterEnum.StartsWith or
                            (int)StringFieldFilterEnum.EndsWith or
                            (int)StringFieldFilterEnum.Equals or
                            (int)StringFieldFilterEnum.Contains =>
                                regexMethodCall,
                            (int)StringFieldFilterEnum.NotEquals or
                            (int)StringFieldFilterEnum.NotContains =>
                                Expression.Not(regexMethodCall),
                            _ => null
                        };
                    }
                    if (newExpression != null)
                    {
                        totalExpression = ExpressionHelper.AddExpression(totalExpression, newExpression, isFirstFilter ? true : filter.IsOrCondition);
                        isFirstFilter = false;
                    }
                }
                break;
            case IEnumerable<FieldFilter<TEnum, TField>> enumFieldFilters when
                    typeof(TField).IsEnum && propertyField.Type.IsEnum:
                foreach (var filter in enumFieldFilters)
                {
                    // cast for being compatible with other enum...
                    var comparator = (int)(object)filter.Comparator;
                    Expression? newExpression = null;
                    if (propertyField.Type.IsEnum)
                    {
                        string[] searchStringValues = filter.SearchValue?.ToString()?.Split(',') ?? [];
                        newExpression = CreateEnumExpression<TField>(propertyField, comparator, searchStringValues);
                    }
                    else
                    {
                        var searchValue = Expression.Constant(Convert.ChangeType(filter.SearchValue, Type.GetTypeCode(typeof(TField))));
                        newExpression = comparator switch
                        {
                            (int)EnumFieldFilterEnum.NotEquals or
                            (int)EnumFieldFilterEnum.NotContains =>
                                Expression.NotEqual(propertyField, searchValue),
                            _ or
                            (int)EnumFieldFilterEnum.Equals or
                            (int)EnumFieldFilterEnum.Contains =>
                                Expression.Equal(propertyField, searchValue),
                        };
                    }
                    if (newExpression != null)
                    {
                        totalExpression = ExpressionHelper.AddExpression(totalExpression, newExpression, isFirstFilter ? true : filter.IsOrCondition);
                        isFirstFilter = false;
                    }
                }
                break;
            case IEnumerable<FieldFilter<TEnum, string[]>> stringArrayFieldFilters:
                break;
            case IEnumerable<FieldFilter<TEnum, DateTime[]>> dateFieldFilters when
                    propertyField.Type == typeof(DateTime?) || propertyField.Type == typeof(DateTime):
                CreateExpressionForDate(propertyField, ref totalExpression, ref isFirstFilter, dateFieldFilters);
                break;
            case IEnumerable<FieldFilter<TEnum, DateOnly[]>> dateFieldFilters when
                    propertyField.Type == typeof(DateOnly?) || propertyField.Type == typeof(DateOnly):
                CreateExpressionForDate(propertyField, ref totalExpression, ref isFirstFilter, dateFieldFilters);
                break;
            case IEnumerable<FieldFilter<TEnum, DateTimeOffset[]>> dateFieldFilters when
                    propertyField.Type == typeof(DateTimeOffset?) || propertyField.Type == typeof(DateTimeOffset):
                CreateExpressionForDate(propertyField, ref totalExpression, ref isFirstFilter, dateFieldFilters);
                break;
            case IEnumerable<FieldFilter<TEnum, TField>> numberFieldFilters when
                typeof(TField) == typeof(Int16) ||
                typeof(TField) == typeof(Int16[]) ||
                typeof(TField) == typeof(Int32) ||
                typeof(TField) == typeof(Int32[]) ||
                typeof(TField) == typeof(Int64) ||
                typeof(TField) == typeof(Int64[]) ||
                typeof(TField) == typeof(Decimal) ||
                typeof(TField) == typeof(Decimal[]) ||
                typeof(TField) == typeof(Single) ||
                typeof(TField) == typeof(Single[]) ||
                typeof(TField) == typeof(Double) ||
                typeof(TField) == typeof(Double[]) ||
                typeof(TField) == typeof(Byte) ||
                typeof(TField) == typeof(Byte[]) ||
                typeof(TField) == typeof(SByte) ||
                typeof(TField) == typeof(SByte[]) ||
                typeof(TField) == typeof(Guid) ||
                typeof(TField) == typeof(Guid[])
                :
                foreach (var filter in numberFieldFilters)
                {
                    Expression? newExpression = null;
                    // cast for being compatible with other enum...
                    var comparator = (int)(object)filter.Comparator;
                    if (filter.SearchValue is Array searchValues)
                    {
                        newExpression = CreateItemExpression(propertyField, comparator, searchValues);
                    }
                    else
                    {
                        var searchValue = Expression.Constant(filter.SearchValue, propertyField.Type);
                        newExpression = comparator switch
                        {
                            (int)NumberFieldFilterEnum.NotEquals =>
                                Expression.NotEqual(propertyField, searchValue),
                            (int)NumberFieldFilterEnum.GreaterThan =>
                                Expression.GreaterThan(propertyField, searchValue),
                            (int)NumberFieldFilterEnum.GreaterThanOrEquals =>
                                Expression.GreaterThanOrEqual(propertyField, searchValue),
                            (int)NumberFieldFilterEnum.LessThan =>
                                Expression.LessThan(propertyField, searchValue),
                            (int)NumberFieldFilterEnum.LessThanOrEquals =>
                                Expression.LessThanOrEqual(propertyField, searchValue),
                            _ or
                            (int)NumberFieldFilterEnum.Equals =>
                                Expression.Equal(propertyField, searchValue),
                        };
                    }
                    totalExpression = ExpressionHelper.AddExpression(totalExpression, newExpression, isFirstFilter ? true : filter.IsOrCondition);
                    isFirstFilter = false;
                }
                break;
            default:
                break;
        }

        return true;
    }

    private static void CreateExpressionForDate<TEnum, TDate>(Expression propertyField, ref Expression? totalExpression, ref bool isFirstFilter, IEnumerable<FieldFilter<TEnum, TDate[]>> dateFieldFilters)
        where TEnum : struct, Enum
    {
        TDate? defaultDate = default(TDate);
        if (propertyField.Type == typeof(TDate?))
        {
            defaultDate = default(TDate);
        }
        foreach (var filter in dateFieldFilters)
        {
            var searchValue = Expression.Constant(filter.SearchValue.Length > 0 ? filter.SearchValue[0] : defaultDate, propertyField.Type);
            var searchValue2 = Expression.Constant(filter.SearchValue.Length > 1 ? filter.SearchValue[1] : defaultDate, propertyField.Type);
            Expression? newExpression = null;
            // cast for being compatible with other enum...
            var comparator = (int)(object)filter.Comparator;
            newExpression = comparator switch
            {
                (int)DateTimeFieldFilterEnum.NotEquals =>
                    Expression.NotEqual(propertyField, searchValue),
                (int)DateTimeFieldFilterEnum.Equals =>
                    Expression.Equal(propertyField, searchValue),
                (int)DateTimeFieldFilterEnum.GreaterThan =>
                    Expression.GreaterThan(propertyField, searchValue),
                (int)DateTimeFieldFilterEnum.GreaterThanOrEquals =>
                    Expression.GreaterThanOrEqual(propertyField, searchValue),
                (int)DateTimeFieldFilterEnum.LessThan =>
                    Expression.LessThan(propertyField, searchValue),
                (int)DateTimeFieldFilterEnum.LessThanOrEquals =>
                    Expression.LessThanOrEqual(propertyField, searchValue),
                (int)DateTimeFieldFilterEnum.Between =>
                    Expression.And(Expression.GreaterThan(propertyField, searchValue), Expression.LessThanOrEqual(propertyField, searchValue2)),
                _ => null
            };
            totalExpression = ExpressionHelper.AddExpression(totalExpression, newExpression, isFirstFilter ? true : filter.IsOrCondition);
            isFirstFilter = false;
        }
    }

    private static Expression? CreateEnumExpression<TField>(Expression propertyField, int comparator, string[] searchStringValues)
    {
        Expression? newExpression = null;
        foreach (var enumStringValue in searchStringValues)
        {
            var searchValue = Expression.Convert(Expression.Constant(Enum.Parse(typeof(TField?), enumStringValue), propertyField.Type), typeof(int));

            var newTempExpression = comparator switch
            {
                (int)EnumFieldFilterEnum.NotEquals or
                (int)EnumFieldFilterEnum.NotContains =>
                    Expression.NotEqual(Expression.Convert(propertyField, typeof(int)), searchValue),
                _ or
                (int)EnumFieldFilterEnum.Equals or
                (int)EnumFieldFilterEnum.Contains =>
                    Expression.Equal(Expression.Convert(propertyField, typeof(int)), searchValue),
            };
            // Tweak for PostgreSQL???
            //var newTempExpression = comparator switch
            //{
            //    (int)EnumFieldFilterEnum.Equals or
            //    (int)EnumFieldFilterEnum.Contains =>
            //        Expression.NotEqual(Expression.And(Expression.Convert(propertyField, typeof(int)), searchValue), Expression.Constant(0)),
            //    (int)EnumFieldFilterEnum.NotEquals or
            //    (int)EnumFieldFilterEnum.NotContains =>
            //        Expression.Equal(Expression.And(Expression.Convert(propertyField, typeof(int)), searchValue), Expression.Constant(0)),
            //    _ => null
            //};

            if (newExpression == null)
                newExpression = newTempExpression;
            else
            {
                newExpression = ExpressionHelper.AddExpression(newExpression, newTempExpression,
                    comparator == (int)EnumFieldFilterEnum.Contains || comparator == (int)EnumFieldFilterEnum.NotContains
                    );
            }

        }

        return newExpression;
    }

    private static Expression? CreateItemExpression(Expression propertyField, int comparator, Array searchGuidValues)
    {
        Expression? newExpression = null;
        foreach (var guidValue in searchGuidValues)
        {
            var searchValue = Expression.Constant(guidValue, propertyField.Type);

            var newTempExpression = comparator switch
            {
                //(int)GuidFieldFilterEnum.Equals or // not needed: same value as (int)NumberFieldFilterEnum.Equals
                (int)NumberFieldFilterEnum.Equals =>
                Expression.Equal(propertyField, searchValue),
                //(int)GuidFieldFilterEnum.NotEquals or // not needed: same value as (int)NumberFieldFilterEnum.NotEquals
                (int)NumberFieldFilterEnum.NotEquals =>
                    Expression.NotEqual(propertyField, searchValue),
                (int)NumberFieldFilterEnum.GreaterThan =>
                    Expression.GreaterThan(propertyField, searchValue),
                (int)NumberFieldFilterEnum.GreaterThanOrEquals =>
                    Expression.GreaterThanOrEqual(propertyField, searchValue),
                (int)NumberFieldFilterEnum.LessThan =>
                    Expression.LessThan(propertyField, searchValue),
                (int)NumberFieldFilterEnum.LessThanOrEquals =>
                    Expression.LessThanOrEqual(propertyField, searchValue),
                _ => null
            };

            if (newExpression == null)
                newExpression = newTempExpression;
            else
            {
                newExpression = ExpressionHelper.AddExpression(newExpression, newTempExpression,
                    comparator == (int)EnumFieldFilterEnum.Equals || comparator == (int)EnumFieldFilterEnum.NotEquals
                    );
            }

        }

        return newExpression;
    }
}
