import React, {Component} from 'react'
import BiteCard from '../BiteCard/BiteCard.js'

class BiteList extends Component {
    constructor(props, context) {
        super(props, context);
    }

    componentDidUpdate(prevProps, prevState) {

    }

    render() {
        return (
            <div>
                {/*<ul className="mix-filter">
                    <li data-filter="all" className="filter active">All</li>
                    <li data-filter=".category_1" className="filter">Category 1</li>
                    <li data-filter=".category_2" className="filter">Category 2</li>
                    <li data-filter=".category_3" className="filter">Category 3</li>
                </ul>*/}

                <div className="row mix-grid portfolio" data-lightbox="gallery">
                    {this.props.bites.map(bite =>
                        <BiteCard key={bite.id} item={bite.object} onLike={this.props.onLike} onComment={this.props.onComment} onShare={this.props.onShare} width='col-md-6'/>
                    )}
                </div>
            </div>
        )
    }
}

export default BiteList;           