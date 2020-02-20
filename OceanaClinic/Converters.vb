Imports System.Globalization
Class PasswordTextConverter
    Implements IValueConverter
    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        Dim passwordString As String = value.ToString()
        Dim returnString As String = ""
        For i = 1 To passwordString.Length
            returnString += "*"
        Next
        Return returnString
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function
End Class
Class UserDetailsConverter
    Implements IMultiValueConverter
    Public Function Convert(values() As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IMultiValueConverter.Convert
        Return values.Clone()
    End Function

    Public Function ConvertBack(value As Object, targetTypes() As Type, parameter As Object, culture As CultureInfo) As Object() Implements IMultiValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function
End Class
Class UtilityConverter
    Public Shared Function SelectedItemsToListOfUsers(selectedItems As IList) As List(Of User)
        Dim users As New List(Of User)
        For Each user As User In selectedItems
            users.Add(New User(user.UserID, user.Firstname, user.Lastname, user.Password, user.Email, user.UserGroup))
        Next
        Return users
    End Function
    Public Shared Function SelectedItemsToListOfTransactions(selectedItems As IList) As List(Of Transaction)
        Dim transactions As New List(Of Transaction)
        For Each transaction As Transaction In selectedItems
            transactions.Add(New Transaction(transaction.Id, transaction.Quantity, transaction.Name, transaction.PricePerUnit.Value))
        Next
        Return transactions
    End Function
End Class
Class StringToDecimalConverter
    Implements IValueConverter
    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        Return value.ToString()
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        If TypeOf (value) Is String Then
            Dim doubleVal As Decimal = 0
            If Decimal.TryParse(value, doubleVal) Then
                Return doubleVal
            Else
                Return 0
            End If
        Else
            Return 0
        End If
    End Function
End Class