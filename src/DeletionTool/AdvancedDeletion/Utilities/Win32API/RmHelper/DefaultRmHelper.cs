using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YYProject.AdvancedDeletion
{
    internal sealed class DefaultRmHelper : RmHelper
    {

        public override Boolean TryGetHolderListOfFiles(String[] fileNames, out RmProcessInfo[] result)
        {

            result = null;
            if (NativeAPI.TryRmRegisterResources(base._SessionHandle, fileNames, null, null) && NativeAPI.TryRmGetList(base._SessionHandle, out var suggestion, out result))
            {
                return true;
            }
            return false;

        }
    }
}
