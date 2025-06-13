const userLocale = navigator.language || 'en-US';

export const convertDateToLocale = (date: Date | string): string => {
    if (typeof date === 'string') {
        date = new Date(date);
    }
    const formatted = date.toLocaleDateString(userLocale, {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
    });
    return formatted;
}