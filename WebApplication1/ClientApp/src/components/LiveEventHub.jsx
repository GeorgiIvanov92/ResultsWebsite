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
    }

    componentDidMount = () => {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("/liveEvents", {
                skipNegotiation: true,
                transport: signalR.HttpTransportType.WebSockets
            })
            .configureLogging(signalR.LogLevel.Information)
            .build();
        connection.on("ReceiveMessage", function (user, message) {
            var msg = message;
            console.log("liveEvent received with message: " + msg);
            this.setState({
                message: msg
            });
        });
        connection.start().then(function () {
            console.log("connected");
        });
    }

    render() {
        return <div>{this.state.message}</div>;
    }
}