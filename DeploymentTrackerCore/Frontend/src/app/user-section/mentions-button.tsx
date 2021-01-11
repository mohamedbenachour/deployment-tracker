import { LoadingOutlined, MessageOutlined } from '@ant-design/icons';
import {
    Badge, Button, Dropdown, Popover,
} from 'antd';
import { observer } from 'mobx-react-lite';
import React, { useState } from 'react';
import ConnectedMentionsMenu from './connected-mentions-menu';
import MentionStore from './mention-store';

const MentionsButton = (): JSX.Element => {
    const [mentionStore] = useState(new MentionStore());

    if (mentionStore.isLoading) {
        return (
            <Button>
                <Badge size="small" count={<LoadingOutlined />}>
                    <MessageOutlined />
                </Badge>
            </Button>
        );
    }

    const loadedMentions = mentionStore.mentions;

    return (
        <Popover
          placement="bottom"
          content={<ConnectedMentionsMenu mentionStore={mentionStore} />}
        >
            <Button>
                <Badge size="small" count={loadedMentions.length}>
                    <MessageOutlined />
                </Badge>
            </Button>
        </Popover>
    );
};

export default observer(MentionsButton);
