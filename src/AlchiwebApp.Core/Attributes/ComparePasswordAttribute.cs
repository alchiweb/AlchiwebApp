using AlchiwebApp.Core.Interfaces;

namespace System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ComparePasswordAttribute : ValidationAttribute
{
    public ComparePasswordAttribute() :base()
    {
    }
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {

        if (validationContext.ObjectInstance != null && validationContext.ObjectInstance is IConfirmPassword confirmPasswordModel && !Equals(confirmPasswordModel.ConfirmPassword, confirmPasswordModel.Password))
        {
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), ["ConfirmPassword","Password"]);
        }

        return null;
    }

    //public override bool IsValid(object? value)
    //{
    //    if (value != null && base.IsValid(value) && value is RoleDtoEnum input && input != 0)
    //    {
    //        return true;
    //    }

    //    return false;
    //}
}
