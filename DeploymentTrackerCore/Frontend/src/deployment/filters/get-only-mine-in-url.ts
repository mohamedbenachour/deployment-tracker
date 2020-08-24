import getCurrentParameters from '../../utils/window-location/get-current-parameters';

const getOnlyMineInUrl = (): boolean => {
    const onlyMineInSearch = (
        getCurrentParameters().get('onlyMine') ?? ''
    ).toLowerCase();

    return onlyMineInSearch === 'true';
};

export default getOnlyMineInUrl;
