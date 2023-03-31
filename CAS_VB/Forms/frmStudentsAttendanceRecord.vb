Imports System.Data.SqlClient
Imports System.IO
Imports Excel = Microsoft.Office.Interop.Excel

Public Class frmStudentsAttendanceRecord
    Dim Status As String

    Sub Reset()
        cmbCCode.Text = ""
        cmbDept.SelectedIndex = -1
        'cmbDept.Text = ""
        cmbSch.SelectedIndex = -1
        'cmbSch.Text = ""
        lblCTitle.Text = ""
        cmbCCode.SelectedIndex = -1
        'cmbSess.Text = ""
        cmbSess.SelectedIndex = -1
        cmbSess.Focus()
        btnSave.Enabled = True
        dtpDate.Value = Today
        cmbAttendanceType.SelectedIndex = -1
        cmbStaffName.Text = ""
        dgw.Rows.Clear()
        dtpDateFrom.Value = Today
        dtpDateTo.Value = Today
        lblTotalClasses.Visible = False
        'txtSt_ID.Text = ""

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

    Sub fillStaffName()
        Try
            con = New SqlConnection(cs)
            con.Open()
            adp = New SqlDataAdapter()
            'adp.SelectCommand = New SqlCommand("SELECT RTRIM(LecturerName) FROM Lecturers where Dept='" & cmbDept.Text & "'", con)
            adp.SelectCommand = New SqlCommand("SELECT RTRIM(LecturerName) FROM Lecturers", con)
            ds = New DataSet("ds")
            adp.Fill(ds)
            dtable = ds.Tables(0)
            cmbStaffName.Items.Clear()
            For Each drow As DataRow In dtable.Rows
                cmbStaffName.Items.Add(drow(0).ToString())
            Next

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Sub fillAttendanceType()
        Try
            con = New SqlConnection(cs)
            con.Open()
            adp = New SqlDataAdapter()
            adp.SelectCommand = New SqlCommand("SELECT RTRIM(Type) FROM AttendanceType", con)
            ds = New DataSet("ds")
            adp.Fill(ds)
            dtable = ds.Tables(0)
            cmbAttendanceType.Items.Clear()
            For Each drow As DataRow In dtable.Rows
                cmbAttendanceType.Items.Add(drow(0).ToString())
            Next

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub frmStudenstAttendanceRecord_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        fillSchool()
        fillAttendanceType()
        fillStaffName()
    End Sub

    Private Sub cmbSch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSch.SelectedIndexChanged
        cmbCStudy.Text = ""
        cmbCStudy.Items.Clear()

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

    Private Sub cmbDept_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbDept.SelectedIndexChanged
        cmbCCode.Text = ""
        cmbCCode.Items.Clear()
        lblCTitle.Text = ""

        Try
            Dim CN As New SqlConnection(cs)
            CN.Open()
            adp = New SqlDataAdapter()
            adp.SelectCommand = New SqlCommand("SELECT RTRIM(CCode) FROM Courses WHERE Dept='" & cmbDept.Text & "'", CN)
            ds = New DataSet("ds")
            adp.Fill(ds)
            Dim dtable As DataTable = ds.Tables(0)
            cmbCCode.Items.Clear()
            For Each drow As DataRow In dtable.Rows
                cmbCCode.Items.Add(drow(0).ToString())
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmbCCode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCCode.SelectedIndexChanged

        lblCTitle.Text = ""
        lblLev.Text = ""
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = con.CreateCommand()
            cmd.CommandText = "SELECT CTitle, Lev FROM Courses WHERE CCode= @d1"
            cmd.Parameters.AddWithValue("@d1", cmbCCode.Text)
            rdr = cmd.ExecuteReader()
            If rdr.Read() Then
                ' lblSem.Text = rdr.GetValue(0)
                lblCTitle.Text = rdr.GetValue(0)
                lblLev.Text = rdr.GetValue(1)
                'lblCUnit.Text = rdr.GetValue(2)
            End If
            If (rdr IsNot Nothing) Then
                rdr.Close()
            End If
            If con.State = ConnectionState.Open Then
                con.Close()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Try
            If Len(Trim(cmbDept.Text)) = 0 Then
                MessageBox.Show("Please select department", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cmbDept.Focus()
                Exit Sub
            End If
            If Len(Trim(cmbSess.Text)) = 0 Then
                MessageBox.Show("Please select session", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cmbSess.Focus()
                Exit Sub
            End If
            If Len(Trim(cmbCCode.Text)) = 0 Then
                MessageBox.Show("Please select course code", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cmbCCode.Focus()
                Exit Sub
            End If
            If Len(Trim(cmbAttendanceType.Text)) = 0 Then
                MessageBox.Show("Please select attendance type", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cmbAttendanceType.Focus()
                Exit Sub
            End If
            con = New SqlConnection(cs)
            con.Open()
            'cmd = New SqlCommand("Select distinct RTRIM(Students.MatricNo),RTRIM(Students.StudName),Count(Attendance.Status),(Count(Attendance.Status) * 100)/(Select Count(distinct Attendance.AttendanceID) from Students,Courses,Attendance,AttendanceMaster where Students.Dept=Courses.Dept and AttendanceMaster.ID=Attendance.AttendanceID and Attendance.MatricNo=Students.MatricNo and Students.Sess=@d1 and Courses.CCode=@d2 and Students.Dept=@d3 and Attendance.Date between @date1 and @date2 ) from Students,Courses,Attendance,AttendanceMaster where Students.Dept=Courses.Dept and Courses.CCode=Attendance.CCode and AttendanceMaster.ID=Attendance.AttendanceID and Attendance.MatricNo=Students.MatricNo and Students.Sess=@d1 and Courses.CCode=@d2 and Students.Dept=@d3 and Attendance.Status='P' and Attendance.Date between @date1 and @date2 group by Students.MatricNo,Students.StudName order by 2", con)
            cmd = New SqlCommand("Select distinct RTRIM(Students.MatricNo),RTRIM(Students.StudName),Count(Attendance.Status),(Count(Attendance.Status) * 100)/(Select Count(distinct Attendance.AttendanceID) from Students,Courses,Attendance,AttendanceMaster where Students.Dept=Courses.Dept and AttendanceMaster.ID=Attendance.AttendanceID and Attendance.MatricNo=Students.MatricNo and Attendance.Sess=@d1 and Courses.CCode=@d2 and Students.Dept=@d3 and AttendanceMaster.AttendanceType=@d4 and Attendance.Date between @date1 and @date2 ) from Students,Courses,Attendance,AttendanceMaster where Students.Dept=Courses.Dept and Courses.CCode=Attendance.CCode and AttendanceMaster.ID=Attendance.AttendanceID and Attendance.MatricNo=Students.MatricNo and Attendance.Sess=@d1 and Courses.CCode=@d2 and Students.Dept=@d3 and AttendanceMaster.AttendanceType=@d4 and Attendance.Status='P' and Attendance.Date between @date1 and @date2 group by Students.MatricNo,Students.StudName order by 1", con)
            'cmd = New SqlCommand("Select distinct RTRIM(Students.MatricNo),RTRIM(Students.StudName),Count(Attendance.Status),(Count(Attendance.Status) * 100)/(Select Count(distinct Attendance.AttendanceID) from Students,Courses,Attendance,AttendanceMaster where Students.Dept=Courses.Dept and AttendanceMaster.ID=Attendance.AttendanceID and Attendance.MatricNo=Students.MatricNo and Courses.CCode=@d2 and Students.Dept=@d3 and AttendanceMaster.AttendanceType=@d4 and Attendance.Date between @date1 and @date2 ) from Students,Courses,Attendance,AttendanceMaster where Students.Dept=Courses.Dept and Courses.CCode=Attendance.CCode and AttendanceMaster.ID=Attendance.AttendanceID and Attendance.MatricNo=Students.MatricNo and Courses.CCode=@d2 and Students.Dept=@d3 and AttendanceMaster.AttendanceType=@d4 and Attendance.Status='P' and Attendance.Date between @date1 and @date2 group by Students.MatricNo,Students.StudName order by 1", con)
            cmd.Parameters.Add("@date1", SqlDbType.DateTime, 30, "Date").Value = dtpDateFrom.Value.Date
            cmd.Parameters.Add("@date2", SqlDbType.DateTime, 30, "Date").Value = dtpDateTo.Value.Date
            cmd.Parameters.AddWithValue("@d1", cmbSess.Text)
            cmd.Parameters.AddWithValue("@d2", cmbCCode.Text)
            cmd.Parameters.AddWithValue("@d3", cmbDept.Text)
            cmd.Parameters.AddWithValue("@d4", cmbAttendanceType.Text)
            rdr = cmd.ExecuteReader()
            dgw.Rows.Clear()
            While rdr.Read()
                dgw.Rows.Add(rdr(0), rdr(1), rdr(2), rdr(3))
            End While
            con.Close()
            lblTotalClasses.Visible = True
            con = New SqlConnection(cs)
            con.Open()
            'cmd = New SqlCommand("Select Count(distinct Attendance.AttendanceID) from Students,Courses,Attendance,AttendanceMaster where  Students.Dept=Courses.Dept and AttendanceMaster.ID=Attendance.AttendanceID and Attendance.MatricNo=Students.MatricNo and Students.Sess=@d1 and Courses.CCode=@d2 and Students.Dept=@d3 and Attendance.Date between @date1 and @date2", con)
            cmd = New SqlCommand("Select Count(distinct Attendance.AttendanceID) from Students,Courses,Attendance,AttendanceMaster where Students.Dept=Courses.Dept and AttendanceMaster.ID=Attendance.AttendanceID and Attendance.MatricNo=Students.MatricNo and Attendance.Sess=@d1 and Courses.CCode=@d2 and Students.Dept=@d3 and AttendanceMaster.AttendanceType=@d4 and Attendance.Date between @date1 and @date2", con)
            cmd.Parameters.Add("@date1", SqlDbType.DateTime, 30, "Date").Value = dtpDateFrom.Value.Date
            cmd.Parameters.Add("@date2", SqlDbType.DateTime, 30, "Date").Value = dtpDateTo.Value.Date
            cmd.Parameters.AddWithValue("@d1", cmbSess.Text)
            cmd.Parameters.AddWithValue("@d2", cmbCCode.Text)
            cmd.Parameters.AddWithValue("@d3", cmbDept.Text)
            cmd.Parameters.AddWithValue("@d4", cmbAttendanceType.Text)
            rdr = cmd.ExecuteReader()
            If rdr.Read() Then
                lblTotalClasses.Text = rdr.GetValue(0)
            End If
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Reset()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If dgw.Rows.Count = 0 Then
            MessageBox.Show("Sorry no record added to grid", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
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
        If cmbCCode.Text = "" Then
            MessageBox.Show("Please select Course code", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbCCode.Focus()
            Return
        End If
        If cmbAttendanceType.Text = "" Then
            MessageBox.Show("Please select attendance type", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbAttendanceType.Focus()
            Return
        End If
        If cmbSess.Text = "" Then
            MessageBox.Show("Please select academic session", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbSess.Focus()
            Return
        End If
        If cmbStaffName.Text = "" Then
            MessageBox.Show("Please select lecturer-in-charge", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbStaffName.Focus()
            Return
        End If
        If Val(lblTotalClasses.Text) < 1 Then
            MessageBox.Show("Error in score entered", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            'txtCA.Focus()
            Return
        End If

        Try

            For Each row As DataGridViewRow In dgw.Rows
                con = New SqlConnection(cs)
                con.Open()
                Dim ct As String = "Select Sess, CCode, Att_Type, MatricNo from AttendancePrint where Sess=@d1 and CCode=@d2 and Att_Type=@d3 and MatricNo=@d4"
                'Dim ctX As String = "Select Sess, Sem, Lev, Dept, MatricNo from Temp_Result where Sess=@d1 and Lev=@d2 and Sem=@d3 and MatricNo=@d5"
                cmd = New SqlCommand(ct)
                cmd.Connection = con
                cmd.Parameters.AddWithValue("@d1", cmbSess.Text)
                cmd.Parameters.AddWithValue("@d2", cmbCCode.Text)
                cmd.Parameters.AddWithValue("@d3", cmbAttendanceType.Text)
                cmd.Parameters.AddWithValue("@d4", row.Cells(0).Value)
                rdr = cmd.ExecuteReader()
                If rdr.Read() Then
                    '   nothhing here
                    con = New SqlConnection(cs)
                    con.Open()

                    Dim cb As String = "Update AttendancePrint set Total_Att=@d13, Att_Per=@d14, Tot_Class=@d15 where Sess=@d1 and CCode=@d2 and Att_Type=@d3 and MatricNo=@d4"
                    cmd = New SqlCommand(cb)
                    cmd.Connection = con
                    cmd.Parameters.AddWithValue("@d1", cmbSess.Text)
                    cmd.Parameters.AddWithValue("@d2", cmbCCode.Text)
                    cmd.Parameters.AddWithValue("@d3", cmbAttendanceType.Text)
                    cmd.Parameters.AddWithValue("@d4", row.Cells(0).Value)
                    cmd.Parameters.AddWithValue("@d13", row.Cells(2).Value)
                    cmd.Parameters.AddWithValue("@d14", row.Cells(3).Value)
                    cmd.Parameters.AddWithValue("@d15", lblTotalClasses.Text)
                    cmd.ExecuteReader()
                    con.Close()

                Else
                    'code to insert AttendancePrint
                    'that is code to copy each record in the gridview to AttendancePrint
                    con = New SqlConnection(cs)
                    con.Open()
                    Dim cb1 As String = "insert into AttendancePrint(Sess, Dept, CCode, SchoolName, Tot_Class, Att_Type, LectName, MatricNo, StudName, Total_Att, Att_Per) VALUES ('" & cmbSess.Text & "', '" & cmbDept.Text & "', '" & cmbCCode.Text & "','" & cmbSch.Text & "', '" & lblTotalClasses.Text & "', '" & cmbAttendanceType.Text & "', '" & cmbStaffName.Text & "',@d1,@d2,@d3,@d4)"
                    cmd = New SqlCommand(cb1)
                    cmd.Connection = con
                    cmd.Parameters.AddWithValue("@d1", row.Cells(0).Value)
                    cmd.Parameters.AddWithValue("@d2", row.Cells(1).Value)
                    cmd.Parameters.AddWithValue("@d3", row.Cells(2).Value)
                    cmd.Parameters.AddWithValue("@d4", row.Cells(3).Value)
                    cmd.ExecuteReader()
                    con.Close()
                End If
            Next

            MessageBox.Show("Successfully Saved", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information)
            'btnSave.Enabled = False
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
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

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim rowsTotal, colsTotal As Short
        Dim I, j, iC As Short
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        Dim xlApp As New Excel.Application
        Try
            Dim excelBook As Excel.Workbook = xlApp.Workbooks.Add
            Dim excelWorksheet As Excel.Worksheet = CType(excelBook.Worksheets(1), Excel.Worksheet)
            xlApp.Visible = True

            rowsTotal = dgw.RowCount
            colsTotal = dgw.Columns.Count - 1
            With excelWorksheet
                .Cells.Select()
                .Cells.Delete()
                For iC = 0 To colsTotal
                    .Cells(1, iC + 1).Value = dgw.Columns(iC).HeaderText
                Next
                For I = 0 To rowsTotal - 1
                    For j = 0 To colsTotal
                        .Cells(I + 2, j + 1).value = dgw.Rows(I).Cells(j).Value
                    Next j
                Next I
                .Rows("1:1").Font.FontStyle = "Bold"
                .Rows("1:1").Font.Size = 12

                .Cells.Columns.AutoFit()
                .Cells.Select()
                .Cells.EntireColumn.AutoFit()
                .Cells(1, 1).Select()
            End With
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            'RELEASE ALLOACTED RESOURCES
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
            xlApp = Nothing
        End Try
    End Sub
End Class