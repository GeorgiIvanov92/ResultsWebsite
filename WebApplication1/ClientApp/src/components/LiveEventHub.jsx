import React, { Component } from 'react';
import * as  signalR from '@aspnet/signalr';
import { LiveGame } from './LiveGame';
//import { Observable } from '../../node_modules/rxjs';
export class LiveEventHub extends Component {
    constructor(props) {
        super(props);

        this.state = {
            nick: '',
            message: '',
            messages: [],
            liveEvents: [],
            leagueLiveEvents: [],
            dotaLiveEvents: [],
            hubConnection: null,
            pingEpic: null,
        };
        this.parseLiveEvent = this.parseLiveEvent.bind(this);
    }

    componentDidMount = () => {
        //const interval = new Observable(observer => {
        //    setInterval(() => {
        //        let liveEvents = this.state.liveEvents;
        //        let timeObject = new Date().getTime();
        //        if (liveEvents.length > 0) {
        //            let asd = liveEvents[0].updateDateInEpoch;
        //        }
        //        liveEvents = liveEvents.filter(a => a.updateDateInEpoch > timeObject);
        //        this.setState({ liveEvents: liveEvents });
        //        observer.next();
        //    }, 10000);
        //});
        //interval.subscribe();
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("/live", {
                transport: signalR.HttpTransportType.WebSockets
            })
            .configureLogging(signalR.LogLevel.Information)
            .build();
        connection.on("ReceiveMessage", this.parseLiveEvent);
        connection.start().then(function () {
        });
    }
    parseLiveEvent(user, message) {
        let liveEvents = this.state.liveEvents;
        if (user.shouldRemoveEvent === true) {

            let filteredLiveEvents = liveEvents.filter(a => a.homeTeam.teamName !== user.homeTeam.teamName &&
                a.awayTeam.teamName !== user.awayTeam.teamName);     

            let leagueEvents = filteredLiveEvents.filter(a => a.sport === 1);
            let dotaEvents = filteredLiveEvents.filter(a => a.sport === 3);
            this.setState({ liveEvents: filteredLiveEvents, leagueLiveEvents: leagueEvents, dotaLiveEvents: dotaEvents });

            return;
        }
        for (let i = 0; i < liveEvents.length; i++) {
            if (liveEvents[i].homeTeam.teamName === user.homeTeam.teamName &&
                liveEvents[i].awayTeam.teamName === user.awayTeam.teamName) {
                return;
            }
        }

        liveEvents.push(user);

        let leagueEvents = liveEvents.filter(a => a.sport === 1);
        let dotaEvents = liveEvents.filter(a => a.sport === 3);

        this.setState({ liveEvents: liveEvents, leagueLiveEvents: leagueEvents, dotaLiveEvents: dotaEvents });
    }
    render() {
        let body;
        let leagueOfLegends;
        let dota2;
        if (this.state.liveEvents.length === 0) {
            body = <h1> No Live Games Currently, Check back Later </h1>;
        }
        else {
            if (this.state.leagueLiveEvents.length > 0) {
                leagueOfLegends = (
                    <div>

                        <h2> LeagueOfLegends </h2>
                        {this.state.leagueLiveEvents.map((ev) =>
                            <LiveGame
                                liveEvent={ev}
                            />
                        )}

                    </div>
                );
            }
            if (this.state.dotaLiveEvents.length > 0) {
                dota2 = (
                    <div>

                        <h2> Dota 2 </h2>
                        {this.state.dotaLiveEvents.map((ev) =>
                            <LiveGame
                                liveEvent={ev}
                            />
                        )}

                    </div>
                );
            }           
            }      
    
        return (
            <div>
                {body}
                {leagueOfLegends}
                {dota2}
            </div>
        )
    }
}