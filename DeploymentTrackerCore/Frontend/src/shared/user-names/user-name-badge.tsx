import { Avatar } from 'antd';
import React, { forwardRef, Ref } from 'react';
import withStyles from 'react-jss';
import nameToShortForm from './name-to-short-form';

interface UserNameBadgeProps {
  name: string;
}

const UserNameBadge = (
    { name }: UserNameBadgeProps,
    ref: Ref<HTMLElement>,
): JSX.Element => (
    <Avatar ref={ref} size={20}>
        {nameToShortForm(name)}
    </Avatar>
);

export default forwardRef(UserNameBadge);
