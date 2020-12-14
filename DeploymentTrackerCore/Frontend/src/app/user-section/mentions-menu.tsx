import { Menu } from 'antd';
import React from 'react';
import { Deployment } from '../../deployment/deployment-definition';
import getDeploymentForMention from './get-deployment-for-mention';
import { Mention } from './mention-definitions';

interface MentionsMenuProps {
  mentions: Mention[];
  deployments: Deployment[];
}

const getMentionText = (
    mention: Mention,
    deployments: Deployment[],
): string | null => {
    const mentionedDeployment = getDeploymentForMention(deployments, mention);

    if (mentionedDeployment !== null) {
        return `${mention.createdBy.name} mentioned you on ${mentionedDeployment.branchName}`;
    }

    return null;
};

const getMentionsItems = (
    mentions: Mention[],
    deployments: Deployment[],
): JSX.Element | JSX.Element[] => {
    if (mentions.length === 0) {
        return <Menu.Item>No Mentions</Menu.Item>;
    }

    return mentions
        .map((mention) => getMentionText(mention, deployments))
        .filter((mentionText) => mentionText !== null)
        .map((mentionText) => <Menu.Item>{mentionText}</Menu.Item>);
};

const MentionsMenu = ({
    mentions,
    deployments,
}: MentionsMenuProps): JSX.Element => (
    <Menu>{getMentionsItems(mentions, deployments)}</Menu>
);

export default MentionsMenu;
