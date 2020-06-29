Imports MySql.Data.MySqlClient

Public Class Dosen
    Private myData As DataRow
    Private myProfile As DataRow
    Private db As New Database

    Public Function setData(ByVal dt As DataRow)
        myData = dt
        Return True
    End Function

    Private Sub Dosen_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        HideAll()

        Try
            Dim dt As New DataTable
            Dim sql As String = "select A.user_id,A.dosen_nip,c.email,B.nama_dosen, B.place_of_birth, B.date_of_birth, B.address, B.phone from user_dosen A inner join dosen B on A.dosen_nip = B.nip inner join users C on A.user_id = C.id where A.user_id = " + myData.Field(Of Integer)("user_id").ToString
            dt = db.RunQuery(sql)
            If dt.Rows().Count() > 0 Then

                myProfile = dt.Rows(0)
            Else
                MessageBox.Show("No profile loaded! App Can't continue!")
            End If
            Me.Text = "Selamat datang, " + myProfile.Field(Of String)("nama_dosen").ToString
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    Private Sub Dosen_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Signin.Show()
    End Sub

    Private Sub ProfilToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ProfilToolStripMenuItem.Click
        Dim p As New Profile
        p.setData(myProfile, myData)
        p.Show()
    End Sub

    Private Sub TambahMataKuliahToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TambahMataKuliahToolStripMenuItem.Click
        Dim mk As New Makul()
        mk.setNIP(myProfile.Field(Of String)("dosen_nip"))
        mk.Show()
    End Sub

    Private Sub UbahPasswordToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UbahPasswordToolStripMenuItem.Click
        Dim cp As New ChangePassword
        cp.setPassword(myData, Me)
        cp.Show()
    End Sub

    Private Sub HideAll()
        MetroLabel1.Visible = False
        MetroLabel2.Visible = False
        MetroLabel3.Visible = False
        MetroComboBox1.Visible = False
        MetroComboBox2.Visible = False
        MetroComboBox3.Visible = False
        MetroGrid1.Visible = False
    End Sub

    Private Sub BeriNilaiToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BeriNilaiToolStripMenuItem.Click
        MetroComboBox2.Enabled = False
        MetroComboBox3.Enabled = False
        MetroLabel1.Visible = True
        MetroLabel2.Visible = True
        MetroLabel3.Visible = True
        MetroComboBox1.Visible = True
        MetroComboBox2.Visible = True
        MetroComboBox3.Visible = True
        MetroGrid1.Visible = True
    End Sub

    Private Sub MetroComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles MetroComboBox1.SelectedIndexChanged
        MetroComboBox2.Enabled = True
    End Sub

    Private Sub MetroComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles MetroComboBox2.SelectedIndexChanged
        MetroComboBox3.Enabled = True
        MetroComboBox3.Items.Clear()
        Dim tahun = MetroComboBox1.Text
        Dim sem As String
        If MetroComboBox2.Text = "Genap" Then
            sem = 0
        Else
            sem = 1
        End If
        Dim sql = "SELECT * FROM `KRS` A INNER JOIN dosen_makul B ON A.kode_makul = B.kode_makul WHERE A.tahun='" + tahun + "' AND A.semester=" + sem.ToString + " AND B.nip='" + myProfile.Field(Of String)("dosen_nip") + "' GROUP BY A.kode_makul;"
        Dim makulAvailable = db.RunQuery(sql)

        If IsNothing(makulAvailable) Then
            MessageBox.Show("Anda tidak memiliki mahasiswa dan mata kuliah yang di ampu pada semester ini")
        Else
            For Each row As DataRow In makulAvailable.Rows
                MetroComboBox3.Items.Add(row.Field(Of String)("kode_makul"))
            Next
        End If



    End Sub

    Public Sub RefreshDataNilai()
        Dim tahun = MetroComboBox1.Text
        Dim sem As String
        If MetroComboBox2.Text = "Genap" Then
            sem = 0
        Else
            sem = 1
        End If
        Dim kode = MetroComboBox3.Text

        Dim sql = "SELECT A.kode_makul, A.nim, B.name, A.tahun, A.semester, A.nilai FROM `KRS` A INNER JOIN mahasiswa B ON A.nim = B.nim WHERE A.kode_makul = '" + kode + "' AND tahun='" + tahun + "' AND semester =" + sem.ToString + "; "
        Dim da = db.RunQuery(sql)
        Dim dt = da.Clone
        dt.Columns("semester").DataType = GetType(String)
        For Each row As DataRow In da.Rows
            dt.ImportRow(row)
        Next

        For Each row As DataRow In dt.Rows
            If row.Field(Of String)("semester") = 0 Then
                row.SetField("semester", "Genap")
            Else
                row.SetField("semester", "Ganjil")
            End If
        Next
        MetroGrid1.DataSource = dt
    End Sub

    Private Sub MetroComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles MetroComboBox3.SelectedIndexChanged
        RefreshDataNilai()
    End Sub


    Private Sub ClearScreenToolStripMenuItem_Click(sender As Object, e As EventArgs)
        HideAll()
    End Sub

    Private Sub KeluarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles KeluarToolStripMenuItem.Click
        Dim result As DialogResult = MessageBox.Show("Apakah anda yakin untuk keluar", "Siakad", MessageBoxButtons.YesNo)
        If result = DialogResult.Yes Then
            Close()
        End If
    End Sub

    Private Sub MetroGrid1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles MetroGrid1.CellDoubleClick
        Dim g As New Grade
        Dim index As Integer = MetroGrid1.CurrentRow.Index
        Dim dt = MetroGrid1.DataSource
        Dim data As DataRow = dt.Rows(index)
        g.SetKrs(data)
        g.ShowDialog()
        RefreshDataNilai()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        HideAll()
    End Sub
End Class