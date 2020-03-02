Imports System.Collections.ObjectModel
Imports System.ComponentModel
Public Class User
    Enum UserGroupEnum
        Admin
        Doctor
        StaffNurse
    End Enum
    Public Property UserID() As Integer
    Public Property Firstname() As String
    Public Property Lastname() As String
    Public Property Password() As String
    Public Property Email() As String
    Public Property UserGroup() As UserGroupEnum
    Public Sub New(ByVal __userId As Integer, ByVal __Firstname As String, ByVal __Lastname As String, ByVal __Password As String, ByVal __Email As String, ByVal __userGroup As UserGroupEnum)
        Firstname = __Firstname.Trim()
        Lastname = __Lastname.Trim()
        Password = __Password.Trim()
        Email = __Email.Trim()
        UserID = __userId
        UserGroup = __userGroup
    End Sub
    Public Sub New(ByVal _userObject As Object)
        Dim x As User = _userObject
        Firstname = x.Firstname.Trim()
        Lastname = x.Lastname.Trim()
        Password = x.Password.Trim()
        Email = x.Email.Trim()
        UserID = x.UserID
        UserGroup = x.UserGroup
    End Sub
    Public Sub New()
        UserID = 0
        Firstname = ""
        Lastname = ""
        Password = ""
        Email = ""
        UserGroup = UserGroupEnum.Doctor
    End Sub
End Class
Public Class ObservableUsers
    Inherits ObservableCollection(Of User)
End Class