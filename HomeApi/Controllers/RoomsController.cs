using System.Threading.Tasks;
using AutoMapper;
using HomeApi.Contracts.Models.Rooms;
using HomeApi.Data.Models;
using HomeApi.Data.Queries;
using HomeApi.Data.Repos;
using Microsoft.AspNetCore.Mvc;

namespace HomeApi.Controllers
{
    /// <summary>
    /// Контроллер комнат
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RoomsController : ControllerBase
    {
        private IRoomRepository _repository;
        private IMapper _mapper;
        
        public RoomsController(IRoomRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        
        //TODO: Задание - добавить метод на получение всех существующих комнат
        
        /// <summary>
        /// Добавление комнаты
        /// </summary>
        [HttpPost] 
        [Route("")] 
        public async Task<IActionResult> Add([FromBody] AddRoomRequest request)
        {
            var existingRoom = await _repository.GetRoomByName(request.Name);
            if (existingRoom == null)
            {
                var newRoom = _mapper.Map<AddRoomRequest, Room>(request);
                await _repository.AddRoom(newRoom);
                return StatusCode(201, $"Комната {request.Name} добавлена!");
            }
            
            return StatusCode(409, $"Ошибка: Комната {request.Name} уже существует.");
        }

        [HttpPut]
        [Route("{name}")]
        public async Task<IActionResult> UpdateRoom(
            [FromRoute] string name,
            [FromBody] EditRoomRequest request)
        {
            var room = await _repository.GetRoomByName(name);
            if (room == null)
                return StatusCode(400, $"Комната {name} не найдена!");

            // Проверка конфликта имен
            if (!string.IsNullOrEmpty(request.NewName) && request.NewName != name)
            {
                var existingRoom = await _repository.GetRoomByName(request.NewName);
                if (existingRoom != null)
                    return StatusCode(409, $"Комната {request.NewName} уже существует!");
            }

            // Создаем запрос для обновления
            var query = new UpdateRoomQuery(
                request.NewName,
                request.NewArea,
                request.NewGasConnected,
                request.NewVoltage
            );

            // Выполняем обновление
            await _repository.UpdateRoom(room, query);

            return StatusCode(200, $"Комната {name} обновлена!");
        }
    }
}