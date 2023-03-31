Imports System.Data.SqlClient
Public Class frmLogin
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        End

    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        If txtUser.Text = "" Then
            MessageBox.Show("Enter Username", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtUser.Focus()
            Exit Sub
        End If
        If txtPass.Text = "" Then
            MessageBox.Show("Enter Password", "CAS", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtPass.Focus()
            Exit Sub
        End If

        Try

            con = New SqlConnection(cs)
            con.Open()
            cmd = con.CreateCommand()
            cmd.CommandText = "SELECT Username,Password FROM Users where Username = @d1 and Password=@d2"
            cmd.Parameters.AddWithValue("@d1", txtUser.Text)
            cmd.Parameters.AddWithValue("@d2", (txtPass.Text))
            rdr = cmd.ExecuteReader()
            If rdr.Read() Then
                con = New SqlConnection(cs)
                con.Open()
                cmd = con.CreateCommand()
                cmd.CommandText = "SELECT UserType FROM Users where Username=@d3 and Password=@d4"
                cmd.Parameters.AddWithValue("@d3", txtUser.Text)
                cmd.Parameters.AddWithValue("@d4", txtPass.Text)
                rdr = cmd.ExecuteReader()

                'con.Close()
                ' Dim st As String = "Successfully logged in"

                '++++++++ CODE TO MAKE MAIN MENU SHOW USER TYPE, LIKE ADMIN, HARDYEM ETC

                If rdr.Read() Then
                    cmbUserType.Text = rdr.GetValue(0).ToString.Trim
                End If
                If (rdr IsNot Nothing) Then
                    rdr.Close()
                End If
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If

                frmMain.lblUser.Text = txtUser.Text
                frmMain.lblUserType.Text = cmbUserType.Text

                MsgBox("Successfully logged in !", MsgBoxStyle.Information, "ACCESS GRANTED")
                con.Close()
                Me.Hide()
                frmMain.Show()


                '                     OR

                'I CAN JUST MAKE A COMBO BOX THAT HAS (admin,voters)
                ' then transfer the content with FRMMAIN.LBLUSER.TEXT=COMBOUSER.TEXT 
                'WITHOUT EVEN USING ANY USERTYPE FIELD IN THE DATABASE TABLE OF LOGIN
                '+++++++++++++ STOPPED HERE ++++++++++++++++++++++++

                'frmVoters.Show()

            Else
                MsgBox("Login is Failed...Try again !", MsgBoxStyle.Critical, "ACCESS DENIED")
                txtUser.Text = ""
                txtPass.Text = ""
                txtUser.Focus()
            End If
            cmd.Dispose()
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Me.Hide()
        frmChangePass.Show()
        frmChangePass.UserID.Text = ""
        frmChangePass.OldPassword.Text = ""
        frmChangePass.NewPassword.Text = ""
        frmChangePass.ConfirmPassword.Text = ""
        frmChangePass.UserID.Focus()
    End Sub

    Private Sub frmLogin_Load(sender As Object, e As EventArgs) Handles Me.Load
        AcceptButton = btnLogin
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        frmSqlServerSetting.lblSet.Text = "Login Form"
        frmSqlServerSetting.ShowDialog()
    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

    End Sub
End Class