 use doctorsappointment;
CREATE TABLE Patients (
    PatientID INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(255),
    Age INT,
    Gender VARCHAR(10),
    Phone VARCHAR(15),
    Email VARCHAR(255)
);
INSERT INTO Patients (Name, Age, Gender, Phone, Email) VALUES 
('John Doe', 30, 'Male', '123-456-7890', 'john.doe@example.com'),
('Jane Smith', 25, 'Female', '234-567-8901', 'jane.smith@example.com'),
('Alice Johnson', 40, 'Female', '345-678-9012', 'alice.johnson@example.com'),
('Robert Brown', 50, 'Male', '456-789-0123', 'robert.brown@example.com'),
('Mary Davis', 35, 'Female', '567-890-1234', 'mary.davis@example.com'),
('Michael Wilson', 60, 'Male', '678-901-2345', 'michael.wilson@example.com'),
('Linda Martinez', 28, 'Female', '789-012-3456', 'linda.martinez@example.com'),
('James Taylor', 45, 'Male', '890-123-4567', 'james.taylor@example.com');

select * from patients;

CREATE TABLE Doctors (
    DoctorID INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(255),
    Specialization VARCHAR(255),
    Phone VARCHAR(15),
    Email VARCHAR(255)
);
INSERT INTO Doctors (Name, Specialization, Phone, Email) VALUES 
('Dr. Emma Thomas', 'Cardiologist', '901-234-5678', 'emma.thomas@healthcare.com'),
('Dr. Noah Hernandez', 'Dermatologist', '012-345-6789', 'noah.hernandez@healthcare.com'),
('Dr. Olivia Garcia', 'Pediatrician', '123-456-7890', 'olivia.garcia@healthcare.com'),
('Dr. William Martinez', 'Neurologist', '234-567-8901', 'william.martinez@healthcare.com'),
('Dr. Sophia Rodriguez', 'Oncologist', '345-678-9012', 'sophia.rodriguez@healthcare.com'),
('Dr. Benjamin Lee', 'Orthopedic', '456-789-0123', 'benjamin.lee@healthcare.com'),
('Dr. Isabella Gonzalez', 'Psychiatrist', '567-890-1234', 'isabella.gonzalez@healthcare.com'),
('Dr. Lucas Perez', 'General Practitioner', '678-901-2345', 'lucas.perez@healthcare.com');
select * from doctors;

CREATE TABLE Appointments (
    AppointmentID INT AUTO_INCREMENT PRIMARY KEY,
    Date DATE,
    Time TIME,
    PatientID INT,
    DoctorID INT,
    Reason TEXT,
    FOREIGN KEY (PatientID) REFERENCES Patients(PatientID),
    FOREIGN KEY (DoctorID) REFERENCES Doctors(DoctorID)
);
INSERT INTO Appointments (Date, Time, PatientID, DoctorID, Reason) VALUES 
('2023-11-01', '09:00', 1, 4, 'Routine Checkup'),
('2023-11-01', '10:00', 2, 1, 'Skin Rash'),
('2023-11-01', '11:00', 3, 2, 'Child Vaccination'),
('2023-11-01', '13:00', 4, 3, 'Headache'),
('2023-11-02', '09:00', 5, 5, 'Annual Cancer Screening'),
('2023-11-02', '10:00', 6, 6, 'Knee Pain'),
('2023-11-02', '11:00', 7, 7, 'Mental Health Consultation'),
('2023-11-02', '13:00', 8, 8, 'General Checkup');
select * from appointments;

CREATE TABLE Staff (
    StaffID INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(255),
    Role VARCHAR(255),
    Phone VARCHAR(15),
    Email VARCHAR(255)
);
INSERT INTO Staff (Name, Role, Phone, Email) VALUES 
('Sarah Clark', 'Receptionist', '901-234-5678', 'sarah.clark@healthcare.com'),
('Daniel Lewis', 'Nurse', '012-345-6789', 'daniel.lewis@healthcare.com'),
('Elizabeth Walker', 'Administrator', '123-456-7890', 'elizabeth.walker@healthcare.com'),
('Matthew Robinson', 'Lab Technician', '234-567-8901', 'matthew.robinson@healthcare.com'),
('Sofia Hall', 'Billing Specialist', '345-678-9012', 'sofia.hall@healthcare.com'),
('Christopher Young', 'IT Support', '456-789-0123', 'christopher.young@healthcare.com'),
('Grace King', 'Medical Records Clerk', '567-890-1234', 'grace.king@healthcare.com'),
('Alexander Wright', 'Maintenance', '678-901-2345', 'alexander.wright@healthcare.com');
select * from staff;

-- Queries

-- Find Doctors with the Most Appointments on a November 1 2023.
SELECT d.Name, d.Specialization, COUNT(a.AppointmentID) AS NumberOfAppointments
FROM Doctors d
JOIN Appointments a ON d.DoctorID = a.DoctorID
WHERE a.Date = '2023-11-01'
GROUP BY d.DoctorID
ORDER BY NumberOfAppointments DESC;

-- List Patients with Upcoming Appointments Including Doctor Details
SELECT p.Name AS PatientName, p.Age, p.Gender, p.Phone, p.Email, 
d.Name AS DoctorName, d.Specialization, a.Date, a.Time, a.Reason
FROM Patients p
JOIN Appointments AS a ON p.PatientID = a.PatientID
JOIN Doctors AS d ON a.DoctorID = d.DoctorID
WHERE a.Date >= CURDATE();

-- This query updates the email of patients who have had a routine checkup with a specific doctor
-- Note here i am created a patients copy first and then applied query so that it wont affect the actual table.
CREATE TABLE PatientsCopy AS SELECT * FROM Patients;
UPDATE PatientsCopy 
SET Email = CONCAT(Email, '_checked')
WHERE PatientID IN (
    SELECT a.PatientID 
    FROM Appointments a
    JOIN Doctors d ON a.DoctorID = d.DoctorID
    WHERE d.DoctorID = 4 AND a.Reason = 'Routine Checkup'
);
select * from PatientsCopy;
DROP TABLE PatientsCopy; -- if we want to drop table


-- Find the Average Number of Appointments per Doctor
SELECT AVG(AppointmentCount) AS AverageAppointmentsPerDoctor
FROM (SELECT DoctorID, COUNT(*) AS AppointmentCount
      FROM Appointments
      GROUP BY DoctorID) AS SubQuery;
      
-- Doctors Specialization and Their Last Appointment Date  
SELECT d.Name, d.Specialization, MAX(a.Date) AS LastAppointmentDate
FROM Doctors d
LEFT JOIN Appointments a ON d.DoctorID = a.DoctorID
GROUP BY d.DoctorID;

