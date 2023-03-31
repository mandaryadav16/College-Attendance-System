Imports System.Data.SqlClient

Public Class frmLecturer

    Sub Reset()
        txtEMail.Text = ""
        cmbDept.SelectedIndex = -1
        'cmbDept.Text = ""
        cmbSch.SelectedIndex = -1
        'cmbSch.Text = ""
        txtLect.Text = ""
        txtPhone.Text = ""
        'cmbSem.Text = ""
        cmbSex.SelectedIndex = -1
        'cmbSex.Text = ""
        cmbRank.SelectedIndex = -1
        'cmbRank.Text = ""
        btnSave.Enabled = True
        btnDelete.Enabled = False
        btnUpdate.Enabled = False
        cmbSch.Focus()
    End Sub

    Sub fillSchool()
        Try
            Dim CN As New SqlConnection(cs)
            CN.Open()
            adp = New SqlDataAdapter()
            adp.SelectCommand = New SqlCommand("SELECT distinct RTRIM(SchoolName) FROM Schools", CN)
            ds = New DataSet("ds")
            adp.Fill(ds)
            Dim dtable As DataTable = ds.Tables(0)
            cmbSch.Items.Clear()
            For Each drow As DataRow In dtable.Rows
                cmbSch.Items.Add(drow(0).ToString())
            Next

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Reset()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If cmbSch.Text = "" Then
            MessageBox.Show("Please select School name", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbSch.Focus()
            Return
        End If
        If cmbDept.Text = "" Then
            MessageBox.Show("Please select Department", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbDept.Focus()
            Return
        End If
        If txtEMail.Text = "" Then
            MessageBox.Show("Please enter E-Mail", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtEMail.Focus()
            Return
        End If
        Try
            con = New SqlConnection(cs)
            con.Open()
            Dim ct As String = "select LecturerName, Rank, Gender, SchoolName, Dept, PhoneNo, Email from Lecturers where Email=@d1"
            cmd = New SqlCommand(ct)
            cmd.Parameters.AddWithValue("@d1", txtEMail.Text)
            cmd.Connection = con
            rdr = cmd.ExecuteReader()

            If rdr.Read() Then
                MessageBox.Show("E-Mail Already Exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
                txtEMail.Text = ""
                txtEMail.Focus()
                If (rdr IsNot Nothing) Then
                    rdr.Close()
                End If
                Return
            End If

            con = New SqlConnection(cs)
            con.Open()

            Dim cb As String = "insert into Lecturers(LecturerName, Rank, Gender, SchoolName, Dept, PhoneNo, Email) VALUES (@d1,@d2,@d3,@d4,@d5,@d6,@d7)"
            cmd = New SqlCommand(cb)
            cmd.Parameters.AddWithValue("@d1", txtLect.Text)
            cmd.Parameters.AddWithValue("@d2", cmbRank.Text)
            cmd.Parameters.AddWithValue("@d3", cmbSex.Text)
            cmd.Parameters.AddWithValue("@d4", cmbSch.Text)
            cmd.Parameters.AddWithValue("@d5", cmbDept.Text)
            cmd.Parameters.AddWithValue("@d6", txtPhone.Text)
            cmd.Parameters.AddWithValue("@d7", txtEMail.Text)
            cmd.Connection = con
            cmd.ExecuteReader()
            con.Close()
            MessageBox.Show("Successfully Saved", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information)
            btnSave.Enabled = False
            Getdata()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
        End Try
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If cmbSch.Text = "" Then
            MessageBox.Show("Please select School name", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbSch.Focus()
            Return
        End If
        If cmbDept.Text = "" Then
            MessageBox.Show("Please select Department", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbDept.Focus()
            Return
        End If
        If txtEMail.Text = "" Then
            MessageBox.Show("Please enter E-Mail", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtEMail.Focus()
            Return
        End If

        Try

            con = New SqlConnection(cs)
            con.Open()

            Dim cb As String = "Update Lecturers set LecturerName=@d1, Rank=@d2, Gender=@d3, SchoolName=@d4, Dept=@d5, PhoneNo=@d6, Email=@d7 where Email=@d8"
            cmd = New SqlCommand(cb)
            cmd.Connection = con
            cmd.Parameters.AddWithValue("@d1", txtLect.Text)
            cmd.Parameters.AddWithValue("@d2", cmbRank.Text)
            cmd.Parameters.AddWithValue("@d3", cmbSex.Text)
            cmd.Parameters.AddWithValue("@d4", cmbSch.Text)
            cmd.Parameters.AddWithValue("@d5", cmbDept.Text)
            cmd.Parameters.AddWithValue("@d6", txtPhone.Text)
            cmd.Parameters.AddWithValue("@d7", txtEMail.Text)
            cmd.Parameters.AddWithValue("@d8", txtEMailName.Text)
            cmd.ExecuteReader()
            con.Close()

            MessageBox.Show("Successfully updated", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information)
            btnUpdate.Enabled = False
            Getdata()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
        End Try
    End Sub
    Private Sub DeleteRecord()

        Try
            Dim RowsAffected As Integer = 0

            con = New SqlConnection(cs)
            con.Open()
            Dim cq As String = "delete from Lecturers where Email=@d1"
            cmd = New SqlCommand(cq)
            cmd.Parameters.AddWithValue("@d1", txtEMailName.Text)
            cmd.Connection = con
            RowsAffected = cmd.ExecuteNonQuery()
            If RowsAffected > 0 Then
                MessageBox.Show("Successfully deleted", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Getdata()
                Reset()
            Else
                MessageBox.Show("No Record found", "Sorry", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Reset()
            End If
            If con.State = ConnectionState.Open Then
                con.Close()

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
        End Try
    End Sub
    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Try
            If MessageBox.Show("Do you really want to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes Then
                DeleteRecord()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgw_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dgw.MouseClick
        Try
            If dgw.Rows.Count > 0 Then
                Dim dr As DataGridViewRow = dgw.SelectedRows(0)
                cmbSch.Text = dr.Cells(0).Value.ToString()
                cmbDept.Text = dr.Cells(1).Value.ToString()
                txtEMail.Text = dr.Cells(5).Value.ToString()
                txtEMailName.Text = dr.Cells(5).Value.ToString()
                txtLect.Text = dr.Cells(2).Value.ToString()
                cmbSex.Text = dr.Cells(3).Value.ToString()
                cmbRank.Text = dr.Cells(4).Value.ToString()
                txtPhone.Text = dr.Cells(6).Value.ToString()
                btnUpdate.Enabled = True
                btnDelete.Enabled = True
                btnSave.Enabled = False
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgw_RowPostPaint(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowPostPaintEventArgs) Handles dgw.RowPostPaint
        Dim strRowNumber As String = (e.RowIndex + 1).ToString()
        Dim size As SizeF = e.Graphics.MeasureString(strRowNumber, Me.Font)
        If dgw.RowHeadersWidth < Convert.ToInt32((size.Width + 20)) Then
            dgw.RowHeadersWidth = Convert.ToInt32((size.Width + 20))
        End If
        Dim b As Brush = SystemBrushes.ControlText
        e.Graphics.DrawString(strRowNumber, Me.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2))

    End Sub
    Public Sub Getdata()
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("SELECT RTRIM(SchoolName), RTRIM(Dept), RTRIM(LecturerName), RTRIM(Gender), RTRIM(Rank), RTRIM(Email), RTRIM(PhoneNo) from Lecturers order by Dept", con)
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            dgw.Rows.Clear()
            While (rdr.Read() = True)
                dgw.Rows.Add(rdr(0), rdr(1), rdr(2), rdr(3), rdr(4), rdr(5), rdr(6))
            End While
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        ' Reset()
        Me.Close()
    End Sub

    Private Sub frmLecturer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Reset()
        Getdata()
        fillSchool()
        AcceptButton = btnSave
    End Sub

    Private Sub cmbSch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSch.SelectedIndexChanged
        cmbDept.Text = ""
        cmbDept.Items.Clear()

        Try
            Dim CN As New SqlConnection(cs)
            CN.Open()
            adp = New SqlDataAdapter()
            'adp.SelectCommand = New SqlCommand("SELECT distinct RTRIM(Department) FROM Dept WHERE School='" & cmbSchool.Text & "'", CN)
            adp.SelectCommand = New SqlCommand("SELECT RTRIM(Dept) FROM Departments WHERE SchoolName='" & cmbSch.Text & "'", CN)
            ds = New DataSet("ds")
            adp.Fill(ds)
            Dim dtable As DataTable = ds.Tables(0)
            cmbDept.Items.Clear()
            For Each drow As DataRow In dtable.Rows
                cmbDept.Items.Add(drow(0).ToString())
            Next

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        'frmLecturerImport.ShowDialog()
    End Sub
End Class