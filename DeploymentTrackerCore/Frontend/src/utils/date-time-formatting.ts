import { DateTime, Settings } from 'luxon';

// eslint-disable-next-line import/prefer-default-export
export const FormatAsLocalDateTimeString = (
    serverTimestamp: string,
): string => {
    const serverDateTime = DateTime.fromISO(serverTimestamp, {
        zone: 'UTC',
    });

    const localDateTime = serverDateTime.setZone(Settings.defaultZone);

    return `${localDateTime.toLocaleString(
        DateTime.DATE_FULL,
    )} at ${localDateTime.toLocaleString(DateTime.TIME_SIMPLE)}`;
};
