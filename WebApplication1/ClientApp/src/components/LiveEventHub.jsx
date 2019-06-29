import React, { Component } from 'react';
import * as  signalR from '@aspnet/signalr';

export class LiveEventHub extends Component {
    constructor(props) {
        super(props);

        this.state = {
            nick: '',
            message: '',
            messages: [],
            hubConnection: null,
        };
        this.parseLiveEvent = this.parseLiveEvent.bind(this);
    }

    componentDidMount = () => {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("/live", {
                transport: signalR.HttpTransportType.WebSockets
            })
            .configureLogging(signalR.LogLevel.Information)
            .build();
        connection.on("ReceiveMessage", this.parseLiveEvent);
        connection.start().then(function () {
            console.log("connected");
        });
    }
    parseLiveEvent(user,message) {
            var msg = user.homeTeam.teamName;
        console.log("liveEvent received with message: " + msg);
        this.setState({
            message: msg
        });
    }
    render() {
        return <div>{this.state.message}</div>;
    }
}