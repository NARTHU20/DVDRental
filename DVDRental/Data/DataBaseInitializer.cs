using Microsoft.Data.SqlClient;

namespace DVDRental.Data
{
    public class DataBaseInitializer
    {

        private readonly string _ConnectionString;

        public DataBaseInitializer(string connectionString)
        {
            _ConnectionString = connectionString;
        }

        public void Initialize()
        {
            using (var connection = new SqlConnection(_ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                                       IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DVDs')
                                    BEGIN
                                        CREATE TABLE DVDs (
                                            Id NVARCHAR(20) PRIMARY KEY,
                                            Title NVARCHAR(100) NOT NULL,
                                            Director  NVARCHAR(50),
                                            ReleaseDate DATE,
                                            AvailableCopies INT NOT NULL,
                                            ImagePath NVARCHAR(255)
                                        );
                                    END;
                                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Categories')
                                    BEGIN
                                        CREATE TABLE Categories (
                                            CategoryId INT PRIMARY KEY IDENTITY(1,1),
                                            Name NVARCHAR(50) NOT NULL UNIQUE
                                        );
                                    END;

                                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DVD_Categories')
                                     BEGIN
                                        CREATE TABLE DVD_Categories (
                                            DVDId NVARCHAR(20) NOT NULL,
                                            CategoryId INT NOT NULL,
                                            PRIMARY KEY (DVDId, CategoryId),
                                            FOREIGN KEY (DVDId) REFERENCES DVDs(Id) ON DELETE CASCADE,
                                            FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId) ON DELETE CASCADE
                                        );
                                        END;

                                     IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Customers')
                                     BEGIN
                                        CREATE TABLE Customers (
                                            Id INT PRIMARY KEY IDENTITY(1,1),
                                            FirstName NVARCHAR(50) NOT NULL,
                                            LastName NVARCHAR(50) NOT NULL,
                                            Email NVARCHAR(100) UNIQUE NOT NULL,
                                            PhoneNumber NVARCHAR(20)
                                        );
                                     END;

                                     IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Requests')
                                    BEGIN
                                        CREATE TABLE Requests (
                                            Id INT PRIMARY KEY IDENTITY(1,1),
                                            CustomerId INT NOT NULL,
                                            DVDId NVARCHAR(20) NOT NULL,
                                            RequestDate DATETIME NOT NULL,
                                            Status NVARCHAR(20) NOT NULL, -- 'Pending', 'Accepted', 'Rejected'
                                            FOREIGN KEY (CustomerId) REFERENCES Customers(Id),
                                            FOREIGN KEY (DVDId) REFERENCES DVDs(Id)
                                        );
                                    END;

                                 IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Rentals')
                                  BEGIN
                                    CREATE TABLE Rentals (
                                        Id INT PRIMARY KEY IDENTITY(1,1),
                                        CustomerId INT NOT NULL,
                                        DVDId NVARCHAR(20) NOT NULL,
                                        RentalDate DATE NOT NULL,
                                        DueDate DATE NOT NULL,
                                        ReturnDate DATE,
                                        RentalCharge DECIMAL(10, 2) NOT NULL,
                                        AdvancePayment DECIMAL(10, 2) NOT NULL,
                                        DelayFine DECIMAL(10, 2),
                                        DamageFine DECIMAL(10, 2),
                                        Balance DECIMAL(10, 2),
                                        Status NVARCHAR(20) NOT NULL, -- 'Active', 'Returned', 'Overdue'
                                        FOREIGN KEY (CustomerId) REFERENCES Customers(Id),
                                        FOREIGN KEY (DVDId) REFERENCES DVDs(Id)
                                    );
                                 END;
                                ";
                        command.ExecuteNonQuery();
        }
    }
}
}
