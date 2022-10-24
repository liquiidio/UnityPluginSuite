using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AnchorLinkSharp;
using AnchorLinkUnityTransportSharp;
using EosSharp;
using EosSharp.Core;
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
                await loginExample.login();
                try
                {
                    // throws if the account doesn't have enough CPU
                    await loginExample.transfer();
                }
                catch (Exception e)
                {
                    Console.WriteLine(JsonConvert.SerializeObject(e));
                }
                // logout removes the session so it's not restorable
//                await loginExample.logout();
                await loginExample.restoreSession();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            try
            {
                var transactExample = new TransactExample();
                transactExample.vote();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
