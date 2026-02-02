-- Проверка на существование БД 
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'Equestrian_Club')
BEGIN
    CREATE DATABASE Equestrian_Club;
END
GO

-- Текущая база данных
USE Equestrian_Club;
GO

--Удаление внешнего ключа FK_Payment_Membership
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Payment_Membership')
BEGIN
    ALTER TABLE Payment DROP CONSTRAINT FK_Payment_Membership;
END

--Удаление внешнего ключа FK_Payment_Lesson
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Payment_Lesson')
BEGIN
    ALTER TABLE Payment DROP CONSTRAINT FK_Payment_Lesson;
END

--Удаление внешнего ключа FK_Lesson_Coach
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Lesson_Coach')
BEGIN
    ALTER TABLE Lesson DROP CONSTRAINT FK_Lesson_Coach;
END

--Удаление внешнего ключа FK_Lesson_Client
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Lesson_Client')
BEGIN
    ALTER TABLE Lesson DROP CONSTRAINT FK_Lesson_Client;
END

--Удаление внешнего ключа FK_Lesson_Horse
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Lesson_Horse')
BEGIN
    ALTER TABLE Lesson DROP CONSTRAINT FK_Lesson_Horse;
END

--Удаление внешнего ключа FK_Membership_Client
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Membership_Client')
BEGIN
    ALTER TABLE Membership DROP CONSTRAINT FK_Membership_Client;
END

--Удаление внешнего ключа FK_Coach_Employee
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Coach_Employee')
BEGIN
    ALTER TABLE Coach DROP CONSTRAINT FK_Coach_Employee;
END

--Удаление внешнего ключа FK_Horse_Stall
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Horse_Stall')
BEGIN
    ALTER TABLE Horse DROP CONSTRAINT FK_Horse_Stall;
END

--Удаление внешнего ключа FK_Stall_Employee
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Stall_Employee')
BEGIN
    ALTER TABLE Stall DROP CONSTRAINT FK_Stall_Employee;
END
GO

-- Удаление таблиц в правильном порядке
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Payment')
    DROP TABLE Payment;
GO

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Lesson')
    DROP TABLE Lesson;
GO

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Membership')
    DROP TABLE Membership;
GO

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Coach')
    DROP TABLE Coach;
GO

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Horse')
    DROP TABLE Horse;
GO

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Stall')
    DROP TABLE Stall;
GO

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Client')
    DROP TABLE Client;
GO

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Employee')
    DROP TABLE Employee;
GO

-- Создание таблицы Employee (Сотрудник)
CREATE TABLE Employee
(
    ID_Employee INT IDENTITY(1,1) PRIMARY KEY,
    Surname NVARCHAR(50) NOT NULL,
    [Name] NVARCHAR(50) NOT NULL,
    Patronymic NVARCHAR(50) NULL,
    Date_of_birth DATE NOT NULL,
    City NVARCHAR(50) NOT NULL,
    Street NVARCHAR(50) NOT NULL,
    House_number NVARCHAR(10) NOT NULL,
    Flat_number NVARCHAR(10) NULL,
    Post NVARCHAR(50) NOT NULL CHECK (Post IN ('Конюх', 'Ветеринар', 'Администратор', 'Уборщик', 'Менеджер', 'Тренер')),
    Phone NVARCHAR(20) NULL,
    Hire_date DATE NOT NULL DEFAULT GETDATE()
);
GO

-- Создание таблицы Client (Клиент)
CREATE TABLE Client
(
    ID_Client INT IDENTITY(1,1) PRIMARY KEY,
    Surname NVARCHAR(50) NOT NULL,
    [Name] NVARCHAR(50) NOT NULL,
    Patronymic NVARCHAR(50) NULL,
    Date_of_birth DATE NOT NULL,
    Phone NVARCHAR(20) NOT NULL,
    Level_of_training NVARCHAR(20) NOT NULL DEFAULT 'Новичок' 
        CHECK (Level_of_training IN ('Новичок', 'Любитель', 'Спортсмен', 'Профессионал')),
    Passport NVARCHAR(20) NOT NULL,
    City NVARCHAR(50) NOT NULL,
    Street NVARCHAR(50) NOT NULL,
    House NVARCHAR(10) NOT NULL,
    Flat NVARCHAR(10) NULL,
    Balance DECIMAL(10,2) NOT NULL DEFAULT 0.00 CHECK (Balance >= 0),
    Date_of_registration DATE NOT NULL DEFAULT GETDATE(),
    CONSTRAINT CHK_Client_Age CHECK (DATEDIFF(YEAR, Date_of_birth, GETDATE()) >= 5),
    CONSTRAINT UQ_Client_Phone UNIQUE (Phone),
    CONSTRAINT UQ_Client_Passport UNIQUE (Passport)
);
GO

-- Создание таблицы Stall (Денник)
CREATE TABLE Stall
(
    ID_Stall INT IDENTITY(1,1) PRIMARY KEY,
    [Number] NVARCHAR(10) NOT NULL,
    [Type] NVARCHAR(20) NOT NULL CHECK ([Type] IN ('Стандартный', 'Большой', 'Для пони', 'Изолятор')),
    Last_cleaning_date DATE NOT NULL DEFAULT GETDATE(),
    Size DECIMAL(5,2) NOT NULL CHECK (Size BETWEEN 9 AND 25),
    Status NVARCHAR(20) NOT NULL DEFAULT 'Свободен' 
        CHECK (Status IN ('Свободен', 'Занят', 'На уборке', 'На ремонте', 'Забронирован')),
    ID_Employee INT NULL,
    CONSTRAINT UQ_Stall_Number UNIQUE ([Number]),
    CONSTRAINT CHK_Stall_Size_Type CHECK (
        ([Type] = 'Для пони' AND Size BETWEEN 9 AND 12) OR
        ([Type] = 'Стандартный' AND Size BETWEEN 12 AND 16) OR
        ([Type] = 'Большой' AND Size BETWEEN 16 AND 20) OR
        ([Type] = 'Изолятор' AND Size BETWEEN 10 AND 25)
    )
);
GO

-- Создание таблицы Horse (Лошадь)
CREATE TABLE Horse
(
    ID_Horse INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(50) NOT NULL,
    Gender NVARCHAR(10) NOT NULL CHECK (Gender IN ('Жеребец', 'Кобыла', 'Мерин')),
    Breed NVARCHAR(50) NOT NULL,
    Date_of_birth DATE NOT NULL,
    State_of_health NVARCHAR(20) NOT NULL DEFAULT 'Здорова' 
        CHECK (State_of_health IN ('Здорова', 'Больна', 'Травмирована', 'Восстанавливается')),
    Level_of_training NVARCHAR(20) NOT NULL DEFAULT 'Начинающий' 
        CHECK (Level_of_training IN ('Начинающий', 'Продвинутый', 'Спортивный', 'Профессиональный')),
    Passport NVARCHAR(50) NOT NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'В работе' 
        CHECK (Status IN ('В работе', 'На отдыхе', 'На лечении', 'Списана', 'На карантине')),
    ID_Stall INT NULL,
    CONSTRAINT CHK_Horse_Age CHECK (DATEDIFF(YEAR, Date_of_birth, GETDATE()) BETWEEN 3 AND 35),
    CONSTRAINT UQ_Horse_Passport UNIQUE (Passport),
    CONSTRAINT CHK_Horse_Health_Status CHECK (
        NOT (State_of_health IN ('Больна', 'Травмирована') AND Status = 'В работе')
    )
);
GO

-- Создание таблицы Coach (Тренер)
CREATE TABLE Coach
(
    ID_Coach INT IDENTITY(1,1) PRIMARY KEY,
    Surname NVARCHAR(50) NOT NULL,
    [Name] NVARCHAR(50) NOT NULL,
    Patronymic NVARCHAR(50) NULL,
    Date_of_birth DATE NOT NULL,
    Telephone NVARCHAR(20) NOT NULL,
    Qualification NVARCHAR(100) NOT NULL,
    Specialization NVARCHAR(100) NOT NULL 
        CHECK (Specialization IN ('Конкур', 'Выездка', 'Детская группа', 'Вольтижировка', 'Прогулки')),
    ID_Employee INT NULL,
    CONSTRAINT UQ_Coach_Telephone UNIQUE (Telephone),
    CONSTRAINT CHK_Coach_Age CHECK (DATEDIFF(YEAR, Date_of_birth, GETDATE()) >= 18)
);
GO

-- Создание таблицы Membership (Абонемент)
CREATE TABLE Membership
(
    ID_Membership INT IDENTITY(1,1) PRIMARY KEY,
    [Type] NVARCHAR(30) NOT NULL CHECK ([Type] IN ('Разовый', 'Пакет 5', 'Пакет 10', 'Безлимитный месяц', 'Безлимитный квартал')),
    Lessons_Total INT NOT NULL CHECK (Lessons_Total >= 0),
    Lessons_Used INT NOT NULL DEFAULT 0 CHECK (Lessons_Used >= 0),
    Valid_From DATE NOT NULL DEFAULT GETDATE(),
    Valid_Until DATE NOT NULL,
    Price DECIMAL(10,2) NOT NULL CHECK (Price > 0),
    Purchase_Date DATE NOT NULL DEFAULT GETDATE(),
    Status NVARCHAR(20) NOT NULL DEFAULT 'Активен' 
        CHECK (Status IN ('Активен', 'Использован', 'Просрочен', 'Отменен')),
    ID_Client INT NOT NULL,
    CONSTRAINT CHK_Membership_Dates CHECK (Valid_Until > Valid_From),
    CONSTRAINT CHK_Membership_Lessons CHECK (Lessons_Used <= Lessons_Total),
    CONSTRAINT CHK_Membership_Type_Logic CHECK (
        ([Type] = 'Разовый' AND Lessons_Total = 1) OR
        ([Type] = 'Пакет 5' AND Lessons_Total = 5) OR
        ([Type] = 'Пакет 10' AND Lessons_Total = 10) OR
        ([Type] IN ('Безлимитный месяц', 'Безлимитный квартал') AND Lessons_Total = 999)
    )
);
GO

-- Создание таблицы Lesson (Занятие)
CREATE TABLE Lesson
(
    ID_Lesson INT IDENTITY(1,1) PRIMARY KEY,
    [Date] DATE NOT NULL,
    [Type] NVARCHAR(20) NOT NULL CHECK ([Type] IN ('Индивидуальное', 'Групповое', 'Прогулка', 'Тренировка')),
    Start_time TIME NOT NULL,
    End_time TIME NOT NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Запланировано' 
        CHECK (Status IN ('Запланировано', 'Подтверждено', 'В процессе', 'Завершено', 'Отменено', 'Неявка')),
    Price DECIMAL(10,2) NOT NULL CHECK (Price >= 0),
    Payment_type NVARCHAR(20) NULL 
        CHECK (Payment_type IN ('Абонемент', 'Разовая оплата', 'Пробное', 'Компенсация')),
	ID_Horse INT NULL,
    ID_Client INT NOT NULL,
    ID_Coach INT NOT NULL,
    CONSTRAINT CHK_Lesson_Time CHECK (End_time > Start_time),
    CONSTRAINT CHK_Lesson_Duration CHECK (DATEDIFF(MINUTE, Start_time, End_time) BETWEEN 30 AND 240),
    CONSTRAINT CHK_Lesson_Date CHECK ([Date] >= CAST(GETDATE() AS DATE))
);
GO

-- Создание таблицы Payment (Оплата)
CREATE TABLE Payment
(
    ID_Payment INT IDENTITY(1,1) PRIMARY KEY,
    Method_paid NVARCHAR(20) NOT NULL CHECK (Method_paid IN ('Наличные', 'Карта', 'Перевод', 'Онлайн')),
    Purpose_of_the_payment NVARCHAR(50) NOT NULL 
        CHECK (Purpose_of_the_payment IN ('Абонемент', 'Разовое занятие', 'Доп. услуга', 'Штраф', 'Возврат')),
    Summa DECIMAL(10,2) NOT NULL CHECK (Summa != 0),
    Payment_date DATETIME NOT NULL DEFAULT GETDATE(),
    Status NVARCHAR(20) NOT NULL DEFAULT 'Завершено' 
        CHECK (Status IN ('Ожидает', 'Завершено', 'Отклонено', 'Возвращено')),
    ID_Lesson INT NULL,
    ID_Membership INT NULL,
    CONSTRAINT CHK_Payment_Reference CHECK (
        ID_Lesson IS NOT NULL OR 
        ID_Membership IS NOT NULL OR
        Purpose_of_the_payment IN ('Доп. услуга', 'Штраф', 'Возврат')
    ),
    CONSTRAINT CHK_Payment_Single_Reference CHECK (
        (ID_Lesson IS NULL AND ID_Membership IS NULL) OR
        (ID_Lesson IS NULL AND ID_Membership IS NOT NULL) OR
        (ID_Lesson IS NOT NULL AND ID_Membership IS NULL)
    )
);
GO

-- Создание внешнего ключа Horse-Stall 
ALTER TABLE Horse 
ADD CONSTRAINT FK_Horse_Stall 
FOREIGN KEY (ID_Stall) REFERENCES Stall(ID_Stall);
GO

-- Создание внешнего ключа Coach-Employee 
ALTER TABLE Coach 
ADD CONSTRAINT FK_Coach_Employee 
FOREIGN KEY (ID_Employee) REFERENCES Employee(ID_Employee);
GO

-- Создание внешнего ключа Membership-Client 
ALTER TABLE Membership 
ADD CONSTRAINT FK_Membership_Client 
FOREIGN KEY (ID_Client) REFERENCES Client(ID_Client);
GO

-- Создание внешнего ключа Lesson-Horse 
ALTER TABLE Lesson 
ADD CONSTRAINT FK_Lesson_Horse 
FOREIGN KEY (ID_Horse) REFERENCES Horse(ID_Horse);
GO

-- Создание внешнего ключа Lesson-Client 
ALTER TABLE Lesson 
ADD CONSTRAINT FK_Lesson_Client 
FOREIGN KEY (ID_Client) REFERENCES Client(ID_Client);
GO

-- Создание внешнего ключа Lesson-Coach
ALTER TABLE Lesson 
ADD CONSTRAINT FK_Lesson_Coach 
FOREIGN KEY (ID_Coach) REFERENCES Coach(ID_Coach);
GO

-- Создание внешнего ключа Payment-Lesson 
ALTER TABLE Payment 
ADD CONSTRAINT FK_Payment_Lesson 
FOREIGN KEY (ID_Lesson) REFERENCES Lesson(ID_Lesson);
GO

-- Создание внешнего ключа Payment-Membership 
ALTER TABLE Payment 
ADD CONSTRAINT FK_Payment_Membership 
FOREIGN KEY (ID_Membership) REFERENCES Membership(ID_Membership);
GO

-- Создание внешнего ключа Stall-Employee
ALTER TABLE Stall 
ADD CONSTRAINT FK_Stall_Employee 
FOREIGN KEY (ID_Employee) REFERENCES Employee(ID_Employee);
GO
