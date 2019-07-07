import React, { Component } from 'react';
import * as  signalR from '@aspnet/signalr';
import { filter, mapTo } from '../../node_modules/rxjs/operators';
import { Observable } from '../../node_modules/rxjs';
export class LiveEventHub extends Component {
    constructor(props) {
        super(props);

        this.state = {
            nick: '',
            message: '',
            messages: [],
            liveEvents: [],
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
            this.setState({ liveEvents: filteredLiveEvents });
            return;
        }
        for (let i = 0; i < liveEvents.length; i++) {
            if (liveEvents[i].homeTeam.teamName === user.homeTeam.teamName &&
                liveEvents[i].awayTeam.teamName === user.awayTeam.teamName) {
                return;
            }
        }
        liveEvents.push(user);
        if (this.state.liveEvents)
            var msg = user.homeTeam.teamName;
        console.log("liveEvent received with message: " + msg);
        this.setState({
            message: msg
        });
    }
    render() {
        return <div>{this.state.liveEvents.map((ev) =>
            <h1>{ev.homeTeam.teamName} vs {ev.awayTeam.teamName} </h1>
        )}</div>;
    }
}