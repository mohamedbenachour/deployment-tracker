import produce from 'immer';

import {
    DEPLOYMENT_ADD_CLICKED,
    DEPLOYMENT_ADD_CANCEL,
    DEPLOYMENT_ADD_BRANCH_NAME_CHANGE,
    DEPLOYMENT_ADD_SITE_NAME_CHANGE,
    DEPLOYMENT_ADD_PUBLIC_URL_CHANGE,
    DEPLOYMENT_ADD_ENVIRONMENT_CHANGE,
    DEPLOYMENT_ADD,
    DEPLOYMENT_NEW,
    DEPLOYMENT_SAVE_STARTED,
    DEPLOYMENT_SAVE_FAILED,
    DEPLOYMENT_SEARCH,
    DEPLOYMENT_STATUS_FILTER_CHANGE,
    DEPLOYMENT_TYPE_FILTER_CHANGE
} from './action-types';

const getLoadingData = (loading = false) => ({
    loading,
    data: null,
    loadError: null
});

const defaultState = {
    addingADeployment: false,
    saveInProgress: false,
    deploymentBeingAdded: null,
    filters: {
        branchName: '',
        status: 'running',
        type: null
    }
};

const deploymentReducer = (state = defaultState, action) => {
    let nextState = state;

    switch (action.type) {
        case DEPLOYMENT_SAVE_STARTED:
            nextState = produce(state, draftState => { draftState.saveInProgress = true; });
            break;

        case DEPLOYMENT_SAVE_FAILED:
            nextState = produce(state, draftState => { draftState.saveInProgress = false; });
        break;

        case DEPLOYMENT_ADD_CANCEL:
            nextState = produce(state, draftState => {
                draftState.addingADeployment = false;
                draftState.deploymentBeingAdded = null;
            });
            break;
        case DEPLOYMENT_ADD_CLICKED:
            nextState = produce(state, draftState => {
                draftState.addingADeployment = true;
                draftState.deploymentBeingAdded = {
                    branchName: '',
                    publicURL: 'https://',
                    environmentId: null,
                    siteName: '',
                };
            });

            break;
        
        case DEPLOYMENT_SEARCH:
            nextState = produce(state, draftState => {
                draftState.filters.branchName = action.searchName;
            });
            break;

        case DEPLOYMENT_STATUS_FILTER_CHANGE:
            nextState = produce(state, draftState => {
                draftState.filters.status = action.value;
            });
            break;

        case DEPLOYMENT_TYPE_FILTER_CHANGE:
            nextState = produce(state, draftState => {
                draftState.filters.type = action.typeId;
            });
            break;

        case DEPLOYMENT_ADD_BRANCH_NAME_CHANGE:
            nextState = produce(state, draftState => {
                draftState.deploymentBeingAdded.branchName = action.branchName;
            });

            break;

        case DEPLOYMENT_ADD_SITE_NAME_CHANGE:
            nextState = produce(state, draftState => {
                draftState.deploymentBeingAdded.siteName = action.siteName;
            });

            break;

        case DEPLOYMENT_ADD_PUBLIC_URL_CHANGE:
            nextState = produce(state, draftState => {
                draftState.deploymentBeingAdded.publicURL = action.publicURL;
            });

            break;

        case DEPLOYMENT_ADD_ENVIRONMENT_CHANGE:
            nextState = produce(state, draftState => {
                draftState.deploymentBeingAdded.environmentId = action.environmentId;
            });

            break;
        
        case DEPLOYMENT_NEW:
            nextState = produce(state, draftState => {
                draftState.saveInProgress = false;
                draftState.addingADeployment = false;
                draftState.deploymentBeingAdded = null;
            });

        default:
            break;
    }

    return nextState;
};

export default deploymentReducer;