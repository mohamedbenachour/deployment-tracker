import { Filters } from './default-state';
import SetCurrentParameters from '../utils/window-location/set-current-parameters';

const updateWindowLocation = (filters: Filters): void => {
    const urlSearch = new URLSearchParams();

    urlSearch.append('status', filters.status);
    urlSearch.append('onlyMine', `${filters.onlyMine}`);

    if (filters.branchName.trim().length > 0) {
        urlSearch.append('branchName', `${filters.branchName}`);
    }

    SetCurrentParameters(urlSearch);
};

export default updateWindowLocation;
