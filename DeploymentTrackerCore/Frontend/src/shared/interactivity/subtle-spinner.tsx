import { Spin } from 'antd';
import { LoadingOutlined } from '@ant-design/icons';

const SubtleSpinner = (): JSX.Element => (
    <Spin indicator={<LoadingOutlined spin />} />
);

export default SubtleSpinner;
