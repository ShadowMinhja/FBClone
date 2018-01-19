import React, { PropTypes } from 'react'
import {connect} from 'react-redux'
import {browserHistory} from 'react-router'
import {bindActionCreators} from 'redux'
import * as accountActions from '../../actions/accountActions'
import config from '../../config'

import RaisedButton from 'material-ui/RaisedButton'
import FlatButton from 'material-ui/FlatButton'
import TextField from 'material-ui/TextField'
import { orange300, teal300, grey300 } from 'material-ui/styles/colors'

import styles from './styles.css'

class ResetPassword extends React.Component {
    constructor(props, context) {
        super(props, context);

        this.state = {
            password: "",
            confirmPassword: "",
            invalidPasswordError: null,
            errors: {}
        };

        this.setPassword = this.setPassword.bind(this);
        this.setConfirmPassword = this.setConfirmPassword.bind(this);
        this.resetPassword = this.resetPassword.bind(this);
        this.forgotPasswordTap = this.forgotPasswordTap.bind(this);
    }
	
    componentWillMount() {
        this.props.actions.hideHeaderLogin(this.props.fsLogin);
    }

    componentWillReceiveProps(nextProps) {
        if(nextProps.fsLogin.resetPasswordResult == true){
            App.toastMsgBox.success(
                    "Password reset successfully!",
                    "Success", {
                        timeOut: 7000,
                        extendedTimeOut: 0
                    }
                );
            setTimeout(function() {
                browserHistory.replace('/');
            }, 7000);
        }
        if(nextProps.fsLogin.resetPasswordResult == false){
            App.toastMsgBox.error(
                    "Reset link not valid or has expired.",
                    "Error", {
                        timeOut: 7000,
                        extendedTimeOut: 0
                    }
                );            
        }
    }

    componentWillUnmount() {
        if(this.props.fsLogin.hideHeaderLogin == true){
            this.props.actions.leaveResetPassword();
        }
    }

    setPassword(e) {
        this.setState({password: e.target.value});
    }

    setConfirmPassword(e) {
        this.setState({confirmPassword: e.target.value});
        if(e.target.value != this.state.password){
            this.setState({invalidPasswordError: "Passwords must be the same"});
        } else {
            this.setState({invalidPasswordError: null});
        }
    }

    resetPassword(e) {
        e.preventDefault();
        if(this.state.password == this.state.confirmPassword) {
            var resetObject = {
                password: this.state.password, 
                userId: this.props.userId, 
                code: this.props.code
            };
            this.props.actions.resetPassword(resetObject);
            this.setState({password: "", confirmPassword: ""});
        }
    }
    
    forgotPasswordTap(e) {
        e.preventDefault();
        browserHistory.push('/Account/ForgotPassword');
    }

    render () {
        return (
            <div className="container clearfix w-3xl mt-20">
                <h3><i className="fa fa-lock"></i> Reset your Account</h3>
                <form onSubmit={this.resetPassword} >
                    <div className="row">
                        <div className="form-group col-md-12">
                            <TextField
                                type="password"
                                hintText="Enter new password"
                                floatingLabelText="Enter Password"
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

                        <div className="form-group col-md-12">
                            <TextField
                                type="password"
                                hintText="Re-enter password"
                                floatingLabelText="Password Confirmation"
                                floatingLabelFocusStyle={{
                                    color: "#FFB74D"
                                }}
                                value={this.state.confirmPassword}
                                onChange={this.setConfirmPassword}
                                underlineFocusStyle={{
                                    borderColor: "#FFB74D"
                                }}
                                errorText={this.state.invalidPasswordError }
                            />
                        </div>

                        <div className="col-md-12">
                            <RaisedButton type="submit" label="Reset Password" backgroundColor={ this.state.password == this.state.confirmPassword ? orange300 : grey300} labelColor="#FFF"/>
                        </div>
                    </div>
                </form>

                <FlatButton className="pull-right" label="Resend Password Reset" style={{color: teal300}} onTouchTap={this.forgotPasswordTap}/>
            </div>
        )
    }
}

function mapStateToProps(state, ownProps) {
    return {
        fsLogin: state.fsLogin,
        userId: ownProps.location.query.userId,
        code: ownProps.location.query.code.replace(new RegExp(' ', 'g'), '+') ,
    };
}

function mapDispatchToProps(dispatch) {
    return {
        actions: bindActionCreators(accountActions, dispatch)
    };
}

export default connect(mapStateToProps, mapDispatchToProps)(ResetPassword);