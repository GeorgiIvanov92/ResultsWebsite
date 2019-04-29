import React, { Component } from 'react';
import { Navbar, Button, ButtonToolbar, Table } from 'react-bootstrap';
import './Style/SpecificTeamStyle.css';
import Autosuggest from 'react-autosuggest';
import './Utilities';
import { Utilities } from './Utilities';
export class LeagueOfLegendsData extends Component {
   

    displayName = LeagueOfLegendsData.name

  constructor(props) {
      super(props);
      this.state = {
          Utilities: new Utilities(),
          sport: [],
          results: [],
          images: [],
          prelive: [],
          teams: [],
          players: [],
          playerStats: [],
          specificTeams: [],
          loading: true,
          loadedspecificLeague: false,
          shouldLoadPrelive: false,
          shouldLoadResults: false,
          shouldLoadTeams: false,
          shouldLoadSpecificTeam: false,
          shouldloadPlayers: false,
          specificTeam: [],
          playersInLeague: [],
          specificleague: '',
          haveSortedTeamsInLeague: false,
          ascending: true,
          value: '',
          searchTeamResults: [],
      };     
      
      this.renderResults = this.renderResults.bind(this);
      this.determineLeaguesToAdd = this.determineLeaguesToAdd.bind(this);
      this.renderLeagueTable = this.renderLeagueTable.bind(this);
      this.renderTeams = this.renderTeams.bind(this);
      this.renderSpecificTeam = this.renderSpecificTeam.bind(this);
      this.renderPlayers = this.renderPlayers.bind(this);
      this.sortBy = this.sortBy.bind(this); 


      this.compareBy = this.compareBy.bind(this);
      this.onSuggestionsFetchRequested = this.onSuggestionsFetchRequested.bind(this);
      this.onSuggestionsClearRequested = this.onSuggestionsClearRequested.bind(this);
      this.getSuggestionValue = this.getSuggestionValue.bind(this);
      this.renderSuggestion = this.renderSuggestion.bind(this);

    }
    componentDidMount() {
        let urlParam = this.props.location.pathname.split("/");
        if (urlParam.length === 3) {
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
                        playerStats: data.championStats,
                        loading: false,
                        specificleague: this.props.location.pathname.split('/')[2],
                        shouldLoadResults: true,
                        loadedspecificLeague: true,
                    });
                });
        } else {
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
                        playerStats: data.championStats,
                        loading: false,
                        specificleague: '',
                        shouldLoadResults: false,
                        loadedspecificLeague: false,
                    });
                });
        }
    }
    renderLeagueTable(arr) {
        let prelive = this.state.prelive;
        for (var key in prelive) {
            if (!arr.includes(key)) {
                arr.push(key);
            }
        }
    
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
    renderMenuTabs() {
        let specificTeams = this.state.teams[this.state.specificleague];
        let disablePrelive = true;
        if (this.state.specificleague in this.state.prelive) {
            disablePrelive = false;
        }           
        return (
            <ButtonToolbar>

                <Button variant="outline-dark" onClick={() =>
                    this.setState({
                        loadedspecificLeague: false,
                        specificleague: '',
                        playersInLeague: [],
                        shouldLoadPrelive: false,
                        shouldLoadTeams: false,
                        shouldLoadSpecificTeam: false,
                        shouldloadPlayers: false,
                    })}>Back To All Leagues</Button>

                <Button variant="outline-dark" onClick={() => this.setState({
                    shouldLoadResults: true,
                    shouldLoadPrelive: false,
                    shouldLoadTeams: false,
                    shouldLoadSpecificTeam: false,
                    shouldloadPlayers: false,
                    playersInLeague: [],
                    haveSortedTeamsInLeague: false,
                })}>Results</Button> 

                <Button variant="outline-dark" disabled={disablePrelive} onClick={() => this.setState({
                    shouldLoadPrelive: true,
                    shouldLoadResults: false,
                    shouldLoadTeams: false,
                    shouldLoadSpecificTeam: false,
                    shouldloadPlayers: false,
                    playersInLeague: [],
                })}>Prelive</Button>

                {specificTeams ? <Button variant="outline-dark" onClick={() => this.setState({
                    specificTeams: specificTeams,
                    shouldLoadPrelive: false,
                    shouldLoadResults: false,
                    shouldLoadSpecificTeam: false,
                    shouldLoadTeams: true,
                    shouldloadPlayers: false,
                    playersInLeague: [],
                })}>Teams</Button>

                    : <Button variant="outline-dark" disabled={true}>Teams</Button>}

                {specificTeams ? <Button variant="outline-dark" onClick={() => this.setState({
                    shouldLoadPrelive: false,
                    shouldLoadResults: false,
                    shouldLoadSpecificTeam: false,
                    shouldLoadTeams: false,
                    shouldloadPlayers: true,
                    playersInLeague: [],
                })}>Players</Button>

                    : <Button variant="outline-dark" disabled={true}>Players</Button>}

                <Button variant="outline-dark" onClick={() => navigator.clipboard.writeText(
                    this.props.location.protocol + this.props.location.origin + "/leagueoflegends/" + this.state.Utilities.replaceAll(this.state.specificleague," ","%20"))}
                    >Copy League URL to Clipboard</Button>
            </ButtonToolbar>
            );
    }

    renderResults(results, teams) {
        if (!results) {
            this.setState({
                shouldLoadResults: false,
                shouldLoadPrelive: true,
            });
            return <h1>Loading...</h1>
        }       
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
        return (
            
            <div>
                <h1>{this.state.specificleague} Results</h1>
                 {this.renderMenuTabs()}
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
        if (!prelive) {
            this.setState({
                shouldLoadPrelive: false,
                loadedspecificLeague: false,
                specificleague: '',
                playersInLeague: [],
            });
            return;
        }
        
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
       
        return (
            <div>
                <h1>{this.state.specificleague} Prelive Games</h1>
                {this.renderMenuTabs()}

                <Table striped bordered hover variant="dark" className='table'>
                    <thead>
                        <tr>
                            <th>Game Date</th>
                            <th>Home Logo</th>
                            <th>Home Team</th>                            
                            <th>Away Team</th>
                            <th>Away Logo</th>
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
                                <img src={`data:image/png;base64,${pre.awayTeam in this.state.images ?
                                    this.state.images[pre.awayTeam] : this.state.images['default']}`} alt={pre.awayTeam} >
                                </img>
                                <td>{pre.awayTeam}</td>
                                
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
        if (!this.state.haveSortedTeamsInLeague) {
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
            this.setState({ specificTeams: LeagueTeams, haveSortedTeamsInLeague: true });
        }
        return (
            <div>
                <h1>{this.state.specificleague} Teams</h1>
                {this.renderMenuTabs()}

                <Table striped bordered hover variant="dark" className='table'>
                    <thead>
                        <tr>
                            <th>Team Logo</th>
                            <th onClick={() => this.sortBy('name', this.state.specificTeams)}>Team Name</th>
                            <th onClick={() => this.sortBy('winrate', this.state.specificTeams)}>Winrate</th>
                            <th onClick={() => this.sortBy('blueSideWins', this.state.specificTeams)}>Blue Side Wins</th>
                            <th onClick={() => this.sortBy('blueSideLosses', this.state.specificTeams)}>Blue Side Losses</th>
                            <th onClick={() => this.sortBy('redSideWins', this.state.specificTeams)}>Red Side Wins</th>
                            <th onClick={() => this.sortBy('redSideLosses', this.state.specificTeams)}>Red Side Losses</th>
                            <th onClick={() => this.sortBy('averageGameTime', this.state.specificTeams)}>Average Game Time</th>
                            <th onClick={() => this.sortBy('goldPerMinute', this.state.specificTeams)}>Gold Per Minute</th>
                            <th onClick={() => this.sortBy('goldDifferencePerMinute', this.state.specificTeams)}>Gold Difference Per Minute</th>
                            <th onClick={() => this.sortBy('goldDifferenceAt15', this.state.specificTeams)}>Gold Difference At 15 Mins</th>
                            <th onClick={() => this.sortBy('csPerMinute', this.state.specificTeams)}>CS Per Minute</th>
                            <th onClick={() => this.sortBy('csDifferenceAt15', this.state.specificTeams)}>CS Difference At 15 Mins</th>
                            <th onClick={() => this.sortBy('towerDifferenceAt15', this.state.specificTeams)}>Tower Difference At 15 Mins</th>
                            <th onClick={() => this.sortBy('firstTowerPercent', this.state.specificTeams)}>First Tower Taken</th>
                            <th onClick={() => this.sortBy('dragonsPerGame', this.state.specificTeams)}>Dragons Per Game</th>
                            <th onClick={() => this.sortBy('dragonsAt15', this.state.specificTeams)}>Dragons At 15 Mins</th>
                            <th onClick={() => this.sortBy('heraldPerGame', this.state.specificTeams)}>Heralds Per Game</th>
                            <th onClick={() => this.sortBy('nashorsPerGame', this.state.specificTeams)}>Barons Per Game</th>
                            <th onClick={() => this.sortBy('damagePerMinute', this.state.specificTeams)}>Damage Per Minute</th>
                            <th onClick={() => this.sortBy('firstBloodPercent', this.state.specificTeams)}>First Blood</th>
                            <th onClick={() => this.sortBy('killsPerGame', this.state.specificTeams)}>Kills Per Game</th>
                            <th onClick={() => this.sortBy('deathsPerGame', this.state.specificTeams)}>Deaths Per Game</th>
                            <th onClick={() => this.sortBy('kdRatio', this.state.specificTeams)}>KD Ratio</th>
                            <th onClick={() => this.sortBy('wardsPerMinute', this.state.specificTeams)}>Wards Per Minute</th>
                            <th onClick={() => this.sortBy('wardsClearedPerMinute', this.state.specificTeams)}>Wards Cleared Per Minute</th>
                            <th onClick={() => this.sortBy('wardsClearedPercent', this.state.specificTeams)}>Wards Cleared</th>
                            <th onClick={() => this.sortBy('cloudDrakesKilled', this.state.specificTeams)}>Cloud Drakes Killed</th>
                            <th onClick={() => this.sortBy('cloudDrakesLost', this.state.specificTeams)}>Cloud Drakes Lost</th>
                            <th onClick={() => this.sortBy('oceanDrakesKilled', this.state.specificTeams)}>Ocean Drakes Killed</th>
                            <th onClick={() => this.sortBy('oceanDrakesLost', this.state.specificTeams)}>Ocean Drakes Lost</th>
                            <th onClick={() => this.sortBy('infernalDrakesKilled', this.state.specificTeams)}>Infernal Drakes Killed</th>
                            <th onClick={() => this.sortBy('infernalDrakesLost', this.state.specificTeams)}>Infernal Drakes Lost</th>
                            <th onClick={() => this.sortBy('mountainDrakesKilled', this.state.specificTeams)}>Mountain Drakes Killed</th>
                            <th onClick={() => this.sortBy('mountainDrakesLost', this.state.specificTeams)}>Mountain Drakes Lost</th>
                            <th onClick={() => this.sortBy('elderDrakesKilled', this.state.specificTeams)}>Elder Drakes Killed</th>
                            <th onClick={() => this.sortBy('elderDrakesLost', this.state.specificTeams)}>Elder Drakes Lost</th>
                        </tr>
                    </thead>
                    <tbody>
                        {this.state.specificTeams.map(team =>
                            <tr onClick={() => this.setState({
                                shouldLoadResults: false,
                                shouldLoadPrelive: false,
                                shouldLoadTeams: false,
                                shouldLoadSpecificTeam: true,
                                specificTeam: team,
                                haveSortedTeamsInLeague: false,
                            })} key={team.name} >
                                <td>
                                    <img src={this.state.Utilities.getImageString(this.state.images,team.name)}></img>
                                </td>
                                <td>{team.name}</td>
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
                            )};
                    </tbody>
                </Table>
            </div >
        );
    }

    renderSpecificTeam() {
        let team = this.state.specificTeam;
        let players = [];
        let specificTeams = this.state.teams[this.state.specificleague];
        if (!specificTeams) {

            specificTeams = this.state.teams[this.state.Utilities.findTeamSpecificLeague(team, this.state.teams)];
        }
        this.state.players.forEach(function (player) {
            if (player.teamId === team.id) {
                players.push(player);
            }
        });       
        return (
            <div>
                <div className='row'>
                    <div className='column'>
                        <img src={this.state.Utilities.getImageString(this.state.images,team.name)}>
                        </img>
                    </div>
                    <div className='column'>
                        <h1>{team.name}</h1>
                        </div>
                    </div>
                
                {this.renderMenuTabs()}

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
    
    renderPlayers() {
        if (this.state.playersInLeague.length === 0) {
            let players = this.state.players;
            let playersInLeague = [];            
            this.state.teams[this.state.specificleague].forEach(function (team) {

                players.forEach(function (player) {

                    if (team.id === player.teamId) {
                        playersInLeague.push(player);
                    }
                });
            });
            this.setState({ playersInLeague: playersInLeague })
        }       

        return (
            <div>
                <h2 style={{ textAlign: 'center' }}> Players In {this.state.specificleague} </h2>
                {this.renderMenuTabs()}

                <Table striped bordered hover variant='dark' className='table'>
                    <thead>
                        <tr>
                            <th onClick={() => this.sortBy('nickname', this.state.playersInLeague)}>Nickname</th>
                            <th onClick={() => this.sortBy('wins', this.state.playersInLeague)}>Wins</th>
                            <th onClick={() => this.sortBy('losses', this.state.playersInLeague)}>Losses</th>
                            <th onClick={() => this.sortBy('kda', this.state.playersInLeague)}>KDA</th>
                            <th onClick={() => this.sortBy('csPerMinute', this.state.playersInLeague)}>CS Per Minute</th>
                            <th onClick={() => this.sortBy('goldPerMinute', this.state.playersInLeague)}>Gold Per Minute</th>
                            <th onClick={() => this.sortBy('goldPercent', this.state.playersInLeague)}>Gold Percentage In Team</th>
                            <th onClick={() => this.sortBy('killParticipation', this.state.playersInLeague)}>Kill Participation</th>
                            <th onClick={() => this.sortBy('damagePerMinute', this.state.playersInLeague)}>Damage Per Minute</th>
                            <th onClick={() => this.sortBy('damagePercent', this.state.playersInLeague)}>Damage Percent</th>
                            <th onClick={() => this.sortBy('killsAndAssistsPerMinute', this.state.playersInLeague)}>Kills & Assits Per Minute</th>
                            <th onClick={() => this.sortBy('soloKills', this.state.playersInLeague)}>Solo Kills</th>
                            <th onClick={() => this.sortBy('pentakills', this.state.playersInLeague)}>Pentakills</th>
                            <th onClick={() => this.sortBy('visionScorePerMinute', this.state.playersInLeague)}>Vision Score Per Minute</th>
                            <th onClick={() => this.sortBy('wardPerMinute', this.state.playersInLeague)}>Wards Per Minute</th>
                            <th onClick={() => this.sortBy('visionWardsPerMinute', this.state.playersInLeague)}>Vision Wards Per Minute</th>
                            <th onClick={() => this.sortBy('wardsClearedPerMinute', this.state.playersInLeague)}>Wards Cleared Per Minute</th>
                            <th onClick={() => this.sortBy('csDifferenceAt15', this.state.playersInLeague)}>CS Difference at 15 min</th>
                            <th onClick={() => this.sortBy('goldDifferenceAt15', this.state.playersInLeague)}>Gold Difference at 15 min</th>
                            <th onClick={() => this.sortBy('xpDifferenceAt15', this.state.playersInLeague)}>XP Difference at 15 min</th>
                            <th onClick={() => this.sortBy('firstBloodParticipationPercent', this.state.playersInLeague)}>First Blood Participation</th>
                            <th onClick={() => this.sortBy('firstBloodVictimPercent', this.state.playersInLeague)}>First Blood Victim</th>
                        </tr>
                    </thead>
                    <tbody>
                        {this.state.playersInLeague.map(player =>
                            <tr key={player.nickname}>
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
            </div>
        );
    }

    compareBy(key) {
        if (this.state.ascending) {
            return function (a, b) {

                if (a[key] > b[key]) return -1;
                if (a[key] < b[key]) return 1;
                return 0;
            }
        } else {
            return function (a, b) {

                if (a[key] < b[key]) return -1;
                if (a[key] > b[key]) return 1;
                return 0;
            }
        }
    }

    sortBy(key,array) {
        let arrayCopy = array;
        arrayCopy.sort(this.compareBy(key));
        if (this.state.ascending) {
            this.setState({ playersInLeague: arrayCopy, ascending: false });
        } else {
            this.setState({ playersInLeague: arrayCopy, ascending: true });
        }
    }
    onSuggestionsFetchRequested(team) {
        let viableTeams = [];
        let allTeams = this.state.teams;
        for (var key in this.state.teams) {

            allTeams[key].forEach(function (t) {
                if (t.name.toLowerCase().includes(team.value.toLowerCase())) {
                    viableTeams.push(t);
                }
            });
        }
        this.setState({
            searchTeamResults: viableTeams,
        });
    }
    onSuggestionsClearRequested() {
        this.setState({
            searchTeamResults: []
        });
    }
    getSuggestionValue(suggestion) {
        return suggestion.name;
    }
    renderSuggestion(suggestion) {
        return (
            <div onClick={() => this.setState({
                shouldLoadSpecificTeam: true,
                specificTeam: suggestion,
                specificleague: this.state.Utilities.findTeamSpecificLeague(suggestion.name, this.state.teams),
                shouldLoadPrelive: false,
                shouldLoadResults: false,
                shouldLoadTeams: false,
                shouldloadPlayers: false,
                playersInLeague: [],
                haveSortedTeamsInLeague: false,
                loadedspecificLeague: true
            })}><img                   
                    src={this.state.Utilities.getImageString(this.state.images, suggestion.name)} alt='teamImage'></img>
                {suggestion.name}
            </div>
        );
    }
    onChange = (event, { newValue, method }) => {
        this.setState({
            value: newValue
        });
    };       

render() {
    let contents;
    const { value } = this.state;
    const inputProps = {
        placeholder: "Find Team",
        value,
        onChange: this.onChange,
    };
    if (this.state.shouldLoadSpecificTeam) {
        contents = this.renderSpecificTeam();
    }
    else if (this.state.shouldLoadResults && this.state.specificleague === '') {
        this.setState({
            shouldLoadResults: false,
            loadedspecificLeague: false,
        })
    }
    else if (this.state.loadedspecificLeague) {        
        if (this.state.shouldLoadResults) {
            contents = this.renderResults(this.state.results[this.state.specificleague], this.state.teams);
        } else if (this.state.shouldLoadPrelive) {
            contents = this.renderPrelive(this.state.prelive[this.state.specificleague]);
        } else if (this.state.shouldLoadTeams) {
            contents = this.renderTeams();        
        } else if (this.state.shouldloadPlayers) {
            contents = this.renderPlayers();
        }
        
    } else {
        contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.determineLeaguesToAdd(this.state.results);
    }

    return (
        <div>     
            <Autosuggest
                suggestions={this.state.searchTeamResults}
                onSuggestionsFetchRequested={this.onSuggestionsFetchRequested}
                onSuggestionsClearRequested={this.onSuggestionsClearRequested}
                getSuggestionValue={this.getSuggestionValue}
                renderSuggestion={this.renderSuggestion}
                inputProps={inputProps} />
            {contents}
      </div>
    );
  }
}
