Imports System.Data.SqlClient

Public Class frmStudent

    Sub Reset()
        txtMatNo.Text = ""
        cmbSess.SelectedIndex = -1
        cmbDept.SelectedIndex = -1
        'cmbDept.Text = ""
        cmbSch.SelectedIndex = -1
        'cmbSch.Text = ""
        cmbCStudy.SelectedIndex = -1
        'cmbCStudy.Text = ""
        txtStudName.Text = ""
        txtPNo.Text = ""
        'cmbCUnit.Text = ""
        cmbLev.SelectedIndex = -1
        'cmbLev.Text = ""
        cmbSex.SelectedIndex = -1
        'cmbSem.Text = ""
        txtEMail.Text = ""
        'cmbCStatus.Text = ""
        txtAddress.Text = ""
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

    Private Sub BtnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Reset()
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If cmbSch.Text = "" Then
            MessageBox.Show("Please select school name", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbSch.Focus()
            Return
        End If
        If cmbDept.Text = "" Then
            MessageBox.Show("Please select department", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbDept.Focus()
            Return
        End If
        If cmbCStudy.Text = "" Then
            MessageBox.Show("Please select course of study", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbCStudy.Focus()
            Return
        End If
        If txtMatNo.Text = "" Then
            MessageBox.Show("Please enter student matriculation number", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtMatNo.Focus()
            Return
        End If
        If cmbLev.Text = "" Then
            MessageBox.Show("Please select student Level", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbLev.Focus()
            Return
        End If
        If cmbSex.Text = "" Then
            MessageBox.Show("Please select sex", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbSex.Focus()
            Return
        End If
        If cmbSess.Text = "" Then
            MessageBox.Show("Please select session", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbSess.Focus()
            Return
        End If
        Try
            con = New SqlConnection(cs)
            con.Open()
            Dim ct As String = "select MatricNo, StudName, Lev, Gender, SchoolName, Dept, PhoneNo, Email, Address, CComb, Sess from Students where MatricNo=@d1"
            cmd = New SqlCommand(ct)
            cmd.Parameters.AddWithValue("@d1", txtMatNo.Text)
            cmd.Connection = con
            rdr = cmd.ExecuteReader()

            If rdr.Read() Then
                MessageBox.Show("Student Already Exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
                txtMatNo.Text = ""
                txtMatNo.Focus()
                If (rdr IsNot Nothing) Then
                    rdr.Close()
                End If
                Return
            End If

            con = New SqlConnection(cs)
            con.Open()

            Dim cb As String = "insert into Students(MatricNo, StudName, Lev, Gender, SchoolName, Dept, PhoneNo, Email, Address, CComb, Sess) VALUES (@d1,@d2,@d3,@d4,@d5,@d6,@d7,@d8,@d9,@d10,@d11)"
            cmd = New SqlCommand(cb)
            cmd.Parameters.AddWithValue("@d1", txtMatNo.Text)
            cmd.Parameters.AddWithValue("@d2", txtStudName.Text)
            cmd.Parameters.AddWithValue("@d3", cmbLev.Text)
            cmd.Parameters.AddWithValue("@d4", cmbSex.Text)
            cmd.Parameters.AddWithValue("@d5", cmbSch.Text)
            cmd.Parameters.AddWithValue("@d6", cmbDept.Text)
            cmd.Parameters.AddWithValue("@d7", txtPNo.Text)
            cmd.Parameters.AddWithValue("@d8", txtEMail.Text)
            cmd.Parameters.AddWithValue("@d9", txtAddress.Text)
            cmd.Parameters.AddWithValue("@d10", cmbCStudy.Text)
            cmd.Parameters.AddWithValue("@d11", cmbSess.Text)
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

    Private Sub BtnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If cmbSch.Text = "" Then
            MessageBox.Show("Please select school name", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbSch.Focus()
            Return
        End If
        If cmbDept.Text = "" Then
            MessageBox.Show("Please select department", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbDept.Focus()
            Return
        End If
        If cmbCStudy.Text = "" Then
            MessageBox.Show("Please select course of study", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbCStudy.Focus()
            Return
        End If
        If txtMatNo.Text = "" Then
            MessageBox.Show("Please enter student matriculation number", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtMatNo.Focus()
            Return
        End If
        If cmbSess.Text = "" Then
            MessageBox.Show("Please select session", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbSess.Focus()
            Return
        End If
        Try

            con = New SqlConnection(cs)
            con.Open()

            Dim cb As String = "Update Students set MatricNo=@d1, StudName=@d2, Lev=@d3, Gender=@d4, SchoolName=@d5, Dept=@d6, PhoneNo=@d7, Email=@d8, Address=@d9, CComb=@d10, Sess=@d11 where MatricNo=@d12"
            cmd = New SqlCommand(cb)
            cmd.Connection = con
            cmd.Parameters.AddWithValue("@d1", txtMatNo.Text)
            cmd.Parameters.AddWithValue("@d2", txtStudName.Text)
            cmd.Parameters.AddWithValue("@d3", cmbLev.Text)
            cmd.Parameters.AddWithValue("@d4", cmbSex.Text)
            cmd.Parameters.AddWithValue("@d5", cmbSch.Text)
            cmd.Parameters.AddWithValue("@d6", cmbDept.Text)
            cmd.Parameters.AddWithValue("@d7", txtPNo.Text)
            cmd.Parameters.AddWithValue("@d8", txtEMail.Text)
            cmd.Parameters.AddWithValue("@d9", txtAddress.Text)
            cmd.Parameters.AddWithValue("@d10", cmbCStudy.Text)
            cmd.Parameters.AddWithValue("@d11", cmbSess.Text)
            cmd.Parameters.AddWithValue("@d12", txtMatNoName.Text)
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
            Dim cq As String = "delete from Students where MatricNo=@d1"
            cmd = New SqlCommand(cq)
            cmd.Parameters.AddWithValue("@d1", txtMatNoName.Text)
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

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Try
            If MessageBox.Show("Do you really want to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes Then
                DeleteRecord()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgw_MouseClick(sender As Object, e As MouseEventArgs) Handles dgw.MouseClick
        Try
            If dgw.Rows.Count > 0 Then
                Dim dr As DataGridViewRow = dgw.SelectedRows(0)
                txtMatNo.Text = dr.Cells(0).Value.ToString()
                txtMatNoName.Text = dr.Cells(0).Value.ToString()
                txtStudName.Text = dr.Cells(1).Value.ToString()
                cmbLev.Text = dr.Cells(2).Value.ToString()
                cmbSex.Text = dr.Cells(3).Value.ToString()
                cmbSch.Text = dr.Cells(4).Value.ToString()
                cmbDept.Text = dr.Cells(5).Value.ToString()
                cmbCStudy.Text = dr.Cells(6).Value.ToString()
                txtPNo.Text = dr.Cells(7).Value.ToString()
                txtEMail.Text = dr.Cells(9).Value.ToString()
                txtAddress.Text = dr.Cells(8).Value.ToString()
                cmbSess.Text = dr.Cells(10).Value.ToString()
                btnUpdate.Enabled = True
                btnDelete.Enabled = True
                btnSave.Enabled = False
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgw_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles dgw.RowPostPaint
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
            cmd = New SqlCommand("SELECT RTRIM(MatricNo), RTRIM(StudName), RTRIM(Lev), RTRIM(Gender), RTRIM(SchoolName), RTRIM(Dept), RTRIM(CComb), RTRIM(PhoneNo), RTRIM(Email), RTRIM(Address), RTRIM(Sess) from Students order by MatricNo", con)
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            dgw.Rows.Clear()
            While (rdr.Read() = True)
                dgw.Rows.Add(rdr(0), rdr(1), rdr(2), rdr(3), rdr(4), rdr(5), rdr(6), rdr(7), rdr(8), rdr(9), rdr(10))
            End While
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BtnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub FrmStudent_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AcceptButton = btnSave
        'Reset()
        Getdata()
        fillSchool()
        'fillLecturer()
    End Sub

    Private Sub CmbSch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSch.SelectedIndexChanged
        cmbCStudy.Text = ""
        cmbCStudy.Items.Clear()

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

    Private Sub CmbDept_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbDept.SelectedIndexChanged
        cmbCStudy.Text = ""
        cmbCStudy.Items.Clear()

        Try
            Dim CN As New SqlConnection(cs)
            CN.Open()
            adp = New SqlDataAdapter()
            'adp.SelectCommand = New SqlCommand("SELECT distinct RTRIM(Department) FROM Dept WHERE School='" & cmbSchool.Text & "'", CN)
            adp.SelectCommand = New SqlCommand("SELECT RTRIM(CComb) FROM CStudy WHERE Dept='" & cmbDept.Text & "'", CN)
            ds = New DataSet("ds")
            adp.Fill(ds)
            Dim dtable As DataTable = ds.Tables(0)
            cmbCStudy.Items.Clear()
            For Each drow As DataRow In dtable.Rows
                cmbCStudy.Items.Add(drow(0).ToString())
            Next

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BtnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        frmStudentImport.ShowDialog()
    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

    End Sub
End Class