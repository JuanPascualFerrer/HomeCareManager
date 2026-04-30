using ConsoleApp1;

Data data = new Data();
string suffix = DateTime.Now.ToString("yyyyMMddHHmmss");

Role role = new Role
{
    RoleId = $"role-{suffix}",
    RoleName = "Cuidador"
};

Skill skill = new Skill
{
    SkillId = $"skill-{suffix}",
    Name = "Movilizacion"
};

ConsoleApp1.TaskStatus status = new ConsoleApp1.TaskStatus
{
    StatusId = $"status-{suffix}",
    Name = "Pendiente"
};

User user = new User
{
    UserId = $"user-{suffix}",
    Name = "Prueba Cuidador",
    RoleId = role.RoleId,
    Email = $"prueba{suffix}@mail.com",
    PasswordHash = "hash-demo-123",
    IsActive = true,
    CreatedAt = DateTime.Now,
    SkillId = skill.SkillId
};

Patient patient = new Patient
{
    PatientId = $"patient-{suffix}",
    Name = "Paciente Demo",
    Address = "Calle Mayor 123",
    Phone = "600123123",
    Notes = "Paciente creado desde Program.cs",
    Priority = "Alta",
    EmergencyContact = "Familiar Demo - 600999999",
    Zone = "Centro"
};

TaskItem task = new TaskItem
{
    TaskId = $"task-{suffix}",
    RequiredSkillId = skill.SkillId,
    PatientId = patient.PatientId,
    Description = "Visita de prueba para comprobar inserts",
    Date = DateTime.Now.AddDays(1),
    Priority = "Alta",
    StatusId = status.StatusId
};

TaskAssignment assignment = new TaskAssignment
{
    AssignmentId = $"assignment-{suffix}",
    UserId = user.UserId,
    TaskId = task.TaskId,
    AssignedDate = DateTime.Now,
    StatusId = status.StatusId
};

Incident incident = new Incident
{
    IncidentId = $"incident-{suffix}",
    UserId = user.UserId,
    TaskId = task.TaskId,
    Description = "Incidencia de prueba",
    CreatedAt = DateTime.Now,
    Status = "Abierta"
};

Report report = new Report
{
    ReportId = $"report-{suffix}",
    UserId = user.UserId,
    TaskId = task.TaskId,
    Notes = "Reporte de prueba insertado desde consola",
    CreatedAt = DateTime.Now,
    StatusBefore = "Pendiente",
    StatusAfter = "En progreso",
    Duration = "00:30:00"
};

Availability availability = new Availability
{
    AvailabilityId = $"availability-{suffix}",
    UserId = user.UserId,
    StartTime = DateTime.Today.AddHours(9),
    EndTime = DateTime.Today.AddHours(17),
    Zone = "Centro"
};

bool ok = true;

ok &= PrintResult("roles", data.InsertRole(role.RoleId, role.RoleName));
ok &= PrintResult("skills", data.InsertSkill(skill.SkillId, skill.Name));
ok &= PrintResult("task_status", data.InsertTaskStatus(status.StatusId, status.Name));
ok &= PrintResult("users", data.InsertUser(user));
ok &= PrintResult("patients", data.InsertPatient(
    patient.PatientId,
    patient.Name,
    patient.Address,
    patient.Phone,
    patient.Notes,
    patient.Priority,
    patient.EmergencyContact,
    patient.Zone));
ok &= PrintResult("tasks", data.InsertTask(
    task.TaskId,
    task.RequiredSkillId,
    task.PatientId,
    task.Description,
    task.Date,
    task.Priority,
    task.StatusId));
ok &= PrintResult("task_assignments", data.InsertTaskAssignment(
    assignment.AssignmentId,
    assignment.UserId,
    assignment.TaskId,
    assignment.AssignedDate,
    assignment.StatusId));
ok &= PrintResult("incidents", data.InsertIncident(
    incident.IncidentId,
    incident.UserId,
    incident.TaskId,
    incident.Description,
    incident.CreatedAt,
    incident.Status));
ok &= PrintResult("reports", data.InsertReport(
    report.ReportId,
    report.UserId,
    report.Notes,
    report.CreatedAt,
    report.StatusBefore,
    report.StatusAfter,
    report.Duration,
    report.TaskId));
ok &= PrintResult("availability", data.InsertAvailability(
    availability.AvailabilityId,
    availability.StartTime.ToString("yyyy-MM-dd HH:mm:ss"),
    availability.Zone,
    availability.EndTime.ToString("yyyy-MM-dd HH:mm:ss"),
    availability.UserId));

Console.WriteLine();
Console.WriteLine(ok
    ? "Prueba completada: todos los INSERT han funcionado."
    : "Prueba completada con errores: revisa los mensajes anteriores y la estructura de la BBDD.");

static bool PrintResult(string tableName, bool result)
{
    Console.WriteLine(result
        ? $"INSERT OK en {tableName}"
        : $"INSERT ERROR en {tableName}");

    return result;
}
