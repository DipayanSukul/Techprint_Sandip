using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechPrint.Account
{
    public partial class TechPrint : System.Web.UI.MasterPage
    {
        TechPrintEntities _dbContext = new TechPrintEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["UserID"] == null)
            {
                HttpContext.Current.Response.Redirect("~/login.aspx");
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session["Name"] = null;
                HttpContext.Current.Session["UserID"] = null;
            }
            else
            {
                lblLoginTime.Text = "Login Time : " + System.DateTime.UtcNow.ToString("dddd, MMMM d");
                lblCompany.Text = "JYOTI LASER POINT";
                lblCompanyAddress.Text = "63/2D Surya Sen Street, Kol - 700 009" + Environment.NewLine + "Phone - (033) 2241-9473 Office - (033) 2241-9473";
                lbl_UserName.Text = System.Web.HttpContext.Current.Session["Name"].ToString();

                //var obj = _dbContext.TBL_LOGIN.Select(a => a.USERID.ToString() == HttpContext.Current.Session["UserID"].ToString()).ToList();
            }
        }

        protected void lnklogout_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session["Name"] = null;
            HttpContext.Current.Session["UserID"] = null;
            HttpContext.Current.Response.Redirect("../login.aspx");
        }
    }
}