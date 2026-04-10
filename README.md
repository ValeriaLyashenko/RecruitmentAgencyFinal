## Настройка базы данных

1. Откройте SQL Server Object Explorer в Visual Studio
2. Выполните скрипт `SQLQuery1.sql` для создания таблиц
3. Строка подключения в `Web.config`:
   `<add name="DefaultConnection" connectionString="Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=RecruitmentAgencyFinalDB;Integrated Security=True" providerName="System.Data.SqlClient" />`
