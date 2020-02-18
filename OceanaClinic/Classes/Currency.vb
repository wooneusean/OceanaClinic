Public Class Currency
    Public Property Value() As Decimal
    Public Property Prefix() As String

    Public Overrides Function ToString() As String
        Dim x As Decimal = Decimal.Round(Value, 2, MidpointRounding.AwayFromZero)
        Dim split = x.ToString.Split(".")
        Dim wholeNumber As String
        Dim decimalNumber As String
        If split.Count > 1 Then
            wholeNumber = String.Format("{0:#,##0}", CInt(split(0)))
            decimalNumber = split(1)
            If decimalNumber.Length = 1 Then
                decimalNumber += "0"
            ElseIf decimalNumber.Length = 0 Then
                decimalNumber += "00"
            End If
        Else
            wholeNumber = String.Format("{0:#,##0}", CInt(split(0)))
            decimalNumber = "00"
        End If
        Return Prefix + wholeNumber + "." + decimalNumber
    End Function

    Public Shared Operator +(ByVal c1 As Currency, ByVal c2 As Currency) As Currency
        Return New Currency(c1.Value + c2.Value, c1.Prefix)
    End Operator
    Public Shared Operator -(ByVal c1 As Currency, ByVal c2 As Currency) As Currency
        Return New Currency(c1.Value - c2.Value, c1.Prefix)
    End Operator

    Sub New()
        Value = 0
        Prefix = "RM "
    End Sub
    Sub New(val As Double, pre As String)
        Value = val
        Prefix = pre
    End Sub
    Sub New(pre As String)
        Value = 0
        Prefix = pre
    End Sub
    Sub New(val As Double)
        Value = val
        Prefix = "RM "
    End Sub
End Class
