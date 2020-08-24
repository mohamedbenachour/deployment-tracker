import { getPageData } from './page-data';

const getCurrentUser = (): User => getPageData().user;

export default getCurrentUser;
