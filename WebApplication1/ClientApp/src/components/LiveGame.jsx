import React, { Component } from 'react';
import { Button } from 'react-bootstrap';
import { Wave } from 'react-animated-text';
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
            body = <div>
                <Wave
                    effect="jump"
                    effectChange={0.2}
                    text="LIVE!" />
                <Button bsStyle="primary" onClick={this.show}>
                    {this.state.liveEvent.homeTeam.teamName} {this.state.liveEvent.homeTeam.winsInSeries} - {this.state.liveEvent.awayTeam.winsInSeries} {this.state.liveEvent.awayTeam.teamName}
                </Button>
</div>        
                }
                return (
            <div>
                {body}
            </div>
        );
    }
}