Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Xml

Public Class Diary

    Dim FileMenu As New ToolStripMenuItem("File")

    Dim FileMenu2 As New ToolStripMenuItem("Add Bulk XML")

    Dim FileMenu3 As New ToolStripMenuItem("Backup to XML")

    Dim FileMenu4 As New ToolStripMenuItem("Quit")

    Public Sub New()
        InitializeComponent()
        Call PrepareDatas()
        Try
            ComboBox1.SelectedIndex = 0
            ComboBox1_SelectedIndexChanged_1(Nothing, Nothing)
        Catch Ex As Exception
        End Try
        MenuStrip1.Dock = DockStyle.Left

        MenuStrip1.BackColor = Color.OrangeRed

        FileMenu.BackColor = Color.Yellow

        FileMenu.ForeColor = Color.Black

        FileMenu.Text = "File Menu"

        FileMenu.Font = New Font("Georgia", 16)

        FileMenu.TextAlign = ContentAlignment.BottomRight

        FileMenu.TextDirection = ToolStripTextDirection.Vertical90

        FileMenu.ToolTipText = "Click Me"
        MenuStrip1.Items.Add(FileMenu)

        FileMenu2.BackColor = Color.OrangeRed

        FileMenu2.ForeColor = Color.Black

        FileMenu2.Text = "Add Bulk XML"

        FileMenu2.Font = New Font("Georgia", 16)

        FileMenu2.TextAlign = ContentAlignment.BottomRight

        FileMenu2.TextDirection = ToolStripTextDirection.Horizontal

        FileMenu2.ToolTipText = "Click Me"
        FileMenu.DropDownItems.Add(FileMenu2)

        AddHandler FileMenu2.Click, AddressOf addBulkClick

        FileMenu3.BackColor = Color.OrangeRed

        FileMenu3.ForeColor = Color.Black

        FileMenu3.Text = "Backup to XML"

        FileMenu3.Font = New Font("Georgia", 16)

        FileMenu3.TextAlign = ContentAlignment.BottomRight

        FileMenu3.TextDirection = ToolStripTextDirection.Horizontal

        FileMenu3.ToolTipText = "Click Me"
        FileMenu.DropDownItems.Add(FileMenu3)

        AddHandler FileMenu3.Click, AddressOf backupToXmlClick

        FileMenu4.BackColor = Color.OrangeRed

        FileMenu4.ForeColor = Color.Black

        FileMenu4.Text = "Quit"

        FileMenu4.Font = New Font("Georgia", 16)

        FileMenu4.TextAlign = ContentAlignment.BottomRight

        FileMenu4.TextDirection = ToolStripTextDirection.Horizontal

        FileMenu4.ToolTipText = "Click Me"
        FileMenu.DropDownItems.Add(FileMenu4)

        AddHandler FileMenu4.Click, AddressOf quitClick
    End Sub
    Private Sub addBulkClick()
        Button6_Click(Nothing, Nothing)
    End Sub
    Private Sub backupToXmlClick()
        Button7_Click(Nothing, Nothing)
    End Sub
    Private Sub quitClick()
        Application.Exit()
    End Sub


    Private Sub PrepareDatas()
        Dim cmd As SqlCommand
        Dim con = New SqlConnection("server = localhost; database = test; integrated security = true")
        Dim reader As SqlDataReader
        Call con.Open()
        cmd = New SqlCommand("select name from diary", con)
        reader = cmd.ExecuteReader()
        Call ComboBox1.Items.Clear()
        While reader.Read()
            ComboBox1.Items.Add(reader.GetValue(0).ToString)
        End While
        con.Close()
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        RichTextBox2.Focus()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim cmd As SqlCommand
        Dim con = New SqlConnection("server = localhost; database = test; integrated security = true")
        Call con.Open()
        cmd = New SqlCommand("update diary set content = '" & RichTextBox1.Text.Replace("'", "''") & "' where name like '" & ComboBox1.SelectedItem.ToString.Replace("'", "''") & "'", con)
        Call cmd.ExecuteNonQuery()
        con.Close()
        MessageBox.Show("Updated")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim cmd As SqlCommand
        Dim con = New SqlConnection("server = localhost; database = test; integrated security = true")
        Call con.Open()
        cmd = New SqlCommand("insert into diary (name, content, inputdate) values ('" & TextBox1.Text.Replace("'", "''") & "', '" & RichTextBox2.Text.Replace("'", "''") & "', getDate())", con)
        Call cmd.ExecuteNonQuery()
        con.Close()
        PrepareDatas()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs)
        Try
            ComboBox1.SelectedIndex = ComboBox1.SelectedIndex - 1
            ComboBox1_SelectedIndexChanged_1(Nothing, Nothing)
        Catch Ex As Exception
        End Try
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs)
        Try
            ComboBox1.SelectedIndex = ComboBox1.SelectedIndex + 1
            ComboBox1_SelectedIndexChanged_1(Nothing, Nothing)
        Catch Ex As Exception
        End Try
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Try
            Dim a As String = ""
            Dim firstline As String = ""
            Try
                OpenFileDialog1.Title = "Open File..."
                OpenFileDialog1.ShowDialog()
                Using SR As New System.IO.StreamReader(OpenFileDialog1.OpenFile())
                    Do
                        Try
                            firstline = SR.ReadLine
                            a = a & firstline
                        Catch Ex As Exception
                        End Try
                    Loop Until firstline Is Nothing
                End Using
            Catch Ex As Exception
            End Try
            Dim xml As XDocument = XDocument.Parse(a)
            Dim listRows As List(Of XElement) = xml.Descendants("title").ToList()
            For Each node As XElement In listRows
                Dim values As List(Of XElement) = node.Descendants("content").ToList()
                Dim ip As String = values(0)
                Dim bytes As String = values(1)
                Dim twi As String = values(2)
                Dim cmd As SqlCommand
                Dim con = New SqlConnection("server = localhost; database = test; integrated security = true")
                Call con.Open()
                Try
                    cmd = New SqlCommand("insert into diary (name, content, inputdate) values ('" & ip.Replace("'", "''") & "', '" & bytes.Replace("'", "''") & "', '" & twi & "')", con)
                    Call cmd.ExecuteNonQuery()
                Catch Ex As Exception
                    MessageBox.Show(Ex.Message)
                End Try
                con.Close()
                PrepareDatas()
            Next
            Try
                ComboBox1.SelectedIndex = 0
                ComboBox1_SelectedIndexChanged_1(Nothing, Nothing)
            Catch Ex As Exception
            End Try
        Catch Ex As Exception
        End Try
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        RichTextBox2.Text = ""
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Dim myDoc As Xml.XmlDocument = New Xml.XmlDocument
        Dim docNode = myDoc.CreateXmlDeclaration("1.0", "UTF-8", Nothing)
        Dim productsNode = myDoc.CreateElement("datas")
        Dim root As XmlNode = myDoc.DocumentElement
        myDoc.AppendChild(docNode)
        myDoc.AppendChild(productsNode)
        For i = 0 To ComboBox1.Items.Count - 1
            Dim myTitle As XmlElement = myDoc.CreateElement("title")
            Dim myContent1 As XmlElement = myDoc.CreateElement("content")
            Dim myContent2 As XmlElement = myDoc.CreateElement("content")
            Dim myContent3 As XmlElement = myDoc.CreateElement("content")
            Dim cmd As SqlCommand
            Dim con = New SqlConnection("server = localhost; database = test; integrated security = true")
            Call con.Open()
            cmd = New SqlCommand("select name, content, inputdate from diary where name like '" & ComboBox1.Items(i).ToString.Replace("'", "''") & "'", con)
            Dim reader As SqlDataReader
            reader = cmd.ExecuteReader()
            If reader.Read() Then
                myContent1.InnerText = reader.GetValue(0).ToString()
                myContent2.InnerText = reader.GetValue(1).ToString()
                myContent3.InnerText = reader.GetValue(2).ToString()
            End If
            Call con.Close()
            productsNode.AppendChild(myTitle)
            myTitle.AppendChild(myContent1)
            myTitle.AppendChild(myContent2)
            myTitle.AppendChild(myContent3)
        Next
        myDoc.Save("D:\\backup.xml")
        MsgBox("Xml File GeneratedUpdated in D:\backup.xml")
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Application.Exit()
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Dim cmd As SqlCommand
        Dim con = New SqlConnection("server = localhost; database = test; integrated security = true")
        Call con.Open()
        cmd = New SqlCommand("delete from diary where name like '" & ComboBox1.SelectedItem.ToString.Replace("'", "''") & "'", con)
        Call cmd.ExecuteNonQuery()
        con.Close()
        MessageBox.Show("Deleted This")
        Call PrepareDatas()
        ComboBox1.Text = ""
        RichTextBox1.Text = ""


    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Dim cmd As SqlCommand
        Dim con = New SqlConnection("server = localhost; database = test; integrated security = true")
        Call con.Open()
        cmd = New SqlCommand("delete from diary", con)
        Call cmd.ExecuteNonQuery()
        con.Close()
        MessageBox.Show("Deleted All")
        Call PrepareDatas()
        ComboBox1.Text = ""
        RichTextBox1.Text = ""


    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Application.Exit()
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        RichTextBox1.Text = ""

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged_1(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        RichTextBox1.Text = ""
        RichTextBox2.Text = ""
        RichTextBox3.Text = ""
        RichTextBox4.Text = ""
        RichTextBox5.Text = ""
        RichTextBox6.Text = ""
        RichTextBox7.Text = ""
        RichTextBox8.Text = ""

        Try
            Dim cmd As SqlCommand
            Dim con = New SqlConnection("server = localhost; database = test; integrated security = true")
            Dim reader As SqlDataReader
            For i = 0 To ComboBox1.SelectedItems.Count - 1
                Call con.Open()
                Dim a = ComboBox1.SelectedItems.Item(i)
                cmd = New SqlCommand("select name, content, inputdate from diary where name like '" & a.Replace("'", "''") & "'", con)
                reader = cmd.ExecuteReader()
                If reader.Read() Then
                    If i = 0 Then
                        RichTextBox1.Text = reader.GetValue(1).ToString
                        Label3.Text = reader.GetValue(2).ToString
                    End If
                    If i = 1 Then
                        RichTextBox3.Text = reader.GetValue(1).ToString
                        Label6.Text = reader.GetValue(2).ToString
                    End If
                    If i = 2 Then
                        RichTextBox4.Text = reader.GetValue(1).ToString
                        Label5.Text = reader.GetValue(2).ToString
                    End If
                    If i = 3 Then
                        RichTextBox5.Text = reader.GetValue(1).ToString
                        Label10.Text = reader.GetValue(2).ToString
                    End If
                    If i = 4 Then
                        RichTextBox6.Text = reader.GetValue(1).ToString
                        Label7.Text = reader.GetValue(2).ToString
                    End If
                    If i = 5 Then
                        RichTextBox7.Text = reader.GetValue(1).ToString
                        Label9.Text = reader.GetValue(2).ToString
                    End If
                    If i = 6 Then
                        RichTextBox8.Text = reader.GetValue(1).ToString
                        Label8.Text = reader.GetValue(2).ToString
                    End If
                End If
                con.Close()
            Next
        Catch Ex As Exception
            MessageBox.Show(Ex.Message)
        End Try
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        RichTextBox3.Text = ""
    End Sub

    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        RichTextBox4.Text = ""
    End Sub

    Private Sub Button28_Click(sender As Object, e As EventArgs) Handles Button28.Click
        RichTextBox8.Text = ""
    End Sub

    Private Sub Button21_Click(sender As Object, e As EventArgs) Handles Button21.Click
        RichTextBox6.Text = ""
    End Sub

    Private Sub Button23_Click(sender As Object, e As EventArgs) Handles Button23.Click
        RichTextBox7.Text = ""
    End Sub

    Private Sub Button36_Click(sender As Object, e As EventArgs) Handles Button36.Click
        RichTextBox5.Text = ""
    End Sub

    Private Sub Button40_Click(sender As Object, e As EventArgs) Handles Button40.Click
        Dim cmd As SqlCommand
        Dim con = New SqlConnection("server = localhost; database = test; integrated security = true")
        Call con.Open()
        cmd = New SqlCommand("update diary set content = '" & RichTextBox5.Text.Replace("'", "''") & "' where name like '" & ComboBox1.SelectedItems(3).ToString.Replace("'", "''") & "'", con)
        Call cmd.ExecuteNonQuery()
        con.Close()
        MessageBox.Show("Updated")
    End Sub

    Private Sub Button37_Click(sender As Object, e As EventArgs) Handles Button37.Click
        Dim cmd As SqlCommand
        Dim con = New SqlConnection("server = localhost; database = test; integrated security = true")
        Call con.Open()
        cmd = New SqlCommand("delete from diary where name like '" & ComboBox1.SelectedItems(3).ToString.Replace("'", "''") & "'", con)
        Call cmd.ExecuteNonQuery()
        con.Close()
        MessageBox.Show("Deleted This")
        Call PrepareDatas()
        ComboBox1.Text = ""
        RichTextBox5.Text = ""



    End Sub

    Private Sub Button39_Click(sender As Object, e As EventArgs)
        Try
            Dim a = ComboBox1.SelectedIndices(3)
            ComboBox1.SetSelected(a, False)
            ComboBox1.SetSelected(a - 1, True)
            ComboBox1_SelectedIndexChanged_1(Nothing, Nothing)
        Catch Ex As Exception
        End Try
    End Sub

    Private Sub Button38_Click(sender As Object, e As EventArgs)
        Try
            ComboBox1.SetSelected(ComboBox1.SelectedIndices(3) + 1, True)
            ComboBox1.SetSelected(ComboBox1.SelectedIndices(3), False)
            ComboBox1_SelectedIndexChanged_1(Nothing, Nothing)
        Catch Ex As Exception
        End Try
    End Sub

    Private Sub Button27_Click(sender As Object, e As EventArgs) Handles Button27.Click
        Dim cmd As SqlCommand
        Dim con = New SqlConnection("server = localhost; database = test; integrated security = true")
        Call con.Open()
        cmd = New SqlCommand("update diary set content = '" & RichTextBox6.Text.Replace("'", "''") & "' where name like '" & ComboBox1.SelectedItems(4).ToString.Replace("'", "''") & "'", con)
        Call cmd.ExecuteNonQuery()
        con.Close()
        MessageBox.Show("Updated")
    End Sub

    Private Sub Button22_Click(sender As Object, e As EventArgs) Handles Button22.Click
        Dim cmd As SqlCommand
        Dim con = New SqlConnection("server = localhost; database = test; integrated security = true")
        Call con.Open()
        cmd = New SqlCommand("delete from diary where name like '" & ComboBox1.SelectedItems(4).ToString.Replace("'", "''") & "'", con)
        Call cmd.ExecuteNonQuery()
        con.Close()
        MessageBox.Show("Deleted This")
        Call PrepareDatas()
        ComboBox1.Text = ""
        RichTextBox6.Text = ""




    End Sub

    Private Sub Button25_Click(sender As Object, e As EventArgs)
        Try
            Dim a = ComboBox1.SelectedIndices(4)
            ComboBox1.SetSelected(a, False)
            ComboBox1.SetSelected(a - 1, True)
            ComboBox1_SelectedIndexChanged_1(Nothing, Nothing)
        Catch Ex As Exception
        End Try
    End Sub

    Private Sub Button24_Click(sender As Object, e As EventArgs)
        Try
            ComboBox1.SetSelected(ComboBox1.SelectedIndices(4) + 1, True)
            ComboBox1.SetSelected(ComboBox1.SelectedIndices(4), False)
            ComboBox1_SelectedIndexChanged_1(Nothing, Nothing)
        Catch Ex As Exception
        End Try
    End Sub

    Private Sub Button43_Click(sender As Object, e As EventArgs) Handles Button43.Click
        Dim cmd As SqlCommand
        Dim con = New SqlConnection("server = localhost; database = test; integrated security = true")
        Call con.Open()
        cmd = New SqlCommand("update diary set content = '" & RichTextBox4.Text.Replace("'", "''") & "' where name like '" & ComboBox1.SelectedItems(2).ToString.Replace("'", "''") & "'", con)
        Call cmd.ExecuteNonQuery()
        con.Close()
        MessageBox.Show("Updated")
    End Sub

    Private Sub Button19_Click(sender As Object, e As EventArgs) Handles Button19.Click
        Dim cmd As SqlCommand
        Dim con = New SqlConnection("server = localhost; database = test; integrated security = true")
        Call con.Open()
        cmd = New SqlCommand("delete from diary where name like '" & ComboBox1.SelectedItems(4).ToString.Replace("'", "''") & "'", con)
        Call cmd.ExecuteNonQuery()
        con.Close()
        MessageBox.Show("Deleted This")
        Call PrepareDatas()
        ComboBox1.Text = ""
        RichTextBox6.Text = ""





    End Sub

    Private Sub Button42_Click(sender As Object, e As EventArgs)
        Try
            Dim a = ComboBox1.SelectedIndices(2)
            ComboBox1.SetSelected(a, False)
            ComboBox1.SetSelected(a - 1, True)
            ComboBox1_SelectedIndexChanged_1(Nothing, Nothing)
        Catch Ex As Exception
        End Try
    End Sub

    Private Sub Button41_Click(sender As Object, e As EventArgs)
        Try
            ComboBox1.SetSelected(ComboBox1.SelectedIndices(2) + 1, True)
            ComboBox1.SetSelected(ComboBox1.SelectedIndices(2), False)
            ComboBox1_SelectedIndexChanged_1(Nothing, Nothing)
        Catch Ex As Exception
        End Try
    End Sub

    Private Sub Button20_Click(sender As Object, e As EventArgs) Handles Button20.Click
        Dim cmd As SqlCommand
        Dim con = New SqlConnection("server = localhost; database = test; integrated security = true")
        Call con.Open()
        cmd = New SqlCommand("update diary set content = '" & RichTextBox3.Text.Replace("'", "''") & "' where name like '" & ComboBox1.SelectedItems(1).ToString.Replace("'", "''") & "'", con)
        Call cmd.ExecuteNonQuery()
        con.Close()
        MessageBox.Show("Updated")
    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        Dim cmd As SqlCommand
        Dim con = New SqlConnection("server = localhost; database = test; integrated security = true")
        Call con.Open()
        cmd = New SqlCommand("delete from diary where name like '" & ComboBox1.SelectedItems(1).ToString.Replace("'", "''") & "'", con)
        Call cmd.ExecuteNonQuery()
        con.Close()
        MessageBox.Show("Deleted This")
        Call PrepareDatas()
        ComboBox1.Text = ""
        RichTextBox3.Text = ""






    End Sub

    Private Sub Button18_Click(sender As Object, e As EventArgs)
        Try
            Dim a = ComboBox1.SelectedIndices(1)
            ComboBox1.SetSelected(a, False)
            ComboBox1.SetSelected(a - 1, True)
            ComboBox1_SelectedIndexChanged_1(Nothing, Nothing)
        Catch Ex As Exception
        End Try
    End Sub

    Private Sub Button17_Click(sender As Object, e As EventArgs)
        Try
            ComboBox1.SetSelected(ComboBox1.SelectedIndices(1) + 1, True)
            ComboBox1.SetSelected(ComboBox1.SelectedIndices(1), False)
            ComboBox1_SelectedIndexChanged_1(Nothing, Nothing)
        Catch Ex As Exception
        End Try
    End Sub

    Private Sub Button35_Click(sender As Object, e As EventArgs) Handles Button35.Click
        Dim cmd As SqlCommand
        Dim con = New SqlConnection("server = localhost; database = test; integrated security = true")
        Call con.Open()
        cmd = New SqlCommand("update diary set content = '" & RichTextBox7.Text.Replace("'", "''") & "' where name like '" & ComboBox1.SelectedItems(5).ToString.Replace("'", "''") & "'", con)
        Call cmd.ExecuteNonQuery()
        con.Close()
        MessageBox.Show("Updated")
    End Sub

    Private Sub Button26_Click(sender As Object, e As EventArgs) Handles Button26.Click
        Dim cmd As SqlCommand
        Dim con = New SqlConnection("server = localhost; database = test; integrated security = true")
        Call con.Open()
        cmd = New SqlCommand("delete from diary where name like '" & ComboBox1.SelectedItems(5).ToString.Replace("'", "''") & "'", con)
        Call cmd.ExecuteNonQuery()
        con.Close()
        MessageBox.Show("Deleted This")
        Call PrepareDatas()
        ComboBox1.Text = ""
        RichTextBox7.Text = ""







    End Sub

    Private Sub Button33_Click(sender As Object, e As EventArgs)
        Try
            Dim a = ComboBox1.SelectedIndices(5)
            ComboBox1.SetSelected(a, False)
            ComboBox1.SetSelected(a - 1, True)
            ComboBox1_SelectedIndexChanged_1(Nothing, Nothing)
        Catch Ex As Exception
        End Try
    End Sub

    Private Sub Button30_Click(sender As Object, e As EventArgs)
        Try
            ComboBox1.SetSelected(ComboBox1.SelectedIndices(5) + 1, True)
            ComboBox1.SetSelected(ComboBox1.SelectedIndices(5), False)
            ComboBox1_SelectedIndexChanged_1(Nothing, Nothing)
        Catch Ex As Exception
        End Try
    End Sub

    Private Sub Button34_Click(sender As Object, e As EventArgs) Handles Button34.Click
        Dim cmd As SqlCommand
        Dim con = New SqlConnection("server = localhost; database = test; integrated security = true")
        Call con.Open()
        cmd = New SqlCommand("update diary set content = '" & RichTextBox8.Text.Replace("'", "''") & "' where name like '" & ComboBox1.SelectedItems(6).ToString.Replace("'", "''") & "'", con)
        Call cmd.ExecuteNonQuery()
        con.Close()
        MessageBox.Show("Updated")
    End Sub

    Private Sub Button29_Click(sender As Object, e As EventArgs) Handles Button29.Click
        Dim cmd As SqlCommand
        Dim con = New SqlConnection("server = localhost; database = test; integrated security = true")
        Call con.Open()
        cmd = New SqlCommand("delete from diary where name like '" & ComboBox1.SelectedItems(6).ToString.Replace("'", "''") & "'", con)
        Call cmd.ExecuteNonQuery()
        con.Close()
        MessageBox.Show("Deleted This")
        Call PrepareDatas()
        ComboBox1.Text = ""
        RichTextBox8.Text = ""







    End Sub

    Private Sub Button31_Click(sender As Object, e As EventArgs)
        Try
            Dim a = ComboBox1.SelectedIndices(6)
            ComboBox1.SetSelected(a, False)
            ComboBox1.SetSelected(a - 1, True)
            ComboBox1_SelectedIndexChanged_1(Nothing, Nothing)
        Catch Ex As Exception
        End Try
    End Sub

    Private Sub Button32_Click(sender As Object, e As EventArgs)
        Try
            ComboBox1.SetSelected(ComboBox1.SelectedIndices(6) + 1, True)
            ComboBox1.SetSelected(ComboBox1.SelectedIndices(6), False)
            ComboBox1_SelectedIndexChanged_1(Nothing, Nothing)
        Catch Ex As Exception
        End Try
    End Sub
End Class