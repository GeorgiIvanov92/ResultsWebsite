import React, { Component } from 'react';
import { Button } from 'react-bootstrap';
import { Random } from 'react-animated-text';
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
                <Random
                    effect="verticalFadeOut"
                    effectDirection="down"
                    style={{ color: 'red' }}
                    effectChange="3.0"
                    text="LIVE!" />
                <Button bsStyle="primary" onClick={this.show}>
                {this.state.liveEvent.homeTeam.teamName} vs {this.state.liveEvent.awayTeam.teamName}
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