Imports DataBaseExecutors
Imports DataBaseExecutors.Entity
Imports System.Data

Partial Class _Default
    Inherits System.Web.UI.Page

    Private Const TableName As String = "ORC_SALES_ORDER"
    Private Const DataCount As Integer = 100

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not IsPostBack Then
            For Each item As ConnectionStringSettings In ConfigurationManager.ConnectionStrings
                ddlConnection.Items.Add(item.Name)
            Next
            ddlConnection.SelectedIndex = 0
        End If

    End Sub

    Protected Sub btnRun_Click(sender As Object, e As EventArgs) Handles btnRun.Click
        Dim conName As String = ddlConnection.SelectedValue
        Dim db As New DBExecution(conName)
        Dim count As Integer = DataCount

        Try

            If Not String.IsNullOrEmpty(txtRecordCount.Text) Then
                If Not Integer.TryParse(txtRecordCount.Text, count) Then
                    Throw New Exception("Please input number:" + txtRecordCount.Text)
                End If
            End If

            'Table Creation
            db.sqlExecution(createTableSql)
            If Not String.IsNullOrEmpty(db.getErrorMsg) Then
                Throw New Exception("Table Creation Error:" + db.getErrorMsg)
            Else
                tblLog.Rows.Add(makeLog("Table Creation is done", 1))
            End If

            'Insert
            Dim sw As New System.Diagnostics.Stopwatch()
            Dim list As New List(Of SalesOrder)
            sw.Start()
            For i As Integer = 0 To count
                Dim d As New SalesOrder(1, "POTATO00" + i.ToString, i, New DateTime(1000, 1, 1).AddDays(i), DateTime.Now.AddDays(i), "Commentコメント" + i.ToString)
                list.Add(d)
                d.Save(conName)

                If i > 0 And i Mod 100 = 0 Then
                    sw.Stop()
                    tblLog.Rows.Add(makeLog("inserting ... " + i.ToString + "/" + count.ToString + " lap:" + sw.Elapsed.Seconds.ToString + "sec"))
                    sw.Start()
                End If

            Next
            sw.Stop()
            tblLog.Rows.Add(makeLog("Inert is done: " + sw.Elapsed.Seconds.ToString + "sec", 1))

            'Update
            sw.Reset()
            sw.Start()
            For i As Integer = 0 To list.Count - 1
                list(i).Quantity = i + 0.01
                list(i).CommentText = "Comment更新済み"
                list(i).Save(conName)

                If i > 0 And i Mod 100 = 0 Then
                    sw.Stop()
                    tblLog.Rows.Add(makeLog("updating ..." + i.ToString + "/" + count.ToString + " lap:" + sw.Elapsed.Seconds.ToString + "sec"))
                    sw.Start()
                End If
            Next
            sw.Stop()
            tblLog.Rows.Add(makeLog("Update is done: " + sw.Elapsed.Seconds.ToString + "sec", 1))

            'データ削除
            sw.Reset()
            sw.Start()
            db.sqlExecution("DELETE FROM " + TableName)
            sw.Stop()
            tblLog.Rows.Add(makeLog("Delete is done: " + sw.Elapsed.Seconds.ToString + "sec", 1))

        Catch ex As Exception

            tblLog.Rows.Add(makeLog(ex.Message, -1))
        Finally

            'テーブルドロップ
            db.sqlExecution(dropTableSql)
            If Not String.IsNullOrEmpty(db.getErrorMsg) Then
                tblLog.Rows.Add(makeLog("Drop Table Error:" + db.getErrorMsg, -1))
            Else
                tblLog.Rows.Add(makeLog("Drop Table is done", 1))
            End If

        End Try

    End Sub

    Private Function makeLog(ByVal message As String, Optional ByVal level As Integer = 0) As TableRow
        Dim row As New TableRow

        Dim cellMsg As New TableCell
        cellMsg.Text = message
        row.Cells.Add(cellMsg)

        If level < 0 Then
            row.CssClass = "danger"
        End If
        If level > 0 Then
            row.CssClass = "info"
        End If

        Return row

    End Function

    Private Function createTableSql() As String
        Dim sql As String = ""
        sql += "CREATE TABLE " + TableName + "("
        sql += " OrderNo  NUMBER(15,0) NOT NULL,"
        sql += " Material VARCHAR2(18),"
        sql += " Quantity NUMBER(15,3),"
        sql += " OrderDate   VARCHAR2(8),"
        sql += " DeliverDate DATE,"
        sql += " CommentText NVARCHAR2(30),"
        sql += " CONSTRAINT " + TableName + "_IX00 PRIMARY KEY (OrderNo) USING INDEX "
        sql += ")"

        Return sql

    End Function

    Private Function dropTableSql() As String
        Dim sql As String = "DROP TABLE " + TableName
        Return sql

    End Function

    ''' <summary>
    ''' Test Table
    ''' </summary>
    ''' <remarks></remarks>
    <DBSource(Table:=TableName)>
    Public Class SalesOrder
        Implements IDBEntity

        <DBColumn(IsKey:=True, Order:=1)>
        Public Property OrderNo As Integer = 0

        <DBColumn()>
        Public Property Material As String = ""

        <DBColumn()>
        Public Property Quantity As Decimal = 0

        <DBColumn(Format:="yyyyMMdd")>
        Public Property OrderDate As DateTime

        <DBColumn()>
        Public Property DeliverDate As DateTime

        <DBColumn()>
        Public Property CommentText As String = ""

        Public Sub New()
        End Sub

        Public Sub New(no As Integer, m As String, q As Decimal, oYmd As DateTime, dYmd As DateTime, cmt As String)
            Me.OrderNo = no
            Me.Material = m
            Me.Quantity = q
            Me.OrderDate = oYmd
            Me.DeliverDate = dYmd
            Me.CommentText = cmt
        End Sub

        Public Overrides Function ToString() As String
            Dim result = OrderNo.ToString + ":" + Material + " " + Quantity.ToString + "kg @" + OrderDate.ToString("yyyy/MM/dd") + " - " + DeliverDate.ToString("yyyy/MM/dd")
            Return result
        End Function

    End Class

End Class
