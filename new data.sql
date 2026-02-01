-- Clear existing data (optional, for safety if running repeatedly on dev db)
-- DELETE FROM Enrollment;
-- DELETE FROM CourseAssignment;
-- DELETE FROM OfficeAssignment;
-- DELETE FROM Course;
-- DELETE FROM Department;
-- DELETE FROM Instructor;
-- DELETE FROM Student;

-- Students
INSERT INTO Student (LastName, FirstName, EnrollmentDate) VALUES ('Alexander', 'Carson', '2023-09-01');
INSERT INTO Student (LastName, FirstName, EnrollmentDate) VALUES ('Alonso', 'Meredith', '2022-09-01');
INSERT INTO Student (LastName, FirstName, EnrollmentDate) VALUES ('Anand', 'Arturo', '2023-09-01');
INSERT INTO Student (LastName, FirstName, EnrollmentDate) VALUES ('Barzdukas', 'Gytis', '2022-09-01');
INSERT INTO Student (LastName, FirstName, EnrollmentDate) VALUES ('Li', 'Yan', '2022-09-01');
INSERT INTO Student (LastName, FirstName, EnrollmentDate) VALUES ('Justice', 'Peggy', '2021-09-01');
INSERT INTO Student (LastName, FirstName, EnrollmentDate) VALUES ('Norman', 'Laura', '2023-09-01');
INSERT INTO Student (LastName, FirstName, EnrollmentDate) VALUES ('Olivetto', 'Nino', '2019-09-01');

-- Instructors
INSERT INTO Instructor (LastName, FirstName, HireDate) VALUES ('Abercrombie', 'Kim', '1995-03-11');
INSERT INTO Instructor (LastName, FirstName, HireDate) VALUES ('Fakhouri', 'Fadi', '2002-07-06');
INSERT INTO Instructor (LastName, FirstName, HireDate) VALUES ('Harui', 'Roger', '1998-07-01');
INSERT INTO Instructor (LastName, FirstName, HireDate) VALUES ('Kapoor', 'Candace', '2001-01-15');
INSERT INTO Instructor (LastName, FirstName, HireDate) VALUES ('Zheng', 'Roger', '2004-02-12');

-- Variables for IDs to allow script to be more robust (SQL Server specific T-SQL)
DECLARE @InstructorKimID int, @InstructorFadiID int, @InstructorRogerHID int, @InstructorCandaceID int, @InstructorRogerZID int;
SELECT @InstructorKimID = Id FROM Instructor WHERE LastName = 'Abercrombie' AND FirstName = 'Kim';
SELECT @InstructorFadiID = Id FROM Instructor WHERE LastName = 'Fakhouri' AND FirstName = 'Fadi';
SELECT @InstructorRogerHID = Id FROM Instructor WHERE LastName = 'Harui' AND FirstName = 'Roger';
SELECT @InstructorCandaceID = Id FROM Instructor WHERE LastName = 'Kapoor' AND FirstName = 'Candace';
SELECT @InstructorRogerZID = Id FROM Instructor WHERE LastName = 'Zheng' AND FirstName = 'Roger';

-- Departments
-- Note: DepartmentID is Identity, so we let DB assign it.
INSERT INTO Department (Name, Budget, StartDate, InstructorId) VALUES ('English', 350000.00, '2023-09-01', @InstructorKimID);
INSERT INTO Department (Name, Budget, StartDate, InstructorId) VALUES ('Mathematics', 100000.00, '2023-09-01', @InstructorFadiID);
INSERT INTO Department (Name, Budget, StartDate, InstructorId) VALUES ('Engineering', 350000.00, '2023-09-01', @InstructorRogerHID);
INSERT INTO Department (Name, Budget, StartDate, InstructorId) VALUES ('Economics', 100000.00, '2023-09-01', @InstructorCandaceID);

DECLARE @DeptEnglishID int, @DeptMathID int, @DeptEngrID int, @DeptEconID int;
SELECT @DeptEnglishID = DepartmentID FROM Department WHERE Name = 'English';
SELECT @DeptMathID = DepartmentID FROM Department WHERE Name = 'Mathematics';
SELECT @DeptEngrID = DepartmentID FROM Department WHERE Name = 'Engineering';
SELECT @DeptEconID = DepartmentID FROM Department WHERE Name = 'Economics';

-- Courses
-- CourseID is explicit (not identity)
INSERT INTO Course (CourseID, Title, Credits, DepartmentId) VALUES (1050, 'Chemistry', 3, @DeptEngrID);
INSERT INTO Course (CourseID, Title, Credits, DepartmentId) VALUES (4022, 'Microeconomics', 3, @DeptEconID);
INSERT INTO Course (CourseID, Title, Credits, DepartmentId) VALUES (4041, 'Macroeconomics', 3, @DeptEconID);
INSERT INTO Course (CourseID, Title, Credits, DepartmentId) VALUES (1045, 'Calculus', 4, @DeptMathID);
INSERT INTO Course (CourseID, Title, Credits, DepartmentId) VALUES (3141, 'Trigonometry', 4, @DeptMathID);
INSERT INTO Course (CourseID, Title, Credits, DepartmentId) VALUES (2021, 'Composition', 3, @DeptEnglishID);
INSERT INTO Course (CourseID, Title, Credits, DepartmentId) VALUES (2042, 'Literature', 4, @DeptEnglishID);

-- Office Assignments
INSERT INTO OfficeAssignment (InstructorId, Location) VALUES (@InstructorFadiID, 'Smith 17');
INSERT INTO OfficeAssignment (InstructorId, Location) VALUES (@InstructorRogerHID, 'Gowan 27');
INSERT INTO OfficeAssignment (InstructorId, Location) VALUES (@InstructorCandaceID, 'Thompson 304');

-- Course Assignments (Instructor - Course)
INSERT INTO CourseAssignment (InstructorId, CourseId) VALUES (@InstructorKimID, 1050); -- Chemistry
INSERT INTO CourseAssignment (InstructorId, CourseId) VALUES (@InstructorKimID, 2021); -- Composition
INSERT INTO CourseAssignment (InstructorId, CourseId) VALUES (@InstructorFadiID, 2021); -- Composition
INSERT INTO CourseAssignment (InstructorId, CourseId) VALUES (@InstructorRogerHID, 1045); -- Calculus
INSERT INTO CourseAssignment (InstructorId, CourseId) VALUES (@InstructorRogerHID, 3141); -- Trigonometry
INSERT INTO CourseAssignment (InstructorId, CourseId) VALUES (@InstructorCandaceID, 4022); -- Microeconomics
INSERT INTO CourseAssignment (InstructorId, CourseId) VALUES (@InstructorCandaceID, 4041); -- Macroeconomics
INSERT INTO CourseAssignment (InstructorId, CourseId) VALUES (@InstructorRogerZID, 1045); -- Calculus - Team taught? Or maybe just extra assignment

-- Enrollments
DECLARE @StudentAlexanderID int, @StudentAlonsoID int, @StudentAnandID int, @StudentBarzdukasID int, @StudentLiID int, @StudentJusticeID int, @StudentNormanID int, @StudentOlivettoID int;
SELECT @StudentAlexanderID = Id FROM Student WHERE LastName = 'Alexander';
SELECT @StudentAlonsoID = Id FROM Student WHERE LastName = 'Alonso';
SELECT @StudentAnandID = Id FROM Student WHERE LastName = 'Anand';
SELECT @StudentBarzdukasID = Id FROM Student WHERE LastName = 'Barzdukas';
SELECT @StudentLiID = Id FROM Student WHERE LastName = 'Li';
SELECT @StudentJusticeID = Id FROM Student WHERE LastName = 'Justice';
SELECT @StudentNormanID = Id FROM Student WHERE LastName = 'Norman';
SELECT @StudentOlivettoID = Id FROM Student WHERE LastName = 'Olivetto';

-- Grades: 0=A, 1=B, 2=C, 3=D, 4=F
INSERT INTO Enrollment (StudentId, CourseId, Grade) VALUES (@StudentAlexanderID, 1050, 0); -- A
INSERT INTO Enrollment (StudentId, CourseId, Grade) VALUES (@StudentAlexanderID, 4022, 2); -- C
INSERT INTO Enrollment (StudentId, CourseId, Grade) VALUES (@StudentAlexanderID, 4041, 1); -- B

INSERT INTO Enrollment (StudentId, CourseId, Grade) VALUES (@StudentAlonsoID, 1045, 1); -- B
INSERT INTO Enrollment (StudentId, CourseId, Grade) VALUES (@StudentAlonsoID, 3141, 1); -- B
INSERT INTO Enrollment (StudentId, CourseId, Grade) VALUES (@StudentAlonsoID, 2021, 1); -- B

INSERT INTO Enrollment (StudentId, CourseId, Grade) VALUES (@StudentAnandID, 1050, 1); -- B
INSERT INTO Enrollment (StudentId, CourseId, Grade) VALUES (@StudentAnandID, 1045, 1); -- B 

INSERT INTO Enrollment (StudentId, CourseId, Grade) VALUES (@StudentBarzdukasID, 1050, 1); -- B
INSERT INTO Enrollment (StudentId, CourseId, Grade) VALUES (@StudentLiID, 4022, 1); -- B
INSERT INTO Enrollment (StudentId, CourseId, Grade) VALUES (@StudentJusticeID, 4041, 1); -- B
