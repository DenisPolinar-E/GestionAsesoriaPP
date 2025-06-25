using GestionAsesoria.Operator.Domain.Auditable;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GestionAsesoria.Operator.Domain.Entities
{
    [Comment("Representa una Práctica Pre Profesional, incluyendo los contratos de Prácticas Pre Profesionales asociadas.")]
    public class PreProfessionalInternship : AuditableEntity<int>
    {
        public PreProfessionalInternship()
        {
            PreProfessionalInternshipContracts = new HashSet<PreProfessionalInternshipByAdvisoringContract>();
        }

        [Comment("ID de la solicitud de Prácticas Pre Profesionales asociada.")]
        public int RequestPPPId { get; set; }

        [Comment("Solicitud de Prácticas Pre Profesionales asociada.")]
        public virtual RequestPPP RequestPPP { get; set; }

        public int ResolutionId { get; set; }

        public virtual DocumentCollection DocumentResolution { get; set; }

        public int ReviewCommitteePrimaryId { get; set; }

        public virtual ActorSecondaryRole ActorReviewCommitteePrimary { get; set; }


        public int ReviewCommitteeSecondaryId { get; set; }
        public virtual ActorSecondaryRole ActorReviewCommitteeSecondary { get; set; }



        [Comment("Colección de contratos de Prácticas Pre Profesionales asociadas.")]
        public virtual ICollection<PreProfessionalInternshipByAdvisoringContract> PreProfessionalInternshipContracts { get; set; }
    }
}