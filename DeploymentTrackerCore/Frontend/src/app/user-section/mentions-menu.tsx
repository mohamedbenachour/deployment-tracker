import { ConfigProvider, List } from 'antd';
import React from 'react';
import { Deployment } from '../../deployment/deployment-definition';
import getMentionItem from './get-mention-item';
import { Mention } from './mention-definitions';
import MentionStore from './mention-store';

interface MentionsMenuProps {
    mentionStore: MentionStore;
    deployments: Deployment[];
}

const MentionsMenu = ({
    mentionStore,
    deployments,
}: MentionsMenuProps): JSX.Element => (
    <ConfigProvider renderEmpty={() => 'No mentions'}>
        <List<Mention>
          size="small"
          dataSource={mentionStore.mentions}
          bordered
          renderItem={(mention) => getMentionItem({
                mention,
                currentDeployments: deployments,
                onMentionAcknowledged: () => mentionStore.acknowledge(mention.id),
            })}
        />
    </ConfigProvider>
);

export default MentionsMenu;
