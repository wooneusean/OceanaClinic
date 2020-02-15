'https://www.youtube.com/watch?v=LEKngPq342s - C# WPF Tutorial - Using INotifyPropertyChanged
Imports System.ComponentModel

Public Class ObservableObject
    Implements INotifyPropertyChanged
    Public Event PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

    Protected Sub OnPropertyChanged(ByVal e As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(e))
    End Sub

End Class

