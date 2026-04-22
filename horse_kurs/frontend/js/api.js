
const API_URL = 'http://localhost:28280/api';

function setAuthToken(token) {
    localStorage.setItem('token', token);
}

function getAuthToken() {
    return localStorage.getItem('token');
}

async function apiRequest(endpoint, method = 'GET', body = null) {
    const headers = { 'Content-Type': 'application/json' };
    const token = getAuthToken();
    if (token) headers['Authorization'] = `Bearer ${token}`;

    const options = { method, headers };
    if (body) options.body = JSON.stringify(body);

    const response = await fetch(`${API_URL}${endpoint}`, options);

    if (response.status === 401) {
        localStorage.clear();
        window.location.href = '/index.html';
        return null;
    }

    if (!response.ok) {
        const error = await response.text();
        throw new Error(error || 'Ошибка запроса');
    }

    if (response.status === 204) return null;
    return await response.json();
}

const AuthAPI = {
    login: (login, password) => apiRequest('/Auth/login', 'POST', { login, password })
};

const AdminAPI = {
    getUsers: () => apiRequest('/Equestrian/admin/users'),
    updateUserRole: (userId, newRole) => apiRequest(`/Equestrian/admin/user/${userId}/role`, 'PUT', newRole),
    exportToExcel: () => window.open(`${API_URL}/Equestrian/admin/export/full`, '_blank'),
    createEmployee: (data) => apiRequest('/Equestrian/admin/employee', 'POST', data),
    createClient: (data) => apiRequest('/Equestrian/admin/client', 'POST', data),
    createHorse: (data) => apiRequest('/Equestrian/admin/horse', 'POST', data)
};

const CoachAPI = {
    getSchedule: (coachId) => apiRequest(`/Equestrian/coach/${coachId}/schedule`)
};

const ClientAPI = {
    getProfile: (clientId) => apiRequest(`/Equestrian/client/${clientId}/profile`),
    getCompetitions: () => apiRequest('/Equestrian/competitions'),
    registerForCompetition: (data) => apiRequest('/Equestrian/competition/register', 'POST', data),
    getClientSchedule: (clientId, startDate, endDate) => {
        let url = `/Equestrian/client/${clientId}/schedule`;    
        const params = [];
        if (startDate) params.push(`startDate=${encodeURIComponent(startDate)}`);
        if (endDate) params.push(`endDate=${encodeURIComponent(endDate)}`);
        if (params.length) url += '?' + params.join('&');
        return apiRequest(url);
    },
    getBookingData: () => apiRequest('/Equestrian/booking/data'),
    getAvailableHorses: (date, lessonType) => apiRequest(`/Equestrian/booking/horses?date=${date}&lessonType=${encodeURIComponent(lessonType)}`),
    createBooking: (data) => apiRequest('/Equestrian/booking/create', 'POST', data),
    printSchedule: (clientId) => window.open(`${API_URL}/Equestrian/client/${clientId}/schedule/print`, '_blank')
};