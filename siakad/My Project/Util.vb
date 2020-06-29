
Imports System.Text.RegularExpressions

Public Class Util
    Public Function IsValidEmailFormat(ByVal s As String) As Boolean
        Return Regex.IsMatch(s, "^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$")
    End Function

    Public Function isValidIdFormat(ByVal s As String) As Boolean
        Return Regex.IsMatch(s, "[0-9]")
    End Function
End Class
