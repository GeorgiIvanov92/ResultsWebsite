import React, { Component } from 'react';
import { Utilities } from '../Utilities';
import { Navbar, Button, ButtonToolbar, Table } from 'react-bootstrap';
export class Teams extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            Utilities: new Utilities(),
            haveSortedTeamsInLeague: false,
            specificTeams: this.props.teams,
        };
        
    }
    render() {
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
                <h1>{this.props.specificleague} Teams</h1>

                <Table striped bordered hover variant="dark" className='table'>
                    <thead>
                        <tr>
                            <th>Team Logo</th>
                            <th onClick={() => this.props.sortBy('name', this.state.specificTeams)}>Team Name</th>
                            <th onClick={() => this.props.sortBy('winrate', this.state.specificTeams)}>Winrate</th>
                            <th onClick={() => this.props.sortBy('blueSideWins', this.state.specificTeams)}>Blue Side Wins</th>
                            <th onClick={() => this.props.sortBy('blueSideLosses', this.state.specificTeams)}>Blue Side Losses</th>
                            <th onClick={() => this.props.sortBy('redSideWins', this.state.specificTeams)}>Red Side Wins</th>
                            <th onClick={() => this.props.sortBy('redSideLosses', this.state.specificTeams)}>Red Side Losses</th>
                            <th onClick={() => this.props.sortBy('averageGameTime', this.state.specificTeams)}>Average Game Time</th>
                            <th onClick={() => this.props.sortBy('goldPerMinute', this.state.specificTeams)}>Gold Per Minute</th>
                            <th onClick={() => this.props.sortBy('goldDifferencePerMinute', this.state.specificTeams)}>Gold Difference Per Minute</th>
                            <th onClick={() => this.props.sortBy('goldDifferenceAt15', this.state.specificTeams)}>Gold Difference At 15 Mins</th>
                            <th onClick={() => this.props.sortBy('csPerMinute', this.state.specificTeams)}>CS Per Minute</th>
                            <th onClick={() => this.props.sortBy('csDifferenceAt15', this.state.specificTeams)}>CS Difference At 15 Mins</th>
                            <th onClick={() => this.props.sortBy('towerDifferenceAt15', this.state.specificTeams)}>Tower Difference At 15 Mins</th>
                            <th onClick={() => this.props.sortBy('firstTowerPercent', this.state.specificTeams)}>First Tower Taken</th>
                            <th onClick={() => this.props.sortBy('dragonsPerGame', this.state.specificTeams)}>Dragons Per Game</th>
                            <th onClick={() => this.props.sortBy('dragonsAt15', this.state.specificTeams)}>Dragons At 15 Mins</th>
                            <th onClick={() => this.props.sortBy('heraldPerGame', this.state.specificTeams)}>Heralds Per Game</th>
                            <th onClick={() => this.props.sortBy('nashorsPerGame', this.state.specificTeams)}>Barons Per Game</th>
                            <th onClick={() => this.props.sortBy('damagePerMinute', this.state.specificTeams)}>Damage Per Minute</th>
                            <th onClick={() => this.props.sortBy('firstBloodPercent', this.state.specificTeams)}>First Blood</th>
                            <th onClick={() => this.props.sortBy('killsPerGame', this.state.specificTeams)}>Kills Per Game</th>
                            <th onClick={() => this.props.sortBy('deathsPerGame', this.state.specificTeams)}>Deaths Per Game</th>
                            <th onClick={() => this.props.sortBy('kdRatio', this.state.specificTeams)}>KD Ratio</th>
                            <th onClick={() => this.props.sortBy('wardsPerMinute', this.state.specificTeams)}>Wards Per Minute</th>
                            <th onClick={() => this.props.sortBy('wardsClearedPerMinute', this.state.specificTeams)}>Wards Cleared Per Minute</th>
                            <th onClick={() => this.props.sortBy('wardsClearedPercent', this.state.specificTeams)}>Wards Cleared</th>
                            <th onClick={() => this.props.sortBy('cloudDrakesKilled', this.state.specificTeams)}>Cloud Drakes Killed</th>
                            <th onClick={() => this.props.sortBy('cloudDrakesLost', this.state.specificTeams)}>Cloud Drakes Lost</th>
                            <th onClick={() => this.props.sortBy('oceanDrakesKilled', this.state.specificTeams)}>Ocean Drakes Killed</th>
                            <th onClick={() => this.props.sortBy('oceanDrakesLost', this.state.specificTeams)}>Ocean Drakes Lost</th>
                            <th onClick={() => this.props.sortBy('infernalDrakesKilled', this.state.specificTeams)}>Infernal Drakes Killed</th>
                            <th onClick={() => this.props.sortBy('infernalDrakesLost', this.state.specificTeams)}>Infernal Drakes Lost</th>
                            <th onClick={() => this.props.sortBy('mountainDrakesKilled', this.state.specificTeams)}>Mountain Drakes Killed</th>
                            <th onClick={() => this.props.sortBy('mountainDrakesLost', this.state.specificTeams)}>Mountain Drakes Lost</th>
                            <th onClick={() => this.props.sortBy('elderDrakesKilled', this.state.specificTeams)}>Elder Drakes Killed</th>
                            <th onClick={() => this.props.sortBy('elderDrakesLost', this.state.specificTeams)}>Elder Drakes Lost</th>
                        </tr>
                    </thead>
                    <tbody>
                        {this.state.specificTeams.map(team =>
                            <tr onClick={() => this.props.clickedOnTeam(team)} key={team.name} >
                                <td>
                                    <img alt='' src={this.state.Utilities.getImageString(this.props.images, team.name)}></img>
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
                        )}
                    </tbody>
                </Table>
            </div >
        );
    }
}