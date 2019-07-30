import React, { Component } from 'react';
import { LiveEventHub } from './LiveEventHub';
export class Home extends Component {
  displayName = Home.name

  render() {
    return (
        <div>
            <h2 style={{ font: 'oblique' }}> Live Games will appear here if there are any </h2>
            <LiveEventHub sportToLoad="1,3" />
      </div>
    );
  }
}
