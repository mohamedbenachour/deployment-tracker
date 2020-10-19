import { DateTime, Settings } from 'luxon';

const parseServerTimestampToLocalDateTime = (
    serverTimestamp: string,
): DateTime => {
    const serverDateTime = DateTime.fromISO(serverTimestamp, {
        zone: 'UTC',
    });

    return serverDateTime.setZone(Settings.defaultZone);
};

// eslint-disable-next-line import/prefer-default-export
export const FormatAsLocalDateTimeString = (
    serverTimestamp: string,
): string => {
    const localDateTime = parseServerTimestampToLocalDateTime(serverTimestamp);

    return `${localDateTime.toLocaleString(
        DateTime.DATE_FULL,
    )} at ${localDateTime.toLocaleString(DateTime.TIME_SIMPLE)}`;
};
