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
                if (e.Data == "3")
                {
                    _ws.Send("2");
                    return;
                }
                if (e.Data == "40/score,")
                {
                    _ws.Send(@"42/score,[""live_list""]");
                }
                else if (e.Data.Contains("live_list"))
                {
                    var gameJson = JArray.Parse(e.Data.Replace("42/score,", ""))[1];
                    foreach (var game in gameJson)
                    {
                        try
                        {
                            var key = game["battle_id"].ToString();
                            if (ActiveGames.ContainsKey(key))
                            {
                                continue;
                            }
                            var league = game["league_name"].ToString();
                            var bestOf = game["bo_num"].ToString();
                            LiveEvent liveEvent = new LiveEvent();
                            liveEvent.LeagueName = league;
                            liveEvent.BestOf = int.Parse(bestOf);
                            liveEvent.HomeTeam = new LiveTeam();
                            liveEvent.AwayTeam = new LiveTeam();
                            liveEvent.HomeTeam.TeamName = game["team_a"]["team_name"].ToString();
                            liveEvent.HomeTeam.WinsInSeries = int.Parse(game["team_a"]["score"].ToString());
                            liveEvent.AwayTeam.TeamName = game["team_b"]["team_name"].ToString();
                            liveEvent.AwayTeam.WinsInSeries = int.Parse(game["team_b"]["score"].ToString());
                            ActiveGames.Add(key, liveEvent);
                        }
                        catch (Exception ex)
                        {

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
