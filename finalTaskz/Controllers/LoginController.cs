using finalTaskz.Models.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace finalTaskz.Controllers
{
    public class LoginController : Controller
    {
        
        public List<User> UserCheck(User user)
        {
          
            using(SqlConnection conn=new SqlConnection(Connection.ConnStr())){
                SqlDataAdapter adapter = new SqlDataAdapter();

             
                adapter.SelectCommand = new SqlCommand("Use RentCars;SELECT * FROM dbo.Users WHERE Username=@userName", conn);
                adapter.SelectCommand.Parameters.AddWithValue("@userName", user.Username);
                DataTable tb = new DataTable();
                adapter.Fill(tb);
                List<User> Users = new List<User>();
                foreach (DataRow dataRow in tb.Rows)

                {
                    User User = new User();
                    user.IdUser = dataRow["IdUser"].ToString();
                    user.Email = dataRow["Email"].ToString();
                    user.Firstname = dataRow["Firstname"].ToString();
                    user.Lastname = dataRow["Lastname"].ToString();
                    user.Username = dataRow["Username"].ToString();
                    user.Password = Decrypt(dataRow["Password"].ToString());
                
                    Users.Add(user);
                }
                conn.Close();
               
                return Users;
            }
            
        
        }

        [HttpPost]
        public JsonResult getUser(User user)
        {
            var userLog = UserCheck(user).AsQueryable().Select(UserViewModel.FromUsers);
            return this.Json(userLog, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Username(User user)
        {
            
            return PartialView();
        }
        
        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }


    }
}
