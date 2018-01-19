import React, { PropTypes } from 'react'
import {connect} from 'react-redux'
import {browserHistory} from 'react-router'
import {bindActionCreators} from 'redux'
import * as accountActions from '../../actions/accountActions'
import * as searchBarActions from '../../components/SearchBar/searchBarActions'

import RaisedButton from 'material-ui/RaisedButton'
import TextField from 'material-ui/TextField'
import { orange300 } from 'material-ui/styles/colors'

import styles from './styles.css'

class RegisterPage extends React.Component {
    constructor(props, context) {
        super(props, context);

        this.state = {         
            fsLogin: Object.assign({}, props.fsLogin),
            username: "",
            email: "",
            firstName: "",
            lastName: "",
            loginProvider: "",
            token: "",
            invalidUserName: {error: false, msg: null},
            errors: {}
        };

        if(!window.App){
            browserHistory.goBack();
        }
        else {
            this.state.email = window.App.externalAuthInfo.email;
            this.state.firstName = window.App.externalAuthInfo.externalUserName.split(' ')[0];
            this.state.lastName = window.App.externalAuthInfo.externalUserName.split(' ')[1];
            this.state.loginProvider = window.App.externalAuthInfo.loginProvider;
            this.state.token = window.App.externalAuthInfo.externalAccessToken;

            this.setUserName = this.setUserName.bind(this);
            this.doRegistration = this.doRegistration.bind(this);
            delete window.App.externalAuthInfo;
            delete window.App.browserHistory;
        }
    }
	
    componentWillMount() {
        this.props.actions.hideHeaderLogin(this.props.fsLogin);
    }

    componentDidUpdate(prevProps, prevState) {

    }

    componentWillUnmount() {
        if(this.props.fsLogin.hideHeaderLogin == true){
            this.props.actions.leaveLoginPage();
        }
    }

    componentWillReceiveProps(nextProps) {
        this.setState({invalidUserName: {error: false, msg: null}});
        //Check username entry live on the fly
        if(nextProps.searchResults !== null && nextProps.searchResults.resultText != undefined && nextProps.searchResults.resultText != null){
            this.setState({invalidUserName: {error: true, msg: "User Name Already Taken"}});
        } 

        //If status changes from logged in to not logged in, Registration Success, Redirect to Email Confirmation
        if(this.props.fsLogin.confirmationSent == undefined && nextProps.fsLogin.confirmationSent == true) {
            browserHistory.replace('/Account/ConfirmationSent');
        }        
        
        if(nextProps.fsLogin.signup == "Error"){
            if(nextProps.fsLogin.signupError.indexOf("is already taken") != -1)
            {
                App.toastMsgBox.info(
                    "There is already an account associated with this email. Please sign in using that account.",
                    "Account Exists", {
                        timeOut: 7000,
                        extendedTimeOut: 0
                    }
                );
                this.props.actions.leaveRegisterPage();
            }
        }
    }

    setUserName(e) {
        this.setState({username: e.target.value});
        this.props.searchBarActions.queryUserName(e.target.value);
    }

    doRegistration(e) {
        e.preventDefault();
        var registrationItem = {
            userName: this.state.username,
            firstName: this.state.firstName,
            lastName: this.state.lastName,
            email: this.state.email,
            subscriptionPlan: "free",
            provider: this.state.loginProvider,
            externalAccessToken: this.state.token
        };

        this.props.actions.accountSignupExternal(registrationItem);
        this.setState({username: ""});
    }

    render () {
        return (
            <div className="container clearfix w-3xl mt-20" style={{textAlign: 'center'}}>
                <h3><i className="fa fa-refresh fa-spin fa-2x fa-fw"></i>Associate your {this.state.loginProvider} Account</h3>
                <p className="text-success">You have successfully authenticated with <strong>{this.state.loginProvider}</strong>. Please enter a user name below and click the Register button.</p>
                <form onSubmit={this.doRegistration} >
                    <div className="row">
                        <div className="form-group col-md-12">
                            <TextField
                                hintText="Username"
                                floatingLabelText="Username"
                                floatingLabelFocusStyle={{
                                    color: "#FFB74D"
                                }}
                                value={this.state.username}
                                onChange={this.setUserName}
                                errorText={this.state.invalidUserName.error ? this.state.invalidUserName.msg : ""}
                                underlineFocusStyle={{
                                    borderColor: "#FFB74D"
                                }}
                            />
                        </div>

                        <div className="col-md-12">
                            <RaisedButton type="submit" 
                                label="Register" 
                                backgroundColor={orange300}
                                labelColor="#FFF"
                                disabled={this.state.invalidUserName.error || this.state.username == ""}
                            />
                        </div>
                    </div>
                </form>
            </div>
        )
    }
}

function mapStateToProps(state, ownProps) {
    return {
        fsLogin: state.fsLogin,
        loginProvider: state.loginProvider,
        searchResults: state.searchResults.users
    };
}

function mapDispatchToProps(dispatch) {
    return {
        actions: bindActionCreators(accountActions, dispatch),
        searchBarActions: bindActionCreators(searchBarActions, dispatch)
    };
}

export default connect(mapStateToProps, mapDispatchToProps)(RegisterPage);