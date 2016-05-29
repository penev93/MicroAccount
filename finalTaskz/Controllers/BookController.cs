
using finalTaskz.Models.BookDetails;
using finalTaskz.Models.BookRecord;
using finalTaskz.Models.Cars;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace finalTaskz.Controllers
{
    public class BookController : Controller
    {
        /*Startup brand loading*/
        public List<Car> startBrand()
        {

            using (SqlConnection conn = new SqlConnection(Connection.ConnStr()))
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand("Use RentCars;SELECT Brand FROM dbo.Cars", conn);

                DataTable tb = new DataTable();
                adapter.Fill(tb);
                List<Car> Cars = new List<Car>();
                foreach (DataRow dataRow in tb.Rows)
                {
                    Car c = new Car();
                    c.Brand = dataRow["Brand"].ToString();
                    Cars.Add(c);
                }
                conn.Close();

                return Cars;
            }

        }
        public JsonResult getBrand()
        {
            var userLog = startBrand().AsQueryable().Select(CarViewModel.FromCars);
            return this.Json(userLog, JsonRequestBehavior.AllowGet);

        }

        /*END Startup brand loading*/

        /*Start of Select by brand*/

        public List<Car> Brand(Car car)
        {

            using (SqlConnection conn = new SqlConnection(Connection.ConnStr()))
            {
                SqlDataAdapter adapter = new SqlDataAdapter();

                /*TODO Decode password and pass it as parameter*/
                adapter.SelectCommand = new SqlCommand("Use RentCars;SELECT *  FROM dbo.Cars WHERE Brand=@brand", conn);
                adapter.SelectCommand.Parameters.AddWithValue("@brand", car.Brand);
                DataTable tb = new DataTable();
                adapter.Fill(tb);
                List<Car> Cars = new List<Car>();
                foreach (DataRow dataRow in tb.Rows)
                {
                    Car c = new Car();

                    c.Brand = dataRow["Brand"].ToString();
                    c.Color = dataRow["Color"].ToString();
                    c.Engine = dataRow["Engine"].ToString();
                    c.Model = dataRow["Model"].ToString();
                    c.Power = dataRow["Power"].ToString();
                    c.Plate = dataRow["RegistrationNumber"].ToString();
                    c.Price = dataRow["Price"].ToString();
                    Cars.Add(c);
                }
                conn.Close();

                return Cars;
            }

        }


        public JsonResult byBrand(Car c)
        {
            var userLog = Brand(c).AsQueryable().Select(CarViewModel.FromCars);
            return this.Json(userLog, JsonRequestBehavior.AllowGet);
        }
        /*END of Select by brand*/

        public List<Car> SearchCar(Details detail)
        {
            using (SqlConnection conn = new SqlConnection(Connection.ConnStr()))
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                string cmd = "WHERE ";
                if (detail.Model != null)
                {
                    cmd += "Model=@model AND ";
                }

                if (detail.Brand != null)
                {
                    cmd += "Brand=@brand AND ";
                }

                if (detail.Engine != null)
                {
                    cmd += "Engine=@engine AND ";

                }
                if (detail.Color != null)
                {
                    cmd += "Color=@color AND ";
                }
                if (detail.Plate != null)
                {
                    cmd += "RegistrationNumber=@plate AND ";
                }
                if(cmd=="WHERE "){
                    cmd = "";
                }
                string fakeCommand = "Use  RentCars;select IdCar, Brand ,Model ,RegistrationNumber, Power, Engine ,Color ,Price   from Cars " + cmd +" EXCEPT " +
                    "SELECT c.IdCar, c.Brand ,c.Model ,c.RegistrationNumber, c.Power, c.Engine ,c.Color ,c.Price " +
                    "FROM dbo.RentedCars cl LEFT JOIN dbo.Cars c ON c.IdCar=cl.CarId  WHERE   @From BETWEEN cl.RentDate AND cl.ReturnedDate  OR @To BETWEEN cl.RentDate AND cl.ReturnedDate OR @From < cl.RentDate AND @To > cl.ReturnedDate"; //"Use RentCars;SELECT DISTINCT c.IdCar, c.Brand ,c.Model ,c.RegistrationNumber, c.Power, c.Engine ,c.Color ,c.Price " +

                int pos = fakeCommand.LastIndexOf("EXCEPT");
                if(cmd!=""){
                    string Command = fakeCommand.Remove(pos-5, 4);
                    fakeCommand = Command;
                }
                 
                adapter.SelectCommand = new SqlCommand(fakeCommand, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@From", detail.From);
                adapter.SelectCommand.Parameters.AddWithValue("@To", detail.To);
                if (detail.Model != null)
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@model", detail.Model);
                }

                if (detail.Brand != null)
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@brand", detail.Brand);
                }

                if (detail.Engine != null)
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@engine", detail.Engine);

                }
                if (detail.Color != null)
                {

                    adapter.SelectCommand.Parameters.AddWithValue("@color", detail.Color);
                }
                if (detail.Plate != null)
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@plate", detail.Plate);
                }
                DataTable tb = new DataTable();
                adapter.Fill(tb);
                List<Car> Cars = new List<Car>();
                foreach (DataRow dataRow in tb.Rows)
                {
                    Car c = new Car();

                    c.idCar = dataRow["IdCar"].ToString();
                    c.Brand = dataRow["Brand"].ToString();
                    c.Color = dataRow["Color"].ToString();
                    c.Engine = dataRow["Engine"].ToString();
                    c.Model = dataRow["Model"].ToString();
                    c.Power = dataRow["Power"].ToString();
                    c.Plate = dataRow["RegistrationNumber"].ToString();
                    c.Price = dataRow["Price"].ToString();
                    Cars.Add(c);
                }
                conn.Close();

                return Cars;
            }
        }
        [HttpGet]
        public JsonResult Search(Details d)
        {
            var x = SearchCar(d);
            return this.Json(x, JsonRequestBehavior.AllowGet);
        }
        /*Insert into table  RentedCars booked record*/

        [HttpPost]
        public bool Insert(BookRecord record)
        {


            using (SqlConnection conn = new SqlConnection(Connection.ConnStr()))
            {
                conn.Open();
                SqlTransaction myTrans = conn.BeginTransaction();
                SqlDataAdapter adapter = new SqlDataAdapter();

                adapter.InsertCommand = new SqlCommand("Use RentCars;INSERT INTO dbo.RentedCars(CarId,RentDate,ReturnedDate,UserId) VALUES(@carid,@from,@to,@userid)", conn);
                adapter.InsertCommand.Transaction = myTrans;


                adapter.InsertCommand.Parameters.AddWithValue("@carid", record.CarId);
                adapter.InsertCommand.Parameters.AddWithValue("@from", record.RentDate);
                adapter.InsertCommand.Parameters.AddWithValue("@to", record.ReturnDate);
                adapter.InsertCommand.Parameters.AddWithValue("@userid", record.UserId);

             

                try
                {
                    int affectedRows = adapter.InsertCommand.ExecuteNonQuery();
                    if (affectedRows == 0)
                    {
                        myTrans.Rollback("myTrans");
                    }
                    else
                    {

                        myTrans.Commit();
                    }
                    conn.Close();
                }
                catch (Exception)
                {
                    return false;

                }
                return true;
            }


        }
    }

}