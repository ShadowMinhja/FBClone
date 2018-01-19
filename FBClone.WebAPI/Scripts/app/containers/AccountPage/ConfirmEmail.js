import React, { PropTypes } from 'react'
import {connect} from 'react-redux'
import {browserHistory} from 'react-router'
import {bindActionCreators} from 'redux'
import * as accountActions from '../../actions/accountActions'

import RaisedButton from 'material-ui/RaisedButton'
import TextField from 'material-ui/TextField'
import { orange300 } from 'material-ui/styles/colors'

import styles from './styles.css'

class ConfirmEmail extends React.Component {
    constructor(props, context) {
        super(props, context);

        this.state = {         
            confirmationSent: false,
            messageText: "Confirming account..."
        };

        this.sendNewConfirmation = this.sendNewConfirmation.bind(this);
    }
	
    sendNewConfirmation() {
        this.props.actions.resendConfirmationEmail(this.props.userId);
        browserHistory.replace('/Account/ConfirmationSent');
    }

    componentWillMount() {
        this.props.actions.hideHeaderLogin(this.props.fsLogin);
        var confirmationInfo = {
            userId: this.props.userId,
            code: this.props.code
        };
        this.props.actions.confirmEmail(confirmationInfo);
    }

    componentWillUnmount() {
        if(this.props.fsLogin.hideHeaderLogin == true){
            this.props.actions.leaveLoginPage();
        }
    }

    componentDidUpdate(prevProps, prevState) {
        const ConfirmEmail = this;
        if(this.props.fsLogin.confirmationError == false && this.props.fsLogin.isLoggedIn != undefined){
            if(this.props.loginProvider != null) {
                var externalAuthInfo = {
                    externalAccessToken: this.props.externalAccessToken,
                    loginProvider: this.props.loginProvider                  
                };
                this.props.actions.obtainAccessToken(externalAuthInfo).then(function() {
                    if(ConfirmEmail.props.fsLogin.confirmationError == undefined) {
                        ConfirmEmail.setState({
                            messageText: "Your account has been confirmed successfully! Please wait to be redirected to fbClone!"
                        });
                        setTimeout(function() {
                            browserHistory.replace('/');
                        }, 7000);
                    }
                });        
            } else {
                //Redirect to Main Page
                setTimeout(function() {
                    browserHistory.replace('/');
                }, 7000);
            }
        }
    }

    componentWillReceiveProps(nextProps) {
        if(nextProps.fsLogin.confirmationError == false){
            this.setState({
                messageText: "Your account has been confirmed successfully! Please wait to be redirected to fbClone!"
            });
        }        
        else if(nextProps.fsLogin.confirmationError !== undefined){
            this.setState({
                messageText: nextProps.fsLogin.confirmationError
            });
        } 
        else {
            this.setState({
                messageText: "Please wait..."
            });
        }
    }

    renderIcon () {
        if(this.props.fsLogin.confirmationError == undefined || this.props.fsLogin.confirmationError == false){
            return (
                <div>
                    <p className="text-success text-lg">{this.state.messageText}</p>       
                    <h3><i className="fa fa-refresh fa-spin fa-2x fa-fw"></i></h3>
                </div>
            );
        } else {
            return (
               <div>
                   <p className="text-success text-lg">{this.state.messageText}</p>       
                   <h3><i className="fa fa-frown-o fa-2x fa-fw"></i></h3>
                   <button className="btn btn-secondary account-action-button" onTouchTap={this.sendNewConfirmation} 
                        style={this.props.fsLogin.id == null ? {display:"none"} : {display:"block"}}
                    >
                        Click to send new confirmation email
                    </button>
               </div>
            );
        }
    }

    render () {
        return (
            <div className="container clearfix w-3xl mt-20" style={{textAlign: 'center'}}>
                <h3><i className="fa fa-check-circle-o fa-2x fa-fw"></i>Confirm Your Account</h3>
                {this.renderIcon()}                
            </div>
        )
    }
}

function mapStateToProps(state, ownProps) {    
    return {
        fsLogin: state.fsLogin,
        userId: ownProps.location.query.userId,
        code: ownProps.location.query.code.replace(new RegExp(' ', 'g'), '+') ,
        loginProvider: ownProps.location.query.loginProvider, 
        externalAccessToken: ownProps.location.query.externalAccessToken
    };
}

function mapDispatchToProps(dispatch) {
    return {
        actions: bindActionCreators(accountActions, dispatch)
    };
}

export default connect(mapStateToProps, mapDispatchToProps)(ConfirmEmail);