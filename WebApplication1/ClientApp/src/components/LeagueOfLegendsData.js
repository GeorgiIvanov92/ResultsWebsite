import React, { Component } from 'react';

export class LeagueOfLegendsData extends Component {
    displayName = LeagueOfLegendsData.name

  constructor(props) {
    super(props);
      this.state = { results: [], loading: true };
    
    fetch('api/EsportsResults/GetResults')
      .then(response => response.json())
      .then(data => {
        this.setState({ results: data, loading: false });
      });
  }

    static renderResults(results) {
        return (
            <table className='table'>
                <thead>
                    <tr>
                        <th>Game Date</th>
                        <th>Home Team</th>
                        <th>Away Team</th>
                        <th>Home Score</th>
                        <th>Away Score</th>
                        <th>League</th>
                    </tr>
                </thead>
                <tbody>
                    {results.map(result =>
                        <tr key={result.gameDate}>
                            <td>{result.gameDate}</td>
                            <td>{result.homeTeam}</td>
                            <td>{result.awayTeam}</td>
                            <td>{result.homeScore}</td>
                            <td>{result.awayScore}</td>
                            <td>{result.leagueName}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        if (this.state.results.length>0)
        console.log(this.state.results[0].AwayTeam);
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : LeagueOfLegendsData.renderResults(this.state.results);

    return (
      <div>
        <h1>League of Legends Results</h1>
            {contents}
      </div>
    );
  }
}
