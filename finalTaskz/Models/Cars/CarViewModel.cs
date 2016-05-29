using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace finalTaskz.Models.Cars
{
    public class CarViewModel
    {
        public static Expression<Func<Car,CarViewModel>> FromCars{
            get
            {
                return car => new CarViewModel
                {
                    idCar=car.idCar,
                    Brand=car.Brand,
                    Model=car.Model,
                    Power=car.Power,
                    Engine=car.Engine,
                    Color=car.Color,
                    Plate=car.Plate,
                    Price=car.Price,
                     
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
        
    }
}