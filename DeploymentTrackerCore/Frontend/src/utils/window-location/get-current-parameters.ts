import { getHistory } from '../../app/application-history';

const getCurrentParameters = (): URLSearchParams => {
    const parameters = new URLSearchParams(getHistory().location.search);

    return parameters;
};

export default getCurrentParameters;
