import { createSelector } from 'reselect';
import {
    Deployment,
    DeploymentType,
} from '../deployment/deployment-definition';
import ApplicationState from '../state-definition';
import { Environment } from './environment-definition';

export const getLoading = ({
    environment: {
        environments: { loading },
    },
}: ApplicationState): boolean => loading;

export const getEnvironments = ({
    environment: {
        environments: { data },
    },
}: ApplicationState): Environment[] => data || [];

export const getDeployments = createSelector(
    [getEnvironments],
    (environments): Deployment[] => environments.flatMap(
        (environment): Deployment[] => environment.deployments || [],
    ),
);

export const getTypes = createSelector(
    [getDeployments],
    (deployments): DeploymentType[] => {
        const resultKeys: Record<string, DeploymentType> = {};

        deployments.forEach(({ type }) => {
            resultKeys[type.name] = type;
        });

        return Object.values(resultKeys);
    },
);
