Imports System.Collections.ObjectModel

Public Class Transaction
    Implements IComparable
    Public Property TransactionId() As Integer
    Public Property ItemId() As Integer
    Public Property Name() As String 'Name from BillingItems table
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
        End Set
    End Property
    Public Property Quantity() As Integer
    Public Property ItemType() As BillingItem.ItemTypeEnum
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

    Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
        Dim other As Transaction = obj
        Return Price.Value.CompareTo(other.Price.Value)
    End Function
End Class
Public Class ObservableTransactions
    Inherits ObservableCollection(Of Transaction)
End Class