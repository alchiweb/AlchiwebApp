namespace System.ComponentModel.DataAnnotations;

public class StringMinLengthAttribute : StringLengthAttribute
{
    public StringMinLengthAttribute(int minimumLength) : base(int.MaxValue)
    {
        MinimumLength = minimumLength;
    }
}
