import { createSelector } from 'reselect';

export const getLoading = ({ environment: { environments: { loading } } }) => loading;

export const getEnvironments = ({ environment: { environments: { data } } }) => data || [];

export const getDeployments = createSelector(
    [getEnvironments],
    (environments) => environments.flatMap((environment) => environment.deployments || []),
);

export const getTypes = createSelector(
    [getDeployments],
    (deployments) => {
        const resultKeys = {};

        deployments.forEach(({ type }) => {
            resultKeys[type.name] = type;
        });

        return Object.values(resultKeys);
    },
);
