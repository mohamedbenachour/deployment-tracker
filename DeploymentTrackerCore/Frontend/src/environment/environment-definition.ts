import { Deployment } from '../deployment/deployment-definition';

interface Environment {
    deployments: Deployment[];
}

export { Environment };
