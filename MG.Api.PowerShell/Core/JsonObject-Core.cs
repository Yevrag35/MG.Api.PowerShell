using Microsoft.PowerShell.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace MG.Api.PowerShell
{
    public abstract partial class JsonPsObjectCmdlet
    {
        private ErrorRecord WriteJsonObject(string jsonString, bool asHashtable)
        {
            object obj = JsonObject.ConvertFromJson(jsonString, asHashtable, out ErrorRecord rec);
            if (obj != null)
                base.WriteObject(obj, true);

            return rec;
        }
    }
}
