using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AnchorLinkSharp;
using AnchorLinkUnityTransportSharp;
using EosSharp.Core.Api.v1;

namespace AnchorLinkTestApp
{
    public class LoginExample
    {
        // app identifier, should be set to the eosio contract account if applicable
        private const string Identifier = "example";

        // initialize the link
        private readonly AnchorLink _link = new AnchorLink(new LinkOptions()
        {
            Transport = new UnityTransport(new TransportOptions()),
            ChainId = "aca376f206b8fc25a6ed44dbdc66547c36c6c33e3a119ffbeaef943642f0e906",
            Rpc = "https://eos.greymass.com",
            ZlibProvider = new NetZlibProvider(),
            Storage = new JsonLocalStorage()
            //chains: [{
            //    chainId: 'aca376f206b8fc25a6ed44dbdc66547c36c6c33e3a119ffbeaef943642f0e906',
            //    nodeUrl: 'https://eos.greymass.com',
            //}]
        });

        // the session instance, either restored using link.restoreSession() or created with link.login()
        private LinkSession _session;

        // tries to restore session, called when document is loaded
        public async Task RestoreSession()
        {
            var restoreSessionResult = await _link.RestoreSession(Identifier);
            _session = restoreSessionResult;
            if (_session != null)
                DidLogin();
        }

        // login and store session if sucessful
        public async Task Login()
        {
            var loginResult = await _link.Login(Identifier);
            _session = loginResult.Session;
            DidLogin();
        }

        // logout and remove session from storage
        public async Task Logout()
        {
            await _session.Remove();
        }

        // called when session was restored or created
        public void DidLogin()
        {
            Console.WriteLine($"{_session.Auth.actor} logged-in");
        }

        // transfer tokens using a session
        public async Task Transfer()
        {
            var action = new EosSharp.Core.Api.v1.Action()
            {
                account = "eosio.token",
                name = "transfer",
                authorization = new List<PermissionLevel>() { _session.Auth },
                data = new Dictionary<string, object>()
                {
                    { "from", _session.Auth.actor },
                    { "to", "teamgreymass" },
                    { "quantity", "0.0001 EOS" },
                    { "memo", "Anchor is the best! Thank you <3" }
                }
            };

            var transactResult = await _session.Transact(new TransactArgs() { Action = action });
            Console.WriteLine($"Transaction broadcast! {transactResult.Processed}");
        }
    }
}
