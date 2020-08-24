enum DeploymentStatus {
  Destroyed = 'DESTROYED',
  Running = 'RUNNING',
}

interface UserActionDetail {
  name: string;
  userName: string;
}

interface Deployment {
  branchName: string;
  status: DeploymentStatus;
  modifiedBy: UserActionDetail;
  createdBy: UserActionDetail;
}

export { Deployment, DeploymentStatus, UserActionDetail };
