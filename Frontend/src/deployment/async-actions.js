import { notification } from 'antd';

import {
    deploymentBeingSaved,
    deploymentSaveFailed,
    newDeployment
} from './actions';

import { postJSON } from './../utils/io';

export const addDeployment = () => {
    return (dispatch, getState) => {
        dispatch(deploymentBeingSaved());

        const deployment = getState().deployment.deploymentBeingAdded;
        postJSON('/api/deployment', deployment, (addedDeployment) => {
            dispatch(newDeployment(addedDeployment));
        }, (error) => {
            let message = error.title || error;

            dispatch(deploymentSaveFailed());

            notification.error({
                message: 'Failed to add deployment',
                description: message,
            });
        });
    };
};