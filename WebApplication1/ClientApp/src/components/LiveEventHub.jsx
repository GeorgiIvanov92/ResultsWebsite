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

        let sportIds = this.props.sportToLoad.split(",");

        for (let i = 0; i < sportIds.length; i++) {
            connection.on(sportIds[i].toString(), this.parseLiveEvent);
        }
        
        connection.start().then(function () {
        });
        this.setState({ hubConnection: connection });
    }

    componentWillUnmount = () => {
        let connection = this.state.hubConnection;
        connection.stop();
        connection.off();
    }

    parseLiveEvent(liveEvent, message) {
        let liveEvents = this.state.liveEvents;
        if (liveEvent.shouldRemoveEvent === true) {

            let filteredLiveEvents = liveEvents.filter(a => a.homeTeam.teamName !== liveEvent.homeTeam.teamName &&
                a.awayTeam.teamName !== liveEvent.awayTeam.teamName);     

            this.setState({ liveEvents: filteredLiveEvents});

            return;
        }
        for (let i = 0; i < liveEvents.length; i++) {

            if (liveEvents[i].homeTeam.teamName === liveEvent.homeTeam.teamName &&
                liveEvents[i].awayTeam.teamName === liveEvent.awayTeam.teamName) {

                liveEvents[i] = liveEvent;
                this.setState({ liveEvents: liveEvents });

                return;
            }
        }

        liveEvents.push(liveEvent);

        this.setState({ liveEvents: liveEvents });
    }
    render() {
        let body;
        if (this.state.liveEvents.length > 0) {
            body = (
                this.state.liveEvents.map((ev) =>                    
                    <LiveGame
                        liveEvent={ev}
                    />
                ));
        }

        return (
            <div>
                {body}
            </div>
        )
    }
}