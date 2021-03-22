import { Deployment } from '../deployment/deployment-definition';

interface Environment {
    deployments: Deployment[];
    id: number;
    name: string;
}

export { Environment };
