using Application.Dto;
using Application.Dto.Attantance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Serv.Attantance
{
    public interface ITrainerPunchService
    {
        Task<ApiResponse<PunchResponseDto>> PunchInAsync(int userId);
        Task<ApiResponse<string>> PunchOutAsync(int userId);
    }
}
