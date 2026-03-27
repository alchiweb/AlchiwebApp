using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AlchiwebApp.Server.Core.Converters;

public class DateTimeUtcNullableConverter : ValueConverter<DateTime?, DateTime?>
{
    public DateTimeUtcNullableConverter() : base(
        d => d.HasValue ? d.Value.ToUniversalTime() : d,
        d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc).ToLocalTime() : d
        )
    {
    }
}
