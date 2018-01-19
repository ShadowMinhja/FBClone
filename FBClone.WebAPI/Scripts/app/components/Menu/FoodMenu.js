import React, { PropTypes } from 'react';
import { Link, IndexLink, withRouter, browserHistory } from 'react-router'
import {connect} from 'react-redux';
import {bindActionCreators} from 'redux';
import config from '../../config'

import * as menuActions from './menuActions'
import * as _ from 'lodash'

import GridList from 'material-ui/GridList'

import styles from './styles.css'

import MenuSection from './MenuSection.js'

class FoodMenu extends React.Component {
    constructor(props, context) {
        super(props, context);
       
        this.state = {
            open: false,
            anchorEl: null,
            searchText: ""
        };

        this.handleTouchTap = this.handleTouchTap.bind(this);
     
    }
	
    componentDidMount() {        
    }

    handleTouchTap(e) {
        // This prevents ghost click.
        e.preventDefault();

    };

    render() {
        const menu = this.props.menu;
        if (menu != null && menu.menuSections !== undefined) {
            return (
                <div id="menu-container" className=" gridlist-root">
                    <GridList
                        cellHeight={180}
                        className="gridlist"
                    >
                        {menu.menuSections.map(section =>                        
                            <MenuSection key={section.id} item={section} selectMenuItem={this.props.selectMenuItem}/>
                        )}
                    </GridList>
                </div>
            );
        }
        else {
            return null;
        }
    }
}

function mapStateToProps(state, ownProps) {    
    return {
        fsLogin: state.fsLogin
    };
}

function mapDispatchToProps(dispatch) {
    return {
        actions: bindActionCreators(menuActions, dispatch)
    };
}

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(FoodMenu));