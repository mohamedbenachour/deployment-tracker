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

declare let _PAGE_PROPERTIES: PageProperties;
declare let _PAGE_DATA: PageData;
