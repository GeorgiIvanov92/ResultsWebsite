import React, { Component } from 'react';
import { Utilities } from '../Utilities';
import { Navbar, Button, ButtonToolbar, Table } from 'react-bootstrap';
export class Leagues extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            Utilities: new Utilities(),
        };
    }
    render() {
        let results = this.props.data.resultsEvents;
        let prelive = this.props.data.preliveEvents;
        let teams = this.props.data.teamsInLeague;
        let arr = [];
        for (let key in results) {
            arr.push(key);
        }
        for (let key in prelive) {
            if (!arr.includes(key)) {
                arr.push(key);
            }
        }
        for (let key in teams) {
            if (!arr.includes(key)) {
                arr.push(key);
            }
        }
        return (
            <div>
                <h1>{this.props.gameName}</h1>
                {arr.map(league =>

                    <Navbar key={league} inverse style={{ width: '50%' }} onClick={() => this.props.clickedOnLeague(league)}>
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
}