Imports MySql.Data.MySqlClient
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.Win32

Public Class Form1

    Dim sqlcon As New MySqlConnection
    Dim sqlcmd As New MySqlCommand
    Dim sqlrd As MySqlDataReader
    Dim sqldt As New DataTable
    Dim dta As New MySqlDataAdapter

    Dim server As String = "localhost"
    Dim username As String = "root"
    Dim password As String = "WAlid@@2501"
    Dim database As String = "myconnector"
    Dim sqlquery As String
    Private bitmap As Bitmap

    Private Sub updateTable()
        sqlcon.ConnectionString = "server=" + server + ";" + "user id= " + username + ";" + "password = " + password + ";" + "database= " + database

        sqlcon.Open()
        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = "SELECT * FROM myconnector.myconnector"

        sqlrd = sqlcmd.ExecuteReader
        sqldt.Load(sqlrd)
        sqlrd.Close()
        sqlcon.Close()

        DataGridView1.DataSource = sqldt
    End Sub


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        updateTable()
    End Sub

    '---------------------BOUTTON QUITTER-----------------------


    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim exiit As DialogResult
        exiit = MessageBox.Show("voulez vous quitter", "MySql Connector", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If exiit = DialogResult.Yes Then
            Application.Exit()
        End If

    End Sub


    '---------------------BOUTTON AJOUTER-----------------------
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        sqlcon.ConnectionString = "server=" + server + ";" + "user id= " + username + ";" + "password = " + password + ";" + "database= " + database


        Try
            sqlcon.Open()
            sqlquery = "Insert into myconnector.myconnector (CIN, Nom, Prénom , tel , commune , role) value ( '" & cincol.Text & "','" & nomcol.Text & "','" & prenomcol.Text & "','" & telcol.Text & "','" & commcol.SelectedItem & "','" & rolecol.SelectedItem & "')"
            sqlcmd = New MySqlCommand(sqlquery, sqlcon)
            sqlrd = sqlcmd.ExecuteReader
            sqlcon.Close()


        Catch ex As Exception
            MessageBox.Show(ex.Message, "MySql Connector", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Finally
            sqlcon.Dispose()
        End Try

        updateTable()

    End Sub

    '---------------------BOUTTON MODIFIER-----------------------


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        sqlcon.ConnectionString = "server=" + server + ";" + "user id= " + username + ";" + "password = " + password + ";" + "database= " + database

        sqlcon.Open()
        sqlcmd.Connection = sqlcon
        With sqlcmd

            .CommandText = "Update myconnector.myconnector set CIN = @cinn , Nom= @nomm , prénom = @prenomm , tel = @tell , commune = @communee , role = @rolee"

            .CommandType = CommandType.Text
            .Parameters.AddWithValue("@cinn", cincol.Text)
            .Parameters.AddWithValue("@nomm", cincol.Text)
            .Parameters.AddWithValue("@prenomm", cincol.Text)
            .Parameters.AddWithValue("@tell", cincol.Text)
            .Parameters.AddWithValue("@communee", cincol.Text)
            .Parameters.AddWithValue("@rolee", cincol.Text)
        End With

        sqlcmd.ExecuteNonQuery()
        sqlcon.Close()
        updateTable()
    End Sub
    '---------------------DATA GRID VIEW-----------------------

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick

        Try
            cincol.Text = DataGridView1.SelectedRows(0).Cells(0).Value.ToString
            nomcol.Text = DataGridView1.SelectedRows(0).Cells(1).Value.ToString
            prenomcol.Text = DataGridView1.SelectedRows(0).Cells(2).Value.ToString
            telcol.Text = DataGridView1.SelectedRows(0).Cells(3).Value.ToString
            commcol.SelectedItem = DataGridView1.SelectedRows(0).Cells(4).Value.ToString
            rolecol.SelectedItem = DataGridView1.SelectedRows(0).Cells(5).Value.ToString
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try


    End Sub

    '---------------------BOUTTON SUPPRIMER-----------------------

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        For Each row In DataGridView1.SelectedRows
            DataGridView1.Rows.Remove(row)
        Next
        updateTable()
    End Sub

    '---------------------BOUTTON IMPRIMER-----------------------

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim height As Integer = DataGridView1.Height
        DataGridView1.Height = DataGridView1.RowCount * DataGridView1.RowTemplate.Height
        bitmap = New Bitmap(Me.DataGridView1.Width, Me.DataGridView1.Height)
        DataGridView1.DrawToBitmap(bitmap, New Rectangle(0, 0, Me.DataGridView1.Width, Me.DataGridView1.Height))
        PrintPreviewDialog1.Document = PrintDocument1
        PrintPreviewDialog1.PrintPreviewControl.Zoom = 1
        PrintPreviewDialog1.ShowDialog()
        DataGridView1.Height = height
    End Sub
    Private Sub PrintDocument1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        e.Graphics.DrawImage(bitmap, 0, 0)
        Dim recp As RectangleF = e.PageSettings.PrintableArea


        If Me.DataGridView1.Height - recp.Height > 0 Then e.HasMorePages = True
    End Sub

    '---------------------BOUTTON CHERCHER-----------------------


    Private Sub TextBox5_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox5.KeyPress
        Try
            If Asc(e.KeyChar) = 13 Then
                Dim dv As DataView
                dv = sqldt.DefaultView
                dv.RowFilter = String.Format("Nom like '%{0}%'", TextBox5.Text)
                DataGridView1.DataSource = dv.ToTable()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try



    End Sub
End Class
