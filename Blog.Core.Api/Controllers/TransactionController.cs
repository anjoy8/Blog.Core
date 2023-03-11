using Blog.Core.IServices;
using Blog.Core.Model;
using Blog.Core.Model.Models;
using Blog.Core.Repository.UnitOfWorks;
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
        private readonly IUnitOfWorkManage _unitOfWorkManage;


        public TransactionController(IUnitOfWorkManage unitOfWorkManage, IPasswordLibServices passwordLibServices, IGuestbookServices guestbookServices)
        {
            _unitOfWorkManage = unitOfWorkManage;
            _passwordLibServices = passwordLibServices;
            _guestbookServices = guestbookServices;
        }

        // GET: api/Transaction
        [HttpGet]
        public async Task<MessageModel<IEnumerable<string>>> Get()
        {
            List<string> returnMsg = new List<string>() { };
            try
            {
                returnMsg.Add($"Begin Transaction");

                _unitOfWorkManage.BeginTran();
                var passwords = await _passwordLibServices.Query(d => d.IsDeleted == false);
                returnMsg.Add($"first time : the count of passwords is :{passwords.Count}");


                returnMsg.Add($"insert a data into the table PasswordLib now.");
                var insertPassword = await _passwordLibServices.Add(new PasswordLib()
                {
                    IsDeleted = false,
                    plAccountName = "aaa",
                    plCreateTime = DateTime.Now
                });


                passwords = await _passwordLibServices.Query(d => d.IsDeleted == false);
                returnMsg.Add($"second time : the count of passwords is :{passwords.Count}");
                returnMsg.Add($" ");

                //......

                var guestbooks = await _guestbookServices.Query();
                returnMsg.Add($"first time : the count of guestbooks is :{guestbooks.Count}");

                int ex = 0;
                returnMsg.Add($"There's an exception!!");
                returnMsg.Add($" ");
                int throwEx = 1 / ex;

                var insertGuestbook = await _guestbookServices.Add(new Guestbook()
                {
                    username = "bbb",
                    blogId = 1,
                    createdate = DateTime.Now,
                    isshow = true
                });

                guestbooks = await _guestbookServices.Query();
                returnMsg.Add($"first time : the count of guestbooks is :{guestbooks.Count}");
                returnMsg.Add($" ");

                _unitOfWorkManage.CommitTran();
            }
            catch (Exception)
            {
                _unitOfWorkManage.RollbackTran();
                var passwords = await _passwordLibServices.Query();
                returnMsg.Add($"third time : the count of passwords is :{passwords.Count}");

                var guestbooks = await _guestbookServices.Query();
                returnMsg.Add($"third time : the count of guestbooks is :{guestbooks.Count}");
            }

            return new MessageModel<IEnumerable<string>>()
            {
                success = true,
                msg = "操作完成",
                response = returnMsg
            };
        }

        // GET: api/Transaction/5
        [HttpGet("{id}")]
        public async Task<MessageModel<string>> Get(int id)
        {
            return await _guestbookServices.TestTranInRepository();
        }

        [HttpGet]
        public async Task<bool> GetTestTranPropagation()
        {
            return await _guestbookServices.TestTranPropagation();
        }

        [HttpGet]
        public async Task<bool> GetTestTranPropagationNoTran()
        {
            return await _guestbookServices.TestTranPropagationNoTran();
        }

        [HttpGet]
        public async Task<bool> GetTestTranPropagationTran()
        {
            return await _guestbookServices.TestTranPropagationTran();
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

        /// <summary>
        /// 测试事务在AOP中的使用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _guestbookServices.TestTranInRepositoryAOP();
        }
    }
}