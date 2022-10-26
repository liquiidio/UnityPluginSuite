using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AnchorLinkTestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var loginExample = new LoginExample();
                await loginExample.Login();
                try
                {
                    // throws if the account doesn't have enough CPU
                    await loginExample.Transfer();
                }
                catch (Exception e)
                {
                    Console.WriteLine(JsonConvert.SerializeObject(e));
                }
                // logout removes the session so it's not restorable
//                await loginExample.logout();
                await loginExample.RestoreSession();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            try
            {
                var transactExample = new TransactExample();
                transactExample.Vote();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
