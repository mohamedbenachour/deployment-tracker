import { LoadingOutlined, MessageOutlined } from '@ant-design/icons';
import { Badge, Button, Dropdown } from 'antd';
import React, { useState } from 'react';
import { getJSONPromise } from '../../utils/io';
import ConnectedMentionsMenu from './connected-mentions-menu';
import { Mention } from './mention-definitions';
import MentionsMenu from './mentions-menu';

const getMentions = (): Mention[] | null => {
    const [isLoading, setIsLoading] = useState(true);
    const [mentions, setMentions] = useState<Mention[]>([]);
    useState<Promise<Mention[] | null>>(() => {
        const loadingPromise = getJSONPromise<Mention[]>('/api/mention');

        loadingPromise.then((loadedMentions) => {
            setIsLoading(false);

            if (loadedMentions !== null) {
                setMentions(loadedMentions);
            }
        });

        return loadingPromise;
    });

    if (isLoading) {
        return null;
    }

    return mentions;
};

const MentionsButton = (): JSX.Element => {
    const mentions = getMentions();
    const isLoading = mentions === null;

    if (isLoading) {
        return (
            <Button>
                <Badge size="small" count={<LoadingOutlined />}>
                    <MessageOutlined />
                </Badge>
            </Button>
        );
    }

    const loadedMentions = mentions ?? [];

    return (
        <Dropdown overlay={<ConnectedMentionsMenu mentions={loadedMentions} />}>
            <Button>
                <Badge size="small" count={loadedMentions.length}>
                    <MessageOutlined />
                </Badge>
            </Button>
        </Dropdown>
    );
};

export default MentionsButton;
