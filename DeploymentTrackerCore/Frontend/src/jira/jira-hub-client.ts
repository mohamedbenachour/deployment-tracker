import signalR from '@microsoft/signalr';
import { Store } from 'redux';

import { jiraStatusUpdate } from '../deployment/actions';

const createConnection = (store: Store) => {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl('/jiraHub')
        .build();

    connection.on('JiraStatusChange', (jiraIssue, newJiraStatus) => {
        store.dispatch(jiraStatusUpdate(jiraIssue, newJiraStatus));
    });

    return connection;
};

class JiraHubClient {
    private connection: signalR.HubConnection;

    constructor(store: Store) {
        this.connection = createConnection(store);
    }

    start(): Promise<void> {
        return this.connection.start();
    }
}

export default JiraHubClient;
