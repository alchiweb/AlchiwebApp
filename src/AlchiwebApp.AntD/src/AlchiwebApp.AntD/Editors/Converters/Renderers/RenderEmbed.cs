using AlchiwebApp.AntD.Editors.Abstractions.Models;
using AlchiwebApp.AntD.Editors.Abstractions.Models.Data;
using AntDesign;

namespace AlchiwebApp.AntD.Editors.Converters.Renderers;

/// <summary>
/// Embed block renderer.
/// </summary>
public sealed class RenderEmbed : IBlockRenderer<EmbedData>
{
    /// <summary>
    /// Renders the "Embed" block.
    /// </summary>
    /// <param name="render_tree_builder">The custom render tree builder.</param>
    /// <param name="block">The EditorJs block data.</param>
    public static void Render(EditorJsRenderTreeBuilder render_tree_builder, EditorJsBlock<EmbedData> block)
    {
        string id = block.Id;
        EmbedServiceEnum? service = block.Data?.Service;
        string? source = block.Data?.Source;
        string? embed = block.Data?.Embed;
        int width = block.Data?.Width ?? 0;
        int height = block.Data?.Height ?? 0;
        string? caption = block.Data?.Caption;

        // Render embed
        render_tree_builder.Builder.OpenElement(render_tree_builder.SequenceCounter, "div");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "id", id);

        EditorJsStylingMap? css = render_tree_builder.StylingMap.FirstOrDefault(item => item.Type == BlockTypeEnum.Embed && item.Id == id);
        css ??= render_tree_builder.StylingMap.FirstOrDefault(item => item.Type == BlockTypeEnum.Embed && item.Id == null);

        if (css is not null)
        {
            render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "class", css.Style);
        }

        // Add embed source based on the service
        switch (service)
        {
            case EmbedServiceEnum.Vimeo:
                RenderVimeoEmbed(render_tree_builder, source, width, height);
                break;
            case EmbedServiceEnum.YouTube:
                RenderYouTubeEmbed(render_tree_builder, source, width, height);
                break;
            case EmbedServiceEnum.Coub:
                RenderCoubEmbed(render_tree_builder, source, width, height);
                break;
            case EmbedServiceEnum.Facebook:
                RenderFacebookEmbed(render_tree_builder, source, width, height);
                break;
            case EmbedServiceEnum.Instagram:
                RenderInstagramEmbed(render_tree_builder, source, width, height);
                break;
            case EmbedServiceEnum.Twitter:
                RenderTwitterEmbed(render_tree_builder, source, width, height);
                break;
            case EmbedServiceEnum.TwitchChannel:
                RenderTwitchChannelEmbed(render_tree_builder, source, width, height);
                break;
            case EmbedServiceEnum.TwitchVideo:
                RenderTwitchVideoEmbed(render_tree_builder, source, width, height);
                break;
            case EmbedServiceEnum.Miro:
                RenderMiroEmbed(render_tree_builder, source, width, height);
                break;
            case EmbedServiceEnum.Gfycat:
                RenderGfycatEmbed(render_tree_builder, source, width, height);
                break;
            case EmbedServiceEnum.Imgur:
                RenderImgurEmbed(render_tree_builder, source, width, height);
                break;
            case EmbedServiceEnum.Vine:
                RenderVineEmbed(render_tree_builder, source, width, height);
                break;
            case EmbedServiceEnum.Aparat:
                RenderAparatEmbed(render_tree_builder, source, width, height);
                break;
            case EmbedServiceEnum.CodePen:
                RenderCodePenEmbed(render_tree_builder, source, width, height);
                break;
            case EmbedServiceEnum.Pinterest:
                RenderPinterestEmbed(render_tree_builder, source);
                break;
            case EmbedServiceEnum.GithubGist:
                RenderGitHubGistEmbed(render_tree_builder, source);
                break;
            case EmbedServiceEnum.YandexMusicAlbum:
                RenderYandexMusicAlbumEmbed(render_tree_builder, source);
                break;
            case EmbedServiceEnum.YandexMusicTrack:
                RenderYandexMusicTrackEmbed(render_tree_builder, source);
                break;
            case EmbedServiceEnum.YandexMusicPlaylist:
                RenderYandexMusicPlaylistEmbed(render_tree_builder, source);
                break;
            // Add other services as needed
            default:
                // Handle unknown or unsupported service
                break;
        }

        render_tree_builder.Builder.CloseElement(); // Close the div element

        // Render caption
        if (!string.IsNullOrEmpty(caption))
        {
            render_tree_builder.Builder.OpenElement(render_tree_builder.SequenceCounter, "p");
            render_tree_builder.Builder.AddMarkupContent(render_tree_builder.SequenceCounter, caption);
            render_tree_builder.Builder.CloseElement(); // Close the p element
        }
    }

    private static void RenderYandexMusicAlbumEmbed(EditorJsRenderTreeBuilder render_tree_builder, string? source)
    {
        string albumId = GetYandexMusicIdFromSource(source);

        render_tree_builder.Builder.OpenElement(render_tree_builder.SequenceCounter, "iframe");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "frameborder", "0");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "style", "border:none;width:540px;height:400px;");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "width", "100%");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "height", "400");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "src", $"https://music.yandex.ru/iframe/#album/{albumId}/");
        render_tree_builder.Builder.CloseElement(); // Close the iframe element
    }

    // Helper method to render Yandex.Music track embed
    private static void RenderYandexMusicTrackEmbed(EditorJsRenderTreeBuilder render_tree_builder, string? source)
    {
        string[] ids = GetYandexMusicIdsFromSource(source);

        render_tree_builder.Builder.OpenElement(render_tree_builder.SequenceCounter, "iframe");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "frameborder", "0");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "style", "border:none;width:540px;height:100px;");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "width", "100%");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "height", "100");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "src", $"https://music.yandex.ru/iframe/#track/{string.Join('/', ids)}/");
        render_tree_builder.Builder.CloseElement(); // Close the iframe element
    }

    // Helper method to render Yandex.Music playlist embed
    private static void RenderYandexMusicPlaylistEmbed(EditorJsRenderTreeBuilder render_tree_builder, string? source)
    {
        string[] ids = GetYandexMusicIdsFromSource(source);

        render_tree_builder.Builder.OpenElement(render_tree_builder.SequenceCounter, "iframe");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "frameborder", "0");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "style", "border:none;width:540px;height:400px;");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "width", "100%");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "height", "400");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "src", $"https://music.yandex.ru/iframe/#playlist/{string.Join('/', ids)}/show/cover/description/");
        render_tree_builder.Builder.CloseElement(); // Close the iframe element
    }

    // Helper method to extract Yandex.Music IDs from source URL
    private static string GetYandexMusicIdFromSource(string? source)
    {
        // Implement logic to extract Yandex.Music album ID from the source URL
        // Example: https://music.yandex.ru/album/12345
        if (string.IsNullOrWhiteSpace(source))
        {
            return "";
        }

        Uri uri = new(source);
        string[] segments = uri.Segments;
        return segments.Length >= 2 ? segments[^1].TrimEnd('/') : "";
    }

    // Helper method to extract Yandex.Music IDs from source URL for tracks and playlists
    private static string[] GetYandexMusicIdsFromSource(string? source)
    {
        // Implement logic to extract Yandex.Music track or playlist IDs from the source URL
        // Example for track: https://music.yandex.ru/album/12345/track/67890
        // Example for playlist: https://music.yandex.ru/users/username/playlists/12345
        if (string.IsNullOrWhiteSpace(source))
        {
            return Enumerable.Empty<string>().ToArray();
        }

        Uri uri = new(source);
        string[] segments = uri.Segments;
        List<string> ids = [];

        for (int i = 2; i < segments.Length; i++)
        {
            if (!string.IsNullOrWhiteSpace(segments[i]))
            {
                ids.Add(segments[i].TrimEnd('/'));
            }
        }

        return [.. ids];
    }

    private static void RenderGitHubGistEmbed(EditorJsRenderTreeBuilder render_tree_builder, string? source)
    {
        string gistId = GetGitHubGistIdFromSource(source);

        render_tree_builder.Builder.OpenElement(render_tree_builder.SequenceCounter, "iframe");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "width", "100%");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "height", "350");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "frameborder", "0");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "style", "margin: 0 auto;");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "src", $"data:text/html;charset=utf-8,<head><base target=\"_blank\" /></head><body><script src=\"https://gist.github.com/{gistId}.js\" ></script></body>");
        render_tree_builder.Builder.CloseElement(); // Close the iframe element
    }

    // Helper method to extract GitHub Gist ID from source URL
    private static string GetGitHubGistIdFromSource(string? source)
    {
        // Implement logic to extract GitHub Gist ID from the source URL
        // Assuming source is a valid GitHub Gist URL
        // Example: https://gist.github.com/username/gistId
        if (string.IsNullOrWhiteSpace(source))
        {
            return "";
        }

        Uri uri = new(source);
        string[] segments = uri.Segments;
        return segments.Length >= 2 ? segments[^1] : "";
    }

    private static void RenderPinterestEmbed(EditorJsRenderTreeBuilder render_tree_builder, string? source)
    {
        string pinId = GetPinterestPinIdFromSource(source);

        render_tree_builder.Builder.OpenElement(render_tree_builder.SequenceCounter, "iframe");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "scrolling", "no");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "frameborder", "no");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "allowtransparency", "true");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "allowfullscreen", "true");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "style", "width: 100%; min-height: 400px; max-height: 1000px;");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "src", $"https://assets.pinterest.com/ext/embed.html?id={pinId}");
        render_tree_builder.Builder.CloseElement(); // Close the iframe element
    }

    // Helper method to extract Pinterest Pin ID from source URL
    private static string GetPinterestPinIdFromSource(string? source)
    {
        // Implement logic to extract Pinterest Pin ID from the source URL
        // Assuming source is a valid Pinterest Pin URL in the format mentioned in the JavaScript code
        // Example: https://www.pinterest.com/pin/123456789/
        if (string.IsNullOrWhiteSpace(source))
        {
            return "";
        }

        Uri uri = new(source);
        return uri.Segments.Last().Trim('/');
    }

    private static void RenderCodePenEmbed(EditorJsRenderTreeBuilder render_tree_builder, string? source, int width, int height)
    {
        string[] ids = GetCodePenIdsFromSource(source);

        render_tree_builder.Builder.OpenElement(render_tree_builder.SequenceCounter, "iframe");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "height", height);
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "scrolling", "no");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "frameborder", "no");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "allowtransparency", "true");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "allowfullscreen", "true");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "style", "width: 100%;");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "src", $"https://codepen.io/{ids[0]}/embed/{ids[1]}?height={height}&theme-id=0&default-tab=css,result&embed-version=2");
        render_tree_builder.Builder.CloseElement(); // Close the iframe element
    }

    // Helper method to extract CodePen IDs from source URL
    private static string[] GetCodePenIdsFromSource(string? source)
    {
        // Implement logic to extract CodePen IDs from the source URL
        // Assuming source is a valid CodePen URL in the format mentioned in the JavaScript code
        // Example: https://codepen.io/username/pen/abcdef123456
        if (string.IsNullOrWhiteSpace(source))
        {
            return Enumerable.Empty<string>().ToArray();
        }

        Uri uri = new(source);
        return uri.Segments.SkipWhile(s => s.Trim('/') != "pen").Skip(1).Take(2).ToArray();
    }

    private static void RenderAparatEmbed(EditorJsRenderTreeBuilder render_tree_builder, string? source, int width, int height)
    {
        string aparatVideoId = GetAparatVideoIdFromSource(source);

        render_tree_builder.Builder.OpenElement(render_tree_builder.SequenceCounter, "iframe");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "src", $"https://www.aparat.com/video/video/embed/videohash/{aparatVideoId}/vt/frame");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "width", "100%");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "height", height);
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "style", "margin: 0 auto;");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "frameborder", "0");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "scrolling", "no");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "allowfullscreen", "true");
        render_tree_builder.Builder.CloseElement(); // Close the iframe element
    }

    // Helper method to extract Aparat video ID from source URL
    private static string GetAparatVideoIdFromSource(string? source)
    {
        // Implement logic to extract Aparat video ID from the source URL
        // Assuming source is a valid Aparat URL in the format mentioned in the JavaScript code
        // Example: https://www.aparat.com/v/abcdef123456
        if (string.IsNullOrWhiteSpace(source))
        {
            return "";
        }

        Uri uri = new(source);
        return uri.Segments.LastOrDefault()?.Trim('/') ?? "";
    }

    private static void RenderVineEmbed(EditorJsRenderTreeBuilder render_tree_builder, string? source, int width, int height)
    {
        string vineId = GetVineIdFromSource(source);

        render_tree_builder.Builder.OpenElement(render_tree_builder.SequenceCounter, "iframe");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "src", $"https://vine.co/v/{vineId}/embed/simple/");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "width", "100%");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "height", height);
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "frameborder", "0");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "allowfullscreen", "true");
        render_tree_builder.Builder.CloseElement(); // Close the iframe element
    }

    // Helper method to extract Vine ID from source URL
    private static string GetVineIdFromSource(string? source)
    {
        // Implement logic to extract Vine ID from the source URL
        // Assuming source is a valid Vine URL in the format mentioned in the JavaScript code
        // Example: https://vine.co/v/abcdef123456
        if (string.IsNullOrWhiteSpace(source))
        {
            return "";
        }

        Uri uri = new(source);
        return uri.Segments.LastOrDefault()?.Trim('/') ?? "";
    }

    private static void RenderImgurEmbed(EditorJsRenderTreeBuilder render_tree_builder, string? source, int width, int height)
    {
        string imgurId = GetImgurIdFromSource(source);

        render_tree_builder.Builder.OpenElement(render_tree_builder.SequenceCounter, "iframe");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "src", $"http://imgur.com/{imgurId}/embed");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "width", "100%");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "height", height);
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "allowfullscreen", "true");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "scrolling", "no");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "frameborder", "no");
        render_tree_builder.Builder.CloseElement(); // Close the iframe element
    }

    // Helper method to extract Imgur ID from source URL
    private static string GetImgurIdFromSource(string? source)
    {
        // Implement logic to extract Imgur ID from the source URL
        // Assuming source is a valid Imgur URL in the format mentioned in the JavaScript code
        // Example: https://imgur.com/abcdef123456
        if (string.IsNullOrWhiteSpace(source))
        {
            return "";
        }

        Uri uri = new(source);
        return uri.Segments.LastOrDefault()?.Trim('/') ?? "";
    }

    private static void RenderGfycatEmbed(EditorJsRenderTreeBuilder render_tree_builder, string? source, int width, int height)
    {
        string gfyId = GetGfycatIdFromSource(source);

        render_tree_builder.Builder.OpenElement(render_tree_builder.SequenceCounter, "iframe");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "src", $"https://gfycat.com/ifr/{gfyId}");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "width", width);
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "height", height);
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "style", "margin: 0 auto;");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "frameborder", "0");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "allowfullscreen", "true");
        render_tree_builder.Builder.CloseElement(); // Close the iframe element
    }

    // Helper method to extract Gfycat ID from source URL
    private static string GetGfycatIdFromSource(string? source)
    {
        // Implement logic to extract Gfycat ID from the source URL
        // Assuming source is a valid Gfycat URL in the format mentioned in the JavaScript code
        // Example: https://gfycat.com/gifs/detail/abcdef1234567890
        if (string.IsNullOrWhiteSpace(source))
        {
            return "";
        }

        Uri uri = new(source);
        return uri.Segments.LastOrDefault()?.Trim('/') ?? "";
    }

    private static void RenderMiroEmbed(EditorJsRenderTreeBuilder render_tree_builder, string? source, int width, int height)
    {
        string boardId = GetBoardIdFromMiroSource(source);

        render_tree_builder.Builder.OpenElement(render_tree_builder.SequenceCounter, "iframe");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "src", $"https://miro.com/app/live-embed/{boardId}");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "width", width);
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "height", height);
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "style", "margin: 0 auto;");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "allowfullscreen", "true");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "frameborder", "0");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "scrolling", "no");
        render_tree_builder.Builder.CloseElement(); // Close the iframe element
    }

    // Helper method to extract Miro Board ID from source URL
    private static string GetBoardIdFromMiroSource(string? source)
    {
        // Implement logic to extract Miro Board ID from the source URL
        // Assuming source is a valid Miro Board URL in the format mentioned in the JavaScript code
        // Example: https://miro.com/app/board/abcdef1234567890/
        if (string.IsNullOrWhiteSpace(source))
        {
            return "";
        }

        Uri uri = new(source);
        return uri.Segments.LastOrDefault()?.Trim('/') ?? "";
    }

    private static void RenderTwitchChannelEmbed(EditorJsRenderTreeBuilder render_tree_builder, string? source, int width, int height)
    {
        string channelName = GetChannelNameFromTwitchSource(source);

        render_tree_builder.Builder.OpenElement(render_tree_builder.SequenceCounter, "iframe");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "src", $"https://player.twitch.tv/?channel={channelName}");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "width", width);
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "height", height);
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "style", "margin: 0 auto;");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "frameborder", "0");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "allowfullscreen", "true");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "scrolling", "no");
        render_tree_builder.Builder.CloseElement(); // Close the iframe element
    }

    // Helper method to extract Twitch Channel name from source URL
    private static string GetChannelNameFromTwitchSource(string? source)
    {
        // Implement logic to extract Twitch Channel name from the source URL
        // Assuming source is a valid Twitch Channel URL in the format mentioned in the JavaScript code
        // Example: https://www.twitch.tv/username
        if (string.IsNullOrWhiteSpace(source))
        {
            return "";
        }

        Uri uri = new(source);
        return uri.Segments.LastOrDefault()?.Trim('/') ?? "";
    }

    // Helper method to render Twitch Video embed
    private static void RenderTwitchVideoEmbed(EditorJsRenderTreeBuilder render_tree_builder, string? source, int width, int height)
    {
        string videoId = GetVideoIdFromTwitchSource(source);

        render_tree_builder.Builder.OpenElement(render_tree_builder.SequenceCounter, "iframe");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "src", $"https://player.twitch.tv/?video=v{videoId}");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "width", width);
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "height", height);
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "style", "margin: 0 auto;");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "frameborder", "0");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "allowfullscreen", "true");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "scrolling", "no");
        render_tree_builder.Builder.CloseElement(); // Close the iframe element
    }

    // Helper method to extract Twitch Video ID from source URL
    private static string GetVideoIdFromTwitchSource(string? source)
    {
        // Implement logic to extract Twitch Video ID from the source URL
        // Assuming source is a valid Twitch Video URL in the format mentioned in the JavaScript code
        // Example: https://www.twitch.tv/username/v/1234567890123456789
        if (string.IsNullOrWhiteSpace(source))
        {
            return "";
        }

        Uri uri = new(source);
        string[] segments = uri.Segments;
        return segments.Length == 4 ? segments[3].Trim('/') : "";
    }

    private static void RenderTwitterEmbed(EditorJsRenderTreeBuilder render_tree_builder, string? source, int width, int height)
    {
        string[] ids = GetIdsFromTwitterSource(source);

        render_tree_builder.Builder.OpenElement(render_tree_builder.SequenceCounter, "iframe");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "src", $"https://twitframe.com/show?url=https://twitter.com/{ids[0]}/status/{ids[1]}");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "width", width);
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "height", height);
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "style", "margin: 0 auto;");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "frameborder", "0");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "scrolling", "no");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "allowtransparency", "true");
        render_tree_builder.Builder.CloseElement(); // Close the iframe element
    }

    // Helper method to extract Twitter IDs from source URL
    private static string[] GetIdsFromTwitterSource(string? source)
    {
        // Implement logic to extract Twitter IDs from the source URL
        // Assuming source is a valid Twitter URL in the format mentioned in the JavaScript code
        // Example: https://twitter.com/username/status/1234567890123456789
        string[] segments = source?.Split('/').Where(s => !string.IsNullOrEmpty(s)).ToArray() ?? [];
        return segments.Length == 6 ? [segments[4], segments[5]] : [];
    }

    private static void RenderInstagramEmbed(EditorJsRenderTreeBuilder render_tree_builder, string? source, int width, int height)
    {
        string remoteId = GetRemoteIdFromInstagramSource(source);

        render_tree_builder.Builder.OpenElement(render_tree_builder.SequenceCounter, "iframe");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "src", $"https://www.instagram.com/p/{remoteId}/embed");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "width", width);
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "height", height);
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "style", "margin: 0 auto;");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "frameborder", "0");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "scrolling", "no");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "allowtransparency", "true");
        render_tree_builder.Builder.CloseElement(); // Close the iframe element
    }

    // Helper method to extract Instagram remote ID from source URL
    private static string GetRemoteIdFromInstagramSource(string? source) =>
        // Implement logic to extract remote ID from Instagram source URL
        "instagram_remote_id";

    private static void RenderFacebookEmbed(EditorJsRenderTreeBuilder render_tree_builder, string? source, int width, int height)
    {
        render_tree_builder.Builder.OpenElement(render_tree_builder.SequenceCounter, "iframe");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "src", $"https://www.facebook.com/plugins/post.php?href={source}");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "width", width);
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "height", height);
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "frameborder", "0");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "style", "border:none;overflow:hidden");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "scrolling", "no");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "allowtransparency", "true");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "allowfullscreen", "true");
        render_tree_builder.Builder.CloseElement(); // Close the iframe element
    }

    // Helper method to render Coub embed
    private static void RenderCoubEmbed(EditorJsRenderTreeBuilder render_tree_builder, string? source, int width, int height)
    {
        string coubVideoId = GetCoubVideoIdFromSource(source);

        render_tree_builder.Builder.OpenElement(render_tree_builder.SequenceCounter, "iframe");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "src", $"https://coub.com/embed/{coubVideoId}");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "width", "100%");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "height", height);
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "style", "margin: 0 auto;");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "frameborder", "0");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "allowfullscreen", "true");
        render_tree_builder.Builder.CloseElement(); // Close the iframe element
    }

    // Helper method to extract Coub video ID from source URL
    private static string GetCoubVideoIdFromSource(string? source)
    {
        // Implement logic to extract Coub video ID from the source URL
        // Assuming source is a valid Coub URL in the format mentioned in the JavaScript code
        // Example: https://coub.com/view/abcdef123456
        if (string.IsNullOrWhiteSpace(source))
        {
            return "";
        }

        Uri uri = new(source);
        return uri.Segments.LastOrDefault()?.Trim('/') ?? "";
    }

    private static void RenderVimeoEmbed(EditorJsRenderTreeBuilder render_tree_builder, string? source, int width, int height)
    {
        string remoteId = GetRemoteIdFromSource(source,
            "(?:http[s]?:\\/\\/)?(?:www.)?(?:player.)?vimeo\\.co(?:.+\\/([^\\/]\\d+)(?:#t=[\\d]+)?s?$)",
            1);

        render_tree_builder.Builder.OpenElement(render_tree_builder.SequenceCounter, "iframe");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "src", $"https://player.vimeo.com/video/{remoteId}?title=0&byline=0");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "width", width);
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "height", height);
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "frameborder", "0");
        render_tree_builder.Builder.CloseElement(); // Close the iframe element
    }

    // Helper method to render YouTube embed
    private static void RenderYouTubeEmbed(EditorJsRenderTreeBuilder render_tree_builder, string? source, int width, int height)
    {
        string remoteId = GetRemoteIdFromSource(source,
            "(?:https?:\\/\\/)?(?:www\\.)?(?:(?:youtu\\.be\\/)|(?:youtube\\.com)\\/(?:v\\/|u\\/\\w\\/|embed\\/|watch))(?:(?:\\?v=)?([^#&?=]*))?((?:[?&]\\w*=\\w*)*)",
            1);

        render_tree_builder.Builder.OpenElement(render_tree_builder.SequenceCounter, "iframe");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "src", $"https://www.youtube.com/embed/{remoteId}");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "width", width);
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "height", height);
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "frameborder", "0");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "allowfullscreen", "true");
        render_tree_builder.Builder.CloseElement(); // Close the iframe element
    }

    // Helper method to extract remote ID from source URL
    private static string GetRemoteIdFromSource(string? source, string? regExStr, uint indexResult)
    {
        if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(regExStr))
        {
            return "";
        }
        // Implement logic to extract remote ID from YouTube source URL
        var regEx = new Regex(regExStr);
        var results = regEx.Split(source);
        return (results == null || results.Length < indexResult + 1) ? "" : results[indexResult];
    }
}
