Imports System.Data.SqlClient
Imports System.Security.Cryptography
Imports System.IO
Imports System.Text
Imports System.Data.Sql
Imports System.Data
'Imports Microsoft.SqlServer.Management.Smo
Imports Microsoft.SqlServer.Management.Common
Public Class frmSqlServerSetting
    Dim st As String
    Dim SqlConnStr As String
    Private Sub btnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click

    End Sub

    Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        If lblSet.Text = "Login Form" Then
            Me.Close()
        Else
            If MsgBox("Do you want to close the application....", MsgBoxStyle.YesNo + MsgBoxStyle.Information) = MsgBoxResult.Yes Then
                End
            End If
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked

    End Sub

    Private Sub cmbAuthentication_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbAuthentication.SelectedIndexChanged
        If cmbAuthentication.SelectedIndex = 0 Then
            txtUserName.ReadOnly = True
            txtPassword.ReadOnly = True
            txtUserName.Text = ""
            txtPassword.Text = ""
        End If
        If cmbAuthentication.SelectedIndex = 1 Then
            txtUserName.ReadOnly = False
            txtPassword.ReadOnly = False
        End If
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick

    End Sub


    Private Sub cmbServerName_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbServerName.SelectedIndexChanged
        cmbAuthentication.Enabled = True
    End Sub

    Sub Reset()
        txtPassword.Text = ""
        txtUserName.Text = ""
        txtServerName.Text = ""
        cmbAuthentication.SelectedIndex = 0
    End Sub

    Private Sub btnTestConnection_Click_1(sender As System.Object, e As System.EventArgs) Handles btnTestConnection.Click

    End Sub


End Class
