import { Deployment } from '../../deployment/deployment-definition';
import { Mention } from './mention-definitions';

const getDeploymentForMention = (
    deployments: Deployment[],
    mention: Mention,
): Deployment | null => {
    const deploymentId = Number(mention.referencedEntity.split('::')[1]);

    return (
        deployments.find((deployment) => deployment.id === deploymentId) ?? null
    );
};

export default getDeploymentForMention;
