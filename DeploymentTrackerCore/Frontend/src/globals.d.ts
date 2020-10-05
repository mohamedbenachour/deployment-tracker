/* eslint-disable no-underscore-dangle */

interface PageProperties {
  csrfToken?: string;
}

interface User {
  userName: string;
  email: string | undefined;
  name: string;
}

interface PageData {
  allowManualDeploymentsToBeAdded: boolean;
  user: User;
}

declare const _PAGE_PROPERTIES: PageProperties;
declare const _PAGE_DATA: PageData;
