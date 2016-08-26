using PetaPoco;
using System;
using System.Collections.Generic;
using YanZhiwei.DotNet.Framework.Contract;

namespace YanZhiwei.DotNet.Framework.DataExample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                //Database db = new Database(@"Data Source=YANZHIWEI-IT-PC\SQLEXPRESS;Initial Catalog=pubs;Persist Security Info=True;User ID=sa;Password=sasa", DbProviderFactories.GetFactory("System.Data.SqlClient"));
                //IEnumerable<Employee> _result = db.Query<Employee>("select * from employee");
                ServiceCallContext.Current.Operater = new Operater()
                {
                    IP = "127.0.0.1",
                    Name = "test",
                    Time = DateTime.Now,
                    Token = Guid.NewGuid(),
                    UserId = 1
                };
                using(PubDbContext db = new PubDbContext())
                {
                    List<Employee> _result2 = db.Fetch<Employee>("select * from [employee] where job_id>@0 and hire_date>@1", 2, "1953-08-30 00:00:00.000");
                    List<Employee> _result = db.ToCacheList<Employee>("select * from [employee] where job_id>@0 and hire_date>@1", 2, "1953-08-30 00:00:00.000");
                    //Console.WriteLine(_result == null ? "0" : _result.Count.ToString());
                    Page<Employee> _pagedResult = db.Page<Employee>(1, 20, "select * from employee ORDER BY emp_id DESC");
                    _pagedResult = db.ToPageCache<Employee>(1, 10, "select * from employee ORDER BY emp_id DESC");
                    _pagedResult = db.ToPageCache<Employee>(1, 2, "select * from [employee] where job_id>@0 and hire_date>@1 ORDER BY emp_id DESC", 2, "1953-08-30 00:00:00.000");
                    //Authors _author = new Authors();
                    //_author.au_id = "172-32-2226";
                    //_author.au_lname = "zhiwei";
                    //_author.au_fname = "yan";
                    //_author.phone = "415 548-2222";
                    //_author.address = "zhuzhou";
                    //_author.city = "zhuzhou";
                    //_author.state = "KS";
                    //_author.zip = "94609";
                    //_author.contract = true;
                    //db.Insert(_author);
                    db.Update<Authors>("SET au_lname=@0 WHERE au_id=@1", "zhiwei223", "172-32-2222");
                }
                using(PubDbContext db = new PubDbContext())
                {
                    db.Update<Authors>("SET au_lname=@0 WHERE au_id=@1", "zhiwei223", "172-32-2222");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}