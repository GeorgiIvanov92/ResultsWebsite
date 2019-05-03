import React, { Component } from 'react';
import { Utilities } from '../Utilities';
import { Navbar, Button, ButtonToolbar, Table } from 'react-bootstrap';
export class Results extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            Utilities: new Utilities(),
        };
    }
    render() {
        let results = this.props.results;
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
                <h1>{this.props.specificleague} Results</h1>
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
                                <img alt='' src={this.state.Utilities.getImageString(this.props.images, result.homeTeam)} />
                                <td>{result.homeScore}</td>
                                <td>{result.awayScore}</td>
                                <img alt='' src={this.state.Utilities.getImageString(this.props.images, result.awayTeam)} />
                                <td>{result.awayTeam}</td>
                            </tr>
                        )}
                    </tbody>
                </Table>
            </div>
        );
    }
}