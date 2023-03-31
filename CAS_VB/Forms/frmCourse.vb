Imports System.Data.SqlClient

Public Class frmCourse

    Sub Reset()
        txtCCode.Text = ""
        cmbDept.SelectedIndex = -1
        'cmbDept.Text = ""
        cmbSch.SelectedIndex = -1
        'cmbSch.Text = ""
        txtCTitle.Text = ""
        cmbCUnit.SelectedIndex = -1
        'cmbCUnit.Text = ""
        cmbLev.SelectedIndex = -1
        'cmbLev.Text = ""
        cmbSem.SelectedIndex = -1
        'cmbSem.Text = ""
        cmbCStatus.SelectedIndex = -1
        'cmbCStatus.Text = ""
        cmbLect.SelectedIndex = -1
        cmbLect.Text = ""
        btnSave.Enabled = True
        btnDelete.Enabled = False
        btnUpdate.Enabled = False
        cmbSch.Focus()
    End Sub

    Sub fillSchool()
        Try
            Dim CN1 As New SqlConnection(cs)
            CN1.Open()
            adp = New SqlDataAdapter()
            adp.SelectCommand = New SqlCommand("SELECT distinct RTRIM(SchoolName) FROM Schools", CN1)
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

    Sub fillLecturer()
        Try
            Dim CN As New SqlConnection(cs)
            CN.Open()
            adp = New SqlDataAdapter()
            adp.SelectCommand = New SqlCommand("SELECT distinct RTRIM(LecturerName) FROM Lecturers", CN)
            ds = New DataSet("ds")
            adp.Fill(ds)
            Dim dtable As DataTable = ds.Tables(0)
            cmbLect.Items.Clear()
            For Each drow As DataRow In dtable.Rows
                cmbLect.Items.Add(drow(0).ToString())
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
        If txtCCode.Text = "" Then
            MessageBox.Show("Please enter Course code", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtCCode.Focus()
            Return
        End If
        If cmbLev.Text = "" Then
            MessageBox.Show("Please select Course Level", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbLev.Focus()
            Return
        End If
        If cmbSem.Text = "" Then
            MessageBox.Show("Please select Course Semester", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbSem.Focus()
            Return
        End If
        Try
            con = New SqlConnection(cs)
            con.Open()
            Dim ct As String = "select SchoolName, Dept, CCode, CTitle, CUnit, Lev, Sem, CStatus, Lect_In_Charge from Courses where CCode=@d1"
            cmd = New SqlCommand(ct)
            cmd.Parameters.AddWithValue("@d1", txtCCode.Text)
            cmd.Connection = con
            rdr = cmd.ExecuteReader()

            If rdr.Read() Then
                MessageBox.Show("Course Already Exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
                txtCCode.Text = ""
                txtCCode.Focus()
                If (rdr IsNot Nothing) Then
                    rdr.Close()
                End If
                Return
            End If

            con = New SqlConnection(cs)
            con.Open()

            Dim cb As String = "insert into Courses(SchoolName, Dept, CCode, CTitle, CUnit, Lev, Sem, CStatus, Lect_In_Charge) VALUES (@d1,@d2,@d3,@d4,@d5,@d6,@d7,@d8,@d9)"
            cmd = New SqlCommand(cb)
            cmd.Parameters.AddWithValue("@d1", cmbSch.Text)
            cmd.Parameters.AddWithValue("@d2", cmbDept.Text)
            cmd.Parameters.AddWithValue("@d3", txtCCode.Text)
            cmd.Parameters.AddWithValue("@d4", txtCTitle.Text)
            cmd.Parameters.AddWithValue("@d5", cmbCUnit.Text)
            cmd.Parameters.AddWithValue("@d6", cmbLev.Text)
            cmd.Parameters.AddWithValue("@d7", cmbSem.Text)
            cmd.Parameters.AddWithValue("@d8", cmbCStatus.Text)
            cmd.Parameters.AddWithValue("@d9", cmbLect.Text)
            cmd.Connection = con
            cmd.ExecuteReader()
            con.Close()
            'Dim st As String = "added the new Location '" & txtSchool.Text & "'"
            'LogFunc(lblUser.Text, st)
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
        If txtCCode.Text = "" Then
            MessageBox.Show("Please enter Course Code", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtCCode.Focus()
            Return
        End If

        Try

            con = New SqlConnection(cs)
            con.Open()

            Dim cb As String = "Update Courses set SchoolName=@d1, Dept=@d2, CCode=@d3, CTitle=@d4, CUnit=@d5, Lev=@d6, Sem=@d7, CStatus=@d8, Lect_In_Charge=@d9 where CCode=@d10"
            cmd = New SqlCommand(cb)
            cmd.Connection = con
            cmd.Parameters.AddWithValue("@d1", cmbSch.Text)
            cmd.Parameters.AddWithValue("@d2", cmbDept.Text)
            cmd.Parameters.AddWithValue("@d3", txtCCode.Text)
            cmd.Parameters.AddWithValue("@d4", txtCTitle.Text)
            cmd.Parameters.AddWithValue("@d5", cmbCUnit.Text)
            cmd.Parameters.AddWithValue("@d6", cmbLev.Text)
            cmd.Parameters.AddWithValue("@d7", cmbSem.Text)
            cmd.Parameters.AddWithValue("@d8", cmbCStatus.Text)
            cmd.Parameters.AddWithValue("@d9", cmbLect.Text)
            cmd.Parameters.AddWithValue("@d10", txtCCodeName.Text)
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
            Dim cq As String = "delete from Courses where CCode=@d1"
            cmd = New SqlCommand(cq)
            cmd.Parameters.AddWithValue("@d1", txtCCodeName.Text)
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
                txtCCode.Text = dr.Cells(2).Value.ToString()
                txtCCodeName.Text = dr.Cells(2).Value.ToString()
                txtCTitle.Text = dr.Cells(3).Value.ToString()
                cmbCUnit.Text = dr.Cells(4).Value.ToString()
                cmbLev.Text = dr.Cells(5).Value.ToString()
                cmbSem.Text = dr.Cells(6).Value.ToString()
                cmbCStatus.Text = dr.Cells(7).Value.ToString()
                cmbLect.Text = dr.Cells(8).Value.ToString()
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
            cmd = New SqlCommand("SELECT RTRIM(SchoolName), RTRIM(Dept), RTRIM(CCode), RTRIM(CTitle), RTRIM(CUnit), RTRIM(Lev), RTRIM(Sem), RTRIM(CStatus), RTRIM(Lect_In_Charge) from Courses order by CCode", con)
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            dgw.Rows.Clear()
            While (rdr.Read() = True)
                dgw.Rows.Add(rdr(0), rdr(1), rdr(2), rdr(3), rdr(4), rdr(5), rdr(6), rdr(7), rdr(8))
            End While
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        'Reset()
        Me.Close()
    End Sub

    Private Sub frmCourse_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Reset()
        Getdata()
        fillSchool()
        AcceptButton = btnSave
        'fillLecturer()
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

    Private Sub cmbDept_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbDept.SelectedIndexChanged
        cmbLect.Text = ""
        cmbLect.Items.Clear()

        Try
            Dim CN As New SqlConnection(cs)
            CN.Open()
            adp = New SqlDataAdapter()
            'adp.SelectCommand = New SqlCommand("SELECT distinct RTRIM(Department) FROM Dept WHERE School='" & cmbSchool.Text & "'", CN)
            adp.SelectCommand = New SqlCommand("SELECT RTRIM(LecturerName) FROM Lecturers WHERE Dept='" & cmbDept.Text & "'", CN)
            ds = New DataSet("ds")
            adp.Fill(ds)
            Dim dtable As DataTable = ds.Tables(0)
            cmbLect.Items.Clear()
            For Each drow As DataRow In dtable.Rows
                cmbLect.Items.Add(drow(0).ToString())
            Next

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        'frmCourseImport.ShowDialog()
    End Sub
End Class