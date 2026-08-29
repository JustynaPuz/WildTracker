import client from './client';
export const usersApi = {
    getAll: () => client.get('/users').then((r) => r.data.items),
    getById: (id) => client.get(`/users/${id}`).then((r) => r.data),
    changeRole: (id, request) => client.put(`/users/${id}/role`, request).then((r) => r.data),
    activate: (id) => client.post(`/users/${id}/activate`).then((r) => r.data),
    deactivate: (id) => client.post(`/users/${id}/deactivate`).then((r) => r.data),
};
