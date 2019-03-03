using MyUtil.CommonMethod;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility = TechPrint.ApplicationUtility;

namespace TechPrint.Transaction
{
    public partial class JobCard : Page
    {
        TechPrintEntities _dbContext = new TechPrintEntities();
        CultureInfo provider = CultureInfo.InvariantCulture;
        private string _finYear = ApplicationUtility.CurrentSession().ToString();
        private int QuotationID = 0;
        private int _MasterTypeID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillTaxList();
                FillClientList();
                FillPaymentMode();

                ApplicationUtility.FillMasterDataDropDownlist(ddlItem, MasterType.itemDescription);
                ResetControl();
                if (Request.QueryString["ID"] != null)
                {
                    QuotationID = Convert.ToInt32(Request.QueryString["ID"].ToString());
                    GetQuotationSheetDetail(QuotationID);
                }
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "text", "GSTCalculation()", true);
        }

        #region Paper Detail
        private void SetInitialPaperDetailRow()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("NoofSheet", typeof(Int32)));
            dt.Columns.Add(new DataColumn("PaperSizeID", typeof(Int32)));
            dt.Columns.Add(new DataColumn("PaperLength", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("Paperwidth", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("GSMID", typeof(Int32)));
            dt.Columns.Add(new DataColumn("GSMValue", typeof(Int32)));
            dt.Columns.Add(new DataColumn("PaperTypeID", typeof(Int32)));
            dt.Columns.Add(new DataColumn("PaperManufaturerID", typeof(String)));
            dt.Columns.Add(new DataColumn("LineRate", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("LineAmount", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("LineGST", typeof(Decimal)));

            dr = dt.NewRow();
            dr["NoofSheet"] = 0;
            dr["PaperSizeID"] = 0;
            dr["PaperLength"] = 0;
            dr["Paperwidth"] = 0;
            dr["GSMID"] = 0;
            dr["GSMValue"] = 0;
            dr["PaperTypeID"] = 0;
            dr["PaperManufaturerID"] = 0;
            dr["LineRate"] = 0;
            dr["LineAmount"] = 0;
            dr["LineGST"] = 0;
            dt.Rows.Add(dr);

            gvPaperDetail.DataSource = dt;
            gvPaperDetail.DataBind();

            DropDownList ddlPaperSize = gvPaperDetail.Rows[0].FindControl("ddlPaperSize") as DropDownList;
            Utility.FillMasterDataDropDownlist(ddlPaperSize, MasterType.paperSize);

            DropDownList ddlGSM = gvPaperDetail.Rows[0].FindControl("ddlGSM") as DropDownList;
            Utility.FillMasterDataDropDownlist(ddlGSM, MasterType.gsm);

            DropDownList ddlPaperType = gvPaperDetail.Rows[0].FindControl("ddlPaperType") as DropDownList;
            Utility.FillMasterDataDropDownlist(ddlPaperType, MasterType.paperType);

            DropDownList ddlPaperManufature = gvPaperDetail.Rows[0].FindControl("ddlPaperManufature") as DropDownList;
            Utility.FillMasterDataDropDownlist(ddlPaperManufature, MasterType.paperCompany);

            ImageButton itbnNew = gvPaperDetail.Rows[0].FindControl("ibtnAddPaperCosting") as ImageButton;
            ImageButton itbndelete = gvPaperDetail.Rows[0].FindControl("ibtnDeletePaperCosting") as ImageButton;

            itbnNew.Enabled = true;
            itbndelete.Enabled = true;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GSTCalculation()", true);
        }

        protected void ibtnAddPaperCosting_Click(object sender, ImageClickEventArgs e)
        {
            String message;
            if (gvPrintingDetail.Rows.Count > 0)
            {
                message = "alert('Sorry! printing detail already populate based on paper detail.')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
            }
            else
            {
                DataTable dt = new DataTable();
                DataRow dr = null;
                //Define the Columns
                dt.Columns.Add(new DataColumn("NoofSheet", typeof(Int32)));
                dt.Columns.Add(new DataColumn("PaperSizeID", typeof(Int32)));
                dt.Columns.Add(new DataColumn("PaperLength", typeof(Decimal)));
                dt.Columns.Add(new DataColumn("Paperwidth", typeof(Decimal)));
                dt.Columns.Add(new DataColumn("GSMID", typeof(Int32)));
                dt.Columns.Add(new DataColumn("GSMValue", typeof(Int32)));
                dt.Columns.Add(new DataColumn("PaperTypeID", typeof(Int32)));
                dt.Columns.Add(new DataColumn("PaperManufaturerID", typeof(String)));
                dt.Columns.Add(new DataColumn("LineRate", typeof(Decimal)));
                dt.Columns.Add(new DataColumn("LineAmount", typeof(Decimal)));
                dt.Columns.Add(new DataColumn("LineGST", typeof(Decimal)));

                foreach (GridViewRow row in gvPaperDetail.Rows)
                {
                    TextBox txtNoofSheet = row.FindControl("txtNoofSheet") as TextBox;
                    DropDownList ddlPaperSize = row.FindControl("ddlPaperSize") as DropDownList;
                    TextBox txtPaperLength = row.FindControl("txtPaperLength") as TextBox;
                    TextBox txtPaperwidth = row.FindControl("txtPaperwidth") as TextBox;
                    DropDownList ddlGSM = row.FindControl("ddlGSM") as DropDownList;
                    TextBox txtGSMValue = row.FindControl("txtGSMValue") as TextBox;
                    DropDownList ddlPaperType = row.FindControl("ddlPaperType") as DropDownList;
                    DropDownList ddlPaperManufature = row.FindControl("ddlPaperManufature") as DropDownList;
                    TextBox txtPaperRate = row.FindControl("txtPaperRate") as TextBox;
                    TextBox txtAmount = row.FindControl("txtAmount") as TextBox;
                    TextBox txtPaperDetailGST = row.FindControl("txtPaperDetailGST") as TextBox;

                    dr = dt.NewRow();

                    dr["NoOfSheet"] = txtNoofSheet.Text.Trim() != "" ? txtNoofSheet.Text.Trim() : "0";
                    dr["PaperSizeID"] = ddlPaperSize.SelectedValue != "" ? ddlPaperSize.SelectedValue : "0";
                    dr["PaperLength"] = txtPaperLength.Text.Trim() != "" ? txtPaperLength.Text.Trim() : "0";
                    dr["PaperWidth"] = txtPaperwidth.Text.Trim() != "" ? txtPaperwidth.Text.Trim() : "0";
                    dr["GSMID"] = ddlGSM.SelectedValue != "" ? ddlGSM.SelectedValue : "0";
                    dr["GSMValue"] = txtGSMValue.Text.Trim() != "" ? txtGSMValue.Text.Trim() : "0";
                    dr["PaperTypeID"] = ddlPaperType.SelectedValue != "" ? ddlPaperType.SelectedValue : "0";
                    dr["PaperManufaturerID"] = ddlPaperManufature.SelectedValue != "" ? ddlPaperManufature.SelectedValue : "0";
                    dr["LineRate"] = txtPaperRate.Text.Trim() != "" ? txtPaperRate.Text.Trim() : "0";
                    dr["LineAmount"] = txtAmount.Text.Trim() != "" ? txtAmount.Text.Trim() : "0";
                    dr["LineGST"] = txtPaperDetailGST.Text.Trim() != "" ? txtPaperDetailGST.Text.Trim() : "0";
                    dt.Rows.Add(dr);
                }
                dt.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                gvPaperDetail.DataSource = dt;
                gvPaperDetail.DataBind();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DropDownList ddlPaperSize = gvPaperDetail.Rows[i].FindControl("ddlPaperSize") as DropDownList;
                    Utility.FillMasterDataDropDownlist(ddlPaperSize, MasterType.paperSize);
                    ddlPaperSize.SelectedValue = dt.Rows[i]["PaperSizeID"].ToString();

                    DropDownList ddlGSM = gvPaperDetail.Rows[i].FindControl("ddlGSM") as DropDownList;
                    Utility.FillMasterDataDropDownlist(ddlGSM, MasterType.gsm);
                    ddlGSM.SelectedValue = dt.Rows[i]["GSMID"].ToString();

                    DropDownList ddlPaperType = gvPaperDetail.Rows[i].FindControl("ddlPaperType") as DropDownList;
                    Utility.FillMasterDataDropDownlist(ddlPaperType, MasterType.paperType);
                    ddlPaperType.SelectedValue = dt.Rows[i]["PaperTypeID"].ToString();

                    DropDownList ddlPaperManufature = gvPaperDetail.Rows[i].FindControl("ddlPaperManufature") as DropDownList;
                    Utility.FillMasterDataDropDownlist(ddlPaperManufature, MasterType.paperCompany);
                    ddlPaperManufature.SelectedValue = dt.Rows[i]["PaperManufaturerID"].ToString();
                }
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GSTCalculation()", true);
            }
        }

        protected void gvPaperDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            String message;
            if (gvPaperDetail.Rows.Count == 1)
            {
                message = "alert('Last row cannot deleteted')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                //Do Nothing
            }
            else
            {
                if (gvPrintingDetail.Rows.Count > 0)
                {
                    message = "alert('Sorry! printing detail already populate based on paper detail.')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                }
                else
                {
                    int index = Convert.ToInt32(e.RowIndex);
                    DataTable dt = new DataTable();
                    DataRow dr = null;
                    //Define the Columns
                    dt.Columns.Add(new DataColumn("NoofSheet", typeof(Int32)));
                    dt.Columns.Add(new DataColumn("PaperSizeID", typeof(Int32)));
                    dt.Columns.Add(new DataColumn("PaperLength", typeof(Decimal)));
                    dt.Columns.Add(new DataColumn("Paperwidth", typeof(Decimal)));
                    dt.Columns.Add(new DataColumn("GSMID", typeof(Int32)));
                    dt.Columns.Add(new DataColumn("GSMValue", typeof(Int32)));
                    dt.Columns.Add(new DataColumn("PaperTypeID", typeof(Int32)));
                    dt.Columns.Add(new DataColumn("PaperManufaturerID", typeof(String)));
                    dt.Columns.Add(new DataColumn("LineRate", typeof(Decimal)));
                    dt.Columns.Add(new DataColumn("LineAmount", typeof(Decimal)));
                    dt.Columns.Add(new DataColumn("LineGST", typeof(Decimal)));

                    foreach (GridViewRow row in gvPaperDetail.Rows)
                    {
                        TextBox txtNoofSheet = row.FindControl("txtNoofSheet") as TextBox;
                        DropDownList ddlPaperSize = row.FindControl("ddlPaperSize") as DropDownList;
                        TextBox txtPaperLength = row.FindControl("txtPaperLength") as TextBox;
                        TextBox txtPaperwidth = row.FindControl("txtPaperwidth") as TextBox;
                        DropDownList ddlGSM = row.FindControl("ddlGSM") as DropDownList;
                        TextBox txtGSMValue = row.FindControl("txtGSMValue") as TextBox;
                        DropDownList ddlPaperType = row.FindControl("ddlPaperType") as DropDownList;
                        DropDownList ddlPaperManufature = row.FindControl("ddlPaperManufature") as DropDownList;
                        TextBox txtPaperRate = row.FindControl("txtPaperRate") as TextBox;
                        TextBox txtAmount = row.FindControl("txtAmount") as TextBox;
                        TextBox txtPaperDetailGST = row.FindControl("txtPaperDetailGST") as TextBox;

                        dr = dt.NewRow();

                        dr["NoOfSheet"] = txtNoofSheet.Text.Trim() != "" ? txtNoofSheet.Text.Trim() : "0";
                        dr["PaperSizeID"] = ddlPaperSize.SelectedValue != "" ? ddlPaperSize.SelectedValue : "0";
                        dr["PaperLength"] = txtPaperLength.Text.Trim() != "" ? txtPaperLength.Text.Trim() : "0";
                        dr["PaperWidth"] = txtPaperwidth.Text.Trim() != "" ? txtPaperwidth.Text.Trim() : "0";
                        dr["GSMID"] = ddlGSM.SelectedValue != "" ? ddlGSM.SelectedValue : "0";
                        dr["GSMValue"] = txtGSMValue.Text.Trim() != "" ? txtGSMValue.Text.Trim() : "0";
                        dr["PaperTypeID"] = ddlPaperType.SelectedValue != "" ? ddlPaperType.SelectedValue : "0";
                        dr["PaperManufaturerID"] = ddlPaperManufature.SelectedValue != "" ? ddlPaperManufature.SelectedValue : "0";
                        dr["LineRate"] = txtPaperRate.Text.Trim() != "" ? txtPaperRate.Text.Trim() : "0";
                        dr["LineAmount"] = txtAmount.Text.Trim() != "" ? txtAmount.Text.Trim() : "0";
                        dr["LineGST"] = txtPaperDetailGST.Text.Trim() != "" ? txtPaperDetailGST.Text.Trim() : "0";
                        dt.Rows.Add(dr);
                    }
                    dt.Rows[index].Delete();
                    dt.AcceptChanges();
                    gvPaperDetail.DataSource = dt;
                    gvPaperDetail.DataBind();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DropDownList ddlPaperSize = gvPaperDetail.Rows[i].FindControl("ddlPaperSize") as DropDownList;
                        Utility.FillMasterDataDropDownlist(ddlPaperSize, MasterType.paperSize);
                        ddlPaperSize.SelectedValue = dt.Rows[i]["PaperSizeID"].ToString();

                        DropDownList ddlGSM = gvPaperDetail.Rows[i].FindControl("ddlGSM") as DropDownList;
                        Utility.FillMasterDataDropDownlist(ddlGSM, MasterType.gsm);
                        ddlGSM.SelectedValue = dt.Rows[i]["GSMID"].ToString();

                        DropDownList ddlPaperType = gvPaperDetail.Rows[i].FindControl("ddlPaperType") as DropDownList;
                        Utility.FillMasterDataDropDownlist(ddlPaperType, MasterType.paperType);
                        ddlPaperType.SelectedValue = dt.Rows[i]["PaperTypeID"].ToString();

                        DropDownList ddlPaperManufature = gvPaperDetail.Rows[i].FindControl("ddlPaperManufature") as DropDownList;
                        Utility.FillMasterDataDropDownlist(ddlPaperManufature, MasterType.paperCompany);
                        ddlPaperManufature.SelectedValue = dt.Rows[i]["PaperManufaturerID"].ToString();
                    }
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GSTCalculation()", true);
                }
            }
        }

        protected void gvPaperDetail_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void ddlGSM_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlLabTest = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlLabTest.NamingContainer;
            DropDownList ddlGSM = row.FindControl("ddlGSM") as DropDownList;
            DropDownList ddlPaperSize = row.FindControl("ddlPaperSize") as DropDownList;
            DropDownList ddlPaperType = row.FindControl("ddlPaperType") as DropDownList;
            DropDownList ddlPaperManufature = row.FindControl("ddlPaperManufature") as DropDownList;

            TextBox txtGSMValue = row.FindControl("txtGSMValue") as TextBox;

            if (ddlGSM.SelectedIndex > -1)
            {
                int MasterId = Convert.ToInt32(ddlGSM.SelectedValue != "" ? ddlGSM.SelectedValue : "0");
                var data = _dbContext.PrintingParameters.Where(p => p.PrintingParameterID == MasterId).SingleOrDefault();
                txtGSMValue.Text = Convert.ToInt32(data.ParameterValue1).ToString();

                TextBox txtPaperRate = row.FindControl("txtPaperRate") as TextBox;
                if (ddlPaperSize.SelectedIndex > -1 && ddlGSM.SelectedIndex > -1 && ddlPaperManufature.SelectedIndex > -1 && ddlPaperType.SelectedIndex > -1)
                {
                    int ParameterValue1 = Convert.ToInt32(ddlPaperSize.SelectedValue != "" ? ddlPaperSize.SelectedValue : "0");
                    int ParameterValue2 = Convert.ToInt32(ddlGSM.SelectedValue != "" ? ddlGSM.SelectedValue : "0");
                    int ParameterValue3 = Convert.ToInt32(ddlPaperManufature.SelectedValue != "" ? ddlPaperManufature.SelectedValue : "0");
                    int ParameterValue4 = Convert.ToInt32(ddlPaperType.SelectedValue != "" ? ddlPaperType.SelectedValue : "0");

                    var PaperRate = _dbContext.PrintingParameters.Where(p => p.ParameterValue1 == ParameterValue1 && p.ParameterValue2 == ParameterValue2 && p.ParameterValue3 == ParameterValue3 && p.ParameterValue4 == ParameterValue4 && p.PrintingParameterTypeID == MasterType.paperRate).SingleOrDefault();
                    if (PaperRate == null) return;
                    txtPaperRate.Text = Convert.ToInt32(PaperRate.SellPrice) != 0 ? PaperRate.SellPrice.ToString() : "0";
                }
            }
            else
            {
                txtGSMValue.Text = "0";
            }
        }

        protected void DdlPaperSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlLabTest = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlLabTest.NamingContainer;
            DropDownList ddlPaperSize = row.FindControl("ddlPaperSize") as DropDownList;
            DropDownList ddlGSM = row.FindControl("ddlGSM") as DropDownList;
            DropDownList ddlPaperType = row.FindControl("ddlPaperType") as DropDownList;
            DropDownList ddlPaperManufature = row.FindControl("ddlPaperManufature") as DropDownList;

            TextBox txtPaperLength = row.FindControl("txtPaperLength") as TextBox;
            TextBox txtPaperwidth = row.FindControl("txtPaperwidth") as TextBox;

            if (ddlPaperSize.SelectedIndex > -1)
            {
                int MasterId = Convert.ToInt32(ddlPaperSize.SelectedValue != "" ? ddlPaperSize.SelectedValue : "0");
                var data = _dbContext.PrintingParameters.Where(p => p.PrintingParameterID == MasterId).SingleOrDefault();
                txtPaperLength.Text = data.ParameterValue1.ToString() != "" ? data.ParameterValue1.ToString() : "0";
                txtPaperwidth.Text = data.ParameterValue2.ToString() != "" ? data.ParameterValue2.ToString() : "0";
                TextBox txtPaperRate = row.FindControl("txtPaperRate") as TextBox;

                if (ddlPaperSize.SelectedIndex > -1 && ddlGSM.SelectedIndex > -1 && ddlPaperManufature.SelectedIndex > -1 && ddlPaperType.SelectedIndex > -1)
                {
                    int ParameterValue1 = Convert.ToInt32(ddlPaperSize.SelectedValue != "" ? ddlPaperSize.SelectedValue : "0");
                    int ParameterValue2 = Convert.ToInt32(ddlGSM.SelectedValue != "" ? ddlGSM.SelectedValue : "0");
                    int ParameterValue3 = Convert.ToInt32(ddlPaperManufature.SelectedValue != "" ? ddlPaperManufature.SelectedValue : "0");
                    int ParameterValue4 = Convert.ToInt32(ddlPaperType.SelectedValue != "" ? ddlPaperType.SelectedValue : "0");

                    var PaperRate = _dbContext.PrintingParameters.Where(p => p.ParameterValue1 == ParameterValue1 && p.ParameterValue2 == ParameterValue2 && p.ParameterValue3 == ParameterValue3 && p.ParameterValue4 == ParameterValue4 && p.PrintingParameterTypeID == MasterType.paperRate).SingleOrDefault();
                    if (PaperRate == null) return;
                    txtPaperRate.Text = Convert.ToInt32(PaperRate.SellPrice) != 0 ? PaperRate.SellPrice.ToString() : "0";
                }
            }
            else
            {
                txtPaperLength.Text = "0";
                txtPaperwidth.Text = "0";
            }
        }

        protected void ddlPaperType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlLabTest = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlLabTest.NamingContainer;
            DropDownList ddlGSM = row.FindControl("ddlGSM") as DropDownList;
            DropDownList ddlPaperSize = row.FindControl("ddlPaperSize") as DropDownList;
            DropDownList ddlPaperType = row.FindControl("ddlPaperType") as DropDownList;
            DropDownList ddlPaperManufature = row.FindControl("ddlPaperManufature") as DropDownList;
            TextBox txtPaperRate = row.FindControl("txtPaperRate") as TextBox;

            if (ddlPaperSize.SelectedIndex > -1 && ddlGSM.SelectedIndex > -1 && ddlPaperManufature.SelectedIndex > -1 && ddlPaperType.SelectedIndex > -1)
            {
                int ParameterValue1 = Convert.ToInt32(ddlPaperSize.SelectedValue != "" ? ddlPaperSize.SelectedValue : "0");
                int ParameterValue2 = Convert.ToInt32(ddlGSM.SelectedValue != "" ? ddlGSM.SelectedValue : "0");
                int ParameterValue3 = Convert.ToInt32(ddlPaperManufature.SelectedValue != "" ? ddlPaperManufature.SelectedValue : "0");
                int ParameterValue4 = Convert.ToInt32(ddlPaperType.SelectedValue != "" ? ddlPaperType.SelectedValue : "0");

                var PaperRate = _dbContext.PrintingParameters.Where(p => p.ParameterValue1 == ParameterValue1 && p.ParameterValue2 == ParameterValue2 && p.ParameterValue3 == ParameterValue3 && p.ParameterValue4 == ParameterValue4 && p.PrintingParameterTypeID == MasterType.paperRate).SingleOrDefault();
                if (PaperRate == null) return;
                txtPaperRate.Text = Convert.ToInt32(PaperRate.SellPrice) != 0 ? PaperRate.SellPrice.ToString() : "0";
            }

            else
            {
                txtPaperRate.Text = "0";
            }
        }

        protected void ddlPaperManufature_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlLabTest = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlLabTest.NamingContainer;
            DropDownList ddlGSM = row.FindControl("ddlGSM") as DropDownList;
            DropDownList ddlPaperSize = row.FindControl("ddlPaperSize") as DropDownList;
            DropDownList ddlPaperType = row.FindControl("ddlPaperType") as DropDownList;
            DropDownList ddlPaperManufature = row.FindControl("ddlPaperManufature") as DropDownList;
            TextBox txtPaperRate = row.FindControl("txtPaperRate") as TextBox;

            if (ddlPaperSize.SelectedIndex > -1 && ddlGSM.SelectedIndex > -1 && ddlPaperManufature.SelectedIndex > -1 && ddlPaperType.SelectedIndex > -1)
            {
                int ParameterValue1 = Convert.ToInt32(ddlPaperSize.SelectedValue != "" ? ddlPaperSize.SelectedValue : "0");
                int ParameterValue2 = Convert.ToInt32(ddlGSM.SelectedValue != "" ? ddlGSM.SelectedValue : "0");
                int ParameterValue3 = Convert.ToInt32(ddlPaperManufature.SelectedValue != "" ? ddlPaperManufature.SelectedValue : "0");
                int ParameterValue4 = Convert.ToInt32(ddlPaperType.SelectedValue != "" ? ddlPaperType.SelectedValue : "0");

                var PaperRate = _dbContext.PrintingParameters.Where(p => p.ParameterValue1 == ParameterValue1 && p.ParameterValue2 == ParameterValue2 && p.ParameterValue3 == ParameterValue3 && p.ParameterValue4 == ParameterValue4 && p.PrintingParameterTypeID == MasterType.paperRate).SingleOrDefault();
                if (PaperRate == null)
                    txtPaperRate.Text = "0";
                else
                    txtPaperRate.Text = Convert.ToInt32(PaperRate.SellPrice) != 0 ? PaperRate.SellPrice.ToString() : "0";
            }

            else
            {
                txtPaperRate.Text = "0";
            }
        }

        #endregion

        #region Printing
        protected void ddlSide_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlLabTest = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlLabTest.NamingContainer;
            DropDownList ddlSide = row.FindControl("ddlSide") as DropDownList;
            TextBox txtNoofSheetPrintDetail = row.FindControl("txtNoofSheetPrintDetail") as TextBox;
            TextBox txtNoofCuttingPices = row.FindControl("txtNoofCuttingPices") as TextBox;
            TextBox txtImpression = row.FindControl("txtImpression") as TextBox;

            if (ddlSide.SelectedIndex > -1)
            {
                int MasterId = Convert.ToInt32(ddlSide.SelectedValue != "" ? ddlSide.SelectedValue : "0");
                var data = _dbContext.PrintingParameters.Where(p => p.PrintingParameterID == MasterId).SingleOrDefault();
                Decimal Impression = 0;
                if (data != null)
                {
                    Impression = Convert.ToDecimal((data.ParameterValue1.ToString() != "" ? data.ParameterValue1.ToString() : "0")) * Convert.ToDecimal((txtNoofCuttingPices.Text.Trim() != "" ? txtNoofCuttingPices.Text.Trim() : "0")) * Convert.ToDecimal((txtNoofSheetPrintDetail.Text.Trim() != "" ? txtNoofSheetPrintDetail.Text.Trim() : "0"));
                }
                txtImpression.Text = Impression.ToString("0");
                if (Impression > 0)
                {
                    txtNoofCuttingPices.Enabled = false;
                }
                else
                {
                    txtNoofCuttingPices.Enabled = true;
                }
            }
            else
            {
                txtNoofCuttingPices.Enabled = true;
                txtImpression.Text = "0";
            }
        }

        protected void ddlPlateType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlLabTest = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlLabTest.NamingContainer;
            DropDownList ddlCutting = row.FindControl("ddlCutting") as DropDownList;
            DropDownList ddlColor = row.FindControl("ddlColor") as DropDownList;
            DropDownList ddlPlateType = row.FindControl("ddlPlateType") as DropDownList;
            DropDownList ddlSide = row.FindControl("ddlSide") as DropDownList;
            TextBox txtImpression = row.FindControl("txtImpression") as TextBox;
            TextBox txtPrinintgAmount = row.FindControl("txtPrinintgAmount") as TextBox;
            TextBox txtPrintingQuantity = row.FindControl("txtPrintingQuantity") as TextBox; // PLATE NO

            if (ddlCutting.SelectedIndex > -1 && ddlColor.SelectedIndex > -1 && ddlPlateType.SelectedIndex > -1 && ddlSide.SelectedIndex > 0)
            {
                int CuttingID = Convert.ToInt32(ddlCutting.SelectedValue != "" ? ddlCutting.SelectedValue : "0");
                int ColorID = Convert.ToInt32(ddlColor.SelectedValue != "" ? ddlColor.SelectedValue : "0");
                int PlateID = Convert.ToInt32(ddlPlateType.SelectedValue != "" ? ddlPlateType.SelectedValue : "0");
                int Impression = Convert.ToInt32(txtImpression.Text.Trim() != "" ? txtImpression.Text.Trim() : "0");
                int SideID = Convert.ToInt32(ddlSide.SelectedValue != "" ? ddlSide.SelectedValue : "0");
                //int PlateNo = Convert.ToInt32(txtPrintingQuantity.Text.Trim() != "" ? txtPrintingQuantity.Text.Trim() : "0"); // PLATE NO
                int PlateNo = 0;
                int.TryParse(txtPrintingQuantity.Text.Trim() != "" ? txtPrintingQuantity.Text.Trim() : "0", out PlateNo);

                txtPrinintgAmount.Text = ApplicationService.PrintRateCalculation(CuttingID, ColorID, PlateID, PlateNo, Impression, SideID).ToString("0.00");
            }
            //if (ddlCutting.SelectedIndex > -1 && ddlColor.SelectedIndex > -1 && ddlPlateType.SelectedIndex > -1 && ddlSide.SelectedIndex > 0)
            //{
            //    int CuttingID = Convert.ToInt32(ddlCutting.SelectedValue != "" ? ddlCutting.SelectedValue : "0");
            //    int ColorID = Convert.ToInt32(ddlColor.SelectedValue != "" ? ddlColor.SelectedValue : "0");
            //    int PlateID = Convert.ToInt32(ddlPlateType.SelectedValue != "" ? ddlPlateType.SelectedValue : "0");
            //    int Impression = Convert.ToInt32(txtImpression.Text.Trim() != "" ? txtImpression.Text.Trim() : "0");
            //    var data = _dbContext.PrintRateCalculations.Where(p => p.PlateID == PlateID && p.ColorID == ColorID && p.CuttingID == CuttingID).SingleOrDefault();
            //    Decimal PrintAmount = 0;
            //    if (data != null)
            //    {
            //        if (Impression <= 1000)
            //        {
            //            PrintAmount = Convert.ToDecimal(data.PlateCost1K.ToString() != "" ? data.PlateCost1K.ToString() : "0") + Convert.ToDecimal(data.ColorCost1K.ToString() != "" ? data.ColorCost1K.ToString() : "0");
            //        }
            //        else
            //        {
            //            PrintAmount = Convert.ToDecimal(data.PlateCost10K.ToString() != "" ? data.PlateCost10K.ToString() : "0") + Convert.ToDecimal(data.ColorCost10K.ToString() != "" ? data.ColorCost10K.ToString() : "0");
            //        }
            //    }
            //    txtPrinintgAmount.Text = PrintAmount.ToString("0");
            //}
            //else
            //{
            //    txtPrinintgAmount.Text = "0";
            //}
        }
        #endregion

        #region Fabrication Item
        private void SetInitialFabricationItemRow()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            //Define the Columns
            dt.Columns.Add(new DataColumn("FabricationItemID", typeof(Int32)));
            dt.Columns.Add(new DataColumn("FabricationItemSize", typeof(String)));
            dt.Columns.Add(new DataColumn("LineQuantity", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("LineRate", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("LineAmount", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("LineGST", typeof(Decimal)));

            dr = dt.NewRow();
            dr["FabricationItemID"] = 0;
            dr["FabricationItemSize"] = 0;
            dr["LineQuantity"] = 0;
            dr["LineRate"] = 0;
            dr["LineAmount"] = 0;
            dr["LineGST"] = 0;

            dt.Rows.Add(dr);
            gvFabricator.DataSource = dt;
            gvFabricator.DataBind();

            DropDownList ddlfabricationItem = gvFabricator.Rows[0].FindControl("ddlfabricationItem") as DropDownList;
            Utility.FillMasterDataDropDownlist(ddlfabricationItem, MasterType.fabricationItem);

            ImageButton itbnNew = gvFabricator.Rows[0].FindControl("ibtnAddFabricationCosting") as ImageButton;
            ImageButton itbndelete = gvFabricator.Rows[0].FindControl("ibtnDeleteFabricationCosting") as ImageButton;

            itbnNew.Enabled = true;
            itbndelete.Enabled = true;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GSTCalculation()", true);
        }

        protected void ibtnAddFabricationCosting_Click(object sender, ImageClickEventArgs e)
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            //Define the Columns
            dt.Columns.Add(new DataColumn("FabricationItemID", typeof(Int32)));
            dt.Columns.Add(new DataColumn("FabricationItemSize", typeof(String)));
            dt.Columns.Add(new DataColumn("LineQuantity", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("LineRate", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("LineAmount", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("LineGST", typeof(Decimal)));

            foreach (GridViewRow row in gvFabricator.Rows)
            {
                DropDownList ddlfabricationItem = row.FindControl("ddlfabricationItem") as DropDownList;
                TextBox txtFabricationSize = row.FindControl("txtFabricationSize") as TextBox;
                TextBox txtFabricationQuantity = row.FindControl("txtFabricationQuantity") as TextBox;
                TextBox txtFabricationRate = row.FindControl("txtFabricationRate") as TextBox;
                TextBox txtFabricationAmount = row.FindControl("txtFabricationAmount") as TextBox;
                TextBox txtFabricationGST = row.FindControl("txtFabricationGST") as TextBox;

                dr = dt.NewRow();
                dr["FabricationItemID"] = ddlfabricationItem.SelectedValue != "" ? ddlfabricationItem.SelectedValue : "0";
                dr["FabricationItemSize"] = txtFabricationSize.Text.Trim();
                dr["LineQuantity"] = txtFabricationQuantity.Text.Trim() != "" ? txtFabricationQuantity.Text.Trim() : "0";
                dr["LineRate"] = txtFabricationRate.Text.Trim() != "" ? txtFabricationRate.Text.Trim() : "0";
                dr["LineAmount"] = txtFabricationAmount.Text.Trim() != "" ? txtFabricationAmount.Text.Trim() : "0";
                dr["LineGST"] = txtFabricationGST.Text.Trim() != "" ? txtFabricationGST.Text.Trim() : "0";

                dt.Rows.Add(dr);
            }
            dt.Rows.Add(0, 0, 0, 0, 0, 0);
            gvFabricator.DataSource = dt;
            gvFabricator.DataBind();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DropDownList ddlfabricationItem = gvFabricator.Rows[i].FindControl("ddlfabricationItem") as DropDownList;
                Utility.FillMasterDataDropDownlist(ddlfabricationItem, MasterType.fabricationItem);
                ddlfabricationItem.SelectedValue = dt.Rows[i]["FabricationItemID"].ToString();
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GSTCalculation()", true);
        }

        protected void gvFabricator_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (gvFabricator.Rows.Count == 1)
            {
                //Do Nothing
            }
            else
            {
                int index = Convert.ToInt32(e.RowIndex);

                DataTable dt = new DataTable();
                DataRow dr = null;
                //Define the Columns
                dt.Columns.Add(new DataColumn("FabricationItemID", typeof(Int32)));
                dt.Columns.Add(new DataColumn("FabricationItemSize", typeof(String)));
                dt.Columns.Add(new DataColumn("LineQuantity", typeof(Decimal)));
                dt.Columns.Add(new DataColumn("LineRate", typeof(Decimal)));
                dt.Columns.Add(new DataColumn("LineAmount", typeof(Decimal)));
                dt.Columns.Add(new DataColumn("LineGST", typeof(Decimal)));

                foreach (GridViewRow row in gvFabricator.Rows)
                {
                    DropDownList ddlfabricationItem = row.FindControl("ddlfabricationItem") as DropDownList;
                    TextBox txtFabricationSize = row.FindControl("txtFabricationSize") as TextBox;
                    TextBox txtFabricationQuantity = row.FindControl("txtFabricationQuantity") as TextBox;
                    TextBox txtFabricationRate = row.FindControl("txtFabricationRate") as TextBox;
                    TextBox txtFabricationAmount = row.FindControl("txtFabricationAmount") as TextBox;
                    TextBox txtFabricationGST = row.FindControl("txtFabricationGST") as TextBox;

                    dr = dt.NewRow();
                    dr["FabricationItemID"] = ddlfabricationItem.SelectedValue != "" ? ddlfabricationItem.SelectedValue : "0";
                    dr["FabricationItemSize"] = txtFabricationSize.Text.Trim();
                    dr["LineQuantity"] = txtFabricationQuantity.Text.Trim() != "" ? txtFabricationQuantity.Text.Trim() : "0";
                    dr["LineRate"] = txtFabricationRate.Text.Trim() != "" ? txtFabricationRate.Text.Trim() : "0";
                    dr["LineAmount"] = txtFabricationAmount.Text.Trim() != "" ? txtFabricationAmount.Text.Trim() : "0";
                    dr["LineGST"] = txtFabricationGST.Text.Trim() != "" ? txtFabricationGST.Text.Trim() : "0";
                    dt.Rows.Add(dr);
                }
                dt.Rows[index].Delete();
                dt.AcceptChanges();
                gvFabricator.DataSource = dt;
                gvFabricator.DataBind();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DropDownList ddlfabricationItem = gvFabricator.Rows[i].FindControl("ddlfabricationItem") as DropDownList;
                    Utility.FillMasterDataDropDownlist(ddlfabricationItem, MasterType.fabricationItem);
                    ddlfabricationItem.SelectedValue = dt.Rows[i]["FabricationItemID"].ToString();
                }
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GSTCalculation()", true);
        }

        protected void gvFabricator_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        #endregion

        #region Packaging
        private void SetInitialPackagingRow()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            //Define the Columns
            dt.Columns.Add(new DataColumn("PackagingItemID", typeof(Int32)));
            dt.Columns.Add(new DataColumn("LineQuantity", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("LineRate", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("LineAmount", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("LineGST", typeof(Decimal)));

            dr = dt.NewRow();
            dr["PackagingItemID"] = 0;
            dr["LineQuantity"] = 0;
            dr["LineRate"] = 0;
            dr["LineAmount"] = 0;
            dr["LineGST"] = 0;

            dt.Rows.Add(dr);
            gvPackaging.DataSource = dt;
            gvPackaging.DataBind();

            DropDownList ddlPackagingItem = gvPackaging.Rows[0].FindControl("ddlPackagingItem") as DropDownList;
            Utility.FillMasterDataDropDownlist(ddlPackagingItem, MasterType.packingItem);

            ImageButton itbnNew = gvPackaging.Rows[0].FindControl("ibtnAddPackaingCosting") as ImageButton;
            ImageButton itbndelete = gvPackaging.Rows[0].FindControl("ibtnDeletePackaingCosting") as ImageButton;

            itbnNew.Enabled = true;
            itbndelete.Enabled = true;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GSTCalculation()", true);
        }

        protected void ibtnAddpackaingCosting_Click(object sender, ImageClickEventArgs e)
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            //Define the Columns
            dt.Columns.Add(new DataColumn("PackagingItemID", typeof(Int32)));
            dt.Columns.Add(new DataColumn("LineQuantity", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("LineRate", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("LineAmount", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("LineGST", typeof(Decimal)));

            foreach (GridViewRow row in gvPackaging.Rows)
            {
                DropDownList ddlPackagingItem = row.FindControl("ddlPackagingItem") as DropDownList;
                TextBox txtPackaingQuantity = row.FindControl("txtPackaingQuantity") as TextBox;
                TextBox txtPackaingRate = row.FindControl("txtPackaingRate") as TextBox;
                TextBox txtPackaingAmount = row.FindControl("txtPackaingAmount") as TextBox;
                TextBox txtPackaingGST = row.FindControl("txtPackaingGST") as TextBox;

                dr = dt.NewRow();
                dr["PackagingItemID"] = ddlPackagingItem.SelectedValue != "" ? ddlPackagingItem.SelectedValue : "0";
                dr["LineQuantity"] = txtPackaingQuantity.Text.Trim() != "" ? txtPackaingQuantity.Text.Trim() : "0";
                dr["LineRate"] = txtPackaingRate.Text.Trim() != "" ? txtPackaingRate.Text.Trim() : "0";
                dr["LineAmount"] = txtPackaingAmount.Text.Trim() != "" ? txtPackaingAmount.Text.Trim() : "0";
                dr["LineGST"] = txtPackaingGST.Text.Trim() != "" ? txtPackaingGST.Text.Trim() : "0";
                dt.Rows.Add(dr);
            }
            dt.Rows.Add(0, 0, 0, 0, 0);
            gvPackaging.DataSource = dt;
            gvPackaging.DataBind();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DropDownList ddlPackagingItem = gvPackaging.Rows[i].FindControl("ddlPackagingItem") as DropDownList;
                Utility.FillMasterDataDropDownlist(ddlPackagingItem, MasterType.packingItem);
                ddlPackagingItem.SelectedValue = dt.Rows[i]["PackagingItemID"].ToString();
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GSTCalculation()", true);
        }

        protected void gvPackaging_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (gvPackaging.Rows.Count == 1)
            {
                //Do Nothing
            }
            else
            {
                int index = Convert.ToInt32(e.RowIndex);

                DataTable dt = new DataTable();
                DataRow dr = null;
                //Define the Columns
                dt.Columns.Add(new DataColumn("PackagingItemID", typeof(Int32)));
                dt.Columns.Add(new DataColumn("LineQuantity", typeof(Decimal)));
                dt.Columns.Add(new DataColumn("LineRate", typeof(Decimal)));
                dt.Columns.Add(new DataColumn("LineAmount", typeof(Decimal)));
                dt.Columns.Add(new DataColumn("LineGST", typeof(Decimal)));

                foreach (GridViewRow row in gvPackaging.Rows)
                {
                    DropDownList ddlPackagingItem = row.FindControl("ddlPackagingItem") as DropDownList;
                    TextBox txtPackaingQuantity = row.FindControl("txtPackaingQuantity") as TextBox;
                    TextBox txtPackaingRate = row.FindControl("txtPackaingRate") as TextBox;
                    TextBox txtPackaingAmount = row.FindControl("txtPackaingAmount") as TextBox;
                    TextBox txtPackaingGST = row.FindControl("txtPackaingGST") as TextBox;

                    dr = dt.NewRow();
                    dr["PackagingItemID"] = ddlPackagingItem.SelectedValue != "" ? ddlPackagingItem.SelectedValue : "0";
                    dr["LineQuantity"] = txtPackaingQuantity.Text.Trim() != "" ? txtPackaingQuantity.Text.Trim() : "0";
                    dr["LineRate"] = txtPackaingRate.Text.Trim() != "" ? txtPackaingRate.Text.Trim() : "0";
                    dr["LineAmount"] = txtPackaingAmount.Text.Trim() != "" ? txtPackaingAmount.Text.Trim() : "0";
                    dr["LineGST"] = txtPackaingGST.Text.Trim() != "" ? txtPackaingGST.Text.Trim() : "0";
                    dt.Rows.Add(dr);
                }
                dt.Rows[index].Delete();
                dt.AcceptChanges();
                gvPackaging.DataSource = dt;
                gvPackaging.DataBind();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DropDownList ddlPackagingItem = gvPackaging.Rows[i].FindControl("ddlPackagingItem") as DropDownList;
                    Utility.FillMasterDataDropDownlist(ddlPackagingItem, MasterType.packingItem);
                    ddlPackagingItem.SelectedValue = dt.Rows[i]["PackagingItemID"].ToString();
                }
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GSTCalculation()", true);
        }

        protected void gvPackaging_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        #endregion

        #region Transport
        private void SetInitialTransportRow()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            //Define the Columns
            dt.Columns.Add(new DataColumn("LabourCharge", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("TransportCharge", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("LineQuantity", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("TransportUnitID", typeof(Int32)));
            dt.Columns.Add(new DataColumn("LineRate", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("LineAmount", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("LineGST", typeof(Decimal)));

            dr = dt.NewRow();
            dr["LabourCharge"] = 0;
            dr["TransportCharge"] = 0;
            dr["LineQuantity"] = 0;
            dr["TransportUnitID"] = 0;
            dr["LineRate"] = 0;
            dr["LineAmount"] = 0;
            dr["LineGST"] = 0;
            dt.Rows.Add(dr);

            gvTransport.DataSource = dt;
            gvTransport.DataBind();

            DropDownList ddlTransportUnit = gvTransport.Rows[0].FindControl("ddlTransportUnit") as DropDownList;
            Utility.FillMasterDataDropDownlist(ddlTransportUnit, MasterType.unit);

            ImageButton itbnNew = gvTransport.Rows[0].FindControl("ibtnAddTransportCosting") as ImageButton;
            ImageButton itbndelete = gvTransport.Rows[0].FindControl("ibtnDeleteTransportCosting") as ImageButton;

            itbnNew.Enabled = true;
            itbndelete.Enabled = true;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GSTCalculation()", true);
        }

        protected void ibtnAddTransportCosting_Click(object sender, ImageClickEventArgs e)
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            //Define the Columns
            dt.Columns.Add(new DataColumn("LabourCharge", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("TransportCharge", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("LineQuantity", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("TransportUnitID", typeof(Int32)));
            dt.Columns.Add(new DataColumn("LineRate", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("LineAmount", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("LineGST", typeof(Decimal)));

            foreach (GridViewRow row in gvTransport.Rows)
            {
                TextBox txtLabourCharge = row.FindControl("txtLabourCharge") as TextBox;
                TextBox txtCarFare = row.FindControl("txtCarFare") as TextBox;
                TextBox txtTransportQty = row.FindControl("txtTransportQty") as TextBox;
                DropDownList ddlTransportUnit = row.FindControl("ddlTransportUnit") as DropDownList;
                TextBox txtTransportRate = row.FindControl("txtTransportRate") as TextBox;
                TextBox txtTransportAmount = row.FindControl("txtTransportAmount") as TextBox;
                TextBox txtTransportGST = row.FindControl("txtTransportGST") as TextBox;
                dr = dt.NewRow();
                dr["LabourCharge"] = txtLabourCharge.Text.Trim() != "" ? txtLabourCharge.Text.Trim() : "0";
                dr["TransportCharge"] = txtCarFare.Text.Trim() != "" ? txtCarFare.Text.Trim() : "0";
                dr["LineQuantity"] = txtTransportQty.Text.Trim() != "" ? txtTransportQty.Text.Trim() : "0";
                dr["TransportUnitID"] = ddlTransportUnit.SelectedValue != "" ? ddlTransportUnit.SelectedValue : "0";
                dr["LineRate"] = txtTransportRate.Text.Trim() != "" ? txtTransportRate.Text.Trim() : "0";
                dr["LineAmount"] = txtTransportAmount.Text.Trim() != "" ? txtTransportAmount.Text.Trim() : "0";
                dr["LineGST"] = txtTransportGST.Text.Trim() != "" ? txtTransportGST.Text.Trim() : "0";
                dt.Rows.Add(dr);
            }
            dt.Rows.Add(0, 0, 0, 0, 0, 0, 0);
            gvTransport.DataSource = dt;
            gvTransport.DataBind();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DropDownList ddlTransportUnit = gvTransport.Rows[i].FindControl("ddlTransportUnit") as DropDownList;
                Utility.FillMasterDataDropDownlist(ddlTransportUnit, MasterType.unit);
                ddlTransportUnit.SelectedValue = dt.Rows[i]["TransportUnitID"].ToString();
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GSTCalculation()", true);
        }

        protected void gvTransport_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (gvTransport.Rows.Count == 1)
            {
                //Do Nothing
            }
            else
            {
                int index = Convert.ToInt32(e.RowIndex);

                DataTable dt = new DataTable();
                DataRow dr = null;
                //Define the Columns
                dt.Columns.Add(new DataColumn("LabourCharge", typeof(Decimal)));
                dt.Columns.Add(new DataColumn("TransportCharge", typeof(Decimal)));
                dt.Columns.Add(new DataColumn("LineQuantity", typeof(Decimal)));
                dt.Columns.Add(new DataColumn("TransportUnitID", typeof(Int32)));
                dt.Columns.Add(new DataColumn("LineRate", typeof(Decimal)));
                dt.Columns.Add(new DataColumn("LineAmount", typeof(Decimal)));
                dt.Columns.Add(new DataColumn("LineGST", typeof(Decimal)));

                foreach (GridViewRow row in gvTransport.Rows)
                {
                    TextBox txtLabourCharge = row.FindControl("txtLabourCharge") as TextBox;
                    TextBox txtCarFare = row.FindControl("txtCarFare") as TextBox;
                    TextBox txtTransportQty = row.FindControl("txtTransportQty") as TextBox;
                    DropDownList ddlTransportUnit = row.FindControl("ddlTransportUnit") as DropDownList;
                    TextBox txtTransportRate = row.FindControl("txtTransportRate") as TextBox;
                    TextBox txtTransportAmount = row.FindControl("txtTransportAmount") as TextBox;
                    TextBox txtTransportGST = row.FindControl("txtTransportGST") as TextBox;
                    dr = dt.NewRow();
                    dr["LabourCharge"] = txtLabourCharge.Text.Trim() != "" ? txtLabourCharge.Text.Trim() : "0";
                    dr["TransportCharge"] = txtCarFare.Text.Trim() != "" ? txtCarFare.Text.Trim() : "0";
                    dr["LineQuantity"] = txtTransportQty.Text.Trim() != "" ? txtTransportQty.Text.Trim() : "0";
                    dr["TransportUnitID"] = ddlTransportUnit.SelectedValue != "" ? ddlTransportUnit.SelectedValue : "0";
                    dr["LineRate"] = txtTransportRate.Text.Trim() != "" ? txtTransportRate.Text.Trim() : "0";
                    dr["LineAmount"] = txtTransportAmount.Text.Trim() != "" ? txtTransportAmount.Text.Trim() : "0";
                    dr["LineGST"] = txtTransportGST.Text.Trim() != "" ? txtTransportGST.Text.Trim() : "0";
                    dt.Rows.Add(dr);
                }
                dt.Rows[index].Delete();
                dt.AcceptChanges();
                gvTransport.DataSource = dt;
                gvTransport.DataBind();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DropDownList ddlTransportUnit = gvTransport.Rows[i].FindControl("ddlTransportUnit") as DropDownList;
                    Utility.FillMasterDataDropDownlist(ddlTransportUnit, MasterType.unit);
                    ddlTransportUnit.SelectedValue = dt.Rows[i]["TransportUnitID"].ToString();
                }
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GSTCalculation()", true);
        }

        protected void gvTransport_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        #endregion

        #region Design
        private void SetInitialDesignRow()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            //Define the Columns
            dt.Columns.Add(new DataColumn("OperatorID", typeof(Int32)));
            dt.Columns.Add(new DataColumn("Narration", typeof(String)));
            dt.Columns.Add(new DataColumn("LineAmount", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("LineGST", typeof(Decimal)));

            dr = dt.NewRow();
            dr["OperatorID"] = 0;
            dr["Narration"] = String.Empty;
            dr["LineAmount"] = 0;
            dr["LineGST"] = 0;
            dt.Rows.Add(dr);

            gvDesignEdit.DataSource = dt;
            gvDesignEdit.DataBind();

            DropDownList ddlOperator = gvDesignEdit.Rows[0].FindControl("ddlOperator") as DropDownList;
            Utility.FillMasterDataDropDownlist(ddlOperator, MasterType.operatorprint);

            ImageButton itbnNew = gvDesignEdit.Rows[0].FindControl("ibtnAddDesignCosting") as ImageButton;
            ImageButton itbndelete = gvDesignEdit.Rows[0].FindControl("ibtnDeleteDesignCosting") as ImageButton;

            itbnNew.Enabled = true;
            itbndelete.Enabled = true;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GSTCalculation()", true);
        }

        protected void ibtnAddDesignCosting(object sender, ImageClickEventArgs e)
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            //Define the Columns
            dt.Columns.Add(new DataColumn("OperatorID", typeof(Int32)));
            dt.Columns.Add(new DataColumn("Narration", typeof(String)));
            dt.Columns.Add(new DataColumn("LineAmount", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("LineGST", typeof(Decimal)));
            foreach (GridViewRow row in gvDesignEdit.Rows)
            {
                DropDownList ddlOperator = row.FindControl("ddlOperator") as DropDownList;
                TextBox txtNarration = row.FindControl("txtNarration") as TextBox;
                TextBox txtDesignRate = row.FindControl("txtDesignRate") as TextBox;
                TextBox txtDesignAmountGST = row.FindControl("txtDesignAmountGST") as TextBox;
                dr = dt.NewRow();
                dr["OperatorID"] = ddlOperator.SelectedValue != "" ? ddlOperator.SelectedValue : "0";
                dr["Narration"] = txtNarration.Text.Trim();
                dr["LineAmount"] = txtDesignRate.Text.Trim() != "" ? txtDesignRate.Text.Trim() : "0";
                dr["LineGST"] = txtDesignAmountGST.Text.Trim() != "" ? txtDesignAmountGST.Text.Trim() : "0";
                dt.Rows.Add(dr);
            }
            dt.Rows.Add(0, String.Empty, 0, 0);
            gvDesignEdit.DataSource = dt;
            gvDesignEdit.DataBind();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DropDownList ddlOperator = gvDesignEdit.Rows[i].FindControl("ddlOperator") as DropDownList;
                Utility.FillMasterDataDropDownlist(ddlOperator, MasterType.operatorprint);
                ddlOperator.SelectedValue = dt.Rows[i]["OperatorID"].ToString();
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GSTCalculation()", true);
        }

        protected void gvDesignEdit_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (gvDesignEdit.Rows.Count == 1)
            {
                //Do Nothing
            }
            else
            {
                int index = Convert.ToInt32(e.RowIndex);

                DataTable dt = new DataTable();
                DataRow dr = null;
                //Define the Columns
                dt.Columns.Add(new DataColumn("OperatorID", typeof(Int32)));
                dt.Columns.Add(new DataColumn("Narration", typeof(String)));
                dt.Columns.Add(new DataColumn("LineAmount", typeof(Decimal)));
                dt.Columns.Add(new DataColumn("LineGST", typeof(Decimal)));
                foreach (GridViewRow row in gvDesignEdit.Rows)
                {
                    DropDownList ddlOperator = row.FindControl("ddlOperator") as DropDownList;
                    TextBox txtNarration = row.FindControl("txtNarration") as TextBox;
                    TextBox txtDesignRate = row.FindControl("txtDesignRate") as TextBox;
                    TextBox txtDesignAmountGST = row.FindControl("txtDesignAmountGST") as TextBox;
                    dr = dt.NewRow();
                    dr["OperatorID"] = ddlOperator.SelectedValue != "" ? ddlOperator.SelectedValue : "0";
                    dr["Narration"] = txtNarration.Text.Trim();
                    dr["LineAmount"] = txtDesignRate.Text.Trim() != "" ? txtDesignRate.Text.Trim() : "0";
                    dr["LineGST"] = txtDesignAmountGST.Text.Trim() != "" ? txtDesignAmountGST.Text.Trim() : "0";
                    dt.Rows.Add(dr);
                }
                dt.Rows[index].Delete();
                dt.AcceptChanges();
                gvDesignEdit.DataSource = dt;
                gvDesignEdit.DataBind();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DropDownList ddlOperator = gvDesignEdit.Rows[i].FindControl("ddlOperator") as DropDownList;
                    Utility.FillMasterDataDropDownlist(ddlOperator, MasterType.operatorprint);
                    ddlOperator.SelectedValue = dt.Rows[i]["OperatorID"].ToString();
                }
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GSTCalculation()", true);
        }

        protected void gvDesignEdit_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        #endregion

        private void ResetControl()
        {
            txtQuotationNo.Enabled = true;
            QuotationID = 0;
            _MasterTypeID = 0;
            SetInitialPaperDetailRow();
            SetInitialFabricationItemRow();
            SetInitialPackagingRow();
            SetInitialTransportRow();
            SetInitialDesignRow();
            txtQuotationNo.Text = String.Empty;
            txtDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy", provider);
            chkJobCreation.Checked = true;
            ddlClient.SelectedIndex = -1;
            ddlClient_SelectedIndexChanged(ddlClient, EventArgs.Empty);
            ddlGST.SelectedIndex = -1;
            ddlGST_SelectedIndexChanged(ddlGST, EventArgs.Empty);
            ddlItem.SelectedIndex = -1;
            ddlItem_SelectedIndexChanged(ddlItem, EventArgs.Empty);
            txtTotalBillAmount.Text = String.Empty;
            txtBillDiscountAmount.Text = String.Empty;
            txtNetAmount.Text = String.Empty;
            txtRoundOff.Text = String.Empty;
            txtGSTPercenatge.Text = String.Empty;
            txtGSTWOAmount.Text = String.Empty;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GSTCalculation()", true);
            btnPopulatePrintingDetail.Enabled = true;
            btnRefreshPrintingDetail.Enabled = false;
            gvPaperDetail.Enabled = true;
            gvPrintingDetail.DataSource = null;
            gvPrintingDetail.DataBind();
            ddlGST.SelectedValue = "250"; //GST 12%
            txtGSTPercenatge.Text = "12.00";
            ddlPaymentMode.SelectedIndex = -1;
            txtPaidAmount.Text = "0.00";
            txtPaymentDetail.Text = "";
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

        private void FillTaxList()
        {
            var data = (from p in _dbContext.PrintingParameters
                        where p.PrintingParameterTypeID == 1007 && p.RecMode != "D"
                        orderby p.PrintingParameterName ascending
                        select new { PrintingParameterName = p.PrintingParameterName.ToUpper(), PrintingParameterID = p.PrintingParameterID }).ToList();
            if (data.Count > 0)
            {
                ddlGST.DataSource = data;
                ddlGST.DataTextField = "PrintingParameterName";
                ddlGST.DataValueField = "PrintingParameterID";
                ddlGST.DataBind();
            }
            else
            {
                ddlGST.DataSource = null;
                ddlGST.DataBind();
            }
            ddlGST.Items.Insert(0, new ListItem("- Select -", "0"));
        }

        private void FillPaymentMode()
        {
            var data = (from p in _dbContext.PaymentModes
                        orderby p.PMode ascending
                        select (p)).ToList();
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
        private void GetQuotationSheetDetail(Int32 QuotationID)
        {
            var GetData = _dbContext.QuotationJobSheets.Where(p => p.QuotationID == QuotationID).Single();
            txtQuotationNo.Text = GetData.QuotationNO;
            txtQuotationNo.Enabled = false;
            if (GetData.QuotationDate != null)
            {
                DateTime QuotationDate = Convert.ToDateTime(GetData.QuotationDate);
                txtDate.Text = QuotationDate.ToString("dd/MM/yyyy");
            }
            chkJobCreation.Checked = Convert.ToBoolean(GetData.IS_JOB);
            ddlClient.SelectedValue = Convert.ToString(GetData.CustomerID);
            ddlClient_SelectedIndexChanged(ddlClient, EventArgs.Empty);
            ddlGST.SelectedValue = Convert.ToString(GetData.TaxID);
            ddlGST_SelectedIndexChanged(ddlGST, EventArgs.Empty);

            txtGSTPercenatge.Text = GetData.GSTPercentage.ToString();
            txtGSTWOAmount.Text = GetData.GSTWOPaper.ToString();
            txtGSTAmount.Text = GetData.TaxableAmount.ToString();
            txtBillDiscountAmount.Text = GetData.Discount.ToString();
            txtTotalBillAmount.Text = GetData.TotalBillingAmount.ToString();
            txtNetAmount.Text = GetData.AmountPayable.ToString();

            ddlItem.SelectedValue = Convert.ToString(GetData.ItemID);
            txtItemDescription.Text = GetData.ItemDescription.ToString();

            DataTable dt = new DataTable();
            //Paper Detail
            dt = new DataTable();
            var PaperDetail = _dbContext.QuotationJobSheetDetails.Where(p => p.QuotationID == QuotationID && p.PrintComponentID == QuotationParameter.paperDetail).ToList();
            dt = ApplicationUtility.ListToDataTable(PaperDetail);
            gvPaperDetail.DataSource = dt;
            gvPaperDetail.DataBind();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DropDownList ddlPaperSize = gvPaperDetail.Rows[i].FindControl("ddlPaperSize") as DropDownList;
                Utility.FillMasterDataDropDownlist(ddlPaperSize, MasterType.paperSize);
                ddlPaperSize.SelectedValue = dt.Rows[i]["PaperSizeID"].ToString();

                DropDownList ddlGSM = gvPaperDetail.Rows[i].FindControl("ddlGSM") as DropDownList;
                Utility.FillMasterDataDropDownlist(ddlGSM, MasterType.gsm);
                ddlGSM.SelectedValue = dt.Rows[i]["GSMID"].ToString();

                DropDownList ddlPaperType = gvPaperDetail.Rows[i].FindControl("ddlPaperType") as DropDownList;
                Utility.FillMasterDataDropDownlist(ddlPaperType, MasterType.paperType);
                ddlPaperType.SelectedValue = dt.Rows[i]["PaperTypeID"].ToString();

                DropDownList ddlPaperManufature = gvPaperDetail.Rows[i].FindControl("ddlPaperManufature") as DropDownList;
                Utility.FillMasterDataDropDownlist(ddlPaperManufature, MasterType.paperCompany);
                ddlPaperManufature.SelectedValue = dt.Rows[i]["PaperManufaturerID"].ToString();
            }
            gvPaperDetail.Enabled = false;
            btnPopulatePrintingDetail.Enabled = false;
            btnRefreshPrintingDetail.Enabled = true;

            //Printing
            dt = new DataTable();
            var Printing = _dbContext.QuotationJobSheetDetails.Where(p => p.QuotationID == QuotationID && p.PrintComponentID == QuotationParameter.Printing).ToList();
            dt = ApplicationUtility.ListToDataTable(Printing);
            gvPrintingDetail.DataSource = dt;
            gvPrintingDetail.DataBind();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DropDownList ddlPaperSizePrintDetail = gvPrintingDetail.Rows[i].FindControl("ddlPaperSizePrintDetail") as DropDownList;
                Utility.FillMasterDataDropDownlist(ddlPaperSizePrintDetail, MasterType.paperSize);
                ddlPaperSizePrintDetail.SelectedValue = dt.Rows[i]["PaperSizeIDPrintDetail"].ToString();

                DropDownList ddlCutting = gvPrintingDetail.Rows[i].FindControl("ddlCutting") as DropDownList;
                Utility.FillMasterDataDropDownlist(ddlCutting, MasterType.cutting);
                ddlCutting.SelectedValue = dt.Rows[i]["CuttingID"].ToString();
                //ddlPlateType_SelectedIndexChanged(ddlCutting, EventArgs.Empty);

                DropDownList ddlSide = gvPrintingDetail.Rows[i].FindControl("ddlSide") as DropDownList;
                Utility.FillMasterDataDropDownlist(ddlSide, MasterType.paperSide);
                ddlSide.SelectedValue = dt.Rows[i]["SideID"].ToString();
                //ddlSide_SelectedIndexChanged(ddlSide, EventArgs.Empty);

                DropDownList ddlColor = gvPrintingDetail.Rows[i].FindControl("ddlColor") as DropDownList;
                Utility.FillMasterDataDropDownlist(ddlColor, MasterType.color);
                ddlColor.SelectedValue = dt.Rows[i]["ColourID"].ToString();
                //ddlPlateType_SelectedIndexChanged(ddlColor, EventArgs.Empty);

                DropDownList ddlPlateType = gvPrintingDetail.Rows[i].FindControl("ddlPlateType") as DropDownList;
                Utility.FillMasterDataDropDownlist(ddlPlateType, MasterType.plateType);
                ddlPlateType.SelectedValue = dt.Rows[i]["PlateTypeID"].ToString();
                //ddlPlateType_SelectedIndexChanged(ddlPlateType, EventArgs.Empty);

                DropDownList ddlMachine = gvPrintingDetail.Rows[i].FindControl("ddlMachine") as DropDownList;
                Utility.FillMasterDataDropDownlist(ddlMachine, MasterType.machine);
                ddlMachine.SelectedValue = dt.Rows[i]["MachineID"].ToString();

            }

            //Fabrication Item
            dt = new DataTable();
            var Fabricator = _dbContext.QuotationJobSheetDetails.Where(p => p.QuotationID == QuotationID && p.PrintComponentID == QuotationParameter.FabricationItem).ToList();
            dt = ApplicationUtility.ListToDataTable(Fabricator);
            gvFabricator.DataSource = dt;
            gvFabricator.DataBind();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DropDownList ddlfabricationItem = gvFabricator.Rows[i].FindControl("ddlfabricationItem") as DropDownList;
                Utility.FillMasterDataDropDownlist(ddlfabricationItem, MasterType.fabricationItem);

                for (int k = 0; k < ddlfabricationItem.Items.Count; k++)
                {
                    if (ddlfabricationItem.Items[k].Value == dt.Rows[i]["FabricationItemID"].ToString())
                    {
                        ddlfabricationItem.ClearSelection();
                        ddlfabricationItem.Items[k].Selected = true;
                        break;
                    }
                }
            }

            //Packaging
            dt = new DataTable();
            var Packaging = _dbContext.QuotationJobSheetDetails.Where(p => p.QuotationID == QuotationID && p.PrintComponentID == QuotationParameter.Packaging).ToList();
            dt = ApplicationUtility.ListToDataTable(Packaging);
            gvPackaging.DataSource = dt;
            gvPackaging.DataBind();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DropDownList ddlPackagingItem = gvPackaging.Rows[i].FindControl("ddlPackagingItem") as DropDownList;
                Utility.FillMasterDataDropDownlist(ddlPackagingItem, MasterType.packingItem);

                for (int k = 0; k < ddlPackagingItem.Items.Count; k++)
                {
                    if (ddlPackagingItem.Items[k].Value == dt.Rows[i]["PackagingItemID"].ToString())
                    {
                        ddlPackagingItem.ClearSelection();
                        ddlPackagingItem.Items[k].Selected = true;
                        break;
                    }
                }
            }

            //Transport
            dt = new DataTable();
            var Transport = _dbContext.QuotationJobSheetDetails.Where(p => p.QuotationID == QuotationID && p.PrintComponentID == QuotationParameter.Transport).ToList();
            dt = ApplicationUtility.ListToDataTable(Transport);
            gvTransport.DataSource = dt;
            gvTransport.DataBind();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DropDownList ddlTransportUnit = gvTransport.Rows[i].FindControl("ddlTransportUnit") as DropDownList;
                Utility.FillMasterDataDropDownlist(ddlTransportUnit, MasterType.unit);

                for (int k = 0; k < ddlTransportUnit.Items.Count; k++)
                {
                    if (ddlTransportUnit.Items[k].Value == dt.Rows[i]["TransportUnitID"].ToString())
                    {
                        ddlTransportUnit.ClearSelection();
                        ddlTransportUnit.Items[k].Selected = true;
                        break;
                    }
                }
            }

            //Design
            dt = new DataTable();
            var Design = _dbContext.QuotationJobSheetDetails.Where(p => p.QuotationID == QuotationID && p.PrintComponentID == QuotationParameter.Design).ToList();
            dt = ApplicationUtility.ListToDataTable(Design);
            gvDesignEdit.DataSource = dt;
            gvDesignEdit.DataBind();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DropDownList ddlOperator = gvDesignEdit.Rows[i].FindControl("ddlOperator") as DropDownList;
                Utility.FillMasterDataDropDownlist(ddlOperator, MasterType.operatorprint);

                for (int k = 0; k < ddlOperator.Items.Count; k++)
                {
                    if (ddlOperator.Items[k].Value == dt.Rows[i]["OperatorID"].ToString())
                    {
                        ddlOperator.ClearSelection();
                        ddlOperator.Items[k].Selected = true;
                        break;
                    }
                }
            }

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GSTCalculation()", true);
            btnSave.Text = "Update";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            String message;
            DataTable dt = new DataTable();
            DataRow dr = null;
            try
            {
                if (Convert.ToDecimal(txtPaidAmount.Text) == 0)
                {
                    message = "alert('Amount received must be a value.')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    return;
                }
                //Define the Columns
                dt.Columns.Add(new DataColumn("PrintComponentID", typeof(Int32)));
                dt.Columns.Add(new DataColumn("NoOfSheet", typeof(Int32)));
                dt.Columns.Add(new DataColumn("GSMID", typeof(Int32)));
                dt.Columns.Add(new DataColumn("GSMValue", typeof(Int32)));
                dt.Columns.Add(new DataColumn("PaperSizeID", typeof(Int32)));
                dt.Columns.Add(new DataColumn("PaperLength", typeof(Decimal)));
                dt.Columns.Add(new DataColumn("PaperWidth", typeof(Decimal)));
                dt.Columns.Add(new DataColumn("PaperTypeID", typeof(Int32)));
                dt.Columns.Add(new DataColumn("PaperManufaturerID", typeof(Int32)));

                dt.Columns.Add(new DataColumn("NoOfSheetPrintDetail", typeof(Int32)));
                dt.Columns.Add(new DataColumn("PaperSizeIDPrintDetail", typeof(Int32)));
                dt.Columns.Add(new DataColumn("Impression", typeof(Decimal)));
                dt.Columns.Add(new DataColumn("CuttingID", typeof(Int32)));
                dt.Columns.Add(new DataColumn("CuttingPiece", typeof(Int32)));
                dt.Columns.Add(new DataColumn("FinishSize", typeof(Decimal)));
                dt.Columns.Add(new DataColumn("SideID", typeof(Int32)));
                dt.Columns.Add(new DataColumn("ColourID", typeof(Int32)));
                dt.Columns.Add(new DataColumn("PlateTypeID", typeof(Int32)));
                dt.Columns.Add(new DataColumn("MachineID", typeof(Int32)));
                dt.Columns.Add(new DataColumn("FabricationItemID", typeof(Int32)));
                dt.Columns.Add(new DataColumn("FabricationItemSize", typeof(String)));
                dt.Columns.Add(new DataColumn("PackagingItemID", typeof(Int32)));
                dt.Columns.Add(new DataColumn("LabourCharge", typeof(Decimal)));
                dt.Columns.Add(new DataColumn("TransportCharge", typeof(Decimal)));
                dt.Columns.Add(new DataColumn("TransportUnitID", typeof(Int32)));
                dt.Columns.Add(new DataColumn("OperatorID", typeof(Int32)));
                dt.Columns.Add(new DataColumn("Narration", typeof(String)));
                dt.Columns.Add(new DataColumn("LineQuantity", typeof(Decimal)));
                dt.Columns.Add(new DataColumn("LineRate", typeof(Decimal)));
                dt.Columns.Add(new DataColumn("LineAmount", typeof(Decimal)));
                dt.Columns.Add(new DataColumn("LineGST", typeof(Decimal)));

                dt.Columns["PrintComponentID"].DefaultValue = 0;
                dt.Columns["NoOfSheet"].DefaultValue = 0;
                dt.Columns["GSMID"].DefaultValue = 0;
                dt.Columns["GSMValue"].DefaultValue = 0;
                dt.Columns["PaperSizeID"].DefaultValue = 0;
                dt.Columns["PaperLength"].DefaultValue = 0;
                dt.Columns["PaperWidth"].DefaultValue = 0;
                dt.Columns["PaperTypeID"].DefaultValue = 0;
                dt.Columns["PaperManufaturerID"].DefaultValue = 0;

                dt.Columns["NoOfSheetPrintDetail"].DefaultValue = 0;
                dt.Columns["PaperSizeIDPrintDetail"].DefaultValue = 0;
                dt.Columns["Impression"].DefaultValue = 0;
                dt.Columns["CuttingID"].DefaultValue = 0;
                dt.Columns["CuttingPiece"].DefaultValue = 0;
                dt.Columns["FinishSize"].DefaultValue = 0;
                dt.Columns["SideID"].DefaultValue = 0;
                dt.Columns["ColourID"].DefaultValue = 0;
                dt.Columns["PlateTypeID"].DefaultValue = 0;
                dt.Columns["MachineID"].DefaultValue = 0;
                dt.Columns["FabricationItemID"].DefaultValue = 0;
                dt.Columns["FabricationItemSize"].DefaultValue = String.Empty;
                dt.Columns["PackagingItemID"].DefaultValue = 0;
                dt.Columns["LabourCharge"].DefaultValue = 0;
                dt.Columns["TransportCharge"].DefaultValue = 0;
                dt.Columns["TransportUnitID"].DefaultValue = 0;
                dt.Columns["OperatorID"].DefaultValue = 0;
                dt.Columns["Narration"].DefaultValue = "";
                dt.Columns["LineQuantity"].DefaultValue = 0;
                dt.Columns["LineRate"].DefaultValue = 0;
                dt.Columns["LineAmount"].DefaultValue = 0;
                dt.Columns["LineGST"].DefaultValue = 0;

                //Paper Detail
                if (gvPaperDetail.Rows.Count > 0)
                {
                    foreach (GridViewRow row in gvPaperDetail.Rows)
                    {
                        TextBox txtNoofSheet = row.FindControl("txtNoofSheet") as TextBox;
                        DropDownList ddlGSM = row.FindControl("ddlGSM") as DropDownList;
                        TextBox txtGSMValue = row.FindControl("txtGSMValue") as TextBox;
                        DropDownList ddlPaperSize = row.FindControl("ddlPaperSize") as DropDownList;
                        TextBox txtPaperLength = row.FindControl("txtPaperLength") as TextBox;
                        TextBox txtPaperwidth = row.FindControl("txtPaperwidth") as TextBox;
                        DropDownList ddlPaperType = row.FindControl("ddlPaperType") as DropDownList;
                        DropDownList ddlPaperManufature = row.FindControl("ddlPaperManufature") as DropDownList;
                        TextBox txtPaperRate = row.FindControl("txtPaperRate") as TextBox;
                        TextBox txtAmount = row.FindControl("txtAmount") as TextBox;
                        TextBox txtPaperDetailGST = row.FindControl("txtPaperDetailGST") as TextBox;

                        dr = dt.NewRow();
                        dr["PrintComponentID"] = QuotationParameter.paperDetail;
                        dr["NoOfSheet"] = txtNoofSheet.Text.Trim() != "" ? txtNoofSheet.Text.Trim() : "0";
                        dr["GSMID"] = ddlGSM.SelectedValue != "" ? ddlGSM.SelectedValue : "0";
                        dr["GSMValue"] = txtGSMValue.Text.Trim() != "" ? txtGSMValue.Text.Trim() : "0";
                        dr["PaperSizeID"] = ddlPaperSize.SelectedValue != "" ? ddlPaperSize.SelectedValue : "0";
                        dr["PaperLength"] = txtPaperLength.Text.Trim() != "" ? txtPaperLength.Text.Trim() : "0";
                        dr["PaperWidth"] = txtPaperwidth.Text.Trim() != "" ? txtPaperwidth.Text.Trim() : "0";
                        dr["PaperTypeID"] = ddlPaperType.SelectedValue != "" ? ddlPaperType.SelectedValue : "0";
                        dr["PaperManufaturerID"] = ddlPaperManufature.SelectedValue != "" ? ddlPaperManufature.SelectedValue : "0";
                        dr["LineRate"] = txtPaperRate.Text.Trim() != "" ? txtPaperRate.Text.Trim() : "0";
                        dr["LineAmount"] = txtAmount.Text.Trim() != "" ? txtAmount.Text.Trim() : "0";
                        dr["LineGST"] = txtPaperDetailGST.Text.Trim() != "" ? txtPaperDetailGST.Text.Trim() : "0";
                        dt.Rows.Add(dr);
                    }
                }

                // Printing
                if (gvPrintingDetail.Rows.Count > 0)
                {
                    foreach (GridViewRow row in gvPrintingDetail.Rows)
                    {
                        TextBox txtNoofSheetPrintDetail = row.FindControl("txtNoofSheetPrintDetail") as TextBox;
                        DropDownList ddlPaperSizePrintDetail = row.FindControl("ddlPaperSizePrintDetail") as DropDownList;
                        TextBox txtImpression = row.FindControl("txtImpression") as TextBox;
                        DropDownList ddlCutting = row.FindControl("ddlCutting") as DropDownList;
                        TextBox txtNoofCuttingPices = row.FindControl("txtNoofCuttingPices") as TextBox;
                        TextBox txtFinishSize = row.FindControl("txtFinishSize") as TextBox;
                        DropDownList ddlSide = row.FindControl("ddlSide") as DropDownList;
                        DropDownList ddlColor = row.FindControl("ddlColor") as DropDownList;
                        DropDownList ddlPlateType = row.FindControl("ddlPlateType") as DropDownList;
                        TextBox txtPrintingQuantity = row.FindControl("txtPrintingQuantity") as TextBox;
                        DropDownList ddlMachine = row.FindControl("ddlMachine") as DropDownList;
                        TextBox txtPrintingRate = row.FindControl("txtPrintingRate") as TextBox;
                        TextBox txtPrinintgAmount = row.FindControl("txtPrinintgAmount") as TextBox;
                        TextBox txtPrintingGST = row.FindControl("txtPrintingGST") as TextBox;

                        dr = dt.NewRow();
                        dr["PrintComponentID"] = QuotationParameter.Printing;
                        dr["NoOfSheetPrintDetail"] = txtNoofSheetPrintDetail.Text.Trim() != "" ? txtNoofSheetPrintDetail.Text.Trim() : "0";
                        dr["PaperSizeIDPrintDetail"] = ddlPaperSizePrintDetail.SelectedValue != "" ? ddlPaperSizePrintDetail.SelectedValue : "0";
                        dr["Impression"] = txtImpression.Text.Trim() != "" ? txtImpression.Text.Trim() : "0";
                        dr["CuttingID"] = ddlCutting.SelectedValue != "" ? ddlCutting.SelectedValue : "0";
                        dr["CuttingPiece"] = txtNoofCuttingPices.Text.Trim() != "" ? txtNoofCuttingPices.Text.Trim() : "0";
                        dr["FinishSize"] = txtFinishSize.Text.Trim() != "" ? txtFinishSize.Text.Trim() : "0";
                        dr["SideID"] = ddlSide.SelectedValue != "" ? ddlSide.SelectedValue : "0";
                        dr["ColourID"] = ddlColor.SelectedValue != "" ? ddlColor.SelectedValue : "0";
                        dr["PlateTypeID"] = ddlPlateType.SelectedValue != "" ? ddlPlateType.SelectedValue : "0";
                        dr["LineQuantity"] = txtPrintingQuantity.Text.Trim() != "" ? txtPrintingQuantity.Text.Trim() : "0";
                        dr["MachineID"] = ddlMachine.SelectedValue != "" ? ddlMachine.SelectedValue : "0";
                        dr["LineRate"] = txtPrintingRate.Text.Trim() != "" ? txtPrintingRate.Text.Trim() : "0";
                        dr["LineAmount"] = txtPrinintgAmount.Text.Trim() != "" ? txtPrinintgAmount.Text.Trim() : "0";
                        dr["LineGST"] = txtPrintingGST.Text.Trim() != "" ? txtPrintingGST.Text.Trim() : "0";
                        dt.Rows.Add(dr);
                    }
                }

                // Fabrication Item
                foreach (GridViewRow row in gvFabricator.Rows)
                {
                    DropDownList ddlfabricationItem = row.FindControl("ddlfabricationItem") as DropDownList;
                    TextBox txtFabricationSize = row.FindControl("txtFabricationSize") as TextBox;
                    TextBox txtFabricationQuantity = row.FindControl("txtFabricationQuantity") as TextBox;
                    TextBox txtFabricationRate = row.FindControl("txtFabricationRate") as TextBox;
                    TextBox txtFabricationAmount = row.FindControl("txtFabricationAmount") as TextBox;
                    TextBox txtFabricationGST = row.FindControl("txtFabricationGST") as TextBox;

                    dr = dt.NewRow();
                    dr["PrintComponentID"] = QuotationParameter.FabricationItem;
                    dr["FabricationItemID"] = ddlfabricationItem.SelectedValue != "" ? ddlfabricationItem.SelectedValue : "0";
                    dr["FabricationItemSize"] = txtFabricationSize.Text.Trim();
                    dr["LineQuantity"] = txtFabricationQuantity.Text.Trim() != "" ? txtFabricationQuantity.Text.Trim() : "0";
                    dr["LineRate"] = txtFabricationRate.Text.Trim() != "" ? txtFabricationRate.Text.Trim() : "0";
                    dr["LineAmount"] = txtFabricationAmount.Text.Trim() != "" ? txtFabricationAmount.Text.Trim() : "0";
                    dr["LineGST"] = txtFabricationGST.Text.Trim() != "" ? txtFabricationGST.Text.Trim() : "0";
                    dt.Rows.Add(dr);
                }
                // Packaging
                foreach (GridViewRow row in gvPackaging.Rows)
                {
                    DropDownList ddlPackagingItem = row.FindControl("ddlPackagingItem") as DropDownList;
                    TextBox txtPackaingQuantity = row.FindControl("txtPackaingQuantity") as TextBox;
                    TextBox txtPackaingRate = row.FindControl("txtPackaingRate") as TextBox;
                    TextBox txtPackaingAmount = row.FindControl("txtPackaingAmount") as TextBox;
                    TextBox txtPackaingGST = row.FindControl("txtPackaingGST") as TextBox;
                    dr = dt.NewRow();
                    dr["PrintComponentID"] = QuotationParameter.Packaging;
                    dr["PackagingItemID"] = ddlPackagingItem.SelectedValue != "" ? ddlPackagingItem.SelectedValue : "0";
                    dr["LineQuantity"] = txtPackaingQuantity.Text.Trim() != "" ? txtPackaingQuantity.Text.Trim() : "0";
                    dr["LineRate"] = txtPackaingRate.Text.Trim() != "" ? txtPackaingRate.Text.Trim() : "0";
                    dr["LineAmount"] = txtPackaingAmount.Text.Trim() != "" ? txtPackaingAmount.Text.Trim() : "0";
                    dr["LineGST"] = txtPackaingGST.Text.Trim() != "" ? txtPackaingGST.Text.Trim() : "0";
                    dt.Rows.Add(dr);
                }
                // Transport
                foreach (GridViewRow row in gvTransport.Rows)
                {
                    TextBox txtLabourCharge = row.FindControl("txtLabourCharge") as TextBox;
                    TextBox txtCarFare = row.FindControl("txtCarFare") as TextBox;
                    TextBox txtTransportQty = row.FindControl("txtTransportQty") as TextBox;
                    DropDownList ddlTransportUnit = row.FindControl("ddlTransportUnit") as DropDownList;
                    TextBox txtTransportRate = row.FindControl("txtTransportRate") as TextBox;
                    TextBox txtTransportAmount = row.FindControl("txtTransportAmount") as TextBox;
                    TextBox txtTransportGST = row.FindControl("txtTransportGST") as TextBox;
                    dr = dt.NewRow();
                    dr["PrintComponentID"] = QuotationParameter.Transport;
                    dr["LabourCharge"] = txtLabourCharge.Text.Trim() != "" ? txtLabourCharge.Text.Trim() : "0";
                    dr["TransportCharge"] = txtCarFare.Text.Trim() != "" ? txtCarFare.Text.Trim() : "0";
                    dr["LineQuantity"] = txtTransportQty.Text.Trim() != "" ? txtTransportQty.Text.Trim() : "0";
                    dr["TransportUnitID"] = ddlTransportUnit.SelectedValue != "" ? ddlTransportUnit.SelectedValue : "0";
                    dr["LineRate"] = txtTransportRate.Text.Trim() != "" ? txtTransportRate.Text.Trim() : "0";
                    dr["LineAmount"] = txtTransportAmount.Text.Trim() != "" ? txtTransportAmount.Text.Trim() : "0";
                    dr["LineGST"] = txtTransportGST.Text.Trim() != "" ? txtTransportGST.Text.Trim() : "0";
                    dt.Rows.Add(dr);
                }
                // Design
                foreach (GridViewRow row in gvDesignEdit.Rows)
                {
                    DropDownList ddlOperator = row.FindControl("ddlOperator") as DropDownList;
                    TextBox txtNarration = row.FindControl("txtNarration") as TextBox;
                    TextBox txtDesignRate = row.FindControl("txtDesignRate") as TextBox;
                    TextBox txtDesignAmountGST = row.FindControl("txtDesignAmountGST") as TextBox;
                    dr = dt.NewRow();
                    dr["PrintComponentID"] = QuotationParameter.Design;
                    dr["OperatorID"] = ddlOperator.SelectedValue != "" ? ddlOperator.SelectedValue : "0";
                    dr["Narration"] = txtNarration.Text.Trim();
                    dr["LineAmount"] = txtDesignRate.Text.Trim() != "" ? txtDesignRate.Text.Trim() : "0";
                    dr["LineGST"] = txtDesignAmountGST.Text.Trim() != "" ? txtDesignAmountGST.Text.Trim() : "0";
                    dt.Rows.Add(dr);
                }
                if (Request.QueryString["ID"] != null)
                {
                    QuotationID = Convert.ToInt32(Request.QueryString["ID"].ToString());
                }
                SqlParameter[] p = new SqlParameter[19];
                p[0] = new SqlParameter("@QuotationID", QuotationID);
                p[1] = new SqlParameter("@QuotationNO", txtQuotationNo.Text.Trim());
                p[2] = new SqlParameter("@QuotationDate", DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", provider));
                p[3] = new SqlParameter("@IS_JOB", chkJobCreation.Checked);
                p[4] = new SqlParameter("@CustomerID", Convert.ToInt32(ddlClient.SelectedValue != "" ? ddlClient.SelectedValue : "0"));
                p[5] = new SqlParameter("@ItemID", Convert.ToInt32(ddlItem.SelectedValue != "" ? ddlItem.SelectedValue : "0"));
                p[6] = new SqlParameter("@ItemDescription", txtItemDescription.Text.Trim());
                p[7] = new SqlParameter("@TaxID", Convert.ToInt32(ddlGST.SelectedValue != "" ? ddlGST.SelectedValue : "0"));
                p[8] = new SqlParameter("@GSTPercentage", Convert.ToDecimal(txtGSTPercenatge.Text.Trim() != "" ? txtGSTPercenatge.Text.Trim() : "0"));
                p[9] = new SqlParameter("@GSTWOPaper", Convert.ToDecimal(txtGSTWOAmount.Text.Trim() != "" ? txtGSTWOAmount.Text.Trim() : "0"));
                p[10] = new SqlParameter("@TaxableAmount", Convert.ToDecimal(txtGSTAmount.Text.Trim() != "" ? txtGSTAmount.Text.Trim() : "0"));
                p[11] = new SqlParameter("@Discount", Convert.ToDecimal(txtBillDiscountAmount.Text.Trim() != "" ? txtBillDiscountAmount.Text.Trim() : "0"));
                p[12] = new SqlParameter("@TotalBillingAmount", Convert.ToDecimal(txtTotalBillAmount.Text.Trim() != "" ? txtTotalBillAmount.Text.Trim() : "0"));
                p[13] = new SqlParameter("@AmountPayable", Convert.ToDecimal(txtNetAmount.Text.Trim() != "" ? txtNetAmount.Text.Trim() : "0"));
                p[14] = new SqlParameter("@Remarks", "Remarks");
                p[15] = new SqlParameter("@FinYear", ApplicationUtility.CurrentSession());
                p[16] = new SqlParameter("@RequestBy", Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString()));
                p[17] = new SqlParameter("@QuotationDetail", dt);

                if (btnSave.Text == "Save")
                {
                    p[18] = new SqlParameter("@OpMode", 1);
                    if (!String.IsNullOrEmpty(txtQuotationNo.Text.Trim()))
                    {
                        var data = _dbContext.QuotationJobSheets.Where(q => q.QuotationNO == txtQuotationNo.Text.Trim()).ToList();
                        if (data.Any())
                        {
                            message = "alert('This Quotation No already exists')";
                            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                            txtQuotationNo.Focus();
                            return;
                        }
                    }
                    DataTable dtInsert = ApplicationUtility.ExecuteDataTable("USP_MANAGE_JOB_SHEET", p);
                    if (dtInsert.Rows.Count > 0)
                    {
                        if (string.IsNullOrEmpty(txtClientPhoneNo.Text) == false)
                        {
                            string smsText = "Job No : " + dtInsert.Rows[0][0].ToString() + "; " + txtItemDescription.Text.Trim() + "; Amount:" + txtNetAmount.Text;
                            string sendSMSUri = "http://sms.asaptech.net/api/sendhttp.php?authkey=221128AxHrInnSgKv5b27df67&mobiles=" + txtClientPhoneNo.Text + "&message=" + smsText.Trim() + "&sender=JYOTIG&route=4&country=91";
                            HttpWebRequest httpreq = (HttpWebRequest)WebRequest.Create(sendSMSUri);
                            HttpWebResponse httpres = (HttpWebResponse)httpreq.GetResponse();
                            StreamReader sr = new StreamReader(httpres.GetResponseStream());
                            sr.Close();
                        }
                        message = "alert('Job Card No = " + dtInsert.Rows[0][0].ToString() + " saved sucessfully')";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);



                        var data = _dbContext.USP_PAYMENT(0, DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", provider)
                       , Convert.ToInt32(ddlClient.SelectedValue != "" ? ddlClient.SelectedValue : "0")
                       , Convert.ToInt32(ddlPaymentMode.SelectedValue != "" ? ddlPaymentMode.SelectedValue : "0")
                       , Convert.ToDecimal(txtPaidAmount.Text.Trim() != "" ? txtPaidAmount.Text.Trim() : "0")
                       , txtPaymentDetail.Text
                       , Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString())
                       , ApplicationUtility.CurrentSession()
                       , 1);








                        ResetControl();
                    }
                    else
                    {
                        message = "alert('Error Occur - Unable to saved Quotation/Job Card')";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    }
                }
                else
                {
                    p[18] = new SqlParameter("@OpMode", 2);
                    int i = ApplicationUtility.ExecuteNonQuery("USP_MANAGE_JOB_SHEET", p);
                    if (i > 0)
                    {
                        message = "alert('Quotation/Job Card updated sucessfully')";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                        ResetControl();
                    }
                    else
                    {
                        message = "alert('Error Occur - Unable to updated Quotation/Job Card')";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    }
                }
            }
            catch (Exception ex)
            {
                message = "alert('" + ex.ToString() + "')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                ErrorLog.WriteToErrorLog(Convert.ToString(System.Reflection.MethodBase.GetCurrentMethod().Name), ex);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            String message = String.Empty;
            if (QuotationID == 0)
            {
                message = "alert('No Quotation/Job Note Found')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
            }
            else
            {
                try
                {
                    SqlParameter[] p = new SqlParameter[2];
                    p[0] = new SqlParameter("@OpMode", 3);
                    p[1] = new SqlParameter("@QuotationID", QuotationID);
                    int i = ApplicationUtility.ExecuteNonQuery("USP_MANAGE_JOB_SHEET", p);
                    if (i > 0)
                    {
                        message = "alert('Quotation/Job card deleted sucessfully')";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                        ResetControl();
                    }
                    else
                    {
                        message = "alert('Error Occur - Unable to delete Quotation/Job Card')";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    }
                }
                catch (Exception ex)
                {
                    message = "alert('" + ex.ToString() + "')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                }
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Transaction/JobCardList.aspx", false);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ResetControl();
        }

        protected void ddlClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlClient.SelectedIndex > -1)
            {
                int clientID = Convert.ToInt32(ddlClient.SelectedValue.ToString());
                if (clientID > 0)
                {
                    var GetData = _dbContext.Customers.Where(p => p.CustomerID == clientID).Single();
                    txtClientAddress.Text = GetData.Address;
                    txtClientPhoneNo.Text = GetData.Telephone.ToString();
                    txtClientEmail.Text = GetData.Email.ToString();
                }
                else
                {
                    txtClientAddress.Text = String.Empty;
                    txtClientPhoneNo.Text = String.Empty;
                    txtClientEmail.Text = String.Empty;
                }
            }
        }

        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlItem.SelectedIndex > -1)
            {
                int itemid = Convert.ToInt32(ddlItem.SelectedValue.ToString());
                if (itemid > 0)
                {
                    var GetData = _dbContext.PrintingParameters.Where(p => p.PrintingParameterID == itemid).Single();
                    txtItemDescription.Text = GetData.PrintingParameterDescription;
                }
                else
                {
                    txtItemDescription.Text = String.Empty;
                }
            }
        }

        protected void ddlGST_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlGST.SelectedIndex > -1)
            {
                int MasterId = Convert.ToInt32(ddlGST.SelectedValue.ToString());
                if (MasterId > 0)
                {
                    var GetData = _dbContext.PrintingParameters.Where(p => p.PrintingParameterID == MasterId).Single();
                    txtGSTPercenatge.Text = GetData.ParameterValue1.ToString();
                    txtGSTAmount.Text = "0.00";
                }
                else
                {
                    txtGSTPercenatge.Text = "0.00";
                    txtGSTAmount.Text = "0.00";
                }
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GSTCalculation()", true);
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GSTCalculation()", true);
        }

        protected void btnCreateNewCustomer_Click(object sender, EventArgs e)
        {
            txtNewCustomerName.Text = string.Empty;
            txtNewCustomerAddress.Text = string.Empty;
            txtNewCustomerPhoneNo.Text = string.Empty;
            txtNewCustomerEmail.Text = string.Empty;
            txtNewCustomerGST.Text = string.Empty;
            //DataTable dt = new DataTable();
            //var data = (from p in _dbContext.Customers
            //            orderby p.CustomerName
            //            select new { p.CustomerID, p.CustomerName, p.Address, p.Telephone, p.GSTNo }).ToList(); ;
            //dt = ApplicationUtility.ListToDataTable(data);
            //gvCustomer.DataSource = dt;
            //gvCustomer.DataBind();
            this.ModalPopupExtender1.Show();
        }

        protected void btnRefreshPrintingDetail_Click(object sender, EventArgs e)
        {
            btnPopulatePrintingDetail.Enabled = true;
            btnRefreshPrintingDetail.Enabled = false;
            gvPaperDetail.Enabled = true;
            gvPrintingDetail.DataSource = null;
            gvPrintingDetail.DataBind();
        }

        protected void btnPopulatePrintingDetail_Click(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            DataRow dr = null;
            //Define the Columns
            dt.Columns.Add(new DataColumn("NoOfSheetPrintDetail", typeof(Int32)));
            dt.Columns.Add(new DataColumn("PaperSizeIDPrintDetail", typeof(Int32)));
            dt.Columns.Add(new DataColumn("Impression", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("CuttingID", typeof(Int32)));
            dt.Columns.Add(new DataColumn("NoofCuttingPices", typeof(Int32)));
            dt.Columns.Add(new DataColumn("FinishSize", typeof(String)));
            dt.Columns.Add(new DataColumn("SideID", typeof(Int32)));
            dt.Columns.Add(new DataColumn("ColourID", typeof(Int32)));
            dt.Columns.Add(new DataColumn("PlateTypeID", typeof(Int32)));
            dt.Columns.Add(new DataColumn("LineQuantity", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("MachineID", typeof(Int32)));
            dt.Columns.Add(new DataColumn("LineRate", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("LineAmount", typeof(Decimal)));
            dt.Columns.Add(new DataColumn("LineGST", typeof(Decimal)));

            foreach (GridViewRow row in gvPaperDetail.Rows)
            {
                TextBox txtNoofSheet = row.FindControl("txtNoofSheet") as TextBox;
                DropDownList ddlPaperSize = row.FindControl("ddlPaperSize") as DropDownList;

                dr = dt.NewRow();
                dr["NoOfSheetPrintDetail"] = txtNoofSheet.Text.Trim() != "" ? txtNoofSheet.Text.Trim() : "0";
                dr["PaperSizeIDPrintDetail"] = ddlPaperSize.SelectedValue != "" ? ddlPaperSize.SelectedValue : "0";
                dr["Impression"] = 0;
                dr["CuttingID"] = 0;
                dr["NoofCuttingPices"] = 0;
                dr["FinishSize"] = 0;
                dr["SideID"] = 0;
                dr["ColourID"] = 0;
                dr["PlateTypeID"] = 0;
                dr["LineQuantity"] = 1;
                dr["MachineID"] = 0;
                dr["LineRate"] = 0;
                dr["LineAmount"] = 0;
                dr["LineGST"] = 0;
                dt.Rows.Add(dr);
            }
            if (dt.Rows.Count < 0)
            {
                String message;
                message = "alert('No rows found to add')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
            }
            else
            {
                gvPaperDetail.Enabled = false;
                gvPrintingDetail.DataSource = dt;
                gvPrintingDetail.DataBind();
                btnPopulatePrintingDetail.Enabled = false;
                btnRefreshPrintingDetail.Enabled = true;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DropDownList ddlPaperSizePrintDetail = gvPrintingDetail.Rows[i].FindControl("ddlPaperSizePrintDetail") as DropDownList;
                    Utility.FillMasterDataDropDownlist(ddlPaperSizePrintDetail, MasterType.paperSize);
                    ddlPaperSizePrintDetail.SelectedValue = dt.Rows[i]["PaperSizeIDPrintDetail"].ToString();

                    DropDownList ddlCutting = gvPrintingDetail.Rows[i].FindControl("ddlCutting") as DropDownList;
                    Utility.FillMasterDataDropDownlist(ddlCutting, MasterType.cutting);
                    DropDownList ddlSide = gvPrintingDetail.Rows[i].FindControl("ddlSide") as DropDownList;
                    Utility.FillMasterDataDropDownlist(ddlSide, MasterType.paperSide);
                    DropDownList ddlColor = gvPrintingDetail.Rows[i].FindControl("ddlColor") as DropDownList;
                    Utility.FillMasterDataDropDownlist(ddlColor, MasterType.color);
                    DropDownList ddlPlateType = gvPrintingDetail.Rows[i].FindControl("ddlPlateType") as DropDownList;
                    Utility.FillMasterDataDropDownlist(ddlPlateType, MasterType.plateType);
                    DropDownList ddlMachine = gvPrintingDetail.Rows[i].FindControl("ddlMachine") as DropDownList;
                    Utility.FillMasterDataDropDownlist(ddlMachine, MasterType.machine);
                }
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GSTCalculation()", true);
            }
        }

        protected void BtnNewCustomer_Click(object sender, EventArgs e)
        {
            String message;
            var data = _dbContext.Customers.Where(p => p.CustomerName == txtNewCustomerName.Text && p.GSTNo == txtNewCustomerGST.Text.Trim()).ToList();
            if (data.Any())
            {
                message = "alert('This Customer already exists')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
            }
            else
            {
                Customer para = new Customer();
                para.CustomerName = txtNewCustomerName.Text.Trim().ToUpper();
                para.Address = txtNewCustomerAddress.Text.Trim().ToUpper();
                para.Telephone = txtNewCustomerPhoneNo.Text.Trim();
                para.Email = txtNewCustomerEmail.Text.Trim().ToLower();
                para.GSTNo = txtNewCustomerGST.Text.Trim().ToUpper();
                para.RecMode = "A";
                para.AddEditBy = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
                para.AddEditOn = System.DateTime.Now;
                _dbContext.Customers.Add(para);
                _dbContext.SaveChanges();
                message = "alert('New Customer Added sucessfully')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                FillClientList();
                this.ModalPopupExtender1.Hide();
            }
        }

        protected void btnSearchCustomer_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            if (String.IsNullOrEmpty(txtSearchbyCustomer.Text))
            {
                var data = (from p in _dbContext.Customers
                            where p.RecMode != "D"
                            orderby p.CustomerName ascending
                            select new { p.CustomerID, p.CustomerName, p.Address, p.Telephone, p.GSTNo }).ToList();
                gvCustomer.DataSource = data;
                gvCustomer.DataBind();
            }
            else if (!String.IsNullOrEmpty(txtSearchbyCustomer.Text) && ddlSearchByCustomer.SelectedItem.Text == "Name")
            {
                var data = (from p in _dbContext.Customers
                            where p.CustomerName.Contains(txtSearchbyCustomer.Text.Trim()) && p.RecMode != "D"
                            orderby p.CustomerName ascending
                            select new { p.CustomerID, p.CustomerName, p.Address, p.Telephone, p.GSTNo }).ToList();
                gvCustomer.DataSource = data;
                gvCustomer.DataBind();
            }
            else if (!String.IsNullOrEmpty(txtSearchbyCustomer.Text) && ddlSearchByCustomer.SelectedItem.Text == "Telephone")
            {
                var data = (from p in _dbContext.Customers
                            where p.Telephone.Contains(txtSearchbyCustomer.Text.Trim()) && p.RecMode != "D"
                            orderby p.CustomerName ascending
                            select new { p.CustomerID, p.CustomerName, p.Address, p.Telephone, p.GSTNo }).ToList();
                gvCustomer.DataSource = data;
                gvCustomer.DataBind();
            }
            else if (!String.IsNullOrEmpty(txtSearchbyCustomer.Text) && ddlSearchByCustomer.SelectedItem.Text == "GST No")
            {
                var data = (from p in _dbContext.Customers
                            where p.GSTNo.Contains(txtSearchbyCustomer.Text.Trim()) && p.RecMode != "D"
                            orderby p.CustomerName ascending
                            select new { p.CustomerID, p.CustomerName, p.Address, p.Telephone, p.GSTNo }).ToList();
                gvCustomer.DataSource = data;
                gvCustomer.DataBind();
            }
            this.ModalPopupExtender1.Show();
        }

        protected void gvCustomer_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void gvCustomer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int CustomerID = Convert.ToInt32(e.CommandArgument.ToString());
                ddlClient.SelectedValue = CustomerID.ToString();
            }
        }

        protected void gvCustomer_PageIndexChanged(object sender, EventArgs e)
        {

        }

        //protected void btn_Click(object sender, EventArgs e)
        //{

        //    Button btn = sender as Button;
        //    switch (btn.ID)
        //    {
        //        case "btnNewGSM":
        //            _MasterTypeID = MasterType.gsm;
        //            lblFormHeader.Text = "GSM MEASURMENT";
        //            lblMasterParam.Text = "GSM";
        //            lblParamValue1.Text = "VALUE";
        //            ShowhideControl(true, false, true, false, false, false); // GSM
        //            break;
        //        case "btnNewPaperSize":
        //            _MasterTypeID = MasterType.paperSize;
        //            lblFormHeader.Text = "PAPER SIZE";
        //            lblMasterParam.Text = "SIZE";
        //            lblParamValue1.Text = "LENGTH";
        //            lblParamValue2.Text = "WIDTH";
        //            ShowhideControl(true, false, true, true, false, false); // PAPER SIZE
        //            break;
        //        case "btnNewPaperType":
        //            _MasterTypeID = MasterType.paperType;
        //            lblFormHeader.Text = "PAPER TYPE";
        //            lblMasterParam.Text = "TYPE";
        //            ShowhideControl(true, false, false, false, false, false);  // PAPER TYPE
        //            break;
        //        case "btnNewPaperManufature":
        //            lblFormHeader.Text = "PAPER COMPANY";
        //            lblMasterParam.Text = "COMPANY";
        //            ShowhideControl(true, false, false, false, false, true); //PAPER COMPANY
        //            _MasterTypeID = MasterType.paperCompany;
        //            break;
        //    }            
        //    this.mndlPopupMaster.Show();
        //}

        //private void ShowhideControl(Boolean PrintingParameterName, Boolean PrintingParameterDescription, Boolean ParameterValue1, Boolean ParameterValue2, Boolean ParameterValue3, Boolean SellPrice)
        //{
        //    txtMasterParam.Text = String.Empty;
        //    txtParamValue1.Text = "0.00";
        //    txtParamValue2.Text = "0.00";
        //    txtParamValue3.Text = "0.00";
        //    txtMasterParamDescription.Text = String.Empty;
        //    txtMRP.Text = "0.00";

        //    lblMasterParam.Visible = PrintingParameterName;
        //    txtMasterParam.Visible = PrintingParameterName;
        //    lblMasterParamDescription.Visible = PrintingParameterDescription;
        //    txtMasterParamDescription.Visible = PrintingParameterDescription;
        //    lblParamValue1.Visible = ParameterValue1;
        //    txtParamValue1.Visible = ParameterValue1;
        //    lblParamValue2.Visible = ParameterValue2;
        //    txtParamValue2.Visible = ParameterValue2;
        //    lblParamValue3.Visible = ParameterValue3;
        //    txtParamValue3.Visible = ParameterValue3;
        //    lblMRP.Visible = SellPrice;
        //    txtMRP.Visible = SellPrice;
        //}

        //protected void btnMasterData_Click(object sender, EventArgs e)
        //{
        //    String message;
        //    var data = _dbContext.PrintingParameters.Where(p => p.PrintingParameterName == txtMasterParam.Text && p.PrintingParameterID == _MasterTypeID).ToList();
        //    if (data.Any())
        //    {
        //        message = "alert('This Master Data already exists')";
        //        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
        //    }
        //    else
        //    {
        //        PrintingParameter para = new PrintingParameter();
        //        para.PrintingParameterTypeID = Convert.ToInt32(Request.QueryString["ID"].ToString());
        //        para.PrintingParameterName = txtMasterParam.Text.Trim().ToUpper();
        //        para.ParameterValue1 = Convert.ToDecimal(txtParamValue1.Text.Trim() != "" ? txtParamValue1.Text.Trim() : "0");
        //        para.ParameterValue2 = Convert.ToDecimal(txtParamValue2.Text.Trim() != "" ? txtParamValue2.Text.Trim() : "0");
        //        para.ParameterValue3 = Convert.ToDecimal(txtParamValue3.Text.Trim() != "" ? txtParamValue3.Text.Trim() : "0");
        //        para.PrintingParameterDescription = txtMasterParamDescription.Text.Trim();
        //        para.SellPrice = Convert.ToDecimal(txtMRP.Text.Trim() != "" ? txtMRP.Text.Trim() : "0");
        //        para.SellPrice = 0;
        //        para.RecMode = "A";
        //        para.CreateBy = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        //        para.CreatedDate = System.DateTime.Now;
        //        _dbContext.PrintingParameters.Add(para);
        //        _dbContext.SaveChanges();
        //        message = "alert('This Master Data saved sucessfully')";
        //        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
        //        txtMasterParam.Text = String.Empty;
        //        txtParamValue1.Text = "0.00";
        //        txtParamValue2.Text = "0.00";
        //        txtParamValue3.Text = "0.00";
        //        txtMasterParamDescription.Text = String.Empty;
        //        txtMRP.Text = "0.00";
        //        this.mndlPopupMaster.Hide();
        //    }
        //}
    }
}