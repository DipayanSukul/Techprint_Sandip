using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TechPrint.Transaction
{
    public partial class MasterData : System.Web.UI.Page
    {
        TechPrintEntities _dbContext = new TechPrintEntities();
        private String _MasterTypeID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ResetControl();
                _MasterTypeID = Request.QueryString["ID"].ToString();
            }
        }

        private void ResetControl()
        {
            FillGridView();
            hidPrintingParameterID.Value = "0";
            txtMasterParam.Text = String.Empty;
            txtParamValue1.Text = "0.00";
            txtParamValue2.Text = "0.00";
            txtParamValue3.Text = "0.00";
            txtMasterParamDescription.Text = String.Empty;
            txtMRP.Text = "0.00";
            btnSave.Text = "Save";
        }

        private void FillGridView()
        {
            try
            {
                int _MasterTypeID = Convert.ToInt32(Request.QueryString["ID"].ToString());
                DataTable dt = new DataTable();
                var data = _dbContext.USP_BIND_MASTER_DATA(_MasterTypeID);
                dt = ApplicationUtility.ListToDataTable(data);
                gvMasterParam.DataSource = dt;

                if (_MasterTypeID == MasterType.paperType) // PAPER TYPE
                {
                    lblFormHeader.Text = "PAPER TYPE";
                    lblMasterParam.Text = "TYPE";
                    ShowhideControl(true, false, false, false, false, false);

                    gvMasterParam.Columns[0].HeaderText = "PAPER TYPE";
                    gvMasterParam.Columns[1].Visible = false;
                    gvMasterParam.Columns[1].HeaderText = "";
                    gvMasterParam.Columns[2].Visible = false;
                    gvMasterParam.Columns[2].HeaderText = "";
                    gvMasterParam.Columns[3].HeaderText = "";
                    gvMasterParam.Columns[3].Visible = false;
                    gvMasterParam.Columns[4].HeaderText = "";
                    gvMasterParam.Columns[4].Visible = false;
                    gvMasterParam.Columns[5].HeaderText = String.Empty;
                    gvMasterParam.Columns[5].Visible = false;
                }
                else if (_MasterTypeID == MasterType.paperCompany) //PAPER COMPANY
                {
                    lblFormHeader.Text = "PAPER COMPANY";
                    lblMasterParam.Text = "COMPANY";
                    ShowhideControl(true, false, false, false, false, true);

                    gvMasterParam.Columns[0].HeaderText = "PAPER COMPANY";
                    gvMasterParam.Columns[1].Visible = false;
                    gvMasterParam.Columns[1].HeaderText = "";
                    gvMasterParam.Columns[2].Visible = false;
                    gvMasterParam.Columns[2].HeaderText = "";
                    gvMasterParam.Columns[3].HeaderText = "";
                    gvMasterParam.Columns[3].Visible = false;
                    gvMasterParam.Columns[4].HeaderText = "MRP";
                    gvMasterParam.Columns[5].HeaderText = String.Empty;
                    gvMasterParam.Columns[5].Visible = false;
                }
                else if (_MasterTypeID == MasterType.machine) // MACHINE
                {
                    lblFormHeader.Text = "MACHINE PANEL";
                    lblMasterParam.Text = "MACHINE";
                    ShowhideControl(true, false, false, false, false, false);

                    gvMasterParam.Columns[0].HeaderText = "MACHINE";
                    gvMasterParam.Columns[1].Visible = false;
                    gvMasterParam.Columns[1].HeaderText = "";
                    gvMasterParam.Columns[2].Visible = false;
                    gvMasterParam.Columns[2].HeaderText = "";
                    gvMasterParam.Columns[3].HeaderText = "";
                    gvMasterParam.Columns[3].Visible = false;
                    gvMasterParam.Columns[4].HeaderText = "";
                    gvMasterParam.Columns[4].Visible = false;
                    gvMasterParam.Columns[5].HeaderText = String.Empty;
                    gvMasterParam.Columns[5].Visible = false;
                }
                else if (_MasterTypeID == MasterType.plateType) // PLATE TYPE
                {
                    lblFormHeader.Text = "PLATE TYPE";
                    lblMasterParam.Text = "PLATE";
                    ShowhideControl(true, false, false, false, false, false);

                    gvMasterParam.Columns[0].HeaderText = "PLATE TYPE";
                    gvMasterParam.Columns[1].Visible = false;
                    gvMasterParam.Columns[1].HeaderText = "";
                    gvMasterParam.Columns[2].Visible = false;
                    gvMasterParam.Columns[2].HeaderText = "";
                    gvMasterParam.Columns[3].HeaderText = "";
                    gvMasterParam.Columns[3].Visible = false;
                    gvMasterParam.Columns[4].HeaderText = "";
                    gvMasterParam.Columns[4].Visible = false;
                    gvMasterParam.Columns[5].HeaderText = String.Empty;
                    gvMasterParam.Columns[5].Visible = false;
                }
                else if (_MasterTypeID == MasterType.paperSide) // PAPER SIDE
                {
                    lblFormHeader.Text = "PAPER SIDE";
                    lblMasterParam.Text = "SIDE";
                    lblParamValue1.Text = "VALUE";
                    ShowhideControl(true, false, true, false, false, false);

                    gvMasterParam.Columns[0].HeaderText = "PAPER SIDE";
                    gvMasterParam.Columns[1].Visible = true;
                    gvMasterParam.Columns[1].HeaderText = "VALUE";
                    gvMasterParam.Columns[2].Visible = false;
                    gvMasterParam.Columns[2].HeaderText = "";
                    gvMasterParam.Columns[3].HeaderText = "";
                    gvMasterParam.Columns[3].Visible = false;
                    gvMasterParam.Columns[4].HeaderText = "";
                    gvMasterParam.Columns[4].Visible = false;
                    gvMasterParam.Columns[5].HeaderText = String.Empty;
                    gvMasterParam.Columns[5].Visible = false;
                }
                else if (_MasterTypeID == MasterType.paperSize) // PAPER SIZE
                {
                    lblFormHeader.Text = "PAPER SIZE";
                    lblMasterParam.Text = "SIZE";
                    lblParamValue1.Text = "LENGTH";
                    lblParamValue2.Text = "WIDTH";
                    ShowhideControl(true, false, true, true, false, false);

                    gvMasterParam.Columns[0].HeaderText = "PAPER SIZE";
                    gvMasterParam.Columns[1].Visible = true;
                    gvMasterParam.Columns[1].HeaderText = "LENGTH";
                    gvMasterParam.Columns[2].Visible = true;
                    gvMasterParam.Columns[2].HeaderText = "WIDTH";
                    gvMasterParam.Columns[3].HeaderText = "";
                    gvMasterParam.Columns[3].Visible = false;
                    gvMasterParam.Columns[4].HeaderText = "";
                    gvMasterParam.Columns[4].Visible = false;
                    gvMasterParam.Columns[5].HeaderText = String.Empty;
                    gvMasterParam.Columns[5].Visible = false;
                }
                else if (_MasterTypeID == MasterType.color) // COLOUR
                {
                    lblFormHeader.Text = "COLOUR";
                    lblMasterParam.Text = "COLOUR";
                    ShowhideControl(true, false, false, false, false, false);

                    gvMasterParam.Columns[0].HeaderText = "COLOUR";
                    gvMasterParam.Columns[1].Visible = false;
                    gvMasterParam.Columns[1].HeaderText = "";
                    gvMasterParam.Columns[2].Visible = false;
                    gvMasterParam.Columns[2].HeaderText = "";
                    gvMasterParam.Columns[3].HeaderText = "";
                    gvMasterParam.Columns[3].Visible = false;
                    gvMasterParam.Columns[4].HeaderText = "";
                    gvMasterParam.Columns[4].Visible = false;
                    gvMasterParam.Columns[5].HeaderText = String.Empty;
                    gvMasterParam.Columns[5].Visible = false;
                }
                else if (_MasterTypeID == MasterType.packingItem) // PACKING ITEM
                {
                    lblFormHeader.Text = "PACKING ITEM";
                    lblMasterParam.Text = "ITEM";
                    ShowhideControl(true, false, false, false, false, false);

                    gvMasterParam.Columns[0].HeaderText = "PACKING ITEM";
                    gvMasterParam.Columns[1].Visible = false;
                    gvMasterParam.Columns[1].HeaderText = "";
                    gvMasterParam.Columns[2].Visible = false;
                    gvMasterParam.Columns[2].HeaderText = "";
                    gvMasterParam.Columns[3].HeaderText = "";
                    gvMasterParam.Columns[3].Visible = false;
                    gvMasterParam.Columns[4].HeaderText = "";
                    gvMasterParam.Columns[4].Visible = false;
                    gvMasterParam.Columns[5].HeaderText = String.Empty;
                    gvMasterParam.Columns[5].Visible = false;
                }
                else if (_MasterTypeID == MasterType.transport) // TRANSPORT
                {
                    lblFormHeader.Text = "TRANSPORTER COST";
                    lblMasterParam.Text = "TRANSPORTER";
                    lblMRP.Text = "COST";
                    ShowhideControl(true, false, false, false, false, true);

                    gvMasterParam.Columns[0].HeaderText = "TRANSPORTER";
                    gvMasterParam.Columns[1].Visible = false;
                    gvMasterParam.Columns[1].HeaderText = "";
                    gvMasterParam.Columns[2].Visible = false;
                    gvMasterParam.Columns[2].HeaderText = "";
                    gvMasterParam.Columns[3].HeaderText = "";
                    gvMasterParam.Columns[3].Visible = false;
                    gvMasterParam.Columns[4].HeaderText = "COST";
                    gvMasterParam.Columns[4].Visible = true;
                    gvMasterParam.Columns[5].HeaderText = String.Empty;
                    gvMasterParam.Columns[5].Visible = false;
                }
                else if (_MasterTypeID == MasterType.gsm) // GSM
                {
                    lblFormHeader.Text = "GSM MEASURMENT";
                    lblMasterParam.Text = "GSM";
                    lblParamValue1.Text = "VALUE";
                    ShowhideControl(true, false, true, false, false, false);

                    gvMasterParam.Columns[0].HeaderText = "GSM";
                    gvMasterParam.Columns[1].Visible = true;
                    gvMasterParam.Columns[1].HeaderText = "VALUE";
                    gvMasterParam.Columns[2].Visible = false;
                    gvMasterParam.Columns[2].HeaderText = "";
                    gvMasterParam.Columns[3].HeaderText = "";
                    gvMasterParam.Columns[3].Visible = false;
                    gvMasterParam.Columns[4].HeaderText = "";
                    gvMasterParam.Columns[4].Visible = false;
                    gvMasterParam.Columns[5].HeaderText = String.Empty;
                    gvMasterParam.Columns[5].Visible = false;
                }
                else if (_MasterTypeID == MasterType.cutting) // CUTTING
                {
                    lblFormHeader.Text = "CUTTING MEASURMENT";
                    lblMasterParam.Text = "CUTTING";
                    lblParamValue1.Text = "LENGTH";
                    lblParamValue2.Text = "WIDTH";
                    ShowhideControl(true, false, true, true, false, false);

                    gvMasterParam.Columns[0].HeaderText = "CUTTING";
                    gvMasterParam.Columns[1].Visible = true;
                    gvMasterParam.Columns[1].HeaderText = "LENGTH";
                    gvMasterParam.Columns[2].Visible = true;
                    gvMasterParam.Columns[2].HeaderText = "WIDTH";
                    gvMasterParam.Columns[3].HeaderText = "";
                    gvMasterParam.Columns[3].Visible = false;
                    gvMasterParam.Columns[4].HeaderText = "";
                    gvMasterParam.Columns[4].Visible = false;
                    gvMasterParam.Columns[5].HeaderText = String.Empty;
                    gvMasterParam.Columns[5].Visible = false;
                }
                else if (_MasterTypeID == MasterType.fabricationItem) // FABRICATION ITEM
                {
                    lblFormHeader.Text = "FABRICATION ITEM";
                    lblMasterParam.Text = "ITEM";
                    ShowhideControl(true, false, false, false, false, false);

                    gvMasterParam.Columns[0].HeaderText = "FABRICATION ITEM";
                    gvMasterParam.Columns[1].Visible = false;
                    gvMasterParam.Columns[1].HeaderText = "";
                    gvMasterParam.Columns[2].Visible = false;
                    gvMasterParam.Columns[2].HeaderText = "";
                    gvMasterParam.Columns[3].HeaderText = "";
                    gvMasterParam.Columns[3].Visible = false;
                    gvMasterParam.Columns[4].HeaderText = "";
                    gvMasterParam.Columns[4].Visible = false;
                    gvMasterParam.Columns[5].HeaderText = String.Empty;
                    gvMasterParam.Columns[5].Visible = false;
                }
                else if (_MasterTypeID == MasterType.unit) // UNIT
                {
                    lblFormHeader.Text = "UNIT";
                    lblMasterParam.Text = "UNIT";
                    ShowhideControl(true, false, false, false, false, false);

                    gvMasterParam.Columns[0].HeaderText = "UNIT";
                    gvMasterParam.Columns[1].Visible = false;
                    gvMasterParam.Columns[1].HeaderText = "";
                    gvMasterParam.Columns[2].Visible = false;
                    gvMasterParam.Columns[2].HeaderText = "";
                    gvMasterParam.Columns[3].HeaderText = "";
                    gvMasterParam.Columns[3].Visible = false;
                    gvMasterParam.Columns[4].HeaderText = "";
                    gvMasterParam.Columns[4].Visible = false;
                    gvMasterParam.Columns[5].HeaderText = String.Empty;
                    gvMasterParam.Columns[5].Visible = false;
                }
                else if (_MasterTypeID == MasterType.operatorprint) // OPERATOR
                {
                    lblFormHeader.Text = "OPERATOR";
                    lblMasterParam.Text = "NAME";
                    ShowhideControl(true, false, false, false, false, false);

                    gvMasterParam.Columns[0].HeaderText = "OPERATOR";
                    gvMasterParam.Columns[1].Visible = false;
                    gvMasterParam.Columns[1].HeaderText = "";
                    gvMasterParam.Columns[2].Visible = false;
                    gvMasterParam.Columns[2].HeaderText = "";
                    gvMasterParam.Columns[3].HeaderText = "";
                    gvMasterParam.Columns[3].Visible = false;
                    gvMasterParam.Columns[4].HeaderText = "";
                    gvMasterParam.Columns[4].Visible = false;
                    gvMasterParam.Columns[5].HeaderText = String.Empty;
                    gvMasterParam.Columns[5].Visible = false;
                }
                else if (_MasterTypeID == MasterType.tax) // TAX
                {
                    lblFormHeader.Text = "TAX PERCENTAGE";
                    lblMasterParam.Text = "TAX";
                    lblParamValue1.Text = "RATE";
                    ShowhideControl(true, false, true, false, false, false);

                    gvMasterParam.Columns[0].HeaderText = "TAX";
                    gvMasterParam.Columns[1].Visible = true;
                    gvMasterParam.Columns[1].HeaderText = "RATE";
                    gvMasterParam.Columns[2].Visible = false;
                    gvMasterParam.Columns[2].HeaderText = String.Empty;
                    gvMasterParam.Columns[3].HeaderText = String.Empty;
                    gvMasterParam.Columns[3].Visible = false;
                    gvMasterParam.Columns[4].HeaderText = String.Empty;
                    gvMasterParam.Columns[4].Visible = false;
                    gvMasterParam.Columns[5].HeaderText = String.Empty;
                    gvMasterParam.Columns[5].Visible = false;
                }
                else if (_MasterTypeID == MasterType.itemDescription) // itemDescription
                {
                    lblFormHeader.Text = "ITEM DESCTIPTION";
                    lblMasterParam.Text = "ITEM";
                    lblMasterParamDescription.Text = "DESCRIPTION";
                    ShowhideControl(true, true, false, false, false, false);

                    gvMasterParam.Columns[0].HeaderText = "ITEM";
                    gvMasterParam.Columns[1].Visible = false;
                    gvMasterParam.Columns[1].HeaderText = String.Empty;
                    gvMasterParam.Columns[2].Visible = false;
                    gvMasterParam.Columns[2].HeaderText = String.Empty;
                    gvMasterParam.Columns[3].HeaderText = String.Empty;
                    gvMasterParam.Columns[3].Visible = false;
                    gvMasterParam.Columns[4].HeaderText = String.Empty;
                    gvMasterParam.Columns[4].Visible = false;
                    gvMasterParam.Columns[5].HeaderText = "DESCRIPTION";
                    gvMasterParam.Columns[5].Visible = true;
                }
                gvMasterParam.DataBind();
            }
            catch (Exception ex)
            {
                gvMasterParam.DataSource = null;
                gvMasterParam.DataBind();
            }
        }

        private void ShowhideControl(Boolean PrintingParameterName, Boolean PrintingParameterDescription, Boolean ParameterValue1, Boolean ParameterValue2, Boolean ParameterValue3, Boolean SellPrice)
        {
            lblMasterParam.Visible = PrintingParameterName;
            txtMasterParam.Visible = PrintingParameterName;
            lblMasterParamDescription.Visible = PrintingParameterDescription;
            txtMasterParamDescription.Visible = PrintingParameterDescription;
            lblParamValue1.Visible = ParameterValue1;
            txtParamValue1.Visible = ParameterValue1;
            lblParamValue2.Visible = ParameterValue2;
            txtParamValue2.Visible = ParameterValue2;
            lblParamValue3.Visible = ParameterValue3;
            txtParamValue3.Visible = ParameterValue3;
            lblMRP.Visible = SellPrice;
            txtMRP.Visible = SellPrice;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            String message = String.Empty;
            int _MasterTypeID = 0;
            try
            {
                if (Convert.ToInt16(hidPrintingParameterID.Value) == 0)
                {
                    _MasterTypeID = Convert.ToInt32(Request.QueryString["ID"].ToString());
                    var data = _dbContext.PrintingParameters.Where(p => p.PrintingParameterName == txtMasterParam.Text && p.PrintingParameterID == _MasterTypeID).ToList();
                    if (data.Any())
                    {
                        message = "alert('This Master Data already exists')";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    }
                    else
                    {
                        PrintingParameter para = new PrintingParameter();
                        para.PrintingParameterTypeID = Convert.ToInt32(Request.QueryString["ID"].ToString());
                        para.PrintingParameterName = txtMasterParam.Text.Trim().ToUpper();
                        para.ParameterValue1 = Convert.ToDecimal(txtParamValue1.Text.Trim() != "" ? txtParamValue1.Text.Trim() : "0");
                        para.ParameterValue2 = Convert.ToDecimal(txtParamValue2.Text.Trim() != "" ? txtParamValue2.Text.Trim() : "0");
                        para.ParameterValue3 = Convert.ToDecimal(txtParamValue3.Text.Trim() != "" ? txtParamValue3.Text.Trim() : "0");
                        para.PrintingParameterDescription = txtMasterParamDescription.Text.Trim();
                        para.SellPrice = Convert.ToDecimal(txtMRP.Text.Trim() != "" ? txtMRP.Text.Trim() : "0");
                        para.SellPrice = 0;
                        para.RecMode = "A";
                        para.CreateBy = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
                        para.CreatedDate = System.DateTime.Now;
                        _dbContext.PrintingParameters.Add(para);
                        _dbContext.SaveChanges();
                        message = "alert('This Master Data saved sucessfully')";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                        ResetControl();
                    }
                }
                else if (Convert.ToInt16(hidPrintingParameterID.Value) > 0)
                {
                    int MasterId = Convert.ToInt32(hidPrintingParameterID.Value.Trim() != "" ? hidPrintingParameterID.Value.Trim() : "0");
                    var q = _dbContext.PrintingParameters.Where(p => p.PrintingParameterName == txtMasterParam.Text && p.PrintingParameterID != MasterId).ToList();
                    if (q.Any())
                    {
                        message = "alert('This Master Data already exists')";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    }
                    else
                    {
                        var data = _dbContext.PrintingParameters.Where(p => p.PrintingParameterID == MasterId).SingleOrDefault();
                        data.PrintingParameterName = txtMasterParam.Text.Trim().ToUpper();
                        data.ParameterValue1 = Convert.ToDecimal(txtParamValue1.Text.Trim() != "" ? txtParamValue1.Text.Trim() : "0");
                        data.ParameterValue2 = Convert.ToDecimal(txtParamValue2.Text.Trim() != "" ? txtParamValue2.Text.Trim() : "0");
                        data.ParameterValue3 = Convert.ToDecimal(txtParamValue3.Text.Trim() != "" ? txtParamValue3.Text.Trim() : "0");
                        data.PrintingParameterDescription = txtMasterParamDescription.Text.Trim();
                        data.SellPrice = Convert.ToDecimal(txtMRP.Text.Trim() != "" ? txtMRP.Text.Trim() : "0");
                        data.RecMode = "M";
                        data.LMB = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
                        data.LMD = System.DateTime.Now;
                        _dbContext.SaveChanges();
                        message = "alert('This Master Data updated sucessfully')";
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
                    var data = _dbContext.PrintingParameters.Where(p => p.PrintingParameterID == MasterId).SingleOrDefault();
                    data.RecMode = "D";
                    data.DeleteBy = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]);
                    data.DeleteDate = System.DateTime.Now;
                    _dbContext.SaveChanges();
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
                var GetData = _dbContext.PrintingParameters.Where(p => p.PrintingParameterID == MasterId).Single();
                if (GetData != null)
                {
                    MasterTypeId = Convert.ToInt32(GetData.PrintingParameterTypeID);
                }

                if (MasterTypeId == MasterType.paperType)
                {
                    if (_dbContext.QuotationJobSheetDetails.Where(p => p.PaperTypeID == MasterId).Any())
                    {
                        InUse = true;
                    }
                }
                else if (MasterTypeId == MasterType.paperCompany)
                {
                    if (_dbContext.QuotationJobSheetDetails.Where(p => p.PaperManufaturerID == MasterId).Any())
                    {
                        InUse = true;
                    }
                }
                else if (MasterTypeId == MasterType.machine)
                {
                    if (_dbContext.QuotationJobSheetDetails.Where(p => p.MachineID == MasterId).Any())
                    {
                        InUse = true;
                    }
                }
                else if (MasterTypeId == MasterType.plateType)
                {
                    if (_dbContext.QuotationJobSheetDetails.Where(p => p.PlateTypeID == MasterId).Any())
                    {
                        InUse = true;
                    }
                    else if (_dbContext.PrintRateCalculations.Where(p => p.PlateID == MasterId).Any())
                    {
                        InUse = true;
                    }
                }
                else if (MasterTypeId == MasterType.paperSide)
                {
                    if (_dbContext.QuotationJobSheetDetails.Where(p => p.SideID == MasterId).Any())
                    {
                        InUse = true;
                    }
                }
                else if (MasterTypeId == MasterType.paperSize)
                {
                    if (_dbContext.QuotationJobSheetDetails.Where(p => p.PaperSizeID == MasterId).Any())
                    {
                        InUse = true;
                    }
                }
                else if (MasterTypeId == MasterType.color)
                {
                    if (_dbContext.QuotationJobSheetDetails.Where(p => p.ColourID == MasterId).Any())
                    {
                        InUse = true;
                    }
                    else if (_dbContext.PrintRateCalculations.Where(p => p.ColorID == MasterId).Any())
                    {
                        InUse = true;
                    }
                }
                else if (MasterTypeId == MasterType.packingItem)
                {
                    if (_dbContext.QuotationJobSheetDetails.Where(p => p.PackagingItemID == MasterId).Any())
                    {
                        InUse = true;
                    }
                }
                else if (MasterTypeId == MasterType.gsm)
                {
                    if (_dbContext.QuotationJobSheetDetails.Where(p => p.GSMID == MasterId).Any())
                    {
                        InUse = true;
                    }
                }
                else if (MasterTypeId == MasterType.cutting)
                {
                    if (_dbContext.QuotationJobSheetDetails.Where(p => p.CuttingID == MasterId).Any())
                    {
                        InUse = true;
                    }
                    else if (_dbContext.PrintRateCalculations.Where(p => p.CuttingSizeID == MasterId).Any())
                    {
                        InUse = true;
                    }
                }
                else if (MasterTypeId == MasterType.fabricationItem)
                {
                    if (_dbContext.QuotationJobSheetDetails.Where(p => p.FabricationItemID == MasterId).Any())
                    {
                        InUse = true;
                    }
                }
                else if (MasterTypeId == MasterType.unit)
                {
                    if (_dbContext.QuotationJobSheetDetails.Where(p => p.TransportUnitID == MasterId).Any())
                    {
                        InUse = true;
                    }
                }
                else if (MasterTypeId == MasterType.operatorprint)
                {
                    if (_dbContext.QuotationJobSheetDetails.Where(p => p.OperatorID == MasterId).Any())
                    {
                        InUse = true;
                    }
                }
                else if (MasterTypeId == MasterType.tax)
                {
                    if (_dbContext.QuotationJobSheets.Where(p => p.TaxID == MasterId).Any())
                    {
                        InUse = true;
                    }
                }
                else if (MasterTypeId == MasterType.itemDescription)
                {
                    if (_dbContext.QuotationJobSheets.Where(p => p.ItemID == MasterId).Any())
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

        protected void gvMasterParam_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMasterParam.PageIndex = e.NewPageIndex;
            FillGridView();
        }

        protected void gvMasterParam_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                int MasterId = Convert.ToInt32(e.CommandArgument.ToString());
                var GetData = _dbContext.PrintingParameters.Where(p => p.PrintingParameterID == MasterId).Single();
                hidPrintingParameterID.Value = MasterId.ToString();
                txtMasterParam.Text = GetData.PrintingParameterName;
                txtParamValue1.Text = GetData.ParameterValue1.ToString();
                txtParamValue2.Text = GetData.ParameterValue2.ToString();
                txtParamValue3.Text = GetData.ParameterValue3.ToString();
                txtMasterParamDescription.Text = GetData.PrintingParameterDescription.ToString();
                txtMRP.Text = GetData.SellPrice.ToString();
                btnSave.Text = "Update";
            }
        }

        protected void gvMasterParam_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
    }
}