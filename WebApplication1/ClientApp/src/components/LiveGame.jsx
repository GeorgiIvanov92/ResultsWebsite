import React, { Component } from 'react';
import { Popover, OverlayTrigger, Jumbotron, Button, ButtonToolbar, Overlay } from 'react-bootstrap';
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
            hubConnection: null,
            pingEpic: null,
        };
    }

    render() {
        let body = [];       

        if (this.props.liveEvent) {
            let minutes = ("0" + parseInt(this.props.liveEvent.gameTime / 60)).slice(-2);
            let seconds = ("0" + this.props.liveEvent.gameTime % 60).slice(-2);
            let header = this.props.liveEvent.homeTeam.teamName + " " + this.props.liveEvent.homeTeam.winsInSeries + " - " + this.props.liveEvent.awayTeam.winsInSeries + " " + this.props.liveEvent.awayTeam.teamName;
            let popUnder =
                <Popover id={header} 
                    title={header} >
                    <Jumbotron>
                        <h1>Hello, world!</h1>
                        <p>
                            This is a simple hero unit, a simple jumbotron-style component for calling
                            extra attention to featured content or information.
  </p>                       
                    </Jumbotron>;
  </Popover>;

            body = <div>               
                <Wave
                    effect="jump"
                    effectChange={0.2}
                    text = "LIVE!" />
                     
                <ButtonToolbar>
                    <OverlayTrigger
                        trigger="click"
                        rootClose
                        placement="bottom"
                        overlay={popUnder}
                    >
                        <Button key={this.props.liveEvent.homeTeam.teamName} bsStyle="primary" >                       
                            {minutes}:{seconds} {this.props.liveEvent.homeTeam.teamName} {this.props.liveEvent.homeTeam.winsInSeries} - {this.props.liveEvent.awayTeam.winsInSeries} {this.props.liveEvent.awayTeam.teamName}  (BO{this.props.liveEvent.bestOf})   
                            <img style={{ width: '30px', height: '30px' }} src={this.props.liveEvent.sport === 1 ? leagueLogo : dotaLogo} alt="Logo" />
                    </Button>
                    </OverlayTrigger>
                </ButtonToolbar>

</div>        
                }
                return (
            <div>
                {body}
            </div>
        );
    }
}