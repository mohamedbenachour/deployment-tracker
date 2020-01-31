import { notification } from 'antd';

import {
    deploymentBeingSaved,
    deploymentSaveFailed,
    newDeployment,
} from './actions';

import { postJSON } from '../utils/io';

export const addDeployment = () => (dispatch, getState) => {
    dispatch(deploymentBeingSaved());

    const deployment = getState().deployment.deploymentBeingAdded;
    postJSON('/api/deployment', deployment, (addedDeployment) => {
        dispatch(newDeployment(addedDeployment));
    }, (error) => {
        const message = error.title || error;

        dispatch(deploymentSaveFailed());

        notification.error({
            message: 'Failed to add deployment',
            description: message,
        });
    });
};

export const teardownDeployment = ({ siteName }) => () => {
    postJSON('/api/deployment/destroyed', { siteName }, () => {
    }, (error) => {
        const message = error.title || error;

        notification.error({
            message: 'Failed to teardown deployment',
            description: message,
        });
    });
};
