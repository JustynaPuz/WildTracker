import client from './client';
export const statsApi = {
    getSummary: () => client.get('/stats/summary').then((r) => r.data),
    getBySpecies: () => client.get('/stats/by-species').then((r) => r.data.items),
    getByMonth: (months = 12) => client.get('/stats/by-month', { params: { months } }).then((r) => r.data.items),
};
