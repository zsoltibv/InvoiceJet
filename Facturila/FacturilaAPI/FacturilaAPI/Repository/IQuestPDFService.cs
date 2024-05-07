using FacturilaAPI.Models.Dto;

namespace FacturilaAPI.Repository
{
    public interface IQuestPDFService
    {
        public void GenerateDocument(DocumentRequestDTO documentRequestDTO);
    }
}
