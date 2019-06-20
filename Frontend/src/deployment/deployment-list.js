import React from 'react';
import { List, Typography, Button, Icon, Input, Checkbox, Tag, Popover } from 'antd';

import { statusIsRunning } from './deployment-status';

import { FormatAsLocalDateTimeString } from '../utils/date-time-formatting';

import NewDeploymentModal from './connected-new-deployment-modal';

import { InProgress, Completed } from '../jira/status';

import { getPageData } from '../utils/page-data';

const renderStatus = (status) => {
    if (statusIsRunning(status)) {
        return <Icon type="check-circle" theme="twoTone" twoToneColor="#52c41a" />;
    }

    return <Icon type="close-circle" theme="filled" />;
};

const getJiraStatusColour = (status) => {
    if (status === Completed) {
        return 'green';
    }
    if (status === InProgress) {
        return 'gold';
    }

    return 'grey';
};

const renderJiraDetail = ({ url, status }) => (
            <React.Fragment>
                <a href={url} target="_blank" style={{ marginLeft: 10 }}>
                    <Button
                        size="small"
                        type="link"
                    >
                        {'Jira'}
                    </Button>
                </a>
                <Tag
                    color={getJiraStatusColour(status)}
                    >
                    {status}
                </Tag>
            </React.Fragment>
        );

const renderTitle = ({ branchName, status, publicURL, jira }) => {
    if (statusIsRunning(status)) {
        return (
            <React.Fragment>
                <a href={publicURL} target="_blank">{`${branchName}`}
                    <Icon type="select" style={{ marginLeft: 10 }} />
                </a>
                {jira && renderJiraDetail(jira)}
            </React.Fragment>
        );
    }

    return branchName;
};

const renderLoginContent = (fieldName, value, allowCopy = false) => {
    const labelStyle = {
        paddingRight: 5,
    };
    const valueStyle = {
        backgroundColor: '#e8e8e8',
        padding: 5,
        border: '1px solid',
        borderRadius: 2
    };

    valueStyle.borderColor = valueStyle.backgroundColor;

    return (
    <div style={{ margin: 10}}>
        <label>
            <Typography.Text strong style={labelStyle}>{fieldName}</Typography.Text>
        </label>
        <Typography.Text style={valueStyle}>{value}</Typography.Text>
    </div>
    );
};

const renderLoginDetail = ({ userName, password }) => {
    if (!userName) {
        return <React.Fragment />;
    }

    return (
        <Popover
            content={(
            <React.Fragment>
                {renderLoginContent('Username', userName)}
                {renderLoginContent('Password', password)}
            </React.Fragment>)}
            trigger='click'
        >
            <Button
                size="small"
                type="link"
            >
                {'Site Login'}
            </Button>
        </Popover>
    );
};

const getActualName = (name, userName) => {
    if (name && name.length > 0) {
        return name;
    }

    if (userName && userName.length > 0) {
        return `(${userName})`;
    }

    return null;
};

const renderDescription = ({ status, modifiedBy: { name, userName, timestamp }, siteLogin }) => {
    const actualName = getActualName(name, userName);
    const deploymentText = statusIsRunning(status) ? 'Deployed' : 'Torndown';
    const actualDeploymentText = actualName ? `${deploymentText} by` : deploymentText;

    return (
        <React.Fragment>
            <Typography.Text>{`${actualDeploymentText} `}</Typography.Text>
            {actualName && <Typography.Text strong={true}>{actualName}</Typography.Text>}
            <Typography.Text>{` on ${FormatAsLocalDateTimeString(timestamp)}`}</Typography.Text>
            {renderLoginDetail(siteLogin)}
        </React.Fragment>
    )
};

const getActions = ({ teardownUrl, status }) => {
    if (statusIsRunning(status)) {
        return [<a href={teardownUrl} target="_blank"><Icon style={{ color: 'red' }} type="stop" /></a>];
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

const renderAddDeploymentButton = (addDeployment) => {
    if (getPageData().allowManualDeploymentsToBeAdded) {
        return <Button
            onClick={addDeployment}
            type="primary"
            shape="circle"
            icon="plus"
            style={{ marginRight: 10 }}
            size="small" />;
    }

    return <React.Fragment />;
};

const renderHeader = (branchNameFilter, addDeployment, onSearch, showDestroyed, onShowDestroyedChange) => (
    <React.Fragment>
        {renderAddDeploymentButton(addDeployment)}
        <Input.Search
            placeholder="Search by branch name"
            onChange={({ target: { value }}) => onSearch(value)}
            style={{ width: 200, marginRight: 10 }}
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