import URLBuilder from './url-builder';

const getBaseUrl = (): URLBuilder => new URLBuilder('');
const getBaseApiUrl = (): URLBuilder => getBaseUrl().appendPath('api');

const getDeploymentApiUrl = (): URLBuilder => getBaseApiUrl().appendPath('deployment');

const getMentionsApiUrl = (): URLBuilder => getBaseApiUrl().appendPath('mention');

export {
    getBaseUrl, getBaseApiUrl, getDeploymentApiUrl, getMentionsApiUrl,
};
