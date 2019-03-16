using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TechPrint.Transaction
{
    public partial class Payment : System.Web.UI.Page
    {
        TechPrintEntities _dbContext = new TechPrintEntities();
        CultureInfo provider = CultureInfo.InvariantCulture;
        private string _finYear = ApplicationUtility.CurrentSession().ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillClientList();
                FillPaymentMode();
                ResetControl();
                if (Request.QueryString["ID"] != null)
                {
                    lblPaymentID.Text = Request.QueryString["ID"].ToString();
                    GetPaymentDetail();
                }
            }
        }

        private void ResetControl()
        {
            lblPaymentID.Text = "0";
            var count = (from o in _dbContext.Payments
                         where o.FinYear == _finYear
                         select o).Count() + 1;
            if (count > 0)
            {
                txtPaymentEntryNo.Text = "P/" + ApplicationUtility.CurrentSession() + "/" + count.ToString("00000");
            }
            ddlClient.SelectedIndex = -1;
            ddlClient_SelectedIndexChanged(ddlClient, EventArgs.Empty);
            ddlPaymentMode.SelectedIndex = -1;
            txtDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy", provider);
            txtOutstanding.Text = "0.00";
            txtPaidAmount.Text = "0.00";
            txtPaymentDetail.Text = String.Empty;
            btnSave.Text = "Save";
        }

        private void FillClientList()
        {
            var data = (from p in _dbContext.Customers
                        where p.RecMode != "D"
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
            ddlClient.Items.Insert(0, new ListItem("- Select -", "0"));
        }

        private void FillPaymentMode()
        {
            var data = (from p in _dbContext.PaymentModes
                        orderby p.PMode ascending
                        select  (p)).ToList();
            if (data.Count > 0)
            {
                ddlPaymentMode.DataSource = data;
                ddlPaymentMode.DataTextField = "PMode";
                ddlPaymentMode.DataValueField = "PaymentModeID";
                ddlPaymentMode.DataBind();
            }
            else
            {
                ddlPaymentMode.DataSource = null;
                ddlPaymentMode.DataBind();
            }
        }

        private void GetPaymentDetail()
        {
            Int32 PaymentID = Convert.ToInt32(lblPaymentID.Text.Trim() != "" ? lblPaymentID.Text.Trim() : "0");
            var GetData = _dbContext.Payments.Where(p => p.PaymentID == PaymentID).Single();
            txtPaymentEntryNo.Text = GetData.PaymentNumber;
            if (GetData.PaymentDate != null)
            {
                DateTime PaymentDate = Convert.ToDateTime(GetData.PaymentDate);
                txtDate.Text = PaymentDate.ToString("dd/MM/yyyy");
            }

            ddlClient.SelectedValue = GetData.CustomerID.ToString();
            ddlClient_SelectedIndexChanged(ddlClient, EventArgs.Empty);
            Decimal PreDue = Convert.ToDecimal(txtOutstanding.Text.Trim() != "" ? txtOutstanding.Text.Trim() : "0");
            PreDue += Convert.ToDecimal(GetData.PaidAmount.ToString());
            txtOutstanding.Text = PreDue.ToString("0.00");
            txtPaidAmount.Text = GetData.PaidAmount.ToString();
            ddlPaymentMode.SelectedValue = GetData.PaymentMode.ToString();
            txtPaymentDetail.Text = GetData.PaymentDetail;
            btnSave.Text = "Update";
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Transaction/PaymentList.aspx", false);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ResetControl();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            String message = String.Empty;
            try
            {
                if (btnSave.Text == "Save")
                {
                    var data = _dbContext.USP_PAYMENT(0, DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", provider)
                        , Convert.ToInt32(ddlClient.SelectedValue != "" ? ddlClient.SelectedValue : "0")
                        , Convert.ToInt32(ddlPaymentMode.SelectedValue != "" ? ddlPaymentMode.SelectedValue : "0")
                        , Convert.ToDecimal(txtPaidAmount.Text.Trim() != "" ? txtPaidAmount.Text.Trim() : "0")
                        , txtPaymentDetail.Text
                        , Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString())
                        , ApplicationUtility.CurrentSession()
                        , 1, 0);
                    message = "alert('Payment saved sucessfully.')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    ResetControl();                    
                }
                else if (btnSave.Text == "Update")
                {
                    var data = _dbContext.USP_PAYMENT(Convert.ToInt32(lblPaymentID.Text.Trim() != "" ? lblPaymentID.Text.Trim() : "0")
                        , DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", provider)
                        , Convert.ToInt32(ddlClient.SelectedValue != "" ? ddlClient.SelectedValue : "0")
                        , Convert.ToInt32(ddlPaymentMode.SelectedValue != "" ? ddlPaymentMode.SelectedValue : "0")
                        , Convert.ToDecimal(txtPaidAmount.Text.Trim() != "" ? txtPaidAmount.Text.Trim() : "0")
                        , txtPaymentDetail.Text.ToUpper()
                        , Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString())
                        , ApplicationUtility.CurrentSession()
                        , 2, 0);
                    message = "alert('Payment note updated sucessfully')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    ResetControl();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", ex.ToString(), true);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            String message = String.Empty;
            if (lblPaymentID.Text == "0")
            {
                message = "alert('No Payment Note Found')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
            }
            else
            {
                try
                {
                    var data = _dbContext.USP_PAYMENT(Convert.ToInt32(lblPaymentID.Text.Trim() != "" ? lblPaymentID.Text.Trim() : "0")
                        , DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", provider)
                        , Convert.ToInt32(ddlClient.SelectedValue != "" ? ddlClient.SelectedValue : "0")
                        , Convert.ToInt32(ddlPaymentMode.SelectedValue != "" ? ddlPaymentMode.SelectedValue : "0")
                        , Convert.ToDecimal(txtPaidAmount.Text.Trim() != "" ? txtPaidAmount.Text.Trim() : "0")
                        , txtPaymentDetail.Text
                        , Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString())
                        , ApplicationUtility.CurrentSession()
                        , 3, 0);
                    message = "alert('Payment note deleted sucessfully')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    ResetControl();

                }
                catch (Exception ex)
                {
                    message = "alert('" + ex.ToString() + "')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                }
            }
        }

        protected void ddlClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlClient.SelectedIndex > -1)
            {
                int clientID = Convert.ToInt32(ddlClient.SelectedValue.ToString());
                if (clientID > 0)
                {
                    var GetData = _dbContext.QuotationJobSheets.Where(p => p.CustomerID == clientID).Sum(s => s.OutstandingAmount);
                    txtOutstanding.Text = Convert.ToDecimal(GetData).ToString("0.00");
                }
                else
                {
                    txtOutstanding.Text = "0.00";
                }
            }
        }
    }
}