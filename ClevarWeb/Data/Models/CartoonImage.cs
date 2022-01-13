using ClevarWeb.Data.Models.Internals;
using ClevarWeb.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Text;

namespace ClevarWeb.Data.Models
{
    /// <summary>
    /// Used by clevar employee class to overlay images
    /// </summary>
    public class CartoonImage: ClevarTableBase
    {
        #region PROPERTIES

        public Person Person { get; set; }

        public Document Document { get; set; }

        [Required]
        public int LayerNumber { get; set; } = 1;

        public string Styles { get; set; }

        #endregion

        public static CartoonImage CreateCartoonImage(int layerNumber)
        {
            var newCartoonImage = new CartoonImage();
            newCartoonImage.LayerNumber = layerNumber;
            newCartoonImage.Document = Document.CreateDocument(Path.Join(General.Instance.wwwRootPath, "img", "DefaultCartoonImage.png"));
            newCartoonImage.Document.DocumentType = Document.DocumentTypeEnum.Default.ToString();
            return newCartoonImage;
        }

        
    }
}
