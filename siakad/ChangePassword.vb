Imports BCrypt
Imports MySql.Data.MySqlClient

Public Class ChangePassword
    Dim data As DataRow
    Dim db As New Database
    Dim conn As MySqlConnection = db.getConnection
    Dim dosen As Dosen
    Dim mhs As Mahasiswa
    Public Function setPassword(ByVal p As DataRow, ByVal d As Dosen)
        data = p
        dosen = d
        Return True
    End Function

    Public Function setPassword(ByVal p As DataRow, ByVal d As Mahasiswa)
        data = p
        mhs = d
        Return True
    End Function
    Private Sub ChangePassword_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If IsNothing(data) Then
            MetroLabel4.Text = "NO DATA!"
            MetroLabel4.Show()
        Else
            MetroLabel4.Hide()
        End If
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Cursor.Current = Cursors.WaitCursor
        MetroLabel4.Hide()
        If MetroTextBox2.Text <> MetroTextBox3.Text Then
            MetroLabel4.Text = "Konfirmasi password salah"
            MetroLabel4.Show()
        Else
            If BCrypt.CheckPassword(MetroTextBox1.Text, data.Field(Of String)("password")) Then
                Dim result As DialogResult = MessageBox.Show("Apakah anda yakin untuk mengubah password?", "Siakad", MessageBoxButtons.YesNo)
                If result = DialogResult.Yes Then
                    Dim sql As String = "UPDATE users SET password='" + BCrypt.HashPassword(MetroTextBox2.Text, BCrypt.GenerateSalt(12)) + "' WHERE id=" + data.Field(Of Integer)("user_id").ToString
                    Try
                        conn.Open()
                        Dim cmd As New MySqlCommand(sql, conn)
                        cmd.ExecuteNonQuery()
                        MessageBox.Show("Password berhasil diubah!")

                        Dim dt As New DataTable
                        Dim nsql As String = "SELECT A.user_id, B.email, B.password,C.role_name, C.role_table, C.role_r_table FROM user_role A inner join users B on A.user_id = B.id INNER JOIN roles C on A.role_id = C.id WHERE A.user_id = '" + data.Field(Of Integer)("user_id").ToString + "'"
                        Dim ncmd As New MySqlCommand(nsql, conn)
                        Dim adapter As New MySqlDataAdapter(ncmd)
                        adapter.Fill(dt)
                        data = dt.Rows(0)
                        If IsNothing(mhs) Then
                            dosen.setData(data)
                        Else
                            mhs.setData(data)
                        End If

                        conn.Close()
                        MetroTextBox1.Text = "Password"
                        MetroTextBox2.Text = "Password"
                        MetroTextBox3.Text = "Password"
                        Me.Close()
                    Catch ex As Exception
                        MessageBox.Show(ex.Message)
                        Console.WriteLine(ex.Message)
                    End Try
                End If

            Else
                MetroLabel4.Text = "Password lama salah!"
                MetroLabel4.Show()
            End If
        End If
        Cursor.Current = Cursors.Default
    End Sub
End Class