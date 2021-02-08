import React from 'react';

import { combineReducers } from 'redux';

import { Provider } from 'react-redux';
import { HashRouter } from 'react-router-dom';
import bootstrapToPage from './utils/page-bootstrapper';

import StandardLayout from './layout/standard-layout';

import HeaderMenu from './app/header-menu';
import Content from './app/content';
import { createStore } from './utils/redux-store';

import appReducer from './app/reducer';

import environmentsReducer from './environment/reducer';
import deploymentsReducer from './deployment/reducer';

import { loadEnvironmentList } from './environment/async-actions';
import {
    deploymentStatusFilterChanged,
    deploymentOnlyMineFilterChange,
    deploymentSearch,
} from './deployment/actions';

import DeploymentHubClient from './deployment/deployment-hub-client';
import JiraHubClient from './jira/jira-hub-client';
import Notifier from './app/notifier';
import createInitialState from './deployment/create-initial-state';
import getStatusInUrl from './deployment/get-status-in-url';
import getOnlyMineInUrl from './deployment/filters/get-only-mine-in-url';
import getSearchTermInUrl from './deployment/filters/get-search-term-in-url';

const initialDeploymentState = createInitialState();

const rootReducer = combineReducers({
    app: appReducer,
    environment: environmentsReducer,
    deployment: deploymentsReducer,
});
const store = createStore(rootReducer, { deployment: initialDeploymentState });

store.dispatch(loadEnvironmentList());

new DeploymentHubClient(store, new Notifier()).start();
new JiraHubClient(store).start();

window.addEventListener('popstate', () => {
    store.dispatch(deploymentStatusFilterChanged(getStatusInUrl(), true));
    store.dispatch(deploymentOnlyMineFilterChange(getOnlyMineInUrl(), true));
    store.dispatch(deploymentSearch(getSearchTermInUrl(), true));
});

bootstrapToPage(
    <Provider store={store}>
        <HashRouter>
            <StandardLayout header={<HeaderMenu />}>
                <Content />
            </StandardLayout>
        </HashRouter>
    </Provider>,
);
