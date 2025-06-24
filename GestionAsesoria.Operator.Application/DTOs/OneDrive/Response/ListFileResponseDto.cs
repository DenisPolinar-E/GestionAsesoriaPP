using GestionAsesoria.Operator.Application.DTOs.OneDrive.Request;
using System.Collections.Generic;

namespace GestionAsesoria.Operator.Application.DTOs.OneDrive.Response
{
    public class ListFileResponseDto
    {
        public IEnumerable<FileItemRequestDto> Items { get; set; }
    }
}
