import React, { Component } from 'react';
import { Navbar, Button, ButtonToolBar, Table } from 'react-bootstrap';

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
          loading: true,
          loadedspecificLeague: false,
          shouldLoadPrelive: false,
          shouldLoadResults: false,
          specificleague: ''
      };
      fetch('api/LeagueOfLegends/GetSport')
          .then(response => response.json())
          .then(data => {
              this.setState({
                  sport: data,
                  results: data.resultsEvents,
                  images: data.teamLogos,
                  prelive: data.preliveEvents,
                  teams: data.teamsInLeagues,
                  loading: false
              });
          });
      
      this.renderResults = this.renderResults.bind(this);
      this.determineLeaguesToAdd = this.determineLeaguesToAdd.bind(this);
      this.renderLeagueTable = this.renderLeagueTable.bind(this);
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
        let ad = teams;
        for (let a = 0; a < results.length; a++) {
            for (let i = 0; i < results.length - 1; i++) {
                if (results[i].gameDate < results[i + 1].gameDate) {
                    tempRes = results[i + 1];
                    results[i + 1] = results[i];
                    results[i] = tempRes;
                }
            }
        }
        {
            let disablePrelive = true;
            if (this.state.specificleague in this.state.prelive)
            {
                disablePrelive = false;
            }
            return (
                <div>
                    <h1>{this.state.specificleague} Results</h1>
                    <Button variant="outline-dark" onClick={() => this.setState({
                        loadedspecificLeague: false,
                        specificleague: ''
                    })}>Back To All Leagues</Button>

                    <Button variant="outline-dark" disabled={disablePrelive} onClick={() => this.setState({
                        shouldLoadPrelive: true,
                        shouldLoadResults: false,
                    })}>Upcoming Games</Button>



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
        return (
            <div>
                <h1>{this.state.specificleague} Upcoming Games</h1> 
                <Button variant="outline-dark" onClick={() => this.setState({
                    loadedspecificLeague: false,
                    specificleague: ''
                })}>Back To All Leagues</Button>

                <Button variant="outline-dark" onClick={() => this.setState({
                    shouldLoadResults: true,
                    shouldLoadPrelive: false
                })}>Results</Button>


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


render() {
    let contents;
    if (this.state.loadedspecificLeague) {
        contents = this.state.shouldLoadResults ?
            this.renderResults(this.state.results[this.state.specificleague], this.state.teams)
            : this.renderPrelive(this.state.prelive[this.state.specificleague]);
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
