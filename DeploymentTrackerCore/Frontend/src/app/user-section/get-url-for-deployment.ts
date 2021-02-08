import { Deployment } from '../../deployment/deployment-definition';

const getUrlForDeployment = (deployment: Deployment): string => `/#?branchName=${encodeURI(deployment.branchName)}`;

export default getUrlForDeployment;
