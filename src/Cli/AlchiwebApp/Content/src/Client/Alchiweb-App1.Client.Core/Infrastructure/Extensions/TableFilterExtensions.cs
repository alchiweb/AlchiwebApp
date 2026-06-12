using AntDesign;

namespace AlchiwebApp.PagingFiltering.Filtering;

public static class TableFilterExtensions
{
    public static FieldFilter<TFieldEnum, TField>? GetFieldFilter<TFieldEnum, TField>(this TableFilter tableFilter)
    where TFieldEnum : struct, Enum
    where TField : notnull
    {
        TFieldEnum comparator;
        if (!Enum.TryParse(((int)tableFilter.FilterCompareOperator).ToString(), out comparator))
            return null;

        if (tableFilter.Value == null)
            return null;

        var searchValue = (TField)tableFilter.Value;
        if (searchValue == null)
            return null;

        var filter = new FieldFilter<TFieldEnum, TField>(comparator, searchValue, tableFilter.FilterCondition == TableFilterCondition.Or);
        return filter;
    }
}
