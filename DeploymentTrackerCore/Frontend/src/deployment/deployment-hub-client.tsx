import * as signalR from '@microsoft/signalr';

import { notification } from 'antd';
import { Store } from 'redux';
import { statusIsRunning } from './deployment-status';

import { newDeployment } from './actions';
import Notifier from '../app/notifier';
import { Deployment } from './deployment-definition';

const notifyUser = (
    {
        branchName, publicURL, status, modifiedBy: { name },
    }: Deployment,
    notifier: Notifier,
) => {
    const message = statusIsRunning(status)
        ? 'New Deployment'
        : 'Torndown Deployment';

    notification.info({
        message,
        description: (
            <div>
                <div>{branchName}</div>
                <div>{`By ${name}`}</div>
                <a href={publicURL} target="_blank" rel="noreferrer">
                    Visit
                </a>
            </div>
        ),
    });

    notifier.notify(message, branchName);
};

const createConnection = (store: Store, notifier: Notifier) => {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl('/deploymentHub')
        .build();

    connection.on('DeploymentChange', (deployment) => {
        store.dispatch(newDeployment(deployment));
        notifyUser(deployment, notifier);
    });

    return connection;
};

class DeploymentHubClient {
    private connection: signalR.HubConnection;

    constructor(store: Store, notifier: Notifier) {
        this.connection = createConnection(store, notifier);
    }

    start(): Promise<void> {
        return this.connection.start();
    }
}

export default DeploymentHubClient;
