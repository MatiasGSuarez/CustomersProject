using Castle.Core.Internal;
using Castle.Core.Resource;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Model;
using System.Collections.Generic;

namespace IntuitChallengeAPI.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly DbModelContext _context; //Creamos un objeto de la clase DbModelContext, para acceder a la bd de ese contexto.
        public CustomerController(DbModelContext context)
        {
            _context = context;
        } //Constructor de la clase. Para "introducir" el context.

        #region GET
        [HttpGet()]
        public async Task<ActionResult<List<Customer>>> GetCustomersAsync([FromQuery] string name)
        {
            var customers = new List<Customer>();
            if (name == "todos")
                customers = await _context.Customers.ToListAsync();
            else
                customers = await _context.Customers.Where(x => x.CustomerName.Contains(name) || x.CustomerSurname.Contains(name)).ToListAsync();
            return customers;
        }

        #endregion

        #region GET by ID
        [HttpGet("{Id}")] //Buscamos por ID.
        public async Task<Customer> GetCustomersByIdAsync([FromRoute] long Id)
        {
            try
            {
                var entity = await _context.Customers.FindAsync(Id); //Buscando un customer por Id.
                if (entity == null)
                    throw new Exception("El cliente no existe.");
                return entity;
            }
            catch (Exception ex)
            {
                throw;
            }

        } //Esto es una peticion Get. Este endpoint "trae" los registros de la tabla.
        #endregion

        #region POST
        [HttpPost()]
        public async Task<ActionResult<long>> PostCustomerAsync([FromBody] Customer customer)
        {
            if (customer.Id == 0) //Id 0 = usuario nuevo
            {
                customer.Creation = DateTime.Now; 
                var response = await _context.Customers.AddAsync(customer); // Guarda en la base de datos.                
            }
            else
            {
                throw new Exception("Ya se encuentra registrado");
            }
            await _context.SaveChangesAsync();
            return customer.Id;
        }
        #endregion

        #region PUT
        [HttpPut]
        public async Task<ActionResult<long>> PutCustomerAsync([FromBody] Customer customer)
        {

            if (customer.Id != 0) //Id !=0 es un usuario ya registrado.
            {
                var entity = await _context.Customers.FindAsync(customer.Id); //Buscando en la bd para modificarlo.
                if (entity == null)
                    throw new Exception("El cliente no existe.");
                entity.CustomerName = customer.CustomerName;
                entity.CustomerSurname = customer.CustomerSurname;
                entity.BirthDate = customer.BirthDate;
                entity.PhoneNumber = customer.PhoneNumber;
                entity.Email = customer.Email;
                entity.CUIT = customer.CUIT;
                entity.Address = customer.Address;
                entity.Modification = DateTime.Now;

                var response = _context.Customers.Attach(entity); // Guarda en la base de datos. No es un metodo asincrono, por lo que no se usa el await.
            }

            else
            {
                throw new Exception("El usuario no existe.");
            }

            await _context.SaveChangesAsync();
            return customer.Id;
        }
        #endregion

        #region Delete
        [HttpDelete("{Id}")]
        public async Task<ActionResult<bool>> DeleteCustomerAsync([FromRoute] long Id)
        {
            var entity = await _context.Customers.FindAsync(Id); //Buscando en la bd para eliminarlo.
            if (entity == null)
                throw new Exception("El cliente no existe.");
            _context.Customers.Remove(entity);  
           await _context.SaveChangesAsync();
            return true;
        }

        #endregion
    }
}

