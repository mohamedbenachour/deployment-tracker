import React from 'react';

import { Button } from 'antd';

const JiraUrl = ({ url, style }) => (
    <React.Fragment>
        {url && 
            <a href={url} target="_blank" style={style}>
                <Button
                    size="small"
                    type="link"
                >
                    {'Jira'}
                </Button>
            </a>
        }
    </React.Fragment>
);

export default JiraUrl;