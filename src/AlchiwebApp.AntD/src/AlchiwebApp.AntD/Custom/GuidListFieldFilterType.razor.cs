using AntDesign;
using AntDesign.Filters;

namespace AlchiwebApp.AntD.Custom;

public class ListFieldFilterType<TData, TDataItem> : GuidFieldFilterType

{
    public List<TData> DataSource { get; } = [];

    public override TableFilterCompareOperator DefaultCompareOperator => TableFilterCompareOperator.Equals;

    private static readonly IEnumerable<TableFilterCompareOperator> _supportedCompareOperators =
    [
        TableFilterCompareOperator.Equals,
        TableFilterCompareOperator.NotEquals,
    ];


    public override RenderFragment<TableFilterInputRenderOptions> FilterInput { get; }
    public ListFieldFilterType(Func<TData, TDataItem> itemValue, Func<TData, string> itemLabel) : base()
    {
        SupportedCompareOperators = _supportedCompareOperators;
        FilterInput = CustomFilterInputs.Instance.GetGuidListInput(DataSource, itemValue, itemLabel);
    }
}
