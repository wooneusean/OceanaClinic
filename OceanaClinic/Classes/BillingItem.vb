Imports System.Collections.ObjectModel

Public Class BillingItem
    Enum ItemTypeEnum
        Medication
        Consultation
        Service
    End Enum

    Public Property ItemId() As Integer
    Public Property Name() As String
    Public Property Type() As ItemTypeEnum
    Public Property Price() As Currency
    Sub New()

    End Sub
    Sub New(_itemid As Integer, _name As String, _type As ItemTypeEnum, _price As Currency)
        ItemId = _itemid
        Name = _name
        Type = _type
        Price = _price
    End Sub
End Class

Public Class ObservableBillingItems
    Inherits ObservableCollection(Of BillingItem)
End Class