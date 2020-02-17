Public Class Currency
    Public Property Value() As Double
    Public Property Prefix() As String

    Public Overrides Function ToString() As String
        Dim x As Double = Decimal.Round(Value, 2, MidpointRounding.AwayFromZero)
        Dim y As String = x.ToString.Split(".")(1)
        Dim z As String = x.ToString.Split(".")(0)
        If y.Length = 1 Then
            y += "0"
        ElseIf y.Length = 0 Then
            y += "00"
        End If
        Return Prefix + z + "." + y
    End Function
    Sub New()
        Value = 0
        Prefix = "RM "
    End Sub
    Sub New(val As Integer, pre As String)
        Value = val
        Prefix = pre
    End Sub
    Sub New(pre As String)
        Value = 0
        Prefix = pre
    End Sub
    Sub New(val As Integer)
        Value = val
        Prefix = "RM "
    End Sub
End Class
