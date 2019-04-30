import { createSelector } from 'reselect'

export const getLoading = ({ environment: { environments: { loading }}}) => loading;

export const getEnvironments = ({ environment: { environments: { data }}}) => data || [];

export const getDeployments = createSelector(
    [getEnvironments],
    (environments) => environments.flatMap((environment) => environment.deployments || [])
);