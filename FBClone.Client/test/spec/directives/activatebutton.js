'use strict';

describe('Directive: activateButton', function () {

  // load the directive's module
  beforeEach(module('fbCloneApp'));

  var element,
    scope;

  beforeEach(inject(function ($rootScope) {
    scope = $rootScope.$new();
  }));

  it('should make hidden element visible', inject(function ($compile) {
    element = angular.element('<activate-button></activate-button>');
    element = $compile(element)(scope);
    expect(element.text()).toBe('this is the activateButton directive');
  }));
});
