import React, { PropTypes } from 'react'
import {connect} from 'react-redux'
import {bindActionCreators} from 'redux'
import {Link, browserHistory} from 'react-router'
import {findDOMNode} from 'react-dom'

import * as feedActions from './feedActions'

import * as _ from 'lodash'
import config from '../../config'

import RaisedButton from 'material-ui/RaisedButton'
import {Card, CardActions, CardHeader, CardMedia, CardTitle, CardText} from 'material-ui/Card'
import FlatButton from 'material-ui/FlatButton'
import Divider from 'material-ui/Divider'
import Paper from 'material-ui/Paper'
import TextField from 'material-ui/TextField'
import {teal300} from 'material-ui/styles/colors'

import BiteList from '../BiteList/BiteList.js'
import FBUtil from '../../utils/FBUtil.js'

import styles from './styles.css'

class Feed extends React.Component {
    constructor(props, context) {
        super(props, context);
        
        const apiServiceBaseUri = config.api.baseUrl;        

        this.bodyOffset = 0;
        this.state = {
            FBUtil: FBUtil,
            apiServiceBaseUri: apiServiceBaseUri
        };
        //this.renderListOrMessage = this.renderListOrMessage.bind(this);
        this.refreshFeed = this.refreshFeed.bind(this);
        this.handleRefresh = this.handleRefresh.bind(this);
        this.handleScroll = this.handleScroll.bind(this);
        this.handleLike = this.handleLike.bind(this);
        this.handleShare = this.handleShare.bind(this);
        this.handleFetch = this.handleFetch.bind(this);
        this.handleLoadHidden = this.handleLoadHidden.bind(this);
    }

    componentDidMount() {
        const Feed = this;
        if(this.props.fsLogin.isLoggedIn == true) {
            this.props.actions.loadBites(this.props.userName, true).then(function() {
                Feed.refreshFeed();
            });
        }
        this.bodyOffset = document.getElementById("header-wrap").clientHeight;

        //Add Infinite Scroll Event Handler
        document.addEventListener('scroll', this.handleScroll)

    }

    componentWillUnmount() {
        document.removeEventListener('scroll', this.handleScroll)
    }

    componentWillReceiveProps (nextProps) {
        const Feed = this;
        var mixGrid = $(".mix-grid");

        //If status changes from logged in to not logged in, load bites
        if(this.props.fsLogin.isLoggedIn == false && nextProps.fsLogin.isLoggedIn == true) {
            this.props.actions.loadBites(this.props.userName, false);
        }        

        //Retrieve Profile Info is Switching UserNames
        if(this.props.userName != nextProps.userName){
            //if(mixGrid.length > 0) {
            //    mixGrid.mixItUp('changeLayout', 'none'); 
            //}
            this.props.actions.loadBites(nextProps.userName, true);
        }

        //If number of bites has changed
        if(nextProps.bites.length != this.props.bites.length){
            if(nextProps.bites.length > 0){
                Feed.refreshFeed();
            }
        }
    }

    componentDidUpdate(prevProps, prevState) {
        //If FeedForm has new inserted post, push into bites
        if(this.props.feedForm.status == "success" && this.props.feedForm.insertedPost !== undefined && _.find(this.props.bites, { id: this.props.feedForm.insertedPost.id}) === undefined)
        {
            this.props.actions.addBite(prevProps.bites, this.props.feedForm.insertedPost);
        }
    }

    refreshFeed() {
        //TODO: There should be a better way to do this (without setTimeout)
        var mixGrid = $(".mix-grid");
        if(mixGrid.length > 0) {            
            setTimeout(function() {
                mixGrid.mixItUp('changeLayout', 'block'); 
            }, 50);
        }
    }

    handleScroll(e) {

        if (this.$scroll) clearTimeout(this.$scroll)

        this.$scroll = setTimeout(() => {

            const d = findDOMNode(this._pageElm)
            if(d === null)
                return; //Escape

            const threshold = (d.offsetHeight / 2)
            if(d.offsetHeight > 150) {
                var scrollPos = document.getElementsByTagName("body")[0].scrollTop;
            
                if ((scrollPos + this.bodyOffset) >= (d.scrollHeight - threshold)) {
                    this.handleFetch()
                }
            }

        }, 25)
    }

    handleRefresh(e) {
        this.props.onLoadHidden()
    }

    handleLike(data) {
        //this.props.dispatch(PhotosActions[data.liked ? 'like' : 'unlike'](data.id))
    }

    handleShare(bite) {
        var url = `${this.state.apiServiceBaseUri}/Grab/Bite/${bite.id}`;
        this.state.FBUtil.prototype.shareBite(url);
    }

    handleFetch() {
        const Feed = this;
        //Load Bites with Pagination to load more
        this.props.actions.loadBites(this.props.userName, false).then(function() {
            Feed.refreshFeed();
        });
    }

    handleLoadHidden() {
        //this.props.dispatch(PhotosActions.loadHidden())
    }
    renderMessage() {        
        if(this.props.bites.feedPost == "failure") {
            return(
                <h3 className="text-default">There was some error retrieving the munchies. Please come back and try again later.</h3>
            );
        }
        else if(this.props.bites.length > 0) {
            return null;
        }        
        else {
            if(this.props.fsLogin.userName == this.props.userName){
                if(this.props.bites.length == 0 && this.props.feedPagination.fetching == false){
                    return(
                        <h3 className="text-default">Don't be shy! Let us know what savory tastes you've been having.</h3>
                    );
                } else {
                    return(
                        <div className="feed-area-refresh">
                            <span className="fa fa-refresh fa-spin fa-5x loading-feed"></span>
                        </div>
                    );
                }
            }
            else {
                return(
                    <h3 className="text-default">Nothing yet...but come back later for munchies from this connoisseur.</h3>
                );
            }
        }
}

    render() {
        let bites = [];
        _.forEach(this.props.bites, function(obj, key) {
            var bite = { id: obj.id, object: obj };
            bites.push(bite);
        });     

        return (
            <div className="feed-area">
                <BiteList ref={c => this._pageElm = c} bites={bites.filter(p => !p.hidden)} 
                    onLike={this.handleLike} onLoadHidden={this.handleLoadHidden}
                    onComment={this.handleLike} onLoadHidden={this.handleLoadHidden}
                    onShare={this.handleShare} onLoadHidden={this.handleLoadHidden}
                    onFollow={this.handleFollow}
                />
                {this.renderMessage()}
            </div>
        );
    }
}

//Feed.propTypes = {
//    myProp: PropTypes.string.isRequired
//};

function mapStateToProps(state, ownProps) {
    return {
        fsLogin: state.fsLogin,
        bites: state.feed,
        feedForm: state.feedForm,
        feedPagination: state.feedPagination
    };
}

function mapDispatchToProps(dispatch) {
    return {
        actions: bindActionCreators(feedActions, dispatch)
    };
}

export default connect(mapStateToProps, mapDispatchToProps)(Feed);