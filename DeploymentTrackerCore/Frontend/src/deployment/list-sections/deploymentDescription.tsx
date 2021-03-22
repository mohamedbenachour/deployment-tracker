import { Typography } from 'antd';
import { FormatAsLocalDateTimeString } from '../../utils/date-time-formatting';
import { Deployment, SiteLogin } from '../deployment-definition';
import LoginDetail from './loginDetail';
import NoteIndicator from '../notes/note-indicator';
import { statusIsRunning } from '../deployment-status';

interface DeploymentDescriptionProps {
    deployment: Deployment;
}

const renderLoginDetail = (login: SiteLogin): JSX.Element => (
    <LoginDetail login={login} />
);

const getActualName = (name: string, userName: string): string | null => {
    if (name && name.length > 0) {
        return name;
    }

    if (userName && userName.length > 0) {
        return `(${userName})`;
    }

    return null;
};

const DeploymentDescription = ({
    deployment: {
        status,
        modifiedBy: { name, userName, timestamp },
        siteLogin,
        id,
        hasNotes,
    },
}: DeploymentDescriptionProps): JSX.Element => {
    const actualName = getActualName(name, userName);
    const deploymentText = statusIsRunning(status) ? 'Deployed' : 'Torndown';
    const actualDeploymentText = actualName
        ? `${deploymentText} by`
        : deploymentText;

    return (
        <>
            <Typography.Text>{`${actualDeploymentText} `}</Typography.Text>
            {actualName && <Typography.Text strong>{actualName}</Typography.Text>}
            <Typography.Text>
                {` on ${FormatAsLocalDateTimeString(timestamp)}`}
            </Typography.Text>
            {siteLogin && renderLoginDetail(siteLogin)}
            <NoteIndicator deploymentId={id} hasNotes={hasNotes} />
        </>
    );
};

export default DeploymentDescription;
