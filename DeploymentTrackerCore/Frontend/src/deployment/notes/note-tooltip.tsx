import { Tooltip } from 'antd';
import React from 'react';
import UserActionDetail from '../../shared/definitions/user-action-detail';
import { FormatAsLocalDateTimeString } from '../../utils/date-time-formatting';

interface NoteTooltipProps {
  createdBy: UserActionDetail;
  children: JSX.Element;
}

const getTooltipText = (createdBy: UserActionDetail): string => `By ${createdBy.name} on ${FormatAsLocalDateTimeString(createdBy.timestamp)}`;

const NoteTooltip = ({
    createdBy,
    children,
}: NoteTooltipProps): JSX.Element => (
    <Tooltip title={getTooltipText(createdBy)}>{children}</Tooltip>
);

export default NoteTooltip;
