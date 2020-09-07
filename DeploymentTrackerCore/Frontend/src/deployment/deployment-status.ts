/* eslint-disable implicit-arrow-linebreak */
import { Deployment, DeploymentStatus } from './deployment-definition';

export const statusIsRunning = (status: DeploymentStatus): boolean =>
    status === DeploymentStatus.Running;
export const statusIsDestroyed = (status: DeploymentStatus): boolean =>
    status === DeploymentStatus.Destroyed;

export const deploymentIsRunning = ({ status }: Deployment): boolean =>
    statusIsRunning(status);
export const deploymentIsDestroyed = ({ status }: Deployment): boolean =>
    statusIsDestroyed(status);
