Imports System.Collections.ObjectModel

Public Class Transaction
    Inherits ObservableObject
    'Private _transactionId As Integer
    'Public Property TransactionId() As Integer
    '    Get
    '        Return _transactionId
    '    End Get
    '    Set(value As Integer)
    '        _transactionId = value
    '        OnPropertyChanged(NameOf(TransactionId))
    '    End Set
    'End Property
    Private _id As Integer
    Public Property Id() As Integer
        Get
            Return _id
        End Get
        Set(value As Integer)
            _id = value
            OnPropertyChanged(NameOf(Id))
        End Set
    End Property
    Private _name As String
    Public Property Name() As String 'Name from BillingItems table
        Get
            Return _name
        End Get
        Set(value As String)
            _name = value
            OnPropertyChanged(NameOf(Name))
        End Set
    End Property
    Private _price As Currency
    Public Property PricePerUnit() As Currency 'Price from BillingItems table
        Get
            Return _price
        End Get
        Set(value As Currency)
            If _price Is Nothing Then
                _price = New Currency(value.Value)
            Else
                _price = value
            End If
            OnPropertyChanged(NameOf(PricePerUnit))
        End Set
    End Property
    Private _quantity As Integer
    Public Property Quantity() As Integer
        Get
            Return _quantity
        End Get
        Set(value As Integer)
            _quantity = value
            OnPropertyChanged(NameOf(Quantity))
        End Set
    End Property
    'Private Property ItemType() As BillingItem.ItemTypeEnum 'Price from BillingItems table
    Sub New(_trId As Integer, _quantity As Integer, _itemName As String, _itemPrice As Decimal)
        'TransactionId = _trId
        Id = _trId
        Quantity = _quantity
        Name = _itemName
        PricePerUnit = New Currency(_itemPrice)
        'ItemType = CType(_itemType, BillingItem.ItemTypeEnum)
    End Sub
End Class
Public Class ObservableTransactions
    Inherits ObservableCollection(Of Transaction)
End Class