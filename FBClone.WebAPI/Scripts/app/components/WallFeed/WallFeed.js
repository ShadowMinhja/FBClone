import React, { PropTypes } from 'react'
import {connect} from 'react-redux'
import {bindActionCreators} from 'redux'
import {Link, browserHistory} from 'react-router'
import {findDOMNode} from 'react-dom'

import * as wallFeedActions from './wallFeedActions'

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

class WallFeed extends React.Component {
    constructor(props, context) {
        super(props, context);

        const apiServiceBaseUri = config.api.baseUrl;        

        this.bodyOffset = 0;
        this.state = {
            FBUtil: FBUtil,
            apiServiceBaseUri: apiServiceBaseUri
        };
        //this.renderListOrMessage = this.renderListOrMessage.bind(this);
        this.refreshWallFeed = this.refreshWallFeed.bind(this);
        this.handleRefresh = this.handleRefresh.bind(this);
        this.handleScroll = this.handleScroll.bind(this);
        this.handleLike = this.handleLike.bind(this);
        this.handleShare = this.handleShare.bind(this);
        this.handleFetch = this.handleFetch.bind(this);
        this.handleLoadHidden = this.handleLoadHidden.bind(this);
    }

    componentDidMount() {
        const WallFeed = this;
        if(this.props.fsLogin.isLoggedIn == true) {            
            this.props.actions.loadWallFeedBites().then(function() {
                WallFeed.refreshWallFeed();
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
        const WallFeed = this;
        var mixGrid = $(".mix-grid");
        //If status changes from logged in to not logged in, load bites
        if(this.props.fsLogin.isLoggedIn == false && nextProps.fsLogin.isLoggedIn == true) {
            this.props.actions.loadWallFeedBites();
        }        

        //If number of bites has changed
        if(nextProps.bites.length != this.props.bites.length){
            if(nextProps.bites.length > 0){
                WallFeed.refreshWallFeed();
            }
        }
    }

    componentDidUpdate(prevProps, prevState) {

    }

    refreshWallFeed() {
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
        const WallFeed = this;
        //Load Bites with Pagination to load more
        this.props.actions.loadWallFeedBites().then(function() {
            WallFeed.refreshWallFeed();
        });
    }

    handleLoadHidden() {
        //this.props.dispatch(PhotosActions.loadHidden())
    }

    render() {
        let bites = [];
        _.forEach(this.props.bites, function(obj, key) {
            var bite = { id: obj.id, object: obj };
            bites.push(bite);
        });
        return (
            <BiteList ref={c => this._pageElm = c} bites={bites.filter(p => !p.hidden)} 
                onLike={this.handleLike} onLoadHidden={this.handleLoadHidden}
                onComent={this.handleLike} onLoadHidden={this.handleLoadHidden}
                onShare={this.handleShare} onLoadHidden={this.handleLoadHidden}
                onFollow={this.handleFollow}
            />
        );
    }
}

//WallFeed.propTypes = {
//    myProp: PropTypes.string.isRequired
//};

function mapStateToProps(state, ownProps) {
    return {
        fsLogin: state.fsLogin,
        bites: state.wallFeed
    };
}

function mapDispatchToProps(dispatch) {
    return {
        actions: bindActionCreators(wallFeedActions, dispatch)
    };
}

export default connect(mapStateToProps, mapDispatchToProps)(WallFeed);