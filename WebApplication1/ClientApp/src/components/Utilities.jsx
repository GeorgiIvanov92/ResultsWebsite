import { Component } from 'react';
export class Utilities extends Component {


    getImageString = (images, teamName) => {
        return `data:image/png;base64,${
            images[teamName.toLowerCase()]
            || images[teamName[0].toLowerCase()]

            || images[teamName.toLowerCase().trim().split(' ')[0]]
            || images[teamName.toLowerCase().trim().split(' ')[0][teamName.trim().split(' ')[0].length - 1]]

            || images[teamName.toLowerCase().replace('esports', '').trim()]
            || images[teamName.toLowerCase().replace('esports', '').trim()[0]]

            || images[teamName.toLowerCase().replace('gaming', '').trim()]
            || images[teamName.toLowerCase().replace('gaming', '').trim()[0]]

            || images[teamName.toLowerCase().replace('team', '').trim()]
            || images[teamName.toLowerCase().replace('team', '').trim()[0]]

            || images[teamName.toLowerCase().replace('e-sports club', '').trim()]
            || images[teamName.toLowerCase().replace('e-sports club', '').trim()[0]]

            || images[teamName.toLowerCase().replace('e-sports', '').trim()]
            || images[teamName.toLowerCase().replace('e-sports', '').trim()[0]]

            || images[teamName.toLowerCase().replace('gamers', '').trim()]
            || images[teamName.toLowerCase().replace('gamers', '').trim()[0]]

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
    replaceAll = (str, find, replace) => {
        return str.replace(new RegExp(find, 'g'), replace);
    }
}