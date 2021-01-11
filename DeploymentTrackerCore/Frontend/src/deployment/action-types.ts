import { Deployment } from './deployment-definition';

export const DEPLOYMENT_ADD_CLICKED = 'DAC';
export const DEPLOYMENT_ADD_BRANCH_NAME_CHANGE = 'DABNC';
export const DEPLOYMENT_ADD_SITE_NAME_CHANGE = 'DASNC';
export const DEPLOYMENT_ADD_PUBLIC_URL_CHANGE = 'DAPUC';
export const DEPLOYMENT_ADD_ENVIRONMENT_CHANGE = 'DAEC';

export const DEPLOYMENT_ADD_CANCEL = 'DACC';
export const DEPLOYMENT_NEW = 'DN';
export const DEPLOYMENT_SAVE_STARTED = 'DSS';
export const DEPLOYMENT_SAVE_FAILED = 'DSF';

export const DEPLOYMENT_SEARCH = 'DSRCH';
export const DEPLOYMENT_STATUS_FILTER_CHANGE = 'DSFC';
export const DEPLOYMENT_TYPE_FILTER_CHANGE = 'DTFS';
export const DEPLOYMENT_ONLY_MINE_FILTER_CHANGE = 'DOMFC';

export const JIRA_STATUS_UPDATE = 'JSU';

interface DeploymentAddClickedAction {
    type: typeof DEPLOYMENT_ADD_CLICKED;
}

interface DeploymentAddCancelledAction {
    type: typeof DEPLOYMENT_ADD_CANCEL;
}

interface DeploymentBeingSavedAction {
    type: typeof DEPLOYMENT_SAVE_STARTED;
}

interface DeploymentSaveFailedAction {
    type: typeof DEPLOYMENT_SAVE_FAILED;
}

interface DeploymentBeingAddedBranchNameChangedAction {
    type: typeof DEPLOYMENT_ADD_BRANCH_NAME_CHANGE;
    branchName: string;
}

interface DeploymentBeingAddedSiteNameChangedAction {
    type: typeof DEPLOYMENT_ADD_SITE_NAME_CHANGE;
    siteName: string;
}

interface DeploymentBeingAddedPublicURLChangedAction {
    type: typeof DEPLOYMENT_ADD_PUBLIC_URL_CHANGE;
    publicURL: string;
}
interface DeploymentBeingAddedEnvironmentChangedAction {
    type: typeof DEPLOYMENT_ADD_ENVIRONMENT_CHANGE;
    environmentId: number;
}

interface NewDeploymentAction {
    type: typeof DEPLOYMENT_NEW;
    deployment: Deployment;
}

interface DeploymentSearchAction {
    type: typeof DEPLOYMENT_SEARCH;
    searchName: string;
}

interface DeploymentStatusFilterChangedAction {
    type: typeof DEPLOYMENT_STATUS_FILTER_CHANGE;
    value: string;
    externallyInitiated: boolean;
}

interface DeploymentOnlyMinelterChangedAction {
    type: typeof DEPLOYMENT_ONLY_MINE_FILTER_CHANGE;
    value: boolean;
    externallyInitiated: boolean;
}

interface DeploymentTypeFilterChangedAction {
    type: typeof DEPLOYMENT_TYPE_FILTER_CHANGE;
    typeId: number;
}

interface JiraStatusUpdateAction {
    type: typeof JIRA_STATUS_UPDATE;
    jiraIssue: string;
    jiraStatus: string;
}

export type DeploymentActionTypes =
  | DeploymentAddClickedAction
  | DeploymentAddCancelledAction
  | DeploymentBeingSavedAction
  | DeploymentSaveFailedAction
  | DeploymentBeingAddedBranchNameChangedAction
  | DeploymentBeingAddedSiteNameChangedAction
  | DeploymentBeingAddedPublicURLChangedAction
  | DeploymentBeingAddedEnvironmentChangedAction
  | NewDeploymentAction
  | DeploymentSearchAction
  | DeploymentStatusFilterChangedAction
  | DeploymentOnlyMinelterChangedAction
  | DeploymentTypeFilterChangedAction
  | JiraStatusUpdateAction;
