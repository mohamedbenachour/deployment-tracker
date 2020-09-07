enum DeploymentStatus {
  Destroyed = 'DESTROYED',
  Running = 'RUNNING',
}

interface UserActionDetail {
  name: string;
  userName: string;
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
}

export { Deployment, DeploymentStatus, UserActionDetail };
