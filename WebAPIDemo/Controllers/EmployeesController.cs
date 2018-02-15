using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeDataAccess;
using System.Web.Http.Cors;

namespace WebAPIDemo.Controllers
{
    //[EnableCorsAttribute("*", "*", "*")] // This enables Cors for the EmployeeController
    public class EmployeesController : ApiController
    {
        // Creating a customer api method 
        // [HttpGet]
        // public IEnumerable<Employee> Get(String gender = "All")
        public HttpResponseMessage Get(String gender = "All")
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities()) {

                switch (gender.ToLower())
                {
                    case "all":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.ToList());
                    case "male":
                        return Request.CreateResponse(HttpStatusCode.OK, 
                            entities.Employees.Where(e => e.Gender.ToLower() == "male").ToList());
                    case "female":
                        return Request.CreateResponse(HttpStatusCode.OK,
                            entities.Employees.Where(e => e.Gender.ToLower() == "female").ToList());
                    default:
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Value for gender must be either ALL, MALE or FEMALE. " + gender + " is invalid.");

                }

                // return a list of all employees
                // return entities.Employees.ToList();
            }
        }

        // [DisableCors] // This disables Cors for single method
        public HttpResponseMessage Get (int id) {

            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                // return the requested ID
                var entity = entities.Employees.FirstOrDefault(e => e.ID == id);

                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with Id = " + id.ToString() + " not found!");
                }
            }
        }

        public HttpResponseMessage Post([FromBody] Employee employee)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    entities.Employees.Add(employee);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + employee.ID.ToString());

                    return message;
                }
            }
            catch (Exception ex){
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Delete(int id) {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                try{ 
                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with ID = " + id.ToString() + " not found to delete");
                    }
                    else
                    {
                        entities.Employees.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
                    catch(Exception ex)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);                                                                                                              
                    
                    }
            }
        }

        public HttpResponseMessage Put([FromUri]int id, [FromBody]Employee employee)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities()) {
                var entity = entities.Employees.FirstOrDefault(e => e.ID == id);

                try
                {
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with ID = " + id.ToString() + " not found to update");
                    }
                    else
                    {

                        entity.FirstName = employee.FirstName;
                        entity.LastName = employee.LastName;
                        entity.Gender = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(employee.Gender);
                        entity.Salary = employee.Salary;

                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    } 
                }
                catch(Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }
    }
}
