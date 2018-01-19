 import React, { PropTypes } from 'react'
import { Route } from 'react-router';
import ReactDOM from 'react-dom'

import {connect} from 'react-redux'
import {bindActionCreators} from 'redux'
import RcUpload from 'rc-upload'
import Dropzone from 'react-dropzone'
import ReactTooltip  from 'react-tooltip'

import CreateGuidHelper from '../../utils/CreateGuidHelper'
import * as feedFormActions from './feedFormActions'
import * as menuActions from '../Menu/menuActions'
import * as venuePickerActions from '../VenuePicker/venuePickerActions'
//import * as stateProvinceActions from '../../actions/stateProvincesActions'
import * as _ from 'lodash'
//import * as ReactToastr from 'react-toastr'
//import {ToastContainer} from 'react-toastr'

import {Card, CardActions, CardHeader, CardMedia, CardTitle, CardText} from 'material-ui/Card'
import FontIcon from 'material-ui/FontIcon'
import FlatButton from 'material-ui/FlatButton'
import RaisedButton from 'material-ui/RaisedButton'
import Divider from 'material-ui/Divider'
import Paper from 'material-ui/Paper'
import Dialog from 'material-ui/Dialog'
import Toggle from 'material-ui/Toggle'
import TextField from 'material-ui/TextField'
import Chip from 'material-ui/Chip'
import Popover from 'material-ui/Popover'
import Menu from 'material-ui/Menu'
import MenuItem from 'material-ui/MenuItem'
import IconMenu from 'material-ui/IconMenu'
import IconButton from 'material-ui/IconButton'
import SvgIcon from 'material-ui/SvgIcon'

import config from '../../config'

import ImgPreview from '../ImgPreview/ImgPreview.js'
//import StateProvincePicker from '../StateProvincePicker/StateProvincePicker.js'
import VenuePicker from '../VenuePicker/VenuePicker.js'
import FoodMenu from '../Menu/FoodMenu.js'
import Survey from '../Survey/Survey.js'

import TagIconButton from '../TagIconButton/TagIconButton.js'
import { orange300 } from 'material-ui/styles/colors'
import styles from './styles.css'

class FeedForm extends React.Component {
    constructor(props, context) {
        super(props, context);

        const FeedForm = this;
        const apiServiceBaseUri = config.api.baseUrl;

        this.state = {
            fsLogin: Object.assign({}, props.fsLogin),
            //stateProvinces: props.stateProvinces,
            apiServiceBaseUri: apiServiceBaseUri,
            foodType: 'Dine-In',
            comment: '',
            errors: {},
            photos: [],
            openVenuePicker: false,
            openMenu: false,
            openMenuLastClick: null,
            menuAnchorEl: null,
            foodmenus: null,
            menuPickerOpen: false,
            selectedfoodmenu: null,
            foodTags: [],
            allergenTags: [],
            openFoodTags: false,
            openAllergens: false,
            venue: null,
            menuItem: null,
            survey: null,
            openSurvey: false,
            surveyQuestionResponseSet: { questionResponses: []}
        };
        
        //PROPS
        //this.ToastMessageFactory = React.createFactory(ReactToastr.ToastMessage.animation);

        //METHODS
        this.setComment = this.setComment.bind(this);
        this.postComment = this.postComment.bind(this);	
        this.toggleFoodType = this.toggleFoodType.bind(this);

        //Imagery
        this.pushImagePreview = this.pushImagePreview.bind(this);
        this.beforeUpload = this.beforeUpload.bind(this);

        //Dropzone
        this.onImageDrop = this.onImageDrop.bind(this);
        this.removeImagePreview = this.removeImagePreview.bind(this);

        //Venue Searcher
        this.handleTouchVenueButton = this.handleTouchVenueButton.bind(this);
        this.setFormVenue = this.setFormVenue.bind(this);
        
        //Menu
        this.handleTouchMenuButton = this.handleTouchMenuButton.bind(this);
        this.handleMenuPickerRequestClose = this.handleMenuPickerRequestClose.bind(this);
        this.handleCloseFoodMenu = this.handleCloseFoodMenu.bind(this);
        this.selectMenu = this.selectMenu.bind(this);
        this.selectMenuItem = this.selectMenuItem.bind(this);

        //Survey
        this.handleTouchSurveyButton = this.handleTouchSurveyButton.bind(this);
        this.handleCloseSurvey = this.handleCloseSurvey.bind(this);
        this.handleCompleteSurvey = this.handleCompleteSurvey.bind(this);
        this.getProgressBarColor = this.getProgressBarColor.bind(this);

        //Tagging
        this.handleHoverLeave = this.handleHoverLeave.bind(this);

        this.handleHoverFoodTags = this.handleHoverFoodTags.bind(this);
        this.handleRequestFoodTagsClose = this.handleRequestFoodTagsClose.bind(this);
        this.handleFoodTagChangeMultiple = this.handleFoodTagChangeMultiple.bind(this);
        this.removeFoodTag = this.removeFoodTag.bind(this);

        this.handleHoverAllergens = this.handleHoverAllergens.bind(this);
        this.handleRequestAllergensClose = this.handleRequestAllergensClose.bind(this);
        this.handleAllergenChangeMultiple = this.handleAllergenChangeMultiple.bind(this);
        this.removeAllergenTag = this.removeAllergenTag.bind(this);
    }
	
    componentDidMount() {
        //this.props.stateProvinceActions.loadStateProvinces();
        //const FeedForm = this;
        //this.props.venuePickerActions.queryVenue('ChIJw_FD3WG_QIYRPAVCx4KWiws').then(function(result) {
        //    if(result.location != null) {
        //        FeedForm.setFormVenue(result);
        //    }   
        //});
    }

    componentWillReceiveProps (nextProps) {
        //if(nextProps.foodmenus!= null && this.props.foodmenus != nextProps.foodmenus && nextProps.foodmenus.error == undefined) {
        //    this.setState({
        //        foodmenus: nextProps.foodmenus.menus,
        //        selectedfoodmenu: nextProps.foodmenus.menus.length == 1 ? nextProps.foodmenus.menus[0] : null
        //    });
        //}        
    }

    setComment(e) {
        if(e.target.value.length < 255){
            this.setState({comment: e.target.value, openFoodTags: false, openAllergens: false});
        }
    }

    postComment(e) {
        e.preventDefault();
        const FeedForm = this;
        var bite = {
            foodType: this.state.foodType,
            comment: this.state.comment,
            images: _.map(this.state.photos, function(obj) {
                return obj.fileData
            }),
            venue: this.state.venue,
            menuItem: this.state.menuItem,
            foodTags: this.state.foodTags,
            allergenTags: this.state.allergenTags,
            surveyQuestionResponseSet: this.state.surveyQuestionResponseSet
        }
        this.props.actions.postBite(bite).then(function(result) {  
            if(result == "SUCCESS") {
                FeedForm.props.actions.clearFeedForm();
                App.toastMsgBox.info(
                    "Posted successfully!",
                    "Success", {
                        timeOut: 7000,
                        extendedTimeOut: 0
                    }
                );
            }
            else if(result == "BAD_CREDENTIAL"){
                App.toastMsgBox.error(
                    "An error occurred. Please try again.",
                    "Error", {
                        timeOut: 7000,
                        extendedTimeOut: 0
                    }
                );
            }
        });
        this.setState({foodType: "Dine-In", comment: "", photos: [], 
            foodTags: [], allergenTags: [], 
            venue: null, 
            menuItem: null,
            survey: null, openSurvey: false, surveyQuestionResponseSet: { questionResponses: []},
            openAllergens: false,
            openFoodTags: false
        });
    }
    toggleFoodType(){
        var targetFoodType;
        switch(this.state.foodType){
            case 'Dine-In':
                targetFoodType = 'Carry-out';
                break;
            case 'Carry-out':
                targetFoodType = 'Dine-In'
                break;
        }
        this.setState({
            foodType: targetFoodType,
            openAllergens: false,
            openFoodTags: false
        });
    }
    //Imagery
    pushImagePreview(file) {
        const FeedForm = this;
        
        var reader = new FileReader();
        reader.onload = function(e) {
            const p = [...FeedForm.state.photos]
            p.push({uid: file.uid, imgData: e.target.result, fileData: file });
            FeedForm.setState({photos: p});
        }
        reader.readAsDataURL(file);
    }

    beforeUpload(file) {
        this.pushImagePreview(file);
        return false; //Stops built-in component upload
    }

    //Dropzone
    onImageDrop(files) {
        let file = files[0];
        file.uid = CreateGuidHelper.prototype.createGuid();
        this.pushImagePreview(files[0]);
    }

    removeImagePreview(imageUid) {
        const p = [...this.state.photos]
        _.remove(p, { "uid": imageUid});
        this.setState({
            photos: p, 
            openAllergens: false,
            openFoodTags: false
        });
    }

    //Venue Searcher
    handleTouchVenueButton(event) {
        this.setState({
            openVenuePicker: !this.state.openVenuePicker,
            openAllergens: false,
            openFoodTags: false
        });
    }

    setFormVenue(locationModel){
        var location = locationModel.location;
        var foodmenus = locationModel.menus;
        var selectedFoodMenu = foodmenus != undefined && foodmenus.length == 1 ? foodmenus[0] : null;
        var survey = locationModel.survey;
        this.setState({
            venue: location,
            openVenuePicker: false,
            foodmenus: foodmenus,
            selectedfoodmenu: selectedFoodMenu,
            survey: survey
        });
        ////Retrieve menu state
        //if(venue.id != undefined) {
        //    this.props.menuActions.getMenuForVenue(venue.id);
        //}
    }

    //Menu 
    handleTouchMenuButton(event) {        
        if(this.state.foodmenus != null && this.state.foodmenus.length > 0) {
            if(this.state.foodmenus.length > 1) { //If multiple food menus
                this.setState({
                    menuPickerOpen: true,
                    menuAnchorEl: event.currentTarget,
                    openMenuLastClick: moment().format('YYYY-MM-DD HH:mm:ss.SSS')
                });
            }
            if(this.state.selectedfoodmenu != null && this.state.selectedfoodmenu.menuSections !== undefined) {
                this.setState({
                    openMenu: !this.state.openMenu,
                    openMenuLastClick: moment().format('YYYY-MM-DD HH:mm:ss.SSS')
                });
            }
        }
    }
    handleMenuPickerRequestClose(event) {
        var currentDateTime = moment().format('YYYY-MM-DD HH:mm:ss.SSS');
        var menuPickerClickDiff = moment.duration(moment(currentDateTime).diff(moment(this.state.openMenuLastClick))) / 1000; //In Seconds
        if(menuPickerClickDiff < 750) {
            return;
        }
        this.setState({
            menuPickerOpen: false,
            selectedfoodmenu: null
        });
    }
    handleCloseFoodMenu() {
        this.setState({
            openMenu: false,
            selectedfoodmenu: null
        });
    }
    selectMenu(item){
        this.setState({
            menuPickerOpen: false,
            selectedfoodmenu: item,
            openMenu: true
        });
    }
    selectMenuItem(item){
        this.setState({
            openMenu: false,
            menuItem: item,
            selectedfoodmenu: null
        });
    }

    //Survey 
    handleTouchSurveyButton(event) {
        if(this.state.survey != null) {
            this.setState({
                openSurvey: !this.state.openSurvey,
                openAllergens: false,
                openFoodTags: false
            });
        }
    }
    handleCloseSurvey() {
        this.setState({
            openSurvey: false,
        });
    }
    handleCompleteSurvey(surveyQuestionResponseSet) {
        surveyQuestionResponseSet.locationId = this.state.venue.id || null;
        this.setState({
            surveyQuestionResponseSet: surveyQuestionResponseSet,
            openSurvey: false,
        });
    }
    getProgressBarColor(positivity) {
        switch(positivity){
            case "Average":
                return "progress progress-warning"
            case "Bad": 
                return "progress progress-danger"
            case "Good":
            default:
                return "progress progress-info";
        }
    }

    //Tagging
    handleHoverLeave(event) {
        this.setState({
            openAllergens: false,
            openFoodTags: false
        });
    };

    handleHoverFoodTags(event) {
        event.preventDefault();
        this.setState({
            openVenuePicker: false,
            openFoodTags: true,
            openAllergens: false
        });
        //TODO: Fix workaround to style popover menu container
        setTimeout(function() {
            var $foodTagPopup = $("#foodTagPopup");
            $foodTagPopup.parents().eq(4).children().addClass("iconMenu");
            $foodTagPopup.parent().css("width", "auto");
            $foodTagPopup.addClass("row");
        }, 20);
    }
    handleRequestFoodTagsClose(event) {
        this.setState({
            openFoodTags: false,
        });
    };
    handleFoodTagChangeMultiple(event, value) {
        this.setState({
            foodTags: value,
        });
    };
    removeFoodTag(tag) {
        if(_.indexOf(this.state.foodTags, tag) >= 0){
            this.setState({
                foodTags: _.remove(this.state.foodTags, function(t) { return t != tag}),
                openFoodTags: false
            });
        }
    };

    handleHoverAllergens(event) {
        event.preventDefault();
        this.setState({
            openVenuePicker: false,
            openFoodTags: false,
            openAllergens: true
        });
        //TODO: Fix workaround to style popover menu container
        setTimeout(function() {
            var $allergenPopup = $("#allergenPopup");
            $allergenPopup.parents().eq(4).children().addClass("iconMenu");
            $allergenPopup.parent().css("width", "auto");
            $allergenPopup.addClass("row");
        }, 20);
    }
    handleRequestAllergensClose(event) {
        this.setState({
            openAllergens: false,
        });
    };
    handleAllergenChangeMultiple(event, value) {
        this.setState({
            allergenTags: value,
        });
    };
    removeAllergenTag(tag) {
        if(_.indexOf(this.state.allergenTags, tag) >= 0){
            this.setState({
                allergenTags: _.remove(this.state.allergenTags, function(t) { return t != tag}),
                openAllergens: false
            });
        }
    };

    renderVenueInfo(){
        var venue = this.state.venue;
        if(venue  != null){
            return (
                <div className="col-md">
                    <div className="selected-venue row">
                        <div className="col-md-9">
                            <span className="selected-venue-address-block">
                                <h6>{venue.name}</h6>
                                <span>
                                    <p>{venue.address1}</p>
                                    <p>{venue.locality}, {venue.region} {venue.postalCode}</p>
                                </span>
                            </span>
                        </div>
                        <div className="col-md-3">
                            <span className="venue-comment">
                                <i className="fa fa-hand-o-left fa-2x"></i> 
                                <p>Ate here</p>
                            </span>
                        </div>
                    </div>
                </div>
            );
        }
        else {
            return null;
        }
    };
    
    renderMenuItemInfo(){
        var menuItem = this.state.menuItem;
        if(menuItem  != null){
            return (
                <div className="col-md menu-item-display">
                    <div className="selected-menuItem row">
                        <div className="col-md-9">
                            <span>
                                <h6 dangerouslySetInnerHTML={{__html: menuItem.itemText + ' <p>$' + menuItem.price + '</p>'}} />
                            </span>
                        </div>
                        <div className="col-md-3">
                            <span className="menuItem-comment">
                                <i className="fa fa-hand-o-left fa-2x"></i>
                                <br/>
                                <span>Chowed on </span>
                            </span>
                        </div>
                    </div>
                </div>
            );
        }
        else {
            return null;
        }
    };

    renderSurveyResults() {
        if(this.state.surveyQuestionResponseSet.totalScore != null) {
            return (
                <div className="col-md-6">
                    {
                        this.state.surveyQuestionResponseSet.categoryScores.map(catScore => 
                            <div className="row survey-result" key={catScore.category}>
                                <span className="col-md-3">{catScore.category}</span>
                                <div className="col-md">
                                    <progress className={ this.getProgressBarColor(catScore.positivity)} value={catScore.totalScore * 100} max="100"></progress>
                                </div>
                            </div>       
                        )
                    }
                </div>
            );
        } else {
            return null;
        }
    }

    render() {
        const menuDialogActions = [
          <FlatButton
            label="Close"
            labelPosition="after"
            primary={true}
            icon={<span className='fa fa-times'></span>}
            onTouchTap={this.handleCloseFoodMenu}
          />
        ];

        const surveyDialogActions = [
          <FlatButton
            label="Cancel"
            labelPosition="after"
            primary={true}
            icon={<span className='fa fa-times'></span>}
            onTouchTap={this.handleCloseSurvey}
          />
        ];

        const dialogCustomContentStyle = {
            width: '80%',
            maxWidth: 'none',
        };
        return (
            <Paper zDepth={2}>
            	<Card className="portfolio-item">
					<CardHeader className="feedForm-header">
						<div className="row">
							<div className="col-md-6 feedForm-toolbar">       
								<div id="photoButton" className={ this.state.openVenuePicker == false ? "toolbar-button" : "hidden"}>
									<RcUpload {...this.state.uploaderProps} component="div" accept="image/*"
										beforeUpload={this.beforeUpload}
									>
										<IconButton tooltip="Photo" tooltipPosition="top-right" className="pull-left" label="Photo" iconClassName="fa fa-camera" />
									</RcUpload>
								</div>
								<div id="venueButton" className="toolbar-button">
									<IconButton tooltip="Venue" tooltipPosition="top-right" iconClassName="fa fa-map-marker fa-2x" onTouchTap={this.handleTouchVenueButton}/>
								</div>
								<VenuePicker isOpen={this.state.openVenuePicker} setFormVenue={this.setFormVenue}/>
								<div id="menuButton" className={ this.state.openVenuePicker == false ? "toolbar-button" : "hidden"}>
									<IconButton tooltip={this.state.venue == null ? "Select venue first" : this.state.foodmenus == undefined || (this.state.foodmenus != undefined && this.state.foodmenus.length == 0) ? "No Menu" : "What'd you eat?"} 
										tooltipPosition="top-right" 
										iconClassName={this.state.foodmenus == undefined || (this.state.foodmenus != undefined && this.state.foodmenus.length == 0) ? "fa fa-cutlery fa-2x menu-disabled" : "fa fa-cutlery fa-2x"} 
										onTouchTap={this.handleTouchMenuButton}
										disabled={this.state.venue != null ? false : true}
									/>
								</div>
								<Popover
									open={this.state.menuPickerOpen}
									anchorEl={this.state.menuAnchorEl}
									anchorOrigin={{horizontal: 'left', vertical: 'bottom'}}
									targetOrigin={{horizontal: 'left', vertical: 'top'}}
									onRequestClose={this.handleMenuPickerRequestClose}
								>
                                    <Menu>
                                        {
                                            this.state.foodmenus != null ? this.state.foodmenus.map(foodmenu => 
                                                <MenuItem key={foodmenu.id} primaryText={foodmenu.description} onTouchTap={() => this.selectMenu(foodmenu)}/>
                                            ) : null
                                        }
                                    </Menu>
                                </Popover>

                                <Dialog
                                    title={this.state.selectedfoodmenu == null ? '' : this.state.selectedfoodmenu.description}
                                    actions={menuDialogActions}
                                    modal={false}
                                    open={this.state.openMenu}
                                    contentStyle={dialogCustomContentStyle}
                                    onRequestClose={this.handleCloseFoodMenu}
                                    autoScrollBodyContent={true}
                                    contentClassName="foodmenu-dialog"
                                >
                                    <FoodMenu menu={this.state.selectedfoodmenu} selectMenuItem={this.selectMenuItem}/>
                                </Dialog>
                                <div id="surveyButton" className={ this.state.openVenuePicker == false ? "toolbar-button" : "hidden"}>
                                    <IconButton tooltip={this.state.venue == null ? "Select venue first" : this.state.survey != null ? "Leave your feedback!" : "No survey"} 
                                        tooltipPosition="top-right" 
                                        iconClassName={this.state.survey != null ? "fa fa-comments fa-2x" : "fa fa-comments fa-2x survey-disabled"} 
                                        onTouchTap={this.handleTouchSurveyButton}
                                        disabled={this.state.venue != null ? false : true}
                                    />
                                </div>
                                <Dialog
                                    title={this.state.survey == null ? "Your FeedBack" : this.state.survey.description}
                                    actions={surveyDialogActions}
                                    modal={false}
                                    open={this.state.openSurvey}
                                    contentStyle={dialogCustomContentStyle}
                                    onRequestClose={this.handleCloseSurvey}
                                    autoScrollBodyContent={true}
                                    contentClassName="survey-dialog"
                                    >
                                    <Survey 
                                        survey={this.state.survey} 
                                        surveyQuestionResponseSet={this.state.surveyQuestionResponseSet} 
                                        completeSurvey={this.handleCompleteSurvey}
                                    />
                                    </Dialog>
                                {/*<StateProvincePicker stateProvinces={this.props.stateProvinces} style={{ display: "none" }}/>*/}
                            </div>                                
                            {this.renderVenueInfo()}
                                
                            {this.renderMenuItemInfo()}                                
                        </div>
                    </CardHeader>
                    <CardTitle title="" style={{display:"none"}}/>
                    <CardText>
						<form onSubmit={this.postComment}>
                            <div className="row" style={{paddingTop: 12}}>
                                <div className="col-md">
                                    <Toggle
                                        label={this.state.foodType}
                                        labelPosition="right"
                                        style={{marginBottom: 16, display: 'block'}}
                                        thumbStyle={{backgroundColor: '#f5b11a'}}
                                        trackStyle={{backgroundColor: '#fbdd9d'}}
                                        thumbSwitchedStyle={{backgroundColor: '#41bc9e'}}
                                        trackSwitchedStyle={{backgroundColor: '#b3e5d8'}}
                                        onTouchTap={this.toggleFoodType}
                                        />
                                    <TextField
                                        value={this.state.comment}
                                        multiLine={false}           
                                        fullWidth={true}                                 
                                        onChange={this.setComment}
                                        hintText="Whatcha you been eating?"
                                        className="comment-form-style"                                        
                                    />
                                    <button className="default-post-button" type="submit" value="default action"/>
                                </div>
                                <div className="col-md">
                                    <div className="row">
                                        <div className="col-md food-tag">
                                            <TextField name="foodTags"
                                                value={this.state.foodTags}
                                                style={{width: 400, display: 'none'}}
                                            />
                                            <div className="tag-display-area">
                                                <div>
                                                    <span className="tag-display-area-title">Tastes</span>
                                                    <IconMenu id="foodTagPopup"
                                                        iconButtonElement={<IconButton iconClassName="fa fa-tags fa-2x tag-trigger" onTouchTap={this.handleHoverFoodTags}></IconButton>}
                                                        anchorOrigin={{horizontal: 'middle', vertical: 'top'}}
                                                        targetOrigin={{horizontal: 'middle', vertical: 'top'}}
                                                        open={this.state.openFoodTags}
                                                        multiple={true}
                                                        value={this.state.foodTags}                                        
                                                        menuStyle={{marginLeft: 20, overflow: 'hidden'}}
                                                        style={{marginTop: -12}}
                                                        onChange={this.handleFoodTagChangeMultiple}
                                                        touchTapCloseDelay={0}
                                                    >
                                                        <MenuItem value="Crunchy" leftIcon={ 
                                                            <IconButton tooltip="Crunchy!" tooltipPosition="top-right"
                                                                iconStyle={{position: 'absolute', top: 0, height: 35, width: 35}} style={{width:-1}}
                                                            >
                                                                <img src="/assets/images/food_icons/svg/crunchy.svg" type="image/svg+xml"></img>
                                                            </IconButton>
                                                        } />
                                                        <MenuItem value="Fresh" leftIcon={ 
                                                            <IconButton tooltip="Fresh!" tooltipPosition="top-right"
                                                                iconStyle={{position: 'absolute', top: 0, height: 35, width: 35}} style={{width:-1}}
                                                            >
                                                                <img src="/assets/images/food_icons/svg/fresh.svg" type="image/svg+xml"></img>
                                                            </IconButton>
                                                        }/>
                                                        <MenuItem value="Hearty" leftIcon={ 
                                                            <IconButton tooltip="Hearty!" tooltipPosition="top-right"
                                                                iconStyle={{position: 'absolute', top: 0, height: 35, width: 35}} style={{width:-1}}
                                                            >
                                                                <img src="/assets/images/food_icons/svg/hearty.svg" type="image/svg+xml"></img>
                                                            </IconButton>
                                                        }/>
                                                        <MenuItem value="Meaty" leftIcon={ 
                                                            <IconButton tooltip="Meaty!" tooltipPosition="top-right"
                                                                iconStyle={{position: 'absolute', top: 0, height: 35, width: 35}} style={{width:-1}}
                                                            >
                                                                <img src="/assets/images/food_icons/svg/meaty.svg" type="image/svg+xml"></img>
                                                            </IconButton>
                                                        }/>
                                                        <MenuItem value="Soupy" leftIcon={ 
                                                            <IconButton tooltip="Soupy!" tooltipPosition="top-right"
                                                                iconStyle={{position: 'absolute', top: 0, height: 35, width: 35}} style={{width:-1}}
                                                            >
                                                                <img src="/assets/images/food_icons/svg/soupy.svg" type="image/svg+xml"></img>
                                                            </IconButton>
                                                        }/>
                                                        <MenuItem value="Spicy" leftIcon={ 
                                                            <IconButton tooltip="Spicy!" tooltipPosition="top-left"
                                                                iconStyle={{position: 'absolute', top: 0, height: 35, width: 35}} style={{width:-1}}
                                                            >
                                                                <img src="/assets/images/food_icons/svg/spicy.svg" type="image/svg+xml"></img>
                                                            </IconButton>
                                                        }/>
                                                        <MenuItem value="Sweet" leftIcon={ 
                                                            <IconButton tooltip="Sweet!" tooltipPosition="top-left"
                                                                iconStyle={{position: 'absolute', top: 0, height: 35, width: 35}} style={{width:-1}}
                                                            >
                                                                <img src="/assets/images/food_icons/svg/sweet.svg" type="image/svg+xml"></img>
                                                            </IconButton>
                                                        }/>
                                                        <MenuItem value="Veggie" leftIcon={ 
                                                            <IconButton tooltip="Veggie!" tooltipPosition="top-left"
                                                                iconStyle={{position: 'absolute', top: 0, height: 35, width: 35}} style={{width:-1}}
                                                            >
                                                                <img src="/assets/images/food_icons/svg/veggie.svg" type="image/svg+xml"></img>
                                                            </IconButton>
                                                        }/>
                                                    </IconMenu>
                                                        
                                                    <div className="tag-display-area-content">
                                                        {this.state.foodTags.map(tag =>
                                                            <TagIconButton key={tag} item={tag} type='foodTag' onRemoveItem={this.removeFoodTag}/>
                                                        )}
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div className="row">
                                        <div className="col-md allergen-tag">
                                            <TextField name="allergenTags"
                                                value={this.state.allergenTags}
                                                style={{width: 400, display: 'none'}}
                                            />
                                            <div className="tag-display-area">
                                                <div>
                                                    <span className="tag-display-area-title">Allergens</span>
                                                        <IconMenu id="allergenPopup"
                                                            iconButtonElement={<IconButton iconClassName="fa fa-flag fa-2x tag-trigger" onTouchTap={this.handleHoverAllergens} ></IconButton>}
                                                            anchorOrigin={{horizontal: 'middle', vertical: 'top'}}
                                                            targetOrigin={{horizontal: 'middle', vertical: 'top'}}
                                                            open={this.state.openAllergens}
                                                            multiple={true}
                                                            value={this.state.allergenTags}                                        
                                                            menuStyle={{marginLeft: 20, overflow: 'hidden'}}
                                                            style={{marginTop: -12}}
                                                            onChange={this.handleAllergenChangeMultiple}
                                                            touchTapCloseDelay={0}
                                                        >
                                                        <MenuItem value="Dairy" leftIcon={ 
                                                            <IconButton tooltip="Has Dairy" tooltipPosition="top-right"
                                                                iconStyle={{position: 'absolute', top: 0, height: 35, width: 35}} style={{width:-1}}
                                                            >
                                                                <img src="/assets/images/food_icons/svg/Allergens/dairy.svg" type="image/svg+xml"></img>
                                                            </IconButton>
                                                        } />
                                                        <MenuItem value="Eggs" leftIcon={ 
                                                            <IconButton tooltip="Contains Egg Products" tooltipPosition="top-right"
                                                                iconStyle={{position: 'absolute', top: 0, height: 35, width: 35}} style={{width:-1}}
                                                            >
                                                                <img src="/assets/images/food_icons/svg/Allergens/eggs.svg" type="image/svg+xml"></img>
                                                            </IconButton>
                                                        }/>
                                                        <MenuItem value="Fish" leftIcon={ 
                                                            <IconButton tooltip="Contains FIsh Products" tooltipPosition="top-right"
                                                                iconStyle={{position: 'absolute', top: 0, height: 35, width: 35}} style={{width:-1}}
                                                            >
                                                                <img src="/assets/images/food_icons/svg/Allergens/fish.svg" type="image/svg+xml"></img>
                                                            </IconButton>
                                                        }/>
                                                        <MenuItem value="Gluten" leftIcon={ 
                                                            <IconButton tooltip="Has Gluten" tooltipPosition="top-right"
                                                                iconStyle={{position: 'absolute', top: 0, height: 35, width: 35}} style={{width:-1}}
                                                            >
                                                                <img src="/assets/images/food_icons/svg/Allergens/gluten.svg" type="image/svg+xml"></img>
                                                            </IconButton>
                                                        }/>
                                                        <MenuItem value="Onion" leftIcon={ 
                                                            <IconButton tooltip="Has Onion" tooltipPosition="top-right"
                                                                iconStyle={{position: 'absolute', top: 0, height: 35, width: 35}} style={{width:-1}}
                                                            >
                                                                <img src="/assets/images/food_icons/svg/Allergens/onion.svg" type="image/svg+xml"></img>
                                                            </IconButton>
                                                        }/>
                                                        <MenuItem value="Shellfish" leftIcon={ 
                                                            <IconButton tooltip="Has Shellfish" tooltipPosition="top-left"
                                                                iconStyle={{position: 'absolute', top: 0, height: 35, width: 35}} style={{width:-1}}
                                                            >
                                                                <img src="/assets/images/food_icons/svg/Allergens/shellfish.svg" type="image/svg+xml"></img>
                                                            </IconButton>
                                                        }/>
                                                        </IconMenu>
                                                    <div className="tag-display-area-content">
                                                        {this.state.allergenTags.map(tag =>
                                                            <TagIconButton key={tag} item={tag} type='allergen' onRemoveItem={this.removeAllergenTag}/>
                                                        )}
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div className="row" style={this.state.photos.length > 0 ? {display:"block"} : {display:"none"}}>
                                <div className="col-md-12">
                                    {this.state.photos.map(photo =>
                                        <div className="img-preview-container" key={photo.uid} >
                                            <ImgPreview imgData={photo.imgData}/>
                                            <i className="fa fa-times img-preview-cancel" onMouseDown={this.removeImagePreview.bind(this, photo.uid)}></i>
                                        </div>
                                    )}
                                    <Dropzone
                                        className="img-dropzone"
                                        multiple={false}
                                        accept="image/*"
                                        onDrop={this.onImageDrop}>
                                        <span><i className="fa fa-plus fa-2x"></i></span>
                                    </Dropzone>
                                </div>
                            </div>
                            <div className="row" style={this.state.surveyQuestionResponseSet.totalScore != null ? {display:"block", marginTop: 15} : {display:"none"}}>
                                {this.renderSurveyResults()}
                                <div className="col-md-6"></div>
                            </div>

							<RaisedButton type="submit" className="post-actions" label="Post" backgroundColor={orange300} labelColor="#FFF" />
						</form>                            
                    </CardText>
                    <CardActions>
                            
                    </CardActions>
                </Card>
           
            </Paper>                
        )
    }
}

function mapStateToProps(state, ownProps) {
    return {
        fsLogin: state.fsLogin,
        feedForm: state.feedForm,
        foodmenus: state.foodmenus,
        //stateProvinces: state.stateProvinces
    };
}

function mapDispatchToProps(dispatch) {
    return {
        actions: bindActionCreators(feedFormActions, dispatch),
        menuActions: bindActionCreators(menuActions, dispatch),
        venuePickerActions: bindActionCreators(venuePickerActions, dispatch)
        //stateProvinceActions: bindActionCreators(stateProvinceActions, dispatch)
    };
}

export default connect(mapStateToProps, mapDispatchToProps)(FeedForm);