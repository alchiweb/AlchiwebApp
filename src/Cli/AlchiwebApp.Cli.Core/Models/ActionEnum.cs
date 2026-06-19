namespace AlchiwebApp.Cli.Core.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ActionEnum
{
    None = 0,
    [JsonStringEnumMemberName("modify")]
    Modify = 1,
    [JsonStringEnumMemberName("add.before")]
    AddBefore = 2,
    [JsonStringEnumMemberName("add.after")]
    AddAfter = 3,
}

