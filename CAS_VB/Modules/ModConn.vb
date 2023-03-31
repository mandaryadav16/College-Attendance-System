Imports System.IO
Imports System.Configuration
Module ModConn
    'Public Const cs As String = "Data Source=DPROGRAMMER;Initial Catalog=CAS;Integrated Security=True"
    'Public Const cs As String = "Data Source=192.168.0.111,1433;Network Library=DBMSSOCN;Initial Catalog=CAS;User ID=admin;Password=admin123"
    Public cs = ConfigurationManager.ConnectionStrings("CAS_VB.My.MySettings.CASConnString").ConnectionString

    Dim st As String
    Public Function ReadCS() As String
        Using sr As StreamReader = New StreamReader(Application.StartupPath & "\SQLSettings.dat")
            st = sr.ReadLine()
        End Using
        Return st
    End Function
    'Public ReadOnly cs As String = ReadCS()
End Module
