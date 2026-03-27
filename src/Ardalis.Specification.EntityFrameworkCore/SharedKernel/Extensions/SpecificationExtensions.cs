using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore.SharedKernel.Utils;
using AlchiwebApp.PagingFiltering.Filtering;

namespace Ardalis.Specification.EntityFrameworkCore.SharedKernel.Extensions;

public static class SpecificationExtensions
{
    public static void ApplySorting<TEntity, TSortingEnum>(this Specification<TEntity> spec, BaseFilter<TSortingEnum> filter)
        where TSortingEnum : struct, Enum
    {
        ParameterExpression entity;
        Expression propertyField;
        IOrderedSpecificationBuilder<TEntity>? orderedQuery = null;
        foreach (var sorting in filter.SortBy.OrderByDescending(s => s.Value.Priority))
        {
            try
            {
                entity = Expression.Parameter(typeof(TEntity));
                try
                {
                    propertyField = ExpressionHelper.CreateMemberExpression<TEntity, TSortingEnum>(entity, sorting.Key);
                }
                catch (Exception)
                {
                    continue;
                }
                //var stringExpr = Expression.Call(propertyField, "ToString", []);
                var lambdaExpression = Expression.Lambda<Func<TEntity, object?>>(Expression.Convert(propertyField, typeof(object)), [entity]);

                if (orderedQuery == null)
                {
                    if (sorting.Value.IsReversed)
                        orderedQuery = spec.Query.OrderByDescending(lambdaExpression);
                    else
                        orderedQuery = spec.Query.OrderBy(lambdaExpression);
                }
                else
                {
                    if (sorting.Value.IsReversed)
                        orderedQuery = orderedQuery.ThenByDescending(lambdaExpression);
                    else
                        orderedQuery = orderedQuery.ThenBy(lambdaExpression);
                }
            }
            catch (Exception) { }
        }
    }
    public static void ApplyFiltering<TEntity, TEnum, TField, TFilteringEnum>(this Specification<TEntity> spec, IEnumerable<FieldFilter<TEnum, TField>> filterList, TFilteringEnum fieldTypes)
        where TField : notnull
        where TEnum : struct, Enum
        where TFilteringEnum : struct, Enum
    {
        var queryFilters = filterList.GetFiltersForQuery<TEntity, TEnum, TField, TFilteringEnum>(fieldTypes);
        if (queryFilters != null)
            spec.Query.Where(queryFilters);
    }
    public static void ApplyFiltering<TEntity, TEnum, TField>(this Specification<TEntity> spec, IEnumerable<FieldFilter<TEnum, TField>> filterList, Expression expression)
        where TField : notnull
        where TEnum : struct, Enum
    {
        var queryFilters = filterList.GetFiltersForQuery<TEntity, TEnum, TField>(expression);
        if (queryFilters != null)
            spec.Query.Where(queryFilters);
    }
    public static void ApplyFiltering<TEntity, TEnum, TField>(this Specification<TEntity> spec, IEnumerable<FieldFilter<TEnum, TField>> filterList, Expression[] expressions)
        where TField : notnull
        where TEnum : struct, Enum
    {
        var queryFilters = filterList.GetFiltersForQuery<TEntity, TEnum, TField>(expressions);
        if (queryFilters != null)
            spec.Query.Where(queryFilters);
    }

}
