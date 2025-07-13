using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hub.Shared.Model
{
    public  class SignalRMessage<T>
    {
        /// <summary>
        /// // 메시지 유형 (enum)
        /// </summary>
        public MessageType type { get; set; } 
        public string title { get; set; } 
        public T body { get; set; } 

    }
}
