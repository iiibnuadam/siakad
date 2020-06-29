Imports System.Data
Imports MySql.Data
Imports MySql.Data.MySqlClient


Public Class Database
    Dim connStr As String = "server=localhost;user=root;database=siakad;port=3306;password=;"
    Dim conn As New MySqlConnection(connStr)

    Public Function Test() As String
        Dim msg As String = "Unknown Error"
        Try
            conn.Open()
            If (ConnectionState.Open) Then
                msg = "Connect OK"
            End If
            conn.Close()

        Catch ex As Exception
            Console.WriteLine(ex)
            msg = "Connect Error"
        End Try
        Return msg

    End Function

    Public Function getConnection() As MySqlConnection
        Return conn
    End Function

    Public Function RunQuery(ByVal sql As String) As DataTable
        Try
            If (ConnectionState.Open) Then
                conn.Close()
            End If
            conn.Open()
            Dim dt As New DataTable
            Dim cmd As New MySqlCommand(sql, conn)
            Dim adapter As New MySqlDataAdapter(cmd)
            adapter.Fill(dt)
            conn.Close()
            Return dt
        Catch ex As MySqlException
            Console.WriteLine(ex.Message)
        End Try

        Return Nothing
    End Function

    Public Sub RunNonQuery(ByVal sql As String)
        Try
            conn.Open()
            Dim cmd As New MySqlCommand(sql, conn)
            cmd.ExecuteNonQuery()
            conn.Close()
        Catch ex As MySqlException
            Console.WriteLine(ex.Message)
        End Try
    End Sub


End Class
