import React, { Component } from 'react';
import * as  signalr from '@aspnet/signalr';

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
        const connection = new signalr.HubConnectionBuilder()
            .withUrl("/LiveEvents")
            .configureLogging(signalr.LogLevel.Information)
            .build();

        connection.start().then(function () {
            console.log("connected");
        });
    }

    render() {
        return <div>{this.state.message}</div>;
    }
}