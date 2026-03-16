-- Проверка на существование БД 
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'Equestrian_Club')
BEGIN
    CREATE DATABASE Equestrian_Club;
END
GO

-- Текущая база данных
USE Equestrian_Club;
GO

-- Удаление внешних ключей
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Schedule_Arena_Competition')
    ALTER TABLE Schedule_Arena DROP CONSTRAINT FK_Schedule_Arena_Competition;
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Schedule_Arena_Lesson')
    ALTER TABLE Schedule_Arena DROP CONSTRAINT FK_Schedule_Arena_Lesson;
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Schedule_Arena')
    ALTER TABLE Schedule_Arena DROP CONSTRAINT FK_Schedule_Arena;
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Participation_Horse')
    ALTER TABLE Participation DROP CONSTRAINT FK_Participation_Horse;
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Participation_Client')
    ALTER TABLE Participation DROP CONSTRAINT FK_Participation_Client;
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Participation_Competition')
    ALTER TABLE Participation DROP CONSTRAINT FK_Participation_Competition;
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Competition_Arena')
    ALTER TABLE Competition DROP CONSTRAINT FK_Competition_Arena;
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Lesson_Arena')
    ALTER TABLE Lesson DROP CONSTRAINT FK_Lesson_Arena;
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Payment_Competition')
    ALTER TABLE Payment DROP CONSTRAINT FK_Payment_Competition;
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Payment_Membership')
    ALTER TABLE Payment DROP CONSTRAINT FK_Payment_Membership;
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Payment_Lesson')
    ALTER TABLE Payment DROP CONSTRAINT FK_Payment_Lesson;
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Lesson_Coach')
    ALTER TABLE Lesson DROP CONSTRAINT FK_Lesson_Coach;
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Lesson_Client')
    ALTER TABLE Lesson DROP CONSTRAINT FK_Lesson_Client;
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Lesson_Horse')
    ALTER TABLE Lesson DROP CONSTRAINT FK_Lesson_Horse;
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Membership_Client')
    ALTER TABLE Membership DROP CONSTRAINT FK_Membership_Client;
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Coach_Employee')
    ALTER TABLE Coach DROP CONSTRAINT FK_Coach_Employee;
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Horse_Stall')
    ALTER TABLE Horse DROP CONSTRAINT FK_Horse_Stall;
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Stall_Employee')
    ALTER TABLE Stall DROP CONSTRAINT FK_Stall_Employee;
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_MembershipLesson_Membership')
    ALTER TABLE Membership_Lesson DROP CONSTRAINT FK_MembershipLesson_Membership;
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_MembershipLesson_Lesson')
    ALTER TABLE Membership_Lesson DROP CONSTRAINT FK_MembershipLesson_Lesson;
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_LessonHorse_Lesson')
    ALTER TABLE Lesson_Horse DROP CONSTRAINT FK_LessonHorse_Lesson;
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_LessonHorse_Horse')
    ALTER TABLE Lesson_Horse DROP CONSTRAINT FK_LessonHorse_Horse;
GO

-- Удаление таблиц
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Lesson_Horse')
    DROP TABLE Lesson_Horse;
GO
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Schedule_Arena')
    DROP TABLE Schedule_Arena;
GO
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Participation')
    DROP TABLE Participation;
GO
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Competition')
    DROP TABLE Competition;
GO
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Arena')
    DROP TABLE Arena;
GO
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Payment')
    DROP TABLE Payment;
GO
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Membership_Lesson')
    DROP TABLE Membership_Lesson;
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

-- Создание таблицы Employee
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
    Phone NVARCHAR(20) NULL
);
GO

-- Создание таблицы Client
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

-- Создание таблицы Stall
CREATE TABLE Stall
(
    ID_Stall INT IDENTITY(1,1) PRIMARY KEY,
    [Number] NVARCHAR(10) NOT NULL,
    [Type] NVARCHAR(20) NOT NULL CHECK ([Type] IN ('Стандартный', 'Большой', 'Для пони', 'Изолятор')),
    Size DECIMAL(5,2) NOT NULL CHECK (Size BETWEEN 9 AND 25),
    [Status] NVARCHAR(20) NOT NULL DEFAULT 'Свободен' 
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

-- Создание таблицы Horse
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
    [Status] NVARCHAR(20) NOT NULL DEFAULT 'В работе' 
        CHECK (Status IN ('В работе', 'На отдыхе', 'На лечении', 'Списана', 'На карантине')),
    ID_Stall INT NULL,
    CONSTRAINT CHK_Horse_Age CHECK (DATEDIFF(YEAR, Date_of_birth, GETDATE()) BETWEEN 3 AND 35),
    CONSTRAINT UQ_Horse_Passport UNIQUE (Passport),
    CONSTRAINT CHK_Horse_Health_Status CHECK (
        NOT (State_of_health IN ('Больна', 'Травмирована') AND Status = 'В работе')
    )
);
GO

-- Создание таблицы Coach
CREATE TABLE Coach
(
    ID_Coach INT IDENTITY(1,1) PRIMARY KEY,
    Qualification NVARCHAR(100) NOT NULL,
    Specialization NVARCHAR(100) NOT NULL 
        CHECK (Specialization IN ('Конкур', 'Выездка', 'Детская группа', 'Вольтижировка', 'Прогулки')),
    ID_Employee INT NULL,
);
GO

-- Создание таблицы Arena
CREATE TABLE Arena
(
    ID_Arena INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(50) NOT NULL,
    [Type] NVARCHAR(30) NOT NULL CHECK ([Type] IN ('Конкурный', 'Выездковый', 'Универсальный', 'Прогулочный', 'Крытый', 'Открытый')),
    Coverage NVARCHAR(30) NOT NULL CHECK (Coverage IN ('Песок', 'Резиновая крошка', 'Трава', 'Грунт', 'Смешанный')),
    [Length] DECIMAL(5,2) NOT NULL CHECK (Length > 0),
    Width DECIMAL(5,2) NOT NULL CHECK (Width > 0),
    [Status] NVARCHAR(20) NOT NULL DEFAULT 'Доступен' 
        CHECK (Status IN ('Доступен', 'На обслуживании', 'Закрыт', 'Ремонт')),
    CONSTRAINT CHK_Arena_Size CHECK ([Length] >= 20 AND Width >= 10)
);
GO

-- Создание таблицы Membership
CREATE TABLE Membership
(
    ID_Membership INT IDENTITY(1,1) PRIMARY KEY,
    [Type] NVARCHAR(30) NOT NULL,
    Lessons_Total INT NOT NULL CHECK (Lessons_Total >= 0),
    Valid_From DATE NOT NULL DEFAULT GETDATE(),
    Valid_Until DATE NOT NULL,
    Price DECIMAL(10,2) NOT NULL CHECK (Price > 0),
    Purchase_Date DATE NOT NULL DEFAULT GETDATE(),
    [Status] NVARCHAR(20) NOT NULL DEFAULT 'Активен' 
        CHECK (Status IN ('Активен', 'Использован', 'Просрочен', 'Отменен')),
    ID_Client INT NOT NULL,
    CONSTRAINT CHK_Membership_Dates CHECK (Valid_Until > Valid_From)
);
GO

-- Создание таблицы Competition
CREATE TABLE Competition
(
    ID_Competition INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(50) NOT NULL,
    [Date] DATE NOT NULL,
    [Type] NVARCHAR(30) NOT NULL CHECK ([Type] IN ('Конкур', 'Выездка', 'Троеборье', 'Пробеги', 'Вольтижировка', 'Драйвинг')),
    [Level] NVARCHAR(30) NOT NULL CHECK ([Level] IN ('Клубные', 'Любительские', 'Региональные', 'Национальные', 'Международные')),
    [Status] NVARCHAR(20) NOT NULL DEFAULT 'Запланировано' 
        CHECK (Status IN ('Запланировано', 'Регистрация', 'Идет', 'Завершено', 'Отменено')),
	ID_Arena INT NOT NULL,
);
GO

-- Создание таблицы Lesson
CREATE TABLE Lesson
(
    ID_Lesson INT IDENTITY(1,1) PRIMARY KEY,
    [Date] DATE NOT NULL,
    [Type] NVARCHAR(30) NOT NULL CHECK ([Type] IN ('Индивидуальное', 'Групповое', 'Прогулка', 'Тренировка', 'Подготовка к соревнованиям')),
    ID_Client INT NOT NULL,
    ID_Coach INT NOT NULL,
    ID_Arena INT NULL,
    CONSTRAINT CHK_Lesson_Date CHECK ([Date] >= CAST(GETDATE() AS DATE))
);
GO

-- Создание таблицы Membership_Lesson
CREATE TABLE Membership_Lesson
(
    ID_Membership_Lesson INT IDENTITY(1,1) PRIMARY KEY,
    Price DECIMAL(10,2) NOT NULL CHECK (Price > 0),
	ID_Membership INT NOT NULL,
    ID_Lesson INT NOT NULL
);
GO

-- Создание таблицы Payment
CREATE TABLE Payment
(
    ID_Payment INT IDENTITY(1,1) PRIMARY KEY,
    Method_paid NVARCHAR(20) NOT NULL CHECK (Method_paid IN ('Наличные', 'Карта', 'Перевод', 'Онлайн')),
    Summa DECIMAL(10,2) NOT NULL CHECK (Summa != 0),
    Payment_date DATETIME NOT NULL DEFAULT GETDATE(),
    [Status] NVARCHAR(20) NOT NULL DEFAULT 'Завершено' 
        CHECK (Status IN ('Ожидает', 'Завершено', 'Отклонено', 'Возвращено')),
    ID_Lesson INT NULL,
    ID_Membership INT NULL,
    ID_Competition INT NULL,
    CONSTRAINT CHK_Payment_Single_Reference CHECK (
        (ID_Lesson IS NOT NULL AND ID_Membership IS NULL AND ID_Competition IS NULL) OR
        (ID_Lesson IS NULL AND ID_Membership IS NOT NULL AND ID_Competition IS NULL) OR
        (ID_Lesson IS NULL AND ID_Membership IS NULL AND ID_Competition IS NOT NULL)
    )
);
GO

-- Создание таблицы Participation
CREATE TABLE Participation
(
    ID_Participation INT IDENTITY(1,1) PRIMARY KEY,
	Registration_date DATE NOT NULL DEFAULT GETDATE(),
    Start_number INT NULL,
    Result_place INT NULL,
    Score DECIMAL(5,2) NULL,
    ID_Competition INT NOT NULL,
    ID_Client INT NOT NULL,
    ID_Horse INT NOT NULL,
    CONSTRAINT UQ_Participation_StartNumber UNIQUE (ID_Competition, Start_number),
    CONSTRAINT CHK_Result_Place CHECK (Result_place > 0)
);
GO

-- Создание таблицы Schedule_Arena
CREATE TABLE Schedule_Arena
(
    ID_Schedule INT IDENTITY(1,1) PRIMARY KEY,
    ID_Arena INT NOT NULL,
    [Date] DATE NOT NULL,
    Start_time TIME NOT NULL,
    End_time TIME NOT NULL,
	[Status] NVARCHAR(20) NOT NULL DEFAULT 'Запланировано' 
        CHECK ([Status] IN ('Запланировано', 'В процессе', 'Завершено', 'Отменено')),
    ID_Lesson INT NULL,
    ID_Competition INT NULL,
    CONSTRAINT CHK_Schedule_Time CHECK (End_time > Start_time),
    CONSTRAINT CHK_Schedule_Reference CHECK (
        ID_Lesson IS NOT NULL OR 
        ID_Competition IS NOT NULL
    )
);
GO

-- Создание таблицы Lesson_Horse
CREATE TABLE Lesson_Horse
(
    ID_Lesson_Horse INT IDENTITY(1,1) PRIMARY KEY,
    ID_Lesson INT NOT NULL,
    ID_Horse INT NOT NULL
);
GO

-- Создание внешнего ключа Stall-Employee
ALTER TABLE Stall 
ADD CONSTRAINT FK_Stall_Employee 
FOREIGN KEY (ID_Employee) REFERENCES Employee(ID_Employee);
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

-- Создание внешнего ключа Competition-Arena
ALTER TABLE Competition 
ADD CONSTRAINT FK_Competition_Arena 
FOREIGN KEY (ID_Arena) REFERENCES Arena(ID_Arena);
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

-- Создание внешнего ключа Lesson-Arena
ALTER TABLE Lesson 
ADD CONSTRAINT FK_Lesson_Arena 
FOREIGN KEY (ID_Arena) REFERENCES Arena(ID_Arena);
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

-- Создание внешнего ключа Payment-Competition
ALTER TABLE Payment 
ADD CONSTRAINT FK_Payment_Competition 
FOREIGN KEY (ID_Competition) REFERENCES Competition(ID_Competition);
GO

-- Создание внешнего ключа Participation-Competition
ALTER TABLE Participation 
ADD CONSTRAINT FK_Participation_Competition 
FOREIGN KEY (ID_Competition) REFERENCES Competition(ID_Competition);
GO

-- Создание внешнего ключа Participation-Client
ALTER TABLE Participation 
ADD CONSTRAINT FK_Participation_Client 
FOREIGN KEY (ID_Client) REFERENCES Client(ID_Client);
GO

-- Создание внешнего ключа Participation-Horse
ALTER TABLE Participation 
ADD CONSTRAINT FK_Participation_Horse 
FOREIGN KEY (ID_Horse) REFERENCES Horse(ID_Horse);
GO

-- Создание внешнего ключа Schedule_Arena-Arena
ALTER TABLE Schedule_Arena 
ADD CONSTRAINT FK_Schedule_Arena 
FOREIGN KEY (ID_Arena) REFERENCES Arena(ID_Arena);
GO

-- Создание внешнего ключа Schedule_Arena-Lesson
ALTER TABLE Schedule_Arena 
ADD CONSTRAINT FK_Schedule_Lesson 
FOREIGN KEY (ID_Lesson) REFERENCES Lesson(ID_Lesson);
GO

-- Создание внешнего ключа Schedule_Arena-Competition
ALTER TABLE Schedule_Arena 
ADD CONSTRAINT FK_Schedule_Competition 
FOREIGN KEY (ID_Competition) REFERENCES Competition(ID_Competition);
GO

-- Создание внешнего ключа Membership_Lesson-Membership 
ALTER TABLE Membership_Lesson
ADD CONSTRAINT FK_MembershipLesson_Membership 
FOREIGN KEY (ID_Membership) REFERENCES Membership(ID_Membership);
GO

-- Создание внешнего ключа Membership_Lesson-Lesson
ALTER TABLE Membership_Lesson
ADD CONSTRAINT FK_MembershipLesson_Lesson 
FOREIGN KEY (ID_Lesson) REFERENCES Lesson(ID_Lesson);
GO

-- Создание внешнего ключа Lesson_Horse-Lesson
ALTER TABLE Lesson_Horse
ADD  CONSTRAINT FK_LessonHorse_Lesson 
FOREIGN KEY (ID_Lesson) REFERENCES Lesson(ID_Lesson);
GO

-- Создание внешнего ключа Lesson_Horse-Horse 
ALTER TABLE Lesson_Horse
ADD CONSTRAINT FK_LessonHorse_Horse 
FOREIGN KEY (ID_Horse) REFERENCES Horse(ID_Horse);
GO

USE Equestrian_Club;
GO

-- Очистка существующих данных (если нужно перезаполнить)
DELETE FROM Lesson_Horse;
DELETE FROM Schedule_Arena;
DELETE FROM Participation;
DELETE FROM Payment;
DELETE FROM Membership_Lesson;
DELETE FROM Lesson;
DELETE FROM Competition;
DELETE FROM Membership;
DELETE FROM Coach;
DELETE FROM Horse;
DELETE FROM Arena;
DELETE FROM Stall;
DELETE FROM Client;
DELETE FROM Employee;
GO

-- Сброс счетчиков identity
DBCC CHECKIDENT ('Employee', RESEED, 0);
DBCC CHECKIDENT ('Client', RESEED, 0);
DBCC CHECKIDENT ('Stall', RESEED, 0);
DBCC CHECKIDENT ('Horse', RESEED, 0);
DBCC CHECKIDENT ('Coach', RESEED, 0);
DBCC CHECKIDENT ('Arena', RESEED, 0);
DBCC CHECKIDENT ('Membership', RESEED, 0);
DBCC CHECKIDENT ('Competition', RESEED, 0);
DBCC CHECKIDENT ('Lesson', RESEED, 0);
DBCC CHECKIDENT ('Payment', RESEED, 0);
DBCC CHECKIDENT ('Participation', RESEED, 0);
DBCC CHECKIDENT ('Schedule_Arena', RESEED, 0);
DBCC CHECKIDENT ('Membership_Lesson', RESEED, 0);
DBCC CHECKIDENT ('Lesson_Horse', RESEED, 0);
GO

-- 1. Заполнение таблицы Employee (Сотрудники)
INSERT INTO Employee (Surname, [Name], Patronymic, Date_of_birth, City, Street, House_number, Flat_number, Post, Phone)
VALUES 
('Иванов', 'Иван', 'Иванович', '1985-03-15', 'Москва', 'Ленина', '10', '25', 'Тренер', '+7 (495) 123-45-67'),
('Петрова', 'Елена', 'Сергеевна', '1990-07-22', 'Москва', 'Пушкина', '5', '12', 'Тренер', '+7 (495) 234-56-78'),
('Сидоров', 'Алексей', 'Петрович', '1978-11-05', 'Москва', 'Гагарина', '15', NULL, 'Ветеринар', '+7 (495) 345-67-89'),
('Козлова', 'Наталья', 'Ивановна', '1988-09-18', 'Москва', 'Мира', '7', '34', 'Администратор', '+7 (495) 456-78-90'),
('Морозов', 'Дмитрий', 'Андреевич', '1982-04-30', 'Москва', 'Садовая', '22', '5', 'Конюх', '+7 (495) 567-89-01'),
('Волкова', 'Анна', 'Викторовна', '1992-12-12', 'Москва', 'Парковая', '3', '18', 'Конюх', '+7 (495) 678-90-12'),
('Соколов', 'Павел', 'Дмитриевич', '1975-06-25', 'Москва', 'Лесная', '8', NULL, 'Менеджер', '+7 (495) 789-01-23'),
('Михайлова', 'Ольга', 'Алексеевна', '1987-10-08', 'Москва', 'Цветочная', '12', '7', 'Уборщик', '+7 (495) 890-12-34'),
('Николаев', 'Сергей', 'Владимирович', '1983-02-14', 'Москва', 'Солнечная', '9', '42', 'Тренер', '+7 (495) 901-23-45'),
('Федорова', 'Татьяна', 'Николаевна', '1991-08-20', 'Москва', 'Березовая', '4', '15', 'Конюх', '+7 (495) 012-34-56');
GO

-- 2. Заполнение таблицы Client (Клиенты)
INSERT INTO Client (Surname, [Name], Patronymic, Date_of_birth, Phone, Level_of_training, Passport, City, Street, House, Flat, Balance, Date_of_registration)
VALUES
('Смирнов', 'Александр', 'Игоревич', '1995-05-10', '+7 (903) 111-22-33', 'Новичок', '4512 123456', 'Москва', 'Тверская', '15', '8', 5000.00, '2025-01-15'),
('Кузнецова', 'Мария', 'Дмитриевна', '1988-09-23', '+7 (903) 222-33-44', 'Любитель', '4512 234567', 'Москва', 'Арбат', '22', '12', 8500.00, '2025-11-20'),
('Попов', 'Денис', 'Алексеевич', '1992-12-01', '+7 (903) 333-44-55', 'Спортсмен', '4512 345678', 'Москва', 'Новый Арбат', '7', '3', 12000.00, '2025-09-10'),
('Васильева', 'Екатерина', 'Сергеевна', '1997-03-17', '+7 (903) 444-55-66', 'Новичок', '4512 456789', 'Москва', 'Красная', '5', '22', 3000.00, '2026-02-05'),
('Михайлов', 'Андрей', 'Петрович', '1985-07-29', '+7 (903) 555-66-77', 'Профессионал', '4512 567890', 'Москва', 'Ленинский', '45', '18', 25000.00, '2025-06-15'),
('Новикова', 'Анна', 'Владимировна', '1994-11-11', '+7 (903) 666-77-88', 'Любитель', '4512 678901', 'Москва', 'Вернадского', '33', '5', 7200.00, '2025-12-01'),
('Зайцев', 'Илья', 'Андреевич', '1990-04-04', '+7 (903) 777-88-99', 'Спортсмен', '4512 789012', 'Москва', 'Мичурина', '12', '9', 15000.00, '2025-10-10'),
('Морозова', 'Ольга', 'Ивановна', '1987-08-19', '+7 (903) 888-99-00', 'Профессионал', '4512 890123', 'Москва', 'Строителей', '8', '16', 22000.00, '2025-05-20'),
('Волков', 'Максим', 'Сергеевич', '1993-06-25', '+7 (903) 999-00-11', 'Новичок', '4512 901234', 'Москва', 'Молодежная', '17', '4', 1500.00, '2026-02-10'),
('Соколова', 'Дарья', 'Алексеевна', '1996-01-30', '+7 (903) 000-11-22', 'Любитель', '4512 012345', 'Москва', 'Победы', '25', '7', 6800.00, '2025-12-15');
GO

-- 3. Заполнение таблицы Stall (Денники)
INSERT INTO Stall ([Number], [Type], Size, [Status], ID_Employee)
VALUES
('A1', 'Стандартный', 14.5, 'Занят', 5),
('A2', 'Стандартный', 15.0, 'Занят', 5),
('A3', 'Стандартный', 14.0, 'Занят', 6),
('A4', 'Стандартный', 15.5, 'Занят', 6),
('B1', 'Большой', 18.0, 'Занят', 5),
('B2', 'Большой', 19.0, 'Занят', 6),
('C1', 'Для пони', 10.5, 'Свободен', NULL),
('C2', 'Для пони', 11.0, 'Свободен', NULL),
('D1', 'Изолятор', 16.0, 'Свободен', 5),
('E1', 'Стандартный', 14.0, 'На уборке', 5),
('E2', 'Стандартный', 14.5, 'Свободен', NULL),
('F1', 'Большой', 18.5, 'Занят', 6);
GO

-- 4. Заполнение таблицы Horse (Лошади)
INSERT INTO Horse ([Name], Gender, Breed, Date_of_birth, State_of_health, Level_of_training, Passport, [Status], ID_Stall)
VALUES
('Гром', 'Жеребец', 'Тракененская', '2018-04-15', 'Здорова', 'Спортивный', 'H12345', 'В работе', 1),
('Заря', 'Кобыла', 'Ганноверская', '2019-06-20', 'Здорова', 'Продвинутый', 'H12346', 'В работе', 2),
('Ветер', 'Мерин', 'Буденновская', '2017-03-10', 'Здорова', 'Профессиональный', 'H12347', 'В работе', 3),
('Роза', 'Кобыла', 'Арабская', '2020-08-05', 'Здорова', 'Начинающий', 'H12348', 'В работе', 4),
('Алмаз', 'Жеребец', 'Ахалтекинская', '2016-11-12', 'Здорова', 'Спортивный', 'H12349', 'В работе', 5),
('Сивка', 'Кобыла', 'Русская верховая', '2018-09-18', 'Здорова', 'Продвинутый', 'H12350', 'В работе', 6),
('Буран', 'Мерин', 'Владимирский тяжеловоз', '2015-12-03', 'Восстанавливается', 'Профессиональный', 'H12351', 'На отдыхе', 9),
('Ласка', 'Кобыла', 'Пони', '2021-05-22', 'Здорова', 'Начинающий', 'H12352', 'В работе', 7),
('Орлик', 'Жеребец', 'Орловский рысак', '2017-07-30', 'Здорова', 'Спортивный', 'H12353', 'В работе', 8),
('Ночка', 'Кобыла', 'Кабардинская', '2019-10-14', 'Здорова', 'Продвинутый', 'H12354', 'В работе', 12);
GO

-- 5. Заполнение таблицы Coach (Тренеры)
INSERT INTO Coach (Qualification, Specialization, ID_Employee)
VALUES
('Мастер спорта по конкуру', 'Конкур', 1),
('КМС по выездке', 'Выездка', 2),
('Тренер высшей категории', 'Детская группа', 9),
('Мастер спорта по вольтижировке', 'Вольтижировка', 4),
('Инструктор по верховой езде', 'Прогулки', 5);
GO

-- 6. Заполнение таблицы Arena (Манежи)
INSERT INTO Arena ([Name], [Type], Coverage, [Length], Width, [Status])
VALUES
('Большой конкурный', 'Конкурный', 'Песок', 60.0, 40.0, 'Доступен'),
('Малый конкурный', 'Конкурный', 'Песок', 40.0, 20.0, 'Доступен'),
('Выездковый', 'Выездковый', 'Резиновая крошка', 50.0, 30.0, 'Доступен'),
('Универсальный', 'Универсальный', 'Смешанный', 45.0, 25.0, 'Доступен'),
('Крытый манеж', 'Крытый', 'Резиновая крошка', 40.0, 20.0, 'Доступен'),
('Прогулочный', 'Прогулочный', 'Трава', 80.0, 50.0, 'Доступен'),
('Летний', 'Открытый', 'Грунт', 50.0, 30.0, 'На обслуживании');
GO

-- 7. Заполнение таблицы Membership (Абонементы)
INSERT INTO Membership ([Type], Lessons_Total, Valid_From, Valid_Until, Price, Purchase_Date, [Status], ID_Client)
VALUES
('Новичок', 4, '2026-02-01', '2026-04-01', 4000.00, '2026-02-01', 'Активен', 1),
('Любитель', 8, '2026-01-15', '2026-04-15', 7200.00, '2026-01-15', 'Активен', 2),
('Спортивный', 12, '2025-12-01', '2026-03-01', 10200.00, '2025-12-01', 'Активен', 3),
('Новичок', 4, '2026-02-10', '2026-04-10', 4000.00, '2026-02-10', 'Активен', 4),
('Профессиональный', 16, '2025-11-01', '2026-02-01', 14400.00, '2025-11-01', 'Использован', 5),
('Любитель', 8, '2026-01-20', '2026-04-20', 7200.00, '2026-01-20', 'Активен', 6),
('Спортивный', 12, '2025-10-15', '2026-01-15', 10200.00, '2025-10-15', 'Просрочен', 7),
('Профессиональный', 16, '2025-09-01', '2025-12-01', 14400.00, '2025-09-01', 'Использован', 8),
('Новичок', 4, '2026-02-15', '2026-04-15', 4000.00, '2026-02-15', 'Активен', 9),
('Любитель', 8, '2025-12-10', '2026-03-10', 7200.00, '2025-12-10', 'Активен', 10);
GO

-- 8. Заполнение таблицы Competition (Соревнования) - с датами после текущей
INSERT INTO Competition ([Name], [Date], [Type], [Level], [Status], ID_Arena)
VALUES
('Кубок Москвы по конкуру', '2026-04-15', 'Конкур', 'Региональные', 'Запланировано', 1),
('Весенний турнир по выездке', '2026-05-20', 'Выездка', 'Любительские', 'Запланировано', 3),
('Детские старты', '2026-03-10', 'Конкур', 'Клубные', 'Запланировано', 2),
('Открытый чемпионат клуба', '2026-06-05', 'Троеборье', 'Клубные', 'Регистрация', 4),
('Кубок России по вольтижировке', '2026-07-12', 'Вольтижировка', 'Национальные', 'Запланировано', 5),
('Зимний кубок', '2026-02-20', 'Конкур', 'Любительские', 'Завершено', 1),
('Новогодний турнир', '2025-12-25', 'Выездка', 'Клубные', 'Завершено', 3);
GO

-- 9. Заполнение таблицы Lesson (Занятия) - с датами в будущем относительно 2026-03-02
INSERT INTO Lesson ([Date], [Type], ID_Client, ID_Coach, ID_Arena)
VALUES
('2026-03-05', 'Индивидуальное', 1, 1, 1),
('2026-03-05', 'Групповое', 2, 2, 3),
('2026-03-06', 'Прогулка', 3, 3, 6),
('2026-03-06', 'Индивидуальное', 4, 1, 2),
('2026-03-07', 'Тренировка', 5, 4, 4),
('2026-03-07', 'Индивидуальное', 6, 2, 3),
('2026-03-08', 'Подготовка к соревнованиям', 7, 5, 1),
('2026-03-08', 'Групповое', 8, 3, 5),
('2026-03-09', 'Прогулка', 9, 4, 6),
('2026-03-09', 'Индивидуальное', 10, 1, 2),
('2026-03-10', 'Тренировка', 1, 2, 1),
('2026-03-10', 'Индивидуальное', 2, 3, 4);
GO

-- Проверка, что занятия создались
SELECT 'Lesson' AS TableName, COUNT(*) AS RowsCount FROM Lesson;
GO

-- 10. Заполнение таблицы Lesson_Horse (Лошади на занятиях)
INSERT INTO Lesson_Horse (ID_Lesson, ID_Horse)
VALUES
(1, 1), (1, 2),  -- Первое занятие, лошади 1 и 2
(2, 3), (2, 4),  -- Второе занятие, лошади 3 и 4
(3, 5),          -- Третье занятие, лошадь 5
(4, 6), (4, 7),  -- Четвертое занятие, лошади 6 и 7
(5, 8), (5, 9),  -- Пятое занятие, лошади 8 и 9
(6, 10),         -- Шестое занятие, лошадь 10
(7, 1), (7, 3),  -- Седьмое занятие, лошади 1 и 3
(8, 4), (8, 5),  -- Восьмое занятие, лошади 4 и 5
(9, 2),          -- Девятое занятие, лошадь 2
(10, 6), (10, 7), (10, 8),  -- Десятое занятие, лошади 6,7,8
(11, 9), (11, 10),  -- Одиннадцатое занятие, лошади 9 и 10
(12, 1), (12, 2);  -- Двенадцатое занятие, лошади 1 и 2
GO

-- 11. Заполнение таблицы Membership_Lesson (Оплата занятий по абонементу)
INSERT INTO Membership_Lesson (Price, ID_Membership, ID_Lesson)
VALUES
(1000.00, 1, 1),
(900.00, 2, 2),
(850.00, 3, 3),
(1000.00, 4, 4),
(900.00, 5, 5),
(900.00, 6, 6),
(850.00, 7, 7),
(900.00, 8, 8),
(1000.00, 9, 9),
(900.00, 10, 10),
(1000.00, 1, 11),
(900.00, 2, 12);
GO

-- 12. Заполнение таблицы Payment (Платежи) - исправлен формат дат
INSERT INTO Payment (Method_paid, Summa, Payment_date, [Status], ID_Lesson, ID_Membership, ID_Competition)
VALUES
('Карта', 4000.00, '2026-02-01', 'Завершено', NULL, 1, NULL),
('Наличные', 7200.00, '2026-01-15', 'Завершено', NULL, 2, NULL),
('Онлайн', 10200.00, '2025-12-01', 'Завершено', NULL, 3, NULL),
('Карта', 4000.00, '2026-02-10', 'Завершено', NULL, 4, NULL),
('Перевод', 14400.00, '2025-11-01', 'Завершено', NULL, 5, NULL),
('Карта', 7200.00, '2026-01-20', 'Завершено', NULL, 6, NULL),
('Онлайн', 10200.00, '2025-10-15', 'Завершено', NULL, 7, NULL),
('Наличные', 14400.00, '2025-09-01', 'Завершено', NULL, 8, NULL),
('Карта', 4000.00, '2026-02-15', 'Ожидает', NULL, 9, NULL),
('Перевод', 7200.00, '2025-12-10', 'Завершено', NULL, 10, NULL),
('Карта', 1000.00, '2026-02-05', 'Завершено', 1, NULL, NULL),
('Наличные', 900.00, '2026-02-05', 'Завершено', 2, NULL, NULL),
('Онлайн', 2500.00, '2026-02-20', 'Завершено', NULL, NULL, 6);
GO

-- 13. Заполнение таблицы Participation (Участие в соревнованиях)
INSERT INTO Participation (Registration_date, Start_number, Result_place, Score, ID_Competition, ID_Client, ID_Horse)
VALUES
('2026-02-01', 5, 2, 85.5, 6, 3, 1),
('2026-02-01', 8, 4, 78.0, 6, 5, 3),
('2026-02-02', 12, 1, 92.0, 6, 8, 5),
('2025-12-01', 3, 1, 88.5, 7, 2, 4),
('2025-12-01', 7, 3, 75.0, 7, 6, 6),
('2025-12-02', 15, 2, 82.0, 7, 10, 8),
('2026-03-01', 4, NULL, NULL, 3, 1, 2),
('2026-03-01', 9, NULL, NULL, 3, 4, 7),
('2026-03-02', 16, NULL, NULL, 3, 7, 9),
('2026-04-10', 2, NULL, NULL, 1, 3, 1),
('2026-04-10', 10, NULL, NULL, 1, 5, 3),
('2026-04-11', 18, NULL, NULL, 1, 8, 5);
GO

-- 14. Заполнение таблицы Schedule_Arena (Расписание манежей)
INSERT INTO Schedule_Arena (ID_Arena, [Date], Start_time, End_time, [Status], ID_Lesson, ID_Competition)
VALUES
(1, '2026-03-05', '10:00', '11:30', 'Запланировано', 1, NULL),
(3, '2026-03-05', '12:00', '13:30', 'Запланировано', 2, NULL),
(6, '2026-03-06', '15:00', '16:30', 'Запланировано', 3, NULL),
(2, '2026-03-06', '11:00', '12:30', 'Запланировано', 4, NULL),
(4, '2026-03-07', '14:00', '15:30', 'Запланировано', 5, NULL),
(3, '2026-03-07', '16:00', '17:30', 'Запланировано', 6, NULL),
(1, '2026-03-08', '09:00', '10:30', 'Запланировано', 7, NULL),
(5, '2026-03-08', '13:00', '14:30', 'Запланировано', 8, NULL),
(6, '2026-03-09', '11:00', '12:30', 'Запланировано', 9, NULL),
(2, '2026-03-09', '15:00', '16:30', 'Запланировано', 10, NULL),
(1, '2026-03-10', '10:00', '11:30', 'Запланировано', 11, NULL),
(4, '2026-03-10', '14:00', '15:30', 'Запланировано', 12, NULL),
(1, '2026-04-15', '09:00', '18:00', 'Запланировано', NULL, 1),
(3, '2026-05-20', '10:00', '19:00', 'Запланировано', NULL, 2),
(2, '2026-03-10', '09:00', '17:00', 'Запланировано', NULL, 3),
(4, '2026-06-05', '08:00', '20:00', 'Запланировано', NULL, 4),
(5, '2026-07-12', '10:00', '18:00', 'Запланировано', NULL, 5);
GO

-- Финальная проверка всех таблиц
SELECT 'Employee' AS TableName, COUNT(*) AS RowsCount FROM Employee
UNION ALL
SELECT 'Client', COUNT(*) FROM Client
UNION ALL
SELECT 'Stall', COUNT(*) FROM Stall
UNION ALL
SELECT 'Horse', COUNT(*) FROM Horse
UNION ALL
SELECT 'Coach', COUNT(*) FROM Coach
UNION ALL
SELECT 'Arena', COUNT(*) FROM Arena
UNION ALL
SELECT 'Membership', COUNT(*) FROM Membership
UNION ALL
SELECT 'Competition', COUNT(*) FROM Competition
UNION ALL
SELECT 'Lesson', COUNT(*) FROM Lesson
UNION ALL
SELECT 'Lesson_Horse', COUNT(*) FROM Lesson_Horse
UNION ALL
SELECT 'Membership_Lesson', COUNT(*) FROM Membership_Lesson
UNION ALL
SELECT 'Payment', COUNT(*) FROM Payment
UNION ALL
SELECT 'Participation', COUNT(*) FROM Participation
UNION ALL
SELECT 'Schedule_Arena', COUNT(*) FROM Schedule_Arena
ORDER BY TableName;
GO
