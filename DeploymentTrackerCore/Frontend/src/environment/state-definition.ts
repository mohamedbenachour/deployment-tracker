import { LoadableData } from '../shared/definitions/state';
import { Environment } from './environment-definition';

interface EnvironmentState {
    environments: LoadableData<Environment[]>;
}

export default EnvironmentState;
