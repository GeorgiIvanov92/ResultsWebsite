import React, { Component } from 'react';
import { HubConnection } from '@aspnet/signalr-client';

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
        const nick = window.prompt('Your name:', 'John');

        const hubConnection = new HubConnection('/LiveEvents');

        this.setState({ hubConnection, nick }, () => {
            this.state.hubConnection
                .start()
                .then(() => console.log('Connection started!'))
                .catch(err => console.log('Error while establishing connection :('));

            this.state.hubConnection.on('sendToAll', (nick, receivedMessage) => {
                const text = `${nick}: ${receivedMessage}`;
                const messages = this.state.messages.concat([text]);
                this.setState({ message: messages });
            });
        });
    }

    render() {
        return <div>{this.state.message}</div>;
    }
}