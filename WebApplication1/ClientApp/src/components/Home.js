import React, { Component } from 'react';
import { LiveEventHub } from './LiveEventHub'
export class Home extends Component {
  displayName = Home.name

  render() {
    return (
        <div>
            <LiveEventHub/>
      </div>
    );
  }
}
