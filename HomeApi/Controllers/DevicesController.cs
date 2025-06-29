using AutoMapper;
using HomeApi.Configuration;
using HomeApi.Contracts.Models.Devices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HomeApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DevicesController : ControllerBase
    {
        private IOptions<HomeOptions> _options;
        private IMapper _mapper;

        public DevicesController(IOptions<HomeOptions> options, IMapper mapper)
        {
            _options = options;
            _mapper = mapper;
        }

        /// <summary>
        /// Просмотр списка подключенных устройств
        /// </summary>
        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            return StatusCode(200, "Устройства отсутствуют");
        }

        /// <summary>
        /// Добавление нового устройства
        /// </summary>
        [HttpPost]
        [Route("Add")]
        public IActionResult Add(
           [FromBody] // Атрибут, указывающий, откуда биндить значение объекта
   AddDeviceRequest request // Объект запроса
        )
        {
            if (request.CurrentVolts < 120)
            {
                // Добавляем для клиента информативную ошибку
                ModelState.AddModelError("currentVolts", "Устройства с напряжением меньше 120 вольт не поддерживаются!");
                return BadRequest(ModelState);
            }

            return StatusCode(200, $"Устройство {request.Name} добавлено!");
        }
    }
}
