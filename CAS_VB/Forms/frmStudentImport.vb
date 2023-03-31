Imports System.Data.SqlClient
Imports GemBox.Spreadsheet
Imports GemBox.Spreadsheet.WinFormsUtilities

Public Class frmStudentImport
    Dim connsql As SqlConnection
    Dim cmdsql As SqlCommand

    Public Sub New()

        SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY")

        InitializeComponent()
    End Sub


    Sub consql1()
        connsql = New SqlConnection("Data Source=DPROGRAMMER;Initial Catalog=CAS;Integrated Security=True")
        connsql.Open()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        For baris As Integer = 0 To DGV.RowCount - 2
            Call consql1()
            Dim simpan As String = "insert into Students values('" & DGV.Rows(baris).Cells(0).Value & "','" & DGV.Rows(baris).Cells(1).Value & "','" & DGV.Rows(baris).Cells(2).Value & "','" & DGV.Rows(baris).Cells(3).Value & "','" & DGV.Rows(baris).Cells(4).Value & "','" & DGV.Rows(baris).Cells(5).Value & "','" & DGV.Rows(baris).Cells(6).Value & "','" & DGV.Rows(baris).Cells(7).Value & "','" & DGV.Rows(baris).Cells(8).Value & "','" & DGV.Rows(baris).Cells(9).Value & "','" & DGV.Rows(baris).Cells(10).Value & "')"
            cmdsql = New SqlCommand(simpan, connsql)
            cmdsql.ExecuteNonQuery()
        Next
        MsgBox("Data sucessfully saved")
        frmStudent.Getdata()
        DGV.Columns.Clear()

    End Sub

    Private Sub btnLoadFile_Click(sender As Object, e As EventArgs) Handles btnLoadFile.Click
        Dim openFileDialog = New OpenFileDialog()
        openFileDialog.Filter = "XLS files (*.xls, *.xlt)|*.xls;*.xlt|XLSX files (*.xlsx, *.xlsm, *.xltx, *.xltm)|*.xlsx;*.xlsm;*.xltx;*.xltm|ODS files (*.ods, *.ots)|*.ods;*.ots|CSV files (*.csv, *.tsv)|*.csv;*.tsv|HTML files (*.html, *.htm)|*.html;*.htm"
        openFileDialog.FilterIndex = 2

        If (openFileDialog.ShowDialog() = DialogResult.OK) Then

            Dim workbook = ExcelFile.Load(openFileDialog.FileName)

            ' From ExcelFile to DataGridView.
            DataGridViewConverter.ExportToDataGridView(workbook.Worksheets.ActiveWorksheet, Me.DGV, New ExportToDataGridViewOptions() With {.ColumnHeaders = True})
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub
End Class