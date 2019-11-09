import React from 'react';

import { getCsrfToken } from './page-properties';

const CsrfHiddenInput = () => (
    <input name="__RequestVerificationToken" type="hidden" value={getCsrfToken()} />
);

export default CsrfHiddenInput;