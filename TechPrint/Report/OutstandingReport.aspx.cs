using System;
using System.Linq;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;

namespace TechPrint.Report
{
    public partial class OutstandingReport : System.Web.UI.Page
    {
        TechPrintEntities _dbContext = new TechPrintEntities();
        CultureInfo provider = CultureInfo.InvariantCulture;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ResetControl();
                FillClientList();
            }
        }

        private void FillClientList()
        {
            var data = (from p in _dbContext.Customers
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
            ddlClient.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- Select -", "0"));
        }

        private void ResetControl()
        {
            ddlClient.SelectedIndex = -1;
            txtfromDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy", provider);
            txtToDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy", provider);
            gvReport.DataSource = null;
            gvReport.DataBind();
        }

        protected void btnGetReport_Click(object sender, EventArgs e)
        {
            var data = (from p in _dbContext.QuotationJobSheets
                        join q in _dbContext.Customers on p.CustomerID equals q.CustomerID
                        where p.IS_JOB == true
                        select new
                        {
                            p.QuotationID,
                            p.QuotationNO,
                            p.QuotationDate,
                            p.TotalBillingAmount,
                            p.AmountPayable,
                            p.AmountPaid,
                            p.OutstandingAmount,
                            q.CustomerName
                        }).ToList();
            if (data != null)
            {
                gvReport.DataSource = data;
                gvReport.DataBind();
                HttpContext.Current.Session["OutStandingReport"] = ApplicationUtility.ListToDataTable(data);
            }
            else
            {
                gvReport.DataSource = null;
                gvReport.DataBind();
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ResetControl();
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dataTable = new DataTable();
                if (HttpContext.Current.Session["OutStandingReport"] == null) return;
                string Name = "DATE WISE OUTSTANDING REPORT";
                //dataTable = (DataTable)HttpContext.Current.Session["OutStandingReport"];
                //string[] columnNames = (from dc in dataTable.Columns.Cast<DataColumn>()
                //                        select dc.ColumnName).ToArray();
                //int Cell = 0;
                //int count = columnNames.Length;
                //object[] array = new object[count];

                //dataTable.Rows.Add(array);

                //Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
                //System.IO.MemoryStream mStream = new System.IO.MemoryStream();
                //PdfWriter writer = PdfWriter.GetInstance(pdfDoc, mStream);
                //int cols = dataTable.Columns.Count;
                //int rows = dataTable.Rows.Count;


                //HeaderFooter header = new HeaderFooter(new Phrase(Name), false);

                //// Remove the border that is set by default  
                //header.Border = iTextSharp.text.Rectangle.TITLE;
                //// Align the text: 0 is left, 1 center and 2 right.  
                //header.Alignment = Element.ALIGN_CENTER;
                //pdfDoc.Header = header;
                //// Header.  
                //pdfDoc.Open();
                //iTextSharp.text.Table pdfTable = new iTextSharp.text.Table(cols, rows);
                //pdfTable.BorderWidth = 1; pdfTable.Width = 100;
                //pdfTable.Padding = 1; pdfTable.Spacing = 4;

                ////creating table headers  
                //for (int i = 0; i < cols; i++)
                //{
                //    Cell cellCols = new Cell();
                //    Chunk chunkCols = new Chunk();
                //    cellCols.BackgroundColor = new iTextSharp.text.Color(System.Drawing.ColorTranslator.FromHtml("#548B54"));
                //    iTextSharp.text.Font ColFont = FontFactory.GetFont(FontFactory.HELVETICA, 14, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.WHITE);

                //    chunkCols = new Chunk(dataTable.Columns[i].ColumnName, ColFont);

                //    cellCols.Add(chunkCols);
                //    pdfTable.AddCell(cellCols);
                //}
                ////creating table data (actual result)   

                //for (int k = 0; k < rows; k++)
                //{
                //    for (int j = 0; j < cols; j++)
                //    {
                //        Cell cellRows = new Cell();
                //        if (k % 2 == 0)
                //        {
                //            cellRows.BackgroundColor = new iTextSharp.text.Color(System.Drawing.ColorTranslator.FromHtml("#cccccc")); ;
                //        }
                //        else { cellRows.BackgroundColor = new iTextSharp.text.Color(System.Drawing.ColorTranslator.FromHtml("#ffffff")); }
                //        iTextSharp.text.Font RowFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                //        Chunk chunkRows = new Chunk(dataTable.Rows[k][j].ToString(), RowFont);
                //        cellRows.Add(chunkRows);

                //        pdfTable.AddCell(cellRows);
                //    }
                //}

                //pdfDoc.Add(pdfTable);
                //pdfDoc.Close();
                //Response.ContentType = "application/octet-stream";
                //Response.AddHeader("Content-Disposition", "attachment; filename=" + Name + "_" + DateTime.Now.ToString() + ".pdf");
                //Response.Clear();
                //Response.BinaryWrite(mStream.ToArray());
                //Response.End();

            }
            catch (Exception ex)
            {

            }
        }
    }
}