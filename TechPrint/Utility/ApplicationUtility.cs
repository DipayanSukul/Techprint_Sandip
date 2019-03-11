using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Configuration;

namespace TechPrint
{
    public static class MasterType
    {
        public static int paperType { get { return 1; } }
        public static int paperCompany { get { return 2; } }
        public static int machine { get { return 3; } }
        public static int plateType { get { return 4; } }
        public static int paperSide { get { return 5; } }
        public static int paperSize { get { return 6; } }
        public static int color { get { return 7; } }
        public static int packingItem { get { return 8; } }
        public static int transport { get { return 9; } }
        public static int gsm { get { return 1002; } }
        public static int cutting { get { return 1003; } }
        public static int fabricationItem { get { return 1004; } }
        public static int unit { get { return 1005; } }
        public static int operatorprint { get { return 1006; } }
        public static int tax { get { return 1007; } }
        public static int paperRate { get { return 2002; } }
        public static int itemDescription { get { return 2003; } }
    }

    public static class QuotationParameter
    {
        public static int paperDetail { get { return 1; } }
        public static int Printing { get { return 2; } }
        public static int FabricationItem { get { return 3; } }
        public static int Packaging { get { return 4; } }
        public static int Transport { get { return 5; } }
        public static int Design { get { return 6; } }
    }

    public static class GlobalVariable
    {
        public static String SQLConnectionString { get; set; }
        public static SqlConnectionStringBuilder SQLbuilder { get; set; }
        public static void Get()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            String strConn = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
            GlobalVariable.SQLConnectionString = strConn;
            builder = new SqlConnectionStringBuilder(strConn);
            GlobalVariable.SQLbuilder = builder;
        }
    }

    public static class ApplicationUtility
    {
        private static TechPrintEntities _dbContext = new TechPrintEntities();

        public static void FillMasterDataDropDownlist(DropDownList downList, int MasterType)
        {
            if (MasterType > 0)
            {
                var data = (from p in _dbContext.PrintingParameters
                            where p.PrintingParameterTypeID == MasterType
                            && p.RecMode!="D"
                            orderby p.PrintingParameterName ascending
                            select new { PrintingParameterName = p.PrintingParameterName.ToUpper(), PrintingParameterID = p.PrintingParameterID }).ToList();
                if (data.Count > 0)
                {
                    downList.DataSource = data;
                    downList.DataTextField = "PrintingParameterName";
                    downList.DataValueField = "PrintingParameterID";
                    downList.DataBind();
                }
                else
                {
                    downList.DataSource = null;
                    downList.DataBind();
                }
            }
            else
            {
                downList.DataSource = null;
                downList.DataBind();
            }
            downList.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- Select -", "0"));
        }

        public static DataTable ListToDataTable<T>(IEnumerable<T> Linqlist)
        {
            DataTable dt = new DataTable();
            PropertyInfo[] columns = null;
            if (Linqlist == null) return dt;
            foreach (T Record in Linqlist)
            {
                if (columns == null)
                {
                    columns = ((Type)Record.GetType()).GetProperties();
                    foreach (PropertyInfo GetProperty in columns)
                    {
                        Type colType = GetProperty.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dt.Columns.Add(new DataColumn(GetProperty.Name, colType));
                    }
                }
                DataRow dr = dt.NewRow();
                foreach (PropertyInfo pinfo in columns)
                {
                    dr[pinfo.Name] = pinfo.GetValue(Record, null) == null ? DBNull.Value : pinfo.GetValue
                    (Record, null);
                }

                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static ReportViewer FillReport(String ReportPath, Int32 ReportType, Int32 ReportParameter)
        {
            ReportViewer Viewer = new ReportViewer();
            Viewer.LocalReport.ReportPath = System.Web.HttpContext.Current.Server.MapPath(ReportPath);
            ReportDataSource ReportDs = new ReportDataSource();
            DataTable dt = new DataTable();

            if (ReportType == CommonConstants.REPORT_PRINT_BILL)
            {
                SqlParameter[] p = new SqlParameter[3];
                p[0] = new SqlParameter("@QuotationID", ReportParameter);
                p[1] = new SqlParameter("@QuotationDate", System.DateTime.Now); // Useless but don't remove
                p[2] = new SqlParameter("@OpMode", 5);
                dt = ApplicationUtility.ExecuteDataTable("USP_MANAGE_JOB_SHEET", p);
                ReportDs = new ReportDataSource("BillPrint", dt);
            }

            Viewer.LocalReport.DataSources.Clear();
            Viewer.LocalReport.DataSources.Add(ReportDs);
            Viewer.SizeToReportContent = true;

            return Viewer;
        }

        public static String CurrentSession()
        {
            int TempYear = 0;
            int FinanceYear = 0;
            string CurrentSession = "";
            string _Date = System.DateTime.Now.ToString("MM/dd/yyyy");
            string _Month = _Date.Substring(0, 2);
            string _Year = _Date.Substring(8, 2);

            if (Convert.ToInt32(_Month) > 3)
            {
                TempYear = Convert.ToInt32(_Year);
                FinanceYear = TempYear + 1;
                CurrentSession = _Year.ToString() + "-" + FinanceYear.ToString();
            }
            else
            {
                TempYear = Convert.ToInt32(_Year);
                FinanceYear = TempYear - 1;
                CurrentSession = FinanceYear.ToString() + "-" + _Year.ToString();
            }
            return CurrentSession;
        }

        public static int ExecuteNonQuery(string sql)
        {
            return ExecuteNonQuery(sql, null);
        }

        public static int ExecuteNonQuery(string sql, SqlParameter[] p)
        {
            int retval = 0;
            SqlConnection cnn = new SqlConnection(GlobalVariable.SQLConnectionString);
            cnn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                if (p != null)
                {
                    for (int i = 0; i <= p.Length - 1; i++)
                    {
                        cmd.Parameters.Add(p[i]);
                    }
                }
                retval = cmd.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                cnn.Close();
            }
            return retval;
        }

        public static DataTable ExecuteDataTable(string sql)
        {
            return ExecuteDataTable(sql, null);
        }

        public static DataTable ExecuteDataTable(string sql, SqlParameter[] p)
        {
            SqlConnection cnn = new SqlConnection(GlobalVariable.SQLConnectionString);
            cnn.Open();
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                if (p != null)
                {
                    for (int i = 0; i <= p.Length - 1; i++)
                    {
                        cmd.Parameters.Add(p[i]);
                    }
                }
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(dt);
            }
            catch (Exception exception)
            {

            }
            finally
            {
                cnn.Close();
            }
            return dt;
        }

        public static DataSet ExecuteDataSet(string sql, SqlParameter[] p)
        {
            SqlConnection cnn = new SqlConnection(GlobalVariable.SQLConnectionString);
            cnn.Open();
            DataSet dt = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                if (p != null)
                {
                    for (int i = 0; i <= p.Length - 1; i++)
                    {
                        cmd.Parameters.Add(p[i]);
                    }
                }
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(dt);
            }
            catch (Exception exception)
            {

            }
            finally
            {
                cnn.Close();
            }
            return dt;
        }
    }

    public static class ApplicationService
    {
        private static TechPrintEntities _dbContext = new TechPrintEntities();

        public static Decimal PrintRateCalculation(Int32 CuttingSizeID, Int32 ColorID, Int32 PlateID, decimal NoofPlate, decimal Impression, Int32 PaperSideID)
        {
            try
            {
                Decimal PrintRate = 0;

                if (CuttingSizeID > 0 && ColorID > 0 && PlateID > 0 && Impression > 0)
                {
                    Decimal PlateCost1K = 0;
                    Decimal ColorCost1K = 0;
                    Decimal PlateCost10K = 0;
                    Decimal ColorCost10K = 0;

                    Int32 ImpressionReminder = 0;
                    Int32 ImpressionDivisor = 0;
                    Int32 TempImpression = 0;

                    var GetData = _dbContext.PrintRateCalculations.Where(p => p.CuttingSizeID == CuttingSizeID && p.ColorID == ColorID && p.PlateID == PlateID).Single();
                    PlateCost1K = Convert.ToDecimal(GetData.PlateCost1K);
                    ColorCost1K = Convert.ToDecimal(GetData.ColorCost1K);
                    PlateCost10K = Convert.ToDecimal(GetData.PlateCost10K);
                    ColorCost10K = Convert.ToDecimal(GetData.ColorCost10K);

                    if (ColorID == 112 || ColorID == 101 || ColorID == 1006 || ColorID == 1007 || ColorID == 106 || ColorID == 106 ||
                        ColorID == 1005 || ColorID == 107 || ColorID == 108 || ColorID == 109 || ColorID == 110 || ColorID == 111 ||
                        ColorID == 113 || ColorID == 114 || ColorID == 115 || ColorID == 116)
                    // 112 = 1 Colour & 101 = 2 Colour & 1006 = 1 Colour Solid & 1007 = 2 Colour Solid & 106=Black & 106	= Black & 1005 = Black Solid & 107 =Chocolate & 108	= Cyne & 109 = Green & 110 = Grey & 111 = Magenta & 113	= Red & 114	= Royal Blue & 115 = Special Colour & 116 =	Yellow
                    {
                        if (PaperSideID == 82) // Front & Back
                        {
                            TempImpression = Convert.ToInt32(Impression / 2);
                            TempImpression -= 1000;
                        }
                        else
                        {
                            TempImpression = Convert.ToInt32(Impression - 1000);
                        }

                        ImpressionDivisor = Convert.ToInt32(TempImpression / 1000);
                        ImpressionReminder = Convert.ToInt32(TempImpression % 1000);

                        if (ImpressionReminder >= 500)
                        {
                            ImpressionDivisor += 1;
                        }

                        PrintRate = (ColorCost10K * ImpressionDivisor) + ColorCost1K;

                        if (NoofPlate > 1)
                            PrintRate = PrintRate * NoofPlate;

                        if (PaperSideID == 82) // Front & Back
                        {
                            PrintRate = PrintRate * 2;
                        }
                    }
                    else
                    //else if (ColorID == 103 || ColorID == 104 || ColorID == 105 || ColorID == 106) //103 = 3 Colour, 104 = 4 Colour, 105 = 5 Colour, 106 = 6 Colour
                    {
                        if (PaperSideID == 82) // Front & Back
                        {
                            TempImpression = Convert.ToInt32(Impression / 2);
                            if (Impression < 2500)
                            {
                                return (ColorCost1K * 2 * NoofPlate);
                            }
                        }
                        else
                        {
                            TempImpression = Convert.ToInt32(Impression);

                            if (Impression < 2500)
                            {
                                return (ColorCost1K * NoofPlate);
                            }
                        }

                        if (TempImpression < 10000 && TempImpression >= 2000)
                        {
                            TempImpression = TempImpression - 2000;
                        }

                        ImpressionDivisor = Convert.ToInt32(TempImpression / 1000);
                        ImpressionReminder = Convert.ToInt32(TempImpression % 1000);

                        if (ImpressionReminder >= 500)
                        {
                            ImpressionDivisor += 1;
                        }

                        if (TempImpression < 10000)
                        {
                            PrintRate = ((ColorCost10K * ImpressionDivisor) + ColorCost1K);
                            if (NoofPlate > 1)
                                PrintRate = PrintRate * NoofPlate;
                        }
                        else
                        {
                            PrintRate = ((ColorCost10K * ImpressionDivisor) + PlateCost10K) * NoofPlate;
                        }

                        if (PaperSideID == 82) // Front & Back
                        {
                            PrintRate = PrintRate * 2;
                        }
                    }
                }
                return PrintRate;
            }
            catch
            {
                return 0;
            }
        }
    }

    public static class CommonConstants
    {
        public const int REPORT_JOB_SHEET = 1;
        public const int REPORT_JOB_SHEET_LIST = 2;
        public const int REPORT_PRINT_BILL = 3;
    }
}