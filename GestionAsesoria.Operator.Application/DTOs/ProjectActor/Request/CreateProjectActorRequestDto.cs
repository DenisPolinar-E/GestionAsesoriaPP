using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.DTOs.ProjectActor.Request
{
    public class CreateProjectActorRequestDto
    {
        public string Justification { get; set; }
        public int ProjectId { get; set; }
        public int ActorId { get; set; }
        public int AuthorTypeId { get; set; }
    }
} 