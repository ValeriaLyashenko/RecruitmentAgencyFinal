# Кадровое агентство - Курсовая работа

**Автор:** Ляшенко В.Д.  
**Группа:** 3ИСТуд-124  
**Тема:** Разработка программной системы «Кадровое агентство»

---

## Описание

Веб-приложение для автоматизации работы кадрового агентства.

### Основные функции

- Регистрация и авторизация с ролями (Админ, Менеджер, Соискатель)
- Управление пользователями (только Админ)
- Управление вакансиями (Менеджер)
- Создание резюме и отклик на вакансии (Соискатель)
- Подбор персонала с оценкой совпадения
- Экспорт данных в Excel
- Поиск и пагинация

### Технологии

- ASP.NET MVC 5
- C#
- Entity Framework
- SQL Server LocalDB
- Bootstrap 5
- jQuery, AJAX

---

## Запуск проекта
Откройте файл RecruitmentAgencyFinal.sln в Visual Studio.

Запуск
Нажмите F5.

Тестовые пользователи
Роль	             Email	                   Пароль
Администратор	 admin@agency.ru	            admin123
Менеджер	       manager@agency.ru	         manager123
Соискатель	    user@mail.ru	                 123

## Настройка базы данных

1. Откройте SQL Server Object Explorer в Visual Studio
2. Выполните скрипт `SQLQuery1.sql` для создания таблиц
3. Строка подключения в `Web.config`:
   `<add name="DefaultConnection" connectionString="Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=RecruitmentAgencyFinalDB;Integrated Security=True" providerName="System.Data.SqlClient" />`
