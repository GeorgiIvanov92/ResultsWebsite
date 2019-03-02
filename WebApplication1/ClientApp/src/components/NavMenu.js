import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Glyphicon, Nav, Navbar, NavItem } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';
import './NavMenu.css';
import leagueLogo from './Resources/Images/leagueLogo.png';
import dotaLogo from './Resources/Images/dotaLogo.png';
import csLogo from './Resources/Images/csLogo.png';

export class NavMenu extends Component {
  displayName = NavMenu.name

  render() {
    return (
      <Navbar inverse fixedTop fluid collapseOnSelect>
        <Navbar.Header>
          <Navbar.Brand>
            <Link to={'/'}>Esports Stats</Link>
          </Navbar.Brand>
          <Navbar.Toggle />
        </Navbar.Header>
        <Navbar.Collapse>
          <Nav>
            <LinkContainer to={'/'} exact>
              <NavItem>
                <Glyphicon glyph='home' /> Home
              </NavItem>         
            </LinkContainer>
            <LinkContainer to={'/LeagueOfLegends'}>
                        <NavItem>
                            <img style={{width: '50px',height: '50px'}}src={leagueLogo} alt="Logo"  /> League of Legends
              </NavItem>
                    </LinkContainer>
                    <LinkContainer to={'/Dota'}>
                        <NavItem>
                            <img style={{ width: '50px', height: '50px' }} src={dotaLogo} alt="Logo" /> Dota 2
              </NavItem>
                    </LinkContainer>
                    <LinkContainer to={'/counterstrike'}>
                        <NavItem>
                            <img style={{ width: '50px', height: '50px' }} src={csLogo} alt="Logo" /> CS:GO
              </NavItem>
                    </LinkContainer>
          </Nav>
        </Navbar.Collapse>
      </Navbar>
    );
  }
}
