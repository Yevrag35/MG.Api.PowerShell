using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MG.Api.PowerShell
{
    public class CmdletNotReadyException : InvalidOperationException
    {
        private const string MSG = "This cmdlet is not ready, more than likely because the context is not set.";

        public CmdletNotReadyException()
            : base(MSG) { }

        public CmdletNotReadyException(string message)
            : base(message) { }

        public CmdletNotReadyException(Exception e)
            : base(MSG, e) { }

        public CmdletNotReadyException(string message, Exception e)
            : base(message, e) { }
    }
}
