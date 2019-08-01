import React, { Component } from 'react';
import { Popover, OverlayTrigger, Jumbotron, Button, ButtonToolbar, Overlay, Grid, Row, Col } from 'react-bootstrap';
import { Wave } from 'react-animated-text';
import leagueLogo from './Resources/Images/leagueLogo.png';
import dotaLogo from './Resources/Images/dotaLogo.png';
import csLogo from './Resources/Images/csLogo.png';
import kills from './Resources/Images/kills.png';
import { Utilities } from './Utilities';
export class LiveGame extends Component {
    constructor(props) {
        super(props);

        this.state = {
            nick: '',
            message: '',
            messages: [],
            hubConnection: null,
            pingEpic: null,
            Utilities: new Utilities(),
            homeTeamStyle: {
                color: 'green',
            },
            awayTeamStyle: {
                color: 'red',
            }
        };
    }

    render() {
        let body = [];       

        if (this.props.liveEvent) {
            let liveEvent = this.props.liveEvent;
            let minutes = ("0" + parseInt(liveEvent.gameTime / 60)).slice(-2);
            let seconds = ("0" + liveEvent.gameTime % 60).slice(-2);
            let homeTeamGold = liveEvent.homeTeam.gold + " Gold";
            let awayTeamGold = liveEvent.awayTeam.gold + " Gold";
            if (liveEvent.homeTeam.gold > liveEvent.awayTeam.gold) {
                homeTeamGold += "(Leads with " + (liveEvent.homeTeam.gold - liveEvent.awayTeam
                    .gold)+ " Gold)";
            } else if (liveEvent.awayTeam.gold > liveEvent.homeTeam.gold) {
                awayTeamGold += "(Leads with " + (liveEvent.awayTeam.gold - liveEvent.homeTeam.gold) + " Gold)";
            }
            let header = liveEvent.homeTeam.teamName + " " + liveEvent.homeTeam.winsInSeries + " - " + liveEvent.awayTeam.winsInSeries + " " + liveEvent.awayTeam.teamName;
            let popUnder =
                <Popover class='bsClass'
                    id={header} >

                    <Jumbotron style={{ display: 'inline-block', border: 'solid black 2px' }}>
                        <h3 style={{ textAlign: 'center' }}> {liveEvent.leagueName} </h3>
                        <h3 style={{ textAlign: 'center' }}> Map {liveEvent.mapNumber} </h3>
                        <h3 style={{ textAlign: 'center' }}> {minutes}:{seconds} </h3>
                        <Grid>
                            <Row className="show-grid">
                                <Col xs={3} md={8}>
                                    <h3 style={this.state.homeTeamStyle}>{liveEvent.homeTeam.teamName}      <img style={{ width: '30px', height: '30px' }} src={kills}/> {liveEvent.homeTeam.kills}</h3>
                                    <img
                                        src={this.state.Utilities.getImageString(this.props.teamLogos, liveEvent.homeTeam.teamName)} alt='teamImage'></img>
                                    <h5 style={this.state.homeTeamStyle} > {homeTeamGold}</h5>
                                </Col>
                                <Col>
                                    <h3 style={this.state.awayTeamStyle}> {liveEvent.awayTeam.kills}  <img style={{ width: '30px', height: '30px' }} src={kills} /> {liveEvent.awayTeam.teamName}</h3>
                                    <img
                                        src={this.state.Utilities.getImageString(this.props.teamLogos, liveEvent.awayTeam.teamName)} alt='teamImage'></img>
                                    <h5 style={this.state.awayTeamStyle}> {awayTeamGold} </h5>
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