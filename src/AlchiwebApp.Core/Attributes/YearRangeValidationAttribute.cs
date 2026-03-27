namespace System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class YearRangeValidationAttribute : RegularExpressionAttribute
{
    public string? ErrorRegExMessageResourceName { get; set; }
    public string? ErrorTwoYearsBeConsecutiveMessageResourceName { get; set; }
    public YearRangeValidationAttribute() : base(@"^\d{4}/\d{4}$")
    {
        //ErrorMessageResourceType ??= typeof(I18n);
        ErrorRegExMessageResourceName ??= ErrorMessageResourceName;
        ErrorRegExMessageResourceName ??= "Year_Title_RegExValidation";
        ErrorTwoYearsBeConsecutiveMessageResourceName ??= "Year_Title_TwoYearsMustBeConsecutiveValidation";
    }
    public override bool IsValid(object? value)
    {
        if (base.IsValid(value) && value is string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                var years = input.Split('/');
                if (years.Length != 2 || !int.TryParse(years[0], out int firstYear) || !int.TryParse(years[1], out int secondYear))
                {
                    ErrorMessageResourceName = ErrorRegExMessageResourceName;
                    return false;
                }

                if (secondYear != firstYear + 1)
                {
                    ErrorMessageResourceName = ErrorTwoYearsBeConsecutiveMessageResourceName;
                    return false;
                }
            }
            return true;
        }
        ErrorMessageResourceName = ErrorRegExMessageResourceName;
        return false;
    }
}
