'#======================#
'| BEncode Class by hon |
'#======================#
'| Coded by: hon        |
'| Version: 2.2.0       |
'| Date: D07-M08-Y2024  |
'| honcode.blogspot.com |
'+----------------------+
Imports System.Text
Public Class BEncode

    Public Shared Function BEncode(ByVal d As Dictionary(Of String, Object)) As Byte()
        Return BEncode_dict(d)
    End Function

    Private Shared Function BEncode_str(ByVal s As String) As Byte()
        Return BEncode_strBARR(Encoding.UTF8.GetBytes(s)) 'All str in UTF8
    End Function

    Private Shared Function BEncode_int(ByVal i As Int64) As Byte()
        Dim r As New List(Of Byte)
        r.Add(105) '105=i
        r.AddRange(Encoding.ASCII.GetBytes(i))
        r.Add(101) '101=e
        Return r.ToArray
    End Function

    Private Shared Function BEncode_dict(ByVal d As Dictionary(Of String, Object)) As Byte()
        Dim ret As New List(Of Byte)
        ret.Add(100) '100=d
        For Each x As KeyValuePair(Of String, Object) In d
            ret.AddRange(BEncode_str(x.Key))
            If TypeOf x.Value Is String Then
                ret.AddRange(BEncode_str(x.Value))
            ElseIf TypeOf x.Value Is Dictionary(Of String, Object) Then
                ret.AddRange(BEncode_dict(x.Value))
            ElseIf TypeOf x.Value Is List(Of Object) Then
                ret.AddRange(BEncode_list(x.Value))
            ElseIf TypeOf x.Value Is Byte() Then
                ret.AddRange(BEncode_strBARR(x.Value))
            Else
                ret.AddRange(BEncode_int(x.Value))
            End If
        Next
        ret.Add(101) '101=e
        Return ret.ToArray
    End Function

    Private Shared Function BEncode_list(ByVal l As List(Of Object)) As Byte()
        Dim ret As New List(Of Byte)
        ret.Add(108) '108=l
        For Each x As Object In l
            If TypeOf x Is String Then
                ret.AddRange(BEncode_str(x))
            ElseIf TypeOf x Is Dictionary(Of String, Object) Then
                ret.AddRange(BEncode_dict(x))
            ElseIf TypeOf x Is List(Of Object) Then
                ret.AddRange(BEncode_list(x))
            ElseIf TypeOf x Is Byte() Then
                ret.AddRange(BEncode_strBARR(x))
            Else
                ret.AddRange(BEncode_int(x))
            End If
        Next
        ret.Add(101) '101=e
        Return ret.ToArray
    End Function

    Private Shared Function BEncode_strBARR(ByVal b As Byte()) As Byte()
        Dim ret As New List(Of Byte)
        ret.AddRange(Encoding.ASCII.GetBytes(b.Length))
        ret.Add(58) '58=:
        ret.AddRange(b)
        Return ret.ToArray
    End Function
End Class
