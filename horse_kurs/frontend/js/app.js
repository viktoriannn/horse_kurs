const API_URL = 'http://localhost:28280/api';
let currentUser = null;

// Инициализация при загрузке страницы
document.addEventListener('DOMContentLoaded', () => {
    checkAuth();

    const loginBtn = document.getElementById('login-btn');
    if (loginBtn) {
        loginBtn.addEventListener('click', handleLogin);
    }
});

// ========== СИСТЕМА АВТОРИЗАЦИИ ==========

async function checkAuth() {
    const savedUser = localStorage.getItem('currentUser');
    if (savedUser) {
        try {
            currentUser = JSON.parse(savedUser);
            showMainApp();
            loadPage('profile');
        } catch (e) {
            logout();
        }
    } else {
        showLoginForm();
    }
}

async function handleLogin() {
    const loginInput = document.getElementById('login-input').value;
    const passwordInput = document.getElementById('password-input').value;
    const errorMsg = document.getElementById('error-msg');

    if (!loginInput || !passwordInput) {
        errorMsg.innerText = 'Введите логин и пароль';
        return;
    }

    try {
        const response = await fetch(`${API_URL}/Auth/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ login: loginInput, password: passwordInput })
        });

        const data = await response.json();

        if (response.ok) {
            currentUser = data;
            localStorage.setItem('currentUser', JSON.stringify(currentUser));
            showMainApp();
            loadPage('profile');
        } else {
            errorMsg.innerText = data.message || 'Неверный логин или пароль';
        }
    } catch (error) {
        console.error(error);
        errorMsg.innerText = 'Ошибка подключения к серверу';
    }
}

function logout() {
    localStorage.removeItem('currentUser');
    currentUser = null;
    showLoginForm();
}

// ========== УПРАВЛЕНИЕ ИНТЕРФЕЙСОМ ==========

function showLoginForm() {
    document.getElementById('login-wrapper').style.display = 'block';
    document.getElementById('main-nav').style.display = 'none';
    document.getElementById('main-content').style.display = 'none';
    document.getElementById('main-footer').style.display = 'none';
}

function showMainApp() {
    document.getElementById('login-wrapper').style.display = 'none';
    document.getElementById('main-nav').style.display = 'flex';
    document.getElementById('main-content').style.display = 'block';
    document.getElementById('main-footer').style.display = 'block';

    const name = currentUser.fullName || currentUser.FullName || currentUser.login || 'Пользователь';
    document.getElementById('userNameDisplay').innerText = name;
    updateNavigation();
}

function updateNavigation() {
    const navMenu = document.getElementById('nav-menu');
    navMenu.innerHTML = '';

    const role = (currentUser.role || currentUser.Role);
    const commonItems = [{ name: 'Профиль', icon: 'fa-user', page: 'profile' }];
    let roleSpecificItems = [];

    if (role === 'Admin') {
        roleSpecificItems = [
            { name: 'Пользователи', icon: 'fa-users', page: 'users' },
            { name: 'Лошади', icon: 'fa-horse', page: 'horses' },
            { name: 'Соревнования', icon: 'fa-trophy', page: 'competitions' },
            { name: 'Отчеты', icon: 'fa-chart-bar', page: 'reports' }
        ];
    } else if (role === 'Coach') {
        roleSpecificItems = [
            { name: 'Расписание', icon: 'fa-calendar-alt', page: 'schedule' },
            { name: 'Лошади', icon: 'fa-horse', page: 'horses' }
        ];
    } else {
        roleSpecificItems = [
            { name: 'Мои занятия', icon: 'fa-calendar-check', page: 'my-lessons' },
            { name: 'Запись', icon: 'fa-book', page: 'booking' },
            { name: 'Соревнования', icon: 'fa-trophy', page: 'competitions' }
        ];
    }

    [...commonItems, ...roleSpecificItems].forEach(item => {
        const li = document.createElement('li');
        li.className = 'nav-item';
        li.innerHTML = `
            <a class="nav-link" href="#" onclick="loadPage('${item.page}')">
                <i class="fas ${item.icon} me-1"></i> ${item.name}
            </a>`;
        navMenu.appendChild(li);
    });
}

// ========== РОУТИНГ (ЗАГРУЗКА СТРАНИЦ) ==========

async function loadPage(page) {
    const container = document.getElementById('main-content');
    container.innerHTML = '<div class="text-center mt-5"><div class="spinner-border text-primary"></div></div>';

    switch (page) {
        case 'profile': await loadProfile(container); break;
        case 'my-lessons': await loadMyLessons(container); break;
        case 'booking': await loadBookingForm(container); break;
        case 'schedule': await loadCoachSchedule(container); break;
        case 'users': await loadUsers(container); break;
        case 'horses': await loadHorses(container); break;
        case 'competitions': await loadCompetitions(container); break;
        case 'reports': await loadReports(container); break;
        default: container.innerHTML = '<h4>Страница не найдена</h4>';
    }
}

// ========== МОДУЛИ КОНТЕНТА ==========

async function loadProfile(container) {
    const role = currentUser.role || currentUser.Role;
    const clientId = currentUser.idClient || currentUser.IdClient;

    if (role === 'Admin' && !clientId) {
        container.innerHTML = `<div class="alert alert-info"><h4>Админ-панель</h4><p>Вы зашли как системный администратор.</p></div>`;
        return;
    }

    try {
        const response = await fetch(`${API_URL}/Equestrian/client/${clientId}/profile`);
        if (!response.ok) throw new Error('Данные недоступны');
        const profile = await response.json();

        container.innerHTML = `
            <div class="row">
                <div class="col-md-4">
                    <div class="card shadow-sm mb-4">
                        <div class="card-body text-center">
                            <i class="fas fa-user-circle fa-5x text-primary mb-3"></i>
                            <h4>${profile.fullName || 'Пользователь'}</h4>
                            <p class="text-muted">${profile.levelOfTraining || 'Уточняется'}</p>
                            <p><i class="fas fa-phone me-2"></i> ${profile.phone || '—'}</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-8">
                    <div class="card shadow-sm mb-4">
                        <div class="card-body text-center">
                            <h6 class="text-muted">Баланс</h6>
                            <h2 class="text-primary">${(profile.balance || 0).toLocaleString()} руб.</h2>
                            <button class="btn btn-outline-success" onclick="showTopUpModal()">Пополнить</button>
                        </div>
                    </div>
                </div>
            </div>`;
    } catch (e) {
        container.innerHTML = `<div class="alert alert-danger">Ошибка: ${e.message}</div>`;
    }
}

async function loadBookingForm(container) {
    container.innerHTML = `
        <div class="card shadow-sm max-width-600 mx-auto">
            <div class="card-header bg-primary text-white"><h5>Запись на занятие</h5></div>
            <div class="card-body">
                <form id="bookingForm">
                    <div class="mb-3"><label>Дата</label><input type="date" id="lessonDate" class="form-control" required></div>
                    <div class="mb-3"><label>Тип</label>
                        <select id="lessonType" class="form-select">
                            <option value="Индивидуальное">Индивидуальное</option>
                            <option value="Групповое">Групповое</option>
                        </select>
                    </div>
                    <div class="mb-3"><label>ID Тренера</label><input type="number" id="coachId" class="form-control" required></div>
                    <div class="mb-3"><label>ID Арены</label><input type="number" id="arenaId" class="form-control" required></div>
                    <button type="submit" class="btn btn-primary w-100">Записаться</button>
                </form>
            </div>
        </div>`;

    document.getElementById('bookingForm').addEventListener('submit', async (e) => {
        e.preventDefault();
        const data = {
            idClient: currentUser.idClient || currentUser.IdClient,
            idCoach: parseInt(document.getElementById('coachId').value),
            idArena: parseInt(document.getElementById('arenaId').value),
            date: document.getElementById('lessonDate').value,
            lessonType: document.getElementById('lessonType').value,
            horseIds: [] // Можно добавить выбор позже
        };

        const res = await fetch(`${API_URL}/Equestrian/lesson/register`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });

        if (res.ok) { alert('Успешно!'); loadPage('my-lessons'); }
        else alert('Ошибка при записи');
    });
}

async function loadCoachSchedule(container) {
    try {
        const coachId = currentUser.idCoach || 1;
        const res = await fetch(`${API_URL}/Equestrian/coach/${coachId}/schedule`);
        const schedule = await res.json();

        container.innerHTML = `
            <div class="card shadow-sm">
                <div class="card-header bg-primary text-white"><h5>Мое расписание</h5></div>
                <div class="card-body">
                    ${schedule.length > 0 ? schedule.map(l => `
                        <div class="border rounded p-3 mb-2">
                            <strong>${new Date(l.date).toLocaleDateString()}</strong> | ${l.startTime}
                            <br>Клиент: ${l.clientFullName} (${l.clientLevel})
                        </div>`).join('') : '<p>Занятий нет</p>'}
                </div>
            </div>`;
    } catch (e) { container.innerHTML = 'Ошибка расписания'; }
}

async function loadUsers(container) {
    try {
        const res = await fetch(`${API_URL}/Equestrian/admin/users`);
        const users = await res.json();

        container.innerHTML = `
            <div class="card shadow-sm">
                <div class="card-header bg-primary text-white d-flex justify-content-between">
                    <h5 class="mb-0">Пользователи</h5>
                    <button class="btn btn-sm btn-light" onclick="downloadUsersReport()">Отчет</button>
                </div>
                <div class="table-responsive">
                    <table class="table table-hover">
                        <thead><tr><th>ID</th><th>Логин</th><th>Роль</th><th>Баланс</th></tr></thead>
                        <tbody>
                            ${users.map(u => `
                                <tr>
                                    <td>${u.idUser}</td>
                                    <td>${u.login}</td>
                                    <td>
                                        <select class="form-select form-select-sm" onchange="updateUserRole(${u.idUser}, this.value)">
                                            <option value="Client" ${u.role === 'Client' ? 'selected' : ''}>Клиент</option>
                                            <option value="Coach" ${u.role === 'Coach' ? 'selected' : ''}>Тренер</option>
                                            <option value="Admin" ${u.role === 'Admin' ? 'selected' : ''}>Админ</option>
                                        </select>
                                    </td>
                                    <td>${u.client?.balance || 0} р.</td>
                                </tr>`).join('')}
                        </tbody>
                    </table>
                </div>
            </div>`;
    } catch (e) { container.innerHTML = 'Ошибка загрузки пользователей'; }
}

async function loadHorses(container) {
    container.innerHTML = '<div class="row text-center"><p>Список лошадей загружается...</p></div>';
    // Здесь можно добавить fetch к /Equestrian/horses
}

async function loadCompetitions(container) {
    try {
        const res = await fetch(`${API_URL}/Equestrian/competitions`);
        const comps = await res.json();
        container.innerHTML = `<h5>Соревнования</h5><div class="row">${comps.map(c => `
            <div class="col-md-6 mb-3">
                <div class="card">
                    <div class="card-body">
                        <h6>${c.name}</h6>
                        <p>${new Date(c.date).toLocaleDateString()} - ${c.registrationFee} р.</p>
                        <button class="btn btn-primary btn-sm" onclick="registerForCompetition(${c.idCompetition})">Записаться</button>
                    </div>
                </div>
            </div>`).join('')}</div>`;
    } catch (e) { container.innerHTML = 'Ошибка соревнований'; }
}

async function loadReports(container) {
    container.innerHTML = `
        <div class="text-center">
            <button class="btn btn-primary m-2" onclick="downloadUsersReport()">Отчет по юзерам</button>
            <button class="btn btn-success m-2" onclick="alert('В разработке')">Финансовый отчет</button>
        </div>`;
}

// ========== ДОПОЛНИТЕЛЬНЫЕ ФУНКЦИИ ==========

async function updateUserRole(userId, newRole) {
    const res = await fetch(`${API_URL}/Equestrian/admin/user/${userId}/role`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(newRole)
    });
    if (res.ok) alert('Роль обновлена');
}

function downloadUsersReport() {
    window.open(`${API_URL}/Equestrian/admin/users/report`);
}

function showTopUpModal() {
    const amount = prompt('Сумма пополнения:', '1000');
    if (amount) alert('Запрос отправлен');
}

async function loadMyLessons(container) {
    // В данной версии просто показываем профиль или можно сделать отдельный fetch к расписанию клиента
    await loadProfile(container);
}