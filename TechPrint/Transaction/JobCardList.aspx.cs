using Microsoft.Reporting.WebForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TechPrint.Transaction
{
    public partial class JobCardList : System.Web.UI.Page
    {
        TechPrintEntities _dbContext = new TechPrintEntities();
        CultureInfo provider = CultureInfo.InvariantCulture;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var data = (from p in _dbContext.QuotationJobSheets
                            select new { FinYear = p.FinYear.ToUpper() }).ToList();
                if (data.Count > 0)
                {
                    ddlFinancialYear.DataSource = data;
                    ddlFinancialYear.DataTextField = "FinYear";
                    ddlFinancialYear.DataValueField = "FinYear";
                    ddlFinancialYear.DataBind();
                }
                else
                {
                    ddlFinancialYear.DataSource = null;
                    ddlFinancialYear.DataBind();
                    ddlFinancialYear.Items.Insert(0, new ListItem(ApplicationUtility.CurrentSession(), ApplicationUtility.CurrentSession()));
                }
                ResetControl();
            }
        }

        private void ResetControl()
        {
            txtFromDate.Visible = false;
            txtToDate.Visible = false;
            btnCalenderPopup1.Visible = false;
            btnCalenderPopup2.Visible = false;
            txtSearchText.Visible = true;
            ddl_Search_SelectedIndexChanged(ddl_Search, EventArgs.Empty);
            BindQuotationList();
        }

        public void BindQuotationList()
        {
            SqlParameter[] p = new SqlParameter[5];
            p[0] = new SqlParameter("@CurrentSession", ddlFinancialYear.SelectedItem.Text);
            p[1] = new SqlParameter("@SearchColumn", ddl_Search.SelectedItem.Text);
            p[2] = new SqlParameter("@SearchText", txtSearchText.Text);
            p[3] = new SqlParameter("@FromDate", txtFromDate.Text);
            p[4] = new SqlParameter("@ToDate", txtToDate.Text);

            DataTable dt = ApplicationUtility.ExecuteDataTable("USP_SEARCH_QUOTATION_LIST", p);
            if (dt.Rows.Count > 0)
            {
                gvQuotationList.DataSource = dt;
                gvQuotationList.DataBind();
            }
            else
            {
                dt.Rows.Add(dt.NewRow());
                gvQuotationList.DataSource = dt;
                gvQuotationList.DataBind();
                int columncount = gvQuotationList.Rows[0].Cells.Count;
                gvQuotationList.Rows[0].Cells.Clear();
                gvQuotationList.Rows[0].Cells.Add(new TableCell());
                gvQuotationList.Rows[0].Cells[0].ColumnSpan = columncount;
                gvQuotationList.Rows[0].Cells[0].Text = "No Records Found";
                dt.Clear();
            }
        }

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            BindQuotationList();
        }

        protected void gvQuotationList_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            gvQuotationList.PageIndex = e.NewPageIndex;
            BindQuotationList();
        }

        protected void gvQuotationList_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {

        }

        protected void gvQuotationList_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            Int32 QuotationID = 0;
            if (e.CommandName == "Edit")
            {
                QuotationID = Convert.ToInt32(e.CommandArgument.ToString());
                var GetData = _dbContext.QuotationJobSheets.Where(p => p.QuotationID == QuotationID && p.AmountPaid > 0).FirstOrDefault();
                if (GetData == null)
                {
                    Response.Redirect("~/Transaction/JobCard.aspx?ID=" + QuotationID);
                }
                else
                {
                    String message;
                    message = "alert('You cannot Edit/Delete entry because Full/Part Payment already made this entry note.')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                }
            }
            else if (e.CommandName == "Download")
            {
                QuotationID = Convert.ToInt32(e.CommandArgument.ToString());
                StringBuilder sb = new StringBuilder();
                sb.Append("window.open('../Report/PrintBill.aspx?Mode=1&ID=" + QuotationID.ToString() + "','NewWindow','left=50, top=10, height=650, width= 950, status=yes, resizable= no, scrollbars= yes, toolbar= no,location= no, menubar= no')");
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "dialog", sb.ToString(), true);

                //DataTable dt = new DataTable();
                //SqlParameter[] p = new SqlParameter[3];
                //p[0] = new SqlParameter("@QuotationID", QuotationID);
                //p[1] = new SqlParameter("@QuotationDate", System.DateTime.Now); // Useless but don't remove
                //p[2] = new SqlParameter("@OpMode", 5);
                //dt = ApplicationUtility.ExecuteDataTable("USP_MANAGE_JOB_SHEET", p);

                //if (dt.Rows.Count > 0)
                //{
                //    // Variables
                //    Warning[] warnings;
                //    string[] streamIds;
                //    string mimeType = string.Empty;
                //    string encoding = string.Empty;
                //    string extension = string.Empty;

                //     // Create Report DataSource
                //    ReportDataSource rds = new ReportDataSource("PrintBill", dt);

                //    //// Setup the report viewer object and get the array of bytes
                //    //ReportViewer viewer = new ReportViewer();
                //    //viewer.LocalReport.Refresh();
                //    //viewer.Reset();
                //    //viewer.LocalReport.EnableExternalImages = true;
                //    //viewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                //    //ReportDataSource rds2 = new ReportDataSource("PrintBill", dt);
                //    //viewer.LocalReport.DataSources.Add(rds2);
                //    //viewer.ShowParameterPrompts = false;
                //    //viewer.LocalReport.ReportPath = "~/Report/PrintBill.rdlc";
                //    //byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                //    //Response.Buffer = true;
                //    //Response.Clear();
                //    //Response.ContentType = mimeType;
                //    //Response.AddHeader("content-disposition", "attachment; filename=" + dt.Rows[0]["QuotationNO"].ToString() + "." + extension);
                //    //try
                //    //{
                //    //    Response.BinaryWrite(bytes);
                //    //}
                //    //catch (Exception ex)
                //    //{
                //    //    Page.ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('Error while generating PDF.');", true);
                //    //    Console.WriteLine(ex.StackTrace);
                //    //}
                //    //Response.Flush();
                //    //viewer.LocalReport.Refresh();

                //    ReportViewer viewer = new ReportViewer();
                //    viewer.ProcessingMode = ProcessingMode.Local;
                //    viewer.LocalReport.ReportPath = "~/Report/PrintBill.rdlc";
                //    viewer.LocalReport.DataSources.Add(rds); // Add datasource here

                //    byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

                //    // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
                //    Response.Buffer = true;
                //    Response.Clear();
                //    Response.ContentType = mimeType;
                //    Response.AddHeader("content-disposition", "attachment; filename=" + dt.Rows[0]["QuotationNO"].ToString() + "." + extension);
                //    Response.BinaryWrite(bytes); // create the file
                //    Response.Flush(); // send it to the client to download
                //}
            }
        }

        protected void ddl_Search_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_Search.SelectedItem.Text == "DATE")
            {
                txtSearchText.Text = String.Empty;
                txtFromDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy", provider);
                txtToDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy", provider);
                txtFromDate.Visible = true;
                txtToDate.Visible = true;
                btnCalenderPopup1.Visible = true;
                btnCalenderPopup2.Visible = true;
                txtSearchText.Visible = false;
            }
            else
            {
                txtSearchText.Text = String.Empty;
                txtFromDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy", provider);
                txtToDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy", provider);
                txtFromDate.Visible = false;
                txtToDate.Visible = false;
                btnCalenderPopup1.Visible = false;
                btnCalenderPopup2.Visible = false;
                txtSearchText.Visible = true;
            }
        }
    }
}