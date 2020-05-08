using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

/// <summary>
/// Summary description for XtraReport1
/// </summary>
public class XtraReport1 : DevExpress.XtraReports.UI.XtraReport
{
    private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
    private TopMarginBand TopMargin;
    private BottomMarginBand BottomMargin;
    private DetailBand Detail;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public XtraReport1()
    {
        InitializeComponent();
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        DevExpress.DataAccess.Sql.SelectQuery selectQuery1 = new DevExpress.DataAccess.Sql.SelectQuery();
        DevExpress.DataAccess.Sql.Column column1 = new DevExpress.DataAccess.Sql.Column();
        DevExpress.DataAccess.Sql.ColumnExpression columnExpression1 = new DevExpress.DataAccess.Sql.ColumnExpression();
        DevExpress.DataAccess.Sql.Table table1 = new DevExpress.DataAccess.Sql.Table();
        DevExpress.DataAccess.Sql.Column column2 = new DevExpress.DataAccess.Sql.Column();
        DevExpress.DataAccess.Sql.ColumnExpression columnExpression2 = new DevExpress.DataAccess.Sql.ColumnExpression();
        this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
        this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
        this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
        this.Detail = new DevExpress.XtraReports.UI.DetailBand();
        ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
        // 
        // sqlDataSource1
        // 
        this.sqlDataSource1.ConnectionName = "sql";
        this.sqlDataSource1.Name = "sqlDataSource1";
        columnExpression1.ColumnName = "CustomerId";
        table1.Name = "Customers";
        columnExpression1.Table = table1;
        column1.Expression = columnExpression1;
        columnExpression2.ColumnName = "Name";
        columnExpression2.Table = table1;
        column2.Expression = columnExpression2;
        selectQuery1.Columns.Add(column1);
        selectQuery1.Columns.Add(column2);
        selectQuery1.Name = "Customers";
        selectQuery1.Tables.Add(table1);
        this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            selectQuery1});
        this.sqlDataSource1.ResultSchemaSerializable = "PERhdGFTZXQgTmFtZT0ic3FsRGF0YVNvdXJjZTEiPjxWaWV3IE5hbWU9IkN1c3RvbWVycyI+PEZpZWxkI" +
"E5hbWU9IkN1c3RvbWVySWQiIFR5cGU9IkludDMyIiAvPjxGaWVsZCBOYW1lPSJOYW1lIiBUeXBlPSJTd" +
"HJpbmciIC8+PC9WaWV3PjwvRGF0YVNldD4=";
        // 
        // TopMargin
        // 
        this.TopMargin.Name = "TopMargin";
        // 
        // BottomMargin
        // 
        this.BottomMargin.Name = "BottomMargin";
        // 
        // Detail
        // 
        this.Detail.Name = "Detail";
        // 
        // XtraReport1
        // 
        this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.Detail});
        this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
        this.DataMember = "Customers";
        this.DataSource = this.sqlDataSource1;
        this.Font = new System.Drawing.Font("Arial", 9.75F);
        this.Version = "20.1";
        ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}
