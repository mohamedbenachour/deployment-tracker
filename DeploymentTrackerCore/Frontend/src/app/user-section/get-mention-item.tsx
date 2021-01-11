import React from 'react';

import {
    Button, List, Space, Typography,
} from 'antd';
import { CloseOutlined } from '@ant-design/icons';
import { Deployment } from '../../deployment/deployment-definition';
import getDeploymentForMention from './get-deployment-for-mention';
import { Mention } from './mention-definitions';

interface MentionItemProps {
  mention: Mention;
  currentDeployments: Deployment[];
  onMentionAcknowledged: () => void;
}

const getMentionText = (
    mention: Mention,
    deployments: Deployment[],
): JSX.Element | null => {
    const mentionedDeployment = getDeploymentForMention(deployments, mention);

    if (mentionedDeployment !== null) {
        return (
            <Space direction="horizontal">
                <Typography.Text>{`${mention.createdBy.name} mentioned you on`}</Typography.Text>
                <Typography.Text strong>{mentionedDeployment.siteName}</Typography.Text>
            </Space>
        );
    }

    return null;
};

const getMentionItem = ({
    mention,
    currentDeployments,
    onMentionAcknowledged,
}: MentionItemProps) => (
    <List.Item
      key={mention.id}
      actions={[
            <Button
            onClick={onMentionAcknowledged}
            size="small"
            shape="circle"
            danger
            icon={<CloseOutlined />}
          />,
        ]}
    >
        {getMentionText(mention, currentDeployments)}
    </List.Item>
);

export default getMentionItem;
