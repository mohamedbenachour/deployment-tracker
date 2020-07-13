/* eslint-disable no-param-reassign */

import produce from 'immer';

import { APP_SECTION_CHANGED } from './action-types';

const defaultState = {
    section: 'deployments',
};

const reducer = (state = defaultState, action) => {
    let nextState = state;

    switch (action.type) {
    case APP_SECTION_CHANGED:
        if (action.section !== state.section) {
            nextState = produce(state, (draftState) => { draftState.section = action.section; });
        }

        break;

    default:
        break;
    }

    return nextState;
};

export default reducer;
