using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Core.IRepository.UnitOfWork;
using Blog.Core.IServices;
using Blog.Core.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class TransactionController : ControllerBase
    {
        private readonly IPasswordLibServices _passwordLibServices;
        private readonly IGuestbookServices _guestbookServices;
        private readonly IUnitOfWork _unitOfWork;


        public TransactionController(IUnitOfWork unitOfWork, IPasswordLibServices passwordLibServices, IGuestbookServices guestbookServices)
        {
            _unitOfWork = unitOfWork;
            _passwordLibServices = passwordLibServices;
            _guestbookServices = guestbookServices;
        }

        // GET: api/Transaction
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            try
            {
                Console.WriteLine($"");
                Console.WriteLine($"Begin Transaction");
                _unitOfWork.BeginTran();
                Console.WriteLine($"");
                var passwords = await _passwordLibServices.Query(d=>d.IsDeleted==false);
                Console.WriteLine($"first time : the count of passwords is :{passwords.Count}");


                Console.WriteLine($"insert a data into the table PasswordLib now.");
                var insertPassword = await _passwordLibServices.Add(new PasswordLib()
                {
                    IsDeleted = false,
                    plAccountName = "aaa",
                    plCreateTime = DateTime.Now
                });


                passwords = await _passwordLibServices.Query(d => d.IsDeleted == false);
                Console.WriteLine($"second time : the count of passwords is :{passwords.Count}");

                //......

                Console.WriteLine($"");
                var guestbooks = await _guestbookServices.Query();
                Console.WriteLine($"first time : the count of guestbooks is :{guestbooks.Count}");

                int ex = 0;
                Console.WriteLine($"\nThere's an exception!!");
                int throwEx = 1 / ex;

                Console.WriteLine($"insert a data into the table Guestbook now.");
                var insertGuestbook = await _guestbookServices.Add(new Guestbook()
                {
                    username = "bbb",
                    blogId = 1,
                    createdate = DateTime.Now,
                    isshow = true
                });

                guestbooks = await _guestbookServices.Query();
                Console.WriteLine($"second time : the count of guestbooks is :{guestbooks.Count}");

                _unitOfWork.CommitTran();
            }
            catch (Exception)
            {
                _unitOfWork.RollbackTran();
                var passwords = await _passwordLibServices.Query();
                Console.WriteLine($"third time : the count of passwords is :{passwords.Count}");

               var guestbooks = await _guestbookServices.Query();
                Console.WriteLine($"third time : the count of guestbooks is :{guestbooks.Count}");
            }

            return new string[] { "value1", "value2" };
        }

        // GET: api/Transaction/5
        [HttpGet("{id}")]
        public async Task<bool> Get(int id)
        {
            return await _guestbookServices.TestTranInRepository();
        }

        // POST: api/Transaction
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Transaction/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _guestbookServices.TestTranInRepositoryAOP();
        }
    }
}
