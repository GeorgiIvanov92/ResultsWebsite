import React, { Component } from 'react';
import { Utilities } from '../Utilities';
import { Navbar, Button, ButtonToolbar, Table } from 'react-bootstrap';
export class Players extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            Utilities: new Utilities(),
            haveSortedTeamsInLeague: false,
            playersInLeague: [],
        };

    }
    render() {
        if (this.state.playersInLeague.length === 0) {
            let players = this.props.players;
            let teamsInLeague = this.props.teamsInLeague;
            let playersInLeague = [];
            teamsInLeague.forEach(function (team) {

                players.forEach(function (player) {

                    if (team.id === player.teamId) {
                        playersInLeague.push(player);
                    }
                });
            });
            if (playersInLeague.length > 0) {
                this.setState({ playersInLeague: playersInLeague });
            }
        }

        return (
            <div>
                <h2 style={{ textAlign: 'center' }}> Players In {this.props.specificleague} </h2>

                <Table striped bordered hover variant='dark' className='table'>
                    <thead>
                        <tr>
                            <th onClick={() => this.props.sortBy('nickname', this.state.playersInLeague)}>Nickname</th>
                            <th onClick={() => this.props.sortBy('wins', this.state.playersInLeague)}>Wins</th>
                            <th onClick={() => this.props.sortBy('losses', this.state.playersInLeague)}>Losses</th>
                            <th onClick={() => this.props.sortBy('kda', this.state.playersInLeague)}>KDA</th>
                            <th onClick={() => this.props.sortBy('csPerMinute', this.state.playersInLeague)}>CS Per Minute</th>
                            <th onClick={() => this.props.sortBy('goldPerMinute', this.state.playersInLeague)}>Gold Per Minute</th>
                            <th onClick={() => this.props.sortBy('goldPercent', this.state.playersInLeague)}>Gold Percentage In Team</th>
                            <th onClick={() => this.props.sortBy('killParticipation', this.state.playersInLeague)}>Kill Participation</th>
                            <th onClick={() => this.props.sortBy('damagePerMinute', this.state.playersInLeague)}>Damage Per Minute</th>
                            <th onClick={() => this.props.sortBy('damagePercent', this.state.playersInLeague)}>Damage Percent</th>
                            <th onClick={() => this.props.sortBy('killsAndAssistsPerMinute', this.state.playersInLeague)}>Kills & Assits Per Minute</th>
                            <th onClick={() => this.props.sortBy('soloKills', this.state.playersInLeague)}>Solo Kills</th>
                            <th onClick={() => this.props.sortBy('pentakills', this.state.playersInLeague)}>Pentakills</th>
                            <th onClick={() => this.props.sortBy('visionScorePerMinute', this.state.playersInLeague)}>Vision Score Per Minute</th>
                            <th onClick={() => this.props.sortBy('wardPerMinute', this.state.playersInLeague)}>Wards Per Minute</th>
                            <th onClick={() => this.props.sortBy('visionWardsPerMinute', this.state.playersInLeague)}>Vision Wards Per Minute</th>
                            <th onClick={() => this.props.sortBy('wardsClearedPerMinute', this.state.playersInLeague)}>Wards Cleared Per Minute</th>
                            <th onClick={() => this.props.sortBy('csDifferenceAt15', this.state.playersInLeague)}>CS Difference at 15 min</th>
                            <th onClick={() => this.props.sortBy('goldDifferenceAt15', this.state.playersInLeague)}>Gold Difference at 15 min</th>
                            <th onClick={() => this.props.sortBy('xpDifferenceAt15', this.state.playersInLeague)}>XP Difference at 15 min</th>
                            <th onClick={() => this.props.sortBy('firstBloodParticipationPercent', this.state.playersInLeague)}>First Blood Participation</th>
                            <th onClick={() => this.props.sortBy('firstBloodVictimPercent', this.state.playersInLeague)}>First Blood Victim</th>
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
}