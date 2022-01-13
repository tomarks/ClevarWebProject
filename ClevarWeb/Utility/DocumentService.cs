using ClevarWeb.Data;
using ClevarWeb.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ClevarWeb.Utility
{
    /// <summary>
    /// Saves files to the documents folder and returns a guid.
    /// </summary>
    public static class DocumentService
    {
        public static readonly string DocumentsFolderRelativePath = "/documents/";
        public static string ResolveDocumentRelativePath(string fileName) => Path.Join(DocumentsFolderRelativePath, fileName);

        public static FileInfo UploadFile(string fileName, byte[] fileData)
        {
            FileInfo sourceFile = new FileInfo(fileName);
            FileInfo destFile = new FileInfo(Guid.NewGuid().ToString() + sourceFile.Extension);

            WriteDocumentToDisk(destFile.Name, fileData);

            return destFile;
        }

        public static void WriteDocumentToDisk(string fileName, byte[] fileData)
        {
            try
            {
                FileInfo fi = new FileInfo(Path.Join(Utility.General.Instance.wwwRootPath, DocumentsFolderRelativePath, fileName));

                if (!Directory.Exists(fi.Directory.FullName))
                    Directory.CreateDirectory(fi.Directory.FullName);

                File.WriteAllBytes(fi.FullName, fileData);

                // If its an image we will also create an optimised version

            }
            catch (Exception e)
            {
                Utility.General.Instance.WriteToLog(e.ToString(), Microsoft.Extensions.Logging.LogLevel.Error);
            }
        }

        public static void RebuildDatabaseDocuments<dbcontext>(dbcontext db) where dbcontext : ClevarDbContext
        {
            var docs = db.Documents.Include(x => x.DocumentData).ToList();
            foreach (var doc in docs)
            {
                try
                {
                    if(doc.DocumentData != null)
                    {
                        WriteDocumentToDisk(doc.FileName, doc.DocumentData.FileData);
                        CreateThumbnail(doc);
                    }
                }
                catch (Exception e)
                {
                    General.Instance.WriteToLog(e.ToString());
                }
            }

        }

        public static void CreateThumbnail(Document Source, int maxSize = 400)
        {
            if (!CompressableImageTypes.Contains(Source.FileNameOnDiskFileInfo().Extension.Substring(1).ToLower()))
                return;

            if (!Source.FileExistsOnDisk())
                return;

            Bitmap originalBMP;
            Bitmap newBMP ;
            Graphics oGraphics;

            try
            {
                // Create a bitmap of the content of the fileUpload control in memory
                originalBMP = new Bitmap(Path.Join(General.Instance.wwwRootPath, DocumentsFolderRelativePath, Source.FileName));

                var originalSize = new Size(originalBMP.Width, originalBMP.Height);
                var newSize = ResizeKeepAspect(originalSize, maxSize, maxSize);

                // Create a new bitmap which will hold the previous resized bitmap
                newBMP = new Bitmap(originalBMP, (int)newSize.Width, (int)newSize.Height);
                // Create a graphic based on the new bitmap
                oGraphics = Graphics.FromImage(newBMP);

                // Set the properties for the new graphic file
                oGraphics.SmoothingMode = SmoothingMode.AntiAlias; oGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                // Draw the new graphic based on the resized bitmap
                oGraphics.DrawImage(originalBMP, 0, 0, (int)newSize.Width, (int)newSize.Height);

                // Save the new graphic file to the server
                newBMP.Save(Path.Join(General.Instance.wwwRootPath, DocumentsFolderRelativePath, Source.ThumbnailFileName()));

                // Once finished with the bitmap objects, we deallocate them.
                originalBMP.Dispose();
                newBMP.Dispose();
                oGraphics.Dispose();
            }
            catch (Exception e)
            {
                General.Instance.WriteToLog(e.ToString());
            }
        }

        #region PRIVATE

        private static ArrayList CompressableImageTypes => new ArrayList(new string[] { "jpg", "jpeg", "gif", "png", "apng", "bmp" });

        private static Size ResizeKeepAspect(this Size src, int maxWidth, int maxHeight, bool enlarge = false)
        {
            maxWidth = enlarge ? maxWidth : Math.Min(maxWidth, src.Width);
            maxHeight = enlarge ? maxHeight : Math.Min(maxHeight, src.Height);

            decimal rnd = Math.Min(maxWidth / (decimal)src.Width, maxHeight / (decimal)src.Height);
            
            return new Size((int)Math.Round(src.Width * rnd), (int)Math.Round(src.Height * rnd));
        }

        #endregion
    }
}
