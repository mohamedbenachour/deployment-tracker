import React from 'react';

import * as signalR from "@aspnet/signalr";

import { notification } from 'antd';
import { statusIsRunning } from './deployment-status';

import {
    newDeployment
} from './actions';

const createConnection = (store, notifier) => {
    const connection = new signalR.HubConnectionBuilder().withUrl("/deploymentHub").build();

    connection.on('DeploymentChange', (deployment) => {
        store.dispatch(newDeployment(deployment));
        notifyUser(deployment, notifier);
    });

    return connection;
};

const notifyUser = ({ branchName, publicURL, status, modifiedBy: { name }}, notifier) => {
    const message = statusIsRunning(status) ? 'New Deployment' : 'Torndown Deployment';

    notification.info({
        message,
        description: (
            <div>
                <div>{branchName}</div>
                <div>{`By ${name}`}</div>
                <a href={publicURL} target="_blank">Visit</a>
            </div>
        ),
    });

    notifier.notify(message, branchName);
};

class DeploymentHubClient {
    constructor(store, notifier) {
        this._store = store;
        this._notifier = notifier;
        this._connection = createConnection(store, notifier);
    }

    start() {
        this._connection.start();
    }
}

export default DeploymentHubClient;