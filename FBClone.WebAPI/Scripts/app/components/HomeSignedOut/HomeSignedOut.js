import React, { PropTypes } from 'react'
import {connect} from 'react-redux'
import {browserHistory} from 'react-router'
import {bindActionCreators} from 'redux'
import * as accountActions from '../../actions/accountActions'
import * as searchBarActions from '../SearchBar/searchBarActions'
import * as _ from 'lodash'
import config from '../../config'

import FontIcon from 'material-ui/FontIcon'
import {Card, CardHeader, CardTitle, CardText} from 'material-ui/Card'
import Dialog from 'material-ui/Dialog'
import Divider from 'material-ui/Divider'
import FlatButton from 'material-ui/FlatButton'
import Paper from 'material-ui/Paper'
import RaisedButton from 'material-ui/RaisedButton'
import TextField from 'material-ui/TextField'
import {teal300, grey300} from 'material-ui/styles/colors'
import GemPlayArea from '../../components/GemPlayArea/GemPlayArea.js'

import styles from './styles.css'

class HomeSignedOut extends React.Component {    
    constructor(props, context) {
        super(props, context);
        const apiServiceBaseUri = config.api.baseUrl;
        const clientId = config.clientId;
        const env = config.env;

        this.state = {
            apiServiceBaseUri: apiServiceBaseUri,
            env: env,
            clientId: clientId, 
            showSignup: false,
            userName: "",
            firstName: "",
            lastName: "",
            password: "",
            confirmPassword: "",
            email: "",
            subscriptionPlan: "free",
            invalidUserName: {error: false, msg: null},
            invalidFirstName: {error: false, msg: null},
            invalidLastName: {error: false, msg: null},
            invalidEmail: {error: false, msg: null},
            invalidPassword: {error: false, msg: null},
            invalidConfirmPassword: {error: false, msg: null},
            signupErrorText: []
        };
        this.handleShowSignupReset = this.handleShowSignupReset.bind(this);
        this.setUserName = this.setUserName.bind(this);
        this.setFirstName = this.setFirstName.bind(this);
        this.setLastName = this.setLastName.bind(this);
        this.setOrganizationName = this.setOrganizationName.bind(this);
        this.setPassword = this.setPassword.bind(this);        
        this.setConfirmPassword = this.setConfirmPassword.bind(this);        
        this.setEmail = this.setEmail.bind(this);
        this.setSubscriptionPlan = this.setSubscriptionPlan.bind(this);
        this.doRegister = this.doRegister.bind(this);
        //TODO: Move Social Login into its own component for use with LoginPage
        this.doExternalLogin = this.doExternalLogin.bind(this);
        this.redirectConfirmationSent = this.redirectConfirmationSent.bind(this);
        this.facebookTouchTap = this.facebookTouchTap.bind(this);
        this.googleTouchTap = this.googleTouchTap.bind(this);
    };

    componentWillMount() {
        var app = Object.assign({}, window.App, 
            {
                browserHistory: browserHistory,
                doExternalLogin: this.doExternalLogin,
                redirectConfirmationSent: this.redirectConfirmationSent
            }
        );
        window.App = app;        
    }

    componentWillReceiveProps (nextProps) {
        const HomeSignedOut = this;
        //Reset Error States
        HomeSignedOut.setState({invalidUserName: {error: false, msg: null}});
        HomeSignedOut.setState({invalidFirstName: {error: false, msg: null}});
        HomeSignedOut.setState({invalidLastName: {error: false, msg: null}});
        HomeSignedOut.setState({invalidEmail: {error: false, msg: null}});
        HomeSignedOut.setState({invalidPassword: {error: false, msg: null}});
        HomeSignedOut.setState({invalidConfirmPassword: {error: false, msg: null}});
        //If signup Error
        if(nextProps.fsLogin.signupError !== undefined){
            this.setState({signupErrorText: nextProps.fsLogin.signupError});
            if(Array.isArray(nextProps.fsLogin.signupError) == false){
                nextProps.fsLogin.signupError = [nextProps.fsLogin.signupError];
            }
            _.forEach(nextProps.fsLogin.signupError, function(obj){
                if(obj.indexOf("User Name") != -1)
                    HomeSignedOut.setState({invalidUserName: {error: true, msg: obj}});
                if(obj.indexOf("First Name") != -1)
                    HomeSignedOut.setState({invalidFirstName: {error: true, msg: obj}});
                if(obj.indexOf("Last Name") != -1)
                    HomeSignedOut.setState({invalidLastName: {error: true, msg: obj}});
                if(obj.indexOf("Email") != -1)
                    HomeSignedOut.setState({invalidEmail: {error: true, msg: obj}});
                if(obj.indexOf("Password") != -1)
                    HomeSignedOut.setState({invalidPassword: {error: true, msg: obj}});
            });
        }

        //Show Signup
        if(nextProps.fsLogin.showSignup !== undefined){
            this.setState({showSignup: nextProps.fsLogin.showSignup});
        } else {
            this.setState({showSignup: false});
        }

        //Check username entry live on the fly
        if(nextProps.searchResults !== null && nextProps.searchResults.resultText != undefined && nextProps.searchResults.resultText != null){
            this.setState({invalidUserName: {error: true, msg: "User Name Already Taken"}});
        } 

        //Redirect to confirmation sent page
        if(nextProps.fsLogin.isLoggedIn == false && nextProps.fsLogin.confirmationSent == true) {
            HomeSignedOut.props.actions.clearConfirmationSent();
            browserHistory.replace('/Account/ConfirmationSent');
        }
    }

    handleShowSignupReset() {        
        this.props.actions.showSignup(false);
    }

    setUserName(e) {
        this.setState({userName: e.target.value});
        this.props.searchBarActions.queryUserName(e.target.value);
    }

    setFirstName(e) {
        this.setState({firstName: e.target.value});
    }

    setLastName(e) {
        this.setState({lastName: e.target.value});
    }

    setOrganizationName(e) {
        this.setState({setOrganizationName: e.target.value});
    }

    setPassword(e) {
        this.setState({password: e.target.value});
    }
    
    setConfirmPassword(e) {
        this.setState({confirmPassword: e.target.value});
        if(e.target.value != this.state.password){
            this.setState({invalidConfirmPassword: {error: true, msg: "Passwords must be the same"}});
        } else {
            this.setState({invalidConfirmPassword: {error: false, msg: null}});
        }
    }

    setEmail(e) {
        this.setState({email: e.target.value});
    }

    setSubscriptionPlan(e) {
        this.setState({setSubscriptionPlan: e.target.value});
    }

    doRegister(e) {
        e.preventDefault();
        if(this.state.password == this.state.confirmPassword) {
            var registrationItem = {
                userName: this.state.userName,
                firstName: this.state.firstName,
                lastName: this.state.lastName,
                password: this.state.password,
                email: this.state.email,
                subscriptionPlan: "free"
            };

            this.props.actions.accountSignup(registrationItem);
        }
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
        //this.props.actions.authComplete();
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

    render() {
        const actions = [
            <FlatButton
                label="X Close"
                primary={true}
                onTouchTap={this.handleShowSignupReset}
            />
        ];
        return (
            <div style={this.props.fsLogin.isLoggedIn == false ? {display:"block"} : {display:"none"}}>
                <div className="container">
                    <GemPlayArea />
                </div>
                <div id="title-section" className="container clearfix">
                    <div className="heading-block center">
                        <h1 className="text-uppercase">Revolutionizing your personal health</h1>
                        <p className="lead">by putting <strong><u>you</u></strong> in control</p>
                    </div>
                </div>

                <a href="#" className="myBtn myBtn-block myBtn-dark text-center">
                    <div className="container clearfix">
                        What do these things have in common? They are all crucial to your <strong>health!</strong>
                    </div>
                </a>       

                {/*<!-- ============ portfolio section ============ -->*/}
                <div id="portfolio" className="portfolio portfolio-full portfolio-nospace portfolio-notitle">
                    <div className="row">
                        <div className="portfolio-item col-md-3 col-sm-6">
                            <div className="portfolio-image">
                                <a href="#">
                                    <img src="/corporate/assets/images/portfolio/burgers.jpg" alt="" className="img-responsive"/>
                                </a>
                            </div>
                        </div>

                        <div className="portfolio-item col-md-3 col-sm-6">
                            <div className="portfolio-image">
                                <a href="#">
                                    <img src="/corporate/assets/images/portfolio/pizza.jpg" alt="" className="img-responsive" />
                                </a>
                            </div>
                        </div>

                        <div className="portfolio-item col-md-3 col-sm-6">
                            <div className="portfolio-image">
                                <a href="#">
                                    <img src="/corporate/assets/images/portfolio/pills.jpg" alt="" className="img-responsive" />
                                </a>
                            </div>
                        </div>

                        <div className="portfolio-item col-md-3 col-sm-6">
                            <div className="portfolio-image">
                                <a href="#">
                                    <img src="/corporate/assets/images/portfolio/runner.jpg" alt="" className="img-responsive" />
                                </a>
                            </div>
                        </div>
                    </div>
                </div>

                {/*<!-- ============ portfolio section ============ -->*/}             
                    
                <Dialog
                    actions={actions}
                    modal={true}
                    open={this.state.showSignup}
                    contentStyle={{width: '80%', maxWidth: 'none'}}
                    autoScrollBodyContent={true}
                >
                    <div className="row">
                        <div className="col-md-8 sign-up-area-left">
                        </div>
                        <div className="col-md-4 sign-up-area">
                            <h3><i className="fa fa-lock"></i> Sign up </h3>
                            <form onSubmit={this.doRegister}>
                                <Paper zDepth={2}>
                                    <TextField hintText="User Name" floatingLabelText="User Name" 
                                        style={{
                                            width: '95%',
                                            height: 60
                                        }}
                                        floatingLabelFocusStyle={{
                                            color: '#FFB74D'
                                        }}
                                        fullWidth={true}
                                        value={this.state.userName}
                                        onChange={this.setUserName}
                                        underlineShow={false}
                                        errorText={this.state.invalidUserName.error ? this.state.invalidUserName.msg : ""}
                                        className="signUpInput"
                                    />

                                {/*
                                    <Divider />
                                    <TextField hintText="First name" floatingLabelText="First Name" 
                                        style={{
                                            width: '95%',
                                            height: 60
                                        }}
                                        floatingLabelFocusStyle={{
                                            color: '#FFB74D'
                                        }}
                                        fullWidth={true}
                                        value={this.state.firstName}
                                        onChange={this.setFirstName}
                                        underlineShow={false}
                                        errorText={this.state.invalidFirstName.error ? this.state.invalidFirstName.msg : ""}
                                        className="signUpInput"
                                    />
                            
                                    <Divider />
                                    <TextField hintText="Last name" floatingLabelText="Last Name" 
                                        style={{
                                            width: '95%',
                                            height: 60
                                        }}
                                        floatingLabelFocusStyle={{
                                            color: '#FFB74D'
                                        }}
                                        value={this.state.lastName}
                                        onChange={this.setLastName}
                                        errorText={this.state.invalidLastName.error ? this.state.invalidLastName.msg : ""}
                                        underlineShow={false}
                                        className="signUpInput"
                                    />
                                    */}

                                    <Divider />
                                    <TextField hintText="Email address" floatingLabelText="Email" 
                                        style={{
                                            width: '95%',
                                            height: 60
                                        }}
                                        floatingLabelFocusStyle={{
                                            color: '#FFB74D'
                                        }}
                                        value={this.state.email}
                                        onChange={this.setEmail}
                                        errorText={this.state.invalidEmail.error ? this.state.invalidEmail.msg : ""}
                                        underlineShow={false}
                                        className="signUpInput"
                                    />

                                    <Divider />
                                    <TextField hintText="Password" floatingLabelText="Password" type="password" 
                                        style={{
                                            width: '95%',
                                            height: 60
                                        }}
                                        floatingLabelFocusStyle={{
                                            color: '#FFB74D'
                                        }}
                                        value={this.state.password}
                                        onChange={this.setPassword}
                                        errorText={this.state.invalidPassword.error ? this.state.invalidPassword.msg : ""}
                                        underlineShow={false}
                                        className="signUpInput"                                            
                                    />

                                    <Divider />
                                    <TextField hintText="Confirm Password" floatingLabelText="Confirm Password" type="password" 
                                        style={{
                                            width: '95%',
                                            height: 60
                                        }}
                                        floatingLabelFocusStyle={{
                                            color: '#FFB74D'
                                        }}
                                        value={this.state.confirmPassword}
                                        onChange={this.setConfirmPassword}
                                        errorText={this.state.invalidConfirmPassword.error ? this.state.invalidConfirmPassword.msg : ""}
                                        underlineShow={false}
                                        className="signUpInput"                                            
                                    />
                                    <Divider />
                                </Paper>
                                <div className="row">
                                    <div className="col-md-4">
                                        <RaisedButton type="submit" label="Sign Up" backgroundColor={ this.state.password == this.state.confirmPassword ? teal300 : grey300} labelColor="#FFF" className="signUpButton"/> <br/>
                                    </div>
                                </div>
               
                                <div className="row">
                                    <div className="col-xxs-offset-1 col-md-5 header-line">
                                        <h6>Or login with</h6>
                                    </div>

                                    <div className="col-xxs-offset-1 col-md-5 social">
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
                            </form>
                        </div>
                    </div>

                </Dialog>
                <div id="home-description-section" className="container-fluid">
                    <div className="row">
					    <div className="col-md-4" style={{backgroundColor: '#1abc9c'}}>
						    <div className="mt-20 ml-40 mb-20">
							    <h3 style={{fontWeight: 600}}>Food</h3>
							    <p style={{lineHeight: 1.8}}>Use our app to find and track all the best places to eat. Do you have dietary restrictions? Allergies? No problem we can find the right place. Want a cheat day for that key lime pie? That's ok! Our virtual assistant will tell you when it's ok!</p>
							    {/*<a href="#" className="myBtn myBtn-rounded myBtn-border">Read More</a>*/}
							    <i className="fa fa-cutlery fa-5x"></i>
						    </div>
					    </div>

					    <div className="col-md-4" style={{backgroundColor: '#34495e'}}>
						    <div className="mt-20 ml-40 mb-20">
							    <h3 style={{fontWeight: 600}}>Medicine</h3>
							    <p style={{lineHeight: 1.8}}>Certain medications can interact with the foods you eat. Keeping track of what you take can help your health provider get better insights to the whole <strong>you</strong>, so that you aren't "just another patient".</p>							    
                                <i className="fa fa-heartbeat fa-5x"></i>
						    </div>
					    </div>

					    <div className="col-md-4" style={{backgroundColor: '#e74c3c'}}>
						    <div className="mt-20 ml-40 mb-20">
							    <h3 style={{fontWeight: 600}}>Fitness</h3>
							    <p style={{lineHeight: 1.8}}>Our integrations with popular fitness apps will give you the analytics and tools you need to fine-tune your health engine to run at peak optimal condition.</p>
							    <i className="fa fa-line-chart fa-5x"></i>
						    </div>
					    </div>

					    <div className="clear"></div>
				    </div>
                </div>
                <div id="home-description">
                    <div className="container-fluid clearfix">
                        <div className="row">
                            <div className="offset-md-8 col-md-4">                                                                                                
                                <div className="heading-block mb-60">
                                    <h2 className="text-uppercase">Track your intakes</h2>
                                    <p className="lead">Like a high-performing engine, you need to track what you put into your body! Use our app to easily interact with, gather, and track health info.</p>
                                </div>

                                <p>Stored securely on your device, when the need arises you can easily share with health providers so they get a complete picture of your health. Or you can just share with your friends and help them improve theirs!</p>                                

                                <div className="pull-right inline">
							        {/*
                                    <a href="https://itunes.apple.com/us/app/fbClone/id1071187070?ls=1&mt=8" className="apple-store">Apple Store</a>                                    
                                    <a href="https://play.google.com/store/apps/details?id=com.fbClone.app" className="google-play">Google Play</a>
                                    */}
                                    <a href="#" className="apple-store">Apple Store</a>                                    
                                    <a href="#" className="google-play">Google Play</a>
                                </div>
                            </div>
                        </div>
                       
                    </div>
                </div>

            </div>
        )
							    }
							    }


function mapStateToProps(state, ownProps) {
    return {
							        fsLogin: state.fsLogin,
							        searchResults: state.searchResults.users
							    };
							    }

function mapDispatchToProps(dispatch) {
    return {
							        actions: bindActionCreators(accountActions, dispatch),
							        searchBarActions: bindActionCreators(searchBarActions, dispatch)
							    };
							    }

export default connect(mapStateToProps, mapDispatchToProps)(HomeSignedOut);