import client from './client';
export const reportsApi = {
    search: (params) => client.get('/reports', { params }).then((r) => r.data),
    getById: (id) => client.get(`/reports/${id}`).then((r) => r.data),
    create: (request) => client.post('/reports', request).then((r) => r.data),
    update: (id, request) => client.put(`/reports/${id}`, request).then((r) => r.data),
    approve: (id) => client.post(`/reports/${id}/approve`),
    reject: (id) => client.post(`/reports/${id}/reject`),
    resolve: (id) => client.post(`/reports/${id}/resolve`).then((r) => r.data),
    delete: (id) => client.delete(`/reports/${id}`),
};
