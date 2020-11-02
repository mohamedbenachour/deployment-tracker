import UserActionDetail from '../shared/definitions/user-action-detail';

enum DeploymentStatus {
  Destroyed = 'DESTROYED',
  Running = 'RUNNING',
}

interface DeploymentManagementUrls {
  deploymentTriggerUrl: string | null;
}

interface Deployment {
  branchName: string;
  status: DeploymentStatus;
  modifiedBy: UserActionDetail;
  createdBy: UserActionDetail;
  siteName: string;
  teardownUrl: string;
  managementUrls: DeploymentManagementUrls;
  hasNotes: boolean;
}

export { Deployment, DeploymentStatus };
