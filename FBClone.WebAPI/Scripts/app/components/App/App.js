/**
 *
 * App.react.js
 *
 * This component is the skeleton around the actual pages, and should only
 * contain code that should be seen on all pages. (e.g. navigation bar)
 */

import React from 'react'
import {connect} from 'react-redux'
// Import the CSS reset, which HtmlWebpackPlugin transfers to the build folder
//import 'sanitize.css/sanitize.css';

import Img from '../../components/Img'
import Header from '../../components/Header/Header.js'
import Footer from '../../components/Footer/Footer.js'
import A from '../../components/A'

//Import for Material UI
import injectTapEventPlugin from 'react-tap-event-plugin'
injectTapEventPlugin();
import getMuiTheme from 'material-ui/styles/getMuiTheme'
import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider'
import { cyan500, teal300, orange300, pinkA200, grey100, darkBlack, white, fullBlack } from 'material-ui/styles/colors'
import AppBar from 'material-ui/AppBar'
import RaisedButton from 'material-ui/RaisedButton'
import TextField from 'material-ui/TextField'

import styles from './styles.css'

// This replaces the textColor value on the palette
// and then update the keys for each component that depends on it.
// More on Colors: http://www.material-ui.com/#/customization/colors
const muiTheme = getMuiTheme({
    palette: {
        primary1Color: "#F5B11A",
        primary2Color: "#41BC9E",
        primary3Color:  "#F05722",
        accent1Color: pinkA200,
        accent2Color: grey100,
        accent3Color: grey100,
        textColor: darkBlack,
        alternateTextColor: white,
        canvasColor: white,
        borderColor: grey100,
        disabledColor: darkBlack,
        pickerHeaderColor: cyan500,
        clockCircleColor: darkBlack,
        shadowColor: fullBlack,
    },
    appBar: {
        height: 50,
        backgroundColor: teal300
    }
});

class App extends React.Component {
    constructor(props, context) {
        super(props, context);
    };

    render() {
        return (
            <MuiThemeProvider muiTheme={getMuiTheme()}>
                <div id="wrapper" className="clearfix animsition">                    
                    <Header/>
                
                    <section id="content">
                        <div className="content-wrap">
                            {this.props.children}
                        </div>
                    </section>
                  <Footer />
                </div>
            </MuiThemeProvider>
        );
    }
}

App.propTypes = {
  children: React.PropTypes.node
};

function mapStateToProps(state, ownProps) {
    return {
        fsLogin: state.fsLogin
    };
}
export default connect(mapStateToProps)(App);
