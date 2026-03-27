namespace System.ComponentModel.DataAnnotations;

public class GuidNotEmpty : RegularExpressionAttribute
{
    public GuidNotEmpty(): base("^((?!00000000-0000-0000-0000-000000000000).)*$") { }
}
