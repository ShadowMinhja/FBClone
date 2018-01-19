import React, { PropTypes } from 'react'
import * as _ from 'lodash'

import styles from './styles.css'
import IconButton from 'material-ui/IconButton'

class TagIconButton extends React.Component {
//const TagIconButton = ({item, onLike, onComment, onShare}) => {   
    constructor(props, context) {
        super(props, context);
        this.removeItem = this.removeItem.bind(this);
    };

    removeItem() {
        this.props.onRemoveItem(this.props.item);
    }

    render() {
        const {item, type} = this.props;
        return (
            <div className="tag-icon-button">
                <IconButton iconClassName="fa fa-times" tooltip="Remove Tag" tooltipPosition="top-right" onClick={this.removeItem}/>
                <img src={`/assets/images/food_icons/svg/${type == 'allergen' ? 'Allergens/': ''}${item}.svg`}  type="image/svg+xml"></img>
            </div>
        );
    }
}

export default TagIconButton;