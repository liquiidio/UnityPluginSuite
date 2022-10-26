using System;
using System.Collections.Generic;
using AnchorLinkSharp;
using AnchorLinkUnityTransportSharp;
using EosSharp.Core.Api.v1;

namespace AnchorLinkTestApp
{
    public class TransactExample
    {

        // initialize the link
        private readonly AnchorLink _link = new AnchorLink(new LinkOptions()
        {
            Transport = new UnityTransport(new TransportOptions()),
            ChainId = "4667b205c6838ef70ff7988f6e8257e8be0e1284a2f59699054a018f743b1d11",
            Rpc = "https://telos.greymass.com",
            ZlibProvider = new NetZlibProvider(),
            Storage = new JsonLocalStorage()
            //chains: [{
            //    chainId: 'aca376f206b8fc25a6ed44dbdc66547c36c6c33e3a119ffbeaef943642f0e906',
            //    nodeUrl: 'https://eos.greymass.com',
            //}]
        });



        // the EOSIO action we want to sign and broadcast
        public EosSharp.Core.Api.v1.Action Action = new EosSharp.Core.Api.v1.Action()
        {
            account = "eosio",
            name = "voteproducer",
            authorization = new List<PermissionLevel>()
            {
                new PermissionLevel()
                {
                    actor = "............1", // ............1 will be resolved to the signing accounts permission
                    permission = "............2" // ............2 will be resolved to the signing accounts authority
                }
            },
            data = new Dictionary<string, object>()
            {
                { "voter", "............1" },
                { "proxy", "coredevproxy" },
                { "producers", Array.Empty<object>() },
            }
        };

        // ask the user to sign the transaction and then broadcast to chain
        public void Vote()
        {
            _link.Transact(new TransactArgs() { Action = Action }).ContinueWith(transactTask =>
            {
                Console.WriteLine($"Thank you {transactTask.Result.Signer.actor}");
            });
        }
    }
}
