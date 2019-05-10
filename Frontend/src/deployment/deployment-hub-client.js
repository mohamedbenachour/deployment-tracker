import * as signalR from "@aspnet/signalr";

import { notification } from 'antd';
import { statusIsRunning } from './deployment-status';

import {
    newDeployment
} from './actions';

const createConnection = (store) => {
    const connection = new signalR.HubConnectionBuilder().withUrl("/deploymentHub").build();

    connection.on('DeploymentChange', (deployment) => {
        store.dispatch(newDeployment(deployment));
        notifyUser(deployment);
    });

    return connection;
};

const notifyUser = ({ status, modifiedBy: { name }}) => {
    const message = statusIsRunning(status) ? 'New Deployment' : 'Torndown Deployment';

    notification.info({
        message,
        description: `By ${name}`,
    });
};

class DeploymentHubClient {
    constructor(store) {
        this._store = store;
        this._connection = createConnection(store);
    }

    start() {
        this._connection.start();
    }
}

export default DeploymentHubClient;