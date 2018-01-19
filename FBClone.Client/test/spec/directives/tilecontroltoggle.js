'use strict';

describe('Directive: tileControlToggle', function () {

  // load the directive's module
  beforeEach(module('fbCloneApp'));

  var element,
    scope;

  beforeEach(inject(function ($rootScope) {
    scope = $rootScope.$new();
  }));

  it('should make hidden element visible', inject(function ($compile) {
    element = angular.element('<tile-control-toggle></tile-control-toggle>');
    element = $compile(element)(scope);
    expect(element.text()).toBe('this is the tileControlToggle directive');
  }));
});
