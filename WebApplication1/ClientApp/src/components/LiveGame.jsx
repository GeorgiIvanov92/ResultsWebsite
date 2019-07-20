import React, { Component } from 'react';

export class LiveGame extends Component {
    constructor(props) {
        super(props);

        this.state = {
            nick: '',
            message: '',
            messages: [],
            liveEvent: null,
            hubConnection: null,
            pingEpic: null,
        };
    }

    componentDidMount = () => {
        this.setState({ liveEvent: this.props.liveEvent });
    }
 
    
    render() {
        let body = [];
        if (this.state.liveEvent) {
            body = <h1>{this.state.liveEvent.homeTeam.teamName} vs {this.state.liveEvent.awayTeam.teamName} </h1>;
        }
        return (
            <div>
                {body}
            </div>
        );
    }
}