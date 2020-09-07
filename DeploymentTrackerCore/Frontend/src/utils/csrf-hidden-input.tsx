import React, { FunctionComponent } from 'react';

import { getCsrfToken } from './page-properties';

const CsrfHiddenInput: FunctionComponent = () => (
    <input
      name="__RequestVerificationToken"
      type="hidden"
      value={getCsrfToken()}
    />
);

export default CsrfHiddenInput;
