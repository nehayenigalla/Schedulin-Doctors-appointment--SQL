using System;
using MySql.Data.MySqlClient;
class AppointmentSystem
{
    static void Main(string[] args)
    {
        Console.WriteLine("Doctors Appointment Management System");
        Console.WriteLine("Enter your role (Admin/Doctor/Patient): ");
        string userRole = Console.ReadLine().ToLower();

        switch (userRole)
        {
            case "admin":
                AdminInterface();
                break;
            case "doctor":
                DoctorInterface();
                break;
            case "patient":
                PatientInterface();
                break;
            default:
                Console.WriteLine("Invalid role");
                break;
        }
    }
    static void AdminInterface()
    {
        Console.WriteLine("Admin Interface");
        Console.WriteLine("1. Add Patient");
        Console.WriteLine("2. Update Patient");
        Console.WriteLine("3. Delete Patient");
        Console.WriteLine("4. View All Patients");
        // Similar options for Doctors, Appointments, and Staff
        Console.WriteLine("Enter your choice: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                AddPatient();
                break;
            case "2":
                UpdatePatient();
                break;
            case "3":
                DeletePatient();
                break;
            case "4":
                ViewAllPatients();
                break;
            // Implement other cases similarly
            default:
                Console.WriteLine("Invalid choice");
                break;
        }
    }

    static void AddPatient()
    {
        Console.WriteLine("Enter patient's name: ");
        string name = Console.ReadLine();
        Console.WriteLine("Enter patient's age: ");
        int age = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Enter patient's gender: ");
        string gender = Console.ReadLine();
        Console.WriteLine("Enter patient's phone: ");
        string phone = Console.ReadLine();
        Console.WriteLine("Enter patient's email: ");
        string email = Console.ReadLine();

        string connectionString = "server=localhost;port=3306;database=doctorsappointment;user=root;password=root;";
        string query = "INSERT INTO Patients (Name, Age, Gender, Phone, Email) VALUES (@Name, @Age, @Gender, @Phone, @Email)";

        using (var connection = new MySqlConnection(connectionString))
        {
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Age", age);
                command.Parameters.AddWithValue("@Gender", gender);
                command.Parameters.AddWithValue("@Phone", phone);
                command.Parameters.AddWithValue("@Email", email);

                connection.Open();
                int result = command.ExecuteNonQuery();

                if (result < 0)
                    Console.WriteLine("Error inserting data into Database!");
                else
                    Console.WriteLine("Patient added successfully!");
            }
        }
    }


    static void ViewAllPatients()
    {
        string connectionString = "server=localhost;port=3306;database=doctorsappointment;user=root;password=root;";
        string query = "SELECT * FROM Patients";

        using (var connection = new MySqlConnection(connectionString))
        {
            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("No patients found.");
                        return;
                    }

                    Console.WriteLine("Patients:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"Patient ID: {reader["PatientID"]}, Name: {reader["Name"]}, Age: {reader["Age"]}, Gender: {reader["Gender"]}, Phone: {reader["Phone"]}, Email: {reader["Email"]}");
                    }
                }
            }
        }
    }

    static void UpdatePatient()
    {
        Console.WriteLine("Enter patient's ID to update: ");
        int patientId = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Which fields would you like to update? (Name, Age, Gender, Phone, Email)");
        Console.WriteLine("Enter the fields separated by commas (e.g., Name,Email): ");
        string fieldsToUpdate = Console.ReadLine();
        var fields = fieldsToUpdate.Split(',').Select(field => field.Trim()).ToList();

        string connectionString = "server=localhost;port=3306;database=doctorsappointment;user=root;password=root;";

        var queryBuilder = new List<string>();

        if (fields.Contains("Name"))
        {
            Console.WriteLine("Enter new name: ");
            string newName = Console.ReadLine();
            queryBuilder.Add($"Name = '{newName}'");
        }

        if (fields.Contains("Age"))
        {
            Console.WriteLine("Enter new age: ");
            int newAge = Convert.ToInt32(Console.ReadLine());
            queryBuilder.Add($"Age = {newAge}");
        }

        if (fields.Contains("Gender"))
        {
            Console.WriteLine("Enter new gender: ");
            string newGender = Console.ReadLine();
            queryBuilder.Add($"Gender = '{newGender}'");
        }

        if (fields.Contains("Phone"))
        {
            Console.WriteLine("Enter new phone number: ");
            string newPhone = Console.ReadLine();
            queryBuilder.Add($"Phone = '{newPhone}'");
        }

        if (fields.Contains("Email"))
        {
            Console.WriteLine("Enter new email: ");
            string newEmail = Console.ReadLine();
            queryBuilder.Add($"Email = '{newEmail}'");
        }

        string setClause = string.Join(", ", queryBuilder);
        string query = $"UPDATE Patients SET {setClause} WHERE PatientID = @PatientID";

        using (var connection = new MySqlConnection(connectionString))
        {
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PatientID", patientId);

                connection.Open();
                int result = command.ExecuteNonQuery();

                if (result < 0)
                    Console.WriteLine("Error updating patient in Database!");
                else
                    Console.WriteLine("Patient updated successfully!");
            }
        }
    }


    static void DeletePatient()
    {
        Console.WriteLine("Enter patient's ID to delete: ");
        int patientId = Convert.ToInt32(Console.ReadLine());

        string connectionString = "server=localhost;port=3306;database=doctorsappointment;user=root;password=root;";
        string query = "DELETE FROM Patients WHERE PatientID = @PatientID";

        using (var connection = new MySqlConnection(connectionString))
        {
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PatientID", patientId);

                connection.Open();
                int result = command.ExecuteNonQuery();

                if (result < 0)
                    Console.WriteLine("Error deleting patient from Database!");
                else
                    Console.WriteLine("Patient deleted successfully!");
            }
        }
    }



    static void DoctorInterface()
    {
        Console.WriteLine("Doctor Interface");
        Console.WriteLine("1. View Appointments");
        Console.WriteLine("2. Update Appointment");
        // Additional options as needed
        Console.WriteLine("Enter your choice: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                ViewAppointments();
                break;
            case "2":
                UpdateAppointment();
                break;
            // Additional cases as needed
            default:
                Console.WriteLine("Invalid choice");
                break;
        }
    }

    static void ViewAppointments()
    {
        Console.WriteLine("Enter doctor's ID to view appointments: ");
        int doctorId = Convert.ToInt32(Console.ReadLine());

        string connectionString = "server=localhost;port=3306;database=doctorsappointment;user=root;password=root;";
        string query = "SELECT * FROM Appointments WHERE DoctorID = @DoctorID";

        using (var connection = new MySqlConnection(connectionString))
        {
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DoctorID", doctorId);

                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Appointment ID: {reader["AppointmentID"]}, Date: {reader["Date"]}, Time: {reader["Time"]}, Patient ID: {reader["PatientID"]}");
                    }
                }
            }
        }
    }


    static void UpdateAppointment()
    {
        Console.WriteLine("Enter doctor's ID to view their appointments: ");
        int doctorId = Convert.ToInt32(Console.ReadLine());

        string connectionString = "server=localhost;port=3306;database=doctorsappointment;user=root;password=root;";

        string queryFetch = "SELECT * FROM Appointments WHERE DoctorID = @DoctorID";

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            using (var commandFetch = new MySqlCommand(queryFetch, connection))
            {
                commandFetch.Parameters.AddWithValue("@DoctorID", doctorId);

                using (MySqlDataReader reader = commandFetch.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("No appointments found for this doctor.");
                        return;
                    }

                    Console.WriteLine("Appointments for Doctor ID " + doctorId + ":");
                    while (reader.Read())
                    {
                        Console.WriteLine($"Appointment ID: {reader["AppointmentID"]}, Date: {reader["Date"]}, Time: {reader["Time"]}, Patient ID: {reader["PatientID"]}");
                    }
                }
            }
        }

        Console.WriteLine("Enter appointment ID to update: ");
        int appointmentId = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Enter new date (yyyy-mm-dd): ");
        string newDate = Console.ReadLine();
        Console.WriteLine("Enter new time (HH:MM): ");
        string newTime = Console.ReadLine();

        string queryUpdate = "UPDATE Appointments SET Date = @Date, Time = @Time WHERE AppointmentID = @AppointmentID";

        using (var connection = new MySqlConnection(connectionString))
        {
            using (var commandUpdate = new MySqlCommand(queryUpdate, connection))
            {
                commandUpdate.Parameters.AddWithValue("@Date", newDate);
                commandUpdate.Parameters.AddWithValue("@Time", newTime);
                commandUpdate.Parameters.AddWithValue("@AppointmentID", appointmentId);

                connection.Open();
                int result = commandUpdate.ExecuteNonQuery();

                if (result < 0)
                    Console.WriteLine("Error updating appointment in Database!");
                else
                    Console.WriteLine("Appointment updated successfully!");
            }
        }
    }

   

    static void PatientInterface()
    {
        Console.WriteLine("Patient Interface");
        Console.WriteLine("1. Book Appointment");
        Console.WriteLine("2. View My Appointments");
        Console.WriteLine("3. Cancel Appointment");
        Console.WriteLine("Enter your choice: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                BookAppointment();
                break;
            case "2":
                ViewMyAppointments();
                break;
            case "3":
                CancelAppointment();
                break;
            default:
                Console.WriteLine("Invalid choice");
                break;
        }
    }

    static void BookAppointment()
    {
        Console.WriteLine("Enter patient ID: ");
        int patientId = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Enter doctor ID: ");
        int doctorId = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Enter date (yyyy-mm-dd): ");
        string date = Console.ReadLine();
        Console.WriteLine("Enter time (HH:MM): ");
        string time = Console.ReadLine();
        Console.WriteLine("Enter reason for the appointment: ");
        string reason = Console.ReadLine();

        string connectionString = "server=localhost;port=3306;database=doctorsappointment;user=root;password=root;";
        string query = "INSERT INTO Appointments (Date, Time, PatientID, DoctorID, Reason) VALUES (@Date, @Time, @PatientID, @DoctorID, @Reason)";

        using (var connection = new MySqlConnection(connectionString))
        {
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Date", date);
                command.Parameters.AddWithValue("@Time", time);
                command.Parameters.AddWithValue("@PatientID", patientId);
                command.Parameters.AddWithValue("@DoctorID", doctorId);
                command.Parameters.AddWithValue("@Reason", reason);

                connection.Open();
                int result = command.ExecuteNonQuery();

                if (result < 0)
                    Console.WriteLine("Error booking appointment in Database!");
                else
                    Console.WriteLine("Appointment booked successfully!");
            }
        }
    }

    static void ViewMyAppointments()
    {
        Console.WriteLine("Enter your patient ID: ");
        int patientId = Convert.ToInt32(Console.ReadLine());

        string connectionString = "server=localhost;port=3306;database=doctorsappointment;user=root;password=root;";
        string query = "SELECT * FROM Appointments WHERE PatientID = @PatientID";

        using (var connection = new MySqlConnection(connectionString))
        {
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PatientID", patientId);

                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Appointment ID: {reader["AppointmentID"]}, Date: {reader["Date"]}, Time: {reader["Time"]}, Doctor ID: {reader["DoctorID"]}");
                    }
                }
            }
        }
    }


    static void CancelAppointment()
    {
        Console.WriteLine("Enter your appointment ID to cancel: ");
        int appointmentId = Convert.ToInt32(Console.ReadLine());

        string connectionString = "server=localhost;port=3306;database=doctorsappointment;user=root;password=root;";
        string query = "DELETE FROM Appointments WHERE AppointmentID = @AppointmentID";

        using (var connection = new MySqlConnection(connectionString))
        {
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@AppointmentID", appointmentId);

                connection.Open();
                int result = command.ExecuteNonQuery();

                if (result < 0)
                    Console.WriteLine("Error cancelling appointment in Database!");
                else
                    Console.WriteLine("Appointment cancelled successfully!");
            }
        }
    }

}
