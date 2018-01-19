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

class ForgotPassword extends React.Component {
    constructor(props, context) {
        super(props, context);

        this.state = {
            username: "",
            errors: {}
        };

        this.setUserName = this.setUserName.bind(this);
        this.forgotPassword = this.forgotPassword.bind(this);
    }
	
    componentWillMount() {
        this.props.actions.hideHeaderLogin(this.props.fsLogin);
    }

    componentWillReceiveProps(nextProps) {
        if(nextProps.fsLogin.forgotPasswordResult == true){
            App.toastMsgBox.success(
                    "A password reset link was sent to your email!",
                    "Success", {
                        timeOut: 7000,
                        extendedTimeOut: 0
                    }
                );
        }
        if(nextProps.fsLogin.forgotPasswordResult == false){
            App.toastMsgBox.error(
                    "Unable to find the user associated with this username or email.  Please try again.  Is this a social login?",
                    "Error", {
                        timeOut: 7000,
                        extendedTimeOut: 0
                    }
                );
        }
    }

    componentWillUnmount() {
        if(this.props.fsLogin.hideHeaderLogin == true){
            this.props.actions.leaveForgotPassword();
        }
    }

    setUserName(e) {
        this.setState({username: e.target.value});
    }

    forgotPassword(e) {
        e.preventDefault();
        this.props.actions.forgotPassword(this.state.username);
        this.setState({username: ""});
    }
    
    render () {
        return (
            <div className="container clearfix w-3xl mt-20">
                <h3><i className="fa fa-unlock"></i> Retrieve your Password</h3>
                <form onSubmit={this.forgotPassword} >
                    <div className="row">
                        <div className="form-group col-md-12">
                            <TextField
                                hintText="Username or Email"
                                floatingLabelText="Username or Email"
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

                        <div className="col-md-12">
                            <RaisedButton type="submit" label="Reset Password" backgroundColor={orange300} labelColor="#FFF"/>
                        </div>
                    </div>
                </form>

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

export default connect(mapStateToProps, mapDispatchToProps)(ForgotPassword);