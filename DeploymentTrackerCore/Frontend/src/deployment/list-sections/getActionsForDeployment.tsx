import React from 'react';
import { DeleteOutlined, StopTwoTone, SyncOutlined } from '@ant-design/icons';
import { Divider } from 'antd';
import { statusIsRunning } from '../deployment-status';
import { Deployment } from '../deployment-definition';

interface ActionsProps {
  deployment: Deployment;
  teardownDeployment: (parameters: unknown) => void;
}

const getActionsForDeployment = ({
    deployment: {
        status,
        siteName,
        teardownUrl,
        managementUrls: { deploymentTriggerUrl },
    },
    teardownDeployment,
}: ActionsProps): Array<JSX.Element> => {
    if (statusIsRunning(status)) {
        const actionsArray = [
            <DeleteOutlined
              title="Mark as torndown"
              onClick={() => teardownDeployment({ siteName })}
            />,
            <a title="Teardown" href={teardownUrl} target="_blank">
                <StopTwoTone twoToneColor="#ff0000" />
            </a>,
        ];

        if (deploymentTriggerUrl !== null) {
            actionsArray.splice(
                0,
                0,
                <a title="Redeploy" href={deploymentTriggerUrl} target="_blank">
                    <SyncOutlined twoToneColor="#0000ff" />
                </a>,
                <Divider type="vertical" />,
            );
        }

        return actionsArray;
    }

    return [];
};

export default getActionsForDeployment;
