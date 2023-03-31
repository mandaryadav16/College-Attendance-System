Imports System.Data.SqlClient
Public Class frmCourseStudy
    Sub Reset()
        cmbDept.Text = ""
        cmbSch.Text = ""
        cmbDept.SelectedIndex = -1
        cmbSch.SelectedIndex = -1
        txtCStudy.Text = ""
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

    Sub fillDept()
        Try
            Dim CN As New SqlConnection(cs)
            CN.Open()
            adp = New SqlDataAdapter()
            adp.SelectCommand = New SqlCommand("SELECT distinct RTRIM(Dept) FROM Departments", CN)
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


    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Reset()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
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

        Try
            con = New SqlConnection(cs)
            con.Open()
            Dim ct As String = "select SchoolName, Dept, CComb from CStudy where CComb=@d1"
            cmd = New SqlCommand(ct)
            cmd.Parameters.AddWithValue("@d1", txtCStudy.Text)
            cmd.Connection = con
            rdr = cmd.ExecuteReader()

            If rdr.Read() Then
                MessageBox.Show("Course of Study Already Exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
                txtCStudy.Text = ""
                txtCStudy.Focus()
                If (rdr IsNot Nothing) Then
                    rdr.Close()
                End If
                Return
            End If

            con = New SqlConnection(cs)
            con.Open()

            Dim cb As String = "insert into CStudy(SchoolName,Dept,CComb) VALUES (@d1,@d2,@d3)"
            cmd = New SqlCommand(cb)
            cmd.Parameters.AddWithValue("@d1", cmbSch.Text)
            cmd.Parameters.AddWithValue("@d2", cmbDept.Text)
            cmd.Parameters.AddWithValue("@d3", txtCStudy.Text)
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

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click

        If txtCStudy.Text = "" Then
            MessageBox.Show("Please enter Course of Study", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtCStudy.Focus()
            Return
        End If

        Try

            con = New SqlConnection(cs)
            con.Open()

            Dim cb As String = "Update CStudy set SchoolName=@d1, Dept=@d2, CComb=@d3 where CComb=@d4"
            cmd = New SqlCommand(cb)
            cmd.Connection = con
            cmd.Parameters.AddWithValue("@d1", cmbSch.Text)
            cmd.Parameters.AddWithValue("@d2", cmbDept.Text)
            cmd.Parameters.AddWithValue("@d3", txtCStudy.Text)
            cmd.Parameters.AddWithValue("@d4", txtCStudyName.Text)
            cmd.ExecuteReader()
            con.Close()
            ' Dim st As String = "updated the Location '" & txtSchool.Text & "'"
            ' LogFunc(lblUser.Text, st)
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
            Dim cq As String = "delete from CStudy where CComb=@d1"
            cmd = New SqlCommand(cq)
            cmd.Parameters.AddWithValue("@d1", txtCStudyName.Text)
            cmd.Connection = con
            RowsAffected = cmd.ExecuteNonQuery()
            If RowsAffected > 0 Then
                'Dim st As String = "deleted the Location '" & txtSchool.Text & "'"
                'LogFunc(lblUser.Text, st)
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

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

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
                txtCStudy.Text = dr.Cells(2).Value.ToString()
                txtCStudyName.Text = dr.Cells(2).Value.ToString()
                cmbDept.Text = dr.Cells(1).Value.ToString()
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
            cmd = New SqlCommand("SELECT RTRIM(SchoolName), RTRIM(Dept), RTRIM(CComb) from CStudy order by SchoolName, Dept", con)
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            dgw.Rows.Clear()
            While (rdr.Read() = True)
                dgw.Rows.Add(rdr(0), rdr(1), rdr(2))
            End While
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub frmDepartment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Getdata()
        fillSchool()
        AcceptButton = btnSave
        'fillDept()
    End Sub

    Private Sub cmbSch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSch.SelectedIndexChanged
        cmbDept.Text = ""
        cmbDept.Items.Clear()

        Try
            Dim CN As New SqlConnection(cs)
            CN.Open()
            adp = New SqlDataAdapter()
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
End Class