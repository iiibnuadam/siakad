Imports MySql.Data.MySqlClient

Public Class Mahasiswa
    Private myData As DataRow
    Private db As New Database
    Private profile As DataRow


    Public Function setData(ByVal dt As DataRow)
        myData = dt
        Return True
    End Function

    Private Sub Mahasiswa_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Cls()
        Try
            Dim dt As New DataTable
            Dim sql As String = "select A.user_id,A.mhs_nim,c.email,B.name, B.place_of_birth, B.date_of_birth, B.address, B.phone from user_mahasiswa A inner join mahasiswa B on A.mhs_nim = B.nim inner join users C on A.user_id = C.id where A.user_id = " + myData.Field(Of Integer)("user_id").ToString
            dt = db.RunQuery(sql)
            If dt.Rows().Count() > 0 Then

                profile = dt.Rows(0)
            Else
                MessageBox.Show("No profile loaded! App Can't continue!")
            End If
            Me.Text = "Selamat datang " + profile.Field(Of String)("name").ToString
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try

    End Sub

    Private Sub Mahasiswa_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Signin.Show()
    End Sub

    Private Sub LogoutToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub KeluarAplikasiToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Close()
    End Sub

    Private Sub KeluarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles KeluarToolStripMenuItem.Click
        Dim result As DialogResult = MessageBox.Show("Apakah anda yakin untuk keluar", "Siakad", MessageBoxButtons.YesNo)
        If result = DialogResult.Yes Then
            Close()
        End If
    End Sub

    Private Sub UbahPasswordToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UbahPasswordToolStripMenuItem.Click
        Dim cp As New ChangePassword
        cp.setPassword(myData, Me)
        cp.Show()
    End Sub

    Private Sub ProfilToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ProfilToolStripMenuItem.Click
        Dim prof As New Profile
        prof.setData(profile, myData)
        prof.Show()

    End Sub

    Private Sub LihatKRSToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LihatKRSToolStripMenuItem.Click
        Cls()
        Dim currentDate As DateTime = DateTime.Now
        Dim sem As Integer
        Dim year As String
        If currentDate.Month > 0 And currentDate.Month < 7 Then
            sem = 0
            year = (currentDate.Year - 1).ToString + "/" + (currentDate.Year).ToString
        Else
            sem = 1
            year = (currentDate.Year).ToString + "/" + (currentDate.Year + 1).ToString
        End If

        Dim sql As String = "Select A.kode_makul, C.nama,C.sks, D.nama_dosen, B.nip FROM `krs` A INNER JOIN dosen_makul B On A.kode_makul = B.kode_makul INNER JOIN mata_kuliah C On A.kode_makul = C.kode INNER JOIN dosen D On B.nip = D.nip WHERE A.nim = '" + profile.Field(Of String)("mhs_nim") + "' AND A.semester = " + sem.ToString + " AND A.tahun='" + year + "'"

        Dim dataSource As New DataTable
        dataSource = db.RunQuery(sql)
        MetroGrid1.DataSource = dataSource
        MetroGrid1.Visible = True

    End Sub

    Private Sub UbahKRSToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UbahKRSToolStripMenuItem.Click
        Dim krs As New Krs
        krs.SetNIM(profile.Field(Of String)("mhs_nim"))
        krs.Show()
    End Sub

    Sub Cls()
        MetroGrid1.Visible = False
        MetroLabel1.Visible = False
        MetroLabel2.Visible = False
        MetroComboBox1.Visible = False
        MetroComboBox2.Visible = False
        MetroGrid2.Visible = False
    End Sub

    Private Sub MetroComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles MetroComboBox1.SelectedIndexChanged
        MetroComboBox2.Enabled = True
    End Sub

    Private Sub MetroComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles MetroComboBox2.SelectedIndexChanged
        Dim tahun = MetroComboBox1.Text
        Dim sem As String
        If MetroComboBox2.Text = "Genap" Then
            sem = 0
        Else
            sem = 1
        End If

        Dim sql = "SELECT B.kode_makul,D.nama, A.nilai, C.nama_dosen FROM `krs` A INNER JOIN dosen_makul B ON A.kode_makul = B.kode_makul INNER JOIN dosen C ON B.nip = C.nip INNER JOIN mata_kuliah D ON A.kode_makul = D.kode WHERE A.semester=" + sem + " AND A.tahun = '" + tahun + " ' AND A.nim = '" + profile.Field(Of String)("mhs_nim") + "'"

        Dim dt = db.RunQuery(sql)
        MetroGrid2.DataSource = dt

    End Sub

    Private Sub LihatNilaiToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LihatNilaiToolStripMenuItem.Click
        Cls()
        MetroComboBox2.Enabled = False
        MetroLabel1.Visible = True
        MetroLabel2.Visible = True
        MetroComboBox1.Visible = True
        MetroComboBox2.Visible = True
        MetroGrid2.Visible = True
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Cls()
    End Sub
End Class