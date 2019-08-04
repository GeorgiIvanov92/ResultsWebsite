using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using RabbitMQ.TransportObject;
using WebSocketSharp;
using Timer = System.Threading.Timer;

namespace Tracker.Sites.Zplay
{
    public class SocketConnection 
    {
        private WebSocket _ws;
        public bool isSocketActive;
        private bool _shouldGetGameIds = false;
        public Dictionary<string,LiveEvent> ActiveGames;
        private readonly Timer _timer;
        private const string placeHolderString = @"""{0}"":(.+?).""";
        private Regex homeTeamRegex = new Regex(@"""team_a"":{""battle_odd"".+?team_name"":""(.+?)""", RegexOptions.Compiled | RegexOptions.Multiline);
        private Regex awayTeamRegex = new Regex(@"""team_b"":{""battle_odd"".+?team_name"":""(.+?)""", RegexOptions.Compiled | RegexOptions.Multiline);
        private Regex placeHolderRegex;
        public SocketConnection()
        {
            ActiveGames = new Dictionary<string, LiveEvent>();
            _ws = new WebSocket("wss://v2api.1zplay.com/socket.io/?EIO=3&transport=websocket");
            _ws.OnClose += _socket_OnClose;
            _ws.OnError += _socket_OnError;
            _ws.OnMessage += _socketOnMessageReceived;

            _ws.OnOpen += _socket_OnOpen;
            _ws.Connect();
            _timer = new Timer(SocketSendPing, null, 25000, 25000);
        }
        private void SocketSendPing(object source)
        {
            if(_ws != null && _ws.IsAlive && isSocketActive)
            {
                _ws.Send("2");
            }
        }

        private void _socket_OnOpen(object sender, EventArgs e)
        {
            _ws.Send("40/score,");
            isSocketActive = true;
        }

        private void _socketOnMessageReceived(object sender, MessageEventArgs e)
        {
            try
            {
                if (e.Data == "40/score,")
                {
                    _ws.Send(@"42/score,[""live_list""]");
                }
                else if (e.Data.Contains("live_list"))
                {
                    placeHolderRegex = new Regex(string.Format(placeHolderString,"battle_id"), RegexOptions.Multiline | RegexOptions.Compiled);
                    var homeTeamMatches = homeTeamRegex.Matches(e.Data);
                    var awayTeamMatches = awayTeamRegex.Matches(e.Data);
                    var battleIdsMatches = placeHolderRegex.Matches(e.Data);
                    if (homeTeamMatches.Count == awayTeamMatches.Count && awayTeamMatches.Count == battleIdsMatches.Count)
                    {
                        for (int i = 0; i < homeTeamMatches.Count; i++)
                        {
                            var homeTeamGroup = homeTeamMatches[i].Groups[1];
                            var awayTeamGroup = awayTeamMatches[i].Groups[1];
                            var battleIdsGroup = battleIdsMatches[i].Groups[1];
                            if (homeTeamGroup != null && awayTeamGroup != null && battleIdsGroup != null)
                            {
                                string key = battleIdsGroup.Value;
                                if (ActiveGames.ContainsKey(key))
                                {
                                    continue;
                                }
                                LiveEvent liveEvent = new LiveEvent();
                                liveEvent.HomeTeam = new LiveTeam();
                                liveEvent.AwayTeam = new LiveTeam();
                                liveEvent.HomeTeam.TeamName = homeTeamGroup.Value;
                                liveEvent.AwayTeam.TeamName = awayTeamGroup.Value;  
                                ActiveGames.Add(key,liveEvent);
                                // ActiveGames.Add($"{match.Groups[1].Value}@{match.Groups[2].Value}@{match.Groups[3].Value}");
                            }
                        }
                    }
                }             
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void _socket_OnError(object sender, ErrorEventArgs e)
        {
            isSocketActive = false;
            _ws.Close();           
        }

        private void _socket_OnClose(object sender, CloseEventArgs e)
        {
            isSocketActive = false;
            _ws.Close();
        }
    }
}
