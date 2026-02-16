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
