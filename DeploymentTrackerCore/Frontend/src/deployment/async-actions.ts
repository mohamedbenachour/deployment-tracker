import { notification } from 'antd';

import {
    deploymentBeingSaved,
    deploymentSaveFailed,
    newDeployment,
} from './actions';

import { postJSON } from '../utils/io';
import { Deployment } from './deployment-definition';
import { DeploymentActionTypes } from './action-types';

export const addDeployment = () => (
    dispatch: (action: DeploymentActionTypes) => void,
    getState: () => any,
): void => {
    dispatch(deploymentBeingSaved());

    // eslint-disable-next-line @typescript-eslint/no-unsafe-member-access
    const deployment = <Deployment>getState().deployment.deploymentBeingAdded;
    postJSON<Deployment, Deployment>(
        '/api/deployment',
        deployment,
        (addedDeployment) => {
            dispatch(newDeployment(addedDeployment));
        },
        (error) => {
            // eslint-disable-next-line @typescript-eslint/no-unsafe-member-access
            const message = <string>(error.title || error);

            dispatch(deploymentSaveFailed());

            notification.error({
                message: 'Failed to add deployment',
                description: message,
            });
        },
    );
};

export const teardownDeployment = ({
    siteName,
}: Deployment): (() => void) => (): void => {
    postJSON(
        '/api/deployment/destroyed',
        { siteName },
        // eslint-disable-next-line @typescript-eslint/no-empty-function
        () => {},
        (error) => {
            // eslint-disable-next-line @typescript-eslint/no-unsafe-member-access
            const message = <string>(error.title || error);

            notification.error({
                message: 'Failed to teardown deployment',
                description: message,
            });
        },
    );
};
