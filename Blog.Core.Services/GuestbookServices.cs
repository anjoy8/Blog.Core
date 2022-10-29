using Blog.Core.Common;
using Blog.Core.IRepository.Base;
using Blog.Core.IServices;
using Blog.Core.Model;
using Blog.Core.Model.Models;
using Blog.Core.Services.BASE;
using System;
using System.Threading.Tasks;
using Blog.Core.Common.DB;
using Blog.Core.Repository.UnitOfWorks;

namespace Blog.Core.Services
{
    public class GuestbookServices : BaseServices<Guestbook>, IGuestbookServices
    {
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly IBaseRepository<PasswordLib> _passwordLibRepository;

        private readonly IPasswordLibServices _passwordLibServices;

        public GuestbookServices(IUnitOfWorkManage unitOfWorkManage, IBaseRepository<Guestbook> dal, IBaseRepository<PasswordLib> passwordLibRepository, IPasswordLibServices passwordLibServices)
        {
            _unitOfWorkManage = unitOfWorkManage;
            _passwordLibRepository = passwordLibRepository;
            _passwordLibServices = passwordLibServices;
        }

        public async Task<MessageModel<string>> TestTranInRepository()
        {
            try
            {
                Console.WriteLine($"");
                Console.WriteLine($"事务操作开始");
                using (var uow = _unitOfWorkManage.CreateUnitOfWork())
                {
                    Console.WriteLine($"");

                    Console.WriteLine($"insert a data into the table PasswordLib now.");
                    var insertPassword = await _passwordLibRepository.Add(new PasswordLib()
                    {
                        IsDeleted = false,
                        plAccountName = "aaa",
                        plCreateTime = DateTime.Now
                    });


                    var passwords = await _passwordLibRepository.Query(d => d.IsDeleted == false);
                    Console.WriteLine($"second time : the count of passwords is :{passwords.Count}");

                    //......

                    Console.WriteLine($"");
                    var guestbooks = await BaseDal.Query();
                    Console.WriteLine($"first time : the count of guestbooks is :{guestbooks.Count}");

                    int ex = 0;
                    Console.WriteLine($"\nThere's an exception!!");
                    int throwEx = 1 / ex;

                    Console.WriteLine($"insert a data into the table Guestbook now.");
                    var insertGuestbook = await BaseDal.Add(new Guestbook()
                    {
                        username = "bbb",
                        blogId = 1,
                        createdate = DateTime.Now,
                        isshow = true
                    });

                    guestbooks = await BaseDal.Query();
                    Console.WriteLine($"second time : the count of guestbooks is :{guestbooks.Count}");

                    uow.Commit();
                }

                return new MessageModel<string>()
                {
                    success = true,
                    msg = "操作完成"
                };
            }
            catch (Exception)
            {
                var passwords = await _passwordLibRepository.Query();
                Console.WriteLine($"third time : the count of passwords is :{passwords.Count}");

                var guestbooks = await BaseDal.Query();
                Console.WriteLine($"third time : the count of guestbooks is :{guestbooks.Count}");

                return new MessageModel<string>()
                {
                    success = false,
                    msg = "操作异常"
                };
            }
        }

        [UseTran]
        public async Task<bool> TestTranInRepositoryAOP()
        {
            var passwords = await _passwordLibRepository.Query();
            Console.WriteLine($"first time : the count of passwords is :{passwords.Count}");


            Console.WriteLine($"insert a data into the table PasswordLib now.");
            var insertPassword = await _passwordLibRepository.Add(new PasswordLib()
            {
                IsDeleted = false,
                plAccountName = "aaa",
                plCreateTime = DateTime.Now
            });


            passwords = await _passwordLibRepository.Query(d => d.IsDeleted == false);
            Console.WriteLine($"second time : the count of passwords is :{passwords.Count}");

            //......

            Console.WriteLine($"");
            var guestbooks = await BaseDal.Query();
            Console.WriteLine($"first time : the count of guestbooks is :{guestbooks.Count}");

            int ex = 0;
            Console.WriteLine($"\nThere's an exception!!");
            int throwEx = 1 / ex;

            Console.WriteLine($"insert a data into the table Guestbook now.");
            var insertGuestbook = await BaseDal.Add(new Guestbook()
            {
                username = "bbb",
                blogId = 1,
                createdate = DateTime.Now,
                isshow = true
            });

            guestbooks = await BaseDal.Query();
            Console.WriteLine($"second time : the count of guestbooks is :{guestbooks.Count}");

            return true;
        }

        /// <summary>
        /// 测试使用同事务
        /// </summary>
        /// <returns></returns>
        [UseTran(Propagation = Propagation.Required)]
        public async Task<bool> TestTranPropagation()
        {
            var guestbooks = await base.Query();
            Console.WriteLine($"first time : the count of guestbooks is :{guestbooks.Count}");

            var insertGuestbook = await base.Add(new Guestbook()
            {
                username = "bbb",
                blogId = 1,
                createdate = DateTime.Now,
                isshow = true
            });

            await _passwordLibServices.TestTranPropagation2();

            return true;
        }


        /// <summary>
        /// 测试无事务 Mandatory传播机制报错
        /// </summary>
        /// <returns></returns>
        public async Task<bool> TestTranPropagationNoTran()
        {
            var guestbooks = await base.Query();
            Console.WriteLine($"first time : the count of guestbooks is :{guestbooks.Count}");

            var insertGuestbook = await base.Add(new Guestbook()
            {
                username = "bbb",
                blogId = 1,
                createdate = DateTime.Now,
                isshow = true
            });

            await _passwordLibServices.TestTranPropagationNoTranError();

            return true;
        }


        /// <summary>
        /// 测试嵌套事务
        /// </summary>
        /// <returns></returns>
        [UseTran(Propagation = Propagation.Required)]
        public async Task<bool> TestTranPropagationTran()
        {
            var guestbooks = await base.Query();
            Console.WriteLine($"first time : the count of guestbooks is :{guestbooks.Count}");

            var insertGuestbook = await base.Add(new Guestbook()
            {
                username = "bbb",
                blogId = 1,
                createdate = DateTime.Now,
                isshow = true
            });

            await _passwordLibServices.TestTranPropagationTran2();

            return true;
        }
    }
}