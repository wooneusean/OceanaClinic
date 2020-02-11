﻿'https://www.youtube.com/watch?v=UN7nMNVw2hk SQLite with Visual Studio
Imports System.Data.SQLite

Public Class Database
    Private Shared location As String = "data/"
    Private Shared filename As String = "main.db"
    Private Shared fullpath As String = System.IO.Path.Combine(location, filename)
    Private Shared connectionString As String = String.Format("Data Source = {0}", fullpath)
    Public Sub Init()
        If Not DuplicateDatabase(fullpath) Then
            Dim createTable As String =
                "CREATE TABLE ""Users"" (
	                ""userId""	    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	                ""Firstname""	TEXT,
	                ""Lastname""	TEXT,
	                ""Password""	TEXT,
	                ""Email""	    TEXT UNIQUE,
	                ""userGroup""	INTEGER
                );"
            System.IO.Directory.CreateDirectory(location)
            Using SqlConn As New SQLiteConnection(connectionString)
                Dim SqlCmd As New SQLiteCommand(createTable, SqlConn)
                SqlConn.Open()
                SqlCmd.ExecuteNonQuery()
                SqlConn.Close()
            End Using
            DummyData()
        End If
    End Sub
    Private Sub DummyData()
        Using conn As New SQLiteConnection(connectionString)
            conn.Open()
            Dim dummyDataQuery As String =
                "INSERT INTO Users (Firstname, Lastname, Password, Email, userGroup) VALUES(""Admin"",""Man"",""123"",""asd"",""0"")," + Environment.NewLine

            Dim x As New List(Of String)
            x.AddRange({"Queece Boringman",
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
                        "Steelgrippe D'Forte"})

            For Each name As String In x
                Dim y = name.Split(" ")
                If name = x.Last() Then
                    Dim z = String.Format("(""{0}"", ""{1}"", ""{2}"", ""{3}"", ""{4}"");", y(0), y(1), CInt((99999 * Rnd()) + 10000), y(0) + "@mail.com", Math.Floor(3 * Rnd()))
                    dummyDataQuery += z
                Else
                    Dim z = String.Format("(""{0}"", ""{1}"", ""{2}"", ""{3}"", ""{4}"")," + Environment.NewLine, y(0), y(1), CInt((99999 * Rnd()) + 10000), y(0) + "@mail.com", Math.Floor(3 * Rnd()))
                    dummyDataQuery += z
                End If
            Next

            Dim cmd As New SQLiteCommand(dummyDataQuery, conn)
            cmd.ExecuteNonQuery()

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
    Public Class Admin
        'https://www.youtube.com/watch?v=BEBtSZeXGsA CRUD with SQLite
        ''' <summary>
        ''' Adds a new user to the database
        ''' </summary>
        ''' <param name="user">The user to be inserted into the database.</param>
        ''' <returns>Returns the number of rows affected.</returns>
        Public Function InsertNewUser(user As User) As Integer
            Using conn As New SQLiteConnection(connectionString)
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
            Using conn As New SQLiteConnection(connectionString)
                conn.Open()
                Dim i As Integer
                For Each user As User In _users
                    Dim removeUserQuery As String = "DELETE FROM Users WHERE userId = @userId"
                    Dim cmd As New SQLiteCommand(removeUserQuery, conn)
                    cmd.Parameters.AddWithValue("@userId", user.UserID)
                    i += cmd.ExecuteNonQuery()
                Next
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
            Using conn As New SQLiteConnection(connectionString)
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
            Using SqlConn As New SQLiteConnection(connectionString)
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
            Using SqlConn As New SQLiteConnection(connectionString)
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
            Using SqlConn As New SQLiteConnection(connectionString)
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
End Class
