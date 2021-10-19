using System;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;

namespace OFOS
{
    public partial class Daily_Sales_Report : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["admin"] == null)
            {
                Response.Redirect("Admin_Login.aspx?msg=You need to login first");

            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }

        protected void btns_Click(object sender, EventArgs e)
        {
            details.Visible = true;

            string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Shihan\source\repos\OFOS\OFOS\App_Data\ofos.mdf;Integrated Security=True";
            SqlConnection con = new SqlConnection(constr);
            try
            {
                this.BindGrid();
                GridView7.DataBind();

            }
            catch (Exception err)
            {
                Sts.Text = err.Message;
            }
            finally
            {
                con.Close();
            }
        }
        

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Session["admin"] = null;
            Response.Redirect("Admin_Login.aspx");
        }
        protected void BindGrid()
        {

            string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Shihan\source\repos\OFOS\OFOS\App_Data\ofos.mdf;Integrated Security=True";
            SqlConnection con = new SqlConnection(constr);
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Orders.Order_Id, Orders.Cust_Id, Orders.Date, Customers.Name, Order_Details.Quantity, Order_Details.Amount FROM ((Orders INNER JOIN Customers ON Orders.Cust_Id = Customers.Cust_Id)INNER JOIN Order_Details ON Orders.Order_Id = Order_Details.Order_Id)"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            GridView7.DataSource = dt;
                            GridView7.DataBind();
                        }
                    }
                }
            }
        }

        protected void Btn_Export_Click(object sender, EventArgs e)
        {
            Response.ContentType = "Export/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Order Status.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView7.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();
            GridView7.AllowPaging = true;
            GridView7.DataBind();
        }

        protected void GridView7_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }  
}