import React from 'react';

import { Tag } from 'antd';

import { InProgress, Completed } from './status';

const getJiraStatusColour = (status) => {
    if (status === Completed) {
        return 'green';
    }
    if (status === InProgress) {
        return 'gold';
    }

    return 'grey';
};

const JiraStatusBadge = ({ status }) => (
    <React.Fragment>
        {status &&
        <Tag
            color={getJiraStatusColour(status)}
            >
            {status}
        </Tag>
        }
    </React.Fragment>
);

export default JiraStatusBadge;