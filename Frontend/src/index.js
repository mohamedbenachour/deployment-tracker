import React from 'react';
import ReactDom from 'react-dom';

import { combineReducers } from 'redux';

import StandardLayout from './layout/standard-layout';

import HeaderMenu from './app/connected-header-menu';
import Content from './app/connected-content';
import { createStore } from './utils/redux-store';

import { Provider } from 'react-redux';

import appReducer from './app/reducer';

import environmentsReducer from './environment/reducer';
import deploymentsReducer from './deployment/reducer';

import { loadEnvironmentList } from './environment/async-actions';

const applicationStart = () => {
    const rootReducer = combineReducers({ app: appReducer, environment: environmentsReducer, deployment: deploymentsReducer });
    const store = createStore(rootReducer);

    store.dispatch(loadEnvironmentList());

    ReactDom.render((
        <Provider store={store}>
            <StandardLayout header={<HeaderMenu />}>
                <Content />
            </StandardLayout>
        </Provider>
    ),
    document.querySelector(".app-mount-point"));
};

document.addEventListener('DOMContentLoaded', applicationStart, { once: true });