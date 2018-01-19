import React, { PropTypes } from 'react';
import { Link, IndexLink, withRouter, browserHistory } from 'react-router'
import {connect} from 'react-redux';
import {bindActionCreators} from 'redux';
import config from '../../config'

import * as venuePickerActions from './venuePickerActions'
import * as _ from 'lodash'

import Popover from 'material-ui/Popover';
import Menu from 'material-ui/Menu';
import MenuItem from 'material-ui/MenuItem';
import Divider from 'material-ui/Divider';
import TextField from 'material-ui/TextField'

import Subheader from 'material-ui/Subheader';

import styles from './styles.css'

class VenuePicker extends React.Component {
    constructor(props, context) {
        super(props, context);
        //const algoliaAppId = config.algolia.appId;
        //const algoliaSearchOnlyKey = config.algolia.searchOnlyKey;
        //const algoliaUserIndex = config.algolia.userIndex;
        //const algoliaClient = algoliasearch(algoliaAppId, algoliaSearchOnlyKey);

        this.state = {
            open: false,
            anchorEl: null,
            venueText: "",    
            errorText: "",
            showProgress: false,
            searchBox: null
        };

        this.handleRequestClose = this.handleRequestClose.bind(this);
        this.handleClearSearchBox = this. handleClearSearchBox.bind(this);
        this.setVenueText = this.setVenueText.bind(this);
        this.popupVenueDialog = this.popupVenueDialog.bind(this);
        this.venueSearchCallback = this.venueSearchCallback.bind(this);
        this.ParseGPlaceLocation = this.ParseGPlaceLocation.bind(this);
        this.selectVenue = this.selectVenue.bind(this);
    }
	
    componentDidMount() {        
        var googlePlaceSearchInput = document.getElementById('restaurantPicker');
        var searchBox = new google.maps.places.SearchBox(googlePlaceSearchInput);
        searchBox.addListener('places_changed', this.setVenueText);         
        this.setState({
            searchBox: searchBox
        });
    }
  
    componentWillReceiveProps (nextProps) {

    }

    handleRequestClose() {
        this.setState({
            open: false,
        });
    };

    handleClearSearchBox() {
        document.getElementById('restaurantPicker').value = "";
    }

    popupVenueDialog() {
        this.setState({
            open: true,
        });
       
    }

    setVenueText() {
        const VenuePicker = this;
        var gPlace = this.state.searchBox.getPlaces();
        if(gPlace != null && gPlace.length > 0){
            gPlace = gPlace[0];
        }
        var placeId = gPlace.place_id;
        this.setState({showProgress: true});
        VenuePicker.props.actions.queryVenue(placeId).then(function(result) {
            if(result.location != null) {
                VenuePicker.venueSearchCallback();
            } else { 
                VenuePicker.setState({
                    showProgress: false
                });
                var gLocation = VenuePicker.ParseGPlaceLocation(gPlace);
                var locationModel = {
                    location: gLocation,
                    survey: result.survey
                };
                VenuePicker.props.setFormVenue(locationModel);
            }
        });
        //VenuePicker.setState({venueText: e.target.value});
        //if(e.target.value.length > 2) {
        //    this.setState({
        //        showProgress: true
        //    });
        //    //VenuePicker.props.actions.queryVenue(e.target.value).then(function() {
        //    //    VenuePicker.venueSearchCallback();
        //    //});
        //} else {            
        //    VenuePicker.venueSearchCallback();
        //}
    }

    venueSearchCallback() {    
        this.setState({
            showProgress: false
        });
        if(this.props.venuePickerResults != null) {
            //this.popupVenueDialog();
            this.props.setFormVenue(this.props.venuePickerResults);
        }
    }

    ParseGPlaceLocation(googlePlaceAddress) {
        var newLocation = { id: null, name: "", address1: "", locality: "", region: "", postalCode: "", userid: this.props.fsLogin.id  };
        var addrParts = googlePlaceAddress.adr_address.split(/>,|> /);
        newLocation.name = googlePlaceAddress.name;
        _.forEach(addrParts, function (obj, key) {
            var jqObj = $(obj + ">");
            switch (jqObj.attr("class")) {
                case "street-address":
                    newLocation.address1 = jqObj.html();
                    break;
                case "locality":
                    newLocation.locality = jqObj.html();
                    break;
                case "region":
                    newLocation.region = jqObj.html();
                    break;
                case "postal-code":
                    newLocation.postalCode = jqObj.html();
                    break;
                case "country-name":
                    newLocation.country = jqObj.html();
                    break;
            }
        });
        newLocation.address = googlePlaceAddress.formatted_address;
        newLocation.placeId = googlePlaceAddress.place_id;
        newLocation.geoLat = googlePlaceAddress.geometry.location.lat();
        newLocation.geoLng = googlePlaceAddress.geometry.location.lng();
        return newLocation;
    }

    selectVenue(e, item, index) {        
        if(e !== undefined && item !== undefined && index !== undefined){
            switch(item.props.value) {
                case "Venues":                    
                    var venueItem = _.find(this.props.venuePickerResults.venues, {id: item.key});
                    this.props.setFormVenue(venueItem);
                    break;
                default:
                    break;
            }
            //Close Search Dialog
            this.setState({
                open: false,
            });
        }
        this.setState({venueText: ""});        
    }

    renderVenuePickerResults() {
        return null;
        //if(this.props.venuePickerResults.venues.length > 0) { 
        //    return this.props.venuePickerResults.venues.map(v =>
        //        <MenuItem key={v.id} value="Venues">
        //            <span>
        //                <h6 className="venueItem">{v.name}</h6>
        //                <span className="venue-address-block">
        //                    <p>{v.address1}</p>
        //                    <p>{v.locality}, {v.region} {v.postalCode}</p>
        //                </span>
        //            </span>
        //        </MenuItem>
        //    )
        //}
        //else {
        //    return null;
        //}
    }    
    
    render () {
        const isOpen = this.props.isOpen;
        return (
            <div id="venue-container" className="venue-picker-wrapper" style={isOpen == true ? {display:"block"} : {display:"none"}}>
                    <div className="venue-picker">                        
                        <TextField floatingLabelText="Find Restaurant" floatingLabelFixed={true}
                            id="restaurantPicker"
                            style={{
                                width: '95%', marginLeft: 5
                            }}
                            floatingLabelFocusStyle={{
                                color: '#FFB74D'
                            }}
                            onInput={this.handleFocused}
                            fullWidth={true}                                              
                            underlineShow={false}
                            errorText={this.state.errorText ? this.state.errorText : ""}
                        />
                        <i className="venue-search-clear fa fa-times fa-2x" onTouchTap={this.handleClearSearchBox} style={this.state.showProgress == false ? { display: 'inline-block'} : { display: 'none'}}></i>
                        <i className="venue-search-spinner fa fa-spinner fa-2x fa-spin" style={this.state.showProgress == true ? { display: 'inline-block'} : { display: 'none'}}></i>
                        <Popover className='venue-picker-results'
                            open={this.state.open}
                            anchorEl={this.state.anchorEl}
                            anchorOrigin={{horizontal: 'left', vertical: 'bottom'}}
                            targetOrigin={{horizontal: 'left', vertical: 'top'}}
                            onRequestClose={this.handleRequestClose}
                            style={{width:375}}
                        >
                            <Menu disableAutoFocus={true}
                                onItemTouchTap={this.selectVenue}
                            >
                                {this.renderVenuePickerResults()}              
                            </Menu>
                        </Popover>
                    </div>        
            </div>
        )
    }
}

function mapStateToProps(state, ownProps) {
    return {
        fsLogin: state.fsLogin,
        isOpen: ownProps.isOpen,
        venuePickerResults: state.venuePickerResults
    };
}

function mapDispatchToProps(dispatch) {
    return {
        actions: bindActionCreators(venuePickerActions, dispatch)
    };
}

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(VenuePicker));