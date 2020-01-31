import {
    ENVIRONMENT_LIST_LOADING,
    ENVIRONMENT_LIST_LOADED,

    ENVIRONMENT_ADD_CLICKED,
    ENVIRONMENT_ADD_CANCEL,
    ENVIRONMENT_ADD,
    ENVIRONMENT_ADD_NAME_CHANGE,
    ENVIRONMENT_ADD_HOST_NAME_CHANGE,
    ENVIRONMENT_NEW,
    ENVIRONMENT_SAVE_STARTED,
    ENVIRONMENT_SAVE_FAILED,
} from './action-types';

export const environmentListIsLoading = () => ({
    type: ENVIRONMENT_LIST_LOADING,
});

export const environmentListLoaded = (environments) => ({
    type: ENVIRONMENT_LIST_LOADED,
    environments,
});

export const environmentAddClicked = () => ({
    type: ENVIRONMENT_ADD_CLICKED,
});

export const environmentAddCancelled = () => ({
    type: ENVIRONMENT_ADD_CANCEL,
});

export const environmentBeingSaved = () => ({
    type: ENVIRONMENT_SAVE_STARTED,
});

export const environmentSaveFailed = () => ({
    type: ENVIRONMENT_SAVE_FAILED,
});

export const environmentBeingAddedNameChanged = (name) => ({
    type: ENVIRONMENT_ADD_NAME_CHANGE,
    name,
});

export const environmentBeingAddedHostNameChanged = (hostName) => ({
    type: ENVIRONMENT_ADD_HOST_NAME_CHANGE,
    hostName,
});

export const newEnvironment = (environment) => ({
    type: ENVIRONMENT_NEW,
    environment,
});
