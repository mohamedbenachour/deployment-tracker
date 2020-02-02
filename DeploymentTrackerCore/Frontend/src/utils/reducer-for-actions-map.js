import { produce } from 'immer';

const handleAction = (draft, action, actionsMap) => {
    const { type } = action;

    if (actionsMap[type]) {
        actionsMap[type](draft, action);
    }
};

const getReducerForActionsMap = (actionsMap, initialState = {}) => {
    return produce((draft, action) => handleAction(draft, action, actionsMap), initialState);
};

export default getReducerForActionsMap;