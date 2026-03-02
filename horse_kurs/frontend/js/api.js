// ========== БАЗОВАЯ НАСТРОЙКА ==========
const API_BASE = ''; // Пустой, так как API на том же сервере

// ========== УНИВЕРСАЛЬНАЯ ФУНКЦИЯ ==========
async function apiRequest(endpoint, method = 'GET', data = null) {
    const options = {
        method: method,
        headers: {
            'Content-Type': 'application/json',
        },
    };

    if (data) {
        options.body = JSON.stringify(data);
    }

    try {
        const response = await fetch(`${API_BASE}/api/${endpoint}`, options);

        if (!response.ok) {
            const error = await response.text();
            throw new Error(error || `Ошибка ${response.status}`);
        }

        if (response.status === 204) {
            return null;
        }

        return await response.json();
    } catch (error) {
        console.error('API Error:', error);
        throw error;
    }
}

// ========== API ЛОШАДЕЙ ==========
const HorsesAPI = {
    getAll: () => apiRequest('horses'),
    getById: (id) => apiRequest(`horses/${id}`),
    getByStatus: (status) => apiRequest(`horses/status/${encodeURIComponent(status)}`),
    getByHealth: (health) => apiRequest(`horses/health/${encodeURIComponent(health)}`),
    getByBreed: (breed) => apiRequest(`horses/breed/${encodeURIComponent(breed)}`),
    getAvailable: () => apiRequest('horses/available'),
    getBreedStats: () => apiRequest('horses/stats/breeds'),
    create: (horse) => apiRequest('horses', 'POST', horse),
    updateStatus: (id, status) => apiRequest(`horses/${id}/status`, 'PUT', status),
    updateHealth: (id, health) => apiRequest(`horses/${id}/health`, 'PUT', health)
};

// ========== API КЛИЕНТОВ ==========
const ClientsAPI = {
    getAll: () => apiRequest('clients'),
    getById: (id) => apiRequest(`clients/${id}`),
    getByLevel: (level) => apiRequest(`clients/level/${encodeURIComponent(level)}`),
    getWithMemberships: () => apiRequest('clients/with-memberships'),
    getActive: () => apiRequest('clients/active'),
    create: (client) => apiRequest('clients', 'POST', client),
    updateBalance: (id, amount) => apiRequest(`clients/${id}/balance`, 'PUT', amount)
};

// ========== API ДЕННИКОВ ==========
const StallsAPI = {
    getAll: () => apiRequest('stalls'),
    getById: (id) => apiRequest(`stalls/${id}`),
    getFree: () => apiRequest('stalls/free'),
    getByType: (type) => apiRequest(`stalls/type/${encodeURIComponent(type)}`),
    updateStatus: (id, status) => apiRequest(`stalls/${id}/status`, 'PUT', status),
    assignHorse: (stallId, horseId) => apiRequest(`stalls/${stallId}/assign-horse/${horseId}`, 'PUT')
};

// ========== API ЗАНЯТИЙ ==========
const LessonsAPI = {
    getAll: () => apiRequest('lessons'),
    getById: (id) => apiRequest(`lessons/${id}`),
    getByDate: (date) => apiRequest(`lessons/date/${date}`),
    getByClient: (clientId) => apiRequest(`lessons/client/${clientId}`),
    getByCoach: (coachId) => apiRequest(`lessons/coach/${coachId}`),
    create: (lesson) => apiRequest('lessons', 'POST', lesson)
};

// ========== API СОРЕВНОВАНИЙ ==========
const CompetitionsAPI = {
    getAll: () => apiRequest('competitions'),
    getById: (id) => apiRequest(`competitions/${id}`),
    getUpcoming: () => apiRequest('competitions/upcoming'),
    getByType: (type) => apiRequest(`competitions/type/${encodeURIComponent(type)}`),
    create: (competition) => apiRequest('competitions', 'POST', competition),
    updateStatus: (id, status) => apiRequest(`competitions/${id}/status`, 'PUT', status)
};

// ========== API УЧАСТИЙ ==========
const ParticipationsAPI = {
    getAll: () => apiRequest('participations'),
    getByCompetition: (competitionId) => apiRequest(`participations/competition/${competitionId}`),
    getByClient: (clientId) => apiRequest(`participations/client/${clientId}`),
    create: (participation) => apiRequest('participations', 'POST', participation),
    updateResult: (id, place, score) => apiRequest(`participations/${id}/result`, 'PUT', { place, score })
};

// ========== API ПЛАТЕЖЕЙ ==========
const PaymentsAPI = {
    getAll: () => apiRequest('payments'),
    getByClient: (clientId) => apiRequest(`payments/client/${clientId}`),
    getByDate: (date) => apiRequest(`payments/date/${date}`),
    getByPeriod: (from, to) => apiRequest(`payments/period?from=${from}&to=${to}`),
    getDailyStats: () => apiRequest('payments/stats/daily'),
    create: (payment) => apiRequest('payments', 'POST', payment)
};

// ========== API АБОНЕМЕНТОВ ==========
const MembershipsAPI = {
    getAll: () => apiRequest('memberships'),
    getByClient: (clientId) => apiRequest(`memberships/client/${clientId}`),
    getActive: () => apiRequest('memberships/active'),
    getExpiring: () => apiRequest('memberships/expiring'),
    create: (membership) => apiRequest('memberships', 'POST', membership),
    useLesson: (id) => apiRequest(`memberships/${id}/use-lesson`, 'PUT')
};

// ========== ВСПОМОГАТЕЛЬНЫЕ ФУНКЦИИ ==========
function formatDate(dateString) {
    if (!dateString) return '—';
    const date = new Date(dateString);
    return date.toLocaleDateString('ru-RU');
}

function formatDateTime(dateTimeString) {
    if (!dateTimeString) return '—';
    const date = new Date(dateTimeString);
    return date.toLocaleString('ru-RU');
}

function getStatusBadgeClass(status) {
    switch (status) {
        case 'В работе':
        case 'Активен':
        case 'Доступен':
        case 'Свободен':
        case 'Здорова':
        case 'Завершено':
            return 'bg-success';

        case 'На отдыхе':
        case 'Запланировано':
        case 'Ожидает':
        case 'Регистрация':
            return 'bg-warning';

        case 'На лечении':
        case 'Больна':
        case 'Травмирована':
        case 'Отменено':
        case 'Просрочен':
            return 'bg-danger';

        case 'Занят':
        case 'Идет':
            return 'bg-info';

        default:
            return 'bg-secondary';
    }
}

function calculateAge(birthDate) {
    if (!birthDate) return '—';
    const birth = new Date(birthDate);
    const now = new Date();
    let age = now.getFullYear() - birth.getFullYear();
    const m = now.getMonth() - birth.getMonth();
    if (m < 0 || (m === 0 && now.getDate() < birth.getDate())) {
        age--;
    }
    return age + ' лет';
}