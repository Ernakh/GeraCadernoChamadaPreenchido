using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using System.IO;

namespace CadernoChamadaCompleto
{
    public partial class Caderno : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["id_turma"] = 101176;
            //Session["semestre"] = 2;
            //Session["periodo"] = 457;

            //PegaIP pip = new PegaIP();

            GeraCaderno gc = new GeraCaderno();

            MemoryStream documento = gc.ExecuteResult("1", "G201601");

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=Documento.pdf");
            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.OutputStream.Write(documento.GetBuffer(), 0, documento.GetBuffer().Length);
            HttpContext.Current.Response.OutputStream.Flush();
            HttpContext.Current.Response.End();
        }
    }
}