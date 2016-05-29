using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace finalTaskz.Models.BookDetails
{
    public class DetailViewModel
    {
       public static  System.Linq.Expressions.Expression<Func<Details, DetailViewModel>> FromDetails
        {
            get
            {
                return d => new DetailViewModel
                {
                   To=d.To
                };
            }
        }
       public string idCar { get; set; }
       public string Brand { get; set; }
       public string Model { get; set; }
       public string Power { get; set; }
       public string Plate { get; set; }
       public string Engine { get; set; }
       public string Color { get; set; }
       public string Price { get; set; }
       public string From { get; set; }
       public string To { get; set; }
    }
}