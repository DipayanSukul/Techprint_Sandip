using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TechPrint
{
    public partial class login : System.Web.UI.Page
    {
        TechPrintEntities _dbContext = new TechPrintEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["UserName"] != null && Request.Cookies["PassWord"] != null)
                {
                    txtuid.Text = Request.Cookies["UserName"].Value;
                    txtpwd.Attributes["value"] = Request.Cookies["PassWord"].Value;
                    chkRememberMe.Checked = true;
                }
            }
        }

        protected void ImgBtnLogin_Click(object sender, ImageClickEventArgs e)
        {
            var obj = _dbContext.Users.FirstOrDefault(a => a.LoginID == txtuid.Text.Trim() && a.Password == txtpwd.Text.Trim());
            if (obj != null)
            {
                if (chkRememberMe.Checked)
                {
                    Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(30);
                    Response.Cookies["PassWord"].Expires = DateTime.Now.AddDays(30);
                }
                else
                {
                    Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies["PassWord"].Expires = DateTime.Now.AddDays(-1);
                }
                Response.Cookies["UserName"].Value = txtuid.Text.Trim();
                Response.Cookies["PassWord"].Value = txtpwd.Text.Trim();

                HttpContext.Current.Session["UserID"] = obj.UserID;
                HttpContext.Current.Session["Name"] = obj.Nane;
                GlobalVariable.Get();
                Response.Redirect("~/Account/Home.aspx", false);
            }
            else
            {
                lblmsg.Text = "Invalid Login ID / PassWord !";
            }
        }
    }
}