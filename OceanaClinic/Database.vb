'https://www.youtube.com/watch?v=UN7nMNVw2hk SQLite with Visual Studio
Imports System.Data.SQLite
Public Class Database
    Private Shared location As String = "data/"
    Private Shared filename As String = "main.db"
    Private Shared fullpath As String = System.IO.Path.Combine(location, filename)
    Public Shared connectionString As String = String.Format("Data Source = {0}", fullpath)
    Public Sub Init()
        If Not DuplicateDatabase(fullpath) Then
            Dim createUsersTable As String =
                "CREATE TABLE ""Users"" (
					""userId""	    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
					""Firstname""	TEXT,
					""Lastname""	TEXT,
					""Password""	TEXT,
					""Email""	    TEXT UNIQUE COLLATE NOCASE,
					""userGroup""	INTEGER
				);" 'https://stackoverflow.com/questions/1188749/how-to-change-the-collation-of-sqlite3-database-to-sort-case-insensitively
            Dim createPatientsTable As String =
                "CREATE TABLE ""Patients"" (
					""PatientId""	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
					""Firstname""	TEXT,
					""Lastname""	TEXT,
					""Identity""	TEXT UNIQUE COLLATE NOCASE,
					""Mobile""	    TEXT,
					""Address""	    TEXT,
					""Email""	    TEXT,
					""Weight""	    INTEGER DEFAULT 0,
					""Height""	    INTEGER DEFAULT 0,
					""BloodType""	INTEGER DEFAULT -1,
					""Allergies""	TEXT DEFAULT 'None'
				);"
            Dim createBillingItemsTable As String =
                "CREATE TABLE ""BillingItems"" (
	                ""ItemId""	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	                ""Name""	TEXT DEFAULT '<NO_NAME>',
	                ""Type""	INTEGER DEFAULT -1,
	                ""Price""	NUMERIC DEFAULT 0
                );"
            Dim createTransactionsTable As String =
                "CREATE TABLE ""Transactions"" (
                    ""TransactionId""	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    ""ItemId""	        INTEGER NOT NULL,
                    ""PatientId""	    INTEGER NOT NULL,
                    ""Quantity""	    INTEGER DEFAULT 0,
                    ""Completed""	    INTEGER DEFAULT 0,
                    FOREIGN KEY(""ItemId"") REFERENCES ""BillingItems""(""ItemId""),
                    FOREIGN KEY(""PatientId"") REFERENCES ""Patients""(""PatientId"")
                );"

            System.IO.Directory.CreateDirectory(location)
            Using SqlConn As New SQLiteConnection(connectionString)

                Dim createUsersTableCmd As New SQLiteCommand(createUsersTable, SqlConn)
                Dim createPatientTableCmd As New SQLiteCommand(createPatientsTable, SqlConn)
                Dim createBillingItemsTableCmd As New SQLiteCommand(createBillingItemsTable, SqlConn)
                Dim createTransactionsTableCmd As New SQLiteCommand(createTransactionsTable, SqlConn)

                SqlConn.Open()

                createUsersTableCmd.ExecuteNonQuery()
                createPatientTableCmd.ExecuteNonQuery()
                createBillingItemsTableCmd.ExecuteNonQuery()
                createTransactionsTableCmd.ExecuteNonQuery()

                SqlConn.Close()
            End Using
            DummyData()
        End If
    End Sub
    Private Sub DummyData()
        Using conn As New SQLiteConnection(connectionString)
            conn.Open()
            Dim dummyUserDataQuery As String =
                "INSERT INTO Users (Firstname, Lastname, Password, Email, userGroup) 
				VALUES(""Admin"",""Man"",""123"",""asd"",""0"")," + Environment.NewLine

            Dim dummyPatientDataQuery As String =
                "INSERT INTO Patients (Firstname, Lastname, Identity, Email, Weight, Height, BloodType, Allergies)
				VALUES"

            Dim dummyBillingItemDataQuery As String =
                "INSERT INTO BillingItems (Name, Type, Price)
				VALUES (""Lab Services"", 2, 100),
                       (""X-Ray"", 2, 50),
                       (""Comprehensive Health Check"", 1, 120),
                       (""Partial Health Check"", 1, 75)," + Environment.NewLine

            Dim dummyTransactionDataQuery As String =
                "INSERT INTO Transactions (ItemId, PatientId, Quantity, Completed)
                VALUES "

            Dim names As New List(Of String)
            names.AddRange({"Queece Boringman",
                            "Gremlin T.Squarfish",
                            "Mynas Opposite",
                            "Jannider Snoutlick",
                            "Dirt Stick",
                            "Buckrain Politosh",
                            "Fresident Freepaw-Flitzen",
                            "Azimuth Starcrack-II",
                            "Greek Baby",
                            "Danny Sassoon",
                            "Vodorok Pagina",
                            "Biquinn Thunderhands-Elfson",
                            "Dunkrod Gluck",
                            "Brockwyn 1000-Babies",
                            "Exquizardus Sunset-Mitchellham",
                            "Ping Pong-Leg",
                            "Tootee Frootee",
                            "Logan Hymen-Valencia",
                            "Ronce Lafontaine",
                            "Jeneric Tounguetaste",
                            "Frizzy Totay",
                            "Goblin! -",
                            "Rustrap D'Pencil",
                            "Nacho Thigh-Juice",
                            "Lemmicus D'Seuss",
                            "Rorschach Tok-Tok",
                            "Brick-Spunck Badgerballs",
                            "Laporte Dipthong",
                            "Boner P'TiffyJr.",
                            "Horris Sophistofable",
                            "BarkBark HooHaw",
                            "Lucifan Y.Sassafras",
                            "Simon Asterisk",
                            "Carlton TheSeeker",
                            "Shiprecc -",
                            "???? -",
                            "Pete Crapletters",
                            "7th-Hope Skelligan",
                            "Bouncy House",
                            "Snip-Snip Testafuente",
                            "Herpsichord -",
                            "Tri-pecker Floam",
                            "Party Machine",
                            "Oracle Vidunatru",
                            "Nogbone Danky",
                            "Lavalamp Sequeltank",
                            "SirAmblin Flecator",
                            "Hockbew Egress",
                            "Gumbo LeTroutstain",
                            "I1D1G53 -",
                            "Busted-Lip Catharsises",
                            "Steelgrippe D'Forte",
                            "Naru Sheni"})

            Dim items As New List(Of String)
            items.AddRange({"Emerald Potions",
                            "Mandrake Protection",
                            "Moonseed Potion",
                            "Swelling Gas",
                            "Madame Glossy's Silver Polish",
                            "Bundimum",
                            "Potion No. 86",
                            "Manication",
                            "Antidote to Unctuous Uncommon Potion",
                            "Forgetfulness Potion"})

            Dim UserTableData
            Dim PatientTableData
            For Each name As String In names
                Randomize()
                Dim y = name.Split(" ")

                UserTableData = String.Format("(""{0}"", ""{1}"", ""{2}"", ""{3}"", ""{4}"")" + If(name = names.Last(), ";", ",") + Environment.NewLine,
                                              y(0), y(1), CInt((99999 * Rnd()) + 10000), y(0) + "@mail.com", Math.Floor(3 * Rnd()))

                Randomize()
                PatientTableData = String.Format("(""{0}"", ""{1}"", ""{2}"", ""{3}"", ""{4}"", ""{5}"",""{6}"",""{7}"")" + If(name = names.Last(), ";", ",") + Environment.NewLine,
                                                 y(0), y(1), CInt((9999 * Rnd()) + 1000), y(0) + "@mail.com", CInt((200 * Rnd()) + 30),
                                                 CInt((80 * Rnd()) + 130), Math.Floor(8 * Rnd()), y(1))

                dummyUserDataQuery += UserTableData
                dummyPatientDataQuery += PatientTableData
            Next

            Dim BillingItemTableData
            For Each item As String In items
                Randomize()
                BillingItemTableData = String.Format("(""{0}"", ""{1}"", ""{2}"")" + If(item = items.Last(), ";", ",") + Environment.NewLine, item, "0", CInt((200 * Rnd()) + 30))

                dummyBillingItemDataQuery += BillingItemTableData
            Next

            Dim TransactionTableData
            For x = 0 To 50
                Randomize()
                TransactionTableData = String.Format("(""{0}"",""{1}"",""{2}"",""{3}"")" + If(x = 50, ";", ",") + Environment.NewLine,
                                                     Math.Floor(CInt(14 * Rnd())), Math.Floor(CInt(53 * Rnd())), CInt(10 * Rnd()), Math.Floor(CInt(1 * Rnd())))

                dummyTransactionDataQuery += TransactionTableData
            Next

            Dim dummyUserDataCmd As New SQLiteCommand(dummyUserDataQuery, conn)
            Dim dummyPatientDataCmd As New SQLiteCommand(dummyPatientDataQuery, conn)
            Dim dummyBillingItemDataCmd As New SQLiteCommand(dummyBillingItemDataQuery, conn)
            Dim dummyTransactionDataCmd As New SQLiteCommand(dummyTransactionDataQuery, conn)

            dummyUserDataCmd.ExecuteNonQuery()
            dummyPatientDataCmd.ExecuteNonQuery()
            dummyBillingItemDataCmd.ExecuteNonQuery()
            dummyTransactionDataCmd.ExecuteNonQuery()

            conn.Close()
        End Using
    End Sub
    Private Function DuplicateDatabase(fullpath As String) As Boolean
        Return System.IO.File.Exists(fullpath)
    End Function
    ''' <summary>
    ''' Attempts to login with given email and password combination.
    ''' </summary>
    ''' <param name="email">email of user</param>
    ''' <param name="password">password of user</param>
    ''' <returns>Returns -1 if not found, 0 if Admin, 1 if Doctor, 2 if StaffNurse</returns>
    Public Function Login(email As String, password As String) As Integer
        Using conn As New SQLiteConnection(connectionString)
            Dim loginQuery As String =
                "SELECT * FROM Users WHERE Email = @Email AND Password = @Password LIMIT 1"
            Dim cmd As New SQLiteCommand(loginQuery, conn)
            cmd.Parameters.AddWithValue("@Email", email)
            cmd.Parameters.AddWithValue("@Password", password)
            conn.Open()
            Dim reader As SQLiteDataReader = cmd.ExecuteReader()
            Dim i As Integer = -1
            While reader.Read()
                Select Case reader("UserGroup")
                    Case User.UserGroupEnum.Admin
                        i = 0
                    Case User.UserGroupEnum.Doctor
                        i = 1
                    Case User.UserGroupEnum.StaffNurse
                        i = 2
                    Case Else
                End Select
            End While
            conn.Close()
            Return i
        End Using
    End Function
End Class
Public Class AdminDB
    'https://www.youtube.com/watch?v=BEBtSZeXGsA CRUD with SQLite
    ''' <summary>
    ''' Adds a new user to the database
    ''' </summary>
    ''' <param name="user">The user to be inserted into the database.</param>
    ''' <returns>Returns the number of rows affected.</returns>
    Public Function InsertNewUser(user As User) As Integer
        Using conn As New SQLiteConnection(Database.connectionString)
            Dim insertNewUserQuery As String =
                        "INSERT INTO Users(Firstname,Lastname,Password,Email,userGroup) 
						VALUES(@Firstname,@Lastname,@Password,@Email,@userGroup)"
            Dim cmd As New SQLiteCommand(insertNewUserQuery, conn)
            cmd.Parameters.AddWithValue("@Firstname", user.Firstname)
            cmd.Parameters.AddWithValue("@Lastname", user.Lastname)
            cmd.Parameters.AddWithValue("@Password", user.Password)
            cmd.Parameters.AddWithValue("@Email", user.Email)
            cmd.Parameters.AddWithValue("@userGroup", CInt(user.UserGroup))
            conn.Open()
            Dim i As Integer = cmd.ExecuteNonQuery()
            conn.Close()
            Return i
        End Using
    End Function
    'https://stackoverflow.com/questions/16029441/how-to-delete-multiple-rows-in-sql-where-id-x-to-y/16029501 Delete multiple rows using SQL
    ''' <summary>
    ''' Remove selected users.
    ''' </summary>
    ''' <param name="_users">List of users to delete</param>
    ''' <returns>Returns number of effected rows</returns>
    ''' https://stackoverflow.com/questions/337704/parameterize-an-sql-in-clause
    Public Function RemoveUsers(_users As List(Of User)) As Integer
        Using conn As New SQLiteConnection(Database.connectionString)
            conn.Open()
            Dim removeUserQuery As String = "DELETE FROM Users WHERE userId IN ("
            For Each user As User In _users
                If user.UserID = _users.Last.UserID Then
                    removeUserQuery += user.UserID.ToString + ");"
                Else
                    removeUserQuery += user.UserID.ToString + ","
                End If
            Next
            Dim cmd As New SQLiteCommand(removeUserQuery, conn)
            Dim i = cmd.ExecuteNonQuery()
            conn.Close()
            Return i
        End Using
    End Function
    ''' <summary>
    ''' Updates user information
    ''' </summary>
    ''' <param name="user">User to update</param>
    ''' <returns>Returns number of rows affected</returns>
    Public Function UpdateUser(user As User) As Integer
        Using conn As New SQLiteConnection(Database.connectionString)
            Dim updateUserQuery As String = "UPDATE Users SET Firstname = @Firstname, Lastname = @Lastname, Password = @Password, Email = @Email, userGroup = @userGroup WHERE userId = @userId"
            Dim cmd As New SQLiteCommand(updateUserQuery, conn)
            cmd.Parameters.AddWithValue("@Firstname", user.Firstname)
            cmd.Parameters.AddWithValue("@Lastname", user.Lastname)
            cmd.Parameters.AddWithValue("@Password", user.Password)
            cmd.Parameters.AddWithValue("@Email", user.Email)
            cmd.Parameters.AddWithValue("@userGroup", CInt(user.UserGroup))
            cmd.Parameters.AddWithValue("@userId", user.UserID)
            conn.Open()
            Dim i As Integer = cmd.ExecuteNonQuery()
            conn.Close()
            Return i
        End Using
    End Function
    ''' <summary>
    ''' Get a single user entry based associated with given userId
    ''' </summary>
    ''' <param name="userId">userId to find</param>
    ''' <returns>A user associated with the given userId</returns>
    Public Function GetUserById(userId As Integer) As User
        Using SqlConn As New SQLiteConnection(Database.connectionString)
            Dim getUserQuery As String = "SELECT * FROM Users WHERE userId = @userId"
            Dim SqlCmd As New SQLiteCommand(getUserQuery, SqlConn)
            SqlCmd.Parameters.AddWithValue("@userId", userId)
            SqlConn.Open()
            Dim reader As SQLiteDataReader = SqlCmd.ExecuteReader()
            Dim selectedUser As User = Nothing
            While reader.Read()
                selectedUser = New User(CInt(reader("userId")), reader("Firstname"), reader("Lastname"), reader("Password"), reader("Email"), CType(CInt(reader("userGroup")), User.UserGroupEnum))
            End While
            Return selectedUser
        End Using
    End Function

    Public Function GetUserByEmail(email As String) As User
        Using SqlConn As New SQLiteConnection(Database.connectionString)
            Dim getUserQuery As String = "SELECT * FROM Users WHERE Email = @email"
            Dim SqlCmd As New SQLiteCommand(getUserQuery, SqlConn)
            SqlCmd.Parameters.AddWithValue("@email", email)
            SqlConn.Open()

            Dim reader As SQLiteDataReader = SqlCmd.ExecuteReader()
            Dim selectedUser As User = Nothing
            While reader.Read()
                selectedUser = New User(CInt(reader("userId")), reader("Firstname"), reader("Lastname"), reader("Password"), reader("Email"), CType(CInt(reader("userGroup")), User.UserGroupEnum))
            End While
            Return selectedUser
        End Using
    End Function
    ''' <summary>
    ''' Gets all users in the Users table of the database
    ''' </summary>
    ''' <returns>Returns a List(Of User)</returns>
    Public Function GetAllUsers() As List(Of User)
        Dim users As New List(Of User)
        Dim getAllUsersQuery As String = "SELECT * FROM Users"
        Using SqlConn As New SQLiteConnection(Database.connectionString)
            Dim SqlCmd As New SQLiteCommand(getAllUsersQuery, SqlConn)
            SqlConn.Open()
            Dim reader As SQLiteDataReader = SqlCmd.ExecuteReader()
            While reader.Read()
                users.Add(New User(CInt(reader("userId")), reader("Firstname"), reader("Lastname"), reader("Password"), reader("Email"), CType(CInt(reader("userGroup")), User.UserGroupEnum)))
            End While
            Return users
        End Using
    End Function
End Class

Public Class ReceptionistDB
    ''' <summary>
    ''' Adds a new patient to the database
    ''' </summary>
    ''' <param name="patient">The user to be inserted into the database.</param>
    ''' <returns>Returns the number of rows affected.</returns>
    Public Function InsertNewPatient(patient As Patient) As Integer
        Using conn As New SQLiteConnection(Database.connectionString)
            Dim insertNewUserQuery As String =
                        "INSERT INTO Patients(Firstname,Lastname,Identity,Mobile,Address,Email,Weight,Height,BloodType,Allergies) 
						VALUES(@Firstname,@Lastname,@Identity,@Mobile,@Address,@Email,@Weight,@Height,@BloodType,@Allergies)"
            Dim cmd As New SQLiteCommand(insertNewUserQuery, conn)
            cmd.Parameters.AddWithValue("@Firstname", patient.Firstname)
            cmd.Parameters.AddWithValue("@Lastname", patient.Lastname)
            cmd.Parameters.AddWithValue("@Identity", patient.Identity)
            cmd.Parameters.AddWithValue("@Mobile", patient.Mobile)
            cmd.Parameters.AddWithValue("@Address", patient.Address)
            cmd.Parameters.AddWithValue("@Email", patient.Email)
            cmd.Parameters.AddWithValue("@Weight", patient.Weight)
            cmd.Parameters.AddWithValue("@Height", patient.Height)
            cmd.Parameters.AddWithValue("@BloodType", CInt(patient.BloodType))
            cmd.Parameters.AddWithValue("@Allergies", patient.Allergies)
            conn.Open()
            Dim i As Integer = cmd.ExecuteNonQuery()
            conn.Close()
            Return i
        End Using
    End Function
    ''' https://stackoverflow.com/questions/16029441/how-to-delete-multiple-rows-in-sql-where-id-x-to-y/16029501 Delete multiple rows using SQL
    ''' <summary>
    ''' Remove selected patients.
    ''' </summary>
    ''' <param name="patients">List of patients to delete</param>
    ''' <returns>Returns number of effected rows</returns>
    Public Function RemovePatients(patients As List(Of Patient)) As Integer
        Using conn As New SQLiteConnection(Database.connectionString)
            conn.Open()
            Dim removePatientQuery As String = "DELETE FROM Patients WHERE PatientId IN ("
            For Each patient As Patient In patients
                If patient.PatientId = patients.Last.PatientId Then
                    removePatientQuery += patient.PatientId.ToString + ");"
                Else
                    removePatientQuery += patient.PatientId.ToString + ","
                End If
            Next
            Dim cmd As New SQLiteCommand(removePatientQuery, conn)
            Dim i = cmd.ExecuteNonQuery()
            conn.Close()
            Return i
        End Using
    End Function
    ''' <summary>
    ''' Updates patient information
    ''' </summary>
    ''' <param name="patient">patient to update</param>
    ''' <returns>Returns number of rows affected</returns>
    Public Function UpdatePatient(patient As Patient) As Integer
        Using conn As New SQLiteConnection(Database.connectionString)
            Dim updateUserQuery As String = "UPDATE Patients SET Firstname = @Firstname, Lastname = @Lastname, Identity = @Identity,
												Mobile = @Mobile, Address = @Address, Email = @Email, Weight = @Weight, Height = @Height,
												BloodType = @BloodType, Allergies = @Allergies WHERE PatientId = @PatientId"
            Dim cmd As New SQLiteCommand(updateUserQuery, conn)
            cmd.Parameters.AddWithValue("@Firstname", patient.Firstname)
            cmd.Parameters.AddWithValue("@Lastname", patient.Lastname)
            cmd.Parameters.AddWithValue("@Identity", patient.Identity)
            cmd.Parameters.AddWithValue("@Mobile", patient.Mobile)
            cmd.Parameters.AddWithValue("@Address", patient.Address)
            cmd.Parameters.AddWithValue("@Email", patient.Email)
            cmd.Parameters.AddWithValue("@Weight", patient.Weight)
            cmd.Parameters.AddWithValue("@Height", patient.Height)
            cmd.Parameters.AddWithValue("@BloodType", CInt(patient.BloodType))
            cmd.Parameters.AddWithValue("@Allergies", patient.Allergies)
            cmd.Parameters.AddWithValue("@PatientId", patient.PatientId)
            conn.Open()
            Dim i As Integer = cmd.ExecuteNonQuery()
            conn.Close()
            Return i
        End Using
    End Function
    ''' <summary>
    ''' Get a single patient entry based associated with given userId
    ''' </summary>
    ''' <param name="patientId">patientId to find</param>
    ''' <returns>A patient associated with the given userId</returns>
    Public Function GetPatientById(patientId As Integer) As Patient
        Using SqlConn As New SQLiteConnection(Database.connectionString)
            Dim getPatientQuery As String = "SELECT * FROM Patients WHERE PatientId = @PatientId"
            Dim SqlCmd As New SQLiteCommand(getPatientQuery, SqlConn)
            SqlCmd.Parameters.AddWithValue("@PatientId", patientId)
            SqlConn.Open()
            Dim reader As SQLiteDataReader = SqlCmd.ExecuteReader()
            Dim selectedPatient As Patient = Nothing
            While reader.Read()
                selectedPatient = New Patient(CInt(reader("PatientId")), reader("Firstname").ToString, reader("Lastname").ToString, reader("Identity").ToString,
                                             reader("Mobile").ToString, reader("Address").ToString, reader("Email").ToString, CInt(reader("Weight")), CInt(reader("Height")),
                                             CType(CInt(reader("BloodType")), Patient.BloodTypeEnum), reader("Allergies").ToString)
            End While
            Return selectedPatient
        End Using
    End Function

    Public Function GetPatientByIdentity(identity As String) As Patient
        Using SqlConn As New SQLiteConnection(Database.connectionString)
            Dim getPatientQuery As String = "SELECT * FROM Patients WHERE Identity = @Identity"
            Dim SqlCmd As New SQLiteCommand(getPatientQuery, SqlConn)
            SqlCmd.Parameters.AddWithValue("@Identity", identity)
            SqlConn.Open()
            Dim reader As SQLiteDataReader = SqlCmd.ExecuteReader()
            Dim selectedPatient As Patient = Nothing
            While reader.Read()
                selectedPatient = New Patient(CInt(reader("PatientId")), reader("Firstname").ToString, reader("Lastname").ToString, reader("Identity").ToString,
                                             reader("Mobile").ToString, reader("Address").ToString, reader("Email").ToString, CInt(reader("Weight")), CInt(reader("Height")),
                                             CType(CInt(reader("BloodType")), Patient.BloodTypeEnum), reader("Allergies").ToString)
            End While
            Return selectedPatient
        End Using
    End Function
    ''' <summary>
    ''' Gets all patients in the Patients table of the database
    ''' </summary>
    ''' <returns>Returns a List(Of Patient)</returns>
    Public Function GetAllPatients() As List(Of Patient)
        Dim patients As New List(Of Patient)
        Dim getAllPatientsQuery As String = "SELECT * FROM Patients"
        Using SqlConn As New SQLiteConnection(Database.connectionString)
            Dim SqlCmd As New SQLiteCommand(getAllPatientsQuery, SqlConn)
            SqlConn.Open()
            Dim reader As SQLiteDataReader = SqlCmd.ExecuteReader()
            While reader.Read()
                patients.Add(New Patient(CInt(reader("PatientId")), reader("Firstname").ToString, reader("Lastname").ToString, reader("Identity").ToString,
                                             reader("Mobile").ToString, reader("Address").ToString, reader("Email").ToString, CInt(reader("Weight")), CInt(reader("Height")),
                                             CType(CInt(reader("BloodType")), Patient.BloodTypeEnum), reader("Allergies").ToString))
            End While
            Return patients
        End Using
    End Function

    Public Function GetBillingItems(itemId As Integer) As List(Of BillingItem)
        Dim billingItems As New List(Of BillingItem)
        Dim getBillingItemsQuery As String = "SELECT * FROM BillingItems WHERE ItemId = @itemId"
        Using conn As New SQLiteConnection(Database.connectionString)
            Dim cmd As New SQLiteCommand(getBillingItemsQuery, conn)
            cmd.Parameters.AddWithValue("@itemId", itemId)
            conn.Open()
            Dim reader As SQLiteDataReader = cmd.ExecuteReader()
            While reader.Read()
                billingItems.Add(New BillingItem(CInt(reader("ItemId")), reader("Name"), reader("Type"), New Currency(CInt(reader("Price")))))
            End While
            Return billingItems
        End Using
    End Function
    'COMPLETE THIS
    Public Function GetPatientTransactions(patientId As Integer) As List(Of BillingItem)
        Dim billingItems As New List(Of BillingItem)
        Dim getBillingItemsQuery As String = "SELECT * FROM BillingItems WHERE ItemId = @itemId"
        Using conn As New SQLiteConnection(Database.connectionString)
            Dim cmd As New SQLiteCommand(getBillingItemsQuery, conn)
            conn.Open()
            Dim reader As SQLiteDataReader = cmd.ExecuteReader()
            While reader.Read()
                billingItems.Add(New BillingItem(CInt(reader("ItemId")), reader("Name"), reader("Type"), reader("Price")))
            End While
            Return billingItems
        End Using
    End Function
End Class
