document.addEventListener('DOMContentLoaded', () => {
    showHome();
});

function showHome() {
    const content = document.getElementById('content');
    content.innerHTML = `
        <h1>Конно-спортивный клуб</h1>
        <p style="font-size: 18px; color: #666; margin-bottom: 30px;">Добро пожаловать!</p>
        
        <div class="card-grid">
            <div class="card" onclick="showHorses()">
                <i class="fas fa-horse"></i>
                <h3>Лошади</h3>
                <p>Просмотр лошадей клуба</p>
                <button class="btn btn-small">Перейти</button>
            </div>
            
            <div class="card" onclick="showStalls()">
                <i class="fas fa-warehouse"></i>
                <h3>Денники</h3>
                <p>Информация о денниках</p>
                <button class="btn btn-small">Перейти</button>
            </div>
        </div>
    `;

    if (typeof currentUser !== 'undefined' && currentUser) {
        const cardGrid = document.querySelector('.card-grid');
        if (cardGrid) {
            cardGrid.innerHTML += `
                <div class="card" onclick="showClients()">
                    <i class="fas fa-users"></i>
                    <h3>Клиенты</h3>
                    <p>Управление клиентами</p>
                    <button class="btn btn-small">Перейти</button>
                </div>
            `;
        }
    }
}

async function showHorses() {
    let html = '<h2>Лошади</h2>';

    if (typeof currentUser !== 'undefined' && currentUser && currentUser.role === 'admin') {
        html += '<button class="btn" onclick="showAddHorse()">➕ Добавить лошадь</button>';
    }

    html += '<div id="horses-list">Загрузка...</div>';
    document.getElementById('content').innerHTML = html;

    try {
        const headers = {};
        if (typeof getAuthHeaders === 'function') {
            Object.assign(headers, getAuthHeaders());
        }

        let response = await fetch('/api/horses', { headers });
        let horses = await response.json();

        if (horses.length === 0) {
            document.getElementById('horses-list').innerHTML = '<p>Нет данных</p>';
        } else {
            let table = '<table><tr><th>Имя</th><th>Порода</th><th>Пол</th><th>Статус</th><th>Здоровье</th></tr>';

            for (let i = 0; i < horses.length; i++) {
                let h = horses[i];
                table += '<tr>';
                table += '<td>' + (h.name || '—') + '</td>';
                table += '<td>' + (h.breed || '—') + '</td>';
                table += '<td>' + (h.gender || '—') + '</td>';
                table += '<td>' + (h.status || '—') + '</td>';
                table += '<td>' + (h.stateOfHealth || '—') + '</td>';
                table += '</tr>';
            }

            table += '</table>';
            document.getElementById('horses-list').innerHTML = table;
        }
    } catch (error) {
        document.getElementById('horses-list').innerHTML = '<p style="color:red">Ошибка загрузки</p>';
    }

    html += '<br><button class="btn btn-secondary" onclick="showHome()">На главную</button>';
}

function showAddHorse() {
    // Проверка прав
    if (typeof currentUser === 'undefined' || !currentUser || currentUser.role !== 'admin') {
        alert('У вас нет прав для добавления лошадей');
        showHorses();
        return;
    }

    let html = `
        <h2>Добавить лошадь</h2>
        <div style="max-width: 500px">
            <div class="form-group">
                <label>Имя:</label>
                <input type="text" id="name" class="form-control">
            </div>
            <div class="form-group">
                <label>Порода:</label>
                <input type="text" id="breed" class="form-control">
            </div>
            <div class="form-group">
                <label>Пол:</label>
                <select id="gender" class="form-control">
                    <option>Жеребец</option>
                    <option>Кобыла</option>
                    <option>Мерин</option>
                </select>
            </div>
            <div class="form-group">
                <label>Дата рождения:</label>
                <input type="date" id="dateOfBirth" class="form-control">
            </div>
            <button class="btn" onclick="saveHorse()">Сохранить</button>
            <button class="btn btn-secondary" onclick="showHorses()">Отмена</button>
        </div>
    `;
    document.getElementById('content').innerHTML = html;
}

async function saveHorse() {
    let horse = {
        name: document.getElementById('name').value,
        breed: document.getElementById('breed').value,
        gender: document.getElementById('gender').value,
        dateOfBirth: document.getElementById('dateOfBirth').value,
        status: 'В работе',
        stateOfHealth: 'Здорова',
        levelOfTraining: 'Начинающий',
        passport: 'пас_' + Date.now()
    };

    try {
        const headers = { 'Content-Type': 'application/json' };
        if (typeof getAuthHeaders === 'function') {
            Object.assign(headers, getAuthHeaders());
        }

        let response = await fetch('/api/horses', {
            method: 'POST',
            headers: headers,
            body: JSON.stringify(horse)
        });

        if (response.ok) {
            alert('Лошадь добавлена!');
            showHorses();
        } else {
            alert('Ошибка при сохранении');
        }
    } catch (error) {
        alert('Ошибка: ' + error.message);
    }
}

async function showClients() {
    // Проверка авторизации
    if (typeof currentUser === 'undefined' || !currentUser) {
        alert('Необходимо авторизоваться');
        if (typeof openModal === 'function') {
            openModal('loginModal');
        }
        return;
    }

    let html = '<h2>Клиенты</h2>';
    html += '<div id="clients-list">Загрузка...</div>';
    document.getElementById('content').innerHTML = html;

    try {
        const headers = {};
        if (typeof getAuthHeaders === 'function') {
            Object.assign(headers, getAuthHeaders());
        }

        let response = await fetch('/api/clients', { headers });
        let clients = await response.json();

        if (clients.length === 0) {
            document.getElementById('clients-list').innerHTML = '<p>Нет данных</p>';
        } else {
            let table = '<table><tr><th>ФИО</th><th>Телефон</th><th>Уровень</th><th>Баланс</th></tr>';

            for (let i = 0; i < clients.length; i++) {
                let c = clients[i];
                let name = (c.surname || '') + ' ' + (c.name || '');
                table += '<tr>';
                table += '<td>' + name + '</td>';
                table += '<td>' + (c.phone || '—') + '</td>';
                table += '<td>' + (c.levelOfTraining || '—') + '</td>';
                table += '<td>' + (c.balance || 0) + ' ₽</td>';
                table += '</tr>';
            }

            table += '</table>';
            document.getElementById('clients-list').innerHTML = table;
        }
    } catch (error) {
        document.getElementById('clients-list').innerHTML = '<p style="color:red">Ошибка загрузки</p>';
    }

    html += '<br><button class="btn btn-secondary" onclick="showHome()">На главную</button>';
}

async function showStalls() {
    let html = '<h2>Денники</h2>';
    html += '<div id="stalls-list">Загрузка...</div>';
    document.getElementById('content').innerHTML = html;

    try {
        const headers = {};
        if (typeof getAuthHeaders === 'function') {
            Object.assign(headers, getAuthHeaders());
        }

        let response = await fetch('/api/stalls', { headers });
        let stalls = await response.json();

        if (stalls.length === 0) {
            document.getElementById('stalls-list').innerHTML = '<p>Нет данных</p>';
        } else {
            let grid = '<div class="stall-grid">';

            for (let i = 0; i < stalls.length; i++) {
                let s = stalls[i];
                grid += '<div class="stall-card">';
                grid += '<div class="stall-header">Денник ' + (s.number || '—') + '</div>';
                grid += '<div class="stall-body">';
                grid += '<p><i class="fas fa-tag"></i> Тип: ' + (s.type || '—') + '</p>';
                grid += '<p><i class="fas fa-ruler"></i> Размер: ' + (s.size || '—') + ' м²</p>';
                grid += '<p><i class="fas fa-circle"></i> Статус: ' + (s.status || '—') + '</p>';
                grid += '</div>';
                grid += '</div>';
            }

            grid += '</div>';
            document.getElementById('stalls-list').innerHTML = grid;
        }
    } catch (error) {
        document.getElementById('stalls-list').innerHTML = '<p style="color:red">Ошибка загрузки</p>';
    }

    html += '<br><button class="btn btn-secondary" onclick="showHome()">На главную</button>';
}