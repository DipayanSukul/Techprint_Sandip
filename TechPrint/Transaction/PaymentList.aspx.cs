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
    public partial class PaymentList : System.Web.UI.Page
    {
        TechPrintEntities _dbContext = new TechPrintEntities();
        CultureInfo provider = CultureInfo.InvariantCulture;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var data = (from p in _dbContext.Payments
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
            BindList();
        }

        public void BindList()
        {
            DataTable dt = new DataTable();
            SqlParameter[] p = new SqlParameter[5];
            p[0] = new SqlParameter("@CurrentSession", ddlFinancialYear.SelectedItem.Text);
            p[1] = new SqlParameter("@SearchColumn", ddl_Search.SelectedItem.Text);
            p[2] = new SqlParameter("@SearchText", txtSearchText.Text);
            p[3] = new SqlParameter("@FromDate", txtFromDate.Text);
            p[4] = new SqlParameter("@ToDate", txtToDate.Text);

            dt = ApplicationUtility.ExecuteDataTable("USP_SEARCH_PAYMENT_LIST", p);

            if (dt.Rows.Count > 0)
            {
                gvPaymentList.DataSource = dt;
                gvPaymentList.DataBind();
            }
            else
            {
                dt.Rows.Add(dt.NewRow());
                gvPaymentList.DataSource = dt;
                gvPaymentList.DataBind();
                int columncount = gvPaymentList.Rows[0].Cells.Count;
                gvPaymentList.Rows[0].Cells.Clear();
                gvPaymentList.Rows[0].Cells.Add(new TableCell());
                gvPaymentList.Rows[0].Cells[0].ColumnSpan = columncount;
                gvPaymentList.Rows[0].Cells[0].Text = "No Records Found";
                dt.Clear();
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

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            BindList();
        }

        protected void gvPaymentList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                Int32 PaymentID = Convert.ToInt32(e.CommandArgument.ToString());
                Response.Redirect("~/Transaction/Payment.aspx?ID=" + PaymentID);
            }
            else if (e.CommandName == "Download")
            {
                Int32 paymentID = Convert.ToInt32(e.CommandArgument.ToString());
                StringBuilder sb = new StringBuilder();
                sb.Append("window.open('../Report/PrintBill.aspx?Mode=2&ID=" + paymentID.ToString() + "','NewWindow','left=50, top=10, height=650, width= 950, status=yes, resizable= no, scrollbars= yes, toolbar= no,location= no, menubar= no')");
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "dialog", sb.ToString(), true);
            }
        }

        protected void gvPaymentList_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void gvPaymentList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPaymentList.PageIndex = e.NewPageIndex;
            BindList();
        }
    }
}