Imports System.Data.SqlClient

Public Class frmStudLevTransfer
    Sub Reset()
        cmbCStudy.SelectedIndex = -1
        cmbSess.SelectedIndex = -1
        cmbLev.SelectedIndex = -1
        'cmbCStudy.Text = ""
        'cmbLev.Text = ""
        txtSectionID.Text = ""

        cmbCStudy1.SelectedIndex = -1
        cmbSess1.SelectedIndex = -1
        cmbLev1.SelectedIndex = -1
        listView1.Items.Clear()
    End Sub

    Sub fillDept()
        Try
            Dim CN1 As New SqlConnection(cs)
            CN1.Open()
            adp = New SqlDataAdapter()
            adp.SelectCommand = New SqlCommand("SELECT distinct RTRIM(Dept) FROM Departments", CN1)
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

    Sub fillDept1()
        Try
            Dim CN1 As New SqlConnection(cs)
            CN1.Open()
            adp = New SqlDataAdapter()
            adp.SelectCommand = New SqlCommand("SELECT distinct RTRIM(Dept) FROM Departments", CN1)
            ds = New DataSet("ds")
            adp.Fill(ds)
            Dim dtable As DataTable = ds.Tables(0)
            cmbCStudy1.Items.Clear()
            For Each drow As DataRow In dtable.Rows
                cmbCStudy1.Items.Add(drow(0).ToString())
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Try
            btnUpdate.Enabled = True
            If Len(Trim(cmbSess.Text)) = 0 Then
                MessageBox.Show("Please select session", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cmbSess.Focus()
                Exit Sub
            End If
            If Len(Trim(cmbCStudy.Text)) = 0 Then
                MessageBox.Show("Please select department", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cmbCStudy.Focus()
                Exit Sub
            End If
            If Len(Trim(cmbLev.Text)) = 0 Then
                MessageBox.Show("Please select current students level", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cmbLev.Focus()
                Exit Sub
            End If
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("select MatricNo, StudName from Students where Sess=@d1 and Lev=@d2 and Dept=@d3 order by MatricNo", con)
            cmd.Parameters.AddWithValue("@d1", cmbSess.Text)
            cmd.Parameters.AddWithValue("@d2", cmbLev.Text)
            cmd.Parameters.AddWithValue("@d3", cmbCStudy.Text)
            rdr = cmd.ExecuteReader()
            listView1.Items.Clear()
            While rdr.Read()
                Dim item = New ListViewItem()
                item.Text = rdr(0).ToString().Trim()
                item.SubItems.Add(rdr(1).ToString().Trim())
                listView1.Items.Add(item)
            End While
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

    Private Sub frmStudLevTransfer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        fillDept()
        fillDept1()
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Try
            If Len(Trim(cmbSess1.Text)) = 0 Then
                MessageBox.Show("Please select session", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cmbSess1.Focus()
                Exit Sub
            End If
            If Len(Trim(cmbLev1.Text)) = 0 Then
                MessageBox.Show("Please select level", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cmbLev1.Focus()
                Exit Sub
            End If
            If listView1.Items.Count = 0 Then
                MessageBox.Show("Sorry nothing to update.." & vbCrLf & "Please retrieve data in listview", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            For i As Integer = listView1.Items.Count - 1 To 0 Step -1

                If listView1.Items(i).Checked = True Then
                    con = New SqlConnection(cs)
                    Dim cd As String = "update students set Sess=@d1,Lev=" & cmbLev1.Text & " where MatricNo=@d2"
                    'Dim cd As String = "update students set Sess=@d1,Lev=@d1 where MatricNo=@d2"
                    cmd = New SqlCommand(cd)
                    cmd.Parameters.AddWithValue("@d1", cmbSess1.Text)
                    cmd.Parameters.AddWithValue("@d2", listView1.Items(i).SubItems(0).Text)
                    'cmd.Parameters.AddWithValue("@d3", cmbLev1.Text)
                    cmd.Connection = con
                    con.Open()
                    cmd.ExecuteNonQuery()
                    con.Close()
                End If
            Next
            MessageBox.Show("Successfully updated", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information)
            btnUpdate.Enabled = False
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
        End Try
    End Sub
End Class