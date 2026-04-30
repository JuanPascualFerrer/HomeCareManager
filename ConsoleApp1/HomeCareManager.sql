CREATE DATABASE IF NOT EXISTS `HomeCareManager`;
USE `HomeCareManager`;

CREATE TABLE IF NOT EXISTS `roles` (
    `RoleId` VARCHAR(100) NOT NULL,
    `RoleName` VARCHAR(100) NOT NULL,
    PRIMARY KEY (`RoleId`)
);

CREATE TABLE IF NOT EXISTS `skills` (
    `SkillId` VARCHAR(100) NOT NULL,
    `Name` VARCHAR(100) NOT NULL,
    PRIMARY KEY (`SkillId`)
);

CREATE TABLE IF NOT EXISTS `task_status` (
    `StatusId` VARCHAR(100) NOT NULL,
    `Name` VARCHAR(100) NOT NULL,
    PRIMARY KEY (`StatusId`)
);

CREATE TABLE IF NOT EXISTS `users` (
    `UserId` VARCHAR(100) NOT NULL,
    `Name` VARCHAR(150) NOT NULL,
    `RoleId` VARCHAR(100) NOT NULL,
    `Email` VARCHAR(150) NOT NULL,
    `PasswordHash` VARCHAR(255) NOT NULL,
    `IsActive` BOOLEAN NOT NULL,
    `CreatedAt` DATETIME NOT NULL,
    `SkillId` VARCHAR(100) NOT NULL,
    PRIMARY KEY (`UserId`),
    INDEX `IX_users_RoleId` (`RoleId`),
    INDEX `IX_users_SkillId` (`SkillId`),
    CONSTRAINT `FK_users_roles`
        FOREIGN KEY (`RoleId`) REFERENCES `roles` (`RoleId`),
    CONSTRAINT `FK_users_skills`
        FOREIGN KEY (`SkillId`) REFERENCES `skills` (`SkillId`)
);

CREATE TABLE IF NOT EXISTS `patients` (
    `PatientId` VARCHAR(100) NOT NULL,
    `Name` VARCHAR(150) NOT NULL,
    `Address` VARCHAR(200) NOT NULL,
    `Phone` VARCHAR(30) NOT NULL,
    `Notes` TEXT NOT NULL,
    `Priority` VARCHAR(50) NOT NULL,
    `EmergencyContact` VARCHAR(150) NOT NULL,
    `Zone` VARCHAR(100) NOT NULL,
    PRIMARY KEY (`PatientId`)
);

CREATE TABLE IF NOT EXISTS `tasks` (
    `RequiredSkillId` VARCHAR(100) NOT NULL,
    `TaskId` VARCHAR(100) NOT NULL,
    `PatientId` VARCHAR(100) NOT NULL,
    `Description` TEXT NOT NULL,
    `Date` DATETIME NOT NULL,
    `Priority` VARCHAR(50) NOT NULL,
    `StatusId` VARCHAR(100) NOT NULL,
    PRIMARY KEY (`TaskId`),
    INDEX `IX_tasks_RequiredSkillId` (`RequiredSkillId`),
    INDEX `IX_tasks_PatientId` (`PatientId`),
    INDEX `IX_tasks_StatusId` (`StatusId`),
    CONSTRAINT `FK_tasks_skills`
        FOREIGN KEY (`RequiredSkillId`) REFERENCES `skills` (`SkillId`),
    CONSTRAINT `FK_tasks_patients`
        FOREIGN KEY (`PatientId`) REFERENCES `patients` (`PatientId`),
    CONSTRAINT `FK_tasks_task_status`
        FOREIGN KEY (`StatusId`) REFERENCES `task_status` (`StatusId`)
);

CREATE TABLE IF NOT EXISTS `reports` (
    `ReportId` VARCHAR(100) NOT NULL,
    `UserId` VARCHAR(100) NOT NULL,
    `Notes` TEXT NOT NULL,
    `CreatedAt` DATETIME NOT NULL,
    `StatusBefore` VARCHAR(100) NOT NULL,
    `StatusAfter` VARCHAR(100) NOT NULL,
    `Duration` VARCHAR(50) NOT NULL,
    `TaskId` VARCHAR(100) NOT NULL,
    PRIMARY KEY (`ReportId`),
    INDEX `IX_reports_UserId` (`UserId`),
    INDEX `IX_reports_TaskId` (`TaskId`),
    CONSTRAINT `FK_reports_users`
        FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`),
    CONSTRAINT `FK_reports_tasks`
        FOREIGN KEY (`TaskId`) REFERENCES `tasks` (`TaskId`)
);

CREATE TABLE IF NOT EXISTS `incidents` (
    `UserId` VARCHAR(100) NOT NULL,
    `IncidentId` VARCHAR(100) NOT NULL,
    `TaskId` VARCHAR(100) NOT NULL,
    `Description` TEXT NOT NULL,
    `CreatedAt` DATETIME NOT NULL,
    `Status` VARCHAR(50) NOT NULL,
    PRIMARY KEY (`IncidentId`),
    INDEX `IX_incidents_UserId` (`UserId`),
    INDEX `IX_incidents_TaskId` (`TaskId`),
    CONSTRAINT `FK_incidents_users`
        FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`),
    CONSTRAINT `FK_incidents_tasks`
        FOREIGN KEY (`TaskId`) REFERENCES `tasks` (`TaskId`)
);

CREATE TABLE IF NOT EXISTS `task_assignments` (
    `UserId` VARCHAR(100) NOT NULL,
    `TaskId` VARCHAR(100) NOT NULL,
    `AssignmentId` VARCHAR(100) NOT NULL,
    `AssignedDate` DATETIME NOT NULL,
    `StatusId` VARCHAR(100) NOT NULL,
    PRIMARY KEY (`AssignmentId`),
    INDEX `IX_task_assignments_UserId` (`UserId`),
    INDEX `IX_task_assignments_TaskId` (`TaskId`),
    INDEX `IX_task_assignments_StatusId` (`StatusId`),
    CONSTRAINT `FK_task_assignments_users`
        FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`),
    CONSTRAINT `FK_task_assignments_tasks`
        FOREIGN KEY (`TaskId`) REFERENCES `tasks` (`TaskId`),
    CONSTRAINT `FK_task_assignments_task_status`
        FOREIGN KEY (`StatusId`) REFERENCES `task_status` (`StatusId`)
);

CREATE TABLE IF NOT EXISTS `availability` (
    `AvailabilityId` VARCHAR(100) NOT NULL,
    `StartTime` DATETIME NOT NULL,
    `Zone` VARCHAR(100) NOT NULL,
    `EndTime` DATETIME NOT NULL,
    `UserId` VARCHAR(100) NOT NULL,
    PRIMARY KEY (`AvailabilityId`),
    INDEX `IX_availability_UserId` (`UserId`),
    CONSTRAINT `FK_availability_users`
        FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`)
);
