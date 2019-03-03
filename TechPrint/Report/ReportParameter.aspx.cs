using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TechPrint.Report
{
    public partial class ReportParameter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            string reportID = Request.QueryString["ReportID"].ToString();
            Response.Redirect("ViewReport.aspx?ReportID=" + reportID);
        }
    }
}