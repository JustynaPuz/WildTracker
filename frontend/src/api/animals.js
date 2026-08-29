import client from './client';
export const animalsApi = {
    getAll: () => client.get('/animals').then((r) => r.data.items),
    search: (params) => client.get('/animals/search', { params }).then((r) => r.data),
    getById: (id) => client.get(`/animals/${id}`).then((r) => r.data),
    create: (request) => client.post('/animals', request).then((r) => r.data),
    update: (id, request) => client.put(`/animals/${id}`, request).then((r) => r.data),
    delete: (id) => client.delete(`/animals/${id}`),
    getMovement: (id, limit = 50) => client.get(`/animals/${id}/movement`, { params: { limit } })
        .then((r) => r.data.items),
};
