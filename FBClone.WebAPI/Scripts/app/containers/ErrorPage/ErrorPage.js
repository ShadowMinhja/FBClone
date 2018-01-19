import React, { PropTypes } from 'react'
import {connect} from 'react-redux'
import { Link, IndexLink, withRouter } from 'react-router'
import {bindActionCreators} from 'redux'

import styles from './styles.css'

class ErrorPage extends React.Component {
    constructor(props, context) {
        super(props, context);

        this.state = {
            errors: {}
        };

    }
	
    componentWillMount() {
    }

    componentDidUpdate(prevProps, prevState) {
    }

    componentWillUnmount() {
    }

    render () {
        return (
            <div className="container clearfix w-3xl mt-20">
                <h1 className="text-mega text-default lter">Oopsie!</h1>
                <h3><i className="fa fa-exclamation-triangle"></i> Did you click on the wrong link? There's plenty of food to go around, so go back and find some more grub! </h3>
                <h4><Link to="/"><i className="fa fa-arrow-circle-left"></i> Get back to the fun</Link></h4>
            </div>
        )
    }
}

function mapStateToProps(state, ownProps) {
    return {
    };
}

function mapDispatchToProps(dispatch) {
    return {
    };
}

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(ErrorPage));