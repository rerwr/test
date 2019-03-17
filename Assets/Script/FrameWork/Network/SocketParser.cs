using System.Collections.Generic;
using System;
using Game;
using Google.ProtocolBuffers;


namespace Framework
{
    public delegate IMessage ParserFun(byte[] buff);
    public class SocketParser : Singleton<SocketParser>
    {
        private List<List<ParserFun>> _parsers = new List<List<ParserFun>>(256);
        public void RegisterParser(byte module, byte sub, ParserFun parser)
        {
            var parserList = GetFunList(module, sub);
            parserList[sub] = parser;
        }

        public ParserFun GetParser(byte extId, byte sub)
        {
            int ie = extId;
            int ic = sub;
            if (_parsers.Count <= ie)
            {
                return null;
            }
            var parserList = _parsers[ie];
            if (parserList.Count <= ic)
            {
                return null;
            }
            return parserList[sub];
        }

        private List<ParserFun> GetFunList(byte module, byte sub)
        {
            int ie = module;
            int ic = sub;
            while (_parsers.Count <= ie)
            {
                _parsers.Add(new List<ParserFun>());
            }
            var parserList = _parsers[ie];
            while (parserList.Count <= ic)
            {
                parserList.Add(null);
            }
            return parserList;
        }
    }
}
