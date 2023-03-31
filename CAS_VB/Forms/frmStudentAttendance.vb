Imports System.Data.SqlClient

Public Class frmStudentAttendance
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
        listView1.Items.Clear()
        btnUpdate.Enabled = False
        btnSave.Enabled = False
        btnDelete.Enabled = False
        listView1.Items.Clear()
        dtpDate.Value = Today
        cmbAttendanceType.SelectedIndex = -1
        cmbStaffName.Text = ""
        txtMatNo.Text = ""
        'txtSt_ID.Text = ""
        '  auto()

    End Sub
    Sub Getdata()
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("SELECT RTRIM(MatricNo), RTRIM(StudName) from Students Where CComb='" & cmbCStudy.Text & "' AND Lev='" & lblLev.Text & "' AND Sess='" & cmbSess.Text & "' order by MatricNo", con)
            'cmd = New SqlCommand("Select distinct RTRIM(Student.AdmissionNo),RTRIM(Student.StudentName) from Student,Class,Section,SchoolInfo where Student.SectionID=Section.ID and Class.Classname=Section.Class and SchoolInfo.S_ID=Student.SchoolID and Session=@d1 and Classname=@d2 and SchoolName=@d3 order by 2", con)
            'cmd.Parameters.AddWithValue("@d1", cmbSession.Text)
            'cmd.Parameters.AddWithValue("@d2", cmbClass.Text)
            'cmd.Parameters.AddWithValue("@d3", cmbSchool.Text)
            rdr = cmd.ExecuteReader()
            While rdr.Read()
                Dim item = New ListViewItem()
                item.Text = rdr(0).ToString().Trim()
                item.SubItems.Add(rdr(1).ToString().Trim())
                listView1.Items.Add(item)
            End While
            con.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub auto()
        Try
            con = New SqlConnection(cs)
            con.Open()
            Dim ct As String = "select CCode, AttendanceType, Sess, Date from AttendanceMaster where CCode=@d1 and AttendanceType=@d2 and Sess=@d3 and Date=@d4"
            cmd = New SqlCommand(ct)
            cmd.Parameters.AddWithValue("@d1", cmbCCode.Text)
            cmd.Parameters.AddWithValue("@d2", cmbAttendanceType.Text)
            cmd.Parameters.AddWithValue("@d3", cmbSess.Text)
            cmd.Parameters.AddWithValue("@d4", dtpDate.Value.Date)
            cmd.Connection = con
            rdr = cmd.ExecuteReader()
            If rdr.Read() Then
                'CODE TO FETCH DATA (ID) FROM THE DATABASE
                con = New SqlConnection(cs)
                con.Open()
                cmd = con.CreateCommand()
                cmd.CommandText = "SELECT ID FROM AttendanceMaster WHERE CCode=@d1 and AttendanceType=@d2 and Sess=@d3 and Date=@d4"
                cmd.Parameters.AddWithValue("@d1", cmbCCode.Text)
                cmd.Parameters.AddWithValue("@d2", cmbAttendanceType.Text)
                cmd.Parameters.AddWithValue("@d3", cmbSess.Text)
                cmd.Parameters.AddWithValue("@d4", dtpDate.Value.Date)
                rdr = cmd.ExecuteReader()
                If rdr.Read() Then
                    txtID.Text = rdr.GetValue(0)
                End If
                If (rdr IsNot Nothing) Then
                    rdr.Close()
                End If
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If

                '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                'con = New SqlConnection(cs)
                'con.Open()
                'Dim ct1 As String = "select CCode, MatricNo, AttendanceID, Sess, Date from Attendance where CCode=@d1 and MatricNo=@d2 and AttendanceID=@d3 and Sess=@d4 and Date=@d5"
                'cmd = New SqlCommand(ct1)
                'cmd.Parameters.AddWithValue("@d1", cmbCCode.Text)
                'cmd.Parameters.AddWithValue("@d3", txtID.Text)
                'cmd.Parameters.AddWithValue("@d4", cmbSess.Text)
                'cmd.Parameters.AddWithValue("@d5", dtpDate.Value)
                'cmd.Connection = con
                'rdr = cmd.ExecuteReader()
                '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            Else
                'CODE TO AUTO ADD 1 TO THE ID
                Dim Num As Integer = 0
                con = New SqlConnection(cs)
                con.Open()
                Dim sql As String = ("SELECT MAX(ID) FROM AttendanceMaster")
                cmd = New SqlCommand(sql)
                cmd.Connection = con
                If (IsDBNull(cmd.ExecuteScalar)) Then
                    Num = 1
                    txtID.Text = Num.ToString
                Else
                    Num = cmd.ExecuteScalar + 1
                    txtID.Text = Num.ToString
                End If
                cmd.Dispose()
                con.Close()
                con.Dispose()
            End If
            btnSave.Enabled = True
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
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
            listView1.Items.Clear()
            Getdata()

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
            listView1.Items.Clear()
            Getdata()

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
            listView1.Items.Clear()
            Getdata()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub frmStudentAttendance_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        fillSchool()
        fillAttendanceType()
        fillStaffName()
        btnSave.Enabled = False
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
            listView1.Items.Clear()
            Getdata()
            auto()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmbDept_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbDept.SelectedIndexChanged
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
            listView1.Items.Clear()
            Getdata()
            auto()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmbCStudy_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCStudy.SelectedIndexChanged
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
            listView1.Items.Clear()
            Getdata()
            auto()
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
            listView1.Items.Clear()
            Getdata()
            auto()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            If Len(Trim(cmbSch.Text)) = 0 Then
                MessageBox.Show("Please select school name", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cmbSch.Focus()
                Exit Sub
            End If
            If Len(Trim(cmbSess.Text)) = 0 Then
                MessageBox.Show("Please select session", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cmbSess.Focus()
                Exit Sub
            End If
            If Len(Trim(cmbDept.Text)) = 0 Then
                MessageBox.Show("Please select department", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cmbDept.Focus()
                Exit Sub
            End If
            If Len(Trim(cmbCStudy.Text)) = 0 Then
                MessageBox.Show("Please select course study", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cmbCStudy.Focus()
                Exit Sub
            End If
            If Len(Trim(cmbStaffName.Text)) = 0 Then
                MessageBox.Show("Please select lecturer name", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cmbStaffName.Focus()
                Exit Sub
            End If
            If Len(Trim(cmbCCode.Text)) = 0 Then
                MessageBox.Show("Please select course code", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cmbCCode.Focus()
                Exit Sub
            End If
            ' If Len(Trim(txtStaffID.Text)) = 0 Then
            'MessageBox.Show("Please retrieve staff id", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            'txtStaffID.Focus()
            'Exit Sub
            'End If
            If Len(Trim(cmbAttendanceType.Text)) = 0 Then
                MessageBox.Show("Please select attendance type", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cmbAttendanceType.Focus()
                Exit Sub
            End If

            If listView1.Items.Count = 0 Then
                MessageBox.Show("Sorry nothing to save.." & vbCrLf & "Please retrieve data in listview", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If


            con = New SqlConnection(cs)
            con.Open()
            Dim ct As String = "select ID, AttendanceType, Sess, CCode, Date from AttendanceMaster where ID=@d1"
            cmd = New SqlCommand(ct)
            cmd.Parameters.AddWithValue("@d1", txtID.Text)
            ' Dim ct As String = "select CCode, AttendanceType, Sess, Date from Attendance where CCode=@d1 and AttendanceType=@d2 and Sess=@d3 and Date=@d4"
            '  cmd = New SqlCommand(ct)
            ' cmd.Parameters.AddWithValue("@d1", cmbCCode.Text)
            ' cmd.Parameters.AddWithValue("@d2", cmbAttendanceType.Text)
            '  cmd.Parameters.AddWithValue("@d3", cmbSess.Text)
            '  cmd.Parameters.AddWithValue("@d4", dtpDate.Value.Date)
            cmd.Connection = con
            rdr = cmd.ExecuteReader()
            If rdr.Read() Then
                For i As Integer = listView1.Items.Count - 1 To 0 Step -1
                    con = New SqlConnection(cs)
                    con.Open()
                    'Dim ct1 As String = "select CCode, MatricNo, AttendanceID from Attendance where CCode=@d1 and MatricNo=@d2 and AttendanceID=@d3"
                    Dim ct1 As String = "select CCode, MatricNo, AttendanceID, Sess, Date from Attendance where CCode=@d1 and MatricNo=@d2 and AttendanceID=@d3 and Sess=@d4 and Date=@d5"
                    cmd = New SqlCommand(ct1)
                    cmd.Parameters.AddWithValue("@d1", cmbCCode.Text)
                    cmd.Parameters.AddWithValue("@d2", listView1.Items(i).SubItems(0).Text)
                    cmd.Parameters.AddWithValue("@d3", txtID.Text)
                    cmd.Parameters.AddWithValue("@d4", cmbSess.Text)
                    cmd.Parameters.AddWithValue("@d5", dtpDate.Value.Date)
                    cmd.Connection = con
                    rdr = cmd.ExecuteReader()

                    If rdr.Read() Then
                        con = New SqlConnection(cs)
                        con.Open()
                        Dim cq As String = "delete from Attendance where CCode=@d1 and MatricNo=@d2 and AttendanceID=@d3 and Sess=@d4 and Date=@d5"
                        cmd = New SqlCommand(cq)
                        cmd.Parameters.AddWithValue("@d1", cmbCCode.Text)
                        cmd.Parameters.AddWithValue("@d2", listView1.Items(i).SubItems(0).Text)
                        cmd.Parameters.AddWithValue("@d3", txtID.Text)
                        cmd.Parameters.AddWithValue("@d4", cmbSess.Text)
                        cmd.Parameters.AddWithValue("@d5", dtpDate.Value.Date)
                        cmd.Connection = con
                        cmd.ExecuteNonQuery()
                        'For i As Integer = listView1.Items.Count - 1 To 0 Step -1

                        If listView1.Items(i).Checked = True Then
                            Status = "P"
                        Else
                            Status = "A"
                        End If
                        con = New SqlConnection(cs)
                        Dim cd1 As String = "Insert Into Attendance(MatricNo,LecturerName,Date,CCode,Status,AttendanceID,Sess) Values(@d1,@d2,@d3,@d4,@d5,@d6,@d7)"
                        cmd = New SqlCommand(cd1)
                        cmd.Parameters.AddWithValue("@d1", listView1.Items(i).SubItems(0).Text)
                        cmd.Parameters.AddWithValue("@d2", cmbStaffName.Text)
                        cmd.Parameters.AddWithValue("@d3", dtpDate.Value.Date)
                        cmd.Parameters.AddWithValue("@d4", cmbCCode.Text)
                        cmd.Parameters.AddWithValue("@d5", Status)
                        cmd.Parameters.AddWithValue("@d6", txtID.Text)
                        cmd.Parameters.AddWithValue("@d7", cmbSess.Text)
                        cmd.Connection = con
                        con.Open()
                        cmd.ExecuteNonQuery()
                        con.Close()
                        'Next

                    Else
                        '    For i As Integer = listView1.Items.Count - 1 To 0 Step -1

                        If listView1.Items(i).Checked = True Then
                            Status = "P"
                        Else
                            Status = "A"
                        End If
                        con = New SqlConnection(cs)
                        Dim cd1 As String = "Insert Into Attendance(MatricNo,LecturerName,Date,CCode,Status,AttendanceID,Sess) Values(@d1,@d2,@d3,@d4,@d5,@d6,@d7)"
                        cmd = New SqlCommand(cd1)
                        cmd.Parameters.AddWithValue("@d1", listView1.Items(i).SubItems(0).Text)
                        cmd.Parameters.AddWithValue("@d2", cmbStaffName.Text)
                        cmd.Parameters.AddWithValue("@d3", dtpDate.Value.Date)
                        cmd.Parameters.AddWithValue("@d4", cmbCCode.Text)
                        cmd.Parameters.AddWithValue("@d5", Status)
                        cmd.Parameters.AddWithValue("@d6", txtID.Text)
                        cmd.Parameters.AddWithValue("@d7", cmbSess.Text)
                        cmd.Connection = con
                        con.Open()
                        cmd.ExecuteNonQuery()
                        con.Close()
                        ' Next
                    End If
                Next



            Else
                con = New SqlConnection(cs)
                Dim cd As String = "Insert Into AttendanceMaster(ID,AttendanceType,Sess,Date,CCode) Values(@d1,@d2,@d3,@d4,@d5)"
                cmd = New SqlCommand(cd)
                cmd.Parameters.AddWithValue("@d1", txtID.Text)
                cmd.Parameters.AddWithValue("@d2", cmbAttendanceType.Text)
                cmd.Parameters.AddWithValue("@d4", dtpDate.Value.Date)
                cmd.Parameters.AddWithValue("@d3", cmbSess.Text)
                cmd.Parameters.AddWithValue("@d5", cmbCCode.Text)
                cmd.Connection = con
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
                For i As Integer = listView1.Items.Count - 1 To 0 Step -1

                    If listView1.Items(i).Checked = True Then
                        Status = "P"
                    Else
                        Status = "A"
                    End If
                    con = New SqlConnection(cs)
                    Dim cd1 As String = "Insert Into Attendance(MatricNo,LecturerName,Date,CCode,Status,AttendanceID,Sess) Values(@d1,@d2,@d3,@d4,@d5,@d6,@d7)"
                    cmd = New SqlCommand(cd1)
                    cmd.Parameters.AddWithValue("@d1", listView1.Items(i).SubItems(0).Text)
                    cmd.Parameters.AddWithValue("@d2", cmbStaffName.Text)
                    cmd.Parameters.AddWithValue("@d3", dtpDate.Value.Date)
                    cmd.Parameters.AddWithValue("@d4", cmbCCode.Text)
                    cmd.Parameters.AddWithValue("@d5", Status)
                    cmd.Parameters.AddWithValue("@d6", txtID.Text)
                    cmd.Parameters.AddWithValue("@d7", cmbSess.Text)
                    cmd.Connection = con
                    con.Open()
                    cmd.ExecuteNonQuery()
                    con.Close()
                Next

            End If

            ' LogFunc(lblUser.Text, "added the new attendance record having attendance id '" & txtID.Text & "'")
            'auto()
            MessageBox.Show("Successfully saved", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information)
            btnSave.Enabled = False
            btnUpdate.Enabled = True
            btnDelete.Enabled = True
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        'Reset()
        If Len(Trim(cmbSess.Text)) = 0 Then
            MessageBox.Show("Please select session", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbSess.Focus()
            Exit Sub
        End If
        If Len(Trim(dtpDate.Text)) = 0 Then
            MessageBox.Show("Please select date", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            dtpDate.Focus()
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
        auto()
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Try
            If Len(Trim(cmbSch.Text)) = 0 Then
                MessageBox.Show("Please select school name", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cmbSch.Focus()
                Exit Sub
            End If
            If Len(Trim(cmbSess.Text)) = 0 Then
                MessageBox.Show("Please select session", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cmbSess.Focus()
                Exit Sub
            End If
            If Len(Trim(cmbDept.Text)) = 0 Then
                MessageBox.Show("Please select department", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cmbDept.Focus()
                Exit Sub
            End If
            If Len(Trim(cmbCStudy.Text)) = 0 Then
                MessageBox.Show("Please select course study", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cmbCStudy.Focus()
                Exit Sub
            End If
            If Len(Trim(cmbStaffName.Text)) = 0 Then
                MessageBox.Show("Please select lecturer name", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cmbStaffName.Focus()
                Exit Sub
            End If
            If Len(Trim(cmbCCode.Text)) = 0 Then
                MessageBox.Show("Please select course code", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cmbCCode.Focus()
                Exit Sub
            End If
            ' If Len(Trim(txtStaffID.Text)) = 0 Then
            'MessageBox.Show("Please retrieve staff id", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            'txtStaffID.Focus()
            'Exit Sub
            'End If
            If Len(Trim(cmbAttendanceType.Text)) = 0 Then
                MessageBox.Show("Please select attendance type", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cmbAttendanceType.Focus()
                Exit Sub
            End If

            If listView1.Items.Count = 0 Then
                MessageBox.Show("Sorry nothing to save.." & vbCrLf & "Please retrieve data in listview", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            con = New SqlConnection(cs)
            Dim cd As String = "Update AttendanceMaster set AttendanceType=@d2 where ID=@d1"
            cmd = New SqlCommand(cd)
            cmd.Parameters.AddWithValue("@d1", txtID.Text)
            cmd.Parameters.AddWithValue("@d2", cmbAttendanceType.Text)
            cmd.Connection = con
            con.Open()
            cmd.ExecuteNonQuery()
            con.Close()
            con = New SqlConnection(cs)
            con.Open()
            Dim cq As String = "delete from Attendance where AttendanceID=@d1"
            cmd = New SqlCommand(cq)
            cmd.Parameters.AddWithValue("@d1", txtID.Text)
            cmd.Connection = con
            cmd.ExecuteNonQuery()
            For i As Integer = listView1.Items.Count - 1 To 0 Step -1

                If listView1.Items(i).Checked = True Then
                    Status = "P"
                Else
                    Status = "A"
                End If
                con = New SqlConnection(cs)
                Dim cd1 As String = "Insert Into Attendance(MatricNo,LecturerName,Date,CCode,Status,AttendanceID) Values(@d1,@d2,@d3,@d4,@d5,@d6,@d7)"
                cmd = New SqlCommand(cd1)
                cmd.Parameters.AddWithValue("@d1", listView1.Items(i).SubItems(0).Text)
                cmd.Parameters.AddWithValue("@d2", cmbStaffName.Text)
                cmd.Parameters.AddWithValue("@d3", dtpDate.Value.Date)
                cmd.Parameters.AddWithValue("@d4", cmbCCode.Text)
                cmd.Parameters.AddWithValue("@d5", Status)
                cmd.Parameters.AddWithValue("@d6", txtID.Text)
                cmd.Parameters.AddWithValue("@d7", cmbSess.Text)
                cmd.Connection = con
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            Next
            'LogFunc(lblUser.Text, "updated the attendance record having attendance id '" & txtID.Text & "'")
            MessageBox.Show("Successfully updated", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information)
            btnUpdate.Enabled = False
            'auto()
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
    Public Sub DeleteRecord()
        Try
            Dim RowsAffected As Integer = 0
            con = New SqlConnection(cs)
            con.Open()
            Dim cq As String = "delete from AttendanceMaster where ID=@d1"
            cmd = New SqlCommand(cq)
            cmd.Parameters.AddWithValue("@d1", txtID.Text)
            cmd.Connection = con
            RowsAffected = cmd.ExecuteNonQuery()
            If RowsAffected > 0 Then
                'LogFunc(lblUser.Text, "deleted the attendance record having attendance id '" & txtID.Text & "'")
                MessageBox.Show("Successfully deleted", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information)
                'Reset()
                auto()
                listView1.Items.Clear()
                btnUpdate.Enabled = False
                btnUpdate.Enabled = False
                btnSave.Enabled = True
            Else
                MessageBox.Show("No record found", "Sorry", MessageBoxButtons.OK, MessageBoxIcon.Information)
                'Reset()
                auto()
                listView1.Items.Clear()
                btnUpdate.Enabled = False
                btnUpdate.Enabled = False
                btnSave.Enabled = True
            End If
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmbSess_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSess.SelectedIndexChanged
        listView1.Items.Clear()
        Getdata()
        auto()
    End Sub

    Private Sub cmbAttendanceType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbAttendanceType.SelectedIndexChanged
        'If listView1.Items.Count = 0 Then
        listView1.Items.Clear()
        Getdata()
        auto()
        '    Exit Sub
        'End If
    End Sub

    Private Sub txtMatNo_TextChanged(sender As Object, e As EventArgs) Handles txtMatNo.TextChanged
        Try
            'auto()
            btnUpdate.Enabled = False
            btnDelete.Enabled = False
            btnSave.Enabled = True
            con = New SqlConnection(cs)
            con.Open()
            'cmd = New SqlCommand("SELECT RTRIM(MatricNo), RTRIM(StudName) from Students Where CComb='" & cmbCStudy.Text & "' AND Lev='" & lblLev.Text & "' AND Sess='" & cmbSess.Text & "' order by MatricNo", con)
            cmd = New SqlCommand("SELECT RTRIM(MatricNo), RTRIM(StudName) from Students Where CComb='" & cmbCStudy.Text & "' AND Lev='" & lblLev.Text & "' AND Sess='" & cmbSess.Text & "' AND MatricNo like '%" + txtMatNo.Text + "%' order by MatricNo", con)
            rdr = cmd.ExecuteReader()
            listView1.Items.Clear()
            While rdr.Read()
                Dim item = New ListViewItem()
                item.Text = rdr(0).ToString().Trim()
                item.SubItems.Add(rdr(1).ToString().Trim())
                listView1.Items.Add(item)
            End While
            con.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dtpDate_ValueChanged(sender As Object, e As EventArgs) Handles dtpDate.ValueChanged
        auto()
        listView1.Items.Clear()
        Getdata()
        auto()
        ' btnSave.Enabled = True
        btnDelete.Enabled = False
        btnUpdate.Enabled = False
    End Sub

    Private Sub BtnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        Reset()
    End Sub

    Private Sub CmbStaffName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbStaffName.SelectedIndexChanged
        auto()
    End Sub
End Class