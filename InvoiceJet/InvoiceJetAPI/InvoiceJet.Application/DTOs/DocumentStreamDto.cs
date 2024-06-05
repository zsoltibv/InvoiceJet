namespace InvoiceJetAPI.Models.Dto
{
    public class DocumentStreamDto
    {
        public string DocumentNumber { get; set; }
        public byte[] PdfContent { get; set; }
    }
}
