using AntDesign;
using AntDesign.Custom;
using AntDesign.Filters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace AlchiwebApp.AntD.Custom;

public class FlagsEnumFieldFilterType<T> : EnumFieldFilterType<T>

{
    public override TableFilterCompareOperator DefaultCompareOperator => TableFilterCompareOperator.Contains;

    public override RenderFragment<TableFilterInputRenderOptions> FilterInput { get; } = CustomFilterInputs.Instance.GetEnumInput<T>();
    public FlagsEnumFieldFilterType() : base()
    {
    }
}
