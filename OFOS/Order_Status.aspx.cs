using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.IO;
using System.Web;
using System.Web.UI;


namespace OFOS
{
    public partial class Order_Status : System.Web.UI.Page
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

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Session["admin"] = null;
            Response.Redirect("Admin_Login.aspx");
        }

        protected void btns_Click(object sender, EventArgs e)
        {
            details.Visible = true;

            string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Shihan\source\repos\OFOS\OFOS\App_Data\ofos.mdf;Integrated Security=True";
            SqlConnection con = new SqlConnection(constr);
            try
            {
                this.BindGrid();
                gridview3.DataBind();



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

        protected void gridview3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void BindGrid()
        {

            string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Shihan\source\repos\OFOS\OFOS\App_Data\ofos.mdf;Integrated Security=True";
            SqlConnection con = new SqlConnection(constr);
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Order_Id, COD_Pay_Status FROM Payment"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            gridview3.DataSource = dt;
                            gridview3.DataBind();
                        }
                    }
                }
            }
        }

        protected void Btn_Export_Click(object sender, EventArgs e)
        {
            Response.ContentType = "DailySales/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Order Status.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gridview3.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();
            gridview3.AllowPaging = true;
            gridview3.DataBind();
        }
    }
}