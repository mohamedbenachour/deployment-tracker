import createDefaultState, { Filters, DeploymentState } from './default-state';
import getStatusInUrl from './get-status-in-url';
import getOnlyMineInUrl from './filters/get-only-mine-in-url';

const createFiltersState = (): Filters => {
    const { filters: defaultFilters } = createDefaultState();

    defaultFilters.status = getStatusInUrl();
    defaultFilters.onlyMine = getOnlyMineInUrl();

    return defaultFilters;
};

const createInitialState = (): DeploymentState => {
    const defaultState = createDefaultState();

    defaultState.filters = createFiltersState();

    return defaultState;
};

export default createInitialState;
