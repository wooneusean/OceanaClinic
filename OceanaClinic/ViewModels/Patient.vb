Imports System.Collections.ObjectModel

Public Class Patient
    Enum BloodTypeEnum
        APos
        ANeg
        OPos
        ONeg
        BPos
        BNeg
        ABPos
        ABNeg
    End Enum

    Public Property PatientId() As Integer
    Public Property Firstname() As String
    Public Property Lastname() As String
    Public Property Identity() As String
    'Private _fullName As String
    'Public ReadOnly Property Fullname() As String
    '    Get
    '        Return Firstname + " " + Lastname
    '    End Get
    'End Property
    Public Property Mobile() As String
    Public Property Address() As String
    Public Property Email() As String
    Public Property Weight() As Integer
    Public Property Height() As Integer
    Public Property BloodType() As BloodTypeEnum
    Public Property Allergies() As String

    Public Sub New(_patientId As Integer, _firstname As String, _lastname As String, _identity As String, _mobile As String, _address As String, _email As String, _weight As Integer, _height As Integer, _bloodtype As BloodTypeEnum, _allergies As String)
        PatientId = _patientId
        Firstname = _firstname
        Lastname = _lastname
        Identity = _identity
        Mobile = _mobile
        Address = _address
        Email = _email
        Weight = _weight
        Height = _height
        BloodType = _bloodtype
        Allergies = _allergies
    End Sub
    Public Sub New(ByVal _patientObject As Object)
        Dim x As Patient = _patientObject
        PatientId = x.PatientId
        Firstname = x.Firstname
        Lastname = x.Lastname
        Identity = x.Identity
        Mobile = x.Mobile
        Address = x.Address
        Email = x.Email
        Weight = x.Weight
        Height = x.Height
        BloodType = x.BloodType
        Allergies = x.Allergies
    End Sub
    Public Sub New()
        PatientId = -1
        Firstname = ""
        Lastname = ""
        Identity = ""
        Mobile = ""
        Address = ""
        Email = ""
        Weight = -1
        Height = -1
        BloodType = -1
        Allergies = ""
    End Sub
End Class
Public Class ObservablePatients
    Inherits ObservableCollection(Of Patient)
End Class