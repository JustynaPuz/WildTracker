import client from './client';
export const notesApi = {
    getByReport: (reportId) => client.get(`/reports/${reportId}/notes`)
        .then((r) => r.data.items),
    getById: (reportId, noteId) => client.get(`/reports/${reportId}/notes/${noteId}`).then((r) => r.data),
    create: (reportId, request) => client.post(`/reports/${reportId}/notes`, request).then((r) => r.data),
    update: (reportId, noteId, request) => client.put(`/reports/${reportId}/notes/${noteId}`, request).then((r) => r.data),
    delete: (reportId, noteId) => client.delete(`/reports/${reportId}/notes/${noteId}`),
};
