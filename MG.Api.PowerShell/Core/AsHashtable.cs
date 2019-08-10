using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Reflection;

namespace MG.Api.PowerShell
{
    public abstract partial class JsonPsObjectCmdlet
    {
        #region PARAMETER
        public SwitchParameter AsHashtable
        {
            get => _asHash;
            set => _asHash = value;
        }

        #endregion
    }
}