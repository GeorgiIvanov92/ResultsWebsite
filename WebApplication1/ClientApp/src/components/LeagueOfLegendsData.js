import React, { Component } from 'react';
import { Navbar, Nav, NavItem } from 'react-bootstrap';

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
        determineLeaguesToAdd(results)
    {
        //let mappedLeagues = [];
        //results.map(result => {
        //    if (mappedLeagues.indexOf(result.leagueName) === -1) {
        //        mappedLeagues.push(result.leagueName);
        //    }
        //});
        let arr = [];
        Object.keys(results).forEach(function (key) {
            arr.push(key);
        });
        return (
            this.renderLeagueTable(arr)
        );
    }   

    renderResults(results) {
        return (
            <table className='table'>
                <thead>
                    <tr>
                        <th>League</th>
                        <th>Game Date</th>
                        <th>Home Team</th>
                        <th>Away Team</th>
                        <th>Home Score</th>
                        <th>Away Score</th>
                    </tr>
                </thead>
                <tbody>
                    {results.map(result =>
                        <tr key={result.gameDate + "@" + result.homeTeam}>
                            <td>{result.leagueName}</td>
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
                            <td>{result.awayTeam}</td>
                            <td>{result.homeScore}</td>
                            <td>{result.awayScore}</td>
                        </tr>
                    )}
                </tbody>
            </table>
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
            <h1>{this.state.specificleague} Results</h1>           
            {contents}
      </div>
    );
  }
}
