import React, { Component } from 'react';
import { Navbar, Button, ButtonToolBar } from 'react-bootstrap';

export class LeagueOfLegendsData extends Component {
   

    displayName = LeagueOfLegendsData.name

  constructor(props) {
    super(props);
      this.state = {
          results: [], loading: true, loadedspecificLeague: false, specificleague: ''
      };
    fetch('api/LeagueOfLegends/GetResults')
      .then(response => response.json())
      .then(data => {
          this.setState({
              results: data, loading: false
          });
          });
      this.renderResults = this.renderResults.bind(this);
      this.determineLeaguesToAdd = this.determineLeaguesToAdd.bind(this);
      this.renderLeagueTable = this.renderLeagueTable.bind(this);
    }
    renderLeagueTable(arr) {
        return (
            <div>
                {arr.map(league =>

                    <Navbar inverse style={{ width: '50%' }} onClick={() => this.setState({
                        loadedspecificLeague: true,
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

    renderResults(results) {
        let tempRes;
        for (let a = 0; a < results.length; a++)
        {
            for (let i = 0; i < results.length - 1; i++)
            {
                if (results[i].gameDate < results[i + 1].gameDate) {
                    tempRes = results[i + 1];
                    results[i + 1] = results[i];
                    results[i] = tempRes;
                }
            }
        }
        return (
            <div>
           
                <Button variant="outline-dark" onClick={() => this.setState({
                    loadedspecificLeague: false,
                    specificleague: ''
                })}>Back To All Leagues</Button>


            <table className='table'>
                <thead>
                        <tr>
                            <th>Game Date</th>
                            <th>Home Team</th>
                            <th>Home Score</th>
                            <th>Away Score</th>
                            <th>Away Team</th>
                            <th>League</th>
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
                                <td>{result.homeScore}</td>
                                <td>{result.awayScore}</td>
                                <td>{result.awayTeam}</td>
                                <td>{result.leagueName}</td>
                            </tr>
                    )}
                </tbody>
                </table>
                </div>
        );
    }


render() {
    let contents;
    if (this.state.loadedspecificLeague) {
        contents = this.renderResults(this.state.results[this.state.specificleague]);
    } else {
        contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.determineLeaguesToAdd(this.state.results);
    }

    return (
        <div>
            <h1>{this.state.specificleague.length <1 ? 'League of Legends ' : this.state.specificleague} Results</h1>           
            {contents}
      </div>
    );
  }
}
