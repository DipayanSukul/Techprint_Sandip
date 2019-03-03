using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TechPrint.Transaction
{
    public partial class CustomerMaster : System.Web.UI.Page
    {
        TechPrintEntities _dbContext = new TechPrintEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ResetControl();
            }
        }
        private void ResetControl()
        {
            FillGridView();
            lblCustomerId.Text = "0";
            txtName.Text = String.Empty;
            txtAddress.Text = String.Empty;
            txttelephone.Text = String.Empty;
            txtEmail.Text = String.Empty;
            txtGSTNo.Text = String.Empty;
            btnSave.Text = "Save";
        }
        private void FillGridView()
        {
            try
            {
                if (string.IsNullOrEmpty(txtSearch.Text) == false)
                {
                    var data = (from C in _dbContext.Customers
                                where C.RecMode != "D"
                                && (C.CustomerName.StartsWith(txtSearch.Text) ||
                                C.Telephone.StartsWith(txtSearch.Text))
                                orderby C.CustomerName
                                select (C)).ToList();
                    //_dbContext.Customers.Where(p => p.RecMode != "D").ToList();
                    gvCustomer.DataSource = data;
                    gvCustomer.DataBind();
                }
                else
                {
                    var data = (from C in _dbContext.Customers
                                where C.RecMode != "D"
                                orderby C.CustomerName
                                select (C)).ToList();
                    //_dbContext.Customers.Where(p => p.RecMode != "D").ToList();
                    gvCustomer.DataSource = data;
                    gvCustomer.DataBind();
                }
            }

            catch (Exception ex)
            {
                gvCustomer.DataSource = null;
                gvCustomer.DataBind();
            }
        }
        protected void gvCustomer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                int CustomerID = Convert.ToInt32(e.CommandArgument.ToString());
                var GetData = _dbContext.Customers.Where(p => p.CustomerID == CustomerID).Single();
                lblCustomerId.Text = CustomerID.ToString();
                txtName.Text = GetData.CustomerName;
                txtAddress.Text = GetData.Address.ToString();
                txttelephone.Text = GetData.Telephone.ToString();
                txtEmail.Text = GetData.Email.ToString();
                txtGSTNo.Text = GetData.GSTNo.ToString();
                btnSave.Text = "Update";
            }
        }

        protected void gvCustomer_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void gvCustomer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCustomer.PageIndex = e.NewPageIndex;
            FillGridView();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ResetControl();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            String message = String.Empty;
            if (lblCustomerId.Text == "0")
            {
                message = "alert('No Customer Found')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
            }
            else
            {
                int CustomerID = Convert.ToInt32(lblCustomerId.Text.Trim() != "" ? lblCustomerId.Text.Trim() : "0");
                if (!IsMasterDataUse(CustomerID))
                {
                    var data = _dbContext.Customers.Where(p => p.CustomerID == CustomerID).SingleOrDefault();
                    data.RecMode = "D";
                    data.AddEditBy = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]);
                    data.AddEditOn = System.DateTime.Now;
                    _dbContext.SaveChanges();
                    message = "alert('Customer Deleted')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    ResetControl();
                }
                else
                {
                    message = "alert('SORRY! This Customer already use in Transaction so you cannot delete this.')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                }
            }
        }

        private Boolean IsMasterDataUse(int CustomerID)
        {
            Boolean InUse = false;
            try
            {
                var GetData = _dbContext.QuotationJobSheets.Where(p => p.CustomerID == CustomerID && p.RecMode != "D").SingleOrDefault();
                if (GetData != null)
                {
                    InUse = true;
                }
            }
            catch
            {
                // Nothing
            }
            return InUse;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            String message = String.Empty;
            try
            {
                if (btnSave.Text == "Save")
                {
                    var data = _dbContext.Customers.Where(p => p.CustomerName == txtName.Text.Trim() && p.Telephone == txttelephone.Text.Trim() && p.GSTNo == txtGSTNo.Text.Trim()).ToList();
                    if (data.Any())
                    {
                        message = "alert('This Customer already exists')";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    }
                    else
                    {
                        Customer para = new Customer();
                        para.CustomerName = txtName.Text.Trim().ToUpper();
                        para.Address = txtAddress.Text.Trim().ToUpper();
                        para.Telephone = txttelephone.Text.Trim().ToUpper();
                        para.Email = txtEmail.Text.Trim().ToUpper().ToLower();
                        para.GSTNo = txtGSTNo.Text.Trim().ToUpper().ToUpper();
                        para.RecMode = "A";
                        para.AddEditBy = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
                        para.AddEditOn = System.DateTime.Now;
                        _dbContext.Customers.Add(para);
                        _dbContext.SaveChanges();
                        message = "alert('This Customer record saved sucessfully')";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                        ResetControl();
                    }
                }
                else if (btnSave.Text == "Update")
                {
                    int CustomerID = Convert.ToInt32(lblCustomerId.Text.Trim() != "" ? lblCustomerId.Text.Trim() : "0");
                    var q = _dbContext.Customers.Where(p => p.CustomerName == txtName.Text && p.GSTNo == txtGSTNo.Text.Trim() && p.CustomerID != CustomerID).ToList();
                    if (q.Any())
                    {
                        message = "alert('This Customer Data already exists')";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    }
                    else
                    {
                        var data = _dbContext.Customers.Where(p => p.CustomerID == CustomerID).SingleOrDefault();
                        data.CustomerName = txtName.Text.Trim().ToUpper();
                        data.Address = txtAddress.Text.Trim().ToUpper();
                        data.Telephone = txttelephone.Text.Trim();
                        data.Email = txtEmail.Text.Trim().ToLower();
                        data.GSTNo = txtGSTNo.Text.Trim().ToUpper();
                        data.RecMode = "E";
                        data.AddEditBy = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
                        data.AddEditOn = System.DateTime.Now;
                        _dbContext.SaveChanges();
                        message = "alert('This Customer Data updated sucessfully')";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                        ResetControl();
                    }
                }
            }
            catch (Exception ex)
            {
                message = "alert('" + ex.ToString() + "')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            FillGridView();
        }
    }
}