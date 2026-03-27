using AlchiwebApp.AntD.Editors.Models.Options;

namespace AlchiwebApp.AntD.Editors.Models
{
    /// <summary>
    /// Tools configuration for EditorJS.
    /// </summary>
    public class ToolsConfigurations
    {
        /// <summary>
        /// Paragraph tool configuration.
        /// </summary>
        public ToolConfiguration<ParagraphOptions>? Paragraph { get; set; }
        public ToolConfiguration<RawOptions>? Raw { get; set; }
        public ToolConfiguration<LinkAutocompleteOptions>? LinkAutocomplete { get; set; }
        public ToolConfiguration<LinkToolOptions>? LinkTool { get; set; }
        public ToolConfiguration<ListOptions>? List { get; set; }
        public ToolConfiguration<HeaderOptions>? Header { get; set; }
        public ToolConfiguration<WarningOptions>? Warning { get; set; }
        public ToolConfiguration<MarkerOptions>? Marker { get; set; }
        public ToolConfiguration<QuoteOptions>? Quote { get; set; }
        public ToolConfiguration<EmbedOptions>? Embed { get; set; }
        public ToolConfiguration<ChecklistOptions>? Checklist { get; set; }
        public ToolConfiguration<CodeToolOptions>? CodeTool { get; set; }
        public ToolConfiguration<DelimiterOptions>? Delimiter { get; set; }
        public ToolConfiguration<ImageOptions>? Image { get; set; }
        public ToolConfiguration<SimpleImageOptions>? SimpleImage { get; set; }
        public ToolConfiguration<InlineCodeOptions>? InlineCode { get; set; }
        public ToolConfiguration<TableOptions>? Table { get; set; }
        public ToolConfiguration<AttachesOptions>? AttachesTool { get; set; }
        public ToolConfiguration<NestedListOptions>? NestedList { get; set; }
        public ToolConfiguration<PersonalityOptions>? Personality { get; set; }
        public ToolConfiguration<TextOptions>? Text { get; set; }

    }
}
