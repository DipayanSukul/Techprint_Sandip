using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.Data.SqlClient;

namespace TechPrint.Report
{
    public partial class PrintBill : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string mode = Convert.ToString(Request.QueryString["Mode"].ToString());
                if (string.IsNullOrEmpty(mode) == false)
                {
                    if (mode == "1")
                    {
                        Int64 QuotationID = Convert.ToInt64(Request.QueryString["ID"].ToString());

                        SqlParameter[] p = new SqlParameter[3];
                        p[0] = new SqlParameter("@QuotationID", QuotationID);
                        p[1] = new SqlParameter("@QuotationDate", System.DateTime.Now); // Useless but don't remove
                        p[2] = new SqlParameter("@OpMode", 5);
                        DataTable dt = ApplicationUtility.ExecuteDataTable("USP_MANAGE_JOB_SHEET", p);
                        if (dt.Rows.Count > 0)
                        {
                            ReportViewer1.Visible = true;
                            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Report/PrintBill.rdlc");
                            ReportDataSource ReportDs = new ReportDataSource("BillPrint", dt);
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(ReportDs);
                            ReportViewer1.SizeToReportContent = true;
                        }
                    }
                    else if (mode == "2")
                    {
                        int paymentID = Convert.ToInt16(Request.QueryString["ID"].ToString());

                        SqlParameter[] p = new SqlParameter[1];
                        p[0] = new SqlParameter("@PaymentID", paymentID);

                        DataTable dt = ApplicationUtility.ExecuteDataTable("USP_PaymentReceiptPrint", p);
                        if (dt.Rows.Count > 0)
                        {
                            ReportViewer1.Visible = true;
                            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Report/PaymentReceiptPrint.rdlc");
                            ReportDataSource ReportDs = new ReportDataSource("PaymentReceiptPrint", dt);
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(ReportDs);
                            ReportViewer1.SizeToReportContent = true;
                        }
                    }
                }
            }
        }
    }
}