
let currentUser = null;

document.addEventListener('DOMContentLoaded', function () {
    console.log('auth.js загружен');
    loadUserSession();
    updateAuthUI();

    setupModals();

    const loginForm = document.getElementById('loginForm');
    if (loginForm) {
        loginForm.addEventListener('submit', handleLogin);
    } else {
        console.error('Форма входа не найдена!');
    }

    const registerForm = document.getElementById('registerForm');
    if (registerForm) {
        registerForm.addEventListener('submit', handleRegister);
    } else {
        console.error('Форма регистрации не найдена!');
    }

    if (typeof showHome === 'function') {
        showHome();
    }
});

function setupModals() {
    window.onclick = function (event) {
        const modals = ['loginModal', 'registerModal', 'profileModal'];
        modals.forEach(modalId => {
            const modal = document.getElementById(modalId);
            if (event.target === modal) {
                closeModal(modalId);
            }
        });
    };

    document.addEventListener('keydown', function (event) {
        if (event.key === 'Escape') {
            const modals = ['loginModal', 'registerModal', 'profileModal'];
            modals.forEach(modalId => {
                const modal = document.getElementById(modalId);
                if (modal && modal.style.display === 'block') {
                    closeModal(modalId);
                }
            });
        }
    });
}

function loadUserSession() {
    const savedUser = localStorage.getItem('currentUser');
    if (savedUser) {
        try {
            currentUser = JSON.parse(savedUser);
            console.log('Сессия загружена:', currentUser);
        } catch (e) {
            console.error('Ошибка загрузки сессии:', e);
        }
    }
}

function saveUserSession(user) {
    currentUser = user;
    localStorage.setItem('currentUser', JSON.stringify(user));
    console.log('Сессия сохранена:', user);
}

function clearUserSession() {
    currentUser = null;
    localStorage.removeItem('currentUser');
    console.log('Сессия очищена');
}

function updateAuthUI() {
    const authSection = document.getElementById('auth-section');
    const navMenu = document.getElementById('nav-menu');

    if (!authSection) {
        console.error('auth-section не найдена');
        return;
    }

    if (currentUser) {
        const roleLabels = {
            'admin': 'Администратор',
            'coach': 'Тренер',
            'client': 'Клиент'
        };

        const userName = currentUser.name || currentUser.surname || 'Пользователь';
        const userInitial = userName.charAt(0).toUpperCase();

        authSection.innerHTML = `
            <div class="user-info">
                <div class="user-avatar">${userInitial}</div>
                <span class="user-name">${userName}</span>
                <span class="user-role">${roleLabels[currentUser.role] || currentUser.role}</span>
                <button class="btn-small" onclick="showProfile()">
                    <i class="fas fa-user"></i>
                </button>
                <button class="btn-small btn-secondary" onclick="handleLogout()">
                    <i class="fas fa-sign-out-alt"></i>
                </button>
            </div>
        `;

        // Обновляем навигацию в зависимости от роли
        if (navMenu) {
            updateNavByRole(navMenu);
        }

    } else {
        authSection.innerHTML = `
            <button class="nav-btn" onclick="openModal('loginModal')">
                <i class="fas fa-sign-in-alt"></i> <span>Вход</span>
            </button>
            <button class="nav-btn" onclick="openModal('registerModal')">
                <i class="fas fa-user-plus"></i> <span>Регистрация</span>
            </button>
        `;

        if (navMenu) {
            navMenu.innerHTML = `
                <button class="nav-btn" onclick="showHome()">
                    <i class="fas fa-home"></i> <span>Главная</span>
                </button>
                <button class="nav-btn" onclick="showHorses()">
                    <i class="fas fa-horse"></i> <span>Лошади</span>
                </button>
                <button class="nav-btn" onclick="showStalls()">
                    <i class="fas fa-warehouse"></i> <span>Денники</span>
                </button>
            `;
        }
    }
}

function updateNavByRole(navMenu) {
    let navHtml = `
        <button class="nav-btn" onclick="showHome()">
            <i class="fas fa-home"></i> <span>Главная</span>
        </button>
        <button class="nav-btn" onclick="showHorses()">
            <i class="fas fa-horse"></i> <span>Лошади</span>
        </button>
        <button class="nav-btn" onclick="showStalls()">
            <i class="fas fa-warehouse"></i> <span>Денники</span>
        </button>
    `;

    if (currentUser && currentUser.role) {
        if (currentUser.role === 'admin') {
            navHtml += `
                <button class="nav-btn" onclick="showClients()">
                    <i class="fas fa-users"></i> <span>Клиенты</span>
                </button>
            `;
        } else if (currentUser.role === 'coach') {
            navHtml += `
                <button class="nav-btn" onclick="showMyLessons()">
                    <i class="fas fa-calendar-check"></i> <span>Мои занятия</span>
                </button>
            `;
        } else if (currentUser.role === 'client') {
            navHtml += `
                <button class="nav-btn" onclick="showMyLessons()">
                    <i class="fas fa-calendar-check"></i> <span>Мои занятия</span>
                </button>
                <button class="nav-btn" onclick="showMyMemberships()">
                    <i class="fas fa-ticket-alt"></i> <span>Абонементы</span>
                </button>
            `;
        }
    }

    navMenu.innerHTML = navHtml;
}

function openModal(modalId) {
    console.log('Открытие модального окна:', modalId);
    const modal = document.getElementById(modalId);
    if (modal) {
        modal.style.display = 'block';
    } else {
        console.error('Модальное окно не найдено:', modalId);
    }
}

function closeModal(modalId) {
    console.log('Закрытие модального окна:', modalId);
    const modal = document.getElementById(modalId);
    if (modal) {
        modal.style.display = 'none';
    }
}

async function handleLogin(event) {
    event.preventDefault();
    console.log('handleLogin вызван');

    const login = document.getElementById('login').value;
    const password = document.getElementById('password').value;

    if (!login || !password) {
        alert('Заполните все поля');
        return;
    }

    const submitBtn = event.target.querySelector('button[type="submit"]');
    const originalText = submitBtn.innerHTML;
    submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Вход...';
    submitBtn.disabled = true;

    try {
        const response = await fetch('/api/auth/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ login, password })
        });

        const data = await response.json();
        console.log('Ответ сервера:', data);

        if (data.success) {
            saveUserSession(data);
            closeModal('loginModal');
            updateAuthUI();
            alert('Успешный вход!');
            if (typeof showHome === 'function') showHome();
        } else {
            alert(data.message || 'Неверный логин или пароль');
        }
    } catch (error) {
        console.error('Ошибка:', error);
        alert('Ошибка сервера: ' + error.message);
    } finally {
        submitBtn.innerHTML = originalText;
        submitBtn.disabled = false;
    }
}

async function handleRegister(event) {
    event.preventDefault();
    console.log('handleRegister вызван');

    const requiredFields = ['regSurname', 'regName', 'regPhone', 'regPassword', 'regPassport'];
    for (let fieldId of requiredFields) {
        const field = document.getElementById(fieldId);
        if (!field || !field.value) {
            alert('Заполните все обязательные поля');
            return;
        }
    }

    const userData = {
        surname: document.getElementById('regSurname').value,
        name: document.getElementById('regName').value,
        patronymic: document.getElementById('regPatronymic')?.value || null,
        dateOfBirth: document.getElementById('regDateOfBirth')?.value || null,
        phone: document.getElementById('regPhone').value,
        email: document.getElementById('regEmail')?.value || null,
        password: document.getElementById('regPassword').value,
        passport: document.getElementById('regPassport').value,
        city: document.getElementById('regCity')?.value || 'Москва',
        street: document.getElementById('regStreet')?.value || '',
        house: document.getElementById('regHouse')?.value || '',
        flat: document.getElementById('regFlat')?.value || null,
        levelOfTraining: document.getElementById('regLevel')?.value || 'Новичок'
    };

    const submitBtn = event.target.querySelector('button[type="submit"]');
    const originalText = submitBtn.innerHTML;
    submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Регистрация...';
    submitBtn.disabled = true;

    try {
        const response = await fetch('/api/clients/register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(userData)
        });

        if (response.ok) {
            alert('Регистрация успешна! Теперь вы можете войти.');
            closeModal('registerModal');
            document.getElementById('registerForm').reset();
            setTimeout(() => openModal('loginModal'), 500);
        } else {
            const error = await response.json();
            alert(error.message || 'Ошибка регистрации');
        }
    } catch (error) {
        console.error('Ошибка:', error);
        alert('Ошибка сервера: ' + error.message);
    } finally {
        submitBtn.innerHTML = originalText;
        submitBtn.disabled = false;
    }
}

function handleLogout() {
    clearUserSession();
    updateAuthUI();
    alert('Вы вышли из системы');
    if (typeof showHome === 'function') showHome();
}

async function showProfile() {
    if (!currentUser) {
        openModal('loginModal');
        return;
    }

    const profileContent = document.getElementById('profileContent');
    if (!profileContent) {
        console.error('profileContent не найден');
        return;
    }

    profileContent.innerHTML = '<p>Загрузка...</p>';
    openModal('profileModal');

    try {
        const headers = getAuthHeaders();
        const response = await fetch('/api/auth/profile', { headers });

        if (response.ok) {
            const profile = await response.json();
            displayProfile(profile);
        } else {
            displayBasicProfile();
        }
    } catch (error) {
        console.error('Ошибка загрузки профиля:', error);
        displayBasicProfile();
    }
}

function displayBasicProfile() {
    const profileContent = document.getElementById('profileContent');
    if (!currentUser) return;

    if (currentUser.role === 'client') {
        profileContent.innerHTML = `
            <div class="profile-info">
                <p><strong>Имя:</strong> ${currentUser.name || ''}</p>
                <p><strong>Роль:</strong> Клиент</p>
                <p><strong>ID:</strong> ${currentUser.id || ''}</p>
                <p><strong>Уровень:</strong> ${currentUser.levelOfTraining || 'Новичок'}</p>
                <p><strong>Баланс:</strong> ${currentUser.balance || 0} ₽</p>
            </div>
        `;
    } else if (currentUser.role === 'coach') {
        profileContent.innerHTML = `
            <div class="profile-info">
                <p><strong>Имя:</strong> ${currentUser.name || ''}</p>
                <p><strong>Роль:</strong> Тренер</p>
                <p><strong>ID:</strong> ${currentUser.id || ''}</p>
                <p><strong>Квалификация:</strong> ${currentUser.qualification || '—'}</p>
            </div>
        `;
    } else if (currentUser.role === 'admin') {
        profileContent.innerHTML = `
            <div class="profile-info">
                <p><strong>Роль:</strong> Администратор</p>
                <p><strong>Имя:</strong> ${currentUser.name || 'Администратор'}</p>
                <p>У вас полный доступ к системе</p>
            </div>
        `;
    }
}

function displayProfile(profile) {
    const profileContent = document.getElementById('profileContent');
    profileContent.innerHTML = `
        <div class="profile-info">
            <p><strong>ФИО:</strong> ${profile.surname || ''} ${profile.name || ''} ${profile.patronymic || ''}</p>
            <p><strong>Телефон:</strong> ${profile.phone || ''}</p>
            <p><strong>Email:</strong> ${profile.email || '—'}</p>
            <p><strong>Роль:</strong> ${profile.role || currentUser.role}</p>
        </div>
    `;
}

function getAuthHeaders() {
    const headers = {
        'Content-Type': 'application/json'
    };

    if (currentUser && currentUser.token) {
        headers['Authorization'] = `Bearer ${currentUser.token}`;
    }

    return headers;
}

function showMyLessons() {
    if (!currentUser) {
        alert('Необходимо войти в систему');
        openModal('loginModal');
        return;
    }
    const content = document.getElementById('content');
    content.innerHTML = '<h2>Мои занятия</h2><p>Функция в разработке</p><br><button class="btn btn-secondary" onclick="showHome()">На главную</button>';
}

function showMyMemberships() {
    if (!currentUser) {
        alert('Необходимо войти в систему');
        openModal('loginModal');
        return;
    }
    const content = document.getElementById('content');
    content.innerHTML = '<h2>Мои абонементы</h2><p>Функция в разработке</p><br><button class="btn btn-secondary" onclick="showHome()">На главную</button>';
}