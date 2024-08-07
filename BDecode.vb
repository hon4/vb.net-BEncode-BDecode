'#======================#
'| BDecode Class by hon |
'#======================#
'| Coded by: hon        |
'| Version: 2.0.1       |
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
        While Not bytez2str({b(i)}) = "e"
            Dim key As String = bytez2str(BDecode_str(b, i))
                If IsNumeric(bytez2str({b(i)})) Then
                    ret(key) = BDecode_str(b, i)
                ElseIf b(i) = str2bytez("i")(0) Then
                    ret(key) = BDecode_int(b, i)
                ElseIf b(i) = str2bytez("d")(0) Then
                    ret(key) = BDecode_dict(b, i)
                ElseIf b(i) = str2bytez("l")(0) Then
                    ret(key) = BDecode_list(b, i)
                End If
            'i += 1
        End While
        i += 1 'skp 'e'
        Return ret
    End Function

    Private Shared Function BDecode_str(ByVal b As Byte(), ByRef i As UInteger) As Byte()
        Dim lenstr As String = ""
        While IsNumeric(bytez2str({b(i)}))
            lenstr &= bytez2str({b(i)})
            i += 1
        End While
        i += 1 'skp ':'
        Dim len As UInteger = CUInt(lenstr)
        Dim ret As Byte() = b.Skip(i).Take(len).ToArray
        i += len
        Return ret
    End Function

    Private Shared Function BDecode_int(ByVal b As Byte(), ByRef i As UInteger) As Int64
        i += 1 'skp 'i'
        Dim retbytes As Byte() = {}
        While Not bytez2str({b(i)}) = "e"
            retbytes = retbytes.Concat({b(i)}).ToArray
            i += 1
        End While
        i += 1 'skp 'e'
        Return CType(bytez2str(retbytes), Int64)
    End Function

    Private Shared Function BDecode_list(ByVal b As Byte(), ByRef i As UInteger) As List(Of Object)
        Dim ret As New List(Of Object)
        i += 1 'skp 'l'
        While Not bytez2str({b(i)}) = "e"
            If IsNumeric(bytez2str({b(i)})) Then
                ret.Add(BDecode_str(b, i))
            ElseIf b(i) = str2bytez("i")(0) Then
                ret.Add(BDecode_int(b, i))
            ElseIf b(i) = str2bytez("d")(0) Then
                ret.Add(BDecode_dict(b, i))
            ElseIf b(i) = str2bytez("l")(0) Then
                ret.Add(BDecode_list(b, i))
            End If
        End While
        i += 1 'skp 'e'
        Return ret
    End Function

    Private Shared Function str2bytez(ByVal s As String) As Byte()
        Return Encoding.Default.GetBytes(s)
    End Function
    Private Shared Function bytez2str(ByVal b As Byte()) As String
        Return Encoding.Default.GetString(b)
    End Function
End Class
