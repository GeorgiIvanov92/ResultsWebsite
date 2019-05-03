import React, { Component } from 'react';
import { Utilities } from '../Utilities';
import { Navbar, Button, ButtonToolbar, Table } from 'react-bootstrap';
export class Prelive extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            Utilities: new Utilities(),
        };
    }
    render() {
        let prelive = this.props.prelive;
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
                <h1>{this.props.specificleague} Prelive Games</h1>

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
                                <img alt='' src={this.state.Utilities.getImageString(this.props.images, pre.homeTeam)}></img>
                                <td>{pre.homeTeam}</td>
                                <img alt='' src={this.state.Utilities.getImageString(this.props.images, pre.awayTeam)}></img>
                                <td>{pre.awayTeam}</td>

                                <td>{pre.bestOf}</td>
                            </tr>
                        )}
                    </tbody>
                </Table>
            </div>
        );
    }
}