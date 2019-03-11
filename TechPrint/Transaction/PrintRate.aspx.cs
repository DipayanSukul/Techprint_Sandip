using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace TechPrint.Transaction
{
    public partial class PrintRate : System.Web.UI.Page
    {
        TechPrintEntities _dbContext = new TechPrintEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ApplicationUtility.FillMasterDataDropDownlist(ddlCutting, MasterType.cutting);
                ResetControl();
            }
        }

        private void ResetControl()
        {
            ddlCutting.SelectedIndex = -1;
            ddlCutting_SelectedIndexChanged(ddlCutting, EventArgs.Empty);
            gvParam.DataSource = null;
            gvParam.DataBind();
        }

        protected void ddlCutting_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCutting.SelectedIndex > 0)
            {
                SqlParameter[] p = new SqlParameter[2];
                p[0] = new SqlParameter("@CuttingSizeID", Convert.ToInt32(ddlCutting.SelectedValue != "" ? ddlCutting.SelectedValue : "0"));
                p[1] = new SqlParameter("@OpMode", 4);
                DataTable dt = ApplicationUtility.ExecuteDataTable("USP_MANAGE_PRINT_RATE", p);

                gvParam.DataSource = dt;
                gvParam.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            String message;
            DataTable dt = new DataTable();
            DataRow dr = null;
            if (gvParam.Rows.Count > 0)
            {
                dt.Columns.Add(new DataColumn("PlateID", typeof(Int32)));
                dt.Columns.Add(new DataColumn("ColorID", typeof(Int32)));
                dt.Columns.Add(new DataColumn("PlateCost1K", typeof(Decimal)));
                dt.Columns.Add(new DataColumn("ColorCost1K", typeof(Decimal)));
                dt.Columns.Add(new DataColumn("PlateCost10K", typeof(Decimal)));
                dt.Columns.Add(new DataColumn("ColorCost10K", typeof(Decimal)));

                for (int rowID = 0; rowID < gvParam.Rows.Count; rowID++)
                {
                    string plateID = gvParam.DataKeys[rowID].Values["PlateID"].ToString();
                    string colorID = gvParam.DataKeys[rowID].Values["ColorID"].ToString();

                    TextBox txtPlateCost1K = gvParam.Rows[rowID].FindControl("txtPlateCost1K") as TextBox;
                    TextBox txtColorCost1K = gvParam.Rows[rowID].FindControl("txtColorCost1K") as TextBox;
                    TextBox txtPlateCost10K = gvParam.Rows[rowID].FindControl("txtPlateCost10K") as TextBox;
                    TextBox txtColorCost10K = gvParam.Rows[rowID].FindControl("txtColorCost10K") as TextBox;

                    dr = dt.NewRow();
                    dr["PlateID"] = Convert.ToInt32(plateID != "" ? plateID : "0");
                    dr["ColorID"] = Convert.ToInt32(colorID != "" ? colorID : "0");
                    dr["PlateCost1K"] = Convert.ToDecimal(txtPlateCost1K.Text.Trim() != "" ? txtPlateCost1K.Text.Trim() : "0");
                    dr["ColorCost1K"] = Convert.ToDecimal(txtColorCost1K.Text.Trim() != "" ? txtColorCost1K.Text.Trim() : "0");
                    dr["PlateCost10K"] = Convert.ToDecimal(txtPlateCost10K.Text.Trim() != "" ? txtPlateCost10K.Text.Trim() : "0");
                    dr["ColorCost10K"] = Convert.ToDecimal(txtColorCost10K.Text.Trim() != "" ? txtColorCost10K.Text.Trim() : "0");
                    dt.Rows.Add(dr);
                }
            }

            SqlParameter[] p = new SqlParameter[4];
            p[0] = new SqlParameter("@PrintRateList", dt);
            p[1] = new SqlParameter("@CuttingSizeID", Convert.ToInt32(ddlCutting.SelectedValue != "" ? ddlCutting.SelectedValue : "0"));
            p[2] = new SqlParameter("@RequestBy", Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString()));
            p[3] = new SqlParameter("@OpMode", 1);
            int i = ApplicationUtility.ExecuteNonQuery("USP_MANAGE_PRINT_RATE", p);
            if (i > 0)
            {
                message = "alert('Print Rate upload successfully')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                ResetControl();
            }
            else
            {
                message = "alert('Error Occur - Unable to upload Print Rate')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
            }
        }
    }
}