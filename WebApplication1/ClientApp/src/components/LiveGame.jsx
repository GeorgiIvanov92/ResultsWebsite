import React, { Component } from 'react';
import { Button } from 'react-bootstrap';
import Popover from 'react-bootstrap/Popover'
import { Wave } from 'react-animated-text';
import leagueLogo from './Resources/Images/leagueLogo.png';
import dotaLogo from './Resources/Images/dotaLogo.png';
import csLogo from './Resources/Images/csLogo.png';
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
                <Button key={this.state.liveEvent.homeTeam.teamName} bsStyle="primary" onClick={this.show}>
                    {this.state.liveEvent.homeTeam.teamName} {this.state.liveEvent.homeTeam.winsInSeries} - {this.state.liveEvent.awayTeam.winsInSeries} {this.state.liveEvent.awayTeam.teamName}   
                </Button>
                <img style={{ width: '30px', height: '30px' }} src={this.state.liveEvent.sport === 1 ? leagueLogo : dotaLogo} alt="Logo" />
</div>        
                }
                return (
            <div>
                {body}
            </div>
        );
    }
}