using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Reflection;

namespace MG.Api.PowerShell
{
    public abstract partial class JsonPsObjectCmdlet : BaseApiCmdlet
    {
        #region FIELDS/CONSTANTS
        protected List<string> StringResults;   // each string is a separate result from an API request.
        private bool _asHash = false;

        #endregion

        #region CMDLET PROCESSING
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            StringResults = new List<string>();
        }

        protected override void ProcessRecord()
        {
            if (StringResults != null && StringResults.Count > 0)
            {
                for (int i = 0; i < StringResults.Count; i++)
                {
                    ErrorRecord errRec = this.WriteJsonObject(StringResults[i], _asHash);
                    if (errRec != null)
                        base.WriteError(errRec);
                }
            }
        }

        #endregion

        #region BACKEND METHODS
        

        #endregion
    }
}