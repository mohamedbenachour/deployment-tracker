import React from 'react';
import { List, Typography, Button, Icon } from 'antd';

import NewEnvironmentModal from './connected-new-environment-modal';

const renderEnvironmentItem = (environment) => (
    <List.Item actions={[<Icon type="delete" />]}>
        <List.Item.Meta 
            title={environment.name}
        />
    </List.Item>
);

const EnvironmentList = ({ environments, addEnvironment, isLoading }) => (
    <React.Fragment>
        <NewEnvironmentModal />
        <List
            header={<Button onClick={addEnvironment} type="primary" shape="circle" icon="plus" size="small"></Button>}
            bordered
            dataSource={environments}
            loading={isLoading}
            renderItem={renderEnvironmentItem}
            />
    </React.Fragment>
);

export default EnvironmentList;