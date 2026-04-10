-- ============================================
-- Курсовая работа: Кадровое агентство
-- Автор: Ляшенко В.Д., группа 3ИСТуд-124
-- Скрипт создания базы данных и таблиц
-- ============================================

-- Создаём базу данных (если её нет)
CREATE DATABASE RecruitmentAgencyFinalDB;
GO

USE RecruitmentAgencyFinalDB;
GO

-- ============================================
-- 1. Таблица пользователей (Users)
-- ============================================
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Email NVARCHAR(256) NOT NULL,
    Password NVARCHAR(128) NOT NULL,
    FullName NVARCHAR(200) NOT NULL,
    Role NVARCHAR(50) NOT NULL,
    RegisteredAt DATETIME NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    AccessUntil DATETIME NULL
);

-- ============================================
-- 2. Таблица кандидатов (Candidates)
-- ============================================
CREATE TABLE Candidates (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(200) NOT NULL,
    Position NVARCHAR(100) NOT NULL,
    Email NVARCHAR(256) NOT NULL,
    Phone NVARCHAR(50) NOT NULL,
    ApplicationDate DATETIME NOT NULL
);

-- ============================================
-- 3. Таблица вакансий (Vacancies)
-- ============================================
CREATE TABLE Vacancies (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    Salary DECIMAL(18,2) NOT NULL,
    Requirements NVARCHAR(MAX) NULL,
    ExperienceRequired INT NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL,
    CreatedBy NVARCHAR(200) NULL
);

-- ============================================
-- 4. Таблица резюме (Resumes)
-- ============================================
CREATE TABLE Resumes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    FullName NVARCHAR(200) NOT NULL,
    BirthDate DATETIME NOT NULL,
    Position NVARCHAR(200) NOT NULL,
    Experience INT NOT NULL DEFAULT 0,
    Education NVARCHAR(500) NULL,
    Skills NVARCHAR(MAX) NULL,
    ExpectedSalary DECIMAL(18,2) NOT NULL,
    Email NVARCHAR(256) NOT NULL,
    Phone NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME NOT NULL,
    Status NVARCHAR(50) NOT NULL DEFAULT 'Active',
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

-- ============================================
-- 5. Таблица откликов (Applications)
-- ============================================
CREATE TABLE Applications (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ResumeId INT NOT NULL,
    VacancyId INT NOT NULL,
    AppliedAt DATETIME NOT NULL,
    Status NVARCHAR(50) NOT NULL DEFAULT 'Pending',
    ManagerComment NVARCHAR(MAX) NULL,
    FOREIGN KEY (ResumeId) REFERENCES Resumes(Id) ON DELETE CASCADE,
    FOREIGN KEY (VacancyId) REFERENCES Vacancies(Id) ON DELETE CASCADE
);

-- ============================================
-- ДОБАВЛЕНИЕ ТЕСТОВЫХ ДАННЫХ
-- ============================================

-- Добавление пользователей
INSERT INTO Users (Email, Password, FullName, Role, RegisteredAt, IsActive)
VALUES 
('admin@agency.ru', 'admin123', 'Администратор', 'Admin', GETDATE(), 1),
('manager@agency.ru', 'manager123', 'Менеджер', 'Manager', GETDATE(), 1),
('user@mail.ru', '123', 'Обычный пользователь', 'Applicant', GETDATE(), 1);

-- Добавление кандидатов
INSERT INTO Candidates (FullName, Position, Email, Phone, ApplicationDate) VALUES
(N'Иванов Иван Иванович', N'Разработчик C#', N'ivan.ivanov@mail.ru', N'+7 (901) 111-22-33', DATEADD(day, -1, GETDATE())),
(N'Петрова Анна Сергеевна', N'Менеджер по продажам', N'anna.petrova@mail.ru', N'+7 (902) 222-33-44', DATEADD(day, -2, GETDATE())),
(N'Сидоров Алексей Владимирович', N'Java разработчик', N'alex.sidorov@mail.ru', N'+7 (903) 333-44-55', DATEADD(day, -3, GETDATE())),
(N'Кузнецова Екатерина Дмитриевна', N'HR-менеджер', N'ekaterina.k@mail.ru', N'+7 (904) 444-55-66', DATEADD(day, -4, GETDATE())),
(N'Смирнов Дмитрий Александрович', N'Python разработчик', N'dmitry.smirnov@mail.ru', N'+7 (905) 555-66-77', DATEADD(day, -5, GETDATE()));

-- Добавление вакансий
INSERT INTO Vacancies (Title, Description, Salary, Requirements, ExperienceRequired, IsActive, CreatedAt, CreatedBy) VALUES
(N'Разработчик C#', N'Разработка веб-приложений на ASP.NET MVC. Участие в полном цикле разработки.', 120000, N'C#, SQL, ASP.NET Core, Entity Framework', 2, 1, GETDATE(), N'admin@agency.ru'),
(N'Java разработчик', N'Разработка серверных приложений на Java Spring', 130000, N'Java, Spring, Hibernate, Maven', 2, 1, GETDATE(), N'admin@agency.ru'),
(N'Менеджер по продажам', N'Активные продажи, поиск новых клиентов, ведение переговоров', 90000, N'Опыт продаж от 1 года, знание техник продаж', 1, 1, GETDATE(), N'admin@agency.ru');

-- Добавление резюме (для пользователя user@mail.ru)
INSERT INTO Resumes (UserId, FullName, BirthDate, Position, Experience, Education, Skills, ExpectedSalary, Email, Phone, CreatedAt, Status)
VALUES (
    3, 
    N'Обычный пользователь', 
    '1995-01-01', 
    N'Разработчик', 
    3, 
    N'Высшее техническое', 
    N'C#, SQL, ASP.NET, JavaScript', 
    100000, 
    'user@mail.ru', 
    '+7 (900) 111-22-33', 
    GETDATE(), 
    'Active'
);

-- Добавление откликов
INSERT INTO Applications (ResumeId, VacancyId, AppliedAt, Status, ManagerComment)
VALUES (1, 1, GETDATE(), 'Pending', NULL);

-- ============================================
-- ПРОВЕРКА ДАННЫХ
-- ============================================
SELECT 'Users' AS TableName, * FROM Users;
SELECT 'Candidates' AS TableName, * FROM Candidates;
SELECT 'Vacancies' AS TableName, * FROM Vacancies;
SELECT 'Resumes' AS TableName, * FROM Resumes;
SELECT 'Applications' AS TableName, * FROM Applications;
