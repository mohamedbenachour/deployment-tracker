import UserActionDetail from '../shared/definitions/user-action-detail';
import { Deployment } from './deployment-definition';

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
    deploymentBeingAdded: Deployment | null;
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
