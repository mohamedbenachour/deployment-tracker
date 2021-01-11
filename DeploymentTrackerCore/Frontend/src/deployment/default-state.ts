import UserActionDetail from '../shared/definitions/user-action-detail';

interface Filters {
    branchName: string;
    status: string;
    type: string | null;
    onlyMine: boolean;
}

interface DeploymentNote {
    content: string;
    id: number;
    createdBy: UserActionDetail;
}

interface DeploymentState {
    addingADeployment: boolean;
    saveInProgress: boolean;
    deploymentBeingAdded: unknown;
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
        onlyMine: false,
    },
});

export { Filters, DeploymentState, DeploymentNote };

export default createDefaultState;
