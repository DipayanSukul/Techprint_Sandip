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
    public partial class PaymentSummery : System.Web.UI.Page
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
            if (data.Count > 0)
            {
                ddlClient.DataSource = data;
                ddlClient.DataTextField = "CustomerName";
                ddlClient.DataValueField = "CustomerID";
                ddlClient.DataBind();
            }
            else
            {
                ddlClient.DataSource = null;
                ddlClient.DataBind();
            }
            ddlClient.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- Select -", "0"));
        }

        private void ResetControl()
        {
            rbtnPaymentSummery.Checked = true;
            ddlClient.SelectedIndex = -1;
            txtfromDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy", provider);
            txtToDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy", provider);
            ReportViewer1.Visible = false;
        }

        protected void btnGetReport_Click(object sender, EventArgs e)
        {
            if (rbtnPaymentSummery.Checked == true)
            {
                DataTable dt = new DataTable();
                SqlParameter[] p = new SqlParameter[4];
                p[0] = new SqlParameter("@FromDate", DateTime.ParseExact(txtfromDate.Text, "dd/MM/yyyy", provider));
                p[1] = new SqlParameter("@ToDate", DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", provider));
                p[2] = new SqlParameter("@CustomerID", Convert.ToInt32(ddlClient.SelectedValue != "" ? ddlClient.SelectedValue : "0"));
                p[3] = new SqlParameter("@IsPaymentReport", true);
                dt = ApplicationUtility.ExecuteDataTable("USP_DATE_WISE_PAYMENT_SUMMERY", p);
                if (dt.Rows.Count > 0)
                {
                    ReportViewer1.Visible = true;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Report/PaymentSummery.rdlc");
                    ReportDataSource ReportDs = new ReportDataSource("PaymentSummery", dt);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(ReportDs);
                    ReportViewer1.SizeToReportContent = true;
                }
                else
                {
                    ReportViewer1.Visible = false;
                }
            }
            else
            {
                DataTable dt = new DataTable();
                SqlParameter[] p = new SqlParameter[4];
                p[0] = new SqlParameter("@FromDate", DateTime.ParseExact(txtfromDate.Text, "dd/MM/yyyy", provider));
                p[1] = new SqlParameter("@ToDate", DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", provider));
                p[2] = new SqlParameter("@CustomerID", Convert.ToInt32(ddlClient.SelectedValue != "" ? ddlClient.SelectedValue : "0"));
                p[3] = new SqlParameter("@IsPaymentReport", false);
                dt = ApplicationUtility.ExecuteDataTable("USP_DATE_WISE_PAYMENT_SUMMERY", p);
                if (dt.Rows.Count > 0)
                {
                    ReportViewer1.Visible = true;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Report/OutstandingReport.rdlc");
                    ReportDataSource ReportDs = new ReportDataSource("OutstandingReport", dt);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(ReportDs);
                    ReportViewer1.SizeToReportContent = true;
                }
                else
                {
                    ReportViewer1.Visible = false;
                }
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ResetControl();
        }
    }
}