import React, { Component } from 'react';
import { Utilities } from '../Utilities';
import { Navbar, Button, ButtonToolbar, Table } from 'react-bootstrap';
export class SpecificTeam extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            Utilities: new Utilities(),
            haveSortedTeamsInLeague: false,
            specificStats: []
        };

    }
    render() {
        let team = this.props.specificTeam;
        let players = [];
        let specificStats = [];
        let champStats = this.props.playerStats;
        this.props.players.forEach(function (player) {
            if (player.teamId === team.id) {
                players.push(player);
                specificStats.push(champStats.filter(stat => stat.playerId === player.playerId));
            }
        });
        if (specificStats.length > 0 && (!this.state.specificStats || this.state.specificStats.length === 0)) {
            specificStats = specificStats.filter(stat => stat[0]);
            for (let i = 0; i < specificStats.length; i++) {
                this.props.sortStatsBy('gamesPlayed', specificStats, specificStats[i][0].player.nickname);
            }
            this.setState({
                specificStats: specificStats
            });

        }
        return (
            <div>
                <div className='row'>
                    <div className='column'>
                        <img alt='' src={this.state.Utilities.getImageString(this.props.images, team.name)}>
                        </img>
                    </div>
                    <div className='column'>
                        <h1>{team.name}</h1>
                    </div>
                </div>

                <h2 style={{ textAlign: 'center' }}> Player Stats </h2>
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

                {this.state.specificStats ?
                    <div>
                        {this.state.specificStats.map(stats =>

                            <div>
                                <h2 style={{ textAlign: 'center' }}> {stats[0].player.nickname} Champion Stats </h2>
                                <Table striped bordered hover variant="dark" className='table'>
                                    <thead>
                                        <tr key={stats[0].player.nickname}>
                                            <th onClick={() => this.props.sortStatsBy('championName', this.state.specificStats, stats[0].player.nickname)}>Champion Name</th>
                                            <th onClick={() => this.props.sortStatsBy('gamesPlayed', this.state.specificStats, stats[0].player.nickname)}>Games Played</th>
                                            <th onClick={() => this.props.sortStatsBy('winratePercent', this.state.specificStats, stats[0].player.nickname)}>Winrate</th>
                                            <th onClick={() => this.props.sortStatsBy('kda', this.state.specificStats, stats[0].player.nickname)}>KDA</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {stats.map(stat =>
                                            <tr key={stat.championName + stat.playerId}>
                                                <td>{stat.championName}</td>
                                                <td>{stat.gamesPlayed}</td>
                                                <td>{stat.winratePercent}%</td>
                                                <td>{stat.kda}</td>
                                            </tr>
                                        )}
                                    </tbody>
                                </Table>
                            </div>
                        )}
                    </div>
                    : <h2>No Champion Stats Recorded</h2>}



            </div>
        );
    }
}