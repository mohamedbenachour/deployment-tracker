import { History, createHashHistory } from 'history';

const getHistory = (): History => createHashHistory();

export {
    // eslint-disable-next-line import/prefer-default-export
    getHistory,
};
