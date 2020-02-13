Imports System.Collections.ObjectModel
Imports System.ComponentModel
Public Class User
    Inherits ObservableObject
    Enum UserGroupEnum
        Admin
        Doctor
        StaffNurse
    End Enum
    Private _userId As Integer
    Private _Firstname As String
    Private _Lastname As String
    Private _Password As String
    Private _Email As String
    Private _userGroup As UserGroupEnum
    Public Sub New(ByVal __userId As Integer, ByVal __Firstname As String, ByVal __Lastname As String, ByVal __Password As String, ByVal __Email As String, ByVal __userGroup As UserGroupEnum)
        _Firstname = __Firstname.Trim()
        _Lastname = __Lastname.Trim()
        _Password = __Password.Trim()
        _Email = __Email.Trim()
        _userId = __userId
        _userGroup = __userGroup
    End Sub
    Public Sub New(ByVal _userObject As Object)
        Dim x As User = _userObject
        _Firstname = x.Firstname.Trim()
        _Lastname = x.Lastname.Trim()
        _Password = x.Password.Trim()
        _Email = x.Email.Trim()
        _userId = x.UserID
        _userGroup = x.UserGroup
    End Sub
    Public Sub New()
        _userId = 0
        _Firstname = ""
        _Lastname = ""
        _Password = ""
        _Email = ""
        _userGroup = UserGroupEnum.Doctor
    End Sub
    Public Property UserID() As Integer
        Get
            Return _userId
        End Get
        Set(ByVal value As Integer)
            _userId = value
            OnPropertyChanged(NameOf(UserID))
        End Set
    End Property
    Public Property Firstname() As String
        Get
            Return _Firstname
        End Get
        Set(ByVal value As String)
            _Firstname = value
            OnPropertyChanged(NameOf(Firstname))
        End Set
    End Property
    Public Property Lastname() As String
        Get
            Return _Lastname
        End Get
        Set(ByVal value As String)
            _Lastname = value
            OnPropertyChanged(NameOf(Lastname))
        End Set
    End Property
    Public Property Password() As String
        Get
            Return _Password
        End Get
        Set(ByVal value As String)
            _Password = value
            OnPropertyChanged(NameOf(Password))
        End Set
    End Property
    Public Property Email() As String
        Get
            Return _Email
        End Get
        Set(ByVal value As String)
            _Email = value
            OnPropertyChanged(NameOf(Email))
        End Set
    End Property
    Public Property UserGroup() As UserGroupEnum
        Get
            Return _userGroup
        End Get
        Set(ByVal value As UserGroupEnum)
            _userGroup = value
            OnPropertyChanged(NameOf(UserGroup))
        End Set
    End Property
End Class
Public Class ObservableUsers
    Inherits ObservableCollection(Of User)
End Class