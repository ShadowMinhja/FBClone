import React, { PropTypes } from 'react'
import {connect} from 'react-redux'
import {bindActionCreators} from 'redux'
import {browserHistory, withRouter} from 'react-router'
import * as profileActions from './profileActions'

import AvatarArea from '../../components/AvatarArea/AvatarArea.js'
import FlatButton from 'material-ui/FlatButton'
import FontIcon from 'material-ui/FontIcon'

import FeedForm from '../../components/FeedForm/FeedForm.js'
import Feed from '../../components/Feed/Feed.js'

import styles from './styles.css'

class ProfilePage extends React.Component {
    constructor(props, context) {
        super(props, context);

        this.handleFollowPerson = this.handleFollowPerson.bind(this);
    }

    componentWillMount() {
        this.props.actions.retrieveProfileInfo(this.props.params.userName);
    }

    componentWillReceiveProps (nextProps) {      
        //Retrieve Profile Info if fsLogin Object is Returned And Not Empty
        //if((this.props.fsLogin != nextProps.fsLogin) && nextProps.fsLogin.loginEmpty != true) {
        //    this.props.actions.retrieveProfileInfo(this.props.params.userName);
        //}

        //Retrieve Profile Info is Switching UserNames
        if(this.props.userName != nextProps.params.userName){
            this.props.actions.retrieveProfileInfo(nextProps.params.userName);
        }

        //Redirect to login page if still not logged in after checking profileInfo
        if(nextProps.fsLogin.loginEmpty == true || nextProps.profileInfo.errorText == "Unauthorized") { //User Not Logged In
            browserHistory.push('/login');
        }

        if(nextProps.profileInfo.errorText == "Not Found"){
            browserHistory.replace('/error');
        }

        if(this.props.profileInfo.target == null){
            //this.props.router.push('/error');
        }
    }

    renderAvatarArea() {
        if(!this.props.profileInfo.actor){
            return null;
        }
        else{
            const actor = this.props.profileInfo.actor;
            const user_name = actor.user_name;
            return (
                <div className="row">
                    <div className="col-md-2">
                        <AvatarArea emailHash={actor.email_md5} height={50}/>
                    </div>
                    <div className="col-md-10">
                        <strong className="brandSecondary">{user_name}</strong>
                    </div>
                </div>
            )
        }
    }
    
    handleFollowPerson() {
        const actor = this.props.profileInfo.actor;
        if(this.props.profileInfo.isFollowing){
            this.props.actions.unfollowProfile(actor);
        } else {
            this.props.actions.followProfile(actor);
        }
    }

    /*Show only if you are NOT on your own profile*/
    renderFollowButton() {
        if(this.props.fsLogin.userName != this.props.params.userName) {
            return (
                <FlatButton
                    label={(this.props.profileInfo.isFollowing ? "Unfollow " : "Follow ") + this.props.userName }
                    labelPosition="after"
                    primary={true}
                    style={styles.button}
                    onTouchTap={this.handleFollowPerson}
                    icon={<FontIcon className={this.props.profileInfo.isFollowing ? "fa fa-minus": "fa fa-plus"} />}
                  />
            )
        }   
        else {
            return null;
        }
    }

    /*Show only if you go to your own profile*/
    renderFeedForm() {
        if(this.props.fsLogin.userName == this.props.params.userName) {
            return (
                <div className="row mb-20">
                    <div className="col-md-12">
                        <FeedForm />
                    </div>
                </div>
            )
        }
        else {
            return null;
        }
    }
    
    render() {
        const { fsLogin} = this.props;
       
        return (
            <div className="container clearfix mt-20" >
                <div className="row mb-20">
                    <div className="col-md-5">
                        {this.renderAvatarArea()}
                    </div>
                    <div className="col-md-7">
                        {this.renderFollowButton()}                        
                    </div>
                </div>
                {this.renderFeedForm()}
                <div className="row">
                    <div className="col-md-12">
                        <Feed userName={this.props.params.userName}/>
                    </div>
                </div>
            </div>
        );
    }
}

function mapStateToProps(state, ownProps) {
    return {
        fsLogin: state.fsLogin,
        bites: state.feed,
        userName: ownProps.params.userName,
        profileInfo: state.profileInfo
    };
}

function mapDispatchToProps(dispatch) {
    return {
        actions: bindActionCreators(profileActions, dispatch)
    };
}

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(ProfilePage));