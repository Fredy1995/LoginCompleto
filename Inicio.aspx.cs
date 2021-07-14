using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppWeb
{
    public partial class Inicio : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                //**Intruciones para cargar por primera vez

            }
           
                if(Session["Usuario"] != null)
                {
                   
                    Lbluser.Text = " " + Session["Usuario"].ToString(); ;
                }
                else
                {
                    Response.Redirect("Default.aspx?id=1");
                }
               
        }

        protected void BtnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Remove("Usuario");
            Response.Redirect("Default.aspx");
        }
    }
}