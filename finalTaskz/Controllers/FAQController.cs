using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using finalTaskz.Models.FAQs;
namespace finalTaskz.Controllers
{
    public class FAQController : Controller
    {
       
        [HttpGet]
        public List<FAQ> GetFAQs()
        {
            using(SqlConnection conn=new SqlConnection(Connection.ConnStr())){
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand =new SqlCommand("Use RentCars;SELECT * FROM dbo.FAQ",conn);
              
                DataTable tb = new DataTable();
                adapter.Fill(tb);
                List<FAQ> FAQs = new List<FAQ>();
                foreach (DataRow dataRow in tb.Rows)
                {
                    FAQ question = new FAQ();
                    question.IdQuestion = dataRow["IdQuestion"].ToString();
                    question.Title = dataRow["Title"].ToString();
                    question.Content = dataRow["Content"].ToString();
                    FAQs.Add(question);
                }
                conn.Close();
                return FAQs;
            }
            
        }

        public JsonResult AllFaqs()
        {
            var Faqs = GetFAQs().AsQueryable().Select(FaqViewModel.FromFAQs);
            return this.Json(Faqs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ContentFaqs()
        {
            var FaqsContent = GetFAQs().AsQueryable().Select(FAQContentViewModel.FromFAQsContent);
            return this.Json(FaqsContent, JsonRequestBehavior.AllowGet);
        }
    }
}
