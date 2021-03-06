import produce from 'immer';

import {
    ENVIRONMENT_LIST_LOADING,
    ENVIRONMENT_LIST_LOADED,

    ENVIRONMENT_ADD_CLICKED,
    ENVIRONMENT_ADD_CANCEL,
    ENVIRONMENT_ADD_NAME_CHANGE,
    ENVIRONMENT_ADD_HOST_NAME_CHANGE,
    ENVIRONMENT_SAVE_STARTED,
    ENVIRONMENT_SAVE_FAILED,
    ENVIRONMENT_NEW,
} from './action-types';

import {
    DEPLOYMENT_NEW,
    JIRA_STATUS_UPDATE,
} from '../deployment/action-types';

const getLoadingData = (loading = false) => ({
    loading,
    data: null,
    loadError: null,
});

const defaultState = {
    environments: getLoadingData(),
    addingAnEnvironment: false,
    environmentBeingAdded: null,
    saveInProgress: false,
};

const environmentReducer = (state = defaultState, action) => {
    let nextState = state;

    switch (action.type) {
    case ENVIRONMENT_LIST_LOADING:
        nextState = produce(state, (draftState) => {
            draftState.environments = getLoadingData(true);
        });

        break;

    case ENVIRONMENT_LIST_LOADED:
        nextState = produce(state, (draftState) => {
            draftState.environments = getLoadingData(false);
            draftState.environments.data = action.environments;
        });

        break;

    case ENVIRONMENT_SAVE_STARTED:
        nextState = produce(state, (draftState) => {
            draftState.saveInProgress = true;
        });
        break;

    case ENVIRONMENT_SAVE_FAILED:
        nextState = produce(state, (draftState) => {
            draftState.saveInProgress = false;
        });
        break;

    case ENVIRONMENT_ADD_CANCEL:
        nextState = produce(state, (draftState) => {
            draftState.addingAnEnvironment = false;
            draftState.environmentBeingAdded = null;
        });
        break;
    case ENVIRONMENT_ADD_CLICKED:
        nextState = produce(state, (draftState) => {
            draftState.addingAnEnvironment = true;
            draftState.environmentBeingAdded = {
                name: '',
            };
        });

        break;

    case ENVIRONMENT_ADD_NAME_CHANGE:
        nextState = produce(state, (draftState) => {
            draftState.environmentBeingAdded.name = action.name;
        });

        break;

    case ENVIRONMENT_ADD_HOST_NAME_CHANGE:
        nextState = produce(state, (draftState) => {
            draftState.environmentBeingAdded.hostName = action.hostName;
        });

        break;

    case ENVIRONMENT_NEW:
        nextState = produce(state, (draftState) => {
            draftState.environmentBeingAdded = null;
            draftState.addingAnEnvironment = false;
            draftState.saveInProgress = false;

            draftState.environments.data.push(action.environment);
        });

        break;

    case DEPLOYMENT_NEW:
        nextState = produce(state, (draftState) => {
            const environmentAddedTo = draftState.environments.data.find((env) => env.id === action.deployment.environmentId);

            const existingDeploymentIndex = environmentAddedTo.deployments.findIndex((deployment) => deployment.id === action.deployment.id);

            if (existingDeploymentIndex !== -1) {
                environmentAddedTo.deployments.splice(existingDeploymentIndex, 1, action.deployment);
            } else {
                environmentAddedTo.deployments.push(action.deployment);
            }
        });
        break;

    case JIRA_STATUS_UPDATE:
        nextState = produce(state, (draftState) => {
            const allDeployments = draftState.environments.data.flatMap((environment) => environment.deployments);
            const deploymentsThatMatchIssue = allDeployments.filter(({
                jira,
            }) => {
                if (jira) {
                    return jira.issue === action.jiraIssue;
                }
                return false;
            });

            deploymentsThatMatchIssue.forEach(({
                jira,
            }) => jira.status = action.jiraStatus);
        });
        break;

    default:
        break;
    }

    return nextState;
};

export default environmentReducer;
