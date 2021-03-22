import React from 'react';
import {
    List,
    Typography,
    Button,
    Input,
    Popover,
    Radio,
    notification,
    Tag,
    Select,
    Checkbox,
} from 'antd';
import { PlusOutlined, CopyOutlined, SelectOutlined } from '@ant-design/icons';

import { statusIsRunning } from './deployment-status';

import { FormatAsLocalDateTimeString } from '../utils/date-time-formatting';

import NewDeploymentModal from './connected-new-deployment-modal';

import JiraUrl from '../jira/jira-url';
import JiraStatusBadge from '../jira/jira-status-badge';

import { getPageData } from '../utils/page-data';

import getActionsForDeployment from './list-sections/getActionsForDeployment';

import NoteIndicator from './notes/note-indicator';
import {
    Deployment,
    DeploymentType,
    JiraDetail,
    SiteLogin,
} from './deployment-definition';

const renderJiraDetail = ({ url, status }: JiraDetail): JSX.Element => (
    <>
        <JiraUrl url={url} style={{ marginLeft: 10 }} />
        <JiraStatusBadge status={status} />
    </>
);

const renderType = ({ name }: DeploymentType): JSX.Element => (
    <Tag style={{ marginLeft: 10 }} color="blue">
        {name}
    </Tag>
);

const renderTitle = ({
    branchName,
    status,
    publicURL,
    jira,
    type,
}: Deployment): JSX.Element => {
    if (statusIsRunning(status)) {
        return (
            <>
                <a href={publicURL} target="_blank">
                    {`${branchName}`}
                    <SelectOutlined style={{ marginLeft: 10 }} />
                </a>
                {jira && renderJiraDetail(jira)}
                {renderType(type)}
            </>
        );
    }

    return <Typography.Text delete>{branchName}</Typography.Text>;
};

const copyValue = (value: string): void => {
    void navigator.clipboard.writeText(value).then(() => notification.info({
        message: 'Copied to clipboard',
    }));
};

const renderLoginContent = (
    fieldName: string,
    value: string,
    allowCopy = false,
): JSX.Element => {
    const labelStyle: {
        paddingRight: number;
        userSelect: 'none';
        '-moz-user-select': 'none';
        '-webkit-user-select': 'none';
    } = {
        paddingRight: 5,
        userSelect: 'none',
        '-moz-user-select': 'none',
        '-webkit-user-select': 'none',
    };
    const valueStyle = {
        backgroundColor: '#e8e8e8',
        padding: 5,
        border: '1px solid',
        borderRadius: 2,
        borderColor: '',
    };

    valueStyle.borderColor = valueStyle.backgroundColor;

    return (
        <div style={{ margin: 10 }}>
            <label>
                <Typography.Text strong style={labelStyle}>
                    {fieldName}
                </Typography.Text>
            </label>
            <Typography.Text style={valueStyle}>{value}</Typography.Text>
            {allowCopy && (
            <Button
              icon={<CopyOutlined />}
              onClick={() => copyValue(value)}
              style={{ marginLeft: 10 }}
              title="Copy to clipboard"
            />
      )}
        </div>
    );
};

const renderLoginDetail = ({ userName, password }: SiteLogin): JSX.Element => (
    <Popover
      content={(
          <>
              {renderLoginContent('Username', userName)}
              {renderLoginContent('Password', password, true)}
          </>
    )}
      trigger="click"
    >
        <Button size="small" type="link">
            Site Login
        </Button>
    </Popover>
);

const getActualName = (name: string, userName: string): string | null => {
    if (name && name.length > 0) {
        return name;
    }

    if (userName && userName.length > 0) {
        return `(${userName})`;
    }

    return null;
};

const renderDescription = ({
    status,
    modifiedBy: { name, userName, timestamp },
    siteLogin,
    id,
    hasNotes,
}: Deployment): JSX.Element => {
    const actualName = getActualName(name, userName);
    const deploymentText = statusIsRunning(status) ? 'Deployed' : 'Torndown';
    const actualDeploymentText = actualName
        ? `${deploymentText} by`
        : deploymentText;

    return (
        <>
            <Typography.Text>{`${actualDeploymentText} `}</Typography.Text>
            {actualName && <Typography.Text strong>{actualName}</Typography.Text>}
            <Typography.Text>
                {` on ${FormatAsLocalDateTimeString(timestamp)}`}
            </Typography.Text>
            {siteLogin && renderLoginDetail(siteLogin)}
            <NoteIndicator deploymentId={id} hasNotes={hasNotes} />
        </>
    );
};

const renderDeploymentItem = (
    deployment: Deployment,
    teardownDeployment: (parameters: unknown) => void,
): JSX.Element => (
    <List.Item
      actions={getActionsForDeployment({
      deployment,
      teardownDeployment,
    })}
    >
        <List.Item.Meta
          title={renderTitle(deployment)}
          description={renderDescription(deployment)}
        />
    </List.Item>
);

const renderAddDeploymentButton = (addDeployment: () => void): JSX.Element => {
    if (getPageData().allowManualDeploymentsToBeAdded) {
        return (
            <Button
              onClick={addDeployment}
              type="primary"
              shape="circle"
              icon={<PlusOutlined />}
              style={{ marginRight: 10 }}
              size="small"
            />
        );
    }

    return <></>;
};

const renderTypeOptions = (types: DeploymentType[]): JSX.Element[] => types.map(({ id, name }) => (
    <Select.Option key={id} value={id}>
        {name}
    </Select.Option>
));

const renderTypeFilter = (
    typeFilter: string,
    types: DeploymentType[],
    onChange: (type: string) => void,
): JSX.Element => (
    <Select
      value={typeFilter}
      onChange={onChange}
      style={{ width: 120, marginLeft: 10 }}
    >
        {renderTypeOptions(types)}
    </Select>
);

const renderStatusFilter = (
    statusFilter: string,
    onStatusFilterChange: (status: string) => void,
): JSX.Element => (
    <Radio.Group
      onChange={({ target: { value } }) => onStatusFilterChange(value)}
      value={statusFilter}
    >
        <Radio.Button value="running">Running</Radio.Button>
        <Radio.Button value="completed">Completed</Radio.Button>
        <Radio.Button value="torndown">Torndown</Radio.Button>
    </Radio.Group>
);

const renderOnlyMineFilter = (
    onlyMineFilter: boolean,
    onOnlyMineFilterChange: (onlyMine: boolean) => void,
): JSX.Element => (
    <Checkbox
      onChange={({ target: { checked } }) => onOnlyMineFilterChange(checked)}
      checked={onlyMineFilter}
      style={{ paddingLeft: 10 }}
    >
        Only Mine
    </Checkbox>
);

const renderHeader = (
    branchNameFilter: string,
    addDeployment: () => void,
    onSearch: (type: string) => void,
    statusFilter: string,
    onStatusFilterChange: (type: string) => void,
    typeFilter: string,
    types: DeploymentType[],
    onTypeFilterChange: (type: string) => void,
    onlyMineFilter: boolean,
    onOnlyMineFilterChange: (onlyMine: boolean) => void,
): JSX.Element => (
    <>
        {renderAddDeploymentButton(addDeployment)}
        <Input.Search
          placeholder="Search by branch name"
          onChange={({ target: { value } }) => onSearch(value)}
          style={{ width: 200, marginRight: 10 }}
          value={branchNameFilter}
        />
        {renderStatusFilter(statusFilter, onStatusFilterChange)}
        {renderTypeFilter(typeFilter, types, onTypeFilterChange)}
        {renderOnlyMineFilter(onlyMineFilter, onOnlyMineFilterChange)}
    </>
);

interface DeploymentListProps {
    deployments: Deployment[];
    isLoading: boolean;
    branchNameFilter: string;
    addDeployment: () => void;
    onSearch: (type: string) => void;
    statusFilter: string;
    onStatusFilterChange: (type: string) => void;
    typeFilter: string;
    types: DeploymentType[];
    onTypeFilterChange: (type: string) => void;
    onlyMineFilter: boolean;
    onOnlyMineFilterChange: (onlyMine: boolean) => void;
    teardownDeployment: () => void;
}

const DeploymentList = ({
    deployments,
    isLoading,
    branchNameFilter,
    addDeployment,
    onSearch,
    statusFilter,
    onStatusFilterChange,
    typeFilter,
    types,
    onTypeFilterChange,
    onlyMineFilter,
    onOnlyMineFilterChange,
    teardownDeployment,
}: DeploymentListProps): JSX.Element => (
    <>
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
        onTypeFilterChange,
        onlyMineFilter,
        onOnlyMineFilterChange,
      )}
          bordered
          dataSource={deployments}
          loading={isLoading}
          renderItem={(deployment) => renderDeploymentItem(deployment, teardownDeployment)}
          pagination={{ pageSize: 10 }}
        />
    </>
);

export default DeploymentList;
