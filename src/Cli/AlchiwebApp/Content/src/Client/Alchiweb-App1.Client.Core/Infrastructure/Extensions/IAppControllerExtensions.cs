
using System.Text;
using System.Text.Json.Serialization.Metadata;
using Alchiweb-App1.Client.Core;
using Alchiweb-App1.Core.Infrastructure.Services.Contracts;

namespace Microsoft.Extensions.Configuration;
public static partial class IAppControllerExtensions
{
    //public static void ToJson<TRequest>(this IAppController appController, string name, TRequest obj)
    //{
    //    appController.AddQueryString(name, new StringContent(JsonSerializer.Serialize(obj, typeof(TRequest), AppJsonContext.Default), Encoding.UTF8, "application/json"));
    //}
    public static void AddQueryParameter<T>(this IAppController appController, T filter, JsonTypeInfo<T> jsonTypeInfo, int? with = null)
    {
        var dictionary = new Dictionary<string, object?>();

        using var jsonDocument = JsonSerializer.SerializeToDocument(filter, jsonTypeInfo);

        foreach (var property in jsonDocument.RootElement.EnumerateObject())
        {
            if (!string.IsNullOrEmpty(property.Value.ToString()))
                dictionary.Add(property.Name, property.Value.ToString());
        }
        if (with != null)
            dictionary.Add(nameof(with), with.Value.ToString());

        appController.AddQueryStrings(dictionary);
        // With reflexion
        //return ToUrlValues(filter ?? new IBasePaging<TEnumFilter>());



        //var page = filter?.Page;
        //var pageSize = filter?.PageSize;
        //var sortBy = filter?.SortBy != null  filter.SortBy.Value : (Enum)StudentFieldDtoEnum.None;
        //var reverseOrder = filter?.ReverseOrder;

        //if (page == null || page < 1)
        //    page = 1;
        //if (pageSize == null || pageSize < 1)
        //    pageSize = 10;
        //if (reverseOrder == null)
        //    reverseOrder = false;

        //return new Dictionary<string, string>()
        //{
        //    { nameof(filter.Page), page.Value.ToString() },
        //    { nameof(filter.PageSize), pageSize.Value.ToString() },
        //    { nameof(filter.ReverseOrder), reverseOrder.Value.ToString() },
        //    { nameof(filter.SortBy), sortBy.ToString() }
        //};
    }
}
