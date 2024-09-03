# BEncode and BDecode functions for Visual Basic

## General Notes
- Dictionaries must be always `Dictionary(Of String, Object)`
- Lists must me always `List(Of Object)`
- Byte arrays must be `Byte()`. (setting general number array `{1,2,3}` will throw error, use `New Byte() {1,2,3}`)
- BDecoded strings are in byte array format. You can get the string using `System.Text.Encoding.UTF8.GetString(tz("comment"))`

## BEncode
Example Usage:
```
Dim tz As New Dictionary(Of String, Object)
tz("announce") = "http://tracker.google.com/announce.php"

Dim announce_list As New List(Of Object)
announce_list.Add(New List(Of Object) From {"http://example.com/announce.php"})
announce_list.Add(New List(Of Object) From {"udp://tracker.opentrackr.org:1337/announce"})

tz("announce-list") = announce_list
tz("comment") = "comm"
tz("created by") = "uTorrent/2210"
tz("creation date") = 1723031473
tz("encoding") = "UTF-8"

Dim infodict As New Dictionary(Of String, Object)
infodict("length") = 5
infodict("name") = "hello.txt"
infodict("piece length") = 65536
infodict("pieces") = New Byte() {170, 244, 198, 29, 220, 197, 232, 162, 218, 190, 222, 15, 59, 72, 44, 217, 174, 169, 67, 77}
infodict("private") = 1

tz("info") = infodict

Dim benc As Byte() = BEncode.BEncode(tz)

IO.File.WriteAllBytes("C:\Users\Administrator\Desktop\vb.torrent", benc)
```

## BDecode
Example Usage:
```
Dim tz As Dictionary(Of String, Object) = BDecode.BDecode(IO.File.ReadAllBytes("C:\Users\Administrator\Desktop\hello.txt.torrent"))
```

## Requirements
- .NET Framework 2.0
