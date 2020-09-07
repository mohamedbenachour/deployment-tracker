import React from 'react';

import { Button } from 'antd';

const JiraUrl = ({ url, style }) => (
    <>
        {url
            && (
                <a href={url} target="_blank" style={style}>
                    <Button
                        size="small"
                        type="link"
                    >
                        Jira
                    </Button>
                </a>
            )}
    </>
);

export default JiraUrl;
