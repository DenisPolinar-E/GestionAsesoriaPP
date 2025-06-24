using System;

namespace GestionAsesoria.Operator.Application.DTOs.AdvisoringRequests.Response
{
    public class GetAllAdvisoringRequestResponse
    {
        public int Id { get; set; }
        public string UserSubject { get; set; }
        public string UserMessage { get; set; }
        public DateTime DateRequest { get; set; }
        public string StatusName { get; set; }

        // Información del solicitante
        public string RequesterName { get; set; }
        public string RequesterLastName { get; set; }

        // Información del asesor
        public string AdvisorName { get; set; }
        public string AdvisorLastName { get; set; }
        public string ResponseAdvisor { get; set; }
        public DateTime? DateResponseAdvisor { get; set; }

        // Información del tipo de servicio
        public string ServiceTypeName { get; set; }
    }
}