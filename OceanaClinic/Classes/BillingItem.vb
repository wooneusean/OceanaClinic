Imports System.Collections.ObjectModel

Public Class BillingItem
    Inherits ObservableObject
    Enum ItemTypeEnum
        Medication
        Consultation
        Service
    End Enum
    Private _itemId As Integer
    Public Property ItemId() As Integer
        Get
            Return _itemId
        End Get
        Set(value As Integer)
            _itemId = value
            OnPropertyChanged(NameOf(ItemId))
        End Set
    End Property
    Private _name As String
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(value As String)
            _name = value
            OnPropertyChanged(NameOf(Name))
        End Set
    End Property
    Private _type As ItemTypeEnum
    Public Property Type() As ItemTypeEnum
        Get
            Return _type
        End Get
        Set(value As ItemTypeEnum)
            _type = value
            OnPropertyChanged(NameOf(Type))
        End Set
    End Property
    Private _price As Currency
    Public Property Price() As Currency
        Get
            Return _price
        End Get
        Set(value As Currency)
            _price = value
            OnPropertyChanged(NameOf(Price))
        End Set
    End Property
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