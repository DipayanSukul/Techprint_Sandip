using MyUtil.CommonMethod;
using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace TechPrint.Transaction
{
    public partial class MRPManage : System.Web.UI.Page
    {
        TechPrintEntities dbContext = new TechPrintEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ApplicationUtility.FillMasterDataDropDownlist(ddlPaperSize, MasterType.paperSize);
                ApplicationUtility.FillMasterDataDropDownlist(ddlGSM, MasterType.gsm);
                ApplicationUtility.FillMasterDataDropDownlist(ddlPaperCompany, MasterType.paperCompany);
                ApplicationUtility.FillMasterDataDropDownlist(ddlPaperType, MasterType.paperType);
                ResetControl();
            }
        }

        private void ResetControl()
        {
            hidPrintingParameterID.Value = "0";
            ddlPaperSize.SelectedIndex = -1;
            ddlGSM.SelectedIndex = -1;
            ddlPaperCompany.SelectedIndex = -1;
            ddlPaperType.SelectedIndex = -1;
            txtMRP.Text = "0.00";
            FillGridView();
            btnSave.Text = "Save";
        }

        private void FillGridView()
        {
            var data = (from PR in dbContext.PrintingParameters
                        join PS in dbContext.PrintingParameters on PR.ParameterValue1 equals PS.PrintingParameterID
                        join GSM in dbContext.PrintingParameters on PR.ParameterValue2 equals GSM.PrintingParameterID
                        join PC in dbContext.PrintingParameters on PR.ParameterValue3 equals PC.PrintingParameterID
                        join PT in dbContext.PrintingParameters on PR.ParameterValue4 equals PT.PrintingParameterID
                        orderby PS.PrintingParameterName, GSM.PrintingParameterName, PT.PrintingParameterName, PC.PrintingParameterName
                        where PR.PrintingParameterTypeID == MasterType.paperRate
                        select new
                        {
                            PR.PrintingParameterID,
                            PaperCompany = PC.PrintingParameterName.ToUpper(),
                            PaperSize = PS.PrintingParameterName,
                            GSM = GSM.PrintingParameterName,
                            PaperType = PT.PrintingParameterName.ToUpper(),
                            MRP = PR.SellPrice
                        }).ToList();
            gvParamMRP.DataSource = data;
            gvParamMRP.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            String message = String.Empty;
            try
            {
                Int32 PaperSize = Convert.ToInt32(ddlPaperSize.SelectedValue != "" ? ddlPaperSize.SelectedValue : "0");
                Int32 GSM = Convert.ToInt32(ddlGSM.SelectedValue != "" ? ddlGSM.SelectedValue : "0");
                Int32 PaperCompany = Convert.ToInt32(ddlPaperCompany.SelectedValue != "" ? ddlPaperCompany.SelectedValue : "0");
                Int32 PaperType = Convert.ToInt32(ddlPaperType.SelectedValue != "" ? ddlPaperType.SelectedValue : "0");

                if (Convert.ToInt16(hidPrintingParameterID.Value) == 0)
                {
                    var data = dbContext.PrintingParameters.Where(p => p.ParameterValue1 == PaperSize && p.ParameterValue2 == GSM && p.ParameterValue3 == PaperCompany && p.ParameterValue4 == PaperType && p.PrintingParameterTypeID == MasterType.paperRate).ToList();
                    if (data.Any())
                    {
                        message = "alert('This Master Data already exists')";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    }
                    else
                    {
                        PrintingParameter para = new PrintingParameter();
                        para.PrintingParameterTypeID = MasterType.paperRate;
                        para.PrintingParameterName = "PAPER_RATE";
                        para.ParameterValue1 = PaperSize;
                        para.ParameterValue2 = GSM;
                        para.ParameterValue3 = PaperCompany;
                        para.ParameterValue4 = PaperType;
                        para.SellPrice = Convert.ToDecimal(txtMRP.Text.Trim() != "" ? txtMRP.Text.Trim() : "0");
                        para.RecMode = "A";
                        para.CreateBy = Convert.ToInt32(Session["UserID"]);
                        para.CreatedDate = DateTime.Now;
                        dbContext.PrintingParameters.Add(para);
                        dbContext.SaveChanges();
                        message = "alert('Paper rate inserted successfully')";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                        ResetControl();
                    }
                }
                else if (Convert.ToInt16(hidPrintingParameterID.Value) > 0)
                {
                    int printingParameterID = Convert.ToInt32(hidPrintingParameterID.Value);
                    var q = dbContext.PrintingParameters.Where(p => p.ParameterValue1 == PaperSize && p.ParameterValue2 == GSM && p.ParameterValue3 == PaperCompany && p.ParameterValue4 == PaperType && p.PrintingParameterTypeID == MasterType.paperRate && p.PrintingParameterID != printingParameterID).ToList();
                    if (q.Any())
                    {
                        message = "alert('This Master Data already exists')";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    }
                    else
                    {
                        var data = dbContext.PrintingParameters.Where(p => p.PrintingParameterID == printingParameterID).SingleOrDefault();

                        data.SellPrice = Convert.ToDecimal(txtMRP.Text.Trim() != "" ? txtMRP.Text.Trim() : "0");
                        data.RecMode = "M";
                        data.LMB = Convert.ToInt32(Session["UserID"]);
                        data.LMD = DateTime.Now;
                        dbContext.SaveChanges();
                        message = "alert('Paper rate updated successfully')";
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

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            String message = String.Empty;
            if (hidPrintingParameterID.Value == "0")
            {
                message = "alert('No Parameter Found')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
            }
            else
            {
                int MasterId = Convert.ToInt32(hidPrintingParameterID.Value.Trim() != "" ? hidPrintingParameterID.Value.Trim() : "0");
                if (!IsMasterDataUse(MasterId))
                {
                    var data = dbContext.PrintingParameters.Where(p => p.PrintingParameterID == MasterId).SingleOrDefault();
                    data.RecMode = "D";
                    data.DeleteBy = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]);
                    data.DeleteDate = System.DateTime.Now;
                    dbContext.SaveChanges();
                    message = "alert('Master Data Deleted')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    ResetControl();
                }
                else
                {
                    message = "alert('SORRY! This master data already use in Transaction so you cannot delete this record.')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                }
            }
        }

        private Boolean IsMasterDataUse(int MasterId)
        {
            Boolean InUse = false;
            try
            {
                int MasterTypeId = 0;
                var GetData = dbContext.PrintingParameters.Where(p => p.PrintingParameterID == MasterId).Single();
                if (GetData != null)
                {
                    MasterTypeId = Convert.ToInt32(GetData.PrintingParameterTypeID);
                }

                if (MasterTypeId == MasterType.paperType)
                {
                    if (dbContext.QuotationJobSheetDetails.Where(p => p.PaperTypeID == MasterId).Any())
                    {
                        InUse = true;
                    }
                }
                else if (MasterTypeId == MasterType.paperCompany)
                {
                    if (dbContext.QuotationJobSheetDetails.Where(p => p.PaperManufaturerID == MasterId).Any())
                    {
                        InUse = true;
                    }
                }
                else if (MasterTypeId == MasterType.machine)
                {
                    if (dbContext.QuotationJobSheetDetails.Where(p => p.MachineID == MasterId).Any())
                    {
                        InUse = true;
                    }
                }
                else if (MasterTypeId == MasterType.plateType)
                {
                    if (dbContext.QuotationJobSheetDetails.Where(p => p.PlateTypeID == MasterId).Any())
                    {
                        InUse = true;
                    }
                    else if (dbContext.PrintRateCalculations.Where(p => p.PlateID == MasterId).Any())
                    {
                        InUse = true;
                    }
                }
                else if (MasterTypeId == MasterType.paperSide)
                {
                    if (dbContext.QuotationJobSheetDetails.Where(p => p.SideID == MasterId).Any())
                    {
                        InUse = true;
                    }
                }
                else if (MasterTypeId == MasterType.paperSize)
                {
                    if (dbContext.QuotationJobSheetDetails.Where(p => p.PaperSizeID == MasterId).Any())
                    {
                        InUse = true;
                    }
                }
                else if (MasterTypeId == MasterType.color)
                {
                    if (dbContext.QuotationJobSheetDetails.Where(p => p.ColourID == MasterId).Any())
                    {
                        InUse = true;
                    }
                    else if (dbContext.PrintRateCalculations.Where(p => p.ColorID == MasterId).Any())
                    {
                        InUse = true;
                    }
                }
                else if (MasterTypeId == MasterType.packingItem)
                {
                    if (dbContext.QuotationJobSheetDetails.Where(p => p.PackagingItemID == MasterId).Any())
                    {
                        InUse = true;
                    }
                }
                else if (MasterTypeId == MasterType.gsm)
                {
                    if (dbContext.QuotationJobSheetDetails.Where(p => p.GSMID == MasterId).Any())
                    {
                        InUse = true;
                    }
                }
                else if (MasterTypeId == MasterType.cutting)
                {
                    if (dbContext.QuotationJobSheetDetails.Where(p => p.CuttingID == MasterId).Any())
                    {
                        InUse = true;
                    }
                    else if (dbContext.PrintRateCalculations.Where(p => p.CuttingSizeID == MasterId).Any())
                    {
                        InUse = true;
                    }
                }
                else if (MasterTypeId == MasterType.fabricationItem)
                {
                    if (dbContext.QuotationJobSheetDetails.Where(p => p.FabricationItemID == MasterId).Any())
                    {
                        InUse = true;
                    }
                }
                else if (MasterTypeId == MasterType.unit)
                {
                    if (dbContext.QuotationJobSheetDetails.Where(p => p.TransportUnitID == MasterId).Any())
                    {
                        InUse = true;
                    }
                }
                else if (MasterTypeId == MasterType.operatorprint)
                {
                    if (dbContext.QuotationJobSheetDetails.Where(p => p.OperatorID == MasterId).Any())
                    {
                        InUse = true;
                    }
                }
                else if (MasterTypeId == MasterType.tax)
                {
                    if (dbContext.QuotationJobSheets.Where(p => p.TaxID == MasterId).Any())
                    {
                        InUse = true;
                    }
                }
                else if (MasterTypeId == MasterType.itemDescription)
                {
                    if (dbContext.QuotationJobSheets.Where(p => p.ItemID == MasterId).Any())
                    {
                        InUse = true;
                    }
                }
            }
            catch
            {
                // Nothing
            }
            return InUse;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ResetControl();
        }

        protected void gvParamMRP_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            gvParamMRP.PageIndex = e.NewPageIndex;
            FillGridView();
        }

        protected void gvParamMRP_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {

        }

        protected void gvParamMRP_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                int printingParameterID = Convert.ToInt32(e.CommandArgument.ToString());
                hidPrintingParameterID.Value = printingParameterID.ToString();
                var GetData = dbContext.PrintingParameters.Where(p => p.PrintingParameterID == printingParameterID).Single();

                ddlPaperSize.SelectedValue = Convert.ToInt32(GetData.ParameterValue1).ToString();
                ddlGSM.SelectedValue = Convert.ToInt32(GetData.ParameterValue2).ToString();
                ddlPaperCompany.SelectedValue = Convert.ToInt32(GetData.ParameterValue3).ToString();
                ddlPaperType.SelectedValue = Convert.ToInt32(GetData.ParameterValue4).ToString();
                txtMRP.Text = GetData.SellPrice.ToString();
                btnSave.Text = "Update";
            }
        }

        protected void btnUploadPaperRate_Click(object sender, EventArgs e)
        {
            //try
            //{


                if (string.IsNullOrEmpty(fup1.PostedFile.FileName) == false)
                {
                    btnSave.Enabled = false;
                    btnReset.Enabled = false;
                    btnUploadPaperRate.Enabled = false;
                    fup1.Enabled = false;
                    fup1.SaveAs(Server.MapPath(fup1.PostedFile.FileName));
                    string path = Server.MapPath(fup1.PostedFile.FileName);
                    System.Data.OleDb.OleDbConnection oledbConn = new System.Data.OleDb.OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';");
                    oledbConn.Open();
                    System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand();
                    System.Data.OleDb.OleDbDataAdapter oleda = new System.Data.OleDb.OleDbDataAdapter();
                    DataSet ds = new DataSet();

                    cmd.Connection = oledbConn;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = @"SELECT [Paper Size] AS PaperSize, GSM, [Paper Type] AS PaperType, [Paper Company] AS PaperCompany, 
                                            IIF(Rate IS NULL, 0, Rate) AS Rate
                                    FROM [PaperRate$]";
                    oleda = new System.Data.OleDb.OleDbDataAdapter(cmd);
                    oleda.Fill(ds);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Int32 paperSizeID = 0;
                        Int32 gsmID = 0;
                        Int32 paperTypeID = 0;
                        Int32 paperCompanyID = 0;

                        string paperSize = dr["PaperSize"].ToString();
                        if (string.IsNullOrEmpty(paperSize) == false)
                        {
                            paperSizeID = (from PP in dbContext.PrintingParameters
                                           where PP.PrintingParameterName == paperSize.Trim() && PP.PrintingParameterTypeID == MasterType.paperSize
                                           select (PP)).FirstOrDefault().PrintingParameterID;
                        }

                        string gsm = dr["GSM"].ToString();
                        if (string.IsNullOrEmpty(gsm) == false)
                        {
                            gsmID = (from PP in dbContext.PrintingParameters
                                     where PP.PrintingParameterName == gsm.Trim() && PP.PrintingParameterTypeID == MasterType.gsm
                                     select (PP)).FirstOrDefault().PrintingParameterID;
                        }

                        string paperType = dr["PaperType"].ToString();
                        if (string.IsNullOrEmpty(paperType) == false)
                        {
                            paperTypeID = (from PP in dbContext.PrintingParameters
                                           where PP.PrintingParameterName == paperType.Trim() && PP.PrintingParameterTypeID == MasterType.paperType
                                           select (PP)).FirstOrDefault().PrintingParameterID;
                        }

                        string paperCompany = dr["PaperCompany"].ToString();
                        if (string.IsNullOrEmpty(paperCompany) == false)
                        {
                            paperCompanyID = (from PP in dbContext.PrintingParameters
                                              where PP.PrintingParameterName == paperCompany.Trim() && PP.PrintingParameterTypeID == MasterType.paperCompany
                                              select (PP)).FirstOrDefault().PrintingParameterID;
                        }

                        string rate = dr["Rate"].ToString();
                        if (paperSizeID > 0 && gsmID > 0 && paperTypeID > 0 && paperCompanyID > 0 && Convert.ToDecimal(rate) > 0)
                        {
                            PrintingParameter objPP = (from PP in dbContext.PrintingParameters
                                                       where PP.ParameterValue1 == paperSizeID &&
                                                       PP.ParameterValue2 == gsmID &&
                                                       PP.ParameterValue3 == paperCompanyID &&
                                                       PP.ParameterValue4 == paperTypeID &&
                                                       PP.PrintingParameterTypeID == MasterType.paperRate
                                                       select (PP)).FirstOrDefault();
                            if (objPP == null)
                            {
                                PrintingParameter para = new PrintingParameter();
                                para.PrintingParameterTypeID = MasterType.paperRate;
                                para.PrintingParameterName = "PAPER_RATE";
                                para.ParameterValue1 = paperSizeID;
                                para.ParameterValue2 = gsmID;
                                para.ParameterValue3 = paperCompanyID;
                                para.ParameterValue4 = paperTypeID;
                                para.SellPrice = Convert.ToDecimal(rate);
                                para.RecMode = "A";
                                para.CreateBy = Convert.ToInt32(Session["UserID"]);
                                para.CreatedDate = DateTime.Now;
                                dbContext.PrintingParameters.Add(para);
                                dbContext.SaveChanges();
                            }
                            else
                            {
                                objPP.SellPrice = Convert.ToDecimal(rate);
                                objPP.RecMode = "M";
                                objPP.LMB = Convert.ToInt32(Session["UserID"]);
                                objPP.LMD = DateTime.Now;
                                dbContext.SaveChanges();
                            }
                        }
                    }

                    btnSave.Enabled = true;
                    btnReset.Enabled = true;
                    btnUploadPaperRate.Enabled = true;
                    fup1.Enabled = true;

                    string message = "alert('Paper rate uploaded successfully')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    Response.Redirect("MRPManage.aspx");
                }
            //}
            //catch (Exception ex)
            //{
            //    string message = "alert('" + ex.ToString() + "')";
            //    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
            //    ErrorLog.WriteToErrorLog(Convert.ToString(System.Reflection.MethodBase.GetCurrentMethod().Name), ex);
            //    throw ex;
            //}
        }

        protected void ddlPaperCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 paperSizeID = Convert.ToInt32(ddlPaperSize.SelectedValue);
            Int32 gsmID = Convert.ToInt32(ddlGSM.SelectedValue);
            Int32 paperCompanyID = Convert.ToInt32(ddlPaperCompany.SelectedValue);
            Int32 paperTypeID = Convert.ToInt32(ddlPaperType.SelectedValue);

            PrintingParameter objPP = (from PP in dbContext.PrintingParameters
                                       where PP.ParameterValue1 == paperSizeID &&
                                       PP.ParameterValue2 == gsmID &&
                                       PP.ParameterValue3 == paperCompanyID &&
                                       PP.ParameterValue4 == paperTypeID &&
                                       PP.PrintingParameterTypeID == MasterType.paperRate
                                       select (PP)).FirstOrDefault();
            if (objPP != null)
            {
                hidPrintingParameterID.Value = objPP.PrintingParameterID.ToString();

                ddlPaperSize.SelectedValue = Convert.ToInt32(objPP.ParameterValue1).ToString();
                ddlGSM.SelectedValue = Convert.ToInt32(objPP.ParameterValue2).ToString();
                ddlPaperCompany.SelectedValue = Convert.ToInt32(objPP.ParameterValue3).ToString();
                ddlPaperType.SelectedValue = Convert.ToInt32(objPP.ParameterValue4).ToString();
                txtMRP.Text = objPP.SellPrice.ToString();
                btnSave.Text = "Update";
            }
        }
    }
}