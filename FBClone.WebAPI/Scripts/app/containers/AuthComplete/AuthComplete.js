import React, { PropTypes } from 'react'
import {connect} from 'react-redux'
import {browserHistory} from 'react-router'
import {bindActionCreators} from 'redux'
import * as accountActions from '../../actions/accountActions'

//This page opens as a window upon OAuth login, and handles the login flow from there.
class AuthComplete extends React.Component {
    constructor(props, context) {
        super(props, context);

    }
	
    componentWillMount() {
        var externalAuthInfo = {
            externalAccessToken: this.props.externalAccessToken, 
            loginProvider: this.props.loginProvider,
            hasLocalAccount:  this.props.hasLocalAccount,
            externalUserName: this.props.externalUserName,
            email: this.props.email,
            userId: this.props.userId
        };

        if(this.props.hasLocalAccount.toLowerCase() == "false"){
            window.opener.App.externalAuthInfo = externalAuthInfo;
            window.opener.App.browserHistory.replace('/Register');
        } else if(this.props.isConfirmed.toLowerCase() == "false") {
            window.opener.App.redirectConfirmationSent(externalAuthInfo);
        }
        else {
            window.opener.App.doExternalLogin(externalAuthInfo);
        }
        window.close();
    }

    componentDidUpdate(prevProps, prevState) {

    }

    componentWillUnmount() {

    }
  
    render () {
        return null;
    }
}

function mapStateToProps(state, ownProps) {
    var fragment = parseQueryString(ownProps.location.hash.substr(1));
    return {
        externalAccessToken:fragment.external_access_token,
        loginProvider: fragment.provider,
        hasLocalAccount: fragment.haslocalaccount,
        externalUserName: fragment.external_user_name,
        email: fragment.email,
        isConfirmed: fragment.isConfirmed,
        userId: fragment.userId
    };
}

function mapDispatchToProps(dispatch) {
    return {
        actions: bindActionCreators(accountActions, dispatch)
    };
}

function parseQueryString(queryString) {
    var data = {}, pairs, pair, separatorIndex, escapedKey, escapedValue, key, value;

    if (queryString === null) {
        return data;
    }
    pairs = queryString.split("&");
    
    for (var i = 0; i < pairs.length; i++) {
        pair = pairs[i];
        separatorIndex = pair.indexOf("=");

        if (separatorIndex === -1) {
            escapedKey = pair;
            escapedValue = null;
        } else {
            escapedKey = pair.substr(0, separatorIndex);
            escapedValue = pair.substr(separatorIndex + 1);
        }

        key = decodeURIComponent(escapedKey);
        value = decodeURIComponent(escapedValue);

        data[key] = value;
    }
    return data;
}

export default connect(mapStateToProps, mapDispatchToProps)(AuthComplete);