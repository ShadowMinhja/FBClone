import React, { PropTypes } from 'react'
import { Link, IndexLink, withRouter, browserHistory } from 'react-router'
import {connect} from 'react-redux'
import {bindActionCreators} from 'redux'
import * as accountActions from '../../actions/accountActions'

import SearchBar from '../../components/SearchBar/SearchBar.js'
import RaisedButton from 'material-ui/RaisedButton'
import TextField from 'material-ui/TextField'
import { teal300, orange300 } from 'material-ui/styles/colors'
import * as ReactToastr from 'react-toastr'
import {ToastContainer} from 'react-toastr'

import styles from './styles.css'

class Header extends React.Component {

    constructor(props, context) {
        super(props, context);

        this.state = {
            menuOpen: false,
            fsLogin: Object.assign({}, props.fsLogin),
            username: "",
            password: ""
        };
        this.ToastMessageFactory = React.createFactory(ReactToastr.ToastMessage.animation);
        this.setUserName = this.setUserName.bind(this);
        this.setPassword = this.setPassword.bind(this);
        this.doLogin = this.doLogin.bind(this);
        this.doLogout = this.doLogout.bind(this);
        this.registerToaster = this.registerToaster.bind(this);
        this.handleShowSignup = this.handleShowSignup.bind(this);
        //this.props.actions.retrieveLoginInfo(); <-- Moved to root module
    }

    shouldComponentUpdate(nextProps, nextState) {
        if(nextProps.fsLogin.loginFail == true){
            return false;
        }
        else
            return true;
    }

    componentDidMount() {
        this.registerToaster();
    }

    registerToaster() {
        var app = Object.assign({}, window.App, 
            {
                toastMsgBox: this.refs.toastMsgBox
            }
        );
        window.App = app;
    }

    componentWillReceiveProps(nextProps) {
        if(nextProps.location.search == "?action=LogOff&controller=Home"){ //Logout from /restauranteurs
            this.props.actions.removeLoginCache().then(function() {
                browserHistory.replace('/');
            });
        }
        else {
            if(nextProps.fsLogin.loginError != undefined){     
                if(App.toastMsgBox === undefined){
                    this.registerToaster();
                }
                App.toastMsgBox.error(
                    nextProps.fsLogin.loginError,
                    "Error", {
                        timeOut: 7000,
                        extendedTimeOut: 0
                    }
                );
                if(nextProps.fsLogin.loginError.indexOf('password is incorrect') != -1){
                    if(nextProps.router.location.pathname != "/Login") {
                        browserHistory.replace('/Login');
                    }
                }

                if(nextProps.fsLogin.loginError.indexOf('Account must be confirmed') != -1){
                    if(nextProps.router.location.pathname != "/Account/ConfirmationSent") {
                        browserHistory.replace('/Account/ConfirmationSent');
                    }
                }
            }
        }
    }

    componentDidUpdate(prevProps, prevState) {
        if(this.props.fsLogin.isLoggedIn == true){
            $('.mix-grid').mixItUp(); //<-- Initialize Mix It Up Grid
        }
    }

    handleShowSignup() {        
        this.props.actions.showSignup(true);
    }

    setUserName(e) {
        this.setState({username: e.target.value});
    }

    setPassword(e) {
        this.setState({password: e.target.value});
    }

    doLogin(e) {
        e.preventDefault();
        //let history = this.props.router;
        this.props.actions.accountLogin(this.state.username, this.state.password);
        //this.props.actions.accountLogin(this.state.username, this.state.password).then(function(result) {
        //    if(result == "LOGIN_FAIL"){
        //        history.push('/login');
        //    }
        //});
        this.setState({username: "", password: ""});
    }

    doLogout(e) {
        this.props.actions.accountLogout();
        browserHistory.replace('/');
    }

    render () {
        return (
            <header id="header" className="transparent-header dark">
                <ToastContainer ref="toastMsgBox"
                    toastMessageFactory={this.ToastMessageFactory}
                    className="toast-top-center" />
                <div id="header-wrap">
                    <nav id="main-navbar" className="navbar">
                        <div className="navbar-header">
                            <div id="branding" className="navbar-brand">
                                <IndexLink to="/" className="brand-normal" data-light-logo="/corporate/assets/images/logo.png"><img src="/corporate/assets/images/logo-dark.png" alt="fbClone"/></IndexLink>
                                <IndexLink to="/" className="brand-retina" data-light-logo="/corporate/assets/images/logo@2x.png"><img src="/corporate/assets/images/logo@2x-dark.png" alt="fbClone"/></IndexLink>
                            </div>      
                        </div>

                        <button className="navbar-toggler hidden-lg-up pull-xs-right" type="button" data-toggle="collapse" data-target="#navbarResponsive" aria-controls="navbarResponsive" aria-expanded="false" aria-label="Toggle navigation"
                            style={this.props.fsLogin.hideHeaderLogin != true ? {display:"block"} : {display:"none"}}>
                            <i className="fa fa-bars"></i>
                        </button>
                        <div className="collapse navbar-toggleable-md" id="navbarResponsive">
                            <SearchBar />

                            <ul className="nav navbar-nav menu-actions pull-right">
                                <li style={this.props.fsLogin.isLoggedIn == false && this.props.fsLogin.hideHeaderLogin != true ? {display:"block"} : {display:"none"}}><Link to="/login" className="login-button" activeClassName="active">Login</Link></li>    
                                <li  style={this.props.fsLogin.isLoggedIn == false && this.props.fsLogin.hideHeaderLogin != true ? {display:"block"} : {display:"none"}}><RaisedButton label="Sign Up" backgroundColor={ orange300 } labelColor="#FFF" onTouchTap={() => this.handleShowSignup(this.state.showSignup)}/></li>
                                <li style={this.props.fsLogin.isLoggedIn == true ? {display:"block"} : {display:"none"}}><IndexLink to={"/"}>Home</IndexLink></li>
                                <li style={this.props.fsLogin.isLoggedIn == true ? {display:"block"} : {display:"none"}}><IndexLink to={"/" + this.props.fsLogin.userName}>{this.props.fsLogin.userName}</IndexLink></li>
                                <li  className="logout-button" style={this.props.fsLogin.isLoggedIn == true ? {display:"block"} : {display:"none"}}>
                                    <RaisedButton onTouchTap={this.doLogout} label="Logout" backgroundColor={orange300} labelColor="#FFF"/>
                                </li>
                            </ul>       
                                                                  

                            {/*<div id="search-toggle" style={this.props.fsLogin.isLoggedIn == true ? {display:"block"} : {display:"none"}}><span>|</span> <a href="#"><i className="fa fa-search"></i></a></div>*/}
                            {/*
                            <form id="login-form" onSubmit={this.doLogin}  className="form-inline float-md-right" style={this.props.fsLogin.isLoggedIn == false && this.props.fsLogin.hideHeaderLogin != true ? {display:"block"} : {display:"none"}}>
                                <TextField
                                    hintText="Username"
                                    floatingLabelText="Username"
                                    floatingLabelFocusStyle={{
                                        color: "#FFB74D"
                                    }}
                                    className="form-control login-toggle-field"
                                    value={this.state.username}
                                    onChange={this.setUserName}
                                    underlineFocusStyle={{
                                        borderColor: "#FFB74D"
                                    }}
                                />
                                <TextField
                                    type="password"
                                    hintText="Password"
                                    floatingLabelText="Password"
                                    floatingLabelFocusStyle={{
                                        color: "#FFB74D"
                                    }}
                                    className="form-control login-toggle-field"
                                    value={this.state.password}
                                    onChange={this.setPassword}
                                    underlineFocusStyle={{
                                        borderColor: "#FFB74D"
                                    }}
                                />
                                <RaisedButton type="submit" label="Login" backgroundColor={orange300} labelColor="#FFF" style={{ minWidth: 80 }} />
                            </form>
                            */}
                        </div>  
                    </nav>       
                </div>
            </header>
        )
    }
}

function mapStateToProps(state, ownProps) {
    return {
        fsLogin: state.fsLogin,
        action: ownProps.params.action
    };
}

function mapDispatchToProps(dispatch) {
    return {
        actions: bindActionCreators(accountActions, dispatch)
    };
}

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(Header));