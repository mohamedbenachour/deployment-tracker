import getCurrentParameters from '../utils/window-location/get-current-parameters';
import createDefaultState from './default-state';

const getStatusInUrl = (): string => {
    const validStatuses = ['running', 'completed', 'torndown'];
    const statusInSearch = (
        getCurrentParameters().get('status') ?? ''
    ).toLowerCase();

    return validStatuses.includes(statusInSearch)
        ? statusInSearch
        : createDefaultState().filters.status;
};

export default getStatusInUrl;
