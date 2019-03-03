using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace TechPrint.Report
{
    public partial class ViewReport : System.Web.UI.Page
    {
        TechPrintEntities dbContext = new TechPrintEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            int reportID = Convert.ToInt16(Request.QueryString["ReportID"].ToString());
            if (!IsPostBack)
            {
                switch (reportID)
                {
                    case CommonConstants.REPORT_JOB_SHEET_LIST:
                        //PrintJobSheetList();
                        break;
                    default:
                        break;
                }
            }
        }

        //private void PrintJobSheetList()
        //{
        //    ReportViewer1.Visible = true;
        //    ReportViewer1.ProcessingMode = ProcessingMode.Local;
        //    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Report/PrintBill.rdlc");

        //    System.Data.SqlClient.SqlParameter[] p = new System.Data.SqlClient.SqlParameter[5];
        //    p[0] = new System.Data.SqlClient.SqlParameter("@CurrentSession", "18-19");
        //    p[1] = new System.Data.SqlClient.SqlParameter("@SearchColumn", "");
        //    p[2] = new System.Data.SqlClient.SqlParameter("@SearchText", "");
        //    p[3] = new System.Data.SqlClient.SqlParameter("@FromDate", "");
        //    p[4] = new System.Data.SqlClient.SqlParameter("@ToDate", "");

        //    System.Data.DataSet dt = ApplicationUtility.ExecuteDataSet("USP_SEARCH_QUOTATION_LIST", p);

        //    Microsoft.Reporting.WebForms.ReportDataSource RDS = new Microsoft.Reporting.WebForms.ReportDataSource("PrintBill", dt.Tables[0]);
        //    //ReportViewer1.Visible = true;
        //    ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        //    ReportViewer1.LocalReport.DataSources.Clear();
        //    //ReportViewer1.LocalReport.Refresh();
        //    //if (RDS != null)
        //    ReportViewer1.LocalReport.DataSources.Add(RDS);
        //    ReportViewer1.LocalReport.Refresh();
        //}
    }
}