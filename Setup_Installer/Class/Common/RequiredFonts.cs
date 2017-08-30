using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProlexNetSetup.Class.Common
{
    public class RequiredFonts
    {
        public static void Magneto()
        {
            try
            {
                Shell32.Shell shell = new Shell32.Shell();
                Shell32.Folder fontFolder = shell.NameSpace(0x14);
                fontFolder.CopyHere(@"\Resources\MAGNETOB.ttf");
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Um erro ocorreu durante a instalação da fonte 'Magneto'.",
                    ex);
            }

            return;
        }
    }
}
