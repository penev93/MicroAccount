using finalTaskz.Models.AccountDetails;
using finalTaskz.Models.BookDetails;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace finalTaskz.Controllers
{
    public class HistoryController : Controller
    {

        public List<Details> GetHistory(History hstry)
        {

            using (SqlConnection conn = new SqlConnection(Connection.ConnStr()))
            {
                SqlDataAdapter adapter = new SqlDataAdapter();

                
                adapter.SelectCommand = new SqlCommand("Use  RentCars;SELECT *FROM dbo.Cars cl LEFT JOIN dbo.RentedCars c ON cl.IdCar=c.CarId  WHERE UserId=@IdUser " +
                    "AND ( @From  BETWEEN c.RentDate AND c.ReturnedDate   OR @To BETWEEN c.ReturnedDate  AND  c.RentDate ) OR "+
                "(c.RentDate  BETWEEN @From AND @To   OR c.ReturnedDate BETWEEN @To  AND  @From);", conn);
                
                adapter.SelectCommand.Parameters.AddWithValue("@From", hstry.From);
                adapter.SelectCommand.Parameters.AddWithValue("@To", hstry.To);
                adapter.SelectCommand.Parameters.AddWithValue("@IdUser", hstry.IdUser);
                
                DataTable tb = new DataTable();
                adapter.Fill(tb);
                List<Details> d = new List<Details>();
                
                foreach (DataRow dataRow in tb.Rows)
                {
                    Details detail = new Details();

                    detail.Brand = dataRow["Brand"].ToString();
                    detail.Color = dataRow["Color"].ToString();
                    detail.Engine = dataRow["Engine"].ToString();
                    detail.Model = dataRow["Model"].ToString();
                    detail.Power = dataRow["Power"].ToString();
                    detail.Plate = dataRow["RegistrationNumber"].ToString();
                    detail.Price = dataRow["Price"].ToString();
                    detail.From = dataRow["RentDate"].ToString();
                    detail.To = dataRow["ReturnedDate"].ToString();
                    d.Add(detail);
                }
                conn.Close();
                return d;
            }

        }


        public JsonResult History(History h)
        {
            var userLog = GetHistory(h);
            return this.Json(userLog, JsonRequestBehavior.AllowGet);
        }

        public List<Details> GetSummary(History hstry)
        {

            using (SqlConnection conn = new SqlConnection(Connection.ConnStr()))
            {
                SqlDataAdapter adapter = new SqlDataAdapter();

                adapter.SelectCommand = new SqlCommand("Use  RentCars;SELECT cl.Brand,c.RentDate,c.ReturnedDate,cl.Price FROM dbo.Cars cl LEFT JOIN dbo.RentedCars c ON cl.IdCar=c.CarId  WHERE UserId=@IdUser " +
                " AND @From BETWEEN c.RentDate AND c.ReturnedDate; ;", conn);
               
                adapter.SelectCommand.Parameters.AddWithValue("@From", hstry.From);
                adapter.SelectCommand.Parameters.AddWithValue("@IdUser", hstry.IdUser);
               
                DataTable tb = new DataTable();
                adapter.Fill(tb);
                List<Details> d = new List<Details>();
                foreach (DataRow dataRow in tb.Rows)
                {
                    Details detail = new Details();

                    detail.Brand = dataRow["Brand"].ToString();
                    detail.Price = dataRow["Price"].ToString();
                    detail.From = dataRow["RentDate"].ToString();
                    detail.To = dataRow["ReturnedDate"].ToString();
                    d.Add(detail);
                }
                conn.Close();
                return d;
            }

        }

        public JsonResult Summary(History h)
        {
            var userLog = GetSummary(h);
            return this.Json(userLog, JsonRequestBehavior.AllowGet);
        }
    }
}
