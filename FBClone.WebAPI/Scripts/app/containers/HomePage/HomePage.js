/**
 * A link to a certain page, an anchor tag
 */

import React from 'react'
import {connect} from 'react-redux'
import {bindActionCreators} from 'redux'
import {browserHistory} from 'react-router'

import GemPlayArea from '../../components/GemPlayArea/GemPlayArea.js'
import HomeSignedOut from '../../components/HomeSignedOut/HomeSignedOut.js'
import FeedForm from '../../components/FeedForm/FeedForm.js'
import WallFeed from '../../components/WallFeed/WallFeed.js'

import styles from './styles.css'

class HomePage extends React.Component {
    constructor(props, context) {
        super(props, context);

        this.renderWallFeed = this.renderWallFeed.bind(this);        
    }

    renderWallFeed() {
        if(this.props.fsLogin.isLoggedIn){
            return (
                <WallFeed />
            );
        }
        else 
            return null;
    }
    
    renderHomeSignedOut() {
        if(this.props.fsLogin.isLoggedIn == true) {
            return null;
        }
        else {
            return(
                <HomeSignedOut />
            );
        }
    }

    render() {
        const { fsLogin} = this.props;

        return (
            <div>
                {this.renderHomeSignedOut()}
                <div className="container clearfix mt-20" style={fsLogin.isLoggedIn == true ? {display:"block"} : {display:"none"}}>
                    {/*
                    <div className="mb-60">
                        <h2>Find Your Hidden Gems</h2>
                        <p>With all the food and restaurant choices available, it's like a jungle out there! We have the survival guide just for you!</p>
                    </div>
                    */}

                    <FeedForm /><br/>

                    {this.renderWallFeed()}
                </div>
            </div>
        );
    }
}

function mapStateToProps(state, ownProps) {
    return {
        fsLogin: state.fsLogin
    };
}

function mapDispatchToProps(dispatch) {
    return {
        //actions: bindActionCreators(feedActions, dispatch)
    };
}

export default connect(mapStateToProps, mapDispatchToProps)(HomePage);