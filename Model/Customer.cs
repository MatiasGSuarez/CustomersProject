using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Customer : BaseClass
    {
         
        public String CustomerName { get; set; }
        public String CustomerSurname { get; set; } 
        public DateTime BirthDate { get; set; } 
        public String CUIT {  get; set; }
        public String Address { get; set; }
        public String PhoneNumber { get; set; }
        public String Email { get; set; }

    }
}



