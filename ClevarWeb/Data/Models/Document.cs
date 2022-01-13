using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.ComponentModel.DataAnnotations.Schema;
using ClevarWeb.Data.Models.Internals;
using Newtonsoft.Json;
using ClevarWeb.Utility;
using System.Collections;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ClevarWeb.Data.Models
{
    public class Document : ClevarTableBase
    {
        #region PROPS

        [Required]
        public string DocumentName { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public string DocumentType { get; set; }

        public DocumentData DocumentData { get; set; }

        public bool IsDisplayOnHomePage { get; set; } = false;

        #endregion PROPS

        #region METHODS

        public string GetFilePath(bool isThumbnail = false)
        {
            if (this.DocumentType == DocumentTypeEnum.PreDefined.ToString())
                return Path.Join(RootImgFolder, DocumentName);

            if(!FileExistsOnDisk())
                return Path.Join(RootImgFolder, "404NotFound.jpg");

            if (isThumbnail)
            {
                if(File.Exists(Path.Join(General.Instance.wwwRootPath, RootDocumentsFolder, ThumbnailFileName())))
                    return Path.Join(RootDocumentsFolder, ThumbnailFileName());
                else
                    return Path.Join(RootDocumentsFolder, FileName);
            }
            return Path.Join(RootDocumentsFolder, FileName);
        }

        public string ThumbnailFileName() => "thumb_" + FileName;

        public FileInfo FileNameFileInfo() => new FileInfo(this.DocumentName);

        public bool FileExistsOnDisk()
        {
            if (!this.FileName.HasValue())
                return false;

            FileInfo fi = new FileInfo(Path.Join(General.Instance.wwwRootPath, RootDocumentsFolder, FileName));
            return File.Exists(fi.FullName);
        }

        public FileInfo FileNameOnDiskFileInfo() => new FileInfo(this.FileName);

        public bool IsImage()
        {
            return ImageTypes.Contains(this.FileNameOnDiskFileInfo().Extension.Substring(1).ToLower());
        }
        #endregion METHODS

        #region STATIC

        private static ArrayList ImageTypes => new ArrayList(new string[] {"jpg", "jpeg", "gif", "apng", "svg", "bmp", "png", "ico" });       
        private static string RootDocumentsFolder { get { return DocumentService.DocumentsFolderRelativePath; } }
        private static string RootImgFolder { get { return "/img/"; } }

        public static Document CreatePreDefinedDocument(string name)
        {
            Document doc = new Document();
            doc.DocumentType = DocumentTypeEnum.PreDefined.ToString();
            doc.DocumentName = name;
            return doc;
        }

        public static Document CreateDocument(string name, byte[] fileData)
        {
            Document newDoc = new Document();
            newDoc.DocumentType = DocumentTypeEnum.Uploaded.ToString();
            newDoc.DocumentName = name;
            newDoc.DocumentData = new DocumentData() { FileData = fileData };
            newDoc.FileName = Guid.NewGuid().ToString() + newDoc.FileNameFileInfo().Extension;
            DocumentService.WriteDocumentToDisk(newDoc.FileName, newDoc.DocumentData.FileData);
            DocumentService.CreateThumbnail(newDoc);
            return newDoc;
        }

        public static Document CreateDocument(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            var fileData = File.ReadAllBytes(filePath);
            return Document.CreateDocument(fi.Name, fileData);
        }

        public enum DocumentTypeEnum
        {
            Uploaded,
            Default,
            PreDefined
        }

        public static void DeleteDocument<dbcontext>(int id, dbcontext db) where dbcontext : ClevarDbContext
        {
            var document = db.Documents
                .Include(x => x.DocumentData)
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (document == null)
                return;

            // NULL out any references to this entity.
            foreach (var project in db.Projects.Where(x => x.PrimaryDocument.Id == document.Id).ToList())
                project.PrimaryDocument = null;

            foreach (var cartoonImage in db.CartoonImages.Where(x => x.Document.Id == document.Id).ToList())
                cartoonImage.Document = null;

            foreach (var pub in db.Publications.Where(x => x.PrimaryDocument.Id == document.Id).ToList())
                pub.PrimaryDocument = null;

            // Delete Document File Data
            if (document.DocumentData != null)
                db.DocumentDatas.Remove(document.DocumentData);

            // Remove Document
            db.Documents.Remove(document);

            // Commit
            db.SaveChanges();
        }

        #endregion STATIC
    }
}
