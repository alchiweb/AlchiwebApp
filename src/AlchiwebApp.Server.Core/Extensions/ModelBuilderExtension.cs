using AlchiwebApp.Server.Core.Converters;

namespace Microsoft.EntityFrameworkCore;

public static class ModelBuilderExtension
{
    public static ModelBuilder ApplyDateTimeConverter(this ModelBuilder modelBuilder)
    {
        var dateTimeUtcConverter = new DateTimeUtcConverter();
        var dateTimeUtcNullableConverter = new DateTimeUtcNullableConverter();
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {

                if (property.ClrType == typeof(DateTime))
                    property.SetValueConverter(dateTimeUtcConverter);
                else if (property.ClrType == typeof(DateTime?))
                    property.SetValueConverter(dateTimeUtcNullableConverter);
            }
        }
        return modelBuilder;
    }
}
