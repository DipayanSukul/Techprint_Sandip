using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TechPrint.Account
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TechPrintEntities dbContext = new TechPrintEntities();
            if (!IsPostBack)
            {
                var data = (from J in dbContext.QuotationJobSheets
                            select (J)).ToList().Where(JL => JL.QuotationDate.Value == Convert.ToDateTime(DateTime.Now.Date)).ToList();
                lblTotalBooking.Text += " : " + data.Count.ToString();
           
            }
        }
    }
}