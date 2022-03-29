using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Web;
using MySql.Data.MySqlClient;
using System.Data;
using System.Web.Script.Serialization;

namespace CrudApp.Form
{
    public class index
    {
        
        //Set and Get all variables
        public int id { get; set; }
        public string stdName { get; set; }
        public string stdUsername { get; set; }
        public string stdEmail { get; set; }
        //Add Data Into DB or Insertion
        public string Save(HttpContext context)
        {
            string stdName = context.Request["stdName"].ToString();
            string stdUsername = context.Request["stdUsername"].ToString();
            string stdEmail = context.Request["stdEmail"].ToString();

            string conn = ConfigurationManager.ConnectionStrings["crudAppConnect"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(conn)) 
            {
                using (MySqlCommand cmd = new MySqlCommand("Insert into student (stdName,stdUsername, stdEmail) values(@stdName, @stdUsername, @stdEmail)"))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Parameters.AddWithValue("@stdName", stdName);
                        cmd.Parameters.AddWithValue("@stdUsername", stdUsername);
                        cmd.Parameters.AddWithValue("@stdEmail", stdEmail);
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }}
                }
            return "Data Submitted";
        }
        //Data Get from DB
        public string Read(HttpContext context)
        {
            List<index> listStudent = new List<index>();
   

            string conn = ConfigurationManager.ConnectionStrings["crudAppConnect"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(conn))
            {
                using (MySqlCommand cmd = new MySqlCommand("Select * from student"))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                      

                        con.Open();
                        using (MySqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                listStudent.Add(new index
                                {
                                    id = Convert.ToInt32(sdr["id"]),
                                    stdName = Convert.ToString(sdr["stdName"]),
                                    stdEmail = Convert.ToString(sdr["stdEmail"]),
                                    stdUsername= Convert.ToString(sdr["stdUsername"])
                                });
                            }
                        }
                        con.Close();
                    }

                }
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            //return (js.Serialize(listStudent));
            var response = new
            {
                data = listStudent,
            };
            return (js.Serialize(listStudent));
        }

        // Data Edit Call through Id
        public string EditFormCall(HttpContext context)
        {

            string id = context.Request["id"].ToString();
               
            index data = new index();
            
            string conn = ConfigurationManager.ConnectionStrings["crudAppConnect"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(conn))
            {
                using (MySqlCommand cmd = new MySqlCommand("Select * from student where student.id ="+id))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;


                        con.Open();
                        using (MySqlDataReader sdr = cmd.ExecuteReader())
                        {
                            sdr.Read();
                            data.id = Convert.ToInt32(sdr["id"]);
                            data.stdName = Convert.ToString(sdr["stdName"]);
                            data.stdEmail = Convert.ToString(sdr["stdEmail"]);
                            data.stdUsername = Convert.ToString(sdr["stdUsername"]);
                        }
                        con.Close();
                    }
                }
                JavaScriptSerializer js = new JavaScriptSerializer();
                //return (js.Serialize(listStudent));
                var response = new
                {
                    data = data,
                };
                return (js.Serialize(data));
            }
        }

        //Data Updating
        public string Update(HttpContext context)
        {
            string id = context.Request["id"].ToString();
            string stdName = context.Request["stdName"].ToString();
            string stdUsername = context.Request["stdUsername"].ToString();
            string stdEmail = context.Request["stdEmail"].ToString();

            string conn = ConfigurationManager.ConnectionStrings["crudAppConnect"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(conn))
            {
                using (MySqlCommand cmd = new MySqlCommand("Update student set stdName = @stdName, stdUsername = @stdUsername, stdEmail = @stdEmail where student.id =" + id))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@stdName", stdName);
                        cmd.Parameters.AddWithValue("@stdUsername", stdUsername);
                        cmd.Parameters.AddWithValue("@stdEmail", stdEmail);
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }
                }
            }
            return "Data Updated Submitted";
        }

        //Data Deletion
        public string Delete(HttpContext context)
        {
            string id = context.Request["id"].ToString();
            

            string conn = ConfigurationManager.ConnectionStrings["crudAppConnect"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(conn))
            {
                using (MySqlCommand cmd = new MySqlCommand("Delete from student where student.id=" + id))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }
                }
            }
            return "Data Deleted Succesfully";
        }


        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string classAction = context.Request["action"].ToString();
            switch (classAction)
            {
                //Add Function
                case "add":
                    context.Response.Write(Save(context));
                    break;
                //Read Function
                case "read":
                    context.Response.Write(Read(context));
                    break;
                //Update Function
                case "update":
                    context.Response.Write(Update(context));
                    break;
                //Edit Call Function
                case "EditForm":
                    context.Response.Write(EditFormCall(context));
                    break;
                
                //Delete Function
                case "Delete":
                    context.Response.Write(Delete(context));
                    break;

            }
        }
    }
}