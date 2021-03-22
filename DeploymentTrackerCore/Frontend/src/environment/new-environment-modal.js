import { Modal, Input, Typography } from 'antd';

const NewEnvironmentModal = ({
    visible,
    saveInProgress,
    onOk,
    onCancel,
    onNameChange,
    onHostNameChange,
    environmentBeingAdded: { name, hostName },
}) => (
    <Modal
      visible={visible}
      onOk={onOk}
      onCancel={onCancel}
      title="Add New Environment"
      confirmLoading={saveInProgress}
    >
        <Typography.Text>Name</Typography.Text>
        <Input value={name} onChange={({ target: { value } }) => onNameChange(value)} />
        <Typography.Text>Host Name</Typography.Text>
        <Input value={hostName} onChange={({ target: { value } }) => onHostNameChange(value)} />
    </Modal>
);

export default NewEnvironmentModal;
