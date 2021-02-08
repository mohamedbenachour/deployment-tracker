import {
    DEPLOYMENT_ADD_CLICKED,
    DEPLOYMENT_ADD_BRANCH_NAME_CHANGE,
    DEPLOYMENT_ADD_SITE_NAME_CHANGE,
    DEPLOYMENT_ADD_PUBLIC_URL_CHANGE,
    DEPLOYMENT_ADD_ENVIRONMENT_CHANGE,
    DEPLOYMENT_ADD_CANCEL,
    DEPLOYMENT_NEW,
    DEPLOYMENT_SAVE_STARTED,
    DEPLOYMENT_SAVE_FAILED,
    DEPLOYMENT_SEARCH,
    DEPLOYMENT_STATUS_FILTER_CHANGE,
    DEPLOYMENT_TYPE_FILTER_CHANGE,
    JIRA_STATUS_UPDATE,
    DEPLOYMENT_ONLY_MINE_FILTER_CHANGE,
    DeploymentActionTypes,
} from './action-types';
import { Deployment } from './deployment-definition';

export const deploymentAddClicked = (): DeploymentActionTypes => ({
    type: DEPLOYMENT_ADD_CLICKED,
});

export const deploymentAddCancelled = (): DeploymentActionTypes => ({
    type: DEPLOYMENT_ADD_CANCEL,
});

export const deploymentBeingSaved = (): DeploymentActionTypes => ({
    type: DEPLOYMENT_SAVE_STARTED,
});

export const deploymentSaveFailed = (): DeploymentActionTypes => ({
    type: DEPLOYMENT_SAVE_FAILED,
});

export const deploymentBeingAddedBranchNameChanged = (
    branchName: string,
): DeploymentActionTypes => ({
    type: DEPLOYMENT_ADD_BRANCH_NAME_CHANGE,
    branchName,
});

export const deploymentBeingAddedSiteNameChanged = (
    siteName: string,
): DeploymentActionTypes => ({
    type: DEPLOYMENT_ADD_SITE_NAME_CHANGE,
    siteName,
});

export const deploymentSearch = (
    searchName: string,
    externallyInitiated = false,
): DeploymentActionTypes => ({
    type: DEPLOYMENT_SEARCH,
    searchName,
    externallyInitiated,
});

export const deploymentStatusFilterChanged = (
    value: string,
    externallyInitiated = false,
): DeploymentActionTypes => ({
    type: DEPLOYMENT_STATUS_FILTER_CHANGE,
    value,
    externallyInitiated,
});

export const deploymentBeingAddedPublicURLChanged = (
    publicURL: string,
): DeploymentActionTypes => ({
    type: DEPLOYMENT_ADD_PUBLIC_URL_CHANGE,
    publicURL,
});

export const deploymentBeingAddedEnvironmentChanged = (
    environmentId: number,
): DeploymentActionTypes => ({
    type: DEPLOYMENT_ADD_ENVIRONMENT_CHANGE,
    environmentId,
});

export const newDeployment = (
    deployment: Deployment,
): DeploymentActionTypes => ({
    type: DEPLOYMENT_NEW,
    deployment,
});

export const jiraStatusUpdate = (
    jiraIssue: string,
    jiraStatus: string,
): DeploymentActionTypes => ({
    type: JIRA_STATUS_UPDATE,
    jiraIssue,
    jiraStatus,
});

export const deploymentTypeFilterChange = (
    typeId: number,
): DeploymentActionTypes => ({
    type: DEPLOYMENT_TYPE_FILTER_CHANGE,
    typeId,
});

export const deploymentOnlyMineFilterChange = (
    value: boolean,
    externallyInitiated = false,
): DeploymentActionTypes => ({
    type: DEPLOYMENT_ONLY_MINE_FILTER_CHANGE,
    value,
    externallyInitiated,
});
