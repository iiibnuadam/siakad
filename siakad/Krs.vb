Imports MySql.Data.MySqlClient
Public Class Krs
    Dim db As New Database
    Dim conn As MySqlConnection = db.getConnection()
    Dim krsSource As DataTable
    Dim dt2 As New DataTable
    Dim nim As String


    Function getData(ByVal sql As String) As DataTable
        Try
            Dim dt As New DataTable
            Dim command As New MySqlCommand(sql, conn)
            Dim adapter As New MySqlDataAdapter(command)
            adapter.Fill(dt)
            Return dt
        Catch e As MySqlException
            Console.WriteLine(e.Message)
        End Try
        Return Nothing
    End Function

    Public Sub SetNIM(ByVal ni As String)
        nim = ni
    End Sub
    Private Sub Krs_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MetroButton1.Enabled = False
        Button3.Enabled = False
        MetroComboBox3.Enabled = False
        dt2.Columns.Add(New DataColumn("kode_makul"))
        dt2.Columns.Add(New DataColumn("nama"))
        dt2.Columns.Add(New DataColumn("sks"))
        dt2.Columns.Add(New DataColumn("nama_dosen"))
        dt2.Columns.Add(New DataColumn("nip"))
        MetroGrid2.DataSource = dt2
    End Sub

    Sub doGetData()
        Dim makulData As DataTable
        Dim sem As String = MetroComboBox2.Text
        Console.WriteLine("Semester" + sem)
        Dim sql As String = "SELECT A.kode_makul, C.nama, C.sks,B.nama_dosen, A.nip FROM `dosen_makul` A INNER JOIN `dosen` B ON A.nip = B.nip INNER JOIN `mata_kuliah` C ON A.kode_makul = C.kode WHERE C.semester =" + sem + ";"
        makulData = getData(sql)
        krsSource = makulData
        MetroGrid1.DataSource = makulData

    End Sub

    Private Sub MetroLabel2_Click(sender As Object, e As EventArgs) Handles MetroLabel2.Click

    End Sub

    Private Sub MetroButton1_Click(sender As Object, e As EventArgs) Handles MetroButton1.Click
        Dim row As Integer = MetroGrid1.CurrentRow.Index
        Dim data As DataRow = krsSource.Rows(row)

        If dt2.Rows.Count > 0 Then
            Dim exists As Boolean = False
            For Each dataRow As DataRow In dt2.Rows
                If dataRow.Field(Of String)("kode_makul") = data.Field(Of String)("kode_makul") Then
                    MessageBox.Show("Data Sudah Ada")
                    exists = True
                    Exit For
                End If
            Next

            If Not exists Then
                InsertKRS(data)
            End If

        Else
            InsertKRS(data)
        End If

        GetUserKRS()

    End Sub

    Private Sub InsertKRS(ByVal row As DataRow)
        Dim kodeMakul As String = row.Field(Of String)("kode_makul")
        Dim tahun = MetroComboBox3.Text
        Dim sem As Integer
        If MetroComboBox1.Text = "Genap" Then
            sem = 0
        Else
            sem = 1
        End If
        Try
            conn.Open()
            Dim sql As String = "INSERT INTO krs VALUES('" + nim + "','" + kodeMakul + "','" + tahun + "'," + sem.ToString + ",'-')"
            Dim comm As New MySqlCommand(sql, conn)
            comm.ExecuteNonQuery()
            conn.Close()
        Catch e As MySqlException
            Console.WriteLine(e.Message)
        End Try




    End Sub



    Private Sub MetroComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles MetroComboBox2.SelectedIndexChanged
        doGetData()
    End Sub

    Sub GetUserKRS()
        dt2.Clear()
        Dim sem As Integer
        If MetroComboBox1.Text = "Genap" Then
            sem = 0
        Else
            sem = 1
        End If
        Dim tahun As String = MetroComboBox3.Text
        Dim sql As String = "Select A.kode_makul, C.nama,C.sks, D.nama_dosen, B.nip FROM `krs` A INNER JOIN dosen_makul B On A.kode_makul = B.kode_makul INNER JOIN mata_kuliah C On A.kode_makul = C.kode INNER JOIN dosen D On B.nip = D.nip WHERE A.nim = '" + nim + "' AND A.semester = " + sem.ToString + " AND A.tahun='" + tahun + "'"
        Dim userKrs As DataTable
        userKrs = getData(sql)
        If userKrs.Rows.Count > 0 Then
            For Each dataRow As DataRow In userKrs.Rows
                dt2.ImportRow(dataRow)
            Next
        End If
        Console.WriteLine(sql)
    End Sub

    Private Sub DoDeleteKRS(ByVal row As DataRow)
        Dim kodeMakul As String = row.Field(Of String)("kode_makul")
        Dim tahun = MetroComboBox3.Text
        Dim sem As Integer
        If MetroComboBox1.Text = "Genap" Then
            sem = 0
        Else
            sem = 1
        End If
        Try
            conn.Open()
            Dim sql As String = "DELETE FROM krs WHERE nim='" + nim + "' AND kode_makul='" + kodeMakul + "' AND tahun='" + tahun + "' AND semester=" + sem.ToString + ";"
            Dim comm As New MySqlCommand(sql, conn)
            comm.ExecuteNonQuery()
            conn.Close()
        Catch ex As MySqlException
            Console.WriteLine(ex.Message)
        End Try
    End Sub
    Private Sub MetroComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles MetroComboBox1.SelectedIndexChanged
        MetroComboBox3.Enabled = True
    End Sub

    Private Sub MetroButton3_Click(sender As Object, e As EventArgs)
        GetUserKRS()
    End Sub



    Private Sub MetroComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles MetroComboBox3.SelectedIndexChanged
        GetUserKRS()
        MetroButton1.Enabled = True
        Button3.Enabled = True

    End Sub


    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim row As Integer = MetroGrid2.CurrentRow.Index
        Dim dr As DataRow = dt2.Rows(row)

        DoDeleteKRS(dr)

        GetUserKRS()
    End Sub
End Class