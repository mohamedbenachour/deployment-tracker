import { APP_SECTION_CHANGED } from './action-types';
import getReducerForActionsMap from '../utils/reducer-for-actions-map';

const defaultState = {
    section: 'deployments',
};

const actionsMap = {
    [APP_SECTION_CHANGED]: (draft, { section }) => {
        draft.section = section;
    }
};

const reducer = getReducerForActionsMap(actionsMap, defaultState);

export default reducer;
