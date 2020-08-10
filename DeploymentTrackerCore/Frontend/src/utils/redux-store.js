import thunk from 'redux-thunk';
import {
    createStore as originalCreateStore,
    applyMiddleware,
    compose,
} from 'redux';

const getReduxDevtools = () => {
    /* eslint-disable-next-line no-underscore-dangle */
    const reduxDevTools = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__;

    if (process.env.NODE_ENV === 'development' && reduxDevTools) {
        return reduxDevTools({ trace: true, traceLimit: 25 });
    }

    return reduxDevTools;
};

const composeEnhancers = getReduxDevtools() || compose;

export const createStore = (reducer, initialState = undefined) => originalCreateStore(
    reducer,
    initialState,
    composeEnhancers(applyMiddleware(thunk)),
);
