Imports System.Data.SqlClient

Public Class frmPrintCourse
    Sub Reset()
        cmbDept.SelectedIndex = -1
        'cmbDept.Text = ""
        'cmbSch.Text = ""
        cmbSch.SelectedIndex = -1
        'cmbSch.Text = ""
        lblLev.Text = ""
        cmbLev.SelectedIndex = -1
        cmbCStudy.SelectedIndex = -1
        lblSem.Text = ""
        cmbSess.SelectedIndex = -1
        cmbSem.SelectedIndex = -1
        cmbCCode.SelectedIndex = -1
        cmbAttendanceType.SelectedIndex = -1
        ComboBox1.SelectedIndex = -1
        'cmbLect.Text = ""
        'btnProceed.Enabled = True
        'btnDelete.Enabled = False
        'btnUpdate.Enabled = False
        cmbSess.Focus()
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
            'Getdata()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub frmPrintCourse_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AcceptButton = btnProceed
        fillSchool()
        fillAttendanceType()
    End Sub

    Private Sub cmbDept_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbDept.SelectedIndexChanged
        cmbCCode.Text = ""
        cmbCCode.Items.Clear()
        'lblSem.Text = ""
        'lblLev.Text = ""

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
            'Getdata()
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
                lblCTitle.Text = rdr.GetValue(0)
                lblLev.Text = rdr.GetValue(1)
            End If
            If (rdr IsNot Nothing) Then
                rdr.Close()
            End If
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            'Getdata()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnProceed_Click(sender As Object, e As EventArgs) Handles btnProceed.Click
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
        If lblLev.Text = "" Then
            MessageBox.Show("Please select course code", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbCCode.Focus()
            Return
        End If
        If cmbSess.Text = "" Then
            MessageBox.Show("Please select academic session", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbSess.Focus()
            Return
        End If
        If cmbCCode.Text = "" Then
            MessageBox.Show("Please select course code", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbCCode.Focus()
            Return
        End If

        Try
            con = New SqlConnection(cs)
            con.Open()
            'LATEST'
            'cmd = New SqlCommand("SELECT Merge1.MatricNo,Merge1.StudName,Merge1.Dept,Merge1.CComb, Merge2.Lev,Merge2.Sem,Merge2.CCode,Merge2.CTitle,Merge2.CUnit, Merge1.Sess,Merge2.Total,Merge2.Grade, Merge1.TU,Merge1.TCP,Merge1.TUP, Merge1.CGP,Merge1.CGPA FROM Merge1 INNER JOIN Merge2 ON Merge1.MatricNo = Merge2.MatricNo  Where Merge1.Sess like '" & cmbSess.Text & "%' and Merge1.Lev like '" & cmbLev.Text & "%' and Merge1.CComb like '" & cmbCStudy.Text & "%' and Merge1.Sem like '" & cmbSem.Text & "%' Order by Merge1.MatricNo ", con)
            'cmd = New SqlCommand("SELECT Merge1.MatricNo,Merge1.StudName,Merge1.Dept,Merge1.CComb, Merge2.Lev,Merge2.Sem,Merge2.CCode,Merge2.CTitle,Merge2.CUnit, Merge1.Sess,Merge2.Total,Merge2.Grade, Merge1.TU,Merge1.TCP,Merge1.TUP, Merge1.CGP,Merge1.CGPA FROM Merge1 INNER JOIN Merge2 ON Merge1.MatricNo = Merge2.MatricNo  Where Merge1.Sess=@d1 and Merge1.Lev=@d2 and Merge1.CComb=@d3 and Merge1.Sem=@d4 and Merge1.MatricNo=@d5 Order by Merge1.MatricNo ", con)
            'cmd = New SqlCommand("SELECT Students.MatricNo,Students.StudName,Students.SchoolName,Students.Dept,Students.CComb, Result.Lev,Result.Sem,Result.CCode,Result.CTitle,Result.CUnit, Result.Sess,Result.Q1,Result.Q2,Result.Q3,Result.Q4,Result.Q5,Result.Q6,Result.Exam,Result.CA,Result.Total,Result.Grade FROM Students INNER JOIN Result ON Students.MatricNo = Result.MatricNo  Where Result.Sess=@d1 and Students.Lev=@d2 and Students.CComb=@d5 and Result.Sem=@d4 and Result.CCode=@d3 Order by Students.MatricNo ", con)
            cmd = New SqlCommand("SELECT AttendancePrint.MatricNo,AttendancePrint.StudName,Courses.SchoolName,Courses.Dept,Courses.Lev,Courses.Sem,Courses.CCode,Courses.CTitle,AttendancePrint.Total_att,AttendancePrint.Sess,AttendancePrint.Att_Per,AttendancePrint.Tot_Class,AttendancePrint.Att_Type,AttendancePrint.LectName FROM Courses INNER JOIN AttendancePrint ON Courses.CCode = AttendancePrint.CCode  Where AttendancePrint.Sess=@d1 and Courses.Lev=@d2 and Courses.CCode=@d3 and Courses.Dept=@d4 and AttendancePrint.Att_Type=@d5 and AttendancePrint.Att_Per>=@d6 Order by AttendancePrint.MatricNo", con)
            cmd.Parameters.AddWithValue("@d1", cmbSess.Text)
            cmd.Parameters.AddWithValue("@d2", lblLev.Text)
            cmd.Parameters.AddWithValue("@d3", cmbCCode.Text)
            cmd.Parameters.AddWithValue("@d4", cmbDept.Text)
            cmd.Parameters.AddWithValue("@d5", cmbAttendanceType.Text)
            cmd.Parameters.AddWithValue("@d6", Val(ComboBox1.Text))
            adp = New SqlDataAdapter(cmd)
            dtable = New DataTable()
            adp.Fill(dtable)
            con.Close()
            ds = New DataSet()
            ds.Tables.Add(dtable)
            ds.WriteXmlSchema("AttendanceSheet.xml")
            Dim rpt As New rptLectureNew
            rpt.SetDataSource(ds)
            frmReport.CrystalReportViewer1.ReportSource = rpt
            frmReport.ShowDialog()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Reset()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class