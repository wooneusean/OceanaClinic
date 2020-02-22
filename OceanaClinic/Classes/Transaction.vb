Imports System.Collections.ObjectModel

Public Class Transaction
    Inherits ObservableObject
    Implements IComparable(Of Transaction)
    Private _transactionId As Integer
    Public Property TransactionId() As Integer
        Get
            Return _transactionId
        End Get
        Set(value As Integer)
            _transactionId = value
            OnPropertyChanged(NameOf(TransactionId))
        End Set
    End Property
    Private _ItemId As Integer
    Public Property ItemId() As Integer
        Get
            Return _ItemId
        End Get
        Set(value As Integer)
            _ItemId = value
            OnPropertyChanged(NameOf(ItemId))
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
    Public Property Price() As Currency 'Price from BillingItems table
        Get
            Return _price
        End Get
        Set(value As Currency)
            If _price Is Nothing Then
                _price = New Currency(value.Value)
            Else
                _price = value
            End If
            OnPropertyChanged(NameOf(Price))
            OnPropertyChanged(NameOf(SubTotal))
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
            OnPropertyChanged(NameOf(SubTotal))
        End Set
    End Property
    Private _itemType As BillingItem.ItemTypeEnum
    Public Property ItemType() As BillingItem.ItemTypeEnum
        Get
            Return _itemType
        End Get
        Set(value As BillingItem.ItemTypeEnum)
            _itemType = value
            OnPropertyChanged(NameOf(ItemType))
        End Set
    End Property
    Private _subTotal As Currency
    Public ReadOnly Property SubTotal() As Currency
        Get
            If _subTotal Is Nothing Then
                _subTotal = New Currency(Price.Value * Quantity)
            Else
                _subTotal.Value = Price.Value * Quantity
            End If
            Return _subTotal
        End Get
    End Property
    Sub New(_trId As Integer, _itId As Integer, _itemName As String, _quantity As Integer, _itemPrice As Decimal, _itTy As Integer)
        TransactionId = _trId
        ItemId = _itId
        Quantity = _quantity
        Name = _itemName
        Price = New Currency(_itemPrice)
        ItemType = CType(_itTy, BillingItem.ItemTypeEnum)
    End Sub
    Sub New()
        TransactionId = -1
        ItemId = -1
        Quantity = 1
        Name = ""
        Price = New Currency(0)
        ItemType = CType(-1, BillingItem.ItemTypeEnum)
    End Sub

    Public Function CompareTo(other As Transaction) As Integer Implements IComparable(Of Transaction).CompareTo
        Return Price.CompareTo(other.Price)
    End Function
End Class
Public Class ObservableTransactions
    Inherits ObservableCollection(Of Transaction)
End Class