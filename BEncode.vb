'#======================#
'| BEncode Class by hon |
'#======================#
'| Coded by: hon        |
'| Version: 2.0.0       |
'| Date: D07-M08-Y2024  |
'| honcode.blogspot.com |
'+----------------------+
Imports System.Text
Public Class BEncode

    Public Shared Function BEncode(ByVal d As Dictionary(Of String, Object)) As Byte()
        Return BEncode_dict(d)
    End Function

    Private Shared Function BEncode_str(ByVal s As String) As Byte()
        Return str2bytez(s.Length & ":" & s)
    End Function

    Private Shared Function BEncode_int(ByVal i As Integer) As Byte()
        Return str2bytez("i" & i & "e")
    End Function

    Private Shared Function BEncode_dict(ByVal d As Dictionary(Of String, Object)) As Byte()
        Dim ret As Byte() = str2bytez("d")
        For Each x As KeyValuePair(Of String, Object) In d
            ret = ret.Concat(BEncode_str(x.Key)).ToArray
            If TypeOf x.Value Is String Then
                ret = ret.Concat(BEncode_str(x.Value)).ToArray
            ElseIf TypeOf x.Value Is Dictionary(Of String, Object) Then
                ret = ret.Concat(BEncode_dict(x.Value)).ToArray
            ElseIf TypeOf x.Value Is List(Of Object) Then
                ret = ret.Concat(BEncode_list(x.Value)).ToArray
            ElseIf TypeOf x.Value Is Byte() Then
                ret = ret.Concat(BEncode_strBARR(x.Value)).ToArray
            Else
                ret = ret.Concat(BEncode_int(x.Value)).ToArray
            End If
        Next
        Return ret.Concat(str2bytez("e")).ToArray
    End Function

    Private Shared Function BEncode_list(ByVal l As List(Of Object)) As Byte()
        Dim ret As Byte() = str2bytez("l")
        For Each x As Object In l
            If TypeOf x Is String Then
                ret = ret.Concat(BEncode_str(x)).ToArray
            ElseIf TypeOf x Is Dictionary(Of String, Object) Then
                ret = ret.Concat(BEncode_dict(x)).ToArray
            ElseIf TypeOf x Is List(Of Object) Then
                ret = ret.Concat(BEncode_list(x)).ToArray
            ElseIf TypeOf x Is Byte() Then
                ret = ret.Concat(BEncode_strBARR(x)).ToArray
            Else
                ret = ret.Concat(BEncode_int(x)).ToArray
            End If
        Next
        Return ret.Concat(str2bytez("e")).ToArray
    End Function

    Private Shared Function str2bytez(ByVal s As String) As Byte()
        Return Encoding.Default.GetBytes(s)
    End Function

    Private Shared Function BEncode_strBARR(ByVal b As Byte()) As Byte()
        Return str2bytez(b.Length & ":").Concat(b).ToArray
    End Function
End Class
