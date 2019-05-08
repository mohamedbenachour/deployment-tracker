import React from 'react';

import { Modal, Input, Select, Typography } from 'antd';

const renderEnvironmentOptions = (environments) => {
    return environments.map((environment) => (
        <Select.Option key={environment.id} value={environment.id}>{environment.name}</Select.Option>
    ));
};

const renderEnvironmentSelect = (environments, selectedEnvironmentId, onEnvironmentChange) => (
    <Select
        style={{ width: '100%' }}
        value={selectedEnvironmentId}
        onChange={(value) => onEnvironmentChange(value)}>
        {renderEnvironmentOptions(environments)}
    </Select>
);

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
    deploymentBeingAdded: { branchName, siteName, publicURL, environmentId }
}) => (
    <Modal
        visible={visible}
        onOk={onOk}
        onCancel={onCancel}
        title="Add New Deployment"
        confirmLoading={saveInProgress}
    >
        <Typography.Text>Branch Name</Typography.Text>
        <Input value={branchName} onChange={({ target: { value }}) => onBranchNameChange(value)} />
        <Typography.Text>Site Name</Typography.Text>
        <Input value={siteName} onChange={({ target: { value }}) => onSiteNameChange(value)} />
        <Typography.Text>Public URL</Typography.Text>
        <Input value={publicURL} onChange={({ target: { value }}) => onPublicUrlChange(value)} />
        <Typography.Text>Environment</Typography.Text>
        {renderEnvironmentSelect(environments, environmentId, onEnvironmentChange)}
    </Modal>
);

export default NewDeploymentModal;