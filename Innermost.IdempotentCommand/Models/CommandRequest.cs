using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innermost.IdempotentCommand.Models
{
    public class CommandRequest
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid ID { get; set; }

        public string CommandName { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Time { get; set; }
        public CommandRequest()
        {

        }
        public CommandRequest(Guid id,string commandName,DateTime time)
        {
            ID=id;
            CommandName=commandName;
            Time=time;
        }

    }
}
