using Newtonsoft.Json;

namespace Core.Dto
{
    public class SendWallMessageDto
    {
        /// <summary>
        /// Идентификатор стены
        /// </summary>
        [JsonProperty("wallId")]
        public long WallId { get; set; }

        /// <summary>
        /// Сообщения
        /// </summary>
        [JsonProperty("messages")]
        public string[] Messages { get; set; }
        
        /// <summary>
        /// Задержка
        /// </summary>
        [JsonProperty("delay")] 
        public int Delay { get; set; } = 60;
    }
}