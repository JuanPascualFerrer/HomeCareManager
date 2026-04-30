using System;
using MySqlConnector;

namespace ConsoleApp1
{
    internal class Data
    {
        private readonly string connectionString =
            "datasource = 127.0.0.1;" +
            "port = 3306;" +
            "username = root; password = ;" +
            "database = homecaremanager";

        private bool ExecuteNonQuery(string query)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                using MySqlCommand commandDatabase = new MySqlCommand(query, connection);

                connection.Open();
                return commandDatabase.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private bool RecordExists(string tableName, string idColumn, string idValue)
        {
            string query = $"SELECT COUNT(*) FROM {tableName} WHERE {idColumn} = {ToSqlString(idValue)};";

            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                using MySqlCommand commandDatabase = new MySqlCommand(query, connection);

                connection.Open();
                object? result = commandDatabase.ExecuteScalar();
                return Convert.ToInt32(result) > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private bool InsertIfNotExists(string tableName, string idColumn, string idValue, string query)
        {
            if (RecordExists(tableName, idColumn, idValue))
            {
                Console.WriteLine($"No se puede insertar: ya existe un registro en {tableName} con {idColumn} = {idValue}");
                return false;
            }

            return ExecuteNonQuery(query);
        }

        private bool UpdateIfExists(string tableName, string idColumn, string idValue, string query)
        {
            if (!RecordExists(tableName, idColumn, idValue))
            {
                Console.WriteLine($"No se puede actualizar: no existe un registro en {tableName} con {idColumn} = {idValue}");
                return false;
            }

            return ExecuteNonQuery(query);
        }

        private bool DeleteIfExists(string tableName, string idColumn, string idValue, string query)
        {
            if (!RecordExists(tableName, idColumn, idValue))
            {
                Console.WriteLine($"No se puede borrar: no existe un registro en {tableName} con {idColumn} = {idValue}");
                return false;
            }

            return ExecuteNonQuery(query);
        }

        private string Escape(string value)
        {
            return value.Replace("\\", "\\\\").Replace("'", "''");
        }

        private string ToSqlString(string value)
        {
            return $"'{Escape(value)}'";
        }

        private string ToSqlDateTime(DateTime value)
        {
            return $"'{value:yyyy-MM-dd HH:mm:ss}'";
        }

        private string ToSqlBool(bool value)
        {
            return value ? "1" : "0";
        }

        public bool InsertRole(string roleId, string roleName)
        {
            string query = "INSERT INTO roles(RoleId, RoleName) " +
                $"VALUES({ToSqlString(roleId)}, {ToSqlString(roleName)});";

            return InsertIfNotExists("roles", "RoleId", roleId, query);
        }

        public bool UpdateRole(string roleId, string roleName)
        {
            string query = "UPDATE roles " +
                $"SET RoleName = {ToSqlString(roleName)} " +
                $"WHERE RoleId = {ToSqlString(roleId)};";

            return UpdateIfExists("roles", "RoleId", roleId, query);
        }

        public bool DeleteRole(string roleId)
        {
            string query = $"DELETE FROM roles WHERE RoleId = {ToSqlString(roleId)};";
            return DeleteIfExists("roles", "RoleId", roleId, query);
        }

        public bool InsertSkill(string skillId, string name)
        {
            string query = "INSERT INTO skills(SkillId, Name) " +
                $"VALUES({ToSqlString(skillId)}, {ToSqlString(name)});";

            return InsertIfNotExists("skills", "SkillId", skillId, query);
        }

        public bool UpdateSkill(string skillId, string name)
        {
            string query = "UPDATE skills " +
                $"SET Name = {ToSqlString(name)} " +
                $"WHERE SkillId = {ToSqlString(skillId)};";

            return UpdateIfExists("skills", "SkillId", skillId, query);
        }

        public bool DeleteSkill(string skillId)
        {
            string query = $"DELETE FROM skills WHERE SkillId = {ToSqlString(skillId)};";
            return DeleteIfExists("skills", "SkillId", skillId, query);
        }

        public bool InsertTaskStatus(string statusId, string name)
        {
            string query = "INSERT INTO task_status(StatusId, Name) " +
                $"VALUES({ToSqlString(statusId)}, {ToSqlString(name)});";

            return InsertIfNotExists("task_status", "StatusId", statusId, query);
        }

        public bool UpdateTaskStatus(string statusId, string name)
        {
            string query = "UPDATE task_status " +
                $"SET Name = {ToSqlString(name)} " +
                $"WHERE StatusId = {ToSqlString(statusId)};";

            return UpdateIfExists("task_status", "StatusId", statusId, query);
        }

        public bool DeleteTaskStatus(string statusId)
        {
            string query = $"DELETE FROM task_status WHERE StatusId = {ToSqlString(statusId)};";
            return DeleteIfExists("task_status", "StatusId", statusId, query);
        }

        public bool InsertUser(User user)
        {
            string query = "INSERT INTO users(UserId, Name, RoleId, Email, PasswordHash, IsActive, CreatedAt, SkillId) " +
                $"VALUES({ToSqlString(user.UserId)}, {ToSqlString(user.Name)}, {ToSqlString(user.RoleId)}, " +
                $"{ToSqlString(user.Email)}, {ToSqlString(user.PasswordHash)}, {ToSqlBool(user.IsActive)}, " +
                $"{ToSqlDateTime(user.CreatedAt)}, {ToSqlString(user.SkillId)});";

            return InsertIfNotExists("users", "UserId", user.UserId, query);
        }

        public bool UpdateUser(User user)
        {
            string query = "UPDATE users " +
                $"SET Name = {ToSqlString(user.Name)}, " +
                $"RoleId = {ToSqlString(user.RoleId)}, " +
                $"Email = {ToSqlString(user.Email)}, " +
                $"PasswordHash = {ToSqlString(user.PasswordHash)}, " +
                $"IsActive = {ToSqlBool(user.IsActive)}, " +
                $"CreatedAt = {ToSqlDateTime(user.CreatedAt)}, " +
                $"SkillId = {ToSqlString(user.SkillId)} " +
                $"WHERE UserId = {ToSqlString(user.UserId)};";

            return UpdateIfExists("users", "UserId", user.UserId, query);
        }

        public bool DeleteUser(string userId)
        {
            string query = $"DELETE FROM users WHERE UserId = {ToSqlString(userId)};";
            return DeleteIfExists("users", "UserId", userId, query);
        }

        public bool InsertPatient(
            string patientId,
            string name,
            string address,
            string phone,
            string notes,
            string priority,
            string emergencyContact,
            string zone)
        {
            string query = "INSERT INTO patients(PatientId, Name, Address, Phone, Notes, Priority, EmergencyContact, Zone) " +
                $"VALUES({ToSqlString(patientId)}, {ToSqlString(name)}, {ToSqlString(address)}, {ToSqlString(phone)}, " +
                $"{ToSqlString(notes)}, {ToSqlString(priority)}, {ToSqlString(emergencyContact)}, {ToSqlString(zone)});";

            return InsertIfNotExists("patients", "PatientId", patientId, query);
        }

        public bool UpdatePatient(
            string patientId,
            string name,
            string address,
            string phone,
            string notes,
            string priority,
            string emergencyContact,
            string zone)
        {
            string query = "UPDATE patients " +
                $"SET Name = {ToSqlString(name)}, " +
                $"Address = {ToSqlString(address)}, " +
                $"Phone = {ToSqlString(phone)}, " +
                $"Notes = {ToSqlString(notes)}, " +
                $"Priority = {ToSqlString(priority)}, " +
                $"EmergencyContact = {ToSqlString(emergencyContact)}, " +
                $"Zone = {ToSqlString(zone)} " +
                $"WHERE PatientId = {ToSqlString(patientId)};";

            return UpdateIfExists("patients", "PatientId", patientId, query);
        }

        public bool DeletePatient(string patientId)
        {
            string query = $"DELETE FROM patients WHERE PatientId = {ToSqlString(patientId)};";
            return DeleteIfExists("patients", "PatientId", patientId, query);
        }

        public bool InsertTask(
            string taskId,
            string requiredSkillId,
            string patientId,
            string description,
            DateTime date,
            string priority,
            string statusId)
        {
            string query = "INSERT INTO tasks(RequiredSkillId, TaskId, PatientId, Description, Date, Priority, StatusId) " +
                $"VALUES({ToSqlString(requiredSkillId)}, {ToSqlString(taskId)}, {ToSqlString(patientId)}, " +
                $"{ToSqlString(description)}, {ToSqlDateTime(date)}, {ToSqlString(priority)}, {ToSqlString(statusId)});";

            return InsertIfNotExists("tasks", "TaskId", taskId, query);
        }

        public bool UpdateTask(
            string taskId,
            string requiredSkillId,
            string patientId,
            string description,
            DateTime date,
            string priority,
            string statusId)
        {
            string query = "UPDATE tasks " +
                $"SET RequiredSkillId = {ToSqlString(requiredSkillId)}, " +
                $"PatientId = {ToSqlString(patientId)}, " +
                $"Description = {ToSqlString(description)}, " +
                $"Date = {ToSqlDateTime(date)}, " +
                $"Priority = {ToSqlString(priority)}, " +
                $"StatusId = {ToSqlString(statusId)} " +
                $"WHERE TaskId = {ToSqlString(taskId)};";

            return UpdateIfExists("tasks", "TaskId", taskId, query);
        }

        public bool DeleteTask(string taskId)
        {
            string query = $"DELETE FROM tasks WHERE TaskId = {ToSqlString(taskId)};";
            return DeleteIfExists("tasks", "TaskId", taskId, query);
        }

        public bool InsertTaskAssignment(
            string assignmentId,
            string userId,
            string taskId,
            DateTime assignedDate,
            string statusId)
        {
            string query = "INSERT INTO task_assignments(UserId, TaskId, AssignmentId, AssignedDate, StatusId) " +
                $"VALUES({ToSqlString(userId)}, {ToSqlString(taskId)}, {ToSqlString(assignmentId)}, " +
                $"{ToSqlDateTime(assignedDate)}, {ToSqlString(statusId)});";

            return InsertIfNotExists("task_assignments", "AssignmentId", assignmentId, query);
        }

        public bool UpdateTaskAssignment(
            string assignmentId,
            string userId,
            string taskId,
            DateTime assignedDate,
            string statusId)
        {
            string query = "UPDATE task_assignments " +
                $"SET UserId = {ToSqlString(userId)}, " +
                $"TaskId = {ToSqlString(taskId)}, " +
                $"AssignedDate = {ToSqlDateTime(assignedDate)}, " +
                $"StatusId = {ToSqlString(statusId)} " +
                $"WHERE AssignmentId = {ToSqlString(assignmentId)};";

            return UpdateIfExists("task_assignments", "AssignmentId", assignmentId, query);
        }

        public bool DeleteTaskAssignment(string assignmentId)
        {
            string query = $"DELETE FROM task_assignments WHERE AssignmentId = {ToSqlString(assignmentId)};";
            return DeleteIfExists("task_assignments", "AssignmentId", assignmentId, query);
        }

        public bool InsertIncident(
            string incidentId,
            string userId,
            string taskId,
            string description,
            DateTime createdAt,
            string status)
        {
            string query = "INSERT INTO incidents(UserId, IncidentId, TaskId, Description, CreatedAt, Status) " +
                $"VALUES({ToSqlString(userId)}, {ToSqlString(incidentId)}, {ToSqlString(taskId)}, " +
                $"{ToSqlString(description)}, {ToSqlDateTime(createdAt)}, {ToSqlString(status)});";

            return InsertIfNotExists("incidents", "IncidentId", incidentId, query);
        }

        public bool UpdateIncident(
            string incidentId,
            string userId,
            string taskId,
            string description,
            DateTime createdAt,
            string status)
        {
            string query = "UPDATE incidents " +
                $"SET UserId = {ToSqlString(userId)}, " +
                $"TaskId = {ToSqlString(taskId)}, " +
                $"Description = {ToSqlString(description)}, " +
                $"CreatedAt = {ToSqlDateTime(createdAt)}, " +
                $"Status = {ToSqlString(status)} " +
                $"WHERE IncidentId = {ToSqlString(incidentId)};";

            return UpdateIfExists("incidents", "IncidentId", incidentId, query);
        }

        public bool DeleteIncident(string incidentId)
        {
            string query = $"DELETE FROM incidents WHERE IncidentId = {ToSqlString(incidentId)};";
            return DeleteIfExists("incidents", "IncidentId", incidentId, query);
        }

        public bool InsertReport(
            string reportId,
            string userId,
            string notes,
            DateTime createdAt,
            string statusBefore,
            string statusAfter,
            string duration,
            string taskId)
        {
            string query = "INSERT INTO reports(ReportId, UserId, Notes, CreatedAt, StatusBefore, StatusAfter, Duration, TaskId) " +
                $"VALUES({ToSqlString(reportId)}, {ToSqlString(userId)}, {ToSqlString(notes)}, {ToSqlDateTime(createdAt)}, " +
                $"{ToSqlString(statusBefore)}, {ToSqlString(statusAfter)}, {ToSqlString(duration)}, {ToSqlString(taskId)});";

            return InsertIfNotExists("reports", "ReportId", reportId, query);
        }

        public bool UpdateReport(
            string reportId,
            string userId,
            string notes,
            DateTime createdAt,
            string statusBefore,
            string statusAfter,
            string duration,
            string taskId)
        {
            string query = "UPDATE reports " +
                $"SET UserId = {ToSqlString(userId)}, " +
                $"Notes = {ToSqlString(notes)}, " +
                $"CreatedAt = {ToSqlDateTime(createdAt)}, " +
                $"StatusBefore = {ToSqlString(statusBefore)}, " +
                $"StatusAfter = {ToSqlString(statusAfter)}, " +
                $"Duration = {ToSqlString(duration)}, " +
                $"TaskId = {ToSqlString(taskId)} " +
                $"WHERE ReportId = {ToSqlString(reportId)};";

            return UpdateIfExists("reports", "ReportId", reportId, query);
        }

        public bool DeleteReport(string reportId)
        {
            string query = $"DELETE FROM reports WHERE ReportId = {ToSqlString(reportId)};";
            return DeleteIfExists("reports", "ReportId", reportId, query);
        }

        public bool InsertAvailability(
            string availabilityId,
            string startTime,
            string zone,
            string endTime,
            string userId)
        {
            string query = "INSERT INTO availability(AvailabilityId, StartTime, Zone, EndTime, UserId) " +
                $"VALUES({ToSqlString(availabilityId)}, {ToSqlString(startTime)}, {ToSqlString(zone)}, " +
                $"{ToSqlString(endTime)}, {ToSqlString(userId)});";

            return InsertIfNotExists("availability", "AvailabilityId", availabilityId, query);
        }

        public bool UpdateAvailability(
            string availabilityId,
            string startTime,
            string zone,
            string endTime,
            string userId)
        {
            string query = "UPDATE availability " +
                $"SET StartTime = {ToSqlString(startTime)}, " +
                $"Zone = {ToSqlString(zone)}, " +
                $"EndTime = {ToSqlString(endTime)}, " +
                $"UserId = {ToSqlString(userId)} " +
                $"WHERE AvailabilityId = {ToSqlString(availabilityId)};";

            return UpdateIfExists("availability", "AvailabilityId", availabilityId, query);
        }

        public bool DeleteAvailability(string availabilityId)
        {
            string query = $"DELETE FROM availability WHERE AvailabilityId = {ToSqlString(availabilityId)};";
            return DeleteIfExists("availability", "AvailabilityId", availabilityId, query);
        }
    }
}
