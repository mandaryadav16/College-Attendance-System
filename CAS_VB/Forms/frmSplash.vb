Public Class frmSplash
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        ProgressBar1.Visible = True
        ProgressBar1.Value = ProgressBar1.Value + 2
        If (ProgressBar1.Value = 10) Then
            Label3.Text = "Reading modules.."
        ElseIf (ProgressBar1.Value = 20) Then
            Label3.Text = "Turning on modules."
        ElseIf (ProgressBar1.Value = 40) Then
            Label3.Text = "Starting modules.."
        ElseIf (ProgressBar1.Value = 60) Then
            Label3.Text = "Loading modules.."
        ElseIf (ProgressBar1.Value = 80) Then
            Label3.Text = "Done Loading modules.."
        ElseIf (ProgressBar1.Value = 100) Then
            frmLogin.Show()
            Timer1.Enabled = False
            Me.Hide()
        End If
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub
End Class
