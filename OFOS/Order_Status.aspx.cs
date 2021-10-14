using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

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
    }
}