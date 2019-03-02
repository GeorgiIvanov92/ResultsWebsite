import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { LeagueOfLegendsData } from './components/LeagueOfLegendsData';
import { CounterStrikeData } from './components/CounterStrikeData';
import { Dota2Data } from './components/Dota2Data';

export default class App extends Component {
  displayName = App.name
   
  render() {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
            <Route path='/leagueOfLegends' component={LeagueOfLegendsData} />
            <Route path='/counterstrike' component={CounterStrikeData} />
            <Route path='/dota' component={Dota2Data} />
      </Layout>
    );
  }
}
