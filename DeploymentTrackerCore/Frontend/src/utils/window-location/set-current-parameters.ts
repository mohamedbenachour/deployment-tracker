import { getHistory } from '../../app/application-history';

const SetCurrentParameters = (searchParams: URLSearchParams): void => {
    const currentHistory = getHistory();
    const searchString = searchParams.toString();

    currentHistory.push({
        search: searchString.length !== 0 ? `?${searchString}` : '',
    });
};

export default SetCurrentParameters;
