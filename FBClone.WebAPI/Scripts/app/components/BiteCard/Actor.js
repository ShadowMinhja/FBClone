import React, { Component } from 'react'
import AvatarArea from '../AvatarArea/AvatarArea.js'
import { Link } from 'react-router'
/**
 * Actor component
 */
export default class Actor extends Component {

    /**
     * defaultProps
     * @type {{avatar: string, email: string, firstName: string, lastName: string}}
     */

    /*
    static defaultProps = {
        avatar: '',
        first_name: '',
        last_name: '',
    }
    */

    /**
     * render
     * @returns markup
     */
    render() {
        const {id, email_md5, first_name, last_name, user_name } = this.props;
        //var lastInitial = "";
        //if(last_name != null)
        //    lastInitial = last_name.charAt(0);
        return (
            <Link to={`/profile/${id}`}>
            <div className="row">
                <div className="col-md-2">
                    <AvatarArea emailHash={email_md5} height={50}/>
                </div>
                <div className="col-md-10">{user_name}</div>
            </div>
            </Link>
        )
    }
}
