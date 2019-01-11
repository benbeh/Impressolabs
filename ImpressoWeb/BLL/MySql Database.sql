-- --------------------------------------------------------
-- Сервер:                       127.0.0.1
-- Версія сервера:               10.3.7-MariaDB - mariadb.org binary distribution
-- ОС сервера:                   Win64
-- HeidiSQL Версія:              9.5.0.5196
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- Dumping database structure for impressoweb
CREATE DATABASE IF NOT EXISTS `impressoweb` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `impressoweb`;

-- Dumping structure for таблиця impressoweb.appuserskills
CREATE TABLE IF NOT EXISTS `appuserskills` (
  `AppUserId` varchar(255) NOT NULL,
  `SkillId` int(11) NOT NULL,
  PRIMARY KEY (`AppUserId`,`SkillId`),
  KEY `IX_AppUserSkills_SkillId` (`SkillId`),
  CONSTRAINT `FK_AppUserSkills_AspNetUsers_AppUserId` FOREIGN KEY (`AppUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AppUserSkills_Skills_SkillId` FOREIGN KEY (`SkillId`) REFERENCES `skills` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table impressoweb.appuserskills: ~0 rows (приблизно)
/*!40000 ALTER TABLE `appuserskills` DISABLE KEYS */;
/*!40000 ALTER TABLE `appuserskills` ENABLE KEYS */;

-- Dumping structure for таблиця impressoweb.aspnetroleclaims
CREATE TABLE IF NOT EXISTS `aspnetroleclaims` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `RoleId` varchar(255) NOT NULL,
  `ClaimType` longtext DEFAULT NULL,
  `ClaimValue` longtext DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetRoleClaims_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table impressoweb.aspnetroleclaims: ~0 rows (приблизно)
/*!40000 ALTER TABLE `aspnetroleclaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetroleclaims` ENABLE KEYS */;

-- Dumping structure for таблиця impressoweb.aspnetroles
CREATE TABLE IF NOT EXISTS `aspnetroles` (
  `Id` varchar(255) NOT NULL,
  `Name` varchar(256) DEFAULT NULL,
  `NormalizedName` varchar(256) DEFAULT NULL,
  `ConcurrencyStamp` longtext DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `RoleNameIndex` (`NormalizedName`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table impressoweb.aspnetroles: ~0 rows (приблизно)
/*!40000 ALTER TABLE `aspnetroles` DISABLE KEYS */;
INSERT IGNORE INTO `aspnetroles` (`Id`, `Name`, `NormalizedName`, `ConcurrencyStamp`) VALUES
	('cc434fba-25e8-4464-a9ef-6f863f010885', 'Admins', 'ADMINS', '8c00dc2d-6a1a-458c-b457-669756adb90b');
/*!40000 ALTER TABLE `aspnetroles` ENABLE KEYS */;

-- Dumping structure for таблиця impressoweb.aspnetuserclaims
CREATE TABLE IF NOT EXISTS `aspnetuserclaims` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` varchar(255) NOT NULL,
  `ClaimType` longtext DEFAULT NULL,
  `ClaimValue` longtext DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetUserClaims_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table impressoweb.aspnetuserclaims: ~0 rows (приблизно)
/*!40000 ALTER TABLE `aspnetuserclaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserclaims` ENABLE KEYS */;

-- Dumping structure for таблиця impressoweb.aspnetuserlogins
CREATE TABLE IF NOT EXISTS `aspnetuserlogins` (
  `LoginProvider` varchar(255) NOT NULL,
  `ProviderKey` varchar(255) NOT NULL,
  `ProviderDisplayName` longtext DEFAULT NULL,
  `UserId` varchar(255) NOT NULL,
  PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  KEY `IX_AspNetUserLogins_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table impressoweb.aspnetuserlogins: ~0 rows (приблизно)
/*!40000 ALTER TABLE `aspnetuserlogins` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserlogins` ENABLE KEYS */;

-- Dumping structure for таблиця impressoweb.aspnetuserroles
CREATE TABLE IF NOT EXISTS `aspnetuserroles` (
  `UserId` varchar(255) NOT NULL,
  `RoleId` varchar(255) NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `IX_AspNetUserRoles_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table impressoweb.aspnetuserroles: ~0 rows (приблизно)
/*!40000 ALTER TABLE `aspnetuserroles` DISABLE KEYS */;
INSERT IGNORE INTO `aspnetuserroles` (`UserId`, `RoleId`) VALUES
	('00451e34-4041-4a66-be2b-2f5f0f682386', 'cc434fba-25e8-4464-a9ef-6f863f010885');
/*!40000 ALTER TABLE `aspnetuserroles` ENABLE KEYS */;

-- Dumping structure for таблиця impressoweb.aspnetusers
CREATE TABLE IF NOT EXISTS `aspnetusers` (
  `AccessFailedCount` int(11) NOT NULL,
  `EmailConfirmed` bit(1) NOT NULL,
  `LockoutEnabled` bit(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `PhoneNumberConfirmed` bit(1) NOT NULL,
  `TwoFactorEnabled` bit(1) NOT NULL,
  `Id` varchar(255) NOT NULL,
  `UserName` varchar(256) DEFAULT NULL,
  `NormalizedUserName` varchar(256) DEFAULT NULL,
  `Email` varchar(256) DEFAULT NULL,
  `NormalizedEmail` varchar(256) DEFAULT NULL,
  `PasswordHash` longtext DEFAULT NULL,
  `SecurityStamp` longtext DEFAULT NULL,
  `ConcurrencyStamp` longtext DEFAULT NULL,
  `PhoneNumber` longtext DEFAULT NULL,
  `Photo` longtext DEFAULT NULL,
  `CV` longtext DEFAULT NULL,
  `Tokens` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  KEY `EmailIndex` (`NormalizedEmail`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table impressoweb.aspnetusers: ~0 rows (приблизно)
/*!40000 ALTER TABLE `aspnetusers` DISABLE KEYS */;
INSERT IGNORE INTO `aspnetusers` (`AccessFailedCount`, `EmailConfirmed`, `LockoutEnabled`, `LockoutEnd`, `PhoneNumberConfirmed`, `TwoFactorEnabled`, `Id`, `UserName`, `NormalizedUserName`, `Email`, `NormalizedEmail`, `PasswordHash`, `SecurityStamp`, `ConcurrencyStamp`, `PhoneNumber`, `Photo`, `CV`, `Tokens`) VALUES
	(0, b'1', b'1', NULL, b'0', b'0', '00451e34-4041-4a66-be2b-2f5f0f682386', 'Admin', 'ADMIN', 'Admin@gmail.com', 'ADMIN@GMAIL.COM', 'AQAAAAEAACcQAAAAEG/LxHgP1saEhgs/3Kq/WiXQeKQ1pCBdWUZaw4EWIce+ZvghgTqGYdPkUgV660M7hA==', 'L55H22OEWHLOZHCMC5GUYWIVRMLCQ5AR', '85ef41f6-04cc-4cd7-9b39-fb45ca93ba41', NULL, NULL, NULL, 0);
/*!40000 ALTER TABLE `aspnetusers` ENABLE KEYS */;

-- Dumping structure for таблиця impressoweb.aspnetusertokens
CREATE TABLE IF NOT EXISTS `aspnetusertokens` (
  `UserId` varchar(255) NOT NULL,
  `LoginProvider` varchar(255) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Value` longtext DEFAULT NULL,
  PRIMARY KEY (`UserId`,`LoginProvider`,`Name`),
  CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table impressoweb.aspnetusertokens: ~0 rows (приблизно)
/*!40000 ALTER TABLE `aspnetusertokens` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetusertokens` ENABLE KEYS */;

-- Dumping structure for таблиця impressoweb.companies
CREATE TABLE IF NOT EXISTS `companies` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` longtext DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table impressoweb.companies: ~0 rows (приблизно)
/*!40000 ALTER TABLE `companies` DISABLE KEYS */;
/*!40000 ALTER TABLE `companies` ENABLE KEYS */;

-- Dumping structure for таблиця impressoweb.companyappusers
CREATE TABLE IF NOT EXISTS `companyappusers` (
  `CompanyId` int(11) NOT NULL,
  `AppUserId` varchar(255) NOT NULL,
  PRIMARY KEY (`CompanyId`,`AppUserId`),
  KEY `IX_CompanyAppUsers_AppUserId` (`AppUserId`),
  CONSTRAINT `FK_CompanyAppUsers_AspNetUsers_AppUserId` FOREIGN KEY (`AppUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_CompanyAppUsers_Companies_CompanyId` FOREIGN KEY (`CompanyId`) REFERENCES `companies` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table impressoweb.companyappusers: ~0 rows (приблизно)
/*!40000 ALTER TABLE `companyappusers` DISABLE KEYS */;
/*!40000 ALTER TABLE `companyappusers` ENABLE KEYS */;

-- Dumping structure for таблиця impressoweb.interviews
CREATE TABLE IF NOT EXISTS `interviews` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AppUserId` varchar(255) NOT NULL,
  `JobId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Interviews_AppUserId` (`AppUserId`),
  KEY `IX_Interviews_JobId` (`JobId`),
  CONSTRAINT `FK_Interviews_AspNetUsers_AppUserId` FOREIGN KEY (`AppUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Interviews_Jobs_JobId` FOREIGN KEY (`JobId`) REFERENCES `jobs` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table impressoweb.interviews: ~0 rows (приблизно)
/*!40000 ALTER TABLE `interviews` DISABLE KEYS */;
/*!40000 ALTER TABLE `interviews` ENABLE KEYS */;

-- Dumping structure for таблиця impressoweb.jobs
CREATE TABLE IF NOT EXISTS `jobs` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` longtext DEFAULT NULL,
  `Description` longtext DEFAULT NULL,
  `CompanyId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Jobs_CompanyId` (`CompanyId`),
  CONSTRAINT `FK_Jobs_Companies_CompanyId` FOREIGN KEY (`CompanyId`) REFERENCES `companies` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table impressoweb.jobs: ~0 rows (приблизно)
/*!40000 ALTER TABLE `jobs` DISABLE KEYS */;
/*!40000 ALTER TABLE `jobs` ENABLE KEYS */;

-- Dumping structure for таблиця impressoweb.jobskills
CREATE TABLE IF NOT EXISTS `jobskills` (
  `JobId` int(11) NOT NULL,
  `SkillId` int(11) NOT NULL,
  PRIMARY KEY (`JobId`,`SkillId`),
  KEY `IX_JobSkills_SkillId` (`SkillId`),
  CONSTRAINT `FK_JobSkills_Jobs_JobId` FOREIGN KEY (`JobId`) REFERENCES `jobs` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_JobSkills_Skills_SkillId` FOREIGN KEY (`SkillId`) REFERENCES `skills` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table impressoweb.jobskills: ~0 rows (приблизно)
/*!40000 ALTER TABLE `jobskills` DISABLE KEYS */;
/*!40000 ALTER TABLE `jobskills` ENABLE KEYS */;

-- Dumping structure for таблиця impressoweb.loggs
CREATE TABLE IF NOT EXISTS `loggs` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Message` longtext DEFAULT NULL,
  `ExceptionMessage` longtext DEFAULT NULL,
  `StackTrace` longtext DEFAULT NULL,
  `InnerExceptionMessage` longtext DEFAULT NULL,
  `Date` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

-- Dumping data for table impressoweb.loggs: ~0 rows (приблизно)
/*!40000 ALTER TABLE `loggs` DISABLE KEYS */;
INSERT IGNORE INTO `loggs` (`Id`, `Message`, `ExceptionMessage`, `StackTrace`, `InnerExceptionMessage`, `Date`) VALUES
	(1, 'A test error', 'Exception of type \'System.Exception\' was thrown.', '   at Web.Controllers.AccountController.Login(String returnUrl) in D:\\Projects\\Impresso\\ImpressoWeb\\Web\\Controllers\\AccountController.cs:line 40', '', '2018-06-21 13:53:02.182302'),
	(2, '', 'Attempted to divide by zero.', '   at Web.Controllers.AccountController.Login(AuthorizationViewModel details, String returnUrl) in D:\\Projects\\Impresso\\ImpressoWeb\\Web\\Controllers\\AccountController.cs:line 46\r\n   at Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker.InvokeActionMethodAsync()\r\n   at Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker.InvokeNextActionFilterAsync()\r\n   at Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker.Rethrow(ActionExecutedContext context)\r\n   at Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker.InvokeInnerFilterAsync()\r\n   at Microsoft.AspNetCore.Mvc.Internal.ResourceInvoker.InvokeNextResourceFilter()\r\n   at Microsoft.AspNetCore.Mvc.Internal.ResourceInvoker.Rethrow(ResourceExecutedContext context)\r\n   at Microsoft.AspNetCore.Mvc.Internal.ResourceInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Internal.ResourceInvoker.InvokeFilterPipelineAsync()\r\n   at Microsoft.AspNetCore.Mvc.Internal.ResourceInvoker.InvokeAsync()\r\n   at Microsoft.AspNetCore.Builder.RouterMiddleware.Invoke(HttpContext httpContext)\r\n   at Web.Infrastructure.ErrorHandlingMiddleware.Invoke(HttpContext context, ILoggerFactory loggerFactory) in D:\\Projects\\Impresso\\ImpressoWeb\\Web\\Infrastructure\\ErrorHandlingMiddleware.cs:line 23', '', '2018-06-21 14:03:29.559036');
/*!40000 ALTER TABLE `loggs` ENABLE KEYS */;

-- Dumping structure for таблиця impressoweb.notifications
CREATE TABLE IF NOT EXISTS `notifications` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AppUserId` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Notifications_AppUserId` (`AppUserId`),
  CONSTRAINT `FK_Notifications_AspNetUsers_AppUserId` FOREIGN KEY (`AppUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table impressoweb.notifications: ~0 rows (приблизно)
/*!40000 ALTER TABLE `notifications` DISABLE KEYS */;
/*!40000 ALTER TABLE `notifications` ENABLE KEYS */;

-- Dumping structure for таблиця impressoweb.projectappusers
CREATE TABLE IF NOT EXISTS `projectappusers` (
  `ProjectId` int(11) NOT NULL,
  `AppUserId` varchar(255) NOT NULL,
  PRIMARY KEY (`ProjectId`,`AppUserId`),
  KEY `IX_ProjectAppUsers_AppUserId` (`AppUserId`),
  CONSTRAINT `FK_ProjectAppUsers_AspNetUsers_AppUserId` FOREIGN KEY (`AppUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_ProjectAppUsers_Projects_ProjectId` FOREIGN KEY (`ProjectId`) REFERENCES `projects` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table impressoweb.projectappusers: ~0 rows (приблизно)
/*!40000 ALTER TABLE `projectappusers` DISABLE KEYS */;
/*!40000 ALTER TABLE `projectappusers` ENABLE KEYS */;

-- Dumping structure for таблиця impressoweb.projects
CREATE TABLE IF NOT EXISTS `projects` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` longtext DEFAULT NULL,
  `Description` longtext DEFAULT NULL,
  `CompanyId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Projects_CompanyId` (`CompanyId`),
  CONSTRAINT `FK_Projects_Companies_CompanyId` FOREIGN KEY (`CompanyId`) REFERENCES `companies` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table impressoweb.projects: ~0 rows (приблизно)
/*!40000 ALTER TABLE `projects` DISABLE KEYS */;
/*!40000 ALTER TABLE `projects` ENABLE KEYS */;

-- Dumping structure for таблиця impressoweb.skills
CREATE TABLE IF NOT EXISTS `skills` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` longtext DEFAULT NULL,
  `Description` longtext DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table impressoweb.skills: ~0 rows (приблизно)
/*!40000 ALTER TABLE `skills` DISABLE KEYS */;
/*!40000 ALTER TABLE `skills` ENABLE KEYS */;

-- Dumping structure for таблиця impressoweb.__efmigrationshistory
CREATE TABLE IF NOT EXISTS `__efmigrationshistory` (
  `MigrationId` varchar(95) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table impressoweb.__efmigrationshistory: ~1 rows (приблизно)
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT IGNORE INTO `__efmigrationshistory` (`MigrationId`, `ProductVersion`) VALUES
	('20180621102230_INITIAL', '2.1.1-rtm-30846');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
