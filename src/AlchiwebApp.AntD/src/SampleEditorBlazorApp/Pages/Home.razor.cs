using AlchiwebApp.AntD.Editors;
using AlchiwebApp.AntD.Editors.Abstractions.Models;
using AlchiwebApp.AntD.Editors.Models;
using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;
using System.Text.Json.Nodes;
namespace SampleEditorBlazorApp.Pages;
public partial class Home : ComponentBase
{
    [Inject]
    private IJSRuntime JSRuntime { get; init; } = default!;

    private JsonObject EditorValue { get; set; } = JsonNode.Parse("{ \"time\":1749220223119,\"blocks\":[{\"id\":\"jKswmyl3Za\",\"type\":\"paragraph\",\"data\":{\"text\":\"Description complémentaire, présentant les spécificités du projet pédagogique de l'établissement vis-à-vis de la compétence.\"}},{\"id\":\"jKswmrt2a\",\"type\":\"paragraph\",\"data\":{\"text\":\"Sint id magna eu do eiusmod adipisicing ex ex ipsum amet. Pariatur consectetur duis cillum mollit voluptate tempor mollit Lorem fugiat culpa sint. Adipisicing amet minim Lorem elit minim id qui ea laboris Lorem amet quis cillum. Consectetur cillum ad tempor eu aliquip duis minim esse irure non sunt.\"}}],\"version\":\"2.31.0-rc.9\"}")!.AsObject();
    public ToolsConfigurations EditorTools { get; set; } = default!;
    public EditorJsConfiguration EditorConfigurations { get; set; } = default!;
    public Task OnEditorValueChanged(JsonObject value) => Task.FromResult(EditorValue = value);

    public string Valeur = "{\"time\":1749220223119,\"blocks\":[{\"id\":\"jKswmyl3Za\",\"type\":\"paragraph\",\"data\":{\"text\":\"Description complémentaire, présentant les spécificités du projet pédagogique de l'établissement vis-à-vis de la compétence.\"}},{\"id\":\"jKswmrt2a\",\"type\":\"paragraph\",\"data\":{\"text\":\"Sint id magna eu do eiusmod adipisicing ex ex ipsum amet. Pariatur consectetur duis cillum mollit voluptate tempor mollit Lorem fugiat culpa sint. Adipisicing amet minim Lorem elit minim id qui ea laboris Lorem amet quis cillum. Consectetur cillum ad tempor eu aliquip duis minim esse irure non sunt.\"}}],\"version\":\"2.31.0-rc.9\"}";
    /// <summary>
    /// Use this to render new content into the editor
    /// </summary>
    public TextArea Textarea1 { get; set; } = new ();
    public string TextareaValue { get; set; } = "";
    public AlchiwebApp.AntD.Editors.EditorJs? Editor02 { get; set; }

    public JsonObject? EditorValue02 { get; set; } = EditorJs.CreateEmptyJsonObject();
    public ToolsConfigurations EditorTools02 { get; set; } = default!;
    public EditorJsConfiguration EditorConfigurations02 { get; set; } = default!;
    public Task OnEditorValue02Changed(JsonObject value) => Task.FromResult(EditorValue02 = value);

    protected override void OnAfterRender(bool first_render)
    {
        if (first_render)
        {
            StateHasChanged();
        }
    }

    public async Task InvokeInsertAsync()
    {
        if (Editor02 is null)
        {
            return;
        }

        string json_t = """
            {
              "tjime": 1717207275445,
              "blocks": [
                {
                  "idr": "mhTl6ghSkV",
                  "type": "paragraph",
                  "data": {
                    "text": "Hey. Meet the new Editor. On this picture you can see it in action. Then, try a demo"
                  }
                }
              ],
              "verslmion": "2.29.1"
            }
            """;

        var testData = JsonSerializer.Deserialize<EditorJsData>(json_t);
        
        await Editor02.RenderAsync(JsonSerializer.SerializeToNode(testData)?.AsObject() ?? []);

        await JSRuntime.InvokeVoidAsync("console.log", testData);
    }

    protected override void OnInitialized()
    {
        //string json_t = """{"time": 1717207275445, "blocks": [{"id": "qDEsgkmbL1", "data": {"text": "Heylo, World!", "wrap": "title"}, "type": "text"}], "version": "2.29.1"}""";
        //EditorValue = JsonNode.Parse(json_t)!.AsObject();

        // In this example the Toggle configurations have been dynamically loaded in from an external CDN -> https://github.com/kommitters/editorjs-toggle-block
        // string editor_tools = """{"Toggle":{"LoadActions":{"LoadProviderClassFunctionDefault":"ToggleBlock","OptionsNamingScheme":"CamelCase"},"options":{"inlineToolbar":true}},"Header":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"LinkTool":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"NestedList":{"LoadActions":{"OptionsNamingScheme":"CamelCase","OverrideOptionsKey":"list"}},"Marker":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"Warning":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"Checklist":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"CodeTool":{"LoadActions":{"OptionsNamingScheme":"CamelCase","OverrideOptionsKey":"code"}},"Delimiter":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"SimpleImage":{"LoadActions":{"OptionsNamingScheme":"CamelCase","OverrideOptionsKey":"image"}},"Embed":{"LoadActions":{"OptionsNamingScheme":"CamelCase"},"options":{"config":{"services":{"instagram":true,"youtube":true,"vimeo":true,"imgur":true,"twitter":true,"facebook":true}}}},"InlineCode":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"Quote":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"Table":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}}}""";

        //string editor_tools = """
        //{
        //  "Paragraph": {
        //    "LoadActions": {
        //      "LoadProviderClassFunctionDefault": false,
        //      "OptionsNamingScheme": "CamelCase"
        //    }
        //  },
        //  "Text": {
        //    "LoadActions": {
        //      "LoadProviderClassFunctionDefault": "TextElement",
        //      "OptionsNamingScheme": "CamelCase"
        //    },
        //    "options": {
        //      "inlineToolbar": true,
        //      "config": {
        //        "placeholder": "...",
        //        "preserveBlank": false,
        //        "allowEnterKeyDown": false,
        //        "hidePopoverItem": true,
        //        "hideToolbar": true,
        //        "wrapElement": "title"
        //      }
        //    }
        //  }
        //}
        //""";
        EditorTools = AlchiwebApp.AntD.Editors.EditorJs.DefaultEditorToolsConfigurations; //EditorJs.ParseEditorJsonToolOptions(editor_tools);
        EditorConfigurations = new();// JsonNode.Parse("""{ "DefaultBlock": "text", "CodexEditorRedactor" : { "style": { "paddingBottom": "0px", "maxHeight": "64px", "overflow": "hidden" } } }""")?.AsObject() ?? [];

        // If the browser recieves the following error: "Saving failed due to the Error TypeError: Cannot read properties of undefined (reading 'sanitizeConfig')"
        // This is because edtorjs has certain dependencies caused by the `header.inlineToolbar' array values. EditorJS should have the appropriate tools/plugins enabled.
        // This error will also occur because of unsupported blocks in the editor and the whole editor document may need to be reset.
        //string editor_tools_02 = """
        //{
        //  "Header": {
        //    "LoadActions": {
        //      "OptionsNamingScheme": "CamelCase"
        //    }
        //  },
        //  "LinkTool": {
        //    "LoadActions": {
        //      "OptionsNamingScheme": "CamelCase"
        //    }
        //  },
        //  "NestedList": {
        //    "LoadActions": {
        //      "OptionsNamingScheme": "CamelCase",
        //      "OverrideOptionsKey": "list"
        //    }
        //  },
        //  "Marker": {
        //    "LoadActions": {
        //      "OptionsNamingScheme": "CamelCase"
        //    }
        //  },
        //  "Checklist": {
        //    "LoadActions": {
        //      "OptionsNamingScheme": "CamelCase"
        //    }
        //  },
        //  "CodeTool": {
        //    "LoadActions": {
        //      "OptionsNamingScheme": "CamelCase",
        //      "OverrideOptionsKey": "code"
        //    }
        //  },
        //  "Delimiter": {
        //    "LoadActions": {
        //      "OptionsNamingScheme": "CamelCase"
        //    }
        //  },
        //  "SimpleImage": {
        //    "LoadActions": {
        //      "OptionsNamingScheme": "CamelCase",
        //      "OverrideOptionsKey": "image"
        //    }
        //  },
        //  "Embed": {
        //    "LoadActions": {
        //      "OptionsNamingScheme": "CamelCase"
        //    },
        //    "options": {
        //      "inlineToolbar": true,
        
        //      "config": {
        //        "services": {
        //          "instagram": true,
        //          "youtube": true,
        //          "vimeo": true,
        //          "imgur": true,
        //          "twitter": true,
        //          "facebook": true
        //        }
        //      }
        //    }
        //  },
        //  "InlineCode": {
        //    "LoadActions": {
        //      "OptionsNamingScheme": "CamelCase"
        //    }
        //  },
        //  "Quote": {
        //    "LoadActions": {
        //      "OptionsNamingScheme": "CamelCase"
        //    }
        //  },
        //  "Table": {
        //    "LoadActions": {
        //      "OptionsNamingScheme": "CamelCase"
        //    }
        //  }
        //}
        //"""; // "Warning":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}}

        //EditorTools02 = EditorJs.ParseEditorJsonToolOptions(editor_tools_02);
        //EditorValue02 = EditorJs.CreateEmptyJsonObject();
        //EditorConfigurations02 = JsonNode.Parse("""{ "DefaultBlock": "paragraph" }""")?.AsObject() ?? [];

    }

    public async Task CheckEditorValueAsync()
    {
        await JSRuntime.InvokeVoidAsync("console.log", "-----------");

        await JSRuntime.InvokeVoidAsync("console.log", Valeur);
        await JSRuntime.InvokeVoidAsync("console.log", Editor02?.Value);
        await JSRuntime.InvokeVoidAsync("console.log", Editor02?.EditorValue?.ToJsonString());
        await JSRuntime.InvokeVoidAsync("console.log", "++++++++++++");
    }
}
