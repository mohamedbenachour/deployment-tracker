import React from 'react';

import { combineReducers } from 'redux';

import { Provider } from 'react-redux';
import bootstrapToPage from './utils/page-bootstrapper';

import StandardLayout from './layout/standard-layout';

import HeaderMenu from './app/connected-header-menu';
import Content from './app/connected-content';
import { createStore } from './utils/redux-store';


import appReducer from './app/reducer';

import environmentsReducer from './environment/reducer';
import deploymentsReducer from './deployment/reducer';

import { loadEnvironmentList } from './environment/async-actions';

import DeploymentHubClient from './deployment/deployment-hub-client';
import JiraHubClient from './jira/jira-hub-client';
import Notifier from './app/notifier';

const rootReducer = combineReducers({ app: appReducer, environment: environmentsReducer, deployment: deploymentsReducer });
const store = createStore(rootReducer);

store.dispatch(loadEnvironmentList());

new DeploymentHubClient(store, new Notifier()).start();
new JiraHubClient(store).start();

bootstrapToPage(
    <Provider store={store}>
        <StandardLayout header={<HeaderMenu />}>
            <Content />
        </StandardLayout>
    </Provider>,
);
