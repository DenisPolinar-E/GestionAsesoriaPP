using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAsesoria.Operator.Application.DTOs.RequestPPP.Request
{
    public class UpdateStateRequestPPPByIdDto
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
    }
}
