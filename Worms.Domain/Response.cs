using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Worms.Domain
{
    public class Response
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Direction? Direction { get; set; }
        public bool Split { get; set; }

        public Response(Direction? direction = null, bool split = false)
        {
            Direction = direction;
            Split = split;
        }

        public override string ToString()
        {
            return $"{nameof(Direction)}: {Direction}, {nameof(Split)}: {Split}";
        }
    }
}