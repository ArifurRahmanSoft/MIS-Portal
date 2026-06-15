using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.OracleModel.ViewModel.Sales
{
    public class vmDocuments
    {
        public long? DocumentID { get; set; }
        public long? FileId { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string FilePath { get; set; }
        public string TransactionTypeName { get; set; }
        public int? DocumentPathID { get; set; }
        public string ModelState { get; set; }
        public int? DocumentTypeID { get; set; }
        public string DocumentTypeName { get; set; }
        public string Remarks { get; set; }
        public string VirtualPath { get; set; }
        public string PrevFilePath { get; set; }
        public string ViewPath { get; set; }
        public long? id { get; set; }
        public string label { get; set; }
        public string ParentDocumentNames { get; set; }
    }
}
