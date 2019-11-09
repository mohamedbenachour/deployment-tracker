import * as signalR from "@aspnet/signalr";

import {
    jiraStatusUpdate
} from '../deployment/actions';

const createConnection = (store) => {
    const connection = new signalR.HubConnectionBuilder().withUrl("/jiraHub").build();

    connection.on('JiraStatusChange', (jiraIssue, newJiraStatus) => {
        store.dispatch(jiraStatusUpdate(jiraIssue, newJiraStatus));
    });

    return connection;
};

class JiraHubClient {
    constructor(store) {
        this._store = store;
        this._connection = createConnection(store);
    }

    start() {
        this._connection.start();
    }
}

export default JiraHubClient;