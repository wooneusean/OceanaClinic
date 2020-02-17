Public Class BillingItem
    Inherits ObservableObject
    Enum ItemTypeEnum
        Consultation
        Medication
        Service
    End Enum

    Public Property ItemId() As Integer
    Public Property Name() As String
    Public Property Type() As ItemTypeEnum
    Public Property Price() As Currency
End Class
