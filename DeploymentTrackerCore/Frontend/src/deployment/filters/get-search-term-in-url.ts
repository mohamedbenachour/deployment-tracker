import getCurrentParameters from '../../utils/window-location/get-current-parameters';

const getSearchTermInUrl = (): string => {
    const searchTerm = getCurrentParameters().get('branchName') ?? '';

    return searchTerm.trim();
};

export default getSearchTermInUrl;
