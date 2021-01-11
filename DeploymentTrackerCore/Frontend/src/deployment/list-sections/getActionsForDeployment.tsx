import React from 'react';
import { DeleteOutlined, StopTwoTone, SyncOutlined } from '@ant-design/icons';
import { Divider } from 'antd';
import { statusIsRunning } from '../deployment-status';
import { Deployment } from '../deployment-definition';
import MoreActionsDropdown from './more-actions-dropdown';

interface ActionsProps {
    deployment: Deployment;
    teardownDeployment: (parameters: unknown) => void;
}

const getActionsForDeployment = ({
    deployment,
    teardownDeployment,
}: ActionsProps): Array<JSX.Element> => {
    const {
        status,
        teardownUrl,
        managementUrls: { deploymentTriggerUrl },
    } = deployment;

    if (statusIsRunning(status)) {
        const actionsArray = [
            <a title="Teardown" href={teardownUrl} target="_blank">
                <StopTwoTone twoToneColor="#ff0000" />
            </a>,
            <MoreActionsDropdown
              deployment={deployment}
              markAsTorndown={teardownDeployment}
            />,
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
