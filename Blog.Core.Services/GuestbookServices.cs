using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Common;
using Blog.Core.IRepository;
using Blog.Core.IRepository.UnitOfWork;
using Blog.Core.IServices;
using Blog.Core.Model.Models;
using Blog.Core.Services.BASE;

namespace Blog.Core.Services
{
    public class GuestbookServices : BaseServices<Guestbook>, IGuestbookServices
    {
        private readonly IGuestbookRepository _dal;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordLibRepository _passwordLibRepository;
        public GuestbookServices(IUnitOfWork unitOfWork, IGuestbookRepository dal, IPasswordLibRepository passwordLibRepository)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
            _passwordLibRepository = passwordLibRepository;
        }

        public async Task<bool> TestTranInRepository()
        {
            try
            {
                Console.WriteLine($"");
                Console.WriteLine($"Begin Transaction");
                _unitOfWork.BeginTran();
                Console.WriteLine($"");






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
                var guestbooks = await _dal.Query();
                Console.WriteLine($"first time : the count of guestbooks is :{guestbooks.Count}");

                int ex = 0;
                Console.WriteLine($"\nThere's an exception!!");
                int throwEx = 1 / ex;

                Console.WriteLine($"insert a data into the table Guestbook now.");
                var insertGuestbook = await _dal.Add(new Guestbook()
                {
                    username = "bbb",
                    blogId = 1,
                    createdate = DateTime.Now,
                    isshow = true
                });

                guestbooks = await _dal.Query();
                Console.WriteLine($"second time : the count of guestbooks is :{guestbooks.Count}");










                _unitOfWork.CommitTran();

                return true;
            }
            catch (Exception)
            {
                _unitOfWork.RollbackTran();
                var passwords = await _passwordLibRepository.Query();
                Console.WriteLine($"third time : the count of passwords is :{passwords.Count}");

                var guestbooks = await _dal.Query();
                Console.WriteLine($"third time : the count of guestbooks is :{guestbooks.Count}");

                return false;
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
            var guestbooks = await _dal.Query();
            Console.WriteLine($"first time : the count of guestbooks is :{guestbooks.Count}");

            int ex = 0;
            Console.WriteLine($"\nThere's an exception!!");
            int throwEx = 1 / ex;

            Console.WriteLine($"insert a data into the table Guestbook now.");
            var insertGuestbook = await _dal.Add(new Guestbook()
            {
                username = "bbb",
                blogId = 1,
                createdate = DateTime.Now,
                isshow = true
            });

            guestbooks = await _dal.Query();
            Console.WriteLine($"second time : the count of guestbooks is :{guestbooks.Count}");

            return true;
        }

    }
}
