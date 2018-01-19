// This script removes demo app files
import rimraf from 'rimraf';
import fs from 'fs';
import {chalkSuccess} from './chalkConfig';

/* eslint-disable no-console */

const pathsToRemove = [
  './Scripts/app/actions/*',
  './Scripts/app/components/*',
  './Scripts/app/constants/*',
  './Scripts/app/containers/*',
  './Scripts/app/images',
  './Scripts/app/reducers/*',
  './Scripts/app/store/store.spec.js',
  './Scripts/app/styles',
  './Scripts/app/routes.js',
  './Scripts/app/index.js'
];

const filesToCreate = [
  {
    path: './Scripts/app/components/emptyTest.spec.js',
    content: '// Must have at least one test file in this directory or Mocha will throw an error.'
  },
  {
    path: './Scripts/app/index.js',
    content: '// Set up your application entry point here...'
  },
  {
    path: './Scripts/app/reducers/index.js',
    content: '// Set up your root reducer here...\n import { combineReducers } from \'redux\';\n export default combineReducers;'
  }
];

function removePath(path, callback) {
  rimraf(path, error => {
    if (error) throw new Error(error);
    callback();
  });
}

function createFile(file) {
  fs.writeFile(file.path, file.content, error => {
    if (error) throw new Error(error);
  });
}

let numPathsRemoved = 0;
pathsToRemove.map(path => {
  removePath(path, () => {
    numPathsRemoved++;
    if (numPathsRemoved === pathsToRemove.length) { // All paths have been processed
      // Now we can create files since we're done deleting.
      filesToCreate.map(file => createFile(file));
    }
  });
});

console.log(chalkSuccess('Demo app removed.'));
