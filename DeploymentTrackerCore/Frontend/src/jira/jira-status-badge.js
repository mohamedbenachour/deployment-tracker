/**
* Copyright (C) 2019  Pramod Dematagoda <pmdematagoda@mykolab.ch>
*
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

import React from 'react';

import { Tag } from 'antd';

import { InProgress, Completed } from './status';

const STATUS_TO_TEXT = {
    COMPLETED: 'Completed',
    IN_PROGRESS: 'In Progress',
    UNKNOWN: 'Unknown',
};

const getJiraStatusColour = (status) => {
    if (status === Completed) {
        return 'green';
    }
    if (status === InProgress) {
        return 'gold';
    }

    return 'grey';
};

const getStatusText = (status) => STATUS_TO_TEXT[status];

const JiraStatusBadge = ({ status }) => (
    <>
        {status
        && (
            <Tag
                color={getJiraStatusColour(status)}
                style={{ marginRight: 0 }}
            >
                {getStatusText(status)}
            </Tag>
        )}
    </>
);

export default JiraStatusBadge;
