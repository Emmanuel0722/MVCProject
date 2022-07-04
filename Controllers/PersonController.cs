using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVCProject.Models;
using MVCProject.Data;
using System.Data;
using System.Data.SqlClient;

namespace MVCProject.Controllers
{
    // Todavia me falta arreglar algunas cosas, pero ya este projecto funciona bien.
    [Controller]
    public class PersonController : Controller
    {
        private readonly string context;

        public PersonController(IConfiguration _context)
        {
            context = _context.GetConnectionString("SqlCon");
        }

        public IActionResult Index(int pg = 1){
            
            List<Person> person = new List<Person>();

            using(SqlConnection con = new SqlConnection(context)){
                con.Open();
                SqlCommand com = new SqlCommand("sp_ReadPersonTable", con);
                com.CommandType = CommandType.StoredProcedure;

                using(var dt = com.ExecuteReader()){
                    while (dt.Read())
                    {
                        person.Add(new Person(){
                            Id = Convert.ToInt32(dt["Id"]),
                            firstName = dt["firstName"].ToString(),
                            lastName = dt["lastName"].ToString(),
                            cellPhone = dt["cellPhone"].ToString(),
                            gender = dt["gender"].ToString()
                        });
                    }
                }
            }

            const int pageSize = 5;
            if (pg < 1)
                pg = 1;

            int recsCount = person.Count();

            var pager = new Pagination(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            var data = person.Skip(recSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pagination = pager;
            
            return View(data);
        }

        public IActionResult Details(int Id){
            List<Person> person = new List<Person>();
            var pers = new Person();

            using(SqlConnection con = new SqlConnection(context)){
                con.Open();
                SqlCommand com = new SqlCommand("sp_ReadPersonTable", con);
                com.CommandType = CommandType.StoredProcedure;

                using(var dt = com.ExecuteReader()){
                    while (dt.Read())
                    {
                        person.Add(new Person(){
                            Id = Convert.ToInt32(dt["Id"]),
                            firstName = dt["firstName"].ToString(),
                            lastName = dt["lastName"].ToString(),
                            cellPhone = dt["cellPhone"].ToString(),
                            gender = dt["gender"].ToString()
                        });
                    }
                }
                con.Close();
            }

            pers = person.Where(item => item.Id == Id).FirstOrDefault();
            return View(pers);
        }

        public IActionResult AddPerson(){
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddPerson(Person person){
            
            using (SqlConnection con = new SqlConnection(context))
            {
                con.Open();

                SqlCommand com = new SqlCommand("sp_CreatePersonTable", con);
                com.Parameters.AddWithValue("firstName", person.firstName);
                com.Parameters.AddWithValue("lastName", person.lastName);
                com.Parameters.AddWithValue("cellPhone", person.cellPhone);
                com.Parameters.AddWithValue("gender", person.gender);
                com.CommandType = CommandType.StoredProcedure;
                com.ExecuteNonQuery();

                con.Close();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int Id){
            List<Person> person = new List<Person>();
            var pers = new Person();

            using(var con = new SqlConnection(context)){
                con.Open();
                SqlCommand com = new SqlCommand("sp_ReadPersonTable", con);
                com.CommandType = CommandType.StoredProcedure;

                using(var dt = com.ExecuteReader()){
                    while (dt.Read())
                    {
                        person.Add(new Person(){
                            Id = Convert.ToInt32(dt["Id"]),
                            firstName = dt["firstName"].ToString(),
                            lastName = dt["lastName"].ToString(),
                            cellPhone = dt["cellPhone"].ToString(),
                            gender = dt["gender"].ToString()
                        });
                    }
                }
            }

            pers = person.Where(item => item.Id == Id).FirstOrDefault();

             return View(pers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromForm] Person person){

            using (SqlConnection con = new SqlConnection(context))
            {
                con.Open();
                SqlCommand com = new SqlCommand("sp_UpdatePersonTable", con);
                com.Parameters.AddWithValue("Id", person.Id);
                com.Parameters.AddWithValue("firstName", person.firstName);
                com.Parameters.AddWithValue("lastName", person.lastName);
                com.Parameters.AddWithValue("cellPhone", person.cellPhone);
                com.Parameters.AddWithValue("gender", person.gender);
                com.CommandType = CommandType.StoredProcedure;
                com.ExecuteNonQuery();
                con.Close();
            }

            return RedirectToAction("Index");
        }
        
        public IActionResult Delete(int Id){
            List<Person> person = new List<Person>();
            var pers = new Person();

            using(var con = new SqlConnection(context)){
                con.Open();
                SqlCommand com = new SqlCommand("sp_ReadPersonTable", con);
                com.CommandType = CommandType.StoredProcedure;

                using(var dt = com.ExecuteReader()){
                    while (dt.Read())
                    {
                        person.Add(new Person(){
                            Id = Convert.ToInt32(dt["Id"]),
                            firstName = dt["firstName"].ToString(),
                            lastName = dt["lastName"].ToString(),
                            cellPhone = dt["cellPhone"].ToString(),
                            gender = dt["gender"].ToString()
                        });
                    }
                }
            }

            pers = person.Where(item => item.Id == Id).FirstOrDefault();
            return View(pers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromForm] Person I){

            using(SqlConnection con = new SqlConnection(context)){
                con.Open();

                SqlCommand com = new SqlCommand("sp_DeletePersonTable", con);
                com.Parameters.AddWithValue("Id", I.Id);
                com.CommandType = CommandType.StoredProcedure;
                com.ExecuteNonQuery();

                con.Close();
            }

            return RedirectToAction("Index");
        }
    }
}