using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LA.Enums
{
    public enum ReturnCodesEnum
    {
        SourceFileDoesntExist = -1,
        DKACreationSuccess = 10,
        DKACreationFailed = -10,
        InputFileInWrongFormat = -20,
        LexicalError = -30,
        ExitSuccess = 1
    }
}
