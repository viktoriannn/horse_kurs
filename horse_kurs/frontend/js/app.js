function showHome() {
    document.getElementById('content').innerHTML = `
        <h1>Конный клуб</h1>
        <p>Добро пожаловать!</p>
        <button onclick="showHorses()">Лошади</button>
        <button onclick="showClients()">Клиенты</button>
    `;
}

// Показать список лошадей
async function showHorses() {
    let html = '<h2>Лошади</h2>';

    try {
        // Получаем данные
        let response = await fetch('/api/horses');
        let horses = await response.json();

        // Простая таблица
        html += '<table border="1" style="width:100%">';
        html += '<tr><th>Имя</th><th>Порода</th><th>Пол</th><th>Статус</th></tr>';

        for (let i = 0; i < horses.length; i++) {
            html += '<tr>';
            html += '<td>' + horses[i].name + '</td>';
            html += '<td>' + horses[i].breed + '</td>';
            html += '<td>' + horses[i].gender + '</td>';
            html += '<td>' + horses[i].status + '</td>';
            html += '</tr>';
        }

        html += '</table>';
        html += '<br><button onclick="showAddHorse()">Добавить лошадь</button>';

    } catch (error) {
        html += '<p>Ошибка загрузки</p>';
    }

    document.getElementById('content').innerHTML = html;
}

// Форма добавления лошади
function showAddHorse() {
    let html = `
        <h2>Добавить лошадь</h2>
        <div>
            <p>Имя: <input id="name"></p>
            <p>Порода: <input id="breed"></p>
            <p>Пол: 
                <select id="gender">
                    <option>Жеребец</option>
                    <option>Кобыла</option>
                </select>
            </p>
            <p>Дата рождения: <input id="date" type="date"></p>
            <button onclick="saveHorse()">Сохранить</button>
            <button onclick="showHorses()">Назад</button>
        </div>
    `;
    document.getElementById('content').innerHTML = html;
}

// Сохранить лошадь
async function saveHorse() {
    let horse = {
        name: document.getElementById('name').value,
        breed: document.getElementById('breed').value,
        gender: document.getElementById('gender').value,
        dateOfBirth: document.getElementById('date').value,
        status: 'В работе',
        stateOfHealth: 'Здорова',
        levelOfTraining: 'Начинающий',
        passport: 'пас_' + Date.now()
    };

    await fetch('/api/horses', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(horse)
    });

    alert('Лошадь добавлена!');
    showHorses();
}

// Показать список клиентов
async function showClients() {
    let html = '<h2>Клиенты</h2>';

    try {
        let response = await fetch('/api/clients');
        let clients = await response.json();

        html += '<table border="1" style="width:100%">';
        html += '<tr><th>ФИО</th><th>Телефон</th><th>Уровень</th><th>Баланс</th></tr>';

        for (let i = 0; i < clients.length; i++) {
            let c = clients[i];
            html += '<tr>';
            html += '<td>' + c.surname + ' ' + c.name + '</td>';
            html += '<td>' + c.phone + '</td>';
            html += '<td>' + c.levelOfTraining + '</td>';
            html += '<td>' + c.balance + '</td>';
            html += '</tr>';
        }

        html += '</table>';
        html += '<br><button onclick="showAddClient()">Добавить клиента</button>';

    } catch (error) {
        html += '<p>Ошибка загрузки</p>';
    }

    document.getElementById('content').innerHTML = html;
}

// Форма добавления клиента
function showAddClient() {
    let html = `
        <h2>Добавить клиента</h2>
        <div>
            <p>Фамилия: <input id="surname"></p>
            <p>Имя: <input id="name"></p>
            <p>Телефон: <input id="phone"></p>
            <p>Паспорт: <input id="passport"></p>
            <p>Уровень: 
                <select id="level">
                    <option>Новичок</option>
                    <option>Любитель</option>
                </select>
            </p>
            <button onclick="saveClient()">Сохранить</button>
            <button onclick="showClients()">Назад</button>
        </div>
    `;
    document.getElementById('content').innerHTML = html;
}

// Сохранить клиента
async function saveClient() {
    let client = {
        surname: document.getElementById('surname').value,
        name: document.getElementById('name').value,
        phone: document.getElementById('phone').value,
        passport: document.getElementById('passport').value,
        levelOfTraining: document.getElementById('level').value,
        city: 'Москва',
        street: 'Улица',
        house: '1',
        dateOfBirth: '2000-01-01',
        balance: 0
    };

    await fetch('/api/clients', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(client)
    });

    alert('Клиент добавлен!');
    showClients();
}

// Показать денники
async function showStalls() {
    let html = '<h2>Денники</h2>';

    try {
        let response = await fetch('/api/stalls');
        let stalls = await response.json();

        for (let i = 0; i < stalls.length; i++) {
            html += '<div style="border:1px solid black; margin:10px; padding:10px">';
            html += '<h3>Денник ' + stalls[i].number + '</h3>';
            html += '<p>Тип: ' + stalls[i].type + '</p>';
            html += '<p>Статус: ' + stalls[i].status + '</p>';
            html += '</div>';
        }

    } catch (error) {
        html += '<p>Ошибка загрузки</p>';
    }

    document.getElementById('content').innerHTML = html;
}

// Показать главную при загрузке
window.onload = showHome;