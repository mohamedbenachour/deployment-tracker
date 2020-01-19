import {
    DEPLOYMENT_ADD_CLICKED,
    DEPLOYMENT_ADD_BRANCH_NAME_CHANGE,
    DEPLOYMENT_ADD_SITE_NAME_CHANGE,
    DEPLOYMENT_ADD_PUBLIC_URL_CHANGE,
    DEPLOYMENT_ADD_ENVIRONMENT_CHANGE,
    DEPLOYMENT_ADD_CANCEL,
    DEPLOYMENT_NEW,
    DEPLOYMENT_ADD,
    DEPLOYMENT_SAVE_STARTED,
    DEPLOYMENT_SAVE_FAILED,
    DEPLOYMENT_SEARCH,
    DEPLOYMENT_STATUS_FILTER_CHANGE,
    DEPLOYMENT_TYPE_FILTER_CHANGE,

    JIRA_STATUS_UPDATE
} from './action-types';

export const deploymentAddClicked = () => ({
    type: DEPLOYMENT_ADD_CLICKED,
});

export const deploymentAddCancelled = () => ({
    type: DEPLOYMENT_ADD_CANCEL,
});

export const deploymentBeingSaved = () => ({
    type: DEPLOYMENT_SAVE_STARTED,
});

export const deploymentSaveFailed = () => ({
    type: DEPLOYMENT_SAVE_FAILED,
});

export const deploymentBeingAddedBranchNameChanged = (branchName) => ({
    type: DEPLOYMENT_ADD_BRANCH_NAME_CHANGE,
    branchName,
});

export const deploymentBeingAddedSiteNameChanged = (siteName) => ({
    type: DEPLOYMENT_ADD_SITE_NAME_CHANGE,
    siteName,
});

export const deploymentSearch = (searchName) => ({
    type: DEPLOYMENT_SEARCH,
    searchName,
});

export const deploymentStatusFilterChanged = (value) => ({
    type: DEPLOYMENT_STATUS_FILTER_CHANGE,
    value,
});

export const deploymentBeingAddedPublicURLChanged = (publicURL) => ({
    type: DEPLOYMENT_ADD_PUBLIC_URL_CHANGE,
    publicURL,
});

export const deploymentBeingAddedEnvironmentChanged = (environmentId) => ({
    type: DEPLOYMENT_ADD_ENVIRONMENT_CHANGE,
    environmentId,
});

export const newDeployment = (deployment) => ({
    type: DEPLOYMENT_NEW,
    deployment
});

export const jiraStatusUpdate = (jiraIssue, jiraStatus) => ({
    type: JIRA_STATUS_UPDATE,
    jiraIssue,
    jiraStatus
});

export const deploymentTypeFilterChange = (typeId) => ({
    type: DEPLOYMENT_TYPE_FILTER_CHANGE,
    typeId
});