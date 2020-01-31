import { APP_SECTION_CHANGED } from './action-types';

export const sectionChanged = (section) => ({
    type: APP_SECTION_CHANGED,
    section,
});
