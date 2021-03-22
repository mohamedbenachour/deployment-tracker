import React from 'react';

import {
    Modal, Input, Select, Typography,
} from 'antd';
import { Environment } from '../environment/environment-definition';
import { Deployment } from './deployment-definition';

const renderEnvironmentOptions = (environments: Environment[]): JSX.Element[] => environments.map((environment) => (
    <Select.Option key={environment.id} value={environment.id}>
        {environment.name}
    </Select.Option>
));

const renderEnvironmentSelect = (
    environments: Environment[],
    selectedEnvironmentId: number,
    onEnvironmentChange: (id: number) => void,
): JSX.Element => (
    <Select
      style={{ width: '100%' }}
      value={selectedEnvironmentId}
      onChange={(value) => onEnvironmentChange(value)}
    >
        {renderEnvironmentOptions(environments)}
    </Select>
);

interface NewDeploymentModalProps {
    visible: boolean;
    saveInProgress: boolean;
    onOk: () => void;
    onCancel: () => void;
    onBranchNameChange: (name: string) => void;
    onSiteNameChange: (name: string) => void;
    onPublicUrlChange: (name: string) => void;
    onEnvironmentChange: (id: number) => void;
    environments: Environment[];
    deploymentBeingAdded: Deployment;
}

const NewDeploymentModal = ({
    visible,
    saveInProgress,
    onOk,
    onCancel,
    onBranchNameChange,
    onSiteNameChange,
    onPublicUrlChange,
    onEnvironmentChange,
    environments,
    deploymentBeingAdded: {
        branchName, siteName, publicURL, environmentId,
    },
}: NewDeploymentModalProps) => (
    <Modal
      visible={visible}
      onOk={onOk}
      onCancel={onCancel}
      title="Add New Deployment"
      confirmLoading={saveInProgress}
    >
        <Typography.Text>Branch Name</Typography.Text>
        <Input
          value={branchName}
          onChange={({ target: { value } }) => onBranchNameChange(value)}
        />
        <Typography.Text>Site Name</Typography.Text>
        <Input
          value={siteName}
          onChange={({ target: { value } }) => onSiteNameChange(value)}
        />
        <Typography.Text>Public URL</Typography.Text>
        <Input
          value={publicURL}
          onChange={({ target: { value } }) => onPublicUrlChange(value)}
        />
        <Typography.Text>Environment</Typography.Text>
        {renderEnvironmentSelect(environments, environmentId, onEnvironmentChange)}
    </Modal>
);

export default NewDeploymentModal;
