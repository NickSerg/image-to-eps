using System;
using System.IO;
using System.Linq;

namespace ITE.Infrastructure.Helpers
{
    public static class FileHelper
    {
        public static bool IsImageFile(string fileName)
        {
            return
                new[] {".jpg", ".jpeg", ".bmp", ".png", ".tiff", ".gif"}
                    .Contains(Path.GetExtension(fileName) ?? string.Empty,
                        StringComparer.InvariantCultureIgnoreCase);
        }
    }
}
