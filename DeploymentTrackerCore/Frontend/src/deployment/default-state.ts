interface Filters {
  branchName: string;
  status: string;
  type: string | null;
}

interface DeploymentState {
  addingADeployment: boolean;
  saveInProgress: boolean;
  deploymentBeingAdded: any;
  filters: Filters;
}

const createDefaultState = (): DeploymentState => ({
    addingADeployment: false,
    saveInProgress: false,
    deploymentBeingAdded: null,
    filters: {
        branchName: '',
        status: 'running',
        type: null,
    },
});

export { Filters, DeploymentState };

export default createDefaultState;