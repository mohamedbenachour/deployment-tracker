import {
    List, Typography, Button, Icon,
} from 'antd';
import { PlusOutlined } from '@ant-design/icons';

import NewEnvironmentModal from './connected-new-environment-modal';

const renderEnvironmentItem = (environment) => (
    <List.Item actions={[]}>
        <List.Item.Meta
          title={environment.name}
        />
    </List.Item>
);

const EnvironmentList = ({ environments, addEnvironment, isLoading }) => (
    <>
        <NewEnvironmentModal />
        <List
          header={<Button onClick={addEnvironment} type="primary" shape="circle" icon={<PlusOutlined />} size="small" />}
          bordered
          dataSource={environments}
          loading={isLoading}
          renderItem={renderEnvironmentItem}
        />
    </>
);

export default EnvironmentList;
