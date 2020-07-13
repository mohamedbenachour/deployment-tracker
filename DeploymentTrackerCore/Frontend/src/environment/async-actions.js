import { notification } from 'antd';
import {
    environmentListIsLoading,
    environmentListLoaded,

    environmentBeingSaved,
    environmentSaveFailed,
    newEnvironment,
} from './actions';

import { getJSON, postJSON } from '../utils/io';

export const loadEnvironmentList = () => (dispatch) => {
    dispatch(environmentListIsLoading());

    getJSON('/api/environment', (environments) => {
        dispatch(environmentListLoaded(environments));
    });
};

export const addEnvironment = () => (dispatch, getState) => {
    dispatch(environmentBeingSaved());

    const environment = getState().environment.environmentBeingAdded;
    postJSON('/api/environment', environment, (addedEnvironment) => {
        dispatch(newEnvironment(addedEnvironment));
    }, (error) => {
        const message = error.title || error;

        dispatch(environmentSaveFailed());

        notification.error({
            message: 'Failed to add deployment',
            description: message,
        });
    });
};
