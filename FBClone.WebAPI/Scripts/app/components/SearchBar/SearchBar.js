import React, { PropTypes } from 'react';
import { Link, IndexLink, withRouter, browserHistory } from 'react-router'
import {connect} from 'react-redux';
import {bindActionCreators} from 'redux';
import config from '../../config'

import * as searchBarActions from './searchBarActions'
import * as _ from 'lodash'

import Popover from 'material-ui/Popover';
import Menu from 'material-ui/Menu';
import MenuItem from 'material-ui/MenuItem';
import Divider from 'material-ui/Divider';
import Subheader from 'material-ui/Subheader';

import styles from './styles.css'

class SearchBar extends React.Component {
    constructor(props, context) {
        super(props, context);
        const algoliaAppId = config.algolia.appId;
        const algoliaSearchOnlyKey = config.algolia.searchOnlyKey;
        const algoliaUserIndex = config.algolia.userIndex;
        const algoliaClient = algoliasearch(algoliaAppId, algoliaSearchOnlyKey);

        this.state = {
            open: false,
            anchorEl: null,
            searchText: "",            
            userIndex: algoliaClient.initIndex(algoliaUserIndex)
        };

        this.handleTouchTap = this.handleTouchTap.bind(this);
        this.handleRequestClose = this.handleRequestClose.bind(this);
        this.setSearchText = this.setSearchText.bind(this);
        this.popupSearchDialog = this.popupSearchDialog.bind(this);
        this.captureSearch = this.captureSearch.bind(this);	
        this.userSearchCallback = this.userSearchCallback.bind(this);
        this.selectSearchLink = this.selectSearchLink.bind(this);
    }
	
    componentDidMount() {        
    }
  
    componentWillReceiveProps (nextProps) {
        //If status changes from logged in to not logged in, load search history
        if(this.props.fsLogin.isLoggedIn == false && nextProps.fsLogin.isLoggedIn == true) {
            this.props.actions.retrieveSearches();
        }        
    }

    handleTouchTap(e) {
        // This prevents ghost click.
        e.preventDefault();
        this.setState({
            anchorEl: e.currentTarget
        });
        if(this.props.searchResults.algolia.length > 0 || this.props.searchResults.history.length > 0 ) { 
            this.popupSearchDialog();
        }
        document.getElementById("searchTextBox").focus();
    };

    handleRequestClose() {
        this.setState({
            open: false,
        });
    };

    popupSearchDialog() {
        this.setState({
            open: true,
        });
       
    }

    setSearchText(e) {
        this.setState({searchText: e.target.value});
        if(e.target.value.length > 0) {
            this.state.userIndex.search(
                e.target.value, {
                    hitsPerPage: 15,
                    facets: '*',
                    maxValuesPerFacet: 10
                },
                this.userSearchCallback
            );
        } else {
            this.props.actions.clearAlgolia();
            if(this.props.searchResults.history.length == 0){
                this.setState({
                    open: false,
                });
            }
        }
    }

    userSearchCallback(err, content) {
        if(err) {
            return;
        }
        
        this.props.actions.postSearchResults(content);
        if(content.hits.length > 0) {
            this.popupSearchDialog();
        }
    }

    captureSearch(e) {
        e.preventDefault();
        this.props.actions.captureSearch(this.state.searchText);
        //this.setState({searchText: ""});
    }

    selectSearchLink(e, item, index) {
        if(e !== undefined && item !== undefined && index !== undefined){
            const searchAlgolia = this.props.searchResults.algolia;
            const searchHistory = this.props.searchResults.history;
            switch(item.props.value) {
                case "Algolia":
                    var menuItem = searchAlgolia[index];
                    //Add to Search History if Unique
                    if(_.find(searchHistory, { userId: this.props.fsLogin.guid, searchText: menuItem.UserName }) == undefined){
                        this.props.actions.captureSearch(menuItem.UserName);
                    }
                    //Clear Algolia
                    this.props.actions.clearAlgolia();
                    //Route to New Page
                    browserHistory.push('/' + menuItem.UserName);
                    break
                case "History":                    
                    //Route to New Page
                    browserHistory.push('/' + item.props.primaryText);
                    break;
                default:
                    break;
            }
            //Close Search Dialog
            this.setState({
                open: false,
            });
        }
        this.setState({searchText: ""});        
    }

    renderSearchResults() {
        if(this.props.searchResults.algolia.length > 0) { 
            return this.props.searchResults.algolia.map(searchResult =>
                <MenuItem key={searchResult.Id} value="Algolia">
                    <span>
                        <h5 className="searchItem">{searchResult.UserName}</h5> - {searchResult.FirstName} {searchResult.LastName}
                    </span>
                </MenuItem>
            )
        }
        else {
            return null;
        }
    }
    
    renderHistoryHeader() {
        if(this.props.searchResults.history.length > 0){
            return(
                <div>
                    <Divider inset={true}/>
                    <Subheader>Recent Searches</Subheader>
                </div>
            );
        }
        else {
            return null;
        }
    }

    renderSearchHistoryResults() {
        if(this.props.searchResults.history.length > 0) { 
            return this.props.searchResults.history.map(searchHistory =>
                <MenuItem key={searchHistory.id} primaryText={searchHistory.searchText} value="History"/>
            )
        }
        else {
            return null;
        }
    }

    render () {
        return (
            <div id="search-container" className="search-box-wrapper pull-left mt-40" style={this.props.fsLogin.isLoggedIn == true ? {display:"block"} : {display:"none"}}>
                <div className="container">
                    <i className="fa fa-search"></i>
                    <div className="search-box">
                        <form onSubmit={this.captureSearch} className="form-inline float-lg-left search-form" role="search" >
                            <input id="searchTextBox" type="text" name="s" value={this.state.searchText} placeholder="Search ..." className="search-field" 
                                onChange={this.setSearchText} 
                                onTouchTap={this.handleTouchTap}
                            />
                            <Popover className='search-results'
                                open={this.state.open}
                                anchorEl={this.state.anchorEl}
                                anchorOrigin={{horizontal: 'left', vertical: 'bottom'}}
                                targetOrigin={{horizontal: 'left', vertical: 'top'}}
                                onRequestClose={this.handleRequestClose}
                                style={{width:375}}
                            >
                                <Menu disableAutoFocus={true}
                                    onItemTouchTap={this.selectSearchLink}
                                >
                                    {this.renderSearchResults()}              
                                    {this.renderHistoryHeader()}
                                    {this.renderSearchHistoryResults()}                                    
                                </Menu>
                            </Popover>
                            <input type="submit" value="Search" className="search-submit"/>
                        </form>
                        Powered by&nbsp;
                        <div className="algoliaLogo"></div>
                    </div>        
                </div>
            </div>
        )
    }
}

function mapStateToProps(state, ownProps) {
    return {
        fsLogin: state.fsLogin,
        searchResults: state.searchResults
    };
}

function mapDispatchToProps(dispatch) {
    return {
        actions: bindActionCreators(searchBarActions, dispatch)
    };
}

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(SearchBar));