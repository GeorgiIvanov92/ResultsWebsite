import React, { Component } from 'react';
import { Popover, OverlayTrigger, Jumbotron, Button, ButtonToolbar, Overlay, Grid, Row, Col } from 'react-bootstrap';
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
            let liveEvent = this.props.liveEvent;
            let minutes = ("0" + parseInt(liveEvent.gameTime / 60)).slice(-2);
            let seconds = ("0" + liveEvent.gameTime % 60).slice(-2);
            let header = liveEvent.homeTeam.teamName + " " + liveEvent.homeTeam.winsInSeries + " - " + liveEvent.awayTeam.winsInSeries + " " + liveEvent.awayTeam.teamName;
            let popUnder =
                <Popover class='bsClass'
                    id={header} 
                    title={header}
                    positionLeft={200}
                    positionRight={500} >

                    <Jumbotron style={{ display: 'inline-block', border: 'solid red 1px' }}>
                        <h3 style={{ textAlign: 'center' }}> Currently Playing Game {liveEvent.mapNumber} of Series </h3>
                        <Grid>
                            <Row className="show-grid">
                                <Col xs={12} md={8}>
                                    <h3> {liveEvent.homeTeam.teamName}</h3>
                                </Col>
                                <Col>
                                    <h3> {liveEvent.awayTeam.teamName}</h3>
                                </Col>
                            </Row>
                                    
                        </Grid>
                    </Jumbotron>
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
                        <Button key={liveEvent.homeTeam.teamName} bsStyle="primary" >                       
                            {minutes}:{seconds} {liveEvent.homeTeam.teamName} {liveEvent.homeTeam.winsInSeries} - {liveEvent.awayTeam.winsInSeries} {liveEvent.awayTeam.teamName}  (BO{liveEvent.bestOf})   
                            <img style={{ width: '30px', height: '30px' }} src={liveEvent.sport === 1 ? leagueLogo : dotaLogo} alt="Logo" />
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