using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Products.Helpers
{
    public class FilesHelper
    {

        // PARA TOMAR LAS FOTOS
        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }


    }
}
