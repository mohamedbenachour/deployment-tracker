import { DeploymentState } from './deployment/default-state';
import EnvironmentState from './environment/state-definition';

interface ApplicationState {
    environment: EnvironmentState;
    deployment: DeploymentState;
}

export default ApplicationState;
