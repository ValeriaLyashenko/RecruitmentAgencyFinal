using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;

namespace RecruitmentAgencyFinal.Models
{
    public class DatabaseHelper
    {
        private string connectionString;

        public DatabaseHelper()
        {
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        // ==================== USERS ====================
        public List<UserTemp> GetAllUsers()
        {
            var list = new List<UserTemp>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT Id, Email, Password, FullName, Role, RegisteredAt, IsActive, AccessUntil FROM Users ORDER BY Id";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new UserTemp
                            {
                                Id = reader.GetInt32(0),
                                Email = reader.GetString(1),
                                Password = reader.GetString(2),
                                FullName = reader.GetString(3),
                                Role = reader.GetString(4),
                                RegisteredAt = reader.GetDateTime(5),
                                IsActive = reader.GetBoolean(6),
                                AccessUntil = reader.IsDBNull(7) ? null : (DateTime?)reader.GetDateTime(7)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetAllUsers error: " + ex.Message);
            }
            return list;
        }

        public UserTemp GetUser(string email, string password)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT Id, Email, Password, FullName, Role, RegisteredAt, IsActive, AccessUntil FROM Users WHERE Email = @Email AND Password = @Password AND IsActive = 1";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", password);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new UserTemp
                                {
                                    Id = reader.GetInt32(0),
                                    Email = reader.GetString(1),
                                    Password = reader.GetString(2),
                                    FullName = reader.GetString(3),
                                    Role = reader.GetString(4),
                                    RegisteredAt = reader.GetDateTime(5),
                                    IsActive = reader.GetBoolean(6),
                                    AccessUntil = reader.IsDBNull(7) ? null : (DateTime?)reader.GetDateTime(7)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetUser error: " + ex.Message);
            }
            return null;
        }

        public UserTemp GetUserById(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT Id, Email, Password, FullName, Role, RegisteredAt, IsActive, AccessUntil FROM Users WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new UserTemp
                                {
                                    Id = reader.GetInt32(0),
                                    Email = reader.GetString(1),
                                    Password = reader.GetString(2),
                                    FullName = reader.GetString(3),
                                    Role = reader.GetString(4),
                                    RegisteredAt = reader.GetDateTime(5),
                                    IsActive = reader.GetBoolean(6),
                                    AccessUntil = reader.IsDBNull(7) ? null : (DateTime?)reader.GetDateTime(7)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetUserById error: " + ex.Message);
            }
            return null;
        }

        public bool UserExists(string email)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT COUNT(1) FROM Users WHERE Email = @Email";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("UserExists error: " + ex.Message);
                return false;
            }
        }

        public bool AddUser(UserTemp user)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "INSERT INTO Users (Email, Password, FullName, Role, RegisteredAt, IsActive) VALUES (@Email, @Password, @FullName, @Role, @RegisteredAt, 1)";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", user.Email);
                        cmd.Parameters.AddWithValue("@Password", user.Password);
                        cmd.Parameters.AddWithValue("@FullName", user.FullName);
                        cmd.Parameters.AddWithValue("@Role", user.Role);
                        cmd.Parameters.AddWithValue("@RegisteredAt", user.RegisteredAt);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("AddUser error: " + ex.Message);
                return false;
            }
        }

        public bool UpdateUserRole(int userId, string newRole)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "UPDATE Users SET Role = @Role WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Role", newRole);
                        cmd.Parameters.AddWithValue("@Id", userId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("UpdateUserRole error: " + ex.Message);
                return false;
            }
        }

        public bool UpdateUserAccess(int userId, DateTime? accessUntil)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "UPDATE Users SET AccessUntil = @AccessUntil WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@AccessUntil", (object)accessUntil ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Id", userId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("UpdateUserAccess error: " + ex.Message);
                return false;
            }
        }

        public bool ToggleUserActive(int userId, bool isActive)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "UPDATE Users SET IsActive = @IsActive WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@IsActive", isActive);
                        cmd.Parameters.AddWithValue("@Id", userId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ToggleUserActive error: " + ex.Message);
                return false;
            }
        }

        // ==================== CANDIDATES ====================
        public List<Candidate> GetAllCandidates()
        {
            var list = new List<Candidate>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT Id, FullName, Position, Email, Phone, ApplicationDate FROM Candidates ORDER BY ApplicationDate DESC";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Candidate
                            {
                                Id = reader.GetInt32(0),
                                FullName = reader.GetString(1),
                                Position = reader.GetString(2),
                                Email = reader.GetString(3),
                                Phone = reader.GetString(4),
                                ApplicationDate = reader.GetDateTime(5)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetAllCandidates error: " + ex.Message);
            }
            return list;
        }

        public Tuple<List<Candidate>, int> GetAllCandidatesPaged(int page, int pageSize = 5)
        {
            return SearchCandidates("", page, pageSize);
        }

        public Tuple<List<Candidate>, int> SearchCandidates(string searchTerm, int page, int pageSize = 5)
        {
            var list = new List<Candidate>();
            int totalCount = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string countSql = @"SELECT COUNT(1) FROM Candidates 
                                        WHERE (@SearchTerm = '' OR 
                                               FullName LIKE @SearchPattern OR 
                                               Position LIKE @SearchPattern OR 
                                               Email LIKE @SearchPattern OR 
                                               Phone LIKE @SearchPattern)";

                    using (SqlCommand countCmd = new SqlCommand(countSql, conn))
                    {
                        countCmd.Parameters.AddWithValue("@SearchTerm", searchTerm ?? "");
                        countCmd.Parameters.AddWithValue("@SearchPattern", $"%{searchTerm ?? ""}%");
                        totalCount = (int)countCmd.ExecuteScalar();
                    }

                    int offset = (page - 1) * pageSize;
                    string sql = @"SELECT Id, FullName, Position, Email, Phone, ApplicationDate 
                                   FROM Candidates 
                                   WHERE (@SearchTerm = '' OR 
                                          FullName LIKE @SearchPattern OR 
                                          Position LIKE @SearchPattern OR 
                                          Email LIKE @SearchPattern OR 
                                          Phone LIKE @SearchPattern)
                                   ORDER BY ApplicationDate DESC
                                   OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@SearchTerm", searchTerm ?? "");
                        cmd.Parameters.AddWithValue("@SearchPattern", $"%{searchTerm ?? ""}%");
                        cmd.Parameters.AddWithValue("@Offset", offset);
                        cmd.Parameters.AddWithValue("@PageSize", pageSize);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new Candidate
                                {
                                    Id = reader.GetInt32(0),
                                    FullName = reader.GetString(1),
                                    Position = reader.GetString(2),
                                    Email = reader.GetString(3),
                                    Phone = reader.GetString(4),
                                    ApplicationDate = reader.GetDateTime(5)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("SearchCandidates error: " + ex.Message);
            }

            return Tuple.Create(list, totalCount);
        }

        public bool AddCandidate(Candidate candidate)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "INSERT INTO Candidates (FullName, Position, Email, Phone, ApplicationDate) VALUES (@FullName, @Position, @Email, @Phone, @ApplicationDate)";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@FullName", candidate.FullName);
                        cmd.Parameters.AddWithValue("@Position", candidate.Position);
                        cmd.Parameters.AddWithValue("@Email", candidate.Email);
                        cmd.Parameters.AddWithValue("@Phone", candidate.Phone);
                        cmd.Parameters.AddWithValue("@ApplicationDate", candidate.ApplicationDate);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("AddCandidate error: " + ex.Message);
                return false;
            }
        }

        public bool DeleteCandidate(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "DELETE FROM Candidates WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("DeleteCandidate error: " + ex.Message);
                return false;
            }
        }

        public System.Data.DataTable GetAllCandidatesForExport()
        {
            var dt = new System.Data.DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("ФИО", typeof(string));
            dt.Columns.Add("Должность", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Телефон", typeof(string));
            dt.Columns.Add("Дата подачи", typeof(string));

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT Id, FullName, Position, Email, Phone, ApplicationDate FROM Candidates ORDER BY ApplicationDate DESC";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dt.Rows.Add(
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetString(3),
                                reader.GetString(4),
                                reader.GetDateTime(5).ToString("dd.MM.yyyy HH:mm")
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetAllCandidatesForExport error: " + ex.Message);
            }

            return dt;
        }

        // ==================== VACANCIES ====================
        public List<Vacancy> GetAllVacancies()
        {
            var list = new List<Vacancy>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT Id, Title, Description, Salary, Requirements, ExperienceRequired, IsActive, CreatedAt, CreatedBy FROM Vacancies ORDER BY CreatedAt DESC";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Vacancy
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Description = reader.GetString(2),
                                Salary = reader.GetDecimal(3),
                                Requirements = reader.IsDBNull(4) ? null : reader.GetString(4),
                                ExperienceRequired = reader.GetInt32(5),
                                IsActive = reader.GetBoolean(6),
                                CreatedAt = reader.GetDateTime(7),
                                CreatedBy = reader.IsDBNull(8) ? null : reader.GetString(8)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetAllVacancies error: " + ex.Message);
            }
            return list;
        }

        public Vacancy GetVacancyById(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT Id, Title, Description, Salary, Requirements, ExperienceRequired, IsActive, CreatedAt, CreatedBy FROM Vacancies WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Vacancy
                                {
                                    Id = reader.GetInt32(0),
                                    Title = reader.GetString(1),
                                    Description = reader.GetString(2),
                                    Salary = reader.GetDecimal(3),
                                    Requirements = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    ExperienceRequired = reader.GetInt32(5),
                                    IsActive = reader.GetBoolean(6),
                                    CreatedAt = reader.GetDateTime(7),
                                    CreatedBy = reader.IsDBNull(8) ? null : reader.GetString(8)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetVacancyById error: " + ex.Message);
            }
            return null;
        }

        public bool AddVacancy(Vacancy vacancy)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "INSERT INTO Vacancies (Title, Description, Salary, Requirements, ExperienceRequired, IsActive, CreatedAt, CreatedBy) VALUES (@Title, @Description, @Salary, @Requirements, @ExperienceRequired, 1, @CreatedAt, @CreatedBy)";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Title", vacancy.Title);
                        cmd.Parameters.AddWithValue("@Description", vacancy.Description);
                        cmd.Parameters.AddWithValue("@Salary", vacancy.Salary);
                        cmd.Parameters.AddWithValue("@Requirements", (object)vacancy.Requirements ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ExperienceRequired", vacancy.ExperienceRequired);
                        cmd.Parameters.AddWithValue("@CreatedAt", vacancy.CreatedAt);
                        cmd.Parameters.AddWithValue("@CreatedBy", vacancy.CreatedBy);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("AddVacancy error: " + ex.Message);
                return false;
            }
        }

        public bool UpdateVacancy(Vacancy vacancy)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "UPDATE Vacancies SET Title = @Title, Description = @Description, Salary = @Salary, Requirements = @Requirements, ExperienceRequired = @ExperienceRequired, IsActive = @IsActive WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", vacancy.Id);
                        cmd.Parameters.AddWithValue("@Title", vacancy.Title);
                        cmd.Parameters.AddWithValue("@Description", vacancy.Description);
                        cmd.Parameters.AddWithValue("@Salary", vacancy.Salary);
                        cmd.Parameters.AddWithValue("@Requirements", (object)vacancy.Requirements ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ExperienceRequired", vacancy.ExperienceRequired);
                        cmd.Parameters.AddWithValue("@IsActive", vacancy.IsActive);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("UpdateVacancy error: " + ex.Message);
                return false;
            }
        }

        public bool DeleteVacancy(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "DELETE FROM Vacancies WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("DeleteVacancy error: " + ex.Message);
                return false;
            }
        }

        // ==================== RESUME METHODS ====================
        public List<Resume> GetAllResumes()
        {
            var list = new List<Resume>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT Id, UserId, FullName, BirthDate, Position, Experience, Education, Skills, ExpectedSalary, Email, Phone, CreatedAt, Status FROM Resumes ORDER BY CreatedAt DESC";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Resume
                            {
                                Id = reader.GetInt32(0),
                                UserId = reader.GetInt32(1),
                                FullName = reader.GetString(2),
                                BirthDate = reader.GetDateTime(3),
                                Position = reader.GetString(4),
                                Experience = reader.GetInt32(5),
                                Education = reader.IsDBNull(6) ? null : reader.GetString(6),
                                Skills = reader.IsDBNull(7) ? null : reader.GetString(7),
                                ExpectedSalary = reader.GetDecimal(8),
                                Email = reader.GetString(9),
                                Phone = reader.GetString(10),
                                CreatedAt = reader.GetDateTime(11),
                                Status = reader.GetString(12)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetAllResumes error: " + ex.Message);
            }
            return list;
        }

        public Resume GetResumeByUserId(int userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT Id, UserId, FullName, BirthDate, Position, Experience, Education, Skills, ExpectedSalary, Email, Phone, CreatedAt, Status FROM Resumes WHERE UserId = @UserId";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Resume
                                {
                                    Id = reader.GetInt32(0),
                                    UserId = reader.GetInt32(1),
                                    FullName = reader.GetString(2),
                                    BirthDate = reader.GetDateTime(3),
                                    Position = reader.GetString(4),
                                    Experience = reader.GetInt32(5),
                                    Education = reader.IsDBNull(6) ? null : reader.GetString(6),
                                    Skills = reader.IsDBNull(7) ? null : reader.GetString(7),
                                    ExpectedSalary = reader.GetDecimal(8),
                                    Email = reader.GetString(9),
                                    Phone = reader.GetString(10),
                                    CreatedAt = reader.GetDateTime(11),
                                    Status = reader.GetString(12)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetResumeByUserId error: " + ex.Message);
            }
            return null;
        }

        public Resume GetResumeById(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT Id, UserId, FullName, BirthDate, Position, Experience, Education, Skills, ExpectedSalary, Email, Phone, CreatedAt, Status FROM Resumes WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Resume
                                {
                                    Id = reader.GetInt32(0),
                                    UserId = reader.GetInt32(1),
                                    FullName = reader.GetString(2),
                                    BirthDate = reader.GetDateTime(3),
                                    Position = reader.GetString(4),
                                    Experience = reader.GetInt32(5),
                                    Education = reader.IsDBNull(6) ? null : reader.GetString(6),
                                    Skills = reader.IsDBNull(7) ? null : reader.GetString(7),
                                    ExpectedSalary = reader.GetDecimal(8),
                                    Email = reader.GetString(9),
                                    Phone = reader.GetString(10),
                                    CreatedAt = reader.GetDateTime(11),
                                    Status = reader.GetString(12)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetResumeById error: " + ex.Message);
            }
            return null;
        }

        public bool AddResume(Resume resume)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = @"INSERT INTO Resumes (UserId, FullName, BirthDate, Position, Experience, Education, Skills, ExpectedSalary, Email, Phone, CreatedAt, Status) 
                                   VALUES (@UserId, @FullName, @BirthDate, @Position, @Experience, @Education, @Skills, @ExpectedSalary, @Email, @Phone, @CreatedAt, @Status)";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", resume.UserId);
                        cmd.Parameters.AddWithValue("@FullName", resume.FullName);
                        cmd.Parameters.AddWithValue("@BirthDate", resume.BirthDate);
                        cmd.Parameters.AddWithValue("@Position", resume.Position);
                        cmd.Parameters.AddWithValue("@Experience", resume.Experience);
                        cmd.Parameters.AddWithValue("@Education", (object)resume.Education ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Skills", (object)resume.Skills ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ExpectedSalary", resume.ExpectedSalary);
                        cmd.Parameters.AddWithValue("@Email", resume.Email);
                        cmd.Parameters.AddWithValue("@Phone", resume.Phone);
                        cmd.Parameters.AddWithValue("@CreatedAt", resume.CreatedAt);
                        cmd.Parameters.AddWithValue("@Status", resume.Status);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("AddResume error: " + ex.Message);
                return false;
            }
        }

        public bool UpdateResume(Resume resume)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = @"UPDATE Resumes SET FullName = @FullName, BirthDate = @BirthDate, Position = @Position, 
                                   Experience = @Experience, Education = @Education, Skills = @Skills, 
                                   ExpectedSalary = @ExpectedSalary, Email = @Email, Phone = @Phone 
                                   WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", resume.Id);
                        cmd.Parameters.AddWithValue("@FullName", resume.FullName);
                        cmd.Parameters.AddWithValue("@BirthDate", resume.BirthDate);
                        cmd.Parameters.AddWithValue("@Position", resume.Position);
                        cmd.Parameters.AddWithValue("@Experience", resume.Experience);
                        cmd.Parameters.AddWithValue("@Education", (object)resume.Education ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Skills", (object)resume.Skills ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ExpectedSalary", resume.ExpectedSalary);
                        cmd.Parameters.AddWithValue("@Email", resume.Email);
                        cmd.Parameters.AddWithValue("@Phone", resume.Phone);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("UpdateResume error: " + ex.Message);
                return false;
            }
        }

        // ==================== APPLICATION METHODS ====================
        public bool AddApplication(Application application)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "INSERT INTO Applications (ResumeId, VacancyId, AppliedAt, Status, ManagerComment) VALUES (@ResumeId, @VacancyId, @AppliedAt, @Status, @ManagerComment)";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ResumeId", application.ResumeId);
                        cmd.Parameters.AddWithValue("@VacancyId", application.VacancyId);
                        cmd.Parameters.AddWithValue("@AppliedAt", application.AppliedAt);
                        cmd.Parameters.AddWithValue("@Status", application.Status);
                        cmd.Parameters.AddWithValue("@ManagerComment", (object)application.ManagerComment ?? DBNull.Value);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("AddApplication error: " + ex.Message);
                return false;
            }
        }

        public bool HasApplied(int resumeId, int vacancyId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT COUNT(1) FROM Applications WHERE ResumeId = @ResumeId AND VacancyId = @VacancyId";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ResumeId", resumeId);
                        cmd.Parameters.AddWithValue("@VacancyId", vacancyId);
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("HasApplied error: " + ex.Message);
                return false;
            }
        }

        public List<Application> GetAllApplications()
        {
            var list = new List<Application>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT Id, ResumeId, VacancyId, AppliedAt, Status, ManagerComment FROM Applications ORDER BY AppliedAt DESC";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Application
                            {
                                Id = reader.GetInt32(0),
                                ResumeId = reader.GetInt32(1),
                                VacancyId = reader.GetInt32(2),
                                AppliedAt = reader.GetDateTime(3),
                                Status = reader.GetString(4),
                                ManagerComment = reader.IsDBNull(5) ? null : reader.GetString(5)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetAllApplications error: " + ex.Message);
            }
            return list;
        }

        public bool UpdateApplicationStatus(int id, string status, string comment)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "UPDATE Applications SET Status = @Status, ManagerComment = @ManagerComment WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.Parameters.AddWithValue("@ManagerComment", (object)comment ?? DBNull.Value);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("UpdateApplicationStatus error: " + ex.Message);
                return false;
            }
        }

        // ==================== MATCHING ====================
        public List<MatchResult> MatchCandidatesToVacancy(int vacancyId)
        {
            var results = new List<MatchResult>();
            var vacancy = GetVacancyById(vacancyId);
            if (vacancy == null) return results;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT Id, FullName, Position, Experience, Skills FROM Candidates";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int matchPercent = 50;
                            int experience = reader.GetInt32(3);
                            string skills = reader.IsDBNull(4) ? "" : reader.GetString(4);

                            if (experience >= vacancy.ExperienceRequired)
                                matchPercent += 30;
                            else if (experience >= vacancy.ExperienceRequired - 1)
                                matchPercent += 15;

                            if (!string.IsNullOrEmpty(skills) && !string.IsNullOrEmpty(vacancy.Requirements))
                            {
                                if (skills.ToLower().Contains(vacancy.Requirements.ToLower()))
                                    matchPercent += 20;
                            }

                            results.Add(new MatchResult
                            {
                                ResumeId = reader.GetInt32(0),
                                FullName = reader.GetString(1),
                                Position = reader.GetString(2),
                                Experience = experience,
                                Skills = skills,
                                MatchPercentage = Math.Min(matchPercent, 100)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("MatchCandidatesToVacancy error: " + ex.Message);
            }
            return results;
        }
    }

    // ==================== HELPER CLASSES ====================
    public class MatchResult
    {
        public int ResumeId { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public int Experience { get; set; }
        public string Skills { get; set; }
        public int MatchPercentage { get; set; }
    }
}
