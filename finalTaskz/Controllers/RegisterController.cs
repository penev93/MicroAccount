using finalTaskz.Models.Users;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace finalTaskz.Controllers
{
    public class RegisterController : Controller
    {
       
        [HttpPost]
        public bool Insert(User user)
        {

              using (SqlConnection conn = new SqlConnection(Connection.ConnStr()))
            {
                conn.Open();
                SqlTransaction myTrans = conn.BeginTransaction();
                SqlDataAdapter adapter = new SqlDataAdapter();
                
                adapter.InsertCommand = new SqlCommand("Use RentCars;INSERT INTO dbo.Users(Username,Password,Firstname,Lastname,Email) VALUES(@userName,@passwrod,@firstName,@lastName,@email)", conn);
                adapter.InsertCommand.Transaction = myTrans;

                adapter.InsertCommand.Parameters.AddWithValue("@userName", user.Username);
                adapter.InsertCommand.Parameters.AddWithValue("@passwrod", Encrypt(user.Password));
                adapter.InsertCommand.Parameters.AddWithValue("@firstName", user.Firstname);
                adapter.InsertCommand.Parameters.AddWithValue("@lastName", user.Lastname);
                adapter.InsertCommand.Parameters.AddWithValue("@email", user.Email);
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

        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                //Gets or sets the secret key for the symmetric algorithm.
                encryptor.Key = pdb.GetBytes(32);
                //Gets or sets the initialization vector (IV) for the symmetric algorithm
                encryptor.IV = pdb.GetBytes(16);
                //Create the streams used for encryption. 
                using (MemoryStream ms = new MemoryStream())
                {
                    //CryptoStream->    Defines a stream that links data streams to cryptographic transformations.
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
    }

}
