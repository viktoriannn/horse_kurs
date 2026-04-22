// auth.js - Управление авторизацией

// Текущий пользователь
let currentUser = null;

// Получить текущего пользователя
function getCurrentUser() {
    const saved = localStorage.getItem('currentUser');
    if (saved) {
        currentUser = JSON.parse(saved);
    }
    return currentUser;
}

// Сохранить пользователя
function setCurrentUser(user) {
    currentUser = user;
    localStorage.setItem('currentUser', JSON.stringify(user));
    if (user.token) {
        setAuthToken(user.token);
    }
}

// Очистить данные пользователя
function clearCurrentUser() {
    currentUser = null;
    localStorage.removeItem('currentUser');
    localStorage.removeItem('token');
}

// Проверить авторизацию
function checkAuth() {
    const user = getCurrentUser();
    if (!user) {
        window.location.href = '/index.html';
        return false;
    }
    return true;
}

// Проверить роль
function checkRole(allowedRoles) {
    const user = getCurrentUser();
    if (!user || !allowedRoles.includes(user.role)) {
        window.location.href = '/index.html';
        return false;
    }
    return true;
}

// Выход
function logout() {
    clearCurrentUser();
    window.location.href = '/index.html';
}   