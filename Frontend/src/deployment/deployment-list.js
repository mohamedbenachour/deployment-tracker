import React from 'react';
import { List, Typography, Button, Icon, Input, Popover, Radio, notification, Tag, Select } from 'antd';

import { statusIsRunning } from './deployment-status';

import { FormatAsLocalDateTimeString } from '../utils/date-time-formatting';

import NewDeploymentModal from './connected-new-deployment-modal';

import JiraUrl from '../jira/jira-url';
import JiraStatusBadge from '../jira/jira-status-badge';

import { getPageData } from '../utils/page-data';

const renderStatus = (status) => {
    if (statusIsRunning(status)) {
        return <Icon type="check-circle" theme="twoTone" twoToneColor="#52c41a" />;
    }

    return <Icon type="close-circle" theme="filled" />;
};

const renderJiraDetail = ({ url, status }) => (
    <React.Fragment>
        <JiraUrl url={url} style={{ marginLeft: 10 }} />
        <JiraStatusBadge status={status} />
    </React.Fragment>
);

const renderType = ({ name }) => (
    <Tag style={{ marginLeft: 10 }} color="blue">{name}</Tag>
);

const renderTitle = ({ branchName, status, publicURL, jira, type }) => {
    if (statusIsRunning(status)) {
        return (
            <React.Fragment>
                <a href={publicURL} target="_blank">{`${branchName}`}
                    <Icon type="select" style={{ marginLeft: 10 }} />
                </a>
                {jira && renderJiraDetail(jira)}
                {renderType(type)}
            </React.Fragment>
        );
    }

    return branchName;
};

const copyValue = (value) => {
    navigator.clipboard.writeText(value).then(() => notification.info({
        message: 'Copied to clipboard'
    }));
};

const renderLoginContent = (fieldName, value, allowCopy = false) => {
    const labelStyle = {
        paddingRight: 5,
        userSelect: 'none',
        '-moz-user-select': 'none',
        '-webkit-user-select': 'none'
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
        {allowCopy &&
            <Button
                icon='copy'
                onClick={() => copyValue(value)} style={{ marginLeft: 10 }}
                title='Copy to clipboard'
            />
        }
    </div>
    );
};

const renderLoginDetail = ({ userName, password }) => {
    return (
        <Popover
            content={(
            <React.Fragment>
                {renderLoginContent('Username', userName)}
                {renderLoginContent('Password', password, true)}
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
            {siteLogin && renderLoginDetail(siteLogin)}
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

const renderTypeOptions = (types) => types.map(({ id, name }) => (
    <Select.Option key={id} value={id}>
        {name}
    </Select.Option>
));

const renderTypeFilter = (typeFilter, types, onChange) => (
    <Select
        value={typeFilter}
        onChange={onChange}
        style={{ width: 120, marginLeft: 10 }}
        >
        {renderTypeOptions(types)}
    </Select>
);

const renderStatusFilter = (statusFilter, onStatusFilterChange) => (
    <Radio.Group onChange={onStatusFilterChange} value={statusFilter}>
        <Radio.Button value="running">Running</Radio.Button>
        <Radio.Button value="completed">Completed</Radio.Button>
        <Radio.Button value="torndown">Torndown</Radio.Button>
    </Radio.Group>
);

const renderHeader = (
    branchNameFilter,
    addDeployment,
    onSearch,
    statusFilter,
    onStatusFilterChange,
    typeFilter,
    types,
    onTypeFilterChange
    ) => (
    <React.Fragment>
        {renderAddDeploymentButton(addDeployment)}
        <Input.Search
            placeholder="Search by branch name"
            onChange={({ target: { value }}) => onSearch(value)}
            style={{ width: 200, marginRight: 10 }}
            value={branchNameFilter}
            />
        {renderStatusFilter(statusFilter, ({ target: { value }}) => onStatusFilterChange(value))}
        {renderTypeFilter(typeFilter, types, onTypeFilterChange)}
    </React.Fragment>
);

const DeploymentList = ({ 
    deployments,
    isLoading,
    addDeployment,
    branchNameFilter,
    onSearch,
    statusFilter,
    onStatusFilterChange,
    typeFilter,
    types,
    onTypeFilterChange
}) => (
    <React.Fragment>
        <NewDeploymentModal />
        <List
            header={renderHeader(
                branchNameFilter,
                addDeployment,
                onSearch,
                statusFilter,
                onStatusFilterChange,
                typeFilter,
                types,
                onTypeFilterChange
                )}
            bordered
            dataSource={deployments}
            loading={isLoading}
            renderItem={renderDeploymentItem}
            pagination={{ pageSize: 10 }}
            />
    </React.Fragment>
);

export default DeploymentList;