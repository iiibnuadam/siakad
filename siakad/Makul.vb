Imports MySql.Data.MySqlClient
Public Class Makul
    Dim db As New Database()
    Dim conn As MySqlConnection = db.getConnection
    Dim makul As DataRow
    Dim nip As String

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        AddMakul()
    End Sub

    Public Sub setNIP(ByVal n As String)
        nip = n
    End Sub

    Private Sub AddMakul()
        Dim kode As String = MetroTextBox1.Text.ToUpper
        Dim name As String = MetroTextBox2.Text
        Dim sks As Integer = CInt(MetroTextBox3.Text)
        Dim sem As Integer = CInt(MetroTextBox4.Text)
        Dim sql As String = "INSERT INTO mata_kuliah VALUES(null, '" + kode + "','" + name + "'," + sks.ToString + "," + sem.ToString + ");"
        Try
            conn.Open()
            Dim cmd As New MySqlCommand(sql, conn)
            Dim cmd2 As New MySqlCommand("INSERT INTO dosen_makul VALUES('" + nip + "','" + kode + "')", conn)
            cmd.ExecuteNonQuery()
            cmd2.ExecuteNonQuery()
            conn.Close()
            MessageBox.Show("Data dimasukkan!")
            MetroTextBox1.Enabled = True
            MetroTextBox2.Enabled = False
            MetroTextBox3.Enabled = False
            MetroTextBox4.Enabled = False
            Button3.Hide()
            Button2.Hide()
            Button1.Hide()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Function setMakul(ByVal mk As DataRow)
        makul = mk
        Return True
    End Function

    Private Sub Makul_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not IsNothing(makul) Then
            Me.Text = makul.Field(Of String)("nama").ToString
            MetroTextBox1.Text = makul.Field(Of String)("kode").ToString
            MetroTextBox2.Text = makul.Field(Of String)("nama").ToString
            MetroTextBox3.Text = makul.Field(Of Integer)("sks").ToString
            MetroTextBox4.Text = makul.Field(Of Integer)("semester").ToString
            MetroTextBox1.Enabled = False
            MetroTextBox2.Enabled = False
            MetroTextBox3.Enabled = False
            MetroTextBox4.Enabled = False
            Button1.Show()
        Else
            MetroTextBox1.Enabled = True
            MetroTextBox2.Enabled = False
            MetroTextBox3.Enabled = False
            MetroTextBox4.Enabled = False
            Button3.Hide()
            Button2.Hide()
            Button1.Hide()
        End If
    End Sub

    Private Sub CheckMK()
        Dim sql As String = "SELECT * FROM mata_kuliah WHERE kode='" + MetroTextBox1.Text.ToUpper + "';"
        Try
            conn.Open()
            Dim com As New MySqlCommand(sql, conn)
            Dim adapter As New MySqlDataAdapter(com)
            Dim dt As New DataTable
            adapter.Fill(dt)
            If dt.Rows().Count() > 0 Then
                MessageBox.Show("Kode mata kuliah ditemukan!")
                Dim dr As DataRow = dt.Rows(0)
                makul = dr
                MetroTextBox1.Enabled = False
                MetroTextBox1.Text = MetroTextBox1.Text.ToUpper
                MetroTextBox2.Text = dr.Field(Of String)("nama").ToString
                MetroTextBox3.Text = dr.Field(Of Integer)("sks").ToString
                MetroTextBox3.Text = dr.Field(Of Integer)("semester").ToString
                Button1.Show()
            Else
                MessageBox.Show("Kode mata kuliah belum ada, silahkan dapat mengisi data mata kuliah!")
                MetroTextBox1.Enabled = False
                MetroTextBox1.Text = MetroTextBox1.Text.ToUpper
                MetroTextBox2.Enabled = True
                MetroTextBox2.Text = ""
                MetroTextBox3.Enabled = True
                MetroTextBox4.Enabled = True
                MetroTextBox3.Text = ""
                MetroTextBox4.Text = ""
                Button3.Show()
                MetroTextBox2.Select()
            End If
            conn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub MetroButton1_Click(sender As Object, e As EventArgs) Handles MetroButton1.Click
        Cursor.Current = Cursors.WaitCursor
        CheckMK()
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim name As String = MetroTextBox2.Text
        Dim sks As Integer = CInt(MetroTextBox3.Text)
        Dim sem As Integer = CInt(MetroTextBox4.Text)
        Dim kode As String = MetroTextBox1.Text
        Dim sql As String = "UPDATE mata_kuliah SET nama='" + name + "', sks=" + sks.ToString + " , semester=" + sem.ToString + " WHERE kode='" + kode.ToUpper + "';"
        Dim result As DialogResult = MessageBox.Show("Apakah anda yakin untuk mengubah data mata kuliah " + name + "?", "Siakad", MessageBoxButtons.YesNo)
        If result = DialogResult.Yes Then
            Try
                conn.Open()
                Dim cmd As New MySqlCommand(sql, conn)
                cmd.ExecuteNonQuery()
                MessageBox.Show("Data berhasil diubah!")
                MetroTextBox1.Enabled = True
                MetroTextBox2.Enabled = False
                MetroTextBox3.Enabled = False
                MetroTextBox4.Enabled = False
                Button3.Hide()
                Button2.Hide()
                Button1.Hide()
                MetroButton1.Enabled = True
                conn.Close()
            Catch ex As Exception
                Console.WriteLine(ex.Message)
                MessageBox.Show(ex.Message)
            End Try
        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Button1.Hide()
        MetroButton1.Enabled = False
        Button2.Show()
        MetroTextBox2.Enabled = True
        MetroTextBox3.Enabled = True
        MetroTextBox4.Enabled = True
    End Sub

End Class