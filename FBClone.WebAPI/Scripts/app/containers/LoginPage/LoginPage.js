import React, { PropTypes } from 'react'
import {connect} from 'react-redux'
import {browserHistory} from 'react-router'
import {bindActionCreators} from 'redux'
import * as accountActions from '../../actions/accountActions'
import config from '../../config'

import RaisedButton from 'material-ui/RaisedButton'
import FlatButton from 'material-ui/FlatButton'
import TextField from 'material-ui/TextField'
import { orange300, teal300 } from 'material-ui/styles/colors'

import styles from './styles.css'

class LoginPage extends React.Component {
    constructor(props, context) {
        super(props, context);
        const apiServiceBaseUri = config.api.baseUrl;
        const clientId = config.clientId;
        const env = config.env;

        this.state = {
            apiServiceBaseUri: apiServiceBaseUri,
            env: env,
            clientId: clientId, 
            fsLogin: Object.assign({}, props.fsLogin),
            username: "",
            password: "",
            errors: {}
        };

        this.setUserName = this.setUserName.bind(this);
        this.setPassword = this.setPassword.bind(this);
        this.doLogin = this.doLogin.bind(this);
        //TODO: Move Social Login into its own component for use with LoginPage
        this.doExternalLogin = this.doExternalLogin.bind(this);
        this.redirectConfirmationSent = this.redirectConfirmationSent.bind(this);
        this.facebookTouchTap = this.facebookTouchTap.bind(this);
        this.googleTouchTap = this.googleTouchTap.bind(this);
        this.forgotPasswordTap = this.forgotPasswordTap.bind(this);
    }
	
    componentWillMount() {
         var app = Object.assign({}, window.App, 
            {
                browserHistory: browserHistory,
                doExternalLogin: this.doExternalLogin,
                redirectConfirmationSent: this.redirectConfirmationSent
            }
        );
        window.App = app;
        if(this.props.fsLogin.loginError != undefined) {
            this.props.actions.resetLoginStatus();
        }
        this.props.actions.hideHeaderLogin(this.props.fsLogin);
    }

    componentDidUpdate(prevProps, prevState) {
        if(this.props.fsLogin.isLoggedIn == true){
            //Redirect to Main Page
            if(document.referrer !== undefined && document.referrer.toUpperCase().indexOf('/LOGIN') == -1){
                window.location = document.referrer.replace(document.location.origin, "");
            } else {
                browserHistory.push('/');
            }
        }
    }

    componentWillUnmount() {
        if(this.props.fsLogin.hideHeaderLogin == true){
            this.props.actions.leaveLoginPage();
        }
    }

    setUserName(e) {
        this.setState({username: e.target.value});
    }

    setPassword(e) {
        this.setState({password: e.target.value});
    }

    doLogin(e) {
        e.preventDefault();
        this.props.actions.accountLogin(this.state.username, this.state.password);
        this.setState({username: "", password: ""});
    }

    doExternalLogin(externalAuthInfo) {
        delete window.App.doExternalLogin;
        delete window.App.browserHistory;
        this.props.actions.obtainAccessToken(externalAuthInfo).then(function() {
            browserHistory.replace('/');
        });        
    }
    
    redirectConfirmationSent(externalAuthInfo) {
        delete window.App.redirectConfirmationSent;
        delete window.App.browserHistory;
        this.props.actions.updateFSLogin(externalAuthInfo.userId, externalAuthInfo.email);
        browserHistory.replace('/Account/ConfirmationSent');
    }
  
    facebookTouchTap(e) {
        e.preventDefault();
        var redirectUri = this.state.apiServiceBaseUri + '/authComplete';
        if(this.state.env == "PROD"){ //TODO: Remove this hack later
            redirectUri = redirectUri.replace('https', 'http');
        }
        var externalProviderUrl = this.state.apiServiceBaseUri + "/api/Account/ExternalLogin?provider=Facebook"
                                                            + "&response_type=token&client_id=" + this.state.clientId
                                                            + "&redirect_uri=" + redirectUri;
        var oauthWindow = window.open(externalProviderUrl, "Authenticate Account", "location=0,status=0,width=600,height=750");
    }

    googleTouchTap(e) {
        e.preventDefault();
        var redirectUri = this.state.apiServiceBaseUri + '/authComplete';
        if(this.state.env == "PROD"){ //TODO: Remove this hack later
            redirectUri = redirectUri.replace('https', 'http');
        }
        var externalProviderUrl = this.state.apiServiceBaseUri + "/api/Account/ExternalLogin?provider=Google"
                                                            + "&response_type=token&client_id=" + this.state.clientId
                                                            + "&redirect_uri=" + redirectUri;
        var oauthWindow = window.open(externalProviderUrl, "Authenticate Account", "location=0,status=0,width=600,height=750");
    }

    forgotPasswordTap(e) {
        e.preventDefault();
        browserHistory.push('/Account/ForgotPassword');
    }

    render () {
        return (
            <div className="container clearfix w-3xl mt-20">
                <h3><i className="fa fa-lock"></i> Login to your account</h3>
                <form onSubmit={this.doLogin} >
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
                                underlineFocusStyle={{
                                    borderColor: "#FFB74D"
                                }}
                            />
                        </div>

                        <div className="form-group col-md-12">
                            <TextField
                                type="password"
                                hintText="Password"
                                floatingLabelText="Password"
                                floatingLabelFocusStyle={{
                                    color: "#FFB74D"
                                }}
                                value={this.state.password}
                                onChange={this.setPassword}
                                underlineFocusStyle={{
                                    borderColor: "#FFB74D"
                                }}
                            />
                        </div>

                        <div className="col-md-12">
                            <RaisedButton type="submit" label="Login" backgroundColor={orange300} labelColor="#FFF"/>
                        </div>
                    </div>
                </form>

                <FlatButton className="pull-right" label="Forgot Password?" style={{color: teal300}} onTouchTap={this.forgotPasswordTap}/>

                <div className="header-line mt-40">
                    <h4>Or login with</h4>
                 </div>

                <div className="social">
                    <a className="social-icon social-facebook" href="#" onTouchTap={this.facebookTouchTap}>
                        <div className="front">
                            <i className="fa fa-2x fa-facebook"></i>
                        </div>
                        <div className="back">
                            <i className="fa fa-2x fa-facebook"></i>
                        </div>
                    </a>

                    <a className="social-icon social-googleplus" href="#" onTouchTap={this.googleTouchTap}>
                        <div className="front">
                            <i className="fa fa-2x fa-google-plus"></i>
                        </div>
                        <div className="back">
                            <i className="fa fa-2x fa-google-plus"></i>
                        </div>
                     </a>
                </div>
            </div>
        )
    }
}

function mapStateToProps(state, ownProps) {
    return {
        fsLogin: state.fsLogin
    };
}

function mapDispatchToProps(dispatch) {
    return {
        actions: bindActionCreators(accountActions, dispatch)
    };
}

export default connect(mapStateToProps, mapDispatchToProps)(LoginPage);