namespace AlchiwebApp.Cli.Core.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ActionEnum
{
    None = 0,
    [JsonStringEnumMemberName("modify")]
    Modify = 1,
    [JsonStringEnumMemberName("core.modify")]
    CoreModify = 2,
    [JsonStringEnumMemberName("add.before")]
    AddBefore = 3,
    [JsonStringEnumMemberName("core.add.before")]
    CoreAddBefore = 4,
    [JsonStringEnumMemberName("add.after")]
    AddAfter = 5,
    [JsonStringEnumMemberName("core.add.after")]
    CoreAddAfter = 6,
}

