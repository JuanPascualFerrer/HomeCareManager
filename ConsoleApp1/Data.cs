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
            "database = HomeCareManager";

        private bool Insert(string query)
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

            return Insert(query);
        }

        public bool InsertSkill(string skillId, string name)
        {
            string query = "INSERT INTO skills(SkillId, Name) " +
                $"VALUES({ToSqlString(skillId)}, {ToSqlString(name)});";

            return Insert(query);
        }

        public bool InsertTaskStatus(string statusId, string name)
        {
            string query = "INSERT INTO task_status(StatusId, Name) " +
                $"VALUES({ToSqlString(statusId)}, {ToSqlString(name)});";

            return Insert(query);
        }

        public bool InsertUser(User user)
        {
            string query = "INSERT INTO users(UserId, Name, RoleId, Email, PasswordHash, IsActive, CreatedAt, SkillId) " +
                $"VALUES({ToSqlString(user.UserId)}, {ToSqlString(user.Name)}, {ToSqlString(user.RoleId)}, " +
                $"{ToSqlString(user.Email)}, {ToSqlString(user.PasswordHash)}, {ToSqlBool(user.IsActive)}, " +
                $"{ToSqlDateTime(user.CreatedAt)}, {ToSqlString(user.SkillId)});";

            return Insert(query);
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

            return Insert(query);
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

            return Insert(query);
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

            return Insert(query);
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

            return Insert(query);
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

            return Insert(query);
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

            return Insert(query);
        }
    }
}
