using Hub.Server.Repository;
using Hub.Shared;

namespace Hub.Server.Interfaces.Database.Voice
{
    /// <summary>
    /// Adapter
    /// </summary>
    public interface iVoiceBroadcastRepository : iInMemoryVoiceRepository, iRdbmsVoiceRepository
    {
        
    }
}
