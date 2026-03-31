const API_URL = "http://localhost:28280/api";

let currentUser = {
    isLoggedIn: false,
    token: null,
    clientId: null,
    name: ''
};

function checkAuth() {
    const savedId = localStorage.getItem('clientId');
    if (savedId) {
        currentUser.isLoggedIn = true;
        currentUser.clientId = savedId;
        currentUser.name = localStorage.getItem('userName');
        updateNav();
    }
}

async function loadPage(page) {
    const container = document.getElementById('main-content');
    container.innerHTML = '<div class="text-center my-5"><div class="spinner-border text-primary"></div></div>';
    container.scrollIntoView({ behavior: 'smooth' });

    switch (page) {
        case 'horses':
            await loadHorses();
            break;
        case 'schedule':
            loadSchedulePage();
            break;
        case 'competitions':
            await loadCompetitions();
            break;
        case 'profile':
            const id = localStorage.getItem('clientId') || 1;
            await loadProfile(id);
            break;
        case 'booking':
            showBookingForm();
            break;
        default:
            container.innerHTML = '<h3 class="text-center mt-5">Выберите интересующий вас раздел</h3>';
    }
}
async function loadHorses() {
    const container = document.getElementById('main-content');
    try {
        const response = await fetch(`${API_URL}/horses`);
        const horses = await response.json();

        let html = '<h2 class="mb-5 text-center fw-bold">Наши лошади</h2><div class="row">';

        horses.forEach(h => {
            const status = h.stateOfHealth || h.StateOfHealth || "Здорова";

            let badgeClass = 'bg-success'; 
            if (status.toLowerCase().includes('болен') || status.toLowerCase().includes('травма')) {
                badgeClass = 'bg-danger';
            } else if (status.toLowerCase().includes('отдых')) {
                badgeClass = 'bg-warning text-dark';
            }

            html += `
                <div class="col-md-4 mb-4">
                    <div class="card h-100 border-0 shadow-sm">
                        <div class="card-body text-center p-4">
                            <h5 class="card-title fw-bold mb-3">${h.name}</h5>
                            <p class="card-text text-muted mb-3">
                                <strong>Порода:</strong> ${h.breed}
                            </p>
                            <span class="badge ${badgeClass} px-3 py-2 rounded-pill" style="font-size: 0.9rem;">
                                ${status}
                            </span>
                        </div>
                    </div>
                </div>`;
        });
        container.innerHTML = html + '</div>';
    } catch (err) {
        container.innerHTML = '<div class="alert alert-danger">Не удалось загрузить список лошадей.</div>';
    }
}
async function loadProfile(clientId) {
    const container = document.getElementById('main-content');
    try {
        const response = await fetch(`${API_URL}/clients/${clientId}/profile`);
        const p = await response.json();

        container.innerHTML = `
            <div class="card shadow p-4 mx-auto" style="max-width: 600px;">
                <h2 class="text-center">${p.fullName}</h2>
                <hr>
                <div class="d-flex justify-content-between">
                    <span>Баланс:</span>
                    <h4 class="text-success">${p.balance} ₽</h4>
                </div>
                <h5 class="mt-4">Ваши абонементы:</h5>
                <div class="list-group">
                    ${p.activeMemberships && p.activeMemberships.length > 0
                ? p.activeMemberships.map(m => `<div class="list-group-item">${m.type} - до ${m.validUntil}</div>`).join('')
                : '<p class="text-muted">Нет активных абонементов</p>'}
                </div>
            </div>`;
    } catch (err) {
        container.innerHTML = '<div class="alert alert-danger">Ошибка загрузки профиля. Убедитесь, что клиент с таким ID существует.</div>';
    }
}

function loadSchedulePage() {
    const container = document.getElementById('main-content');
    const today = new Date().toISOString().split('T')[0];
    container.innerHTML = `
        <h2 class="text-center">Расписание</h2>
        <div id="schedule-list" class="list-group mt-3">Загрузка...</div>
    `;
    fetchSchedule(today);
}

async function fetchSchedule(date) {
    try {
        const response = await fetch(`${API_URL}/lessons/schedule?date=${date}`);
        const data = await response.json();
        const list = document.getElementById('schedule-list');

        if (!data || data.length === 0) {
            list.innerHTML = '<p class="text-center">Занятий на эту дату пока нет.</p>';
            return;
        }

        list.innerHTML = data.map(l => `
            <div class="list-group-item d-flex justify-content-between align-items-center">
                <div>
                    <strong>${l.type}</strong><br>
                    <small>Клиент: ${l.clientName} | Лошадь: ${l.horseName}</small>
                </div>
                <span class="badge bg-primary rounded-pill">${l.date.split('T')[1].substring(0, 5)}</span>
            </div>
        `).join('');
    } catch (err) {
        document.getElementById('schedule-list').innerHTML = "Ошибка загрузки расписания.";
    }
}

async function loadCompetitions() {
    const container = document.getElementById('main-content');
    try {
        const response = await fetch(`${API_URL}/competitions`);
        const comps = await response.json();

        let html = '<h2 class="text-center mb-4">Ближайшие старты</h2><div class="list-group shadow-sm">';
        comps.forEach(c => {
            html += `
                <div class="list-group-item d-flex justify-content-between align-items-center">
                    <div>
                        <h5 class="mb-0">${c.name}</h5>
                        <small class="text-muted">Дата: ${c.date}</small>
                    </div>
                    <button class="btn btn-warning btn-sm" onclick="alert('Регистрация через ЛК')">Участвовать</button>
                </div>`;
        });
        container.innerHTML = html + '</div>';
    } catch (err) {
        container.innerHTML = '<div class="alert alert-danger">Ошибка загрузки соревнований.</div>';
    }
}

function showBookingForm() {

    const container = document.getElementById('main-content');

    container.innerHTML = `

        <div class="card mx-auto" style="max-width: 500px;">
            <div class="card-body">
                <h3 class="card-title text-center mb-4">Запись на тренировку</h3>
                <form id="bookForm">
                    <div class="mb-3">
                        <label class="form-label">Дата и время</label>
                        <input type="datetime-local" class="form-control" id="lessonDate" required>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Тип занятия</label>
                        <select class="form-select" id="lessonType">
                            <option value="Индивидуальное">Индивидуальное</option>
                            <option value="Групповое">Групповое</option>
                            <option value="Конкур">Конкур</option>
                        </select>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">ID Клиента (для теста)</label>
                        <input type="number" class="form-control" id="clientId" value="1">
                    </div>
                    <div class="mb-3">
                        <label class="form-label">ID Тренера</label>
                        <input type="number" class="form-control" id="coachId" value="1">
                    </div>
                    <div class="mb-3">
                        <label class="form-label">ID Лошади</label>
                        <input type="number" class="form-control" id="horseId" value="1">
                    </div>
                    <button type="submit" class="btn btn-primary w-100">Забронировать</button>
                </form>
                <div id="formMessage" class="mt-3"></div>
            </div>
        </div>
    `;
    document.getElementById('bookForm').onsubmit = async (e) => {
        e.preventDefault();
        const msg = document.getElementById('formMessage');
        const data = {

            date: document.getElementById('lessonDate').value,
            type: document.getElementById('lessonType').value,
            clientId: parseInt(document.getElementById('clientId').value),
            coachId: parseInt(document.getElementById('coachId').value),
            horseId: parseInt(document.getElementById('horseId').value)
        };
        try {
            const response = await fetch(`${API_URL}/lessons/book`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(data)
            });
            const result = await response.json();
            if (response.ok) {
                msg.innerHTML = `<div class="alert alert-success">${result.message}</div>`;
                setTimeout(() => loadPage('schedule'), 2000);
            } else {
                msg.innerHTML = `<div class="alert alert-danger">${result.message || result}</div>`;
            }
        } catch (err) {
            msg.innerHTML = `<div class="alert alert-danger">Ошибка сервера</div>`;

        }
    };
}

function showAuthForm(type = 'login') {
    const container = document.getElementById('main-content');
    if (type === 'login') {
        container.innerHTML = `
            <div class="card mx-auto shadow" style="max-width: 400px;">
                <div class="card-body">
                    <h3 class="text-center">Вход</h3>
                    <form onsubmit="handleLogin(event)">
                        <input type="email" id="email" class="form-control mb-2" placeholder="Email" required>
                        <input type="password" id="password" class="form-control mb-3" placeholder="Пароль" required>
                        <button class="btn btn-primary w-100">Войти</button>
                    </form>
                    <p class="mt-3 text-center">Нет аккаунта? <a href="#" onclick="showAuthForm('reg')">Регистрация</a></p>
                </div>
            </div>`;
    } else {
        container.innerHTML = `
            <div class="card mx-auto shadow" style="max-width: 400px;">
                <div class="card-body">
                    <h3 class="text-center">Регистрация</h3>
                    <form onsubmit="handleRegister(event)">
                        <input type="text" id="regName" class="form-control mb-2" placeholder="Имя" required>
                        <input type="text" id="regSurname" class="form-control mb-2" placeholder="Фамилия" required>
                        <input type="email" id="regEmail" class="form-control mb-2" placeholder="Email" required>
                        <input type="password" id="regPass" class="form-control mb-3" placeholder="Пароль" required>
                        <button class="btn btn-success w-100">Создать аккаунт</button>
                    </form>
                </div>
            </div>`;
    }
}

async function handleLogin(e) {
    e.preventDefault();
    const loginVal = document.getElementById('email').value; 
    const passVal = document.getElementById('password').value;

    try {
        const response = await fetch(`${API_URL}/auth/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ login: loginVal, password: passVal }) 
        });

        if (response.ok) {
            const user = await response.json();

            localStorage.setItem('clientId', user.id);
            localStorage.setItem('userName', user.name);

            currentUser.isLoggedIn = true;
            currentUser.clientId = user.id;

            alert(`Добро пожаловать, ${user.name}!`);
            location.reload();
        } else {
            const error = await response.json();
            alert(error.message || "Ошибка входа");
        }
    } catch (err) {
        alert("Сервер не отвечает.");
    }
}
function logout() {
    localStorage.clear();
    location.reload();
}

function updateNav() {
    const nav = document.querySelector('.navbar-nav');
    if (currentUser.isLoggedIn) {
    }
}
document.addEventListener('DOMContentLoaded', () => {
    loadPage('home'); 
});