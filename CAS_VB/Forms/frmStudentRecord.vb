Imports System.Data.SqlClient
Imports System.IO
Imports Excel = Microsoft.Office.Interop.Excel

Public Class frmStudentRecord
    Sub Reset()
        cmbDept.SelectedIndex = -1
        ComboBox1.SelectedIndex = -1
        'TextBox1.Text = ""
        TextBox2.Text = ""
        'ComboBox1.Text = ""
        Getdata()
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

    Public Sub Getdata()
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("SELECT RTRIM(Sess) as [Session], RTRIM(MatricNo) as [Matric No.], RTRIM(StudName) as [Student Name], RTRIM(Lev) as [Level], RTRIM(Gender) as [Gender], RTRIM(SchoolName) as [Faculty], RTRIM(Dept) as [Department], RTRIM(CComb) as [Course Study], RTRIM(PhoneNo) as [Phone No.], RTRIM(Email) as [E-Mail], RTRIM(Address) as [Address] FROM Students order by Dept,CComb,MatricNo", con)
            adp = New SqlDataAdapter(cmd)
            ds = New DataSet()
            adp.Fill(ds, "Students")
            dgw.DataSource = ds.Tables("Students").DefaultView
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub frmCourseRecord_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Getdata()
        fillDept()
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("SELECT RTRIM(Sess) as [Session], RTRIM(MatricNo) as [Matric No.], RTRIM(StudName) as [Student Name], RTRIM(Lev) as [Level], RTRIM(Gender) as [Gender], RTRIM(SchoolName) as [Faculty], RTRIM(Dept) as [Department], RTRIM(CComb) as [Course Study], RTRIM(PhoneNo) as [Phone No.], RTRIM(Email) as [E-Mail], RTRIM(Address) as [Address] FROM Students where StudName like '%" & TextBox2.Text & "%' order by MatricNo", con)
            Dim myDA As SqlDataAdapter = New SqlDataAdapter(cmd)
            Dim myDataSet As DataSet = New DataSet()
            myDA.Fill(myDataSet, "Students")
            dgw.DataSource = myDataSet.Tables("Students").DefaultView
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("SELECT RTRIM(Sess) as [Session], RTRIM(MatricNo) as [Matric No.], RTRIM(StudName) as [Student Name], RTRIM(Lev) as [Level], RTRIM(Gender) as [Gender], RTRIM(SchoolName) as [Faculty], RTRIM(Dept) as [Department], RTRIM(CComb) as [Course Study], RTRIM(PhoneNo) as [Phone No.], RTRIM(Email) as [E-Mail], RTRIM(Address) as [Address] FROM Students where Lev like '" & ComboBox1.Text & "%' order by Dept,CComb", con)
            Dim myDA As SqlDataAdapter = New SqlDataAdapter(cmd)
            Dim myDataSet As DataSet = New DataSet()
            myDA.Fill(myDataSet, "Students")
            dgw.DataSource = myDataSet.Tables("Students").DefaultView
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        Reset()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()

    End Sub

    Private Sub btnExportExcel_Click(sender As Object, e As EventArgs) Handles btnExportExcel.Click
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

    Private Sub dgw_RowPostPaint(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowPostPaintEventArgs) Handles dgw.RowPostPaint
        Dim strRowNumber As String = (e.RowIndex + 1).ToString()
        Dim size As SizeF = e.Graphics.MeasureString(strRowNumber, Me.Font)
        If dgw.RowHeadersWidth < Convert.ToInt32((size.Width + 20)) Then
            dgw.RowHeadersWidth = Convert.ToInt32((size.Width + 20))
        End If
        Dim b As Brush = SystemBrushes.ControlText
        e.Graphics.DrawString(strRowNumber, Me.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2))

    End Sub

    Private Sub cmbDept_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbDept.SelectedIndexChanged
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("SELECT RTRIM(Sess) as [Session], RTRIM(MatricNo) as [Matric No.], RTRIM(StudName) as [Student Name], RTRIM(Lev) as [Level], RTRIM(Gender) as [Gender], RTRIM(SchoolName) as [Faculty], RTRIM(Dept) as [Department], RTRIM(CComb) as [Course Study], RTRIM(PhoneNo) as [Phone No.], RTRIM(Email) as [E-Mail], RTRIM(Address) as [Address] FROM Students where Dept like '" & cmbDept.Text & "%' order by MatricNo", con)
            Dim myDA As SqlDataAdapter = New SqlDataAdapter(cmd)
            Dim myDataSet As DataSet = New DataSet()
            myDA.Fill(myDataSet, "Students")
            dgw.DataSource = myDataSet.Tables("Students").DefaultView
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BtnGetData_Click(sender As Object, e As EventArgs) Handles btnGetData.Click

    End Sub
End Class