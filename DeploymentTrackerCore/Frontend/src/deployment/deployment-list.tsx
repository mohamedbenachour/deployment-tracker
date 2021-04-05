import {
    List,
    Typography,
    Button,
    Input,
    Radio,
    Tag,
    Select,
    Checkbox,
} from 'antd';
import { PlusOutlined, SelectOutlined } from '@ant-design/icons';

import { statusIsRunning } from './deployment-status';

import NewDeploymentModal from './connected-new-deployment-modal';

import JiraUrl from '../jira/jira-url';
import JiraStatusBadge from '../jira/jira-status-badge';

import { getPageData } from '../utils/page-data';

import getActionsForDeployment from './list-sections/getActionsForDeployment';

import {
    Deployment,
    DeploymentType,
    JiraDetail,
} from './deployment-definition';
import DeploymentDescription from './list-sections/deployment-description';

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
                <a href={publicURL} target="_blank" rel="noreferrer">
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
          description={<DeploymentDescription deployment={deployment} />}
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
