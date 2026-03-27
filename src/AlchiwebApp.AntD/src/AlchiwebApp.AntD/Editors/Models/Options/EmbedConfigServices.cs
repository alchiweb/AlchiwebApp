using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlchiwebApp.AntD.Editors.Models.Options
{
    /// <summary>
    /// Embed configuration for enabling or disabling specific services.
    /// </summary>
    public class EmbedConfigServices
    {
        /// <summary>
        /// Enables or disables Facebook embedding.
        /// </summary>
        [JsonPropertyName("facebook")]
        public bool? Facebook { get; set; }
        /// <summary>
        /// Enables or disables Instagram embedding.
        /// </summary>
        [JsonPropertyName("instagram")]
        public bool? Instagram { get; set; }
        /// <summary>
        /// Enables or disables YouTube embedding.
        /// </summary>
        [JsonPropertyName("youtube")]
        public bool? YouTube { get; set; }
        /// <summary>
        /// Enables or disables Twitter (X) embedding.
        /// </summary>
        [JsonPropertyName("twitter")]
        public bool? Twitter { get; set; }
        /// <summary>
        /// Enables or disables Twitch channel embedding.
        /// </summary>
        [JsonPropertyName("twitch-channel")]
        public bool? TwitchChannel { get; set; }
        /// <summary>
        /// Enables or disables Twitch video embedding.
        /// </summary>
        [JsonPropertyName("twitch-video")]
        public bool? TwitchVideo { get; set; }
        /// <summary>
        /// Enables or disables Miro embedding.
        /// </summary>
        [JsonPropertyName("miro")]
        public bool? Miro { get; set; }
        /// <summary>
        /// Enables or disables Vimeo embedding.
        /// </summary>
        [JsonPropertyName("vimeo")]
        public bool? Vimeo { get; set; }
        /// <summary>
        /// Enables or disables Dailymotion embedding.
        /// </summary>
        [JsonPropertyName("gfycat")]
        public bool? Gfycat { get; set; }
        /// <summary>
        /// Enables or disables Giphy embedding.
        /// </summary>
        [JsonPropertyName("imgur")]
        public bool? Imgur { get; set; }
        /// <summary>
        /// Enables or disables Imgur embedding.
        /// </summary>
        [JsonPropertyName("Vine")]
        public bool? Vine { get; set; }
        /// <summary>
        /// Enables or disables Aparat embedding.
        /// </summary>
        [JsonPropertyName("aparat")]
        public bool? Aparat { get; set; }
        /// <summary>
        /// Enables or disables Yandex Music Album embedding.
        /// </summary>
        [JsonPropertyName("yandex-music-album")]
        public bool? YandexMusicAlbum { get; set; }
        /// <summary>
        /// Enables or disables Yandex Music Track embedding.
        /// </summary>
        [JsonPropertyName("yandex-music-track")]
        public bool? YandexMusicTrack { get; set; }
        /// <summary>
        /// Enables or disables Yandex Music Playlist embedding.
        /// </summary>
        [JsonPropertyName("yandex-music-playlist")]
        public bool? YandexMusicPlaylist { get; set; }
        /// <summary>
        /// Enables or disables Coub embedding.
        /// </summary>
        [JsonPropertyName("coub")]
        public bool? Coub { get; set; }
        /// <summary>
        /// Enables or disables CodePen embedding.
        /// </summary>
        [JsonPropertyName("codepen")]
        public bool? CodePen { get; set; }
        /// <summary>
        /// Enables or disables Pinterest embedding.
        /// </summary>
        [JsonPropertyName("pinterest")]
        public bool? Pinterest { get; set; }
        /// <summary>
        /// Enables or disables Reddit embedding.
        /// </summary>
        [JsonPropertyName("github")]
        public bool? GithubGist { get; set; }
    }
}
