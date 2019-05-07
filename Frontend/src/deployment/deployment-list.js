import React from 'react';
import { List, Typography, Button, Icon, Input, Checkbox } from 'antd';

import { statusIsRunning } from './deployment-status';

import NewDeploymentModal from './connected-new-deployment-modal';

const renderStatus = (status) => {
    if (statusIsRunning(status)) {
        return <Icon type="check-circle" theme="twoTone" twoToneColor="#52c41a" />;
    }

    return <Icon type="close-circle" theme="filled" />;
};

const renderTitle = ({ branchName, status, publicURL }) => {
    if (statusIsRunning(status)) {
        return <a href={publicURL}>{`${branchName} - [Visit]`}</a>;
    }

    return branchName;
};

const renderDescription = ({ status, modifiedBy: { name, userName, timestamp }}) => {
    const actualName = (name && name.length > 0) ? name : `(${userName})`;
    const deploymentText = statusIsRunning(status) ? 'Deployed' : 'Torndown';
    const timestampDateText = new Date(timestamp).toDateString();
    const timestampTimeText = new Date(timestamp).toLocaleTimeString();

    return (
        <React.Fragment>
            <Typography.Text>{`${deploymentText} by: `}</Typography.Text>
            <Typography.Text strong={true}>{actualName}</Typography.Text>
            <Typography.Text>{` on ${timestampDateText} at ${timestampTimeText}`}</Typography.Text>
        </React.Fragment>
    )
};

const getActions = ({ teardownUrl, status }) => {
    if (statusIsRunning(status)) {
        return [<a href={teardownUrl}><Icon style={{ color: 'red' }} type="stop" /></a>];
    }

    return [];
};

const renderDeploymentItem = (deployment) => (
    <List.Item actions={getActions(deployment)}>
        <List.Item.Meta 
            title={renderTitle(deployment)}
            description={renderDescription(deployment)}
        />
        {renderStatus(deployment.status)}
    </List.Item>
);

const renderHeader = (branchNameFilter, addDeployment, onSearch, showDestroyed, onShowDestroyedChange) => (
    <React.Fragment>
        <Button onClick={addDeployment} type="primary" shape="circle" icon="plus" size="small" />
        <Input.Search
            placeholder="Search by branch name"
            onChange={({ target: { value }}) => onSearch(value)}
            style={{ width: 200, marginLeft: 10, marginRight: 10 }}
            value={branchNameFilter}
            />
        <Checkbox value={showDestroyed} onChange={({ target: { checked }}) => onShowDestroyedChange(checked)}>{'Show Destroyed'}</Checkbox>
    </React.Fragment>
);

const DeploymentList = ({ deployments, isLoading, addDeployment, branchNameFilter, onSearch, showDestroyed, onShowDestroyedChange }) => (
    <React.Fragment>
        <NewDeploymentModal />
        <List
            header={renderHeader(branchNameFilter, addDeployment, onSearch, showDestroyed, onShowDestroyedChange)}
            bordered
            dataSource={deployments}
            loading={isLoading}
            renderItem={renderDeploymentItem}
            />
    </React.Fragment>
);

export default DeploymentList;