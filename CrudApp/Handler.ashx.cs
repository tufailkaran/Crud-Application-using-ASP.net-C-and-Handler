using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrudApp
{
    /// <summary>
    /// Summary description for Handler
    /// </summary>
    public class Handler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            
            context.Response.ContentType = "text/plain";
            string className = context.Request["class"].ToString();
            switch (className)
            {
                //Add Data
                case "AddCrudForm":
                     Form.index index = new Form.index();   
                     index.ProcessRequest(context);
                    break;
                //Edit through Id
                case "EditCrudForm":
                    Form.index edit = new Form.index();
                    edit.ProcessRequest(context);
                    break;
                //Update Data
                case "updateData":
                    Form.index update = new Form.index();
                    update.ProcessRequest(context);
                    break;
                //Delete Record
                case "DeleteRecord":
                    Form.index delete = new Form.index();
                    delete.ProcessRequest(context);
                    break;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}