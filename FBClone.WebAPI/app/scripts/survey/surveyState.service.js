'use strict';

angular.module('fbCloneApp')
    .service("surveyStateService", function () {
    var surveyStateService = this;
    surveyStateService.selectedSurvey = null;
    surveyStateService.selectedQuestion = null;
    surveyStateService.selectedAnswer = null;

    surveyStateService.questionGrid = null;
    surveyStateService.answerGrid = null;
    surveyStateService.refreshQuestionGrid = function () {
        surveyStateService.questionGrid.reload();
    }
});