using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlchiwebApp.AntD.Editors.Abstractions.Models.Data
{
    /// <summary>
    /// Enum of supported embed services.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EmbedServiceEnum
    {
        /// <summary>
        /// Facebook
        /// </summary>
        [JsonStringEnumMemberName("facebook")]
        Facebook = 0,
        /// <summary>
        /// Instagram
        /// </summary>
        [JsonStringEnumMemberName("instagram")]
        Instagram,
        /// <summary>
        /// YouTube
        /// </summary>
        [JsonStringEnumMemberName("youtube")]
        YouTube,
        /// <summary>
        /// X (formerly Twitter)
        /// </summary>
        [JsonStringEnumMemberName("twitter")]
        Twitter,
        /// <summary>
        /// Twitch Channel
        /// </summary>
        /// <remarks>
        /// Both channel and video are supported, but must be specified separately
        /// </remarks>
        [JsonStringEnumMemberName("twitch-channel")]
        TwitchChannel,
        /// <summary>
        /// Twitch Video
        /// </summary>
        /// <remarks>
        /// Both channel and video are supported, but must be specified separately
        /// </remarks>
        [JsonStringEnumMemberName("twitch-video")]
        TwitchVideo,
        /// <summary>
        /// Miro
        /// </summary>
        [JsonStringEnumMemberName("miro")]
        Miro,
        /// <summary>
        /// Vimeo
        /// </summary>
        [JsonStringEnumMemberName("vimeo")]
        Vimeo,
        /// <summary>
        /// Gfycat
        /// </summary>
        [JsonStringEnumMemberName("gfycat")]
        Gfycat,
        /// <summary>
        /// Imgur
        /// </summary>
        [JsonStringEnumMemberName("imgur")]
        Imgur,
        /// <summary>
        /// Vine
        /// </summary>
        [JsonStringEnumMemberName("vine")]
        Vine,
        /// <summary>
        /// Aparat
        /// </summary>
        [JsonStringEnumMemberName("aparat")]
        Aparat,
        /// <summary>
        /// Yandex Music Album
        /// </summary>
        [JsonStringEnumMemberName("yandex-music-album")]
        YandexMusicAlbum,
        /// <summary>
        /// Yandex Music Track
        /// </summary>
        [JsonStringEnumMemberName("yandex-music-track")]
        YandexMusicTrack,
        /// <summary>
        /// Yandex Music Playlist
        /// </summary>
        [JsonStringEnumMemberName("yandex-music-playlist")]
        YandexMusicPlaylist,
        /// <summary>
        /// Coub
        /// </summary>
        [JsonStringEnumMemberName("coub")]
        Coub,
        /// <summary>
        /// CodePen
        /// </summary>
        [JsonStringEnumMemberName("codepen")]
        CodePen,
        /// <summary>
        /// Pinterest
        /// </summary>
        [JsonStringEnumMemberName("pinterest")]
        Pinterest,
        /// <summary>
        /// Github Gist
        /// </summary>
        [JsonStringEnumMemberName("github")]
        GithubGist,
    }
}
