using AntDesign;
using AntDesign.Filters;

namespace AlchiwebApp.Client.Core.AntD;
public class CustomStringFieldFilterType : StringFieldFilterType
{
    public override TableFilterCompareOperator DefaultCompareOperator => TableFilterCompareOperator.StartsWith;


    private static readonly IEnumerable<TableFilterCompareOperator> _supportedCompareOperators =
[
        TableFilterCompareOperator.StartsWith,
    ];
    public CustomStringFieldFilterType() : base()
    {
        SupportedCompareOperators = _supportedCompareOperators;
    }
}
