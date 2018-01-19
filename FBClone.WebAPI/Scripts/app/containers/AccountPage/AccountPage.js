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

class AccountPage extends React.Component {
    constructor(props, context) {
        super(props, context);

        this.state = {         
            
        };

    }
	
    componentWillMount() {
        this.props.actions.hideHeaderLogin(this.props.fsLogin);
    }

    componentDidUpdate(prevProps, prevState) {
     
    }

    componentWillUnmount() {
     
    }

    componentWillReceiveProps(nextProps) {
     
    }

    render () {
        return (
            <div className="container clearfix w-3xl mt-20">
                <h3><i className="fa fa-user-circle-o fa-2x fa-fw"></i>Manage Your Profile</h3>
                <p className="text-success text-lg">Use this page to Manage Your Login, Information, and Check Your fbClone Points!</p>
                
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

export default connect(mapStateToProps, mapDispatchToProps)(AccountPage);