using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Victoria;
using Victoria.Enums;
using OWuffel.Services;
using Victoria.Payloads;
using SpotifyAPI.Web;
using Victoria.Responses.Rest;

namespace OWuffel.Modules
{
    public sealed class AudioModule : ModuleBase<Cipska>
    {
        private readonly LavaNode _lavaNode;
        private SpotifyClient Spotify;
        private string Continue;


        public AudioModule(LavaNode lavaNode)
        {
            Continue = "yes";
            _lavaNode = lavaNode;
            var config = SpotifyClientConfig
                .CreateDefault()
                .WithAuthenticator(new ClientCredentialsAuthenticator("9fad3f787c754a348c6d574472645f6a", "efe43c48dab54c36aa624255840bae19"));

            Spotify = new SpotifyClient(config);
        }

        [Command("Join")]
        public async Task JoinAsync(bool announce = true)
        {
            if (_lavaNode.HasPlayer(Context.Guild))
            {
                await ReplyAsync("I'm already connected to a voice channel!");
                return;
            }

            var voiceState = Context.User as IVoiceState;
            if (voiceState?.VoiceChannel == null)
            {
                await ReplyAsync("You must be connected to a voice channel!");
                return;
            }

            try
            {
                var channelPerms = Context.Guild.CurrentUser.GetPermissions(voiceState.VoiceChannel);
                if (channelPerms.Connect)
                {
                    await _lavaNode.JoinAsync(voiceState.VoiceChannel, Context.Channel as ITextChannel);
                    if (announce) await ReplyAsync($"Joined {voiceState.VoiceChannel.Name}!");
                    if (!channelPerms.Speak) await ReplyAsync("I dont have permission to speak on this channel.");

                }
                else
                {
                    await ReplyAsync("I dont have permission to join your channel.");
                }
            }
            catch (Exception exception)
            {
                await ReplyAsync(exception.Message);
            }
        }

        [Command("Leave")]
        public async Task LeaveAsync()
        {
            if (!_lavaNode.HasPlayer(Context.Guild))
            {
                await ReplyAsync("I'm not connected to a voice channel!");
                return;
            }

            var voiceState = Context.User as IVoiceState;
            if (voiceState?.VoiceChannel == null)
            {
                await ReplyAsync("You must be connected to a voice channel!");
                return;
            }

            if (voiceState.VoiceChannel != _lavaNode.GetPlayer(Context.Guild).VoiceChannel)
            {
                await ReplyAsync("You must be connected to the same channel as bot.");
                return;
            }

            try
            {
                await _lavaNode.LeaveAsync(voiceState.VoiceChannel);
                await ReplyAsync($"Left {voiceState.VoiceChannel.Name}!");
            }
            catch (Exception exception)
            {
                await ReplyAsync(exception.Message);
            }
        }

        [Command("Search", RunMode = RunMode.Async)]
        [RequireOwner]
        public async Task Search([Remainder] string arg = "")
        {
            var link = Uri.IsWellFormedUriString(arg, UriKind.RelativeOrAbsolute);
            var player = _lavaNode.GetPlayer(Context.Guild);

            if (link)
            {
                var linktype = arg.Contains("spotify") ? "Spotify" : "Youtube";

                switch (linktype)
                {
                    case "Spotify":
                        var type = "track";
                        string[] searchid;
                        FullTrack Track = null;
                        FullPlaylist Playlist = null;
                        if (arg.Contains("https://open.spotify.com/playlist/"))
                        {
                            type = "playlist";
                            searchid = arg.Replace("https://open.spotify.com/playlist/", "").Split("?si=");
                            Playlist = await Spotify.Playlists.Get(searchid[0]);
                        }
                        else
                        {
                            searchid = arg.Replace("https://open.spotify.com/track/", "").Split("?si=");
                            Track = await Spotify.Tracks.Get(searchid[0]);
                        }

                        //jest trackiem
                        if (Track != null)
                        {
                            //var searchResponse = await _lavaNode.SearchYouTubeAsync(Track.Artists[0].Name + " " + Track.Name);

                        }
                        else if (Playlist != null)
                        {
                            foreach (PlaylistTrack<IPlayableItem> item in Playlist.Tracks.Items)
                            {
                                if (item.Track is FullTrack track)
                                {
                                    Console.WriteLine(Continue);
                                    Console.WriteLine(track.Name);
                                    var searchResponse = await _lavaNode.SearchYouTubeAsync(track.Artists[0].Name + " " + track.Name);
                                    Console.WriteLine(track.Name);
                                    //player.Queue.Enqueue(searchResponse.Tracks[0]);
                                }

                            }
                            Console.WriteLine("done");
                        }
                        break;

                    default:
                        break;
                }
            }

        }
        [Command("dick")]
        [RequireOwner]
        public async Task Dick(string arg)
        {
            Console.WriteLine(Continue);
            Continue = "tak";
            Console.WriteLine(Continue);
        }

        [Command("Play", RunMode = RunMode.Async)]
        public async Task PlayAsync([Remainder] string searchQuery = null)
        {

            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                await ReplyAsync("Please provide search terms.");
                return;
            }

            var voiceState = Context.User as IVoiceState;
            if (!_lavaNode.HasPlayer(Context.Guild))
            {
                if (voiceState.VoiceChannel == null)
                {
                    await ReplyAsync("You must be connected to voice channel.");
                    return;
                }
                await JoinAsync(false);
                if (!_lavaNode.HasPlayer(Context.Guild)) return;
            }
            var player = _lavaNode.GetPlayer(Context.Guild);

            var link = Uri.IsWellFormedUriString(searchQuery, UriKind.RelativeOrAbsolute);
            Victoria.Responses.Rest.SearchResponse searchResponse;
            List<LavaTrack> Tracks = new List<LavaTrack>();
            string PlaylistName = "";
            if (link == true)
            {
                searchResponse = await _lavaNode.SearchAsync(searchQuery);
                if (searchResponse.LoadStatus == LoadStatus.LoadFailed || searchResponse.LoadStatus == LoadStatus.NoMatches)
                {
                    string[] cutstring;
                    if (searchQuery.Contains("https://open.spotify.com/playlist/"))
                    {
                        cutstring = searchQuery.Replace("https://open.spotify.com/playlist/", "").Split("?si=");
                        var playlist = await Spotify.Playlists.Get(cutstring[0]);
                        if (playlist == null)
                        {
                            await ReplyAsync($"I wasn't able to find anything for `{searchQuery}`.");
                            return;
                        }
                        PlaylistName = playlist.Name;
                        foreach (PlaylistTrack<IPlayableItem> item in playlist.Tracks.Items)
                        {
                            if (item.Track is FullTrack track)
                            {
                                searchResponse = await _lavaNode.SearchYouTubeAsync(track.Artists[0].Name + " " + track.Name);
                                Tracks.Add(searchResponse.Tracks[0]);
                                if (searchResponse.LoadStatus == LoadStatus.LoadFailed || searchResponse.LoadStatus == LoadStatus.NoMatches)
                                {
                                    await ReplyAsync($"I wasn't able to find anything for `{track.Name}`.");
                                    return;
                                }
                            }
                        }
                    }
                    else if (searchQuery.Contains("https://open.spotify.com/track/"))
                    {
                        cutstring = searchQuery.Replace("https://open.spotify.com/track/", "").Split("?si=");
                        var SpotifyTrack = await Spotify.Tracks.Get(cutstring[0]);
                        if (SpotifyTrack == null)
                        {
                            await ReplyAsync($"I wasn't able to find anything for `{searchQuery}`.");
                            return;
                        }
                        searchResponse = await _lavaNode.SearchYouTubeAsync(SpotifyTrack.Artists[0].Name + " " + SpotifyTrack.Name);
                        if (searchResponse.LoadStatus == LoadStatus.LoadFailed || searchResponse.LoadStatus == LoadStatus.NoMatches)
                        {
                            await ReplyAsync($"I wasn't able to find anything for `{SpotifyTrack.Name}`.");
                            return;
                        }
                    }
                }
            }
            else
            {
                searchResponse = await _lavaNode.SearchYouTubeAsync(searchQuery);
                if (searchResponse.LoadStatus == LoadStatus.LoadFailed || searchResponse.LoadStatus == LoadStatus.NoMatches)
                {
                    await ReplyAsync($"I wasn't able to find anything for `{searchQuery}`.");
                    return;
                }
            }

            if (player.PlayerState == PlayerState.Playing || player.PlayerState == PlayerState.Paused)
            {
                if (!string.IsNullOrWhiteSpace(searchResponse.Playlist.Name) || !string.IsNullOrWhiteSpace(PlaylistName))
                {
                    if (searchResponse.Tracks.Count > 0 && Tracks.Count == 0)
                    {
                        foreach (var track in searchResponse.Tracks)
                        {
                            player.Queue.Enqueue(track);
                        }

                        await ReplyAsync($"Enqueued {searchResponse.Tracks.Count} tracks.");
                    }
                    else if (Tracks.Count > 0)
                    {
                        foreach (var track in Tracks)
                        {
                            player.Queue.Enqueue(track);
                        }

                        await ReplyAsync($"Enqueued {Tracks.Count} tracks.");
                    }
                }
                else
                {
                    var track = searchResponse.Tracks[0];
                    player.Queue.Enqueue(track);
                    await ReplyAsync($"Enqueued: {track.Title}");
                }
            }
            else
            {
                var track = searchResponse.Tracks.Count > 0 && Tracks.Count == 0 ? searchResponse.Tracks[0] : Tracks[0];

                if (!string.IsNullOrWhiteSpace(searchResponse.Playlist.Name))
                {
                    for (var i = 0; i < searchResponse.Tracks.Count; i++)
                    {
                        if (i == 0)
                        {
                            await player.PlayAsync(searchResponse.Tracks[i]);
                            await ReplyAsync($"Now Playing: {searchResponse.Tracks[i].Title}");
                        }
                        else
                        {
                            player.Queue.Enqueue(searchResponse.Tracks[i]);
                        }
                    }

                    await ReplyAsync($"Enqueued {searchResponse.Tracks.Count} tracks.");
                }
                else if (!string.IsNullOrWhiteSpace(PlaylistName))
                {
                    for (var i = 0; i < Tracks.Count; i++)
                    {
                        if (i == 0)
                        {
                            await player.PlayAsync(Tracks[i]);
                            await ReplyAsync($"Now Playing: {Tracks[i].Title}");
                        }
                        else
                        {
                            player.Queue.Enqueue(Tracks[i]);
                        }
                    }

                    await ReplyAsync($"Enqueued {Tracks.Count} tracks.");
                }
                else
                {
                    await player.PlayAsync(searchResponse.Tracks[0]);
                    await ReplyAsync($"Now Playing: {searchResponse.Tracks[0].Title}");
                }
            }
        }

        [Command("Pause")]
        [Alias("Stop")]
        public async Task PauseTrack()
        {
            if (!_lavaNode.HasPlayer(Context.Guild))
            {
                await ReplyAsync("I'm not connected to voice channel!");
                return;
            }
            var player = _lavaNode.GetPlayer(Context.Guild);

            var userVoice = Context.User as IVoiceState;
            if (userVoice.VoiceChannel == null)
            {
                await ReplyAsync("You must be connected to voice channel.");
                return;
            }
            if (player.Track.Url == null)
            {
                await ReplyAsync("Im not playing anything.");
                return;
            }
            if (player.PlayerState == PlayerState.Playing)
            {
                await player.PauseAsync();
                await ReplyAsync($"Paused the player");
            }
            else if (player.PlayerState == PlayerState.Paused)
            {
                await player.ResumeAsync();
                await ReplyAsync($"Resumed playing: {player.Track.Title}");
            }
        }

        [Command("Skip")]
        public async Task SkipTrack(int toskip = 1)
        {
            if (!_lavaNode.HasPlayer(Context.Guild))
            {
                await ReplyAsync("I'm not connected to voice channel!");
                return;
            }
            var player = _lavaNode.GetPlayer(Context.Guild);

            var userVoice = Context.User as IVoiceState;
            if (userVoice.VoiceChannel == null)
            {
                await ReplyAsync("You must be connected to voice channel.");
                return;
            }
            if (player.PlayerState == PlayerState.Playing || player.PlayerState == PlayerState.Paused)
            {
                try
                {
                    await player.SkipAsync();
                }
                catch (Exception)
                {
                    await player.StopAsync();
                }

                await ReplyAsync($"Skipped: {player.Track.Title}");
            }
        }
        [Command("Volume")]
        public async Task ChangeVolumeAsync(ushort volume)
        {
            if (!_lavaNode.HasPlayer(Context.Guild))
            {
                await ReplyAsync("I'm not connected to voice channel!");
                return;
            }
            var player = _lavaNode.GetPlayer(Context.Guild);

            var userVoice = Context.User as IVoiceState;
            if (userVoice.VoiceChannel == null)
            {
                await ReplyAsync("You must be connected to voice channel.");
                return;
            }
            if (player.PlayerState == PlayerState.Playing)
            {
                await player.UpdateVolumeAsync(volume);
                await ReplyAsync($"Changed volume to {volume}");
            }
            else
            {
                await ReplyAsync("I playing anything right now.");
            }

        }
        [Command("Equalizer")]
        public async Task ChangeEqualizerAsync(short eq, short value)
        {
            var player = _lavaNode.GetPlayer(Context.Guild);
            EqualizerBand equalizer = new EqualizerBand(eq, value);
            if (player.PlayerState == PlayerState.Playing)
            {
                await player.EqualizerAsync(new EqualizerBand(eq, value));
            };
        }
        [Command("Shuffle")]
        public async Task ShuffleQueue()
        {
            if (!_lavaNode.HasPlayer(Context.Guild))
            {
                await ReplyAsync("I'm not connected to voice channel!");
                return;
            }
            var player = _lavaNode.GetPlayer(Context.Guild);

            var userVoice = Context.User as IVoiceState;
            if (userVoice.VoiceChannel == null)
            {
                await ReplyAsync("You must be connected to voice channel.");
                return;
            }
            if (player.PlayerState == PlayerState.Stopped || player.Queue.Count < 2)
            {
                await ReplyAsync("There is nothing to shuffle.");
                return;
            }
            if (player.PlayerState == PlayerState.Playing || player.PlayerState == PlayerState.Paused)
            {
                player.Queue.Shuffle();
                await ReplyAsync($"Shuffled queue. Next in queue: {player.Queue.Peek().Title}");
            }

        }

        [Command("Clear")]
        [Alias("Destroy")]
        public async Task ClearQueueAsync()
        {
            if (!_lavaNode.HasPlayer(Context.Guild))
            {
                await ReplyAsync("I'm not connected to voice channel!");
                return;
            }
            var player = _lavaNode.GetPlayer(Context.Guild);

            var userVoice = Context.User as IVoiceState;
            if (userVoice.VoiceChannel == null)
            {
                await ReplyAsync("You must be connected to voice channel.");
                return;
            }
            if (player.Queue.Count > 1 && (player.PlayerState == PlayerState.Playing || player.PlayerState == PlayerState.Paused))
            {
                player.Queue.Clear();
                await ReplyAsync("Cleared queue.");
            }
        }

        [Command("np")]
        public async Task NowPlayingAsync()
        {
            if (!_lavaNode.HasPlayer(Context.Guild))
            {
                await ReplyAsync("I'm not connected to voice channel!");
                return;
            }
            var player = _lavaNode.GetPlayer(Context.Guild);

            var userVoice = Context.User as IVoiceState;
            if (userVoice.VoiceChannel == null)
            {
                await ReplyAsync("You must be connected to voice channel.");
                return;
            }
            if (player.PlayerState == PlayerState.Playing)
            {
                await ReplyAsync($"Now Playing: {player.Track.Url}");
            }
            else if (player.PlayerState == PlayerState.Paused)
            {
                await ReplyAsync($"Player is paused. Last played track: {player.Track.Url}");
            }
        }

    }
}
