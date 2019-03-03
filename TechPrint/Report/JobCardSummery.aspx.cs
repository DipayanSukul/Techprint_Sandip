using System;
using System.Linq;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.Data.SqlClient;

namespace TechPrint.Report
{
    public partial class JobCardSummery : System.Web.UI.Page
    {
        TechPrintEntities _dbContext = new TechPrintEntities();
        CultureInfo provider = CultureInfo.InvariantCulture;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ResetControl();
                FillClientList();
            }
        }
        private void FillClientList()
        {
            var data = (from p in _dbContext.Customers
                        orderby p.CustomerName ascending
                        select new { CustomerName = p.CustomerName.ToUpper(), CustomerID = p.CustomerID }).ToList();

            ddlClient.DataSource = data;
            ddlClient.DataTextField = "CustomerName";
            ddlClient.DataValueField = "CustomerID";
            ddlClient.DataBind();

            ddlClient.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- Select -", "0"));
        }

        private void ResetControl()
        {
            ddlClient.SelectedIndex = -1;
            txtfromDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy", provider);
            txtToDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy", provider);
            ReportViewer1.Visible = false;
        }

        protected void btnGetReport_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlParameter[] p = new SqlParameter[3];
            p[0] = new SqlParameter("@FromDate", DateTime.ParseExact(txtfromDate.Text, "dd/MM/yyyy", provider));
            p[1] = new SqlParameter("@ToDate", DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", provider));
            p[2] = new SqlParameter("@CustomerID", Convert.ToInt32(ddlClient.SelectedValue != "" ? ddlClient.SelectedValue : "0"));
            dt = ApplicationUtility.ExecuteDataTable("USP_DATE_WISE_JOB_SUMMERY", p);
            if (dt.Rows.Count > 0)
            {
                ReportViewer1.Visible = true;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Report/JobSummeryReport.rdlc");
                ReportDataSource ReportDs = new ReportDataSource("JobCardReport", dt);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(ReportDs);
                ReportViewer1.SizeToReportContent = true;
            }
            else
            {
                ReportViewer1.Visible = false;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ResetControl();
        }
    }
}