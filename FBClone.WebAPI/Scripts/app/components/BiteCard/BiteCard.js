import React, { PropTypes } from 'react'

import * as _ from 'lodash'

import RaisedButton from 'material-ui/RaisedButton'
import {Card, CardActions, CardHeader, CardMedia, CardTitle, CardText} from 'material-ui/Card'
import FlatButton from 'material-ui/FlatButton'
import Divider from 'material-ui/Divider'
import Paper from 'material-ui/Paper'
import Toggle from 'material-ui/Toggle'
import TextField from 'material-ui/TextField'
import IconButton from 'material-ui/IconButton'
import ImageGallery from 'react-image-gallery'
import {teal300} from 'material-ui/styles/colors'

import Actor from '../BiteCard/Actor.js'
import LikeButton from '../LikeButton/LikeButton.js'
import CommentButton from '../CommentButton/CommentButton.js'
import ShareButton from '../ShareButton/ShareButton.js'

import styles from './styles.css'

class BiteCard extends React.Component {
//const BiteCard = ({item, onLike, onComment, onShare}) => {   
    constructor(props, context) {
        super(props, context);
        this.handleImageLoad = this.handleImageLoad.bind(this);

        this.getTotalScore = this.getTotalScore.bind(this);
        this.getPositivity = this.getPositivity.bind(this);
        this.getCategoryScores = this.getCategoryScores.bind(this);
        this.getProgressBarColor = this.getProgressBarColor.bind(this);
    };

    handleImageLoad(event) {
        
    }

    getProgressBarColor(positivity) {
        switch(positivity){
            case "Average":
                return "progress progress-warning";
            case "Bad": 
                return "progress progress-danger";
            case "Good":
            default:
                return "progress progress-info";
        }
    }

    renderFoodType(foodType) {
        if(foodType != null) {
            return (
                <Toggle
                    label={foodType}
                    labelPosition="right"
                    toggled={foodType == "Dine-In" ? false : true}
                    style={{marginBottom: 16}}
                    thumbStyle={{backgroundColor: '#f5b11a'}}
                    trackStyle={{backgroundColor: '#fbdd9d'}}
                    thumbSwitchedStyle={{backgroundColor: '#41bc9e'}}
                    trackSwitchedStyle={{backgroundColor: '#b3e5d8'}}
                />
            );
        } else {
            return null;
        }
    }

    renderVenue(venue) {
        if(venue != null) {
            return (
                <div className="bitecard-selected-venue">
                    <span>
                        <h7>{venue.name}</h7>
                        <span className="selected-venue-address-block">
                            <p>{venue.address1}</p>
                            <p>{venue.locality}, {venue.region} {venue.postalCode}</p>
                        </span>
                    </span>
                    <span className="venue-comment">
                        <i className="fa fa-hand-o-up fa-2x"></i> Location
                    </span>
                </div>
            );
        } else {
            return null;
        }
    }

    renderMenuItem(menuItem) {
        if(menuItem != null) {
            return (
                <div>
                    <span className="bitecard-menuItem-comment">
                        ... chowed on <i className="fa fa-hand-o-down fa-2x"></i> 
                    </span><br/>
                    <span>
                        <h7 dangerouslySetInnerHTML={{__html: menuItem.itemText + ' <p>$' + menuItem.price + '</p>'}} />
                    </span>
                </div>
            );
        } else {
            return null;
        }
    }

    renderFoodTags(tags) {
        const foodTags = tags;
        if(foodTags !== null && foodTags[0] != "") {
            return (
                foodTags.map(tag =>
                    <div key={tag}>
                        <IconButton tooltip={tag} tooltipPosition="top-left"
                                iconStyle={{position: 'absolute', top: 0, height: 35, width: 35}} style={{width:-1}}
                            >
                                <img src={"/assets/images/food_icons/svg/" + tag + ".svg"} type="image/svg+xml"></img>
                        </IconButton>
                    </div>
                )
            );
        }
        else {
            return null;                    
        }
    }

    renderAllergens(tags) {
        const allergenTags = tags;
        if(allergenTags !== null && allergenTags[0] != "") {            
            return (
                allergenTags.map(tag =>
                    <div key={tag}>
                        <IconButton tooltip={tag} tooltipPosition="top-left"
                            iconStyle={{marginTop: -10}} style={{width:-1}}
                        >
                            <svg width="100%" height="100%" viewBox="0 0 100 100">
                                <image href={"/assets/images/food_icons/svg/Allergens/" + tag + ".svg"} x="0" y="0" height="100" width="100"/>
                            </svg>
                        </IconButton>
                    </div>
                )
            );
        }
        else {
            return null;                    
        }
    }

    renderSurvey(questionResponseSet, categoryScores) {
        if(questionResponseSet != null && questionResponseSet.totalScore != null) {
            var categoryScores;
            if(categoryScores == undefined){
                categoryScores = this.getCategoryScores(questionResponseSet.questionResponses);
            }
            return (
                <div className="col-md-12">
                    {
                        categoryScores.map(catScore => 
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
        const {item, onLike, onComment, onShare, width} = this.props;
        var isExpanded = item.images !== null ? item.images.length > 0 ? true : false : false;
        return (
            <div className={ width + " mix category_1 mix_all"}>
                <Paper zDepth={2}>
                    <Card className="portfolio-item"
                        initiallyExpanded={isExpanded}>
                        <CardHeader
                            title=""                        
                            actAsExpander={true}
                            showExpandableButton={true}
                            >
                            <span style={{display: "none"}}>{item.id}</span>
                            <Actor  {...item.actor}  />
                        </CardHeader>
                        <CardMedia
                            expandable={true}
                            style={isExpanded ? { display: "block"} : { display: "none"}}
                        >
                            <ImageGallery ref={i => this._imageGallery = i}
                                items={item.images} 
                                slideInterval={5000}
                                onImageLoad={this.handleImageLoad}
                            />
                        </CardMedia>
                        <CardTitle title={item.comment} />
                        <CardText expandable={false}>
                            <div className="row">
                                <div className="col-md">
                                    {this.renderFoodType(item.foodType)}
                                </div>
                            </div>
                            <div className="row">
                                <div className="col-md venue-display">
                                    {this.renderVenue(item.venue)}
                                </div>
                                <div className="col-md menu-item-display">
                                    {this.renderMenuItem(item.menuItem)}
                                </div>
                            </div>
                            <div className="row">
                                <div className="col-md food-tag-display">
                                    {this.renderFoodTags(item.foodTags)}
                                </div>
                                <div className="col-md food-tag-display">
                                    {this.renderAllergens(item.allergenTags)}
                                </div>
                            </div>
                            <div className="row">
                                {this.renderSurvey(item.questionResponseSet, item.categoryScores)}
                            </div>
                        </CardText>
                        <CardActions expandable={false} >
                            <LikeButton
                                id={item.id}
                                userID={item.userId}
                                liked={item.liked}
                                onLike={onLike} />

                            <CommentButton
                                id={item.id}
                                userID={item.userId}
                                onComment={onComment}
                            />

                            <ShareButton
                                id={item.id}
                                userID={item.userId}
                                onShare={onShare}
                            />
                        </CardActions>
                    </Card>
                </Paper>
            </div>
        );
    }

    //PRIVATE METHODS
    getTotalScore(questionResponsesList) {
        var questionScores = [];
        var questionFactors = [];
        var totalQuestionFactors = 0;
        var totalScore = 0;
        if (questionResponsesList != null) {
            //Collect all score and factors
            for (var i = 0; i < questionResponsesList.length; i++) {
                var q = questionResponsesList[i];
                if (q.question.questionType.toUpperCase() != "COMMENT" && q.question.questionType.toUpperCase() != "CHECKBOX" && questionResponsesList[i].answerId != null) {
                    var questionResponseScore = questionResponsesList[i].questionScore;
                    var questionFactor = q.question.questionFactor;
                    questionScores.push(questionResponseScore);
                    questionFactors.push(questionFactor);
                    totalQuestionFactors += questionFactor;
                }
            }
            //Compute total weighted score
            for (var j = 0; j < questionScores.length; j++) {
                var weightedScore = questionScores[j] * (questionFactors[j] / totalQuestionFactors);
                totalScore += weightedScore;
            }
            return totalScore;
        }
    }

    getPositivity(totalScore) {
        if (totalScore != null) {
            if (totalScore >= 0.80)
                return "Good";
            else if(totalScore > 0.65 && totalScore < 0.79){
                return "Average";
            } else
                return "Bad";
        }
        else
            return "N/A";
    }

    getCategoryScores(questionResponsesList) {
        const BiteCard = this;
        var categoryScores = [];
        var categories = _.uniq(_.map(questionResponsesList, 'question.category.name'));
        _.forEach(categories, function(obj) {
            var categoryList = _.filter(questionResponsesList, { question : { category : { name: obj}}});
            var  totalCategoryScore = BiteCard.getTotalScore(categoryList);
            categoryScores.push({
                category: obj,
                totalScore: totalCategoryScore,
                positivity: BiteCard.getPositivity(totalCategoryScore)
            });
        });
        return categoryScores;
    }
} //END class BiteCard

export default BiteCard;