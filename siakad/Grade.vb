Public Class Grade
    Dim db As New Database
    Dim krs As DataRow


    Public Sub SetKrs(ByVal nkrs As DataRow)
        krs = nkrs
    End Sub

    Private Sub UpdateNilai()
        Dim nim = krs.Field(Of String)("nim")
        Dim kode = krs.Field(Of String)("kode_makul")
        Dim tahun = krs.Field(Of String)("tahun")
        Dim sem = krs.Field(Of String)("semester")
        If sem = "Genap" Then
            sem = 0
        Else
            sem = 1
        End If
        Dim nilai = MetroTextBox3.Text

        If nilai.Length > 2 Then
            MessageBox.Show("Nilai Maksimal 2 Karakter!")
        Else
            Dim sql = "UPDATE krs SET nilai='" + nilai + "' WHERE nim='" + nim + "' AND kode_makul = '" + kode + "' AND tahun = '" + tahun + "' AND semester = " + sem + ";"
            db.RunNonQuery(sql)
        End If


    End Sub
    Private Sub Grade_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Nilai " + krs.Field(Of String)("kode_makul")
        MetroTextBox1.Text = krs.Field(Of String)("nim")
        MetroTextBox2.Text = krs.Field(Of String)("name")
        MetroTextBox3.Text = krs.Field(Of String)("nilai")

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim result As DialogResult = MessageBox.Show("Apakah anda yakin untuk memberi nilai " + krs.Field(Of String)("name") + " dengan nilai " + MetroTextBox3.Text, "Siakad", MessageBoxButtons.YesNo)
        If result = DialogResult.Yes Then
            UpdateNilai()
            Dosen.RefreshDataNilai()
            Me.Close()
        End If
    End Sub

End Class