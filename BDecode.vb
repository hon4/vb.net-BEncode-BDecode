'#======================#
'| BDecode Class by hon |
'#======================#
'| Coded by: hon        |
'| Version: 2.2.0       |
'| Date: D07-M08-Y2024  |
'| honcode.blogspot.com |
'+----------------------+
Imports System.Text
Public Class BDecode

    Public Shared Function BDecode(ByVal b As Byte()) As Dictionary(Of String, Object)
        Dim i As UInteger = 0
        Return BDecode_dict(b, i)
    End Function

    Private Shared Function BDecode_dict(ByVal b As Byte(), ByRef i As UInteger) As Dictionary(Of String, Object)
        Dim ret As New Dictionary(Of String, Object)
        i += 1 'skp 'd'
        While Not b(i) = 101 '101=e
            Dim key As String = Encoding.UTF8.GetString(BDecode_str(b, i))
            If IsNumeric(Encoding.ASCII.GetString({b(i)})) Then
                ret(key) = BDecode_str(b, i)
            ElseIf b(i) = 105 Then '105=i
                ret(key) = BDecode_int(b, i)
            ElseIf b(i) = 100 Then '100=d
                ret(key) = BDecode_dict(b, i)
            ElseIf b(i) = 108 Then '108=l
                ret(key) = BDecode_list(b, i)
            End If
            'i += 1
        End While
        i += 1 'skp 'e'
        Return ret
    End Function

    Private Shared Function BDecode_str(ByVal b As Byte(), ByRef i As UInteger) As Byte()
        Dim lenstr As String = ""
        While IsNumeric(Encoding.ASCII.GetString({b(i)}))
            lenstr &= Encoding.ASCII.GetString({b(i)})
            i += 1
        End While
        i += 1 'skp ':'
        Dim len As UInteger = CUInt(lenstr)
        Dim ret As New List(Of Byte)
        Dim ito As UInteger = i + len
        While i < ito
            ret.Add(b(i))
            i += 1
        End While
        Return ret.ToArray
    End Function

    Private Shared Function BDecode_int(ByVal b As Byte(), ByRef i As UInteger) As Int64
        i += 1 'skp 'i'
        Dim retbytes As New List(Of Byte)
        While Not b(i) = 101 '101=e
            retbytes.Add(b(i))
            i += 1
        End While
        i += 1 'skp 'e'
        Return CType(Encoding.ASCII.GetString(retbytes.ToArray), Int64)
    End Function

    Private Shared Function BDecode_list(ByVal b As Byte(), ByRef i As UInteger) As List(Of Object)
        Dim ret As New List(Of Object)
        i += 1 'skp 'l'
        While Not b(i) = 101 '101=e
            If IsNumeric(Encoding.ASCII.GetString({b(i)})) Then
                ret.Add(BDecode_str(b, i))
            ElseIf b(i) = 105 Then '105=i
                ret.Add(BDecode_int(b, i))
            ElseIf b(i) = 100 Then '100=d
                ret.Add(BDecode_dict(b, i))
            ElseIf b(i) = 108 Then '108=l
                ret.Add(BDecode_list(b, i))
            End If
        End While
        i += 1 'skp 'e'
        Return ret
    End Function
End Class
