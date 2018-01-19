import React, { PropTypes } from 'react'
import { Route } from 'react-router';
import ReactDOM from 'react-dom'

import {connect} from 'react-redux'
import {bindActionCreators} from 'redux'
import RcUpload from 'rc-upload'
import Dropzone from 'react-dropzone'
import ReactTooltip  from 'react-tooltip'

import * as menuActions from '../Menu/menuActions'
//import * as stateProvinceActions from '../../actions/stateProvincesActions'
import * as _ from 'lodash'
//import * as ReactToastr from 'react-toastr'
//import {ToastContainer} from 'react-toastr'

import {Card, CardActions, CardHeader, CardMedia, CardTitle, CardText} from 'material-ui/Card'
import FontIcon from 'material-ui/FontIcon'
import { Step, Stepper, StepLabel, StepButton } from 'material-ui/Stepper'
import {RadioButton, RadioButtonGroup} from 'material-ui/RadioButton'
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

import { orange300 } from 'material-ui/styles/colors'
import styles from './styles.css'

class Survey extends React.Component {
    constructor(props, context) {
        super(props, context);

        const Survey = this;

        this.state = {
            finished: false,
            stepIndex: 0,
            nextDisabled: true,
            surveyQuestions: _.sortBy(this.props.survey.questions, ['sequence']),
            surveyQuestionResponses: this.props.surveyQuestionResponseSet.questionResponses,
            initialTime: new Date()
        };
        
        //PROPS

        //METHODS
        this.handleNext = this.handleNext.bind(this);
        this.handlePrev = this.handlePrev.bind(this);
        this.handleFinish = this.handleFinish.bind(this);
        this.getQuestion = this.getQuestion.bind(this);
        this.getAnswerChoices = this.getAnswerChoices.bind(this);
        this.updateSurveyQuestionResponses = this.updateSurveyQuestionResponses.bind(this);
        this.checkQuestionOptional = this.checkQuestionOptional.bind(this);

        //PRIVATE METHODS
        this.getQuestionScore = this.getQuestionScore.bind(this);
        this.getTotalScore = this.getTotalScore.bind(this);
        this.getPositivity = this.getPositivity.bind(this);
        this.getCategoryScores = this.getCategoryScores.bind(this);
        this.displayAnswer = this.displayAnswer.bind(this);
    }
	
    componentDidMount() {
        this.checkQuestionOptional(0);
        this.displayAnswer(0);
    }

    componentDidUpdate(prevProps, prevState) {
        if(this.state.stepIndex != prevState.stepIndex){
            this.checkQuestionOptional(this.state.stepIndex);            
        }
    }

    componentWillReceiveProps (nextProps) {

    }

    handleNext() {
        const {stepIndex} = this.state;
        this.displayAnswer(stepIndex + 1);
        this.setState({
            stepIndex: stepIndex + 1,
            finished: stepIndex >= this.props.survey.questions.length - 1,
        });
    }

    handlePrev() {
        const {stepIndex} = this.state;
        this.displayAnswer(stepIndex - 1);
        if (stepIndex > 0) {
            this.setState({stepIndex: stepIndex - 1});
        }
    }

    handleFinish() {
        var finalTime = new Date();
        var sessionDuration = (finalTime.getTime() - this.state.initialTime.getTime()) / 1000;
        var surveyQuestionResponseSet = {
            survey: this.props.survey,
            surveyId: this.props.survey.id,
            questionResponses: this.state.surveyQuestionResponses,
            customerEmail: null,
            sessionDuration: sessionDuration,
            isSubscribe: false,
            positivity: null,
            totalScore: null,
            categoryScores: [],
            userId: this.props.fsLogin.id,
            createdBy: "Web User Entry",
            updatedBy: "Web User Entry"
        };
        var totalScore = this.getTotalScore(this.state.surveyQuestionResponses);
        surveyQuestionResponseSet.totalScore = totalScore;
        surveyQuestionResponseSet.positivity = this.getPositivity(totalScore);
        surveyQuestionResponseSet.categoryScores = this.getCategoryScores(this.state.surveyQuestionResponses);
        this.props.completeSurvey(surveyQuestionResponseSet);
    }

    updateSurveyQuestionResponses(event, answerId) {
        var question = _.find(this.state.surveyQuestions, { 'sequence': this.state.stepIndex + 1 });
        let surveyQuestionResponses = this.state.surveyQuestionResponses;
        var questionScore = this.getQuestionScore(question, answerId) 
        if (_.find(this.state.surveyQuestionResponses, { 'questionId': question.id }) === undefined)
            surveyQuestionResponses.push({ 
                'position': this.state.stepIndex, 
                'questionId': question.id, 
                'answerId': answerId, 
                'surveyId': this.props.survey.id, 
                'questionType': question.questionType, 
                'questionFactor': question.questionFactor, 
                'questionScore': questionScore,
                'category': question.category.name
            });
        else {
            var questionResponse = _.find(surveyQuestionResponses, { 'questionId': question.id });
            if(questionResponse !== undefined) {
                questionResponse.answerId = answerId;
                questionResponse.questionScore = questionScore;
            }
        }
        this.setState({
            stepIndex: this.state.stepIndex + 1,
            surveyQuestionResponses: surveyQuestionResponses,
            finished: this.state.stepIndex >= this.props.survey.questions.length - 1,
        });
    }

    renderSurveyHeader() {
        return (
            <Stepper activeStep={this.state.stepIndex} className="survey-header">
                {this.state.surveyQuestions.map(q =>
                    <Step key={q.id}>
                        <StepButton>{q.category.name}</StepButton>
                    </Step>                
                )}           
            </Stepper>
        );
    }

    renderSurveyContent() {
        if(this.state.finished && this.state.stepIndex > this.props.survey.questions.length - 1) {
            return (
                <div>
                    <p>
                        <a href="#" onClick={(event) => {
                                event.preventDefault();
                                this.setState({stepIndex: 0, finished: false, surveyQuestionResponses: []});
                            }}
                        >
                            Click here
                        </a> to reset the survey.
                    </p>
                    <FlatButton
                        label="Back"
                        disabled={this.state.stepIndex === 0}
                        onTouchTap={this.handlePrev}
                        style={{marginRight: 12}}
                    />
                    <RaisedButton
                        label='Finish'
                        primary={true}
                        onTouchTap={this.handleFinish}
                    />
                </div>
            );
        } 
        else {
            return (
                <div>
                    <div>{this.getQuestion(this.state.stepIndex)}</div>
                    <div style={{marginTop: 12}}>
                        <FlatButton
                            label="Back"
                            disabled={this.state.stepIndex === 0}
                            onTouchTap={this.handlePrev}
                            style={{marginRight: 12}}
                        />
                        <RaisedButton
                            label='Next'
                            primary={true}
                            onTouchTap={this.handleNext}
                            disabled={this.state.nextDisabled}
                        />
                    </div>
                </div>
            );
        }
    }

    getQuestion(stepIndex) {
        if(stepIndex < this.state.surveyQuestions.length) {
            const q = this.state.surveyQuestions[stepIndex];        
            return (
                <div className="survey-body">
                    <div className="row">
                        <div className="col-md">
                            <h3>{q.questionText}</h3>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-md">
                            { this.getAnswerChoices(q)}
                        </div>
                    </div>
                </div>
            );
        } else {
            return null;
        }
    }

    getAnswerChoices(q) {
        return (  
            <RadioButtonGroup name="questionAnswers" onChange={this.updateSurveyQuestionResponses} valueSelected={this.state.selectedValue}>
                {q.answers.map(a => 
                    <RadioButton key={a.id}
                        value={a.id}
                        label={a.answerText}
                        style={{marginBottom: 16}}
                    />
                )}
            </RadioButtonGroup>
        );
    }

    render () {
        const contentStyle = {margin: '0 16px'};

        return (
            <div style={{width: '100%', maxWidth: 700, margin: 'auto'}}>
                {this.renderSurveyHeader()}
                <div style={contentStyle}>
                    {this.renderSurveyContent()}
                </div>
              </div>
        )
    }
    
    //PRIVATE METHODS
    checkQuestionOptional(stepIndex) {
        if(stepIndex < this.state.surveyQuestions.length && this.state.surveyQuestions[stepIndex].isOptional == false && _.find(this.state.surveyQuestionResponses, { 'position': stepIndex }) === undefined) {
            this.setState({
                nextDisabled: true
            })
        } else {
            this.setState({
                nextDisabled: false
            })
        }
    }

    getQuestionScore(questionvm, answerId) {
        var score = 0;
        for (var i = 0; i < questionvm.answers.length; i++) {
            if (questionvm.answers[i].id == answerId) {
                return 1 - (i / (questionvm.answers.length * 1.0));
            }
        }
    }

    getTotalScore(questionResponsesList) {
        var questionScores = [];
        var questionFactors = [];
        var totalQuestionFactors = 0;
        var totalScore = 0;
        if (questionResponsesList != null) {
            //Collect all score and factors
            for (var i = 0; i < questionResponsesList.length; i++) {
                var q = questionResponsesList[i];
                if (q.questionType.toUpperCase() != "COMMENT" && q.questionType.toUpperCase() != "CHECKBOX" && questionResponsesList[i].answerId != null) {
                    var questionResponseScore = questionResponsesList[i].questionScore;
                    var questionFactor = q.questionFactor;
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
                return "Average"
            } else
                return "Bad";
        }
        else
            return "N/A";
    }

    getCategoryScores(questionResponsesList) {
        const Survey = this;
        var categoryScores = [];
        var categories = _.uniq(_.map(this.props.survey.questions, 'category.name'));
        _.forEach(categories, function(obj) {
            var categoryList = _.filter(questionResponsesList, { category: obj});
            var  totalCategoryScore = Survey.getTotalScore(categoryList);
            categoryScores.push({
                category: obj,
                totalScore: totalCategoryScore,
                positivity: Survey.getPositivity(totalCategoryScore)
            });
        });
        return categoryScores;
    }

    displayAnswer(stepIndex) {
        var questionResponse = _.find(this.state.surveyQuestionResponses, {'position': stepIndex});        
        this.setState({ selectedValue: questionResponse == undefined ? null : questionResponse.answerId});
    }
} //End class Survey

function mapStateToProps(state, ownProps) {
    return {
        fsLogin: state.fsLogin
    };
}

function mapDispatchToProps(dispatch) {
    return {
    };
}

export default connect(mapStateToProps, mapDispatchToProps)(Survey);