import React, { PropTypes } from 'react'
import {connect} from 'react-redux'
import {browserHistory} from 'react-router'
import {bindActionCreators} from 'redux'
import * as accountActions from '../../actions/accountActions'

import RaisedButton from 'material-ui/RaisedButton'
import TextField from 'material-ui/TextField'
import { orange300 } from 'material-ui/styles/colors'

import styles from './styles.css'

class ConfirmationSent extends React.Component {
    constructor(props, context) {
        super(props, context);

        this.state = {         
            confirmationSent: false,
            sendConfirmationBtnText: "Send new confirmation email"
        };
        this.sendNewConfirmation = this.sendNewConfirmation.bind(this);
    }
	
    componentDidUpdate(prevProps, prevState) {
     
    }

    componentWillMount() {
        if(this.props.fsLogin.loginError != undefined) {
            this.props.actions.resetLoginStatus();
        }
        this.props.actions.hideHeaderLogin(this.props.fsLogin);
    }

    componentWillUnmount() {
        if(this.props.fsLogin.hideHeaderLogin == true){
            this.props.actions.leaveLoginPage();
        }
    }

    componentWillReceiveProps(nextProps) {
     
    }

    sendNewConfirmation() {
        this.props.actions.resendConfirmationEmail(this.props.fsLogin.id);
        this.setState({ 
            confirmationSent: true,
            sendConfirmationBtnText: "Confirmation Sent!"
        });
    }

    render () {
        return (
            <div className="container clearfix w-3xl mt-20" style={{textAlign: 'center'}}>
                <h3><i className="fa fa-info-circle fa-2x fa-fw"></i>Confirm Your Account</h3>
                <p className="text-success text-lg">Please check your email {this.props.fsLogin.email !== undefined ? "at " + this.props.fsLogin.email : "" } to confirm your account.</p>               
                <button className="btn btn-secondary account-action-button" disabled={this.state.confirmationSent} onTouchTap={this.sendNewConfirmation} 
                    style={this.props.fsLogin.id == null ? {display:"none"} : {display:"block"}}
                >
                    {this.state.sendConfirmationBtnText}
                </button>
            </div>
        )
    }
}

function mapStateToProps(state, ownProps) {
    return {
        fsLogin: state.fsLogin,
        loginProvider: state.loginProvider
    };
}

function mapDispatchToProps(dispatch) {
    return {
        actions: bindActionCreators(accountActions, dispatch)
    };
}

export default connect(mapStateToProps, mapDispatchToProps)(ConfirmationSent);