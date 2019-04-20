import React, { Component } from 'react';
export class Utilities extends Component {


    getImageString = (images, teamName) => {
        return `data:image/png;base64,${images[teamName]
            || images[teamName.toLowerCase()]
            || images[teamName.toUpperCase()]
            || images[teamName[0].toLowerCase()]
            || images[teamName[0].toUpperCase()]

            || images[teamName.trim().split(' ')[0]]
            || images[teamName.trim().split(' ')[0].toLowerCase()]
            || images[teamName.trim().split(' ')[0].toUpperCase()]
            || images[teamName.trim().split(' ')[0][0].toUpperCase()]
            || images[teamName.trim().split(' ')[0][teamName.trim().split(' ')[0].length - 1].toUpperCase()]
            || images[teamName.trim().split(' ')[0][teamName.trim().split(' ')[0].length - 1].toLowerCase()]

            || images[teamName.replace('Esports', '').trim()]
            || images[teamName.replace('Esports', '').trim().toUpperCase()]
            || images[teamName.replace('Esports', '').trim().toLowerCase()]
            || images[teamName.replace('Esports', '').trim()[0].toUpperCase()]
            || images[teamName.replace('Esports', '').trim()[0].toLowerCase()]

            || images[teamName.replace('eSports', '').trim()]
            || images[teamName.replace('eSports', '').trim().toUpperCase()]
            || images[teamName.replace('eSports', '').trim().toLowerCase()]
            || images[teamName.replace('eSports', '').trim()[0].toUpperCase()]
            || images[teamName.replace('eSports', '').trim()[0].toLowerCase()]

            || images[teamName.replace('Gaming', '').trim()]
            || images[teamName.replace('Gaming', '').trim().toUpperCase()]
            || images[teamName.replace('Gaming', '').trim().toLowerCase()]
            || images[teamName.replace('Gaming', '').trim()[0].toUpperCase()]
            || images[teamName.replace('Gaming', '').trim()[0].toLowerCase()]

            || images[teamName.replace('Team', '').trim()]
            || images[teamName.replace('Team', '').trim().toUpperCase()]
            || images[teamName.replace('Team', '').trim().toLowerCase()]
            || images[teamName.replace('Team', '').trim()[0].toUpperCase()]
            || images[teamName.replace('Team', '').trim()[0].toLowerCase()]

            || images[teamName.replace('e-Sports Club', '').trim()]
            || images[teamName.replace('e-Sports Club', '').trim().toUpperCase()]
            || images[teamName.replace('e-Sports Club', '').trim().toLowerCase()]
            || images[teamName.replace('e-Sports Club', '').trim()[0].toUpperCase()]
            || images[teamName.replace('e-Sports Club', '').trim()[0].toLowerCase()]

            || images[teamName.replace('e-Sports', '').trim()]
            || images[teamName.replace('e-Sports', '').trim().toUpperCase()]
            || images[teamName.replace('e-Sports', '').trim().toLowerCase()]
            || images[teamName.replace('e-Sports', '').trim()[0].toUpperCase()]
            || images[teamName.replace('e-Sports', '').trim()[0].toLowerCase()]

            || images[teamName.replace('Gamers', '').trim()]
            || images[teamName.replace('Gamers', '').trim().toUpperCase()]
            || images[teamName.replace('Gamers', '').trim().toLowerCase()]
            || images[teamName.replace('Gamers', '').trim()[0].toUpperCase()]
            || images[teamName.replace('Gamers', '').trim()[0].toLowerCase()]

            || images['default']}`;

    }
    findTeamSpecificLeague = (team, allTeams) => {
        let correctLeague = '';
        for (var key in allTeams) {
            allTeams[key].forEach(function (t) {
                if (t.name === team) {
                    correctLeague = key;   
                }
            });
        }
        return correctLeague;

    }
}