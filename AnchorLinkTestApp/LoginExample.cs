using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AnchorLinkSharp;
using AnchorLinkUnityTransportSharp;
using EosSharp;
using EosSharp.Core;
using EosSharp.Core.Api.v1;

namespace AnchorLinkTestApp
{
    public class LoginExample
    {
        // app identifier, should be set to the eosio contract account if applicable
        private const string identifier = "example";

        // initialize the link
        private AnchorLink link = new AnchorLink(new LinkOptions()
        {
            transport = new UnityTransport(new TransportOptions()),
            chainId = "aca376f206b8fc25a6ed44dbdc66547c36c6c33e3a119ffbeaef943642f0e906",
            rpc = "https://eos.greymass.com",
            ZlibProvider = new NetZlibProvider(),
            storage = new JsonLocalStorage()
            //chains: [{
            //    chainId: 'aca376f206b8fc25a6ed44dbdc66547c36c6c33e3a119ffbeaef943642f0e906',
            //    nodeUrl: 'https://eos.greymass.com',
            //}]
        });

        // the session instance, either restored using link.restoreSession() or created with link.login()
        private LinkSession session;

        // tries to restore session, called when document is loaded
        public async Task restoreSession()
        {
            var restoreSessionResult = await link.restoreSession(identifier);
            session = restoreSessionResult;
            if (session != null)
                didLogin();
        }

        // login and store session if sucessful
        public async Task login()
        {
            var loginResult = await link.login(identifier);
            session = loginResult.session;
            didLogin();
        }

        // logout and remove session from storage
        public async Task logout()
        {
            await session.remove();
        }

        // called when session was restored or created
        public void didLogin()
        {
            Console.WriteLine($"{session.auth.actor} logged-in");
        }

        // transfer tokens using a session
        public async Task transfer()
        {
            var action = new EosSharp.Core.Api.v1.Action()
            {
                account = "eosio.token",
                name = "transfer",
                authorization = new List<PermissionLevel>() { session.auth },
                data = new Dictionary<string, object>()
                {
                    { "from", session.auth.actor },
                    { "to", "teamgreymass" },
                    { "quantity", "0.0001 EOS" },
                    { "memo", "Anchor is the best! Thank you <3" }
                }
            };

            var transactResult = await session.transact(new TransactArgs() { action = action });
            Console.WriteLine($"Transaction broadcast! {transactResult.processed}");
        }
    }
}
