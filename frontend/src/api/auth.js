import client from './client';
export const authApi = {
    login: (request) => client.post('/auth/login', request).then((r) => r.data),
    register: (request) => client.post('/auth/register', request).then((r) => r.data),
};
