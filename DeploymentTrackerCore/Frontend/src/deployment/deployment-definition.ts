import JiraStatus from '../jira/jira-status';
import UserActionDetail from '../shared/definitions/user-action-detail';

enum DeploymentStatus {
    Destroyed = 'DESTROYED',
    Running = 'RUNNING',
}

interface DeploymentManagementUrls {
    deploymentTriggerUrl: string | null;
}

interface DeploymentType {
    id: number;
    name: string;
}

interface JiraDetail {
    url: string;
    status: JiraStatus;
}

interface SiteLogin {
    userName: string;
    password: string;
}

interface Deployment {
    id: number;
    branchName: string;
    status: DeploymentStatus;
    modifiedBy: UserActionDetail;
    createdBy: UserActionDetail;
    siteName: string;
    teardownUrl: string;
    managementUrls: DeploymentManagementUrls;
    hasNotes: boolean;
    type: DeploymentType;
    properties: Record<string, string>;
    url: string;
    publicURL: string;
    jira: JiraDetail;
    environmentId: number;
    siteLogin: SiteLogin;
}

export {
    Deployment, DeploymentStatus, DeploymentType, JiraDetail, SiteLogin,
};
