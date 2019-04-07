import React, { Component } from 'react';
import { Navbar, Button, ButtonToolbar, Table } from 'react-bootstrap';
import './Style/SpecificTeamStyle.css';

export class LeagueOfLegendsData extends Component {
   

    displayName = LeagueOfLegendsData.name

  constructor(props) {
    super(props);
      this.state = {
          sport: [],
          results: [],
          images: [],
          prelive: [],
          teams: [],
          players: [],
          specificTeams: [],
          loading: true,
          loadedspecificLeague: false,
          shouldLoadPrelive: false,
          shouldLoadResults: false,
          shouldLoadTeams: false,
          shouldLoadSpecificTeam: false,
          specificTeam: [],
          specificleague: '',
      };
      fetch('api/LeagueOfLegends/GetSport')
          .then(response => response.json())
          .then(data => {
              this.setState({
                  sport: data,
                  results: data.resultsEvents,
                  images: data.teamLogos,
                  prelive: data.preliveEvents,
                  teams: data.teamsInLeague,
                  players: data.players,
                  loading: false
              });
          });
      
      this.renderResults = this.renderResults.bind(this);
      this.determineLeaguesToAdd = this.determineLeaguesToAdd.bind(this);
      this.renderLeagueTable = this.renderLeagueTable.bind(this);
      this.renderTeams = this.renderTeams.bind(this);
      this.renderSpecificTeam = this.renderSpecificTeam.bind(this);
    }
    renderLeagueTable(arr) {
        return (
            <div>
                <h1>League of Legends</h1> 
                {arr.map(league =>

                    <Navbar inverse style={{ width: '50%' }} onClick={() => this.setState({
                        loadedspecificLeague: true,
                        shouldLoadResults: true,
                        specificleague: league
                    })}>
                        <Navbar.Header>
                            <Navbar.Brand>
                                <a>{league}</a>
                            </Navbar.Brand>
                        </Navbar.Header>
                    </Navbar>
                    )}
            </div>
        );
        }
    determineLeaguesToAdd(results) {
        let arr = [];
        Object.keys(results).forEach(function (key) {
            arr.push(key);
        });
        return (
            this.renderLeagueTable(arr)
        );
    }   

    renderResults(results,teams) {
        let tempRes;
        for (let a = 0; a < results.length; a++) {
            for (let i = 0; i < results.length - 1; i++) {
                if (results[i].gameDate < results[i + 1].gameDate) {
                    tempRes = results[i + 1];
                    results[i + 1] = results[i];
                    results[i] = tempRes;
                }
            }
        }
        let specificTeams = teams[this.state.specificleague];
            let disablePrelive = true;
            if (this.state.specificleague in this.state.prelive)
            {
                disablePrelive = false;
            }           
        return (
            
            <div>
                              
                <h1>{this.state.specificleague} Results</h1>
                <ButtonToolbar>  
                    
                    <Button variant="outline-dark" onClick={() =>
                        this.setState({
                        loadedspecificLeague: false,
                        specificleague: '',
                    })}>Back To All Leagues</Button>

                    <Button variant="outline-dark" disabled={disablePrelive} onClick={() => this.setState({
                        shouldLoadPrelive: true,
                        shouldLoadResults: false,
                        shouldLoadTeams: false,
                        shouldLoadSpecificTeam: false,
                    })}>Prelive</Button>

                    {specificTeams ? <Button variant="outline-dark" onClick={() => this.setState({
                        specificTeams: specificTeams,
                        shouldLoadPrelive: false,
                        shouldLoadResults: false,
                        shouldLoadSpecificTeam: false,
                        shouldLoadTeams: true,
                    })}>Teams</Button>

                        : <Button variant="outline-dark" disabled={true}>Teams</Button>}
                </ButtonToolbar>
                    <Table striped bordered hover variant="dark" className='table'>
                        <thead>
                            <tr>
                                <th>Game Date</th>
                                <th>Home Team</th>
                                <th>Home Logo</th>
                                <th>Home Score</th>
                                <th>Away Score</th>
                                <th>Away Logo</th>
                                <th>Away Team</th>
                            </tr>
                        </thead>
                        <tbody>
                            {results.map(result =>
                                <tr key={result.gameDate + "@" + result.homeTeam}>

                                    <td>{new Date(result.gameDate)
                                        .toLocaleDateString('en-GB',
                                            {
                                                day: 'numeric',
                                                month: 'short',
                                                year: 'numeric',
                                                hour: 'numeric',
                                                minute: 'numeric',
                                            })}</td>
                                    <td>{result.homeTeam}</td>
                                    <img src={`data:image/png;base64,${result.homeTeam in this.state.images ?
                                        this.state.images[result.homeTeam] : this.state.images['default']}`} alt={result.homeTeam} />
                                    <td>{result.homeScore}</td>
                                    <td>{result.awayScore}</td>
                                    <img src={`data:image/png;base64,${result.awayTeam in this.state.images ?
                                        this.state.images[result.awayTeam] : this.state.images['default']}`} alt={result.awayTeam} />
                                    <td>{result.awayTeam}</td>
                                </tr>
                            )}
                        </tbody>
                    </Table>
                </div>
            );
    }

    renderPrelive(prelive) {
        let tempRes;
        for (let a = 0; a < prelive.length; a++) {
            for (let i = 0; i < prelive.length - 1; i++) {
                if (prelive[i].gameDate > prelive[i + 1].gameDate) {
                    tempRes = prelive[i + 1];
                    prelive[i + 1] = prelive[i];
                    prelive[i] = tempRes;
                }
            }
        }
        let specificTeams = this.state.teams[this.state.specificleague];
        return (
            <div>
                <h1>{this.state.specificleague} Upcoming Games</h1> 
                <Button variant="outline-dark" onClick={() => this.setState({
                    loadedspecificLeague: false,
                    specificleague: ''
                })}>Back To All Leagues</Button>

                <Button variant="outline-dark" onClick={() => this.setState({
                    shouldLoadResults: true,
                    shouldLoadPrelive: false,
                    shouldLoadTeams: false,
                    shouldLoadSpecificTeam: false,
                })}>Results</Button>

                {specificTeams ? <Button variant="outline-dark" onClick={() => this.setState({
                    specificTeams: specificTeams,
                    shouldLoadPrelive: false,
                    shouldLoadResults: false,
                    shouldLoadSpecificTeam: false,
                    shouldLoadTeams: true,
                })}>Teams</Button>

                    : <Button variant="outline-dark" disabled={true}>Teams</Button>}


                <Table striped bordered hover variant="dark" className='table'>
                    <thead>
                        <tr>
                            <th>Game Date</th>
                            <th>Home Team</th>
                            <th>Home Logo</th>                            
                            <th>Away Logo</th>
                            <th>Away Team</th>
                            <th>Best Of</th>
                        </tr>
                    </thead>
                    <tbody>
                        {prelive.map(pre =>
                            <tr key={pre.gameDate + "@" + pre.homeTeam}>

                                <td>{new Date(pre.gameDate)
                                    .toLocaleDateString('en-GB',
                                        {
                                            day: 'numeric',
                                            month: 'short',
                                            year: 'numeric',
                                            hour: 'numeric',
                                            minute: 'numeric',
                                    })}</td>
                                <img src={`data:image/png;base64,${pre.homeTeam in this.state.images ?
                                    this.state.images[pre.homeTeam] : this.state.images['default']}`} alt={pre.homeTeam} >
                                </img>
                                <td>{pre.homeTeam}</td>          
                                <td>{pre.awayTeam}</td>
                                <img src={`data:image/png;base64,${pre.awayTeam in this.state.images ?
                                    this.state.images[pre.awayTeam] : this.state.images['default']}`} alt={pre.awayTeam} >
                                </img>
                                <td>{pre.bestOf}</td>
                            </tr>
                        )}
                    </tbody>
                </Table>
            </div>
        );
    }

    renderTeams() {
        let tempTeams;
        let disablePrelive = true;
        if (this.state.specificleague in this.state.prelive) {
            disablePrelive = false;
        }           
        let LeagueTeams = this.state.specificTeams;
        for (let a = 0; a < LeagueTeams.length; a++) {
            for (let i = 0; i < LeagueTeams.length - 1; i++) {
                if (LeagueTeams[i].winrate < LeagueTeams[i + 1].winrate) {
                    tempTeams = LeagueTeams[i + 1];
                    LeagueTeams[i + 1] = LeagueTeams[i];
                    LeagueTeams[i] = tempTeams;
                }
            }
        }
        return (
            <div>
                <h1>{this.state.specificleague} Teams</h1>
                <Button variant="outline-dark" onClick={() => this.setState({
                    loadedspecificLeague: false,
                    specificleague: ''
                })}>Back To All Leagues</Button>

                <Button variant="outline-dark" onClick={() => this.setState({
                    shouldLoadResults: true,
                    shouldLoadPrelive: false,
                    shouldLoadTeams: false,
                    shouldLoadSpecificTeam: false,
                })}>Results</Button>   

                <Button variant="outline-dark" disabled={disablePrelive} onClick={() => this.setState({
                    shouldLoadPrelive: true,
                    shouldLoadResults: false,
                    shouldLoadTeams: false,
                    shouldLoadSpecificTeam: false,
                })}>Prelive</Button>

                <Table striped bordered hover variant="dark" className='table'>
                    <thead>
                        <tr>
                            <th>Logo</th>
                            <th>Name</th>
                            <th>Wins</th>
                            <th>Losses</th>
                            <th>Winrate</th>
                        </tr>
                    </thead>
                    <tbody>
                        {LeagueTeams.map(team =>
                            <tr onClick={() => this.setState({
                                shouldLoadResults: false,
                                shouldLoadPrelive: false,
                                shouldLoadTeams: false,
                                shouldLoadSpecificTeam: true,
                                specificTeam: team,
                            })} key = { team.name } >
                                <td>
                                    <img src={`data:image/png;base64,${this.state.images[team.name]
                                        || this.state.images[team.name.toLowerCase()]
                                        || this.state.images[team.name.toUpperCase()]
                                        || this.state.images[team.name[0].toLowerCase()]
                                        || this.state.images[team.name[0].toUpperCase()]

                                        || this.state.images[team.name.trim().split(' ')[0]]
                                        || this.state.images[team.name.trim().split(' ')[0].toLowerCase()]
                                        || this.state.images[team.name.trim().split(' ')[0].toUpperCase()]
                                        || this.state.images[team.name.trim().split(' ')[0][0].toUpperCase()]
                                        || this.state.images[team.name.trim().split(' ')[0][team.name.trim().split(' ')[0].length - 1].toUpperCase()]
                                        || this.state.images[team.name.trim().split(' ')[0][team.name.trim().split(' ')[0].length - 1].toLowerCase()]

                                        || this.state.images[team.name.replace('Esports', '').trim()]
                                        || this.state.images[team.name.replace('Esports', '').trim().toUpperCase()]
                                        || this.state.images[team.name.replace('Esports', '').trim().toLowerCase()]
                                        || this.state.images[team.name.replace('Esports', '').trim()[0].toUpperCase()]
                                        || this.state.images[team.name.replace('Esports', '').trim()[0].toLowerCase()]

                                        || this.state.images[team.name.replace('eSports', '').trim()]
                                        || this.state.images[team.name.replace('eSports', '').trim().toUpperCase()]
                                        || this.state.images[team.name.replace('eSports', '').trim().toLowerCase()]
                                        || this.state.images[team.name.replace('eSports', '').trim()[0].toUpperCase()]
                                        || this.state.images[team.name.replace('eSports', '').trim()[0].toLowerCase()]

                                        || this.state.images[team.name.replace('Gaming', '').trim()]
                                        || this.state.images[team.name.replace('Gaming', '').trim().toUpperCase()]
                                        || this.state.images[team.name.replace('Gaming', '').trim().toLowerCase()]
                                        || this.state.images[team.name.replace('Gaming', '').trim()[0].toUpperCase()]
                                        || this.state.images[team.name.replace('Gaming', '').trim()[0].toLowerCase()]

                                        || this.state.images[team.name.replace('Team', '').trim()]
                                        || this.state.images[team.name.replace('Team', '').trim().toUpperCase()]
                                        || this.state.images[team.name.replace('Team', '').trim().toLowerCase()]
                                        || this.state.images[team.name.replace('Team', '').trim()[0].toUpperCase()]
                                        || this.state.images[team.name.replace('Team', '').trim()[0].toLowerCase()]

                                        || this.state.images[team.name.replace('e-Sports Club', '').trim()]
                                        || this.state.images[team.name.replace('e-Sports Club', '').trim().toUpperCase()]
                                        || this.state.images[team.name.replace('e-Sports Club', '').trim().toLowerCase()]
                                        || this.state.images[team.name.replace('e-Sports Club', '').trim()[0].toUpperCase()]
                                        || this.state.images[team.name.replace('e-Sports Club', '').trim()[0].toLowerCase()]

                                        || this.state.images[team.name.replace('e-Sports', '').trim()]
                                        || this.state.images[team.name.replace('e-Sports', '').trim().toUpperCase()]
                                        || this.state.images[team.name.replace('e-Sports', '').trim().toLowerCase()]
                                        || this.state.images[team.name.replace('e-Sports', '').trim()[0].toUpperCase()]
                                        || this.state.images[team.name.replace('e-Sports', '').trim()[0].toLowerCase()]

                                        || this.state.images[team.name.replace('Gamers', '').trim()]
                                        || this.state.images[team.name.replace('Gamers', '').trim().toUpperCase()]
                                        || this.state.images[team.name.replace('Gamers', '').trim().toLowerCase()]
                                        || this.state.images[team.name.replace('Gamers', '').trim()[0].toUpperCase()]
                                        || this.state.images[team.name.replace('Gamers', '').trim()[0].toLowerCase()]

                                        || this.state.images['default']}`} alt={team.name} >
                                    </img>
                                    </td>
                                <td>{team.name}</td>
                                <td>{team.blueSideWins + team.redSideWins}</td>
                                <td>{team.blueSideLosses + team.redSideLosses}</td>
                                <td>{team.winrate}%</td>
                            </tr>
                        )}
                    </tbody>
                </Table>
            </div>
        );
    }

    renderSpecificTeam() {
        let team = this.state.specificTeam;
        let players = [];
        this.state.players.forEach(function (player) {
            if (player.teamId === team.id) {
                players.push(player);
            }
        });
        let disablePrelive = true;
        if (this.state.specificleague in this.state.prelive) {
            disablePrelive = false;
        }  
        return (
            <div>
                <div className='row'>
                    <div className='column'>
                <img src={`data:image/png;base64,${this.state.images[team.name]
                    || this.state.images[team.name.toLowerCase()]
                    || this.state.images[team.name.toUpperCase()]
                    || this.state.images[team.name[0].toLowerCase()]
                    || this.state.images[team.name[0].toUpperCase()]

                    || this.state.images[team.name.trim().split(' ')[0]]
                    || this.state.images[team.name.trim().split(' ')[0].toLowerCase()]
                    || this.state.images[team.name.trim().split(' ')[0].toUpperCase()]
                    || this.state.images[team.name.trim().split(' ')[0][0].toUpperCase()]
                    || this.state.images[team.name.trim().split(' ')[0][team.name.trim().split(' ')[0].length - 1].toUpperCase()]
                    || this.state.images[team.name.trim().split(' ')[0][team.name.trim().split(' ')[0].length - 1].toLowerCase()]

                    || this.state.images[team.name.replace('Esports', '').trim()]
                    || this.state.images[team.name.replace('Esports', '').trim().toUpperCase()]
                    || this.state.images[team.name.replace('Esports', '').trim().toLowerCase()]
                    || this.state.images[team.name.replace('Esports', '').trim()[0].toUpperCase()]
                    || this.state.images[team.name.replace('Esports', '').trim()[0].toLowerCase()]

                    || this.state.images[team.name.replace('eSports', '').trim()]
                    || this.state.images[team.name.replace('eSports', '').trim().toUpperCase()]
                    || this.state.images[team.name.replace('eSports', '').trim().toLowerCase()]
                    || this.state.images[team.name.replace('eSports', '').trim()[0].toUpperCase()]
                    || this.state.images[team.name.replace('eSports', '').trim()[0].toLowerCase()]

                    || this.state.images[team.name.replace('Gaming', '').trim()]
                    || this.state.images[team.name.replace('Gaming', '').trim().toUpperCase()]
                    || this.state.images[team.name.replace('Gaming', '').trim().toLowerCase()]
                    || this.state.images[team.name.replace('Gaming', '').trim()[0].toUpperCase()]
                    || this.state.images[team.name.replace('Gaming', '').trim()[0].toLowerCase()]

                    || this.state.images[team.name.replace('Team', '').trim()]
                    || this.state.images[team.name.replace('Team', '').trim().toUpperCase()]
                    || this.state.images[team.name.replace('Team', '').trim().toLowerCase()]
                    || this.state.images[team.name.replace('Team', '').trim()[0].toUpperCase()]
                    || this.state.images[team.name.replace('Team', '').trim()[0].toLowerCase()]

                    || this.state.images[team.name.replace('e-Sports Club', '').trim()]
                    || this.state.images[team.name.replace('e-Sports Club', '').trim().toUpperCase()]
                    || this.state.images[team.name.replace('e-Sports Club', '').trim().toLowerCase()]
                    || this.state.images[team.name.replace('e-Sports Club', '').trim()[0].toUpperCase()]
                    || this.state.images[team.name.replace('e-Sports Club', '').trim()[0].toLowerCase()]

                    || this.state.images[team.name.replace('e-Sports', '').trim()]
                    || this.state.images[team.name.replace('e-Sports', '').trim().toUpperCase()]
                    || this.state.images[team.name.replace('e-Sports', '').trim().toLowerCase()]
                    || this.state.images[team.name.replace('e-Sports', '').trim()[0].toUpperCase()]
                    || this.state.images[team.name.replace('e-Sports', '').trim()[0].toLowerCase()]

                    || this.state.images[team.name.replace('Gamers', '').trim()]
                    || this.state.images[team.name.replace('Gamers', '').trim().toUpperCase()]
                    || this.state.images[team.name.replace('Gamers', '').trim().toLowerCase()]
                    || this.state.images[team.name.replace('Gamers', '').trim()[0].toUpperCase()]
                    || this.state.images[team.name.replace('Gamers', '').trim()[0].toLowerCase()]

                    || this.state.images['default']}`} alt={team.name} >
                        </img>
                    </div>
                    <div className='column'>
                        <h1>{team.name}</h1>
                        </div>
                    </div>
                
                <Button variant="outline-dark" onClick={() => this.setState({
                    loadedspecificLeague: false,
                    specificleague: ''
                })}>Back To All Leagues</Button>

                <Button variant="outline-dark" onClick={() => this.setState({
                    shouldLoadResults: true,
                    shouldLoadPrelive: false,
                    shouldLoadTeams: false,
                    shouldLoadSpecificTeam: false,
                })}>Results</Button>

                <Button variant="outline-dark" disabled={disablePrelive} onClick={() => this.setState({
                    shouldLoadPrelive: true,
                    shouldLoadResults: false,
                    shouldLoadTeams: false,
                    shouldLoadSpecificTeam: false,
                })}>Prelive</Button>            
                <h2 style={{textAlign: 'center'}}> Player Stats </h2>
                <Table striped bordered hover variant='dark' className='table'>
                    <thead>
                        <tr>
                            <th>Nickname</th>
                            <th>Wins</th>
                            <th>Losses</th>
                            <th>KDA</th>
                            <th>CS Per Minute</th>
                            <th>Gold Per Minute</th>
                            <th>Gold Percentage In Team</th>
                            <th>Kill Participation</th>
                            <th>Damage Per Minute</th>
                            <th>Damage Percent</th>
                            <th>Kills & Assits Per Minute</th>
                            <th>Solo Kills</th>
                            <th>Pentakills</th>
                            <th>Vision Score Per Minute</th>
                            <th>Wards Per Minute</th>
                            <th>Vision Wards Per Minute</th>
                            <th>Wards Cleared Per Minute</th>
                            <th>CS Difference at 15 min</th>
                            <th>Gold Difference at 15 min</th>
                            <th>XP Difference at 15 min</th>
                            <th>First Blood Participation</th>
                            <th>First Blood Victim</th>
                        </tr>
                    </thead>
                    <tbody>
                        {players.map(player =>
                            <tr key={player.nickName}>
                                <td>{player.nickname}</td>
                                <td>{player.wins}</td>
                                <td>{player.losses}</td>
                                <td>{player.kda}</td>
                                <td>{player.csPerMinute}</td>
                                <td>{player.goldPerMinute}</td>
                                <td>{player.goldPercent}%</td>
                                <td>{player.killParticipation}%</td>
                                <td>{player.damagePerMinute}</td>
                                <td>{player.damagePercent}%</td>
                                <td>{player.killsAndAssistsPerMinute}</td>
                                <td>{player.soloKills}</td>
                                <td>{player.pentakills}</td>
                                <td>{player.visionScorePerMinute}</td>
                                <td>{player.wardPerMinute}</td>
                                <td>{player.visionWardsPerMinute}</td>
                                <td>{player.wardsClearedPerMinute}</td>
                                <td>{player.csDifferenceAt15}</td>
                                <td>{player.goldDifferenceAt15}</td>
                                <td>{player.xpDifferenceAt15}</td>
                                <td>{player.firstBloodParticipationPercent}%</td>
                                <td>{player.firstBloodVictimPercent}%</td>
                            </tr>
                        )}
                    </tbody>
                </Table>


                <h2 style={{ textAlign: 'center' }}> Team Stats </h2>
                <Table striped bordered hover variant="dark" className='table'>
                    <thead>
                        <tr>
                            <th>Winrate</th>
                            <th>Blue Side Wins</th>
                            <th>Blue Side Losses</th>
                            <th>Red Side Wins</th>
                            <th>Red Side Losses</th>
                            <th>Average Game Time</th>
                            <th>Gold Per Minute</th>
                            <th>Gold Difference Per Minute</th>
                            <th>Gold Difference At 15 Mins</th>
                            <th>CS Per Minute</th>
                            <th>CS Difference At 15 Mins</th>
                            <th>Tower Difference At 15 Mins</th>
                            <th>First Tower Taken</th>
                            <th>Dragons Per Game</th>
                            <th>Dragons At 15 Mins</th>
                            <th>Heralds Per Game</th>
                            <th>Barons Per Game</th>
                            <th>Damage Per Minute</th>
                            <th>First Blood</th>
                            <th>Kills Per Game</th>
                            <th>Deaths Per Game</th>
                            <th>KD Ratio</th>
                            <th>Wards Per Minute</th>
                            <th>Wards Cleared Per Minute</th>
                            <th>Wards Cleared</th>
                            <th>Cloud Drakes Killed</th>
                            <th>Cloud Drakes Lost</th>
                            <th>Ocean Drakes Killed</th>
                            <th>Ocean Drakes Lost</th>
                            <th>Infernal Drakes Killed</th>
                            <th>Infernal Drakes Lost</th>
                            <th>Mountain Drakes Killed</th>
                            <th>Mountain Drakes Lost</th>
                            <th>Elder Drakes Killed</th>
                            <th>Elder Drakes Lost</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr key={team.winrate}>
                            <td>{team.winrate}%</td>
                                <td>{team.blueSideWins}</td>
                                <td>{team.blueSideLosses}</td>
                                <td>{team.redSideWins}</td>
                                <td>{team.redSideLosses}</td>
                                <td>{team.averageGameTime}</td>
                                <td>{team.goldPerMinute}</td>
                                <td>{team.goldDifferencePerMinute}</td>
                                <td>{team.goldDifferenceAt15}</td>
                                <td>{team.csPerMinute}</td>
                                <td>{team.csDifferenceAt15}</td>
                                <td>{team.towerDifferenceAt15}</td>
                                <td>{team.firstTowerPercent}%</td>
                                <td>{team.dragonsPerGame}</td>
                                <td>{team.dragonsAt15}</td>
                                <td>{team.heraldPerGame}</td>
                                <td>{team.nashorsPerGame}</td>
                                <td>{team.damagePerMinute}</td>
                                <td>{team.firstBloodPercent}%</td>
                                <td>{team.killsPerGame}</td>
                                <td>{team.deathsPerGame}</td>
                                <td>{team.kdRatio}</td>
                                <td>{team.wardsPerMinute}</td>
                                <td>{team.wardsClearedPerMinute}</td>
                                <td>{team.wardsClearedPercent}%</td>
                                <td>{team.cloudDrakesKilled}</td>
                                <td>{team.cloudDrakesLost}</td>
                                <td>{team.oceanDrakesKilled}</td>
                                <td>{team.oceanDrakesLost}</td>
                                <td>{team.infernalDrakesKilled}</td>
                                <td>{team.infernalDrakesLost}</td>
                                <td>{team.mountainDrakesKilled}</td>
                                <td>{team.mountainDrakesLost}</td>
                                <td>{team.elderDrakesKilled}</td>
                                <td>{team.elderDrakesLost}</td>
                            </tr>
                    </tbody>
                </Table>

            </div>
            );
    }


render() {
    let contents;

    if (this.state.loadedspecificLeague) {        
        if (this.state.shouldLoadResults) {
            contents = this.renderResults(this.state.results[this.state.specificleague], this.state.teams);
        } else if (this.state.shouldLoadPrelive) {
            contents = this.renderPrelive(this.state.prelive[this.state.specificleague]);
        } else if (this.state.shouldLoadTeams) {
            contents = this.renderTeams();
        } else if (this.state.shouldLoadSpecificTeam) {
            contents = this.renderSpecificTeam();
        }
        
    } else {
        contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.determineLeaguesToAdd(this.state.results);
    }

    return (
           
        <div>              
            {contents}
      </div>
    );
  }
}
