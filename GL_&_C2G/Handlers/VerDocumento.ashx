<%@ WebHandler Language="C#" Class="GL___C2G.Handlers.VerDocumento" %>
using System;
using System.Web;

namespace GL___C2G.Handlers
{
    public class VerDocumento : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.Clear();

            if (!int.TryParse(context.Request["id"], out int id))
            {
                context.Response.StatusCode = 400;
                context.Response.Write("id inválido");
                return;
            }

            var doc = BIZ.docs.obtener_doc(id);

            if (doc == null || doc.Value.bin == null || doc.Value.bin.Length == 0)
            {
                context.Response.StatusCode = 404;
                context.Response.Write("Documento no encontrado");
                return;
            }

            context.Response.ContentType = "application/pdf";
            context.Response.AddHeader("Content-Disposition",
                $"inline; filename=\"{doc.Value.nombre}\"");
            context.Response.BinaryWrite(doc.Value.bin);
            context.Response.End();
        }

        public bool IsReusable { get { return false; } }
    }
}