import React from 'react';
import { List, Typography, Button, Icon } from 'antd';

import NewDeploymentModal from './connected-new-deployment-modal';

const renderDeploymentItem = (deployment) => (
    <List.Item actions={[<a href={deployment.publicURL}><Icon type="link" /></a>, <Icon type="delete" />]}>
        <List.Item.Meta 
            title={deployment.branchName}
        />
    </List.Item>
);

const DeploymentList = ({ deployments, isLoading, addDeployment }) => (
    <React.Fragment>
        <NewDeploymentModal />
        <List
            header={<Button onClick={addDeployment} type="primary" shape="circle" icon="plus" size="small"></Button>}
            bordered
            dataSource={deployments}
            loading={isLoading}
            renderItem={renderDeploymentItem}
            />
    </React.Fragment>
);

export default DeploymentList;