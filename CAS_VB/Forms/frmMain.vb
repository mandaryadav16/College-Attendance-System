Public Class frmMain
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        lblDateTime.Text = Now.ToString("dd/MM/yyyy hh:mm:ss tt")
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Cursor = Cursors.Default
        Timer2.Enabled = False
    End Sub

    Private Sub NotepadToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NotepadToolStripMenuItem.Click
        Try
            System.Diagnostics.Process.Start("Notepad.exe")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub CalculatorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CalculatorToolStripMenuItem.Click
        Try
            System.Diagnostics.Process.Start("Calc.exe")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub MSWordToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MSWordToolStripMenuItem.Click
        Try
            System.Diagnostics.Process.Start("WinWord.exe")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub WordToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WordToolStripMenuItem.Click
        Try
            System.Diagnostics.Process.Start("Wordpad.exe")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub TaskManagerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TaskManagerToolStripMenuItem.Click
        Try
            System.Diagnostics.Process.Start("TaskMgr.exe")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LogoutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LogoutToolStripMenuItem.Click
        Try
            If MessageBox.Show("Do you really want to logout from application?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                Me.Hide()
                frmLogin.Show()
                frmLogin.txtUser.Text = ""
                frmLogin.txtPass.Text = ""
                frmLogin.txtUser.Focus()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.Click

    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        frmAbout.ShowDialog()
    End Sub

    Private Sub SchoolRegistrationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SchoolRegistrationToolStripMenuItem.Click
        frmSchool.Reset()
        frmSchool.ShowDialog()
    End Sub

    Private Sub DepartmentEntryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DepartmentEntryToolStripMenuItem.Click
        frmDepartment.Reset()
        frmDepartment.ShowDialog()
    End Sub

    Private Sub CourseRegistrationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CourseRegistrationToolStripMenuItem.Click
        frmCourse.Reset()
        frmCourse.ShowDialog()
    End Sub

    Private Sub LecturerEntryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LecturerEntryToolStripMenuItem.Click
        frmLecturer.Reset()
        frmLecturer.ShowDialog()
    End Sub

    Private Sub StudentEntryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StudentEntryToolStripMenuItem.Click
        frmStudent.Reset()
        frmStudent.ShowDialog()
    End Sub

    Private Sub StudentsToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles StudentsToolStripMenuItem1.Click
        'frmCourseRecord.Reset()
        frmStudentImport.ShowDialog()
    End Sub

    Private Sub UserRegistrationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UserRegistrationToolStripMenuItem.Click
        frmNewUser.Getdata()
        frmNewUser.ShowDialog()
    End Sub

    Private Sub ChangePasswordToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChangePasswordToolStripMenuItem.Click
        frmChangePass.ShowDialog()
    End Sub

    Private Sub StudentsToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles StudentsToolStripMenuItem2.Click
        frmStudentRecord.Reset()
        frmStudentRecord.ShowDialog()
    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub frmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        'code to disable form close or exit
        e.Cancel = True
    End Sub

    Private Sub CourseStudyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CourseStudyToolStripMenuItem.Click
        frmCourseStudy.Reset()
        frmCourseStudy.ShowDialog()
    End Sub

    Private Sub ResultEntryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ResultEntryToolStripMenuItem.Click
        frmAttendanceType.Reset()
        frmAttendanceType.ShowDialog()
    End Sub

    Private Sub ResultsEntryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ResultsEntryToolStripMenuItem.Click
        frmStudentAttendance.Reset()
        frmStudentAttendance.ShowDialog()
    End Sub

    Private Sub ResultsToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ResultsToolStripMenuItem1.Click
        'frmStudentsAttendanceRecord.Reset()
        frmStudentsAttendanceRecord.ShowDialog()
    End Sub

    Private Sub TransferStudentsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TransferStudentsToolStripMenuItem.Click
        frmStudLevTransfer.Reset()
        frmStudLevTransfer.ShowDialog()
    End Sub

    Private Sub CourseToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles CourseToolStripMenuItem1.Click
        frmPrintCourse.Reset()
        frmPrintCourse.ShowDialog()
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub

    Private Sub ToolStripStatusLabel3_Click(sender As Object, e As EventArgs) Handles ToolStripStatusLabel3.Click

    End Sub

    Private Sub lblUserType_Click(sender As Object, e As EventArgs) Handles lblUserType.Click

    End Sub
End Class